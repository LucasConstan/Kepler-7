using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Command;
using Kepler7.Core.Patrones.Facade;
using Kepler7.Core.Patrones.Prototype;
using Kepler7.Core.Patrones.Strategy;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Game
{
    /// <summary>
    /// Ventana principal: tablero, oleada, IA enemiga (Strategy), reserva y acciones con deshacer (Command).
    /// El Builder, el Prototype, el Flyweight y el Facade tienen su propia ventana, que se abre desde el menú.
    /// </summary>
    public partial class FormPrincipal : Form
    {
        // Milisegundos por tick para cada opción del combo de velocidad (0 = pausa).
        private static readonly int[] IntervalosVelocidad = { 900, 600, 250, 0 };

        private static readonly Color ColorZonaDefensa = Color.LightBlue;
        private static readonly Color ColorZonaAproximacion = Color.MistyRose;

        private readonly Partida _partida = new Partida(600);

        // Command: todas las acciones del jugador pasan por el gestor (deshacer/rehacer + historial).
        private readonly GestorComandos _gestor = new GestorComandos();

        // Facade: la interfaz solo habla con el control central, nunca con los subsistemas.
        private readonly ControlCentral _control;

        // Prototype: el laboratorio vive toda la partida, así sus prototipos no cambian entre ventanas.
        private readonly LaboratorioClonacion _laboratorio = LaboratorioClonacion.CrearConPrototiposBase();

        // Los controles del tablero, ordenados por carril y columna para recorrerlos fácil.
        private readonly Button[,] _celdas = new Button[Grilla.Carriles, Grilla.Columnas];
        private readonly Label[] _lblCarriles = new Label[Grilla.Carriles];
        private readonly ProgressBar[] _barrasNucleo = new ProgressBar[Grilla.Carriles];

        private Enemigo? _enemigoSeleccionado;
        private Unidad? _defensorAMover;
        private bool _estacionApagada;
        private int _lineasHistorial;

        public FormPrincipal()
        {
            InitializeComponent();
            _control = new ControlCentral(_partida);

            // En la tabla del tablero: columna 0 = subsistema, columna 1 = barra del núcleo, columnas 2 a 10 = celdas.
            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                _lblCarriles[carril] = (Label)tablaTablero.GetControlFromPosition(0, carril)!;
                _barrasNucleo[carril] = (ProgressBar)tablaTablero.GetControlFromPosition(1, carril)!;
                for (int columna = 0; columna < Grilla.Columnas; columna++)
                    _celdas[carril, columna] = (Button)tablaTablero.GetControlFromPosition(columna + 2, carril)!;
            }

            cmbVelocidad.SelectedIndex = 1;
            cmbIA.Items.AddRange(EstrategiasIA.Todas);
            cmbIA.SelectedIndex = 0;
            cmbCarrilReparar.Items.AddRange(Estacion.Subsistemas);
            cmbCarrilReparar.SelectedIndex = 0;
            btnReparar.Text = $"Reparar ({CurarComando.Costo} cr)";
            btnDisparo.Text = $"Disparo orbital al enemigo elegido ({AtacarComando.Costo} cr)";

            _partida.Mensaje += Registrar;
            _partida.Historial.EntradaRegistrada += AgregarAlHistorial;

            Refrescar();
            Registrar("Estación en línea. Usá el menú para ensamblar naves (Astillero) o clonar tripulantes (Laboratorio),");
            Registrar("desplegalos en el tablero y cuando estés listo iniciá la oleada.");
        }

        #region Menú: abre las ventanas de cada patrón

        private void menuAstillero_Click(object? sender, EventArgs e)
        {
            using var ventana = new FormAstillero(_partida, _gestor, Registrar);
            ventana.ShowDialog(this);
            Refrescar();
        }

        private void menuLaboratorio_Click(object? sender, EventArgs e)
        {
            using var ventana = new FormLaboratorio(_partida, _gestor, _laboratorio, Registrar);
            ventana.ShowDialog(this);
            Refrescar();
        }

        private void menuTripulacion_Click(object? sender, EventArgs e)
        {
            using var ventana = new FormTripulacion(_partida, Registrar);
            ventana.ShowDialog(this);
            Refrescar();
        }

        private void menuControlCentral_Click(object? sender, EventArgs e)
        {
            bool estabaApagada = _estacionApagada;

            using (var ventana = new FormControlCentral(_control, _partida, Registrar))
            {
                // La ventana usa la fachada; acá solo nos enteramos para manejar el Timer.
                ventana.OleadaIniciada += AlEmpezarOleada;
                ventana.EstacionApagada += AlApagarEstacion;
                ventana.ShowDialog(this);
            }
            Refrescar();

            if (!estabaApagada && _estacionApagada) PreguntarNuevaPartida();
        }

        private void menuReiniciar_Click(object? sender, EventArgs e)
        {
            var respuesta = MessageBox.Show("¿Reiniciar la partida? Se pierde todo el progreso actual.", "Reiniciar",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Reabre la aplicación desde cero: así también se reinician los contadores (ids, especies...).
            if (respuesta == DialogResult.Yes) Application.Restart();
        }

        #endregion

        #region Oleada y bucle de la partida (Timer)

        // Iniciar la oleada pasa por la fachada: ella prepara los subsistemas y arranca la oleada.
        private void btnIniciarOleada_Click(object? sender, EventArgs e)
        {
            if (_estacionApagada)
            {
                PreguntarNuevaPartida();
                return;
            }
            foreach (var paso in _control.IniciarTurno()) Registrar(paso);
            if (_partida.Fase == FasePartida.Oleada) AlEmpezarOleada();
        }

        private void AlEmpezarOleada()
        {
            _gestor.Limpiar($"Empieza la oleada {_partida.NumeroOleada}");
            _defensorAMover = null;
            AplicarVelocidad();
            Refrescar();
        }

        private void AlApagarEstacion()
        {
            timerTick.Stop();
            _estacionApagada = true;
            Refrescar();
        }

        // Cada tick del Timer es un tick del juego.
        private void timerTick_Tick(object? sender, EventArgs e)
        {
            _partida.AvanzarTick();
            Refrescar();

            if (_partida.Fase == FasePartida.Oleada) return;

            timerTick.Stop();
            _gestor.Limpiar("Terminó la oleada");
            Refrescar();
            if (_partida.Fase == FasePartida.Victoria)
                MessageBox.Show("¡Salvaste Kepler-7!", "Victoria", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (_partida.Fase == FasePartida.Derrota)
                MessageBox.Show("Cayeron demasiados subsistemas. Kepler-7 se perdió.", "Derrota", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cmbVelocidad_SelectedIndexChanged(object? sender, EventArgs e)
        {
            AplicarVelocidad();
        }

        private void AplicarVelocidad()
        {
            int intervalo = IntervalosVelocidad[cmbVelocidad.SelectedIndex];
            if (intervalo == 0 || _partida.Fase != FasePartida.Oleada)
            {
                timerTick.Stop();
                return;
            }
            timerTick.Interval = intervalo;
            timerTick.Start();
        }

        // La partida terminó porque se apagó la estación: se ofrece empezar una nueva.
        private void PreguntarNuevaPartida()
        {
            var nueva = MessageBox.Show("Kepler-7 fue apagada y la partida terminó.\n¿Querés empezar una partida nueva?",
                                        "Estación apagada", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (nueva == DialogResult.Yes) Application.Restart();
        }

        #endregion

        #region Tablero

        // Todos los botones del tablero usan este mismo evento; la posición sale de la TableLayoutPanel.
        private void btnCelda_Click(object? sender, EventArgs e)
        {
            var posicion = tablaTablero.GetPositionFromControl((Control)sender!);
            int carril = posicion.Row;
            int columna = posicion.Column - 2;

            string subsistema = Estacion.Subsistemas[carril];
            var ocupante = _partida.Grilla.ObtenerUnidad(carril, columna);

            if (ocupante is Enemigo enemigo)
            {
                _enemigoSeleccionado = enemigo;
                _defensorAMover = null;
                Registrar($"[{subsistema}] {enemigo.NombreCompleto}: {enemigo.Descripcion()}");
                Refrescar();
                return;
            }
            if (ocupante != null)
            {
                Registrar($"[{subsistema}] {ocupante.NombreCompleto}: {ocupante.Descripcion()}");
                if (_partida.Fase != FasePartida.Preparacion) return;

                // Entre oleadas, un clic en un defensor lo elige para moverlo (otro clic lo suelta).
                _defensorAMover = ocupante == _defensorAMover ? null : ocupante;
                if (_defensorAMover != null) Registrar("   · hacé clic en una celda azul libre para moverlo.");
                Refrescar();
                return;
            }

            // Celda libre: mueve el defensor elegido o despliega la unidad elegida de la reserva.
            var unidad = _defensorAMover ?? lstReserva.SelectedItem as Unidad;
            if (unidad == null)
            {
                Registrar("Seleccioná primero una unidad de la reserva.");
                return;
            }
            if (_partida.Fase != FasePartida.Preparacion)
            {
                Registrar("Solo se puede desplegar entre oleadas.");
                return;
            }
            if (!_partida.Grilla.EsZonaDefensa(columna))
            {
                Registrar("Solo se puede desplegar en la zona de defensa (celdas azules).");
                return;
            }

            if (EjecutarComando(new MoverComando(_partida, unidad, carril, columna)))
                Registrar($"Despliegue: {unidad.NombreCompleto} defiende {subsistema} (columna {columna + 1}).");
            _defensorAMover = null;
            Refrescar();
        }

        private void RefrescarTablero()
        {
            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                int integridad = _partida.IntegridadDelCarril(carril);
                int tripulantes = _partida.Tripulacion.TripulantesEn(carril);
                _lblCarriles[carril].Text = integridad == 0
                    ? $"{Estacion.Subsistemas[carril]}\n¡CAÍDO!"
                    : $"{Estacion.Subsistemas[carril]}\nNúcleo {integridad}/{Partida.IntegridadMaxima}\nTripulantes {tripulantes:N0}";
                _barrasNucleo[carril].Value = integridad;

                for (int columna = 0; columna < Grilla.Columnas; columna++)
                    ActualizarCelda(_celdas[carril, columna], _partida.Grilla.ObtenerUnidad(carril, columna), columna);
            }
        }

        // Una celda vacía muestra el color de su zona; una ocupada, el símbolo, la vida y el color de la unidad.
        private void ActualizarCelda(Button celda, Unidad? unidad, int columna)
        {
            string texto = "";
            Color fondo = _partida.Grilla.EsZonaDefensa(columna) ? ColorZonaDefensa : ColorZonaAproximacion;
            Color letra = Color.Black;

            if (unidad != null)
            {
                texto = unidad.Simbolo + "\n" + unidad.Vida + " PV";
                if (unidad is Enemigo enemigo) texto += "\nIA " + enemigo.Estrategia.Sigla;
                if (unidad == _enemigoSeleccionado || unidad == _defensorAMover) texto = "» " + texto + " «";
                fondo = unidad.Color;
                letra = Color.White;
            }

            // Solo se cambia lo que cambió, así los botones no parpadean en cada tick.
            if (celda.Text != texto) celda.Text = texto;
            if (celda.BackColor != fondo) celda.BackColor = fondo;
            if (celda.ForeColor != letra) celda.ForeColor = letra;
        }

        #endregion

        #region Strategy: cambiar la IA enemiga

        private void cmbIA_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ActualizarInfoIA();
        }

        private void ActualizarInfoIA()
        {
            if (cmbIA.SelectedItem is IEstrategiaIA ia) lblInfoIA.Text = $"{ia.Nombre}: {ia.Descripcion}";
            lblElegido.Text = _enemigoSeleccionado == null
                ? "Clic en un enemigo del tablero para elegirlo."
                : $"Elegido: {_enemigoSeleccionado.NombreCompleto} (IA {_enemigoSeleccionado.Estrategia.Nombre})";
        }

        private void btnIAUno_Click(object? sender, EventArgs e)
        {
            if (_enemigoSeleccionado == null)
            {
                Registrar("Primero hacé clic en un enemigo del tablero para elegirlo.");
                return;
            }
            _partida.CambiarEstrategia(_enemigoSeleccionado, (IEstrategiaIA)cmbIA.SelectedItem!);
            Refrescar();
        }

        private void btnIATodos_Click(object? sender, EventArgs e)
        {
            _partida.CambiarEstrategiaATodos((IEstrategiaIA)cmbIA.SelectedItem!);
            Refrescar();
        }

        #endregion

        #region Command: deshacer, rehacer y acciones de la estación

        // Todas las acciones del jugador pasan por acá: el gestor las ejecuta y las guarda.
        private bool EjecutarComando(IComando comando)
        {
            bool ok = _gestor.Ejecutar(comando);
            if (!ok) Registrar("No se pudo: " + comando.Descripcion + " (faltan créditos o no se puede en este momento).");
            Refrescar();
            return ok;
        }

        private void btnDeshacer_Click(object? sender, EventArgs e)
        {
            Registrar(_gestor.Deshacer());
            _defensorAMover = null;
            Refrescar();
        }

        private void btnRehacer_Click(object? sender, EventArgs e)
        {
            Registrar(_gestor.Rehacer());
            Refrescar();
        }

        private void btnReparar_Click(object? sender, EventArgs e)
        {
            EjecutarComando(new CurarComando(_partida, cmbCarrilReparar.SelectedIndex));
        }

        private void btnDisparo_Click(object? sender, EventArgs e)
        {
            if (_enemigoSeleccionado == null)
            {
                Registrar("Primero hacé clic en un enemigo del tablero para elegirlo.");
                return;
            }
            EjecutarComando(new AtacarComando(_partida, _enemigoSeleccionado));
        }

        private void RefrescarComandos()
        {
            btnDeshacer.Enabled = _gestor.PuedeDeshacer;
            btnRehacer.Enabled = _gestor.PuedeRehacer;

            if (lstComandos.Items.Count == _gestor.Historial.Count) return;
            lstComandos.BeginUpdate();
            lstComandos.Items.Clear();
            foreach (var linea in _gestor.Historial) lstComandos.Items.Add(linea);
            lstComandos.TopIndex = Math.Max(0, lstComandos.Items.Count - 1);
            lstComandos.EndUpdate();
        }

        #endregion

        #region Reserva

        private void lstReserva_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ActualizarDetalleReserva();
        }

        // La lista solo se vuelve a armar si la reserva cambió (así no se pierde la selección en cada tick).
        private void RefrescarReserva()
        {
            var reserva = _partida.Estacion.Reserva;
            if (!lstReserva.Items.Cast<Unidad>().SequenceEqual(reserva))
            {
                var seleccionada = lstReserva.SelectedItem;
                bool hayNuevas = reserva.Count > lstReserva.Items.Count;

                lstReserva.BeginUpdate();
                lstReserva.Items.Clear();
                foreach (var unidad in reserva) lstReserva.Items.Add(unidad);

                if (hayNuevas) lstReserva.SelectedIndex = lstReserva.Items.Count - 1; // la recién construida
                else if (seleccionada != null && lstReserva.Items.Contains(seleccionada)) lstReserva.SelectedItem = seleccionada;
                else if (lstReserva.Items.Count > 0) lstReserva.SelectedIndex = 0;
                lstReserva.EndUpdate();
            }
            ActualizarDetalleReserva();
        }

        private void ActualizarDetalleReserva()
        {
            var unidad = lstReserva.SelectedItem as Unidad;
            lblDetalleReserva.Text = unidad == null
                ? "Reserva vacía. Construí unidades desde el menú (Astillero o Laboratorio)."
                : unidad.Descripcion() + "\nHacé clic en una celda azul para desplegarla.";
        }

        #endregion

        private void Refrescar()
        {
            // Un enemigo elegido deja de estarlo si murió o si salió del tablero (al golpear el núcleo).
            if (_enemigoSeleccionado != null && (!_enemigoSeleccionado.EstaViva || _enemigoSeleccionado.Carril < 0))
                _enemigoSeleccionado = null;

            RefrescarTablero();
            RefrescarReserva();
            RefrescarComandos();
            ActualizarInfoIA();

            string fase = _partida.Fase == FasePartida.Preparacion ? "Preparación"
                        : _partida.Fase == FasePartida.Oleada ? "¡Combate!"
                        : _partida.Fase.ToString();
            lblEstado.Text = $"Oleada {_partida.NumeroOleada}/{_partida.TotalOleadas} · {fase}      " +
                             $"Créditos: {_partida.Estacion.Creditos}      Defensores: {_partida.Grilla.Defensores().Count}      " +
                             $"Enemigos: {_partida.Grilla.Enemigos().Count}      En reserva: {_partida.Estacion.Reserva.Count}      " +
                             $"Bonus por oleada: +{_partida.BonusRecursosPorOleada}";

            if (_partida.Fase == FasePartida.Preparacion)
            {
                btnIniciarOleada.Enabled = true;
                btnIniciarOleada.Text = $"Iniciar oleada {_partida.NumeroOleada}/{_partida.TotalOleadas}";
            }
            else
            {
                // Con la estación apagada el botón sigue activo para volver a ofrecer una partida nueva.
                btnIniciarOleada.Enabled = _estacionApagada;
                btnIniciarOleada.Text = _partida.Fase == FasePartida.Oleada ? "Oleada en curso..." : "Partida terminada";
            }
        }

        private void AgregarAlHistorial(EntradaTurno entrada)
        {
            txtHistorial.AppendText(entrada + Environment.NewLine);
            _lineasHistorial++;

            // Para que el cuadro no crezca sin límite, se borran las líneas más viejas.
            if (_lineasHistorial > 800)
            {
                txtHistorial.Lines = txtHistorial.Lines.Skip(400).ToArray();
                _lineasHistorial = txtHistorial.Lines.Length;
            }
        }

        private void Registrar(string mensaje)
        {
            txtRegistro.AppendText(mensaje + Environment.NewLine);
        }
    }
}

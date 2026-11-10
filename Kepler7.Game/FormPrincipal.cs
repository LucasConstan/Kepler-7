using System.Collections.Generic;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Bridge;
using Kepler7.Core.Patrones.ChainOfResponsibility;
using Kepler7.Core.Patrones.Command;
using Kepler7.Core.Patrones.Facade;
using Kepler7.Core.Patrones.Prototype;
using Kepler7.Core.Patrones.State;
using Kepler7.Core.Patrones.Strategy;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Game
{
    // Ventana principal: tablero, oleada, IA enemiga (Strategy), reserva y acciones con deshacer (Command).
    // El Builder, el Prototype, el Flyweight y el Facade tienen su propia ventana, que se abre desde el menú.
    public partial class FormPrincipal : Form
    {
        // Milisegundos por tick para cada opción del combo de velocidad (0 = pausa).
        private static readonly int[] IntervalosVelocidad = { 900, 600, 250, 0 };

        private static readonly Color ColorZonaDefensa = Color.LightBlue;
        private static readonly Color ColorZonaAproximacion = Color.MistyRose;
        private static readonly Color ColorDestello = Color.Red;

        // Cuántos pasos del Timer de animación dura un destello, y cuánto se mueve una barra por paso.
        private const int PasosDestello = 8;
        private const int PasoBarra = 3;

        // Con cuánto arranca una partida nueva. Era 600: se bajó para que el juego tenga un poco más de dificultad.
        private const int CreditosIniciales = 500;

        // Se crea en el constructor: puede ser una nueva o una guardada que se retoma (ver CrearPartida).
        private readonly Partida _partida;
        private bool _partidaRetomada;

        // Command: todas las acciones del jugador pasan por el gestor (deshacer/rehacer + historial).
        private readonly GestorComandos _gestor = new GestorComandos();

        // Facade: la interfaz solo habla con el control central, nunca con los subsistemas.
        private readonly ControlCentral _control;

        // Prototype: el laboratorio vive toda la partida, así sus prototipos no cambian entre ventanas.
        private readonly LaboratorioClonacion _laboratorio = LaboratorioClonacion.CrearConPrototiposBase();

        // Proxy + Bridge: el hub de progreso no depende de la partida. Arranca leyendo el archivo JSON.
        private readonly HubProgreso _hub = new HubProgreso(Almacenamientos.Archivo);

        // Los controles del tablero, ordenados por carril y columna para recorrerlos fácil.
        private readonly Button[,] _celdas = new Button[Grilla.Carriles, Grilla.Columnas];
        private readonly Label[] _lblCarriles = new Label[Grilla.Carriles];
        private readonly ProgressBar[] _barrasNucleo = new ProgressBar[Grilla.Carriles];

        // --- Animaciones del tablero ---
        // Un Timer aparte del de los ticks del juego, así se anima aunque la partida esté en pausa.
        private readonly System.Windows.Forms.Timer _timerAnimacion = new System.Windows.Forms.Timer();
        // Cuántos pasos de destello le quedan a cada celda y a cada subsistema (0 = sin destello).
        private readonly int[,] _destellosCelda = new int[Grilla.Carriles, Grilla.Columnas];
        private readonly int[] _destellosCarril = new int[Grilla.Carriles];
        // Lo último que se mostró, para darse cuenta de cuándo alguien recibió daño.
        private readonly Dictionary<Unidad, int> _vidaMostrada = new Dictionary<Unidad, int>();
        private readonly int[] _integridadMostrada = new int[Grilla.Carriles];
        // Hacia dónde se mueven las barras: el valor real que tienen que alcanzar de a poco.
        private readonly int[] _objetivoNucleo = new int[Grilla.Carriles];
        private int _objetivoVidaHeroe;
        private readonly Color[] _colorCarrilNormal = new Color[Grilla.Carriles];

        private Enemigo? _enemigoSeleccionado;
        private Unidad? _defensorAMover;
        private bool _estacionApagada;
        private int _lineasHistorial;
        private Golpe? _golpeMostrado;

        // La recompensa del Hub se entrega una sola vez, aunque el resumen se abra varias veces.
        private bool _recompensaEntregada;
        private List<string> _recompensa = new List<string>();

        // Las mejoras de la tienda que se usaron al arrancar esta partida.
        private readonly List<string> _mejorasAplicadas;

        // continuarGuardada viene de la pantalla inicial: true si se eligió "Continuar partida".
        public FormPrincipal(bool continuarGuardada)
        {
            InitializeComponent();
            new TransicionAparecer(this);
            _partida = CrearPartida(continuarGuardada);
            _control = new ControlCentral(_partida);

            // En la tabla del tablero: columna 0 = subsistema, columna 1 = barra del núcleo, columnas 2 a 10 = celdas.
            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                _lblCarriles[carril] = (Label)tablaTablero.GetControlFromPosition(0, carril)!;
                _barrasNucleo[carril] = (ProgressBar)tablaTablero.GetControlFromPosition(1, carril)!;
                _colorCarrilNormal[carril] = _lblCarriles[carril].BackColor;
                _integridadMostrada[carril] = Partida.IntegridadMaxima;
                for (int columna = 0; columna < Grilla.Columnas; columna++)
                {
                    _celdas[carril, columna] = (Button)tablaTablero.GetControlFromPosition(columna + 2, carril)!;
                }
            }

            cmbVelocidad.SelectedIndex = 1;
            cmbIA.Items.AddRange(EstrategiasIA.Todas);
            cmbIA.SelectedIndex = 0;
            cmbCarrilReparar.Items.AddRange(Estacion.Subsistemas);
            cmbCarrilReparar.SelectedIndex = 0;
            btnReparar.Text = "Reparar (" + CurarComando.Costo + " cr)";
            btnDisparo.Text = "Disparo orbital al enemigo elegido (" + AtacarComando.Costo + " cr)";

            _partida.Mensaje += Registrar;
            _partida.Historial.EntradaRegistrada += AgregarAlHistorial;
            _hub.LogroDesbloqueado += MostrarLogro;

            _timerAnimacion.Interval = 40;
            _timerAnimacion.Tick += timerAnimacion_Tick;
            _timerAnimacion.Start();
            FormClosed += FormPrincipal_FormClosed;

            // Las mejoras compradas en la tienda del Hub se usan acá, antes del primer refresco,
            // así la pantalla ya arranca mostrando los créditos y tripulantes de más.
            // Son para partidas nuevas: si se retomó una guardada, quedan esperando a la próxima.
            if (_partidaRetomada)
            {
                _mejorasAplicadas = new List<string>();
            }
            else
            {
                _mejorasAplicadas = _hub.TomarMejorasPendientes();
            }
            List<string> avisosMejoras = new List<string>();
            for (int i = 0; i < _mejorasAplicadas.Count; i++)
            {
                avisosMejoras.Add(CatalogoMejoras.Aplicar(_mejorasAplicadas[i], _partida));
            }
            Shown += FormPrincipal_Shown;

            Refrescar();
            Registrar("Estación en línea. Usá el menú para ensamblar naves (Astillero) o clonar tripulantes (Laboratorio),");
            Registrar("desplegalos en el tablero y cuando estés listo iniciá la oleada.");
            if (_partidaRetomada)
            {
                Registrar("Partida guardada retomada: te toca la oleada " + _partida.NumeroOleada + "/" + _partida.TotalOleadas + ".");
            }
            for (int i = 0; i < avisosMejoras.Count; i++)
            {
                Registrar("Tienda del Hub: " + avisosMejoras[i]);
            }
        }

        // Retoma la partida guardada si en la pantalla inicial se eligió "Continuar partida"; si no, arranca una nueva.
        // El guardado se borra en los dos casos: retomarlo lo usa, y empezar otra lo descarta.
        private Partida CrearPartida(bool continuarGuardada)
        {
            PartidaGuardada? guardada = _hub.LeerPartidaGuardada();
            _hub.BorrarPartidaGuardada();

            if (continuarGuardada && guardada != null)
            {
                try
                {
                    Partida retomada = Partida.DesdeGuardado(guardada);
                    _hub.ContinuarPartida(guardada);
                    _partidaRetomada = true;
                    return retomada;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo retomar la partida guardada (" + ex.Message + "). Empieza una nueva.",
                                    "Partida guardada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            _hub.EmpezarPartidaNueva();
            return new Partida(CreditosIniciales);
        }

        // Un cartel por cada mejora de la tienda que se usó, para que no pase desapercibida.
        // Se muestran recién acá porque el cartel se ubica según dónde quedó la ventana en pantalla.
        private void FormPrincipal_Shown(object? sender, EventArgs e)
        {
            for (int i = 0; i < _mejorasAplicadas.Count; i++)
            {
                FormAviso aviso = new FormAviso("Mejora de la tienda aplicada", _mejorasAplicadas[i], this);
                aviso.Show();
            }
        }

        #region Menú: abre las ventanas de cada patrón

        private void menuAstillero_Click(object? sender, EventArgs e)
        {
            using (FormAstillero ventana = new FormAstillero(_partida, _gestor, Registrar, _hub))
            {
                ventana.ShowDialog(this);
                Refrescar();
            }
        }

        private void menuLaboratorio_Click(object? sender, EventArgs e)
        {
            using (FormLaboratorio ventana = new FormLaboratorio(_partida, _gestor, _laboratorio, Registrar, _hub))
            {
                ventana.ShowDialog(this);
                Refrescar();
            }
        }

        private void menuInventario_Click(object? sender, EventArgs e)
        {
            using (FormInventario ventana = new FormInventario(_partida, Registrar, _hub))
            {
                ventana.ShowDialog(this);
                Refrescar();
            }
        }

        private void menuTripulacion_Click(object? sender, EventArgs e)
        {
            using (FormTripulacion ventana = new FormTripulacion(_partida, Registrar, _hub))
            {
                ventana.ShowDialog(this);
                Refrescar();
            }
        }

        private void menuControlCentral_Click(object? sender, EventArgs e)
        {
            bool estabaApagada = _estacionApagada;

            using (FormControlCentral ventana = new FormControlCentral(_control, _partida, Registrar, _hub))
            {
                // La ventana usa la fachada; acá solo nos enteramos para manejar el Timer.
                ventana.OleadaIniciada += AlEmpezarOleada;
                ventana.EstacionApagada += AlApagarEstacion;
                ventana.ShowDialog(this);
            }
            Refrescar();

            if (!estabaApagada && _estacionApagada)
            {
                MostrarResumen();
            }
        }

        private void menuHub_Click(object? sender, EventArgs e)
        {
            using (FormHub ventana = new FormHub(_hub, _partida, Registrar))
            {
                ventana.ShowDialog(this);
            }
        }

        // El tutorial. Si hay una oleada corriendo, se pausa mientras se lee y después sigue.
        private void menuComoJugar_Click(object? sender, EventArgs e)
        {
            bool estabaCorriendo = timerTick.Enabled;
            timerTick.Stop();

            using (FormComoJugar ventana = new FormComoJugar())
            {
                ventana.ShowDialog(this);
            }

            if (estabaCorriendo)
            {
                timerTick.Start();
            }
        }

        private void menuReiniciar_Click(object? sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("¿Reiniciar la partida? Se pierde todo el progreso actual.", "Reiniciar",
                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Reabre la aplicación desde cero: así también se reinician los contadores (ids, especies...).
            if (respuesta == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        #endregion

        #region Oleada y bucle de la partida (Timer)

        // Iniciar la oleada pasa por la fachada: ella prepara los subsistemas y arranca la oleada.
        private void btnIniciarOleada_Click(object? sender, EventArgs e)
        {
            // Con la partida terminada, este mismo botón vuelve a abrir el resumen final.
            if (_estacionApagada || PartidaTerminada())
            {
                MostrarResumen();
                return;
            }

            List<string> pasos = _control.IniciarTurno();
            for (int i = 0; i < pasos.Count; i++)
            {
                Registrar(pasos[i]);
            }

            if (_partida.Fase == FasePartida.Oleada)
            {
                AlEmpezarOleada();
            }
        }

        private void AlEmpezarOleada()
        {
            _gestor.Limpiar("Empieza la oleada " + _partida.NumeroOleada);
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

            if (_partida.Fase == FasePartida.Oleada)
            {
                return;
            }

            timerTick.Stop();
            _gestor.Limpiar("Terminó la oleada");
            Refrescar();

            // Si la oleada terminó sin perder, todos los turnos (Template Method) se jugaron hasta el final.
            if (_partida.Fase == FasePartida.Preparacion || _partida.Fase == FasePartida.Victoria)
            {
                _hub.DesbloquearLogro(CatalogoLogros.TemplateMethod);

                // La oleada se paga al toque en el Hub (XP y monedas), y si había logros esperando pasan a recompensas.
                int cargasAntes = _hub.ObtenerProgreso().CargasDeLogro;
                // Al ganar, el número de oleada no avanza: la superada es la última.
                int oleadaSuperada = _partida.NumeroOleada - 1;
                if (_partida.Fase == FasePartida.Victoria)
                {
                    oleadaSuperada = _partida.TotalOleadas;
                }
                List<string> recompensa = _hub.OleadaSuperada(oleadaSuperada, _partida.EnemigosDerrotados);
                for (int i = 0; i < recompensa.Count; i++)
                {
                    Registrar("Hub: " + recompensa[i]);
                }

                // El primer mensaje siempre es lo que dio la oleada, por ejemplo "Oleada 1 superada: +26 XP y +10 monedas."
                FormAviso avisoOleada = new FormAviso(recompensa[0], "Mirá tu progreso en el Hub", this);
                avisoOleada.Show();

                if (_hub.ObtenerProgreso().CargasDeLogro > cargasAntes)
                {
                    FormAviso avisoLogros = new FormAviso("Logros listos para cobrar", "Cobralos en el Hub", this);
                    avisoLogros.Show();
                }
            }

            if (PartidaTerminada())
            {
                MostrarResumen();
            }
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

        private bool PartidaTerminada()
        {
            return _partida.Fase == FasePartida.Victoria || _partida.Fase == FasePartida.Derrota;
        }

        // Pantalla final con el resumen. La primera vez también entrega la recompensa en el Hub.
        private void MostrarResumen()
        {
            ResumenPartida resumen = _partida.CrearResumen();

            if (!_recompensaEntregada)
            {
                _recompensa = _hub.RecompensarPartida(resumen);
                _recompensaEntregada = true;
                for (int i = 0; i < _recompensa.Count; i++)
                {
                    Registrar("Hub: " + _recompensa[i]);
                }
            }

            using (FormResumen ventana = new FormResumen(resumen, _recompensa))
            {
                if (ventana.ShowDialog(this) == DialogResult.Retry)
                {
                    Application.Restart();
                }
            }
        }

        #endregion

        #region Logros

        // El Hub avisa cada logro nuevo: queda en el registro y aparece el cartel abajo a la derecha.
        private void MostrarLogro(string logro)
        {
            if (_hub.CobroHabilitado())
            {
                Registrar("★ Logro de esta partida: " + logro + " (ya lo podés cobrar en el Hub)");
            }
            else
            {
                Registrar("★ Logro de esta partida: " + logro + " (se podrá cobrar cuando superes la primera oleada)");
            }
            FormAviso aviso = new FormAviso("★ Logro de esta partida", logro, this);
            aviso.Show();
        }

        #endregion

        #region Tablero

        // Todos los botones del tablero usan este mismo evento; la posición sale de la TableLayoutPanel.
        private void btnCelda_Click(object? sender, EventArgs e)
        {
            TableLayoutPanelCellPosition posicion = tablaTablero.GetPositionFromControl((Control)sender!);
            int carril = posicion.Row;
            int columna = posicion.Column - 2;

            string subsistema = Estacion.Subsistemas[carril];
            Unidad? ocupante = _partida.Grilla.ObtenerUnidad(carril, columna);

            if (ocupante is Enemigo)
            {
                Enemigo enemigo = (Enemigo)ocupante;
                _enemigoSeleccionado = enemigo;
                _defensorAMover = null;
                Registrar("[" + subsistema + "] " + enemigo.NombreCompleto + ": " + enemigo.Descripcion());
                Refrescar();
                return;
            }

            if (ocupante != null)
            {
                Registrar("[" + subsistema + "] " + ocupante.NombreCompleto + ": " + ocupante.Descripcion());
                if (_partida.Fase != FasePartida.Preparacion)
                {
                    return;
                }

                // Entre oleadas, un clic en un defensor lo elige para moverlo (otro clic lo suelta).
                if (ocupante == _defensorAMover)
                {
                    _defensorAMover = null;
                }
                else
                {
                    _defensorAMover = ocupante;
                }

                if (_defensorAMover != null)
                {
                    Registrar("   · hacé clic en una celda azul libre para moverlo.");
                }

                Refrescar();
                return;
            }

            // Celda libre: mueve el defensor elegido o despliega la unidad elegida de la reserva.
            Unidad? unidad = _defensorAMover;
            if (unidad == null)
            {
                unidad = lstReserva.SelectedItem as Unidad;
            }

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
            {
                // Internamente las columnas arrancan en 0, pero al jugador se le muestran desde 1.
                int columnaParaMostrar = columna + 1;
                Registrar("Despliegue: " + unidad.NombreCompleto + " defiende " + subsistema + " (columna " + columnaParaMostrar + ").");
            }

            _defensorAMover = null;
            Refrescar();
        }

        private void RefrescarTablero()
        {
            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                int integridad = _partida.IntegridadDelCarril(carril);
                int tripulantes = _partida.Tripulacion.TripulantesEn(carril);

                if (integridad == 0)
                {
                    _lblCarriles[carril].Text = Estacion.Subsistemas[carril] + "\n¡CAÍDO!";
                }
                else
                {
                    _lblCarriles[carril].Text = Estacion.Subsistemas[carril] + "\nNúcleo " + integridad + "/" + Partida.IntegridadMaxima
                        + "\nTripulantes " + tripulantes.ToString("N0");
                }

                // Si el núcleo perdió integridad desde la última vez, el subsistema destella.
                // Las reparaciones la suben, y ahí no hace falta llamar la atención.
                if (integridad < _integridadMostrada[carril])
                {
                    _destellosCarril[carril] = PasosDestello;
                }
                _integridadMostrada[carril] = integridad;
                _objetivoNucleo[carril] = integridad;
                PintarCarril(carril);

                for (int columna = 0; columna < Grilla.Columnas; columna++)
                {
                    Unidad? unidad = _partida.Grilla.ObtenerUnidad(carril, columna);
                    if (unidad != null)
                    {
                        if (_vidaMostrada.ContainsKey(unidad) && unidad.Vida < _vidaMostrada[unidad])
                        {
                            _destellosCelda[carril, columna] = PasosDestello;
                        }
                        _vidaMostrada[unidad] = unidad.Vida;
                    }

                    ActualizarCelda(carril, columna);
                }
            }
        }

        // Una celda vacía muestra el color de su zona; una ocupada, el símbolo, la vida y el color de la unidad.
        // Si la unidad acaba de recibir daño, el fondo se tiñe de rojo y se va aclarando con cada paso del destello.
        private void ActualizarCelda(int carril, int columna)
        {
            Button celda = _celdas[carril, columna];
            Unidad? unidad = _partida.Grilla.ObtenerUnidad(carril, columna);

            string texto = "";
            Color fondo;
            if (_partida.Grilla.EsZonaDefensa(columna))
            {
                fondo = ColorZonaDefensa;
            }
            else
            {
                fondo = ColorZonaAproximacion;
            }
            Color letra = Color.Black;

            if (unidad != null)
            {
                texto = unidad.Simbolo + "\n" + unidad.Vida + " PV";
                if (unidad is Enemigo)
                {
                    Enemigo enemigo = (Enemigo)unidad;
                    texto = texto + "\nIA " + enemigo.Estrategia.Sigla;
                }

                if (unidad == _enemigoSeleccionado || unidad == _defensorAMover)
                {
                    texto = "» " + texto + " «";
                }

                fondo = unidad.Color;
                letra = Color.White;
            }

            if (_destellosCelda[carril, columna] > 0)
            {
                fondo = Mezclar(fondo, ColorDestello, _destellosCelda[carril, columna], PasosDestello);
            }

            // Solo se cambia lo que cambió, así los botones no parpadean en cada tick.
            if (celda.Text != texto)
            {
                celda.Text = texto;
            }

            if (celda.BackColor != fondo)
            {
                celda.BackColor = fondo;
            }

            if (celda.ForeColor != letra)
            {
                celda.ForeColor = letra;
            }
        }

        private void PintarCarril(int carril)
        {
            Color fondo = _colorCarrilNormal[carril];
            if (_destellosCarril[carril] > 0)
            {
                fondo = Mezclar(fondo, ColorDestello, _destellosCarril[carril], PasosDestello);
            }

            if (_lblCarriles[carril].BackColor != fondo)
            {
                _lblCarriles[carril].BackColor = fondo;
            }
        }

        // Cada 40 ms: apaga de a poco los destellos y acerca las barras a su valor real.
        private void timerAnimacion_Tick(object? sender, EventArgs e)
        {
            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                for (int columna = 0; columna < Grilla.Columnas; columna++)
                {
                    if (_destellosCelda[carril, columna] > 0)
                    {
                        _destellosCelda[carril, columna] = _destellosCelda[carril, columna] - 1;
                        ActualizarCelda(carril, columna);
                    }
                }

                if (_destellosCarril[carril] > 0)
                {
                    _destellosCarril[carril] = _destellosCarril[carril] - 1;
                    PintarCarril(carril);
                }

                AcercarBarra(_barrasNucleo[carril], _objetivoNucleo[carril]);
            }

            AcercarBarra(pbVidaHeroe, _objetivoVidaHeroe);
        }

        // Este Timer no lo creó el diseñador, así que hay que liberarlo a mano al cerrar.
        private void FormPrincipal_FormClosed(object? sender, FormClosedEventArgs e)
        {
            _timerAnimacion.Stop();
            _timerAnimacion.Dispose();
        }

        // Mueve la barra unos puntos hacia el objetivo, en vez de saltar de golpe.
        private void AcercarBarra(ProgressBar barra, int objetivo)
        {
            if (objetivo > barra.Maximum)
            {
                objetivo = barra.Maximum;
            }
            if (objetivo < barra.Minimum)
            {
                objetivo = barra.Minimum;
            }

            if (barra.Value < objetivo)
            {
                barra.Value = Math.Min(objetivo, barra.Value + PasoBarra);
            }
            else if (barra.Value > objetivo)
            {
                barra.Value = Math.Max(objetivo, barra.Value - PasoBarra);
            }
        }

        // Mezcla dos colores: con "cuanto" igual a "total" da el destino, y con 0 da el original.
        private static Color Mezclar(Color original, Color destino, int cuanto, int total)
        {
            int r = original.R + (destino.R - original.R) * cuanto / total;
            int g = original.G + (destino.G - original.G) * cuanto / total;
            int b = original.B + (destino.B - original.B) * cuanto / total;
            return Color.FromArgb(r, g, b);
        }

        #endregion

        #region Strategy: cambiar la IA enemiga

        private void cmbIA_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ActualizarInfoIA();
        }

        private void ActualizarInfoIA()
        {
            IEstrategiaIA? ia = cmbIA.SelectedItem as IEstrategiaIA;
            if (ia != null)
            {
                lblInfoIA.Text = ia.Nombre + ": " + ia.Descripcion;
            }

            if (_enemigoSeleccionado == null)
            {
                lblElegido.Text = "Clic en un enemigo del tablero para elegirlo.";
            }
            else
            {
                lblElegido.Text = "Elegido: " + _enemigoSeleccionado.NombreCompleto + " (IA " + _enemigoSeleccionado.Estrategia.Nombre + ")";
            }
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
            _hub.DesbloquearLogro(CatalogoLogros.Strategy);
        }

        private void btnIATodos_Click(object? sender, EventArgs e)
        {
            _partida.CambiarEstrategiaATodos((IEstrategiaIA)cmbIA.SelectedItem!);
            Refrescar();
            _hub.DesbloquearLogro(CatalogoLogros.Strategy);
        }

        #endregion

        #region Command: deshacer, rehacer y acciones de la estación

        // Todas las acciones del jugador pasan por acá: el gestor las ejecuta y las guarda.
        private bool EjecutarComando(IComando comando)
        {
            bool ok = _gestor.Ejecutar(comando);
            if (!ok)
            {
                Registrar("No se pudo: " + comando.Descripcion + " (faltan créditos o no se puede en este momento).");
            }

            Refrescar();
            return ok;
        }

        private void btnDeshacer_Click(object? sender, EventArgs e)
        {
            bool habiaAlgo = _gestor.PuedeDeshacer;
            Registrar(_gestor.Deshacer());
            _defensorAMover = null;
            Refrescar();

            if (habiaAlgo)
            {
                _hub.DesbloquearLogro(CatalogoLogros.Command);
            }
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

            if (lstComandos.Items.Count == _gestor.Historial.Count)
            {
                return;
            }

            lstComandos.BeginUpdate();
            lstComandos.Items.Clear();
            IReadOnlyList<string> historial = _gestor.Historial;
            for (int i = 0; i < historial.Count; i++)
            {
                lstComandos.Items.Add(historial[i]);
            }
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
            IReadOnlyList<Unidad> reserva = _partida.Estacion.Reserva;
            if (!ListaMuestraLaReserva(reserva))
            {
                object? seleccionada = lstReserva.SelectedItem;
                bool hayNuevas = reserva.Count > lstReserva.Items.Count;

                lstReserva.BeginUpdate();
                lstReserva.Items.Clear();
                for (int i = 0; i < reserva.Count; i++)
                {
                    lstReserva.Items.Add(reserva[i]);
                }

                if (hayNuevas)
                {
                    lstReserva.SelectedIndex = lstReserva.Items.Count - 1; // la recién construida
                }
                else if (seleccionada != null && lstReserva.Items.Contains(seleccionada))
                {
                    lstReserva.SelectedItem = seleccionada;
                }
                else if (lstReserva.Items.Count > 0)
                {
                    lstReserva.SelectedIndex = 0;
                }

                lstReserva.EndUpdate();
            }

            ActualizarDetalleReserva();
        }

        // Compara uno por uno: tienen que ser las mismas unidades y en el mismo orden.
        private bool ListaMuestraLaReserva(IReadOnlyList<Unidad> reserva)
        {
            if (lstReserva.Items.Count != reserva.Count)
            {
                return false;
            }

            for (int i = 0; i < reserva.Count; i++)
            {
                if ((Unidad)lstReserva.Items[i] != reserva[i])
                {
                    return false;
                }
            }

            return true;
        }

        private void ActualizarDetalleReserva()
        {
            Unidad? unidad = lstReserva.SelectedItem as Unidad;
            if (unidad == null)
            {
                lblDetalleReserva.Text = "Reserva vacía. Construí unidades desde el menú (Astillero o Laboratorio).";
            }
            else
            {
                lblDetalleReserva.Text = unidad.Descripcion() + "\nHacé clic en una celda azul para desplegarla.";
            }
        }

        #endregion

        #region Chain of Responsibility: cadena de defensas

        // Se vuelve a armar en cada refresco porque el nombre del escudo muestra su carga.
        private void RefrescarCadena(Heroe heroe)
        {
            int seleccion = lstCadena.SelectedIndex;

            lstCadena.BeginUpdate();
            lstCadena.Items.Clear();
            List<Defensa> defensas = heroe.ObtenerCadena().ObtenerDefensas();
            for (int i = 0; i < defensas.Count; i++)
            {
                lstCadena.Items.Add(defensas[i].ObtenerNombre());
            }
            lstCadena.SelectedIndex = seleccion;
            lstCadena.EndUpdate();
        }

        private void RefrescarRecorrido(Heroe heroe)
        {
            Golpe? golpe = heroe.ObtenerUltimoGolpe();
            if (golpe == null || golpe == _golpeMostrado)
            {
                return;
            }

            _golpeMostrado = golpe;
            lstRecorrido.Items.Clear();
            List<string> pasos = golpe.ObtenerPasos();
            for (int i = 0; i < pasos.Count; i++)
            {
                lstRecorrido.Items.Add(pasos[i]);
            }
        }

        private void btnSubirDefensa_Click(object? sender, EventArgs e)
        {
            int posicion = lstCadena.SelectedIndex;
            if (posicion > 0)
            {
                _partida.ObtenerHeroe().ObtenerCadena().Subir(posicion);
                lstCadena.SelectedIndex = posicion - 1;
                Refrescar();
                _hub.DesbloquearLogro(CatalogoLogros.ChainOfResponsibility);
            }
        }

        private void btnBajarDefensa_Click(object? sender, EventArgs e)
        {
            int posicion = lstCadena.SelectedIndex;
            if (posicion >= 0 && posicion < lstCadena.Items.Count - 1)
            {
                _partida.ObtenerHeroe().ObtenerCadena().Bajar(posicion);
                lstCadena.SelectedIndex = posicion + 1;
                Refrescar();
                _hub.DesbloquearLogro(CatalogoLogros.ChainOfResponsibility);
            }
        }

        #endregion

        // STATE: el panel le pregunta todo al estado actual, sin preguntar en qué estado está.
        // En batalla la reserva no se usa, así que el panel ocupa su lugar.
        private void RefrescarPanelComandante()
        {
            Heroe heroe = _partida.ObtenerHeroe();

            lblEstadoHeroe.Text = "Estado: " + heroe.ObtenerEstado().ObtenerNombre();
            lblVidaHeroe.Text = "Vida: " + heroe.Vida + " / " + heroe.VidaMaxima;

            // Si el máximo baja, la barra recorta su valor sola. La vida se acerca de a poco en el Timer de animación.
            pbVidaHeroe.Maximum = heroe.VidaMaxima;
            _objetivoVidaHeroe = heroe.Vida;
            lblDanioHeroe.Text = "Daño: " + heroe.Danio;

            RefrescarCadena(heroe);
            RefrescarRecorrido(heroe);

            bool enBatalla = _partida.Fase == FasePartida.Oleada;
            grpComandante.Visible = enBatalla;
            grpReserva.Visible = !enBatalla;

            // El único logro que se gana por un cambio de estado y no por un botón.
            if (heroe.ObtenerEstado() is EstadoEnfurecido)
            {
                _hub.DesbloquearLogro(CatalogoLogros.State);
            }
        }

        private void Refrescar()
        {
            // Un enemigo elegido deja de estarlo si murió o si salió del tablero (al golpear el núcleo).
            if (_enemigoSeleccionado != null && (!_enemigoSeleccionado.EstaViva || _enemigoSeleccionado.Carril < 0))
            {
                _enemigoSeleccionado = null;
            }

            RefrescarTablero();
            RefrescarReserva();
            RefrescarComandos();
            RefrescarPanelComandante();
            menuInventario.Enabled = _partida.Fase == FasePartida.Preparacion;
            ActualizarInfoIA();

            string fase;
            if (_partida.Fase == FasePartida.Preparacion)
            {
                fase = "Preparación";
            }
            else if (_partida.Fase == FasePartida.Oleada)
            {
                fase = "¡Combate!";
            }
            else
            {
                fase = _partida.Fase.ToString();
            }

            lblEstado.Text = "Oleada " + _partida.NumeroOleada + "/" + _partida.TotalOleadas + " · " + fase + "      " +
                             "Créditos: " + _partida.Estacion.Creditos + "      Defensores: " + _partida.Grilla.Defensores().Count + "      " +
                             "Enemigos: " + _partida.Grilla.Enemigos().Count + "      En reserva: " + _partida.Estacion.Reserva.Count + "      " +
                             "Bonus por oleada: +" + _partida.BonusRecursosPorOleada;

            if (_partida.Fase == FasePartida.Preparacion)
            {
                btnIniciarOleada.Enabled = true;
                btnIniciarOleada.Text = "Iniciar oleada " + _partida.NumeroOleada + "/" + _partida.TotalOleadas;
            }
            else if (_partida.Fase == FasePartida.Oleada)
            {
                btnIniciarOleada.Enabled = false;
                btnIniciarOleada.Text = "Oleada en curso...";
            }
            else
            {
                // Con la partida terminada el botón sirve para volver a ver el resumen final.
                btnIniciarOleada.Enabled = true;
                btnIniciarOleada.Text = "Ver resumen de la partida";
            }
        }

        private void AgregarAlHistorial(EntradaTurno entrada)
        {
            txtHistorial.AppendText(entrada + Environment.NewLine);
            _lineasHistorial = _lineasHistorial + 1;

            // Para que el cuadro no crezca sin límite, se borran las 400 líneas más viejas.
            if (_lineasHistorial > 800)
            {
                string[] lineas = txtHistorial.Lines;
                int cantidadRestante = Math.Max(0, lineas.Length - 400);
                string[] restantes = new string[cantidadRestante];
                for (int i = 0; i < cantidadRestante; i++)
                {
                    restantes[i] = lineas[i + 400];
                }

                txtHistorial.Lines = restantes;
                _lineasHistorial = txtHistorial.Lines.Length;
            }
        }

        private void Registrar(string mensaje)
        {
            txtRegistro.AppendText(mensaje + Environment.NewLine);
        }
    }
}

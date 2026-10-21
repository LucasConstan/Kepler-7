using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Builder;
using Kepler7.Core.Patrones.Command;
using Kepler7.Core.Patrones.Facade;
using Kepler7.Core.Patrones.Flyweight;
using Kepler7.Core.Patrones.Prototype;
using Kepler7.Core.Patrones.Strategy;
using Kepler7.Core.Patrones.TemplateMethod;
using Kepler7.Game.UI;

namespace Kepler7.Game
{
    public partial class Form1 : Form
    {
        private readonly Partida _partida = new Partida(600);
        private Estacion _estacion => _partida.Estacion;

        // Builder: un director y una receta (builder concreto) por tipo de nave.
        private readonly Astillero _astillero = new Astillero();
        private readonly List<INaveApoyoBuilder> _recetas = new List<INaveApoyoBuilder>
        {
            new NaveExploradoraBuilder(),
            new NaveCombateBuilder(),
            new NaveCargaBuilder(),
        };

        // Prototype: registro de tripulantes base que se clonan.
        private readonly LaboratorioClonacion _laboratorio = LaboratorioClonacion.CrearConPrototiposBase();
        private readonly List<Equipo> _catalogoEquipo = Equipo.Catalogo();

        // Command: todas las acciones del jugador pasan por el gestor (deshacer/rehacer + historial).
        private readonly GestorComandos _gestor = new GestorComandos();

        // Facade: el formulario solo habla con el control central, nunca con los subsistemas.
        private readonly ControlCentral _control;
        private bool _estacionApagada;
        private Unidad? _defensorAMover;

        // Cada cuántos milisegundos pasa un tick (el último valor es la pausa).
        private static readonly string[] _nombresVelocidad = { "Lenta", "Normal", "Rápida", "Pausa" };
        private static readonly int[] _intervalosVelocidad = { 900, 600, 250, 0 };

        private readonly System.Windows.Forms.Timer _timerTick;
        private Enemigo? _enemigoSeleccionado;
        private int _lineasHistorial;

        // Se crean en ConstruirInterfaz(), que corre en el constructor.
        private Label _lblEstado = null!;
        private TableroControl _tablero = null!;
        private ComboBox _cmbRecetas = null!;
        private Label _lblReceta = null!;
        private ListBox _lstPrototipos = null!;
        private ListBox _lstReserva = null!;
        private Label _lblDetalleReserva = null!;
        private ComboBox _cmbEquipo = null!;
        private TextBox _txtRegistro = null!;
        private TextBox _txtHistorial = null!;
        private Button _btnOleada = null!;
        private ComboBox _cmbVelocidad = null!;
        private ComboBox _cmbIA = null!;
        private Label _lblInfoIA = null!;
        private Label _lblBiblioteca = null!;
        private ComboBox _cmbCarrilTripulantes = null!;
        private Button _btnDeshacer = null!;
        private Button _btnRehacer = null!;
        private ListBox _lstComandos = null!;
        private ComboBox _cmbCarrilReparar = null!;
        private Button _btnEmergencia = null!;
        private TextBox _txtControl = null!;

        public Form1()
        {
            InitializeComponent();
            _control = new ControlCentral(_partida);
            _timerTick = new System.Windows.Forms.Timer(components!);
            _timerTick.Tick += Timer_Tick;

            ConstruirInterfaz();

            _partida.Mensaje += Registrar;
            _partida.Historial.EntradaRegistrada += AgregarAlHistorial;

            Refrescar();
            Registrar("Estación en línea. Ensamblá naves en el astillero o cloná tripulantes en el laboratorio,");
            Registrar("desplegalos en el tablero y cuando estés listo iniciá la oleada.");
        }

        #region Construcción de la interfaz

        private void ConstruirInterfaz()
        {
            BackColor = Paleta.Fondo;
            ForeColor = Paleta.Texto;
            Font = new Font("Segoe UI", 9f);
            MinimumSize = new Size(1500, 820);
            StartPosition = FormStartPosition.CenterScreen;

            _lblEstado = new Label
            {
                Dock = DockStyle.Top,
                Height = 38,
                Padding = new Padding(14, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI Semibold", 10.5f),
                BackColor = Paleta.Panel,
            };

            _tablero = new TableroControl { Dock = DockStyle.Fill, Partida = _partida };
            _tablero.CeldaClick += Tablero_CeldaClick;

            var barraCombate = ConstruirBarraCombate();
            var panelInferior = ConstruirPanelInferior();
            var panelDerecho = ConstruirPanelDerecho();
            var panelIzquierdo = ConstruirPanelIzquierdo();

            // Con Dock, el último control agregado se acomoda primero: se agregan de adentro hacia afuera.
            Controls.Add(_tablero);
            Controls.Add(panelDerecho);
            Controls.Add(panelIzquierdo);
            Controls.Add(panelInferior);
            Controls.Add(barraCombate);
            Controls.Add(_lblEstado);

            // Botón de reiniciar en la misma barra de estado, pegado a la derecha.
            var btnReiniciar = CrearBoton("⟳ Reiniciar partida", Point.Empty, 160, Reiniciar_Click);
            btnReiniciar.Dock = DockStyle.Right;
            _lblEstado.Controls.Add(btnReiniciar);

            _cmbRecetas.SelectedIndex = 0;
        }

        private void Reiniciar_Click(object? sender, EventArgs e)
        {
            var respuesta = MessageBox.Show("¿Reiniciar la partida? Se pierde todo el progreso actual.", "Reiniciar",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Reabre la aplicación desde cero: así también se reinician los contadores (ids, especies...).
            if (respuesta == DialogResult.Yes) Application.Restart();
        }

        // Barra de arriba: iniciar oleada, velocidad y cambio de IA enemiga (Strategy).
        private Panel ConstruirBarraCombate()
        {
            var barra = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Paleta.Fondo };

            _btnOleada = CrearBoton("Iniciar oleada", new Point(12, 10), 170, BtnOleada_Click);

            var lblVelocidad = new Label { Text = "Velocidad:", Location = new Point(196, 17), AutoSize = true };
            _cmbVelocidad = CrearCombo(new Point(266, 13), 100);
            _cmbVelocidad.Items.AddRange(_nombresVelocidad.Cast<object>().ToArray());
            _cmbVelocidad.SelectedIndex = 1;
            _cmbVelocidad.SelectedIndexChanged += (s, e) => AplicarVelocidad();

            var lblIA = new Label { Text = "IA enemiga  [Strategy]:", Location = new Point(392, 17), AutoSize = true };
            _cmbIA = CrearCombo(new Point(538, 13), 120);
            _cmbIA.DisplayMember = "Nombre";
            _cmbIA.Items.AddRange(EstrategiasIA.Todas.Cast<object>().ToArray());
            _cmbIA.SelectedIndexChanged += (s, e) => ActualizarInfoIA();
            var btnUno = CrearBoton("Al enemigo elegido", new Point(668, 10), 150, BtnIAUno_Click);
            var btnTodos = CrearBoton("A todos", new Point(826, 10), 90, BtnIATodos_Click);

            _lblInfoIA = new Label
            {
                Location = new Point(12, 48),
                Size = new Size(1100, 28),
                ForeColor = Paleta.TextoSuave,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };

            barra.Controls.AddRange(new Control[]
            {
                _btnOleada, lblVelocidad, _cmbVelocidad, lblIA, _cmbIA, btnUno, btnTodos, _lblInfoIA
            });

            _cmbIA.SelectedIndex = 0;
            return barra;
        }

        // Abajo: historial de turnos (Template Method) y registro general de la estación.
        private Panel ConstruirPanelInferior()
        {
            var panel = new Panel { Dock = DockStyle.Bottom, Height = 180, Padding = new Padding(8, 0, 8, 8), BackColor = Paleta.Panel };
            var tabla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));

            _txtHistorial = CrearTextoSoloLectura();
            _txtRegistro = CrearTextoSoloLectura();

            var grpHistorial = new GroupBox { Text = "Historial de turnos  [Template Method]", Dock = DockStyle.Fill, ForeColor = Paleta.Texto };
            grpHistorial.Controls.Add(_txtHistorial);
            var grpRegistro = new GroupBox { Text = "Registro de la estación", Dock = DockStyle.Fill, ForeColor = Paleta.Texto };
            grpRegistro.Controls.Add(_txtRegistro);

            tabla.Controls.Add(grpHistorial, 0, 0);
            tabla.Controls.Add(grpRegistro, 1, 0);
            panel.Controls.Add(tabla);
            return panel;
        }

        // A la derecha: astillero (Builder), laboratorio (Prototype) y reserva.
        private Panel ConstruirPanelDerecho()
        {
            var panelDerecho = new Panel
            {
                Dock = DockStyle.Right,
                Width = 390,
                Padding = new Padding(8),
                BackColor = Paleta.Panel,
                AutoScroll = true,
                AutoScrollMinSize = new Size(0, 580),
            };

            // --- Builder ---
            var grpAstillero = CrearGrupo("Astillero - Nave de apoyo  [Builder]", 146);
            _cmbRecetas = CrearCombo(new Point(12, 26), 350);
            _cmbRecetas.Items.AddRange(_recetas.Cast<object>().ToArray());
            _cmbRecetas.SelectedIndexChanged += (s, e) => ActualizarDescripcionReceta();
            _lblReceta = new Label { Location = new Point(12, 56), Size = new Size(350, 44), ForeColor = Paleta.TextoSuave };
            var btnEnsamblar = CrearBoton("Ensamblar nave", new Point(12, 104), 350, BtnEnsamblar_Click);
            grpAstillero.Controls.AddRange(new Control[] { _cmbRecetas, _lblReceta, btnEnsamblar });

            // --- Prototype ---
            var grpLaboratorio = CrearGrupo("Laboratorio de clonación  [Prototype]", 192);
            _lstPrototipos = CrearLista(new Point(12, 26), new Size(350, 116));
            _lstPrototipos.Format += (s, e) => e.Value = ResumenPrototipo((Tripulante)e.ListItem!);
            var btnClonar = CrearBoton("Clonar prototipo seleccionado", new Point(12, 150), 350, BtnClonar_Click);
            grpLaboratorio.Controls.AddRange(new Control[] { _lstPrototipos, btnClonar });

            // --- Reserva ---
            var grpReserva = CrearGrupo("Reserva - listas para desplegar", 222);
            _lstReserva = CrearLista(new Point(12, 26), new Size(350, 100));
            _lstReserva.SelectedIndexChanged += (s, e) => ActualizarDetalleReserva();
            _lblDetalleReserva = new Label { Location = new Point(12, 130), Size = new Size(350, 48), ForeColor = Paleta.TextoSuave };
            _cmbEquipo = CrearCombo(new Point(12, 184), 230);
            _cmbEquipo.Items.AddRange(_catalogoEquipo.Cast<object>().ToArray());
            _cmbEquipo.SelectedIndex = 0;
            var btnEquipar = CrearBoton("Equipar clon", new Point(250, 182), 112, BtnEquipar_Click);
            grpReserva.Controls.AddRange(new Control[] { _lstReserva, _lblDetalleReserva, _cmbEquipo, btnEquipar });

            // Con Dock, el último control agregado se acomoda primero: se agregan de abajo hacia arriba.
            panelDerecho.Controls.Add(grpReserva);
            panelDerecho.Controls.Add(grpLaboratorio);
            panelDerecho.Controls.Add(grpAstillero);
            return panelDerecho;
        }

        // A la izquierda: control central (Facade), acciones con deshacer/rehacer (Command)
        // y biblioteca de tripulantes (Flyweight).
        private Panel ConstruirPanelIzquierdo()
        {
            var panelIzquierdo = new Panel
            {
                Dock = DockStyle.Left,
                Width = 340,
                Padding = new Padding(8),
                BackColor = Paleta.Panel,
                AutoScroll = true,
                AutoScrollMinSize = new Size(0, 830),
            };

            // --- Flyweight ---
            var grpBiblioteca = CrearGrupo("Biblioteca de tripulantes  [Flyweight]", 250);
            var lblEnviar = new Label { Text = "Enviar tripulantes al núcleo de:", Location = new Point(12, 24), AutoSize = true, ForeColor = Paleta.TextoSuave };
            _cmbCarrilTripulantes = CrearCombo(new Point(12, 46), 140);
            _cmbCarrilTripulantes.Items.AddRange(Estacion.Subsistemas.Cast<object>().ToArray());
            _cmbCarrilTripulantes.SelectedIndex = 0;
            var btnEnviar = CrearBoton($"+{Partida.TripulantesPorEnvio} ({Partida.CostoEnvioTripulantes} cr)", new Point(162, 44), 140, BtnEnviarTripulantes_Click);
            _lblBiblioteca = new Label { Location = new Point(12, 82), Size = new Size(290, 160), ForeColor = Paleta.TextoSuave };
            grpBiblioteca.Controls.AddRange(new Control[] { lblEnviar, _cmbCarrilTripulantes, btnEnviar, _lblBiblioteca });

            // --- Command ---
            var grpComandos = CrearGrupo("Acciones del jugador  [Command]", 330);
            _btnDeshacer = CrearBoton("↶ Deshacer", new Point(12, 26), 140, BtnDeshacer_Click);
            _btnRehacer = CrearBoton("Rehacer ↷", new Point(162, 26), 140, BtnRehacer_Click);
            _lstComandos = CrearLista(new Point(12, 62), new Size(290, 160));
            var lblReparar = new Label { Text = "Reparar núcleo (entre oleadas):", Location = new Point(12, 230), AutoSize = true, ForeColor = Paleta.TextoSuave };
            _cmbCarrilReparar = CrearCombo(new Point(12, 250), 140);
            _cmbCarrilReparar.Items.AddRange(Estacion.Subsistemas.Cast<object>().ToArray());
            _cmbCarrilReparar.SelectedIndex = 0;
            var btnReparar = CrearBoton($"Reparar ({CurarComando.Costo} cr)", new Point(162, 248), 140, BtnReparar_Click);
            var btnDisparo = CrearBoton($"Disparo orbital al enemigo elegido ({AtacarComando.Costo} cr)", new Point(12, 286), 290, BtnDisparo_Click);
            grpComandos.Controls.AddRange(new Control[]
            {
                _btnDeshacer, _btnRehacer, _lstComandos, lblReparar, _cmbCarrilReparar, btnReparar, btnDisparo
            });

            // --- Facade ---
            var grpControl = CrearGrupo("Control central  [Facade]", 230);
            var btnIniciar = CrearBoton("Iniciar turno", new Point(12, 26), 92, BtnOleada_Click);
            _btnEmergencia = CrearBoton("Emergencia", new Point(110, 26), 92, BtnEmergencia_Click);
            var btnApagar = CrearBoton("Apagar", new Point(208, 26), 94, BtnApagar_Click);
            btnApagar.BackColor = Color.FromArgb(170, 60, 70);
            _txtControl = CrearTextoSoloLectura();
            _txtControl.Dock = DockStyle.None;
            _txtControl.Location = new Point(12, 62);
            _txtControl.Size = new Size(290, 158);
            _txtControl.Text = "Las tres operaciones coordinan a los 6 subsistemas: acá se ve la secuencia.";
            grpControl.Controls.AddRange(new Control[] { btnIniciar, _btnEmergencia, btnApagar, _txtControl });

            // Con Dock, el último control agregado queda arriba.
            panelIzquierdo.Controls.Add(grpBiblioteca);
            panelIzquierdo.Controls.Add(grpComandos);
            panelIzquierdo.Controls.Add(grpControl);
            return panelIzquierdo;
        }

        private static TextBox CrearTextoSoloLectura()
        {
            return new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Paleta.Fondo,
                ForeColor = Paleta.Texto,
                BorderStyle = BorderStyle.None,
                Font = new Font("Consolas", 8.5f),
            };
        }

        private static GroupBox CrearGrupo(string titulo, int alto)
        {
            return new GroupBox { Text = titulo, Dock = DockStyle.Top, Height = alto, ForeColor = Paleta.Texto };
        }

        private static ComboBox CrearCombo(Point ubicacion, int ancho)
        {
            return new ComboBox
            {
                Location = ubicacion,
                Width = ancho,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Paleta.Control,
                ForeColor = Paleta.Texto,
            };
        }

        private static ListBox CrearLista(Point ubicacion, Size tamanio)
        {
            return new ListBox
            {
                Location = ubicacion,
                Size = tamanio,
                BackColor = Paleta.Control,
                ForeColor = Paleta.Texto,
                BorderStyle = BorderStyle.FixedSingle,
                IntegralHeight = false,
                FormattingEnabled = true,
            };
        }

        private static Button CrearBoton(string texto, Point ubicacion, int ancho, EventHandler click)
        {
            var boton = new Button
            {
                Text = texto,
                Location = ubicacion,
                Size = new Size(ancho, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Paleta.Acento,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
            };
            boton.FlatAppearance.BorderSize = 0;
            boton.Click += click;
            return boton;
        }

        #endregion

        #region Bucle de la partida (Timer)

        // Iniciar la oleada pasa por la fachada: ella prepara los subsistemas y arranca la oleada.
        private void BtnOleada_Click(object? sender, EventArgs e)
        {
            if (_estacionApagada)
            {
                PreguntarNuevaPartida();
                return;
            }
            MostrarPasosControl(_control.IniciarTurno());
            if (_partida.Fase != FasePartida.Oleada) return;
            _gestor.Limpiar($"Empieza la oleada {_partida.NumeroOleada}");
            _defensorAMover = null;
            AplicarVelocidad();
            Refrescar();
        }

        // Cada tick de Windows Forms Timer = un tick del juego.
        private void Timer_Tick(object? sender, EventArgs e)
        {
            _partida.AvanzarTick();
            Refrescar();

            if (_partida.Fase != FasePartida.Oleada)
            {
                _timerTick.Stop();
                _gestor.Limpiar("Terminó la oleada");
                Refrescar();
                if (_partida.Fase == FasePartida.Victoria)
                    MessageBox.Show("¡Salvaste Kepler-7!", "Victoria", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (_partida.Fase == FasePartida.Derrota)
                    MessageBox.Show("Cayeron demasiados subsistemas. Kepler-7 se perdió.", "Derrota", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AplicarVelocidad()
        {
            int intervalo = _intervalosVelocidad[_cmbVelocidad.SelectedIndex];
            if (intervalo == 0 || _partida.Fase != FasePartida.Oleada)
            {
                _timerTick.Stop();
                return;
            }
            _timerTick.Interval = intervalo;
            _timerTick.Start();
        }

        #endregion

        #region Strategy: cambiar la IA enemiga

        private void ActualizarInfoIA()
        {
            var ia = (IEstrategiaIA)_cmbIA.SelectedItem!;
            string seleccionado = _enemigoSeleccionado == null
                ? "Hacé clic en un enemigo del tablero para elegirlo."
                : $"Enemigo elegido: {_enemigoSeleccionado.NombreCompleto} (IA {_enemigoSeleccionado.Estrategia.Nombre}).";
            _lblInfoIA.Text = $"{ia.Nombre}: {ia.Descripcion}     {seleccionado}";
        }

        private void BtnIAUno_Click(object? sender, EventArgs e)
        {
            if (_enemigoSeleccionado == null || !_enemigoSeleccionado.EstaViva)
            {
                Registrar("Primero hacé clic en un enemigo del tablero para elegirlo.");
                return;
            }
            _partida.CambiarEstrategia(_enemigoSeleccionado, (IEstrategiaIA)_cmbIA.SelectedItem!);
            Refrescar();
        }

        private void BtnIATodos_Click(object? sender, EventArgs e)
        {
            _partida.CambiarEstrategiaATodos((IEstrategiaIA)_cmbIA.SelectedItem!);
            Refrescar();
        }

        #endregion

        #region Builder: astillero

        private void ActualizarDescripcionReceta()
        {
            var receta = _cmbRecetas.SelectedItem as INaveApoyoBuilder;
            if (receta == null) return;
            _lblReceta.Text = $"{receta.DescripcionReceta}\nCosto: {receta.Costo} créditos";
        }

        private void BtnEnsamblar_Click(object? sender, EventArgs e)
        {
            var receta = (INaveApoyoBuilder)_cmbRecetas.SelectedItem!;
            if (_estacion.Creditos < receta.Costo)
            {
                Registrar($"Astillero: no alcanzan los créditos para una nave {receta.NombreReceta} ({receta.Costo} cr).");
                return;
            }

            // El director arma la nave paso a paso; la receta elegida decide cada pieza.
            // Pagarla y mandarla a la reserva es un comando, así se puede deshacer.
            NaveApoyo nave = _astillero.Ensamblar(receta);
            if (!EjecutarComando(new ConstruirComando(_estacion, nave))) return;

            Registrar($"Astillero: ensamblada {nave.NombreCompleto} (-{nave.Costo} cr)");
            foreach (var componente in nave.Componentes)
                Registrar("   · " + componente);

            Refrescar(seleccionar: nave);
        }

        #endregion

        #region Prototype: laboratorio y equipamiento

        private static string ResumenPrototipo(Tripulante t)
        {
            var equipo = string.Join(", ", t.Equipamiento.Select(eq => eq.Nombre));
            return $"{t.NombreCompleto}  ·  Daño {t.Danio}  ·  Vida {t.VidaMaxima}  ·  {t.Costo} cr  ·  [{equipo}]";
        }

        private void BtnClonar_Click(object? sender, EventArgs e)
        {
            var prototipo = _lstPrototipos.SelectedItem as Tripulante;
            if (prototipo == null)
            {
                Registrar("Laboratorio: seleccioná un prototipo para clonar.");
                return;
            }
            if (_estacion.Creditos < prototipo.Costo)
            {
                Registrar($"Laboratorio: no alcanzan los créditos para clonar un {prototipo.Rol} ({prototipo.Costo} cr).");
                return;
            }

            Tripulante clon = _laboratorio.Clonar(prototipo.Rol);
            if (!EjecutarComando(new ConstruirComando(_estacion, clon))) return;

            Registrar($"Laboratorio: {clon.NombreCompleto} clonado de {prototipo.NombreCompleto} (-{prototipo.Costo} cr)");
            Registrar($"   · equipo copiado: {string.Join(", ", clon.Equipamiento.Select(eq => eq.Nombre))}");

            Refrescar(seleccionar: clon);
        }

        private void BtnEquipar_Click(object? sender, EventArgs e)
        {
            var clon = _lstReserva.SelectedItem as Tripulante;
            if (clon == null)
            {
                Registrar("Equipamiento: seleccioná un tripulante de la reserva (las naves se configuran en el astillero).");
                return;
            }

            var modelo = (Equipo)_cmbEquipo.SelectedItem!;
            if (!_estacion.Gastar(modelo.Costo))
            {
                Registrar($"Equipamiento: no alcanzan los créditos para {modelo.Nombre} ({modelo.Costo} cr).");
                return;
            }

            clon.Equipar(modelo.Clonar());

            // Demostración: el prototipo original no cambió, porque el clon tiene su propia lista de equipo.
            var original = _laboratorio.ObtenerPrototipo(clon.Rol);
            Registrar($"Equipamiento: {clon.NombreCompleto} + {modelo.Nombre} → Daño {clon.Danio}, Vida {clon.VidaMaxima} (-{modelo.Costo} cr)");
            Registrar($"   · prototipo {original.NombreCompleto} sin cambios → Daño {original.Danio}, Vida {original.VidaMaxima}, equipo: {original.Equipamiento.Count} pieza(s)");

            Refrescar(seleccionar: clon);
        }

        #endregion

        #region Flyweight: biblioteca de tripulantes

        private void BtnEnviarTripulantes_Click(object? sender, EventArgs e)
        {
            int carril = _cmbCarrilTripulantes.SelectedIndex;
            if (!_partida.EnviarTripulantes(carril))
                Registrar($"Tripulación: no se pudo enviar a {Estacion.Subsistemas[carril]} (faltan créditos o el subsistema cayó).");
            Refrescar();
        }

        private void ActualizarBiblioteca()
        {
            var tripulacion = _partida.Tripulacion;
            _lblBiblioteca.Text =
                $"Tripulantes vivos: {tripulacion.TotalVivos:N0} de {tripulacion.TotalGenerados:N0}\n" +
                $"Especies en la fábrica: {tripulacion.EspeciesEnFabrica} (siempre 5)\n\n" +
                $"Comparación con los primeros 10.000:\n" +
                $"CON Flyweight: {tripulacion.ObjetosEspecieConFlyweight} objetos Especie, {tripulacion.BytesConFlyweight / 1024} KB\n" +
                $"SIN Flyweight: {tripulacion.ObjetosEspecieSinFlyweight:N0} objetos Especie, {tripulacion.BytesSinFlyweight / 1024} KB\n\n" +
                "Los tripulantes protegen el núcleo: un enemigo que llega al final mata tripulantes, y solo daña el núcleo cuando no queda ninguno.";
        }

        #endregion

        #region Tablero

        private void Tablero_CeldaClick(object? sender, CeldaEventArgs e)
        {
            string subsistema = Estacion.Subsistemas[e.Carril];
            var ocupante = _partida.Grilla.ObtenerUnidad(e.Carril, e.Columna);

            if (ocupante is Enemigo enemigo)
            {
                _enemigoSeleccionado = enemigo;
                _defensorAMover = null;
                _tablero.Seleccionada = enemigo;
                Registrar($"[{subsistema}] {enemigo.NombreCompleto}: {enemigo.Descripcion()}");
                ActualizarInfoIA();
                _tablero.Invalidate();
                return;
            }
            if (ocupante != null)
            {
                Registrar($"[{subsistema}] {ocupante.NombreCompleto}: {ocupante.Descripcion()}");
                if (_partida.Fase != FasePartida.Preparacion) return;

                // Entre oleadas, un clic en un defensor lo elige para moverlo (otro clic lo suelta).
                _defensorAMover = ocupante == _defensorAMover ? null : ocupante;
                _tablero.Seleccionada = _defensorAMover;
                if (_defensorAMover != null) Registrar("   · hacé clic en una celda azul libre para moverlo.");
                _tablero.Invalidate();
                return;
            }

            // Celda libre: mueve el defensor elegido o despliega la unidad elegida de la reserva.
            var unidad = _defensorAMover ?? _lstReserva.SelectedItem as Unidad;
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
            if (!_partida.Grilla.EsZonaDefensa(e.Columna))
            {
                Registrar("Solo se puede desplegar en la zona de defensa (celdas azules).");
                return;
            }

            if (EjecutarComando(new MoverComando(_partida, unidad, e.Carril, e.Columna)))
                Registrar($"Despliegue: {unidad.NombreCompleto} defiende {subsistema} (columna {e.Columna + 1}).");
            _defensorAMover = null;
            _tablero.Seleccionada = null;
            Refrescar();
        }

        #endregion

        #region Facade: control central

        private void BtnEmergencia_Click(object? sender, EventArgs e)
        {
            MostrarPasosControl(_control.EntrarEnEmergencia());
            Refrescar();
        }

        private void BtnApagar_Click(object? sender, EventArgs e)
        {
            var respuesta = MessageBox.Show("¿Apagar la estación? La partida termina.", "Apagar Kepler-7",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (respuesta != DialogResult.Yes) return;

            _timerTick.Stop();
            MostrarPasosControl(_control.ApagarEstacion());
            Refrescar();

            _estacionApagada = true;
            PreguntarNuevaPartida();
        }

        // La partida terminó porque se apagó la estación: se ofrece empezar una nueva.
        // Si el jugador dice que no, solo puede reiniciar con el botón "Reiniciar partida".
        private void PreguntarNuevaPartida()
        {
            var nueva = MessageBox.Show("Kepler-7 fue apagada y la partida terminó.\n¿Querés empezar una partida nueva?",
                                        "Estación apagada", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (nueva == DialogResult.Yes) Application.Restart();
        }

        // Muestra en el panel la secuencia de acciones que coordinó la fachada.
        private void MostrarPasosControl(List<string> pasos)
        {
            _txtControl.Text = string.Join(Environment.NewLine, pasos);
            foreach (var paso in pasos) Registrar(paso);
        }

        // Un subsistema está crítico si su núcleo bajó a menos de la mitad (y no cayó).
        private bool HayCarrilCritico()
        {
            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                int integridad = _partida.IntegridadDelCarril(carril);
                if (integridad > 0 && integridad < Partida.IntegridadMaxima / 2) return true;
            }
            return false;
        }

        private void ActualizarControl()
        {
            _btnEmergencia.Enabled = _control.EmergenciaDisponible;
            bool alerta = _control.EmergenciaDisponible && HayCarrilCritico();
            _btnEmergencia.Text = alerta ? "¡EMERGENCIA!" : "Emergencia";
            _btnEmergencia.BackColor = alerta ? Color.FromArgb(220, 120, 40) : Paleta.Acento;
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

        private void BtnDeshacer_Click(object? sender, EventArgs e)
        {
            Registrar(_gestor.Deshacer());
            _defensorAMover = null;
            Refrescar();
        }

        private void BtnRehacer_Click(object? sender, EventArgs e)
        {
            Registrar(_gestor.Rehacer());
            Refrescar();
        }

        private void BtnReparar_Click(object? sender, EventArgs e)
        {
            EjecutarComando(new CurarComando(_partida, _cmbCarrilReparar.SelectedIndex));
        }

        private void BtnDisparo_Click(object? sender, EventArgs e)
        {
            if (_enemigoSeleccionado == null || !_enemigoSeleccionado.EstaViva)
            {
                Registrar("Primero hacé clic en un enemigo del tablero para elegirlo.");
                return;
            }
            EjecutarComando(new AtacarComando(_partida, _enemigoSeleccionado));
        }

        private void ActualizarComandos()
        {
            _btnDeshacer.Enabled = _gestor.PuedeDeshacer;
            _btnRehacer.Enabled = _gestor.PuedeRehacer;

            if (_lstComandos.Items.Count == _gestor.Historial.Count) return;
            _lstComandos.BeginUpdate();
            _lstComandos.Items.Clear();
            foreach (var linea in _gestor.Historial) _lstComandos.Items.Add(linea);
            _lstComandos.TopIndex = Math.Max(0, _lstComandos.Items.Count - 1);
            _lstComandos.EndUpdate();
        }

        #endregion

        private void Refrescar(Unidad? seleccionar = null)
        {
            _lstPrototipos.BeginUpdate();
            var protoSeleccionado = _lstPrototipos.SelectedItem;
            _lstPrototipos.Items.Clear();
            foreach (var p in _laboratorio.Prototipos) _lstPrototipos.Items.Add(p);
            _lstPrototipos.SelectedItem = protoSeleccionado ?? _laboratorio.Prototipos.First();
            _lstPrototipos.EndUpdate();

            _lstReserva.BeginUpdate();
            var reservaSeleccionada = seleccionar ?? _lstReserva.SelectedItem;
            _lstReserva.Items.Clear();
            foreach (var u in _estacion.Reserva) _lstReserva.Items.Add(u);
            if (reservaSeleccionada != null && _lstReserva.Items.Contains(reservaSeleccionada))
                _lstReserva.SelectedItem = reservaSeleccionada;
            else if (_lstReserva.Items.Count > 0)
                _lstReserva.SelectedIndex = 0;
            _lstReserva.EndUpdate();

            ActualizarDetalleReserva();
            ActualizarBiblioteca();
            ActualizarComandos();
            ActualizarControl();

            if (_enemigoSeleccionado != null && !_enemigoSeleccionado.EstaViva)
            {
                _enemigoSeleccionado = null;
                _tablero.Seleccionada = null;
            }
            ActualizarInfoIA();

            string fase = _partida.Fase == FasePartida.Preparacion ? "Preparación"
                        : _partida.Fase == FasePartida.Oleada ? "¡Combate!"
                        : _partida.Fase.ToString();
            _lblEstado.Text = $"Oleada {_partida.NumeroOleada}/{_partida.TotalOleadas} · {fase}      " +
                              $"Créditos: {_estacion.Creditos}      Defensores: {_partida.Grilla.Defensores().Count}      " +
                              $"Enemigos: {_partida.Grilla.Enemigos().Count}      En reserva: {_estacion.Reserva.Count}      " +
                              $"Bonus por oleada: +{_partida.BonusRecursosPorOleada}";

            if (_partida.Fase == FasePartida.Preparacion)
            {
                _btnOleada.Enabled = true;
                _btnOleada.Text = $"Iniciar oleada {_partida.NumeroOleada}/{_partida.TotalOleadas}";
            }
            else
            {
                // Con la estación apagada el botón sigue activo para volver a ofrecer una partida nueva.
                _btnOleada.Enabled = _estacionApagada;
                _btnOleada.Text = _partida.Fase == FasePartida.Oleada ? "Oleada en curso..." : "Partida terminada";
            }

            _tablero.Invalidate();
        }

        private void ActualizarDetalleReserva()
        {
            var unidad = _lstReserva.SelectedItem as Unidad;
            _lblDetalleReserva.Text = unidad == null
                ? "Reserva vacía."
                : unidad.Descripcion() + "\nHacé clic en una celda azul para desplegarla.";
            _tablero.ModoDespliegue = unidad != null && _partida.Fase == FasePartida.Preparacion;
        }

        private void AgregarAlHistorial(EntradaTurno entrada)
        {
            _txtHistorial.AppendText(entrada + Environment.NewLine);
            _lineasHistorial++;

            // Para que el cuadro no crezca sin límite, se borran las líneas más viejas.
            if (_lineasHistorial > 800)
            {
                _txtHistorial.Lines = _txtHistorial.Lines.Skip(400).ToArray();
                _lineasHistorial = _txtHistorial.Lines.Length;
            }
        }

        private void Registrar(string mensaje)
        {
            _txtRegistro.AppendText(mensaje + Environment.NewLine);
        }
    }
}

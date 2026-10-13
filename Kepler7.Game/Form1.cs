using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Builder;
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

        public Form1()
        {
            InitializeComponent();
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
            MinimumSize = new Size(1200, 780);
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

            // Con Dock, el último control agregado se acomoda primero: se agregan de adentro hacia afuera.
            Controls.Add(_tablero);
            Controls.Add(panelDerecho);
            Controls.Add(panelInferior);
            Controls.Add(barraCombate);
            Controls.Add(_lblEstado);

            _cmbRecetas.SelectedIndex = 0;
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
            var grpAstillero = CrearGrupo("Astillero — Nave de apoyo  [Builder]", 146);
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
            var grpReserva = CrearGrupo("Reserva — listas para desplegar", 222);
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

        private void BtnOleada_Click(object? sender, EventArgs e)
        {
            if (!_partida.IniciarOleada()) return;
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
            if (!_estacion.Gastar(receta.Costo))
            {
                Registrar($"Astillero: no alcanzan los créditos para una nave {receta.NombreReceta} ({receta.Costo} cr).");
                return;
            }

            // El director arma la nave paso a paso; la receta elegida decide cada pieza.
            NaveApoyo nave = _astillero.Ensamblar(receta);
            _estacion.AgregarAReserva(nave);

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
            if (!_estacion.Gastar(prototipo.Costo))
            {
                Registrar($"Laboratorio: no alcanzan los créditos para clonar un {prototipo.Rol} ({prototipo.Costo} cr).");
                return;
            }

            Tripulante clon = _laboratorio.Clonar(prototipo.Rol);
            _estacion.AgregarAReserva(clon);

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

        #region Tablero

        private void Tablero_CeldaClick(object? sender, CeldaEventArgs e)
        {
            string subsistema = Estacion.Subsistemas[e.Carril];
            var ocupante = _partida.Grilla.ObtenerUnidad(e.Carril, e.Columna);

            if (ocupante is Enemigo enemigo)
            {
                _enemigoSeleccionado = enemigo;
                _tablero.Seleccionada = enemigo;
                Registrar($"[{subsistema}] {enemigo.NombreCompleto}: {enemigo.Descripcion()}");
                ActualizarInfoIA();
                _tablero.Invalidate();
                return;
            }
            if (ocupante != null)
            {
                Registrar($"[{subsistema}] {ocupante.NombreCompleto}: {ocupante.Descripcion()}");
                return;
            }

            var unidad = _lstReserva.SelectedItem as Unidad;
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

            _partida.Desplegar(unidad, e.Carril, e.Columna);
            Registrar($"Despliegue: {unidad.NombreCompleto} defiende {subsistema} (columna {e.Columna + 1}).");
            Refrescar();
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
                _btnOleada.Enabled = false;
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

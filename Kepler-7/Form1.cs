using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Kepler_7.Modelo;
using Kepler_7.Patrones.Builder;
using Kepler_7.Patrones.Prototype;
using Kepler_7.UI;

namespace Kepler_7
{
    public partial class Form1 : Form
    {
        private readonly Estacion _estacion = new Estacion(600);

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

        public Form1()
        {
            InitializeComponent();
            ConstruirInterfaz();
            Refrescar();
            Registrar("Estación en línea. Ensamblá naves en el astillero o cloná tripulantes en el laboratorio,");
            Registrar("después seleccioná una unidad de la reserva y hacé clic en una celda azul de un carril.");
        }

        #region Construcción de la interfaz

        private void ConstruirInterfaz()
        {
            BackColor = Paleta.Fondo;
            ForeColor = Paleta.Texto;
            Font = new Font("Segoe UI", 9f);
            MinimumSize = new Size(1100, 700);
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

            _tablero = new TableroControl { Dock = DockStyle.Fill, Estacion = _estacion };
            _tablero.CeldaClick += Tablero_CeldaClick;

            var panelDerecho = new Panel { Dock = DockStyle.Right, Width = 390, Padding = new Padding(8), BackColor = Paleta.Panel };

            // --- Builder ---
            var grpAstillero = CrearGrupo("Astillero — Nave de apoyo  [Builder]", 150);
            _cmbRecetas = CrearCombo(new Point(12, 26), 350);
            _cmbRecetas.Items.AddRange(_recetas.Cast<object>().ToArray());
            _cmbRecetas.SelectedIndexChanged += (s, e) => ActualizarDescripcionReceta();
            _lblReceta = new Label { Location = new Point(12, 56), Size = new Size(350, 44), ForeColor = Paleta.TextoSuave };
            var btnEnsamblar = CrearBoton("Ensamblar nave", new Point(12, 104), 350, BtnEnsamblar_Click);
            grpAstillero.Controls.AddRange(new Control[] { _cmbRecetas, _lblReceta, btnEnsamblar });

            // --- Prototype ---
            var grpLaboratorio = CrearGrupo("Laboratorio de clonación  [Prototype]", 196);
            _lstPrototipos = CrearLista(new Point(12, 26), new Size(350, 116));
            _lstPrototipos.Format += (s, e) => e.Value = ResumenPrototipo((Tripulante)e.ListItem!);
            var btnClonar = CrearBoton("Clonar prototipo seleccionado", new Point(12, 150), 350, BtnClonar_Click);
            grpLaboratorio.Controls.AddRange(new Control[] { _lstPrototipos, btnClonar });

            // --- Reserva ---
            var grpReserva = CrearGrupo("Reserva — listas para desplegar", 226);
            _lstReserva = CrearLista(new Point(12, 26), new Size(350, 100));
            _lstReserva.SelectedIndexChanged += (s, e) => ActualizarDetalleReserva();
            _lblDetalleReserva = new Label { Location = new Point(12, 130), Size = new Size(350, 48), ForeColor = Paleta.TextoSuave };
            _cmbEquipo = CrearCombo(new Point(12, 184), 230);
            _cmbEquipo.Items.AddRange(_catalogoEquipo.Cast<object>().ToArray());
            _cmbEquipo.SelectedIndex = 0;
            var btnEquipar = CrearBoton("Equipar clon", new Point(250, 182), 112, BtnEquipar_Click);
            grpReserva.Controls.AddRange(new Control[] { _lstReserva, _lblDetalleReserva, _cmbEquipo, btnEquipar });

            // --- Registro ---
            var grpRegistro = new GroupBox { Text = "Registro", Dock = DockStyle.Fill, ForeColor = Paleta.Texto };
            _txtRegistro = new TextBox
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
            grpRegistro.Controls.Add(_txtRegistro);

            // Con Dock, el último control agregado se acomoda primero: se agregan de abajo hacia arriba.
            panelDerecho.Controls.Add(grpRegistro);
            panelDerecho.Controls.Add(grpReserva);
            panelDerecho.Controls.Add(grpLaboratorio);
            panelDerecho.Controls.Add(grpAstillero);

            Controls.Add(_tablero);
            Controls.Add(panelDerecho);
            Controls.Add(_lblEstado);

            _cmbRecetas.SelectedIndex = 0;
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

        #region Despliegue en el tablero

        private void Tablero_CeldaClick(object? sender, CeldaEventArgs e)
        {
            string subsistema = Estacion.Subsistemas[e.Carril];
            var ocupante = _estacion.ObtenerUnidad(e.Carril, e.Columna);
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
            if (!_estacion.EsZonaDefensa(e.Columna))
            {
                Registrar("Solo se puede desplegar en la zona de defensa (celdas azules).");
                return;
            }

            _estacion.Desplegar(unidad, e.Carril, e.Columna);
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

            _lblEstado.Text = $"Créditos: {_estacion.Creditos}      Unidades desplegadas: {_estacion.UnidadesDesplegadas().Count()}" +
                              $"      En reserva: {_estacion.Reserva.Count}      Bonus por oleada (naves de carga): +{_estacion.BonusRecursosPorOleada}";
            _tablero.Invalidate();
        }

        private void ActualizarDetalleReserva()
        {
            var unidad = _lstReserva.SelectedItem as Unidad;
            _lblDetalleReserva.Text = unidad == null
                ? "Reserva vacía."
                : unidad.Descripcion() + "\nHacé clic en una celda azul para desplegarla.";
            _tablero.ModoDespliegue = unidad != null;
        }

        private void Registrar(string mensaje)
        {
            _txtRegistro.AppendText(mensaje + Environment.NewLine);
        }
    }
}

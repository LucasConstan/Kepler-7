namespace Kepler7.Game
{
    partial class FormPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuPrincipal = new MenuStrip();
            menuAstillero = new ToolStripMenuItem();
            menuLaboratorio = new ToolStripMenuItem();
            menuInventario = new ToolStripMenuItem();
            menuTripulacion = new ToolStripMenuItem();
            menuControlCentral = new ToolStripMenuItem();
            menuHub = new ToolStripMenuItem();
            menuReiniciar = new ToolStripMenuItem();
            lblEstado = new Label();
            grpOleada = new GroupBox();
            btnIniciarOleada = new Button();
            lblVelocidad = new Label();
            cmbVelocidad = new ComboBox();
            grpEstrategia = new GroupBox();
            cmbIA = new ComboBox();
            btnIAUno = new Button();
            btnIATodos = new Button();
            lblInfoIA = new Label();
            lblElegido = new Label();
            lblLeyenda = new Label();
            tablaTablero = new TableLayoutPanel();
            lblCarril0 = new Label();
            pbNucleo0 = new ProgressBar();
            btnCelda_0_0 = new Button();
            btnCelda_0_1 = new Button();
            btnCelda_0_2 = new Button();
            btnCelda_0_3 = new Button();
            btnCelda_0_4 = new Button();
            btnCelda_0_5 = new Button();
            btnCelda_0_6 = new Button();
            btnCelda_0_7 = new Button();
            btnCelda_0_8 = new Button();
            lblCarril1 = new Label();
            pbNucleo1 = new ProgressBar();
            btnCelda_1_0 = new Button();
            btnCelda_1_1 = new Button();
            btnCelda_1_2 = new Button();
            btnCelda_1_3 = new Button();
            btnCelda_1_4 = new Button();
            btnCelda_1_5 = new Button();
            btnCelda_1_6 = new Button();
            btnCelda_1_7 = new Button();
            btnCelda_1_8 = new Button();
            lblCarril2 = new Label();
            pbNucleo2 = new ProgressBar();
            btnCelda_2_0 = new Button();
            btnCelda_2_1 = new Button();
            btnCelda_2_2 = new Button();
            btnCelda_2_3 = new Button();
            btnCelda_2_4 = new Button();
            btnCelda_2_5 = new Button();
            btnCelda_2_6 = new Button();
            btnCelda_2_7 = new Button();
            btnCelda_2_8 = new Button();
            lblCarril3 = new Label();
            pbNucleo3 = new ProgressBar();
            btnCelda_3_0 = new Button();
            btnCelda_3_1 = new Button();
            btnCelda_3_2 = new Button();
            btnCelda_3_3 = new Button();
            btnCelda_3_4 = new Button();
            btnCelda_3_5 = new Button();
            btnCelda_3_6 = new Button();
            btnCelda_3_7 = new Button();
            btnCelda_3_8 = new Button();
            lblCarril4 = new Label();
            pbNucleo4 = new ProgressBar();
            btnCelda_4_0 = new Button();
            btnCelda_4_1 = new Button();
            btnCelda_4_2 = new Button();
            btnCelda_4_3 = new Button();
            btnCelda_4_4 = new Button();
            btnCelda_4_5 = new Button();
            btnCelda_4_6 = new Button();
            btnCelda_4_7 = new Button();
            btnCelda_4_8 = new Button();
            lblCarril5 = new Label();
            pbNucleo5 = new ProgressBar();
            btnCelda_5_0 = new Button();
            btnCelda_5_1 = new Button();
            btnCelda_5_2 = new Button();
            btnCelda_5_3 = new Button();
            btnCelda_5_4 = new Button();
            btnCelda_5_5 = new Button();
            btnCelda_5_6 = new Button();
            btnCelda_5_7 = new Button();
            btnCelda_5_8 = new Button();
            grpComandante = new GroupBox();
            lblEstadoHeroe = new Label();
            lblVidaHeroe = new Label();
            pbVidaHeroe = new ProgressBar();
            lblDanioHeroe = new Label();
            lblCadena = new Label();
            lstCadena = new ListBox();
            btnSubirDefensa = new Button();
            btnBajarDefensa = new Button();
            grpRecorrido = new GroupBox();
            lstRecorrido = new ListBox();
            grpReserva = new GroupBox();
            lstReserva = new ListBox();
            lblDetalleReserva = new Label();
            grpComandos = new GroupBox();
            btnDeshacer = new Button();
            btnRehacer = new Button();
            lstComandos = new ListBox();
            lblReparar = new Label();
            cmbCarrilReparar = new ComboBox();
            btnReparar = new Button();
            btnDisparo = new Button();
            grpHistorial = new GroupBox();
            txtHistorial = new TextBox();
            grpRegistro = new GroupBox();
            txtRegistro = new TextBox();
            timerTick = new System.Windows.Forms.Timer(components);
            menuPrincipal.SuspendLayout();
            grpOleada.SuspendLayout();
            grpEstrategia.SuspendLayout();
            tablaTablero.SuspendLayout();
            grpComandante.SuspendLayout();
            grpRecorrido.SuspendLayout();
            grpReserva.SuspendLayout();
            grpComandos.SuspendLayout();
            grpHistorial.SuspendLayout();
            grpRegistro.SuspendLayout();
            SuspendLayout();
            //
            // menuPrincipal
            //
            menuPrincipal.Items.AddRange(new ToolStripItem[] { menuAstillero, menuLaboratorio, menuInventario, menuTripulacion, menuControlCentral, menuHub, menuReiniciar });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Size = new Size(1290, 24);
            menuPrincipal.TabIndex = 0;
            menuPrincipal.Text = "menuStrip1";
            //
            // menuAstillero
            //
            menuAstillero.Name = "menuAstillero";
            menuAstillero.Size = new Size(122, 20);
            menuAstillero.Text = "Astillero [Builder]";
            menuAstillero.Click += menuAstillero_Click;
            //
            // menuLaboratorio
            //
            menuLaboratorio.Name = "menuLaboratorio";
            menuLaboratorio.Size = new Size(150, 20);
            menuLaboratorio.Text = "Laboratorio [Prototype]";
            menuLaboratorio.Click += menuLaboratorio_Click;
            //
            // menuInventario
            //
            menuInventario.Name = "menuInventario";
            menuInventario.Size = new Size(130, 20);
            menuInventario.Text = "Inventario [Visitor]";
            menuInventario.Click += menuInventario_Click;
            //
            // menuTripulacion
            //
            menuTripulacion.Name = "menuTripulacion";
            menuTripulacion.Size = new Size(145, 20);
            menuTripulacion.Text = "Tripulación [Flyweight]";
            menuTripulacion.Click += menuTripulacion_Click;
            //
            // menuControlCentral
            //
            menuControlCentral.Name = "menuControlCentral";
            menuControlCentral.Size = new Size(150, 20);
            menuControlCentral.Text = "Control central [Facade]";
            menuControlCentral.Click += menuControlCentral_Click;
            //
            // menuHub
            //
            menuHub.Name = "menuHub";
            menuHub.Size = new Size(170, 20);
            menuHub.Text = "Hub y progreso [Proxy, Bridge]";
            menuHub.Click += menuHub_Click;
            //
            // menuReiniciar
            //
            menuReiniciar.Name = "menuReiniciar";
            menuReiniciar.Size = new Size(112, 20);
            menuReiniciar.Text = "Reiniciar partida";
            menuReiniciar.Click += menuReiniciar_Click;
            //
            // lblEstado
            //
            lblEstado.BorderStyle = BorderStyle.FixedSingle;
            lblEstado.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblEstado.Location = new Point(12, 30);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(1266, 23);
            lblEstado.TabIndex = 1;
            lblEstado.Text = "Estado de la partida";
            lblEstado.TextAlign = ContentAlignment.MiddleLeft;
            //
            // grpOleada
            //
            grpOleada.Controls.Add(btnIniciarOleada);
            grpOleada.Controls.Add(lblVelocidad);
            grpOleada.Controls.Add(cmbVelocidad);
            grpOleada.Location = new Point(12, 58);
            grpOleada.Name = "grpOleada";
            grpOleada.Size = new Size(330, 80);
            grpOleada.TabIndex = 2;
            grpOleada.TabStop = false;
            grpOleada.Text = "Oleada";
            //
            // btnIniciarOleada
            //
            btnIniciarOleada.Location = new Point(10, 22);
            btnIniciarOleada.Name = "btnIniciarOleada";
            btnIniciarOleada.Size = new Size(160, 30);
            btnIniciarOleada.TabIndex = 0;
            btnIniciarOleada.Text = "Iniciar oleada";
            btnIniciarOleada.UseVisualStyleBackColor = true;
            btnIniciarOleada.Click += btnIniciarOleada_Click;
            //
            // lblVelocidad
            //
            lblVelocidad.AutoSize = true;
            lblVelocidad.Location = new Point(180, 29);
            lblVelocidad.Name = "lblVelocidad";
            lblVelocidad.Size = new Size(60, 15);
            lblVelocidad.TabIndex = 1;
            lblVelocidad.Text = "Velocidad:";
            //
            // cmbVelocidad
            //
            cmbVelocidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVelocidad.FormattingEnabled = true;
            cmbVelocidad.Items.AddRange(new object[] { "Lenta", "Normal", "Rápida", "Pausa" });
            cmbVelocidad.Location = new Point(245, 26);
            cmbVelocidad.Name = "cmbVelocidad";
            cmbVelocidad.Size = new Size(75, 23);
            cmbVelocidad.TabIndex = 2;
            cmbVelocidad.SelectedIndexChanged += cmbVelocidad_SelectedIndexChanged;
            //
            // grpEstrategia
            //
            grpEstrategia.Controls.Add(cmbIA);
            grpEstrategia.Controls.Add(btnIAUno);
            grpEstrategia.Controls.Add(btnIATodos);
            grpEstrategia.Controls.Add(lblInfoIA);
            grpEstrategia.Controls.Add(lblElegido);
            grpEstrategia.Location = new Point(350, 58);
            grpEstrategia.Name = "grpEstrategia";
            grpEstrategia.Size = new Size(612, 80);
            grpEstrategia.TabIndex = 3;
            grpEstrategia.TabStop = false;
            grpEstrategia.Text = "IA enemiga  [Strategy]";
            //
            // cmbIA
            //
            cmbIA.DisplayMember = "Nombre";
            cmbIA.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIA.FormattingEnabled = true;
            cmbIA.Location = new Point(10, 24);
            cmbIA.Name = "cmbIA";
            cmbIA.Size = new Size(120, 23);
            cmbIA.TabIndex = 0;
            cmbIA.SelectedIndexChanged += cmbIA_SelectedIndexChanged;
            //
            // btnIAUno
            //
            btnIAUno.Location = new Point(136, 22);
            btnIAUno.Name = "btnIAUno";
            btnIAUno.Size = new Size(140, 28);
            btnIAUno.TabIndex = 1;
            btnIAUno.Text = "Al enemigo elegido";
            btnIAUno.UseVisualStyleBackColor = true;
            btnIAUno.Click += btnIAUno_Click;
            //
            // btnIATodos
            //
            btnIATodos.Location = new Point(282, 22);
            btnIATodos.Name = "btnIATodos";
            btnIATodos.Size = new Size(80, 28);
            btnIATodos.TabIndex = 2;
            btnIATodos.Text = "A todos";
            btnIATodos.UseVisualStyleBackColor = true;
            btnIATodos.Click += btnIATodos_Click;
            //
            // lblInfoIA
            //
            lblInfoIA.Location = new Point(10, 54);
            lblInfoIA.Name = "lblInfoIA";
            lblInfoIA.Size = new Size(592, 20);
            lblInfoIA.TabIndex = 3;
            lblInfoIA.Text = "Descripción de la IA elegida";
            // 
            // lblElegido
            // 
            lblElegido.Location = new Point(370, 20);
            lblElegido.Name = "lblElegido";
            lblElegido.Size = new Size(234, 32);
            lblElegido.TabIndex = 4;
            lblElegido.Text = "Clic en un enemigo del tablero para elegirlo.";
            //
            // lblLeyenda
            //
            lblLeyenda.AutoSize = true;
            lblLeyenda.Location = new Point(12, 144);
            lblLeyenda.Name = "lblLeyenda";
            lblLeyenda.Size = new Size(800, 15);
            lblLeyenda.TabIndex = 4;
            lblLeyenda.Text = "◄ Núcleo   |   Columnas 1 a 6: zona de defensa (azul)   |   Columnas 7 a 9: aproximación enemiga (rosa)   |   Clic en una celda: desplegar, mover o elegir enemigo";
            //
            // tablaTablero
            //
            tablaTablero.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tablaTablero.ColumnCount = 11;
            tablaTablero.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tablaTablero.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            tablaTablero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tablaTablero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tablaTablero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tablaTablero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tablaTablero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tablaTablero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tablaTablero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tablaTablero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tablaTablero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.1111107F));
            tablaTablero.Controls.Add(lblCarril0, 0, 0);
            tablaTablero.Controls.Add(pbNucleo0, 1, 0);
            tablaTablero.Controls.Add(btnCelda_0_0, 2, 0);
            tablaTablero.Controls.Add(btnCelda_0_1, 3, 0);
            tablaTablero.Controls.Add(btnCelda_0_2, 4, 0);
            tablaTablero.Controls.Add(btnCelda_0_3, 5, 0);
            tablaTablero.Controls.Add(btnCelda_0_4, 6, 0);
            tablaTablero.Controls.Add(btnCelda_0_5, 7, 0);
            tablaTablero.Controls.Add(btnCelda_0_6, 8, 0);
            tablaTablero.Controls.Add(btnCelda_0_7, 9, 0);
            tablaTablero.Controls.Add(btnCelda_0_8, 10, 0);
            tablaTablero.Controls.Add(lblCarril1, 0, 1);
            tablaTablero.Controls.Add(pbNucleo1, 1, 1);
            tablaTablero.Controls.Add(btnCelda_1_0, 2, 1);
            tablaTablero.Controls.Add(btnCelda_1_1, 3, 1);
            tablaTablero.Controls.Add(btnCelda_1_2, 4, 1);
            tablaTablero.Controls.Add(btnCelda_1_3, 5, 1);
            tablaTablero.Controls.Add(btnCelda_1_4, 6, 1);
            tablaTablero.Controls.Add(btnCelda_1_5, 7, 1);
            tablaTablero.Controls.Add(btnCelda_1_6, 8, 1);
            tablaTablero.Controls.Add(btnCelda_1_7, 9, 1);
            tablaTablero.Controls.Add(btnCelda_1_8, 10, 1);
            tablaTablero.Controls.Add(lblCarril2, 0, 2);
            tablaTablero.Controls.Add(pbNucleo2, 1, 2);
            tablaTablero.Controls.Add(btnCelda_2_0, 2, 2);
            tablaTablero.Controls.Add(btnCelda_2_1, 3, 2);
            tablaTablero.Controls.Add(btnCelda_2_2, 4, 2);
            tablaTablero.Controls.Add(btnCelda_2_3, 5, 2);
            tablaTablero.Controls.Add(btnCelda_2_4, 6, 2);
            tablaTablero.Controls.Add(btnCelda_2_5, 7, 2);
            tablaTablero.Controls.Add(btnCelda_2_6, 8, 2);
            tablaTablero.Controls.Add(btnCelda_2_7, 9, 2);
            tablaTablero.Controls.Add(btnCelda_2_8, 10, 2);
            tablaTablero.Controls.Add(lblCarril3, 0, 3);
            tablaTablero.Controls.Add(pbNucleo3, 1, 3);
            tablaTablero.Controls.Add(btnCelda_3_0, 2, 3);
            tablaTablero.Controls.Add(btnCelda_3_1, 3, 3);
            tablaTablero.Controls.Add(btnCelda_3_2, 4, 3);
            tablaTablero.Controls.Add(btnCelda_3_3, 5, 3);
            tablaTablero.Controls.Add(btnCelda_3_4, 6, 3);
            tablaTablero.Controls.Add(btnCelda_3_5, 7, 3);
            tablaTablero.Controls.Add(btnCelda_3_6, 8, 3);
            tablaTablero.Controls.Add(btnCelda_3_7, 9, 3);
            tablaTablero.Controls.Add(btnCelda_3_8, 10, 3);
            tablaTablero.Controls.Add(lblCarril4, 0, 4);
            tablaTablero.Controls.Add(pbNucleo4, 1, 4);
            tablaTablero.Controls.Add(btnCelda_4_0, 2, 4);
            tablaTablero.Controls.Add(btnCelda_4_1, 3, 4);
            tablaTablero.Controls.Add(btnCelda_4_2, 4, 4);
            tablaTablero.Controls.Add(btnCelda_4_3, 5, 4);
            tablaTablero.Controls.Add(btnCelda_4_4, 6, 4);
            tablaTablero.Controls.Add(btnCelda_4_5, 7, 4);
            tablaTablero.Controls.Add(btnCelda_4_6, 8, 4);
            tablaTablero.Controls.Add(btnCelda_4_7, 9, 4);
            tablaTablero.Controls.Add(btnCelda_4_8, 10, 4);
            tablaTablero.Controls.Add(lblCarril5, 0, 5);
            tablaTablero.Controls.Add(pbNucleo5, 1, 5);
            tablaTablero.Controls.Add(btnCelda_5_0, 2, 5);
            tablaTablero.Controls.Add(btnCelda_5_1, 3, 5);
            tablaTablero.Controls.Add(btnCelda_5_2, 4, 5);
            tablaTablero.Controls.Add(btnCelda_5_3, 5, 5);
            tablaTablero.Controls.Add(btnCelda_5_4, 6, 5);
            tablaTablero.Controls.Add(btnCelda_5_5, 7, 5);
            tablaTablero.Controls.Add(btnCelda_5_6, 8, 5);
            tablaTablero.Controls.Add(btnCelda_5_7, 9, 5);
            tablaTablero.Controls.Add(btnCelda_5_8, 10, 5);
            tablaTablero.Location = new Point(12, 164);
            tablaTablero.Name = "tablaTablero";
            tablaTablero.RowCount = 6;
            tablaTablero.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tablaTablero.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tablaTablero.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tablaTablero.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tablaTablero.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tablaTablero.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tablaTablero.Size = new Size(950, 360);
            tablaTablero.TabIndex = 5;
            //
            // lblCarril0
            //
            lblCarril0.Dock = DockStyle.Fill;
            lblCarril0.Location = new Point(4, 1);
            lblCarril0.Name = "lblCarril0";
            lblCarril0.Size = new Size(124, 59);
            lblCarril0.TabIndex = 0;
            lblCarril0.Text = "Subsistema";
            lblCarril0.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pbNucleo0
            //
            pbNucleo0.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pbNucleo0.Location = new Point(135, 19);
            pbNucleo0.Name = "pbNucleo0";
            pbNucleo0.Size = new Size(64, 23);
            pbNucleo0.TabIndex = 1;
            pbNucleo0.Value = 100;
            //
            // btnCelda_0_0
            //
            btnCelda_0_0.Dock = DockStyle.Fill;
            btnCelda_0_0.Location = new Point(205, 2);
            btnCelda_0_0.Margin = new Padding(2);
            btnCelda_0_0.Name = "btnCelda_0_0";
            btnCelda_0_0.Size = new Size(79, 56);
            btnCelda_0_0.TabIndex = 2;
            btnCelda_0_0.UseVisualStyleBackColor = true;
            btnCelda_0_0.Click += btnCelda_Click;
            //
            // btnCelda_0_1
            //
            btnCelda_0_1.Dock = DockStyle.Fill;
            btnCelda_0_1.Location = new Point(288, 2);
            btnCelda_0_1.Margin = new Padding(2);
            btnCelda_0_1.Name = "btnCelda_0_1";
            btnCelda_0_1.Size = new Size(79, 56);
            btnCelda_0_1.TabIndex = 3;
            btnCelda_0_1.UseVisualStyleBackColor = true;
            btnCelda_0_1.Click += btnCelda_Click;
            //
            // btnCelda_0_2
            //
            btnCelda_0_2.Dock = DockStyle.Fill;
            btnCelda_0_2.Location = new Point(371, 2);
            btnCelda_0_2.Margin = new Padding(2);
            btnCelda_0_2.Name = "btnCelda_0_2";
            btnCelda_0_2.Size = new Size(79, 56);
            btnCelda_0_2.TabIndex = 4;
            btnCelda_0_2.UseVisualStyleBackColor = true;
            btnCelda_0_2.Click += btnCelda_Click;
            //
            // btnCelda_0_3
            //
            btnCelda_0_3.Dock = DockStyle.Fill;
            btnCelda_0_3.Location = new Point(454, 2);
            btnCelda_0_3.Margin = new Padding(2);
            btnCelda_0_3.Name = "btnCelda_0_3";
            btnCelda_0_3.Size = new Size(79, 56);
            btnCelda_0_3.TabIndex = 5;
            btnCelda_0_3.UseVisualStyleBackColor = true;
            btnCelda_0_3.Click += btnCelda_Click;
            //
            // btnCelda_0_4
            //
            btnCelda_0_4.Dock = DockStyle.Fill;
            btnCelda_0_4.Location = new Point(537, 2);
            btnCelda_0_4.Margin = new Padding(2);
            btnCelda_0_4.Name = "btnCelda_0_4";
            btnCelda_0_4.Size = new Size(79, 56);
            btnCelda_0_4.TabIndex = 6;
            btnCelda_0_4.UseVisualStyleBackColor = true;
            btnCelda_0_4.Click += btnCelda_Click;
            //
            // btnCelda_0_5
            //
            btnCelda_0_5.Dock = DockStyle.Fill;
            btnCelda_0_5.Location = new Point(620, 2);
            btnCelda_0_5.Margin = new Padding(2);
            btnCelda_0_5.Name = "btnCelda_0_5";
            btnCelda_0_5.Size = new Size(79, 56);
            btnCelda_0_5.TabIndex = 7;
            btnCelda_0_5.UseVisualStyleBackColor = true;
            btnCelda_0_5.Click += btnCelda_Click;
            //
            // btnCelda_0_6
            //
            btnCelda_0_6.Dock = DockStyle.Fill;
            btnCelda_0_6.Location = new Point(703, 2);
            btnCelda_0_6.Margin = new Padding(2);
            btnCelda_0_6.Name = "btnCelda_0_6";
            btnCelda_0_6.Size = new Size(79, 56);
            btnCelda_0_6.TabIndex = 8;
            btnCelda_0_6.UseVisualStyleBackColor = true;
            btnCelda_0_6.Click += btnCelda_Click;
            //
            // btnCelda_0_7
            //
            btnCelda_0_7.Dock = DockStyle.Fill;
            btnCelda_0_7.Location = new Point(786, 2);
            btnCelda_0_7.Margin = new Padding(2);
            btnCelda_0_7.Name = "btnCelda_0_7";
            btnCelda_0_7.Size = new Size(79, 56);
            btnCelda_0_7.TabIndex = 9;
            btnCelda_0_7.UseVisualStyleBackColor = true;
            btnCelda_0_7.Click += btnCelda_Click;
            //
            // btnCelda_0_8
            //
            btnCelda_0_8.Dock = DockStyle.Fill;
            btnCelda_0_8.Location = new Point(869, 2);
            btnCelda_0_8.Margin = new Padding(2);
            btnCelda_0_8.Name = "btnCelda_0_8";
            btnCelda_0_8.Size = new Size(79, 56);
            btnCelda_0_8.TabIndex = 10;
            btnCelda_0_8.UseVisualStyleBackColor = true;
            btnCelda_0_8.Click += btnCelda_Click;
            //
            // lblCarril1
            //
            lblCarril1.Dock = DockStyle.Fill;
            lblCarril1.Location = new Point(4, 61);
            lblCarril1.Name = "lblCarril1";
            lblCarril1.Size = new Size(124, 59);
            lblCarril1.TabIndex = 11;
            lblCarril1.Text = "Subsistema";
            lblCarril1.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pbNucleo1
            //
            pbNucleo1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pbNucleo1.Location = new Point(135, 79);
            pbNucleo1.Name = "pbNucleo1";
            pbNucleo1.Size = new Size(64, 23);
            pbNucleo1.TabIndex = 12;
            pbNucleo1.Value = 100;
            //
            // btnCelda_1_0
            //
            btnCelda_1_0.Dock = DockStyle.Fill;
            btnCelda_1_0.Location = new Point(205, 62);
            btnCelda_1_0.Margin = new Padding(2);
            btnCelda_1_0.Name = "btnCelda_1_0";
            btnCelda_1_0.Size = new Size(79, 56);
            btnCelda_1_0.TabIndex = 13;
            btnCelda_1_0.UseVisualStyleBackColor = true;
            btnCelda_1_0.Click += btnCelda_Click;
            //
            // btnCelda_1_1
            //
            btnCelda_1_1.Dock = DockStyle.Fill;
            btnCelda_1_1.Location = new Point(288, 62);
            btnCelda_1_1.Margin = new Padding(2);
            btnCelda_1_1.Name = "btnCelda_1_1";
            btnCelda_1_1.Size = new Size(79, 56);
            btnCelda_1_1.TabIndex = 14;
            btnCelda_1_1.UseVisualStyleBackColor = true;
            btnCelda_1_1.Click += btnCelda_Click;
            //
            // btnCelda_1_2
            //
            btnCelda_1_2.Dock = DockStyle.Fill;
            btnCelda_1_2.Location = new Point(371, 62);
            btnCelda_1_2.Margin = new Padding(2);
            btnCelda_1_2.Name = "btnCelda_1_2";
            btnCelda_1_2.Size = new Size(79, 56);
            btnCelda_1_2.TabIndex = 15;
            btnCelda_1_2.UseVisualStyleBackColor = true;
            btnCelda_1_2.Click += btnCelda_Click;
            //
            // btnCelda_1_3
            //
            btnCelda_1_3.Dock = DockStyle.Fill;
            btnCelda_1_3.Location = new Point(454, 62);
            btnCelda_1_3.Margin = new Padding(2);
            btnCelda_1_3.Name = "btnCelda_1_3";
            btnCelda_1_3.Size = new Size(79, 56);
            btnCelda_1_3.TabIndex = 16;
            btnCelda_1_3.UseVisualStyleBackColor = true;
            btnCelda_1_3.Click += btnCelda_Click;
            //
            // btnCelda_1_4
            //
            btnCelda_1_4.Dock = DockStyle.Fill;
            btnCelda_1_4.Location = new Point(537, 62);
            btnCelda_1_4.Margin = new Padding(2);
            btnCelda_1_4.Name = "btnCelda_1_4";
            btnCelda_1_4.Size = new Size(79, 56);
            btnCelda_1_4.TabIndex = 17;
            btnCelda_1_4.UseVisualStyleBackColor = true;
            btnCelda_1_4.Click += btnCelda_Click;
            //
            // btnCelda_1_5
            //
            btnCelda_1_5.Dock = DockStyle.Fill;
            btnCelda_1_5.Location = new Point(620, 62);
            btnCelda_1_5.Margin = new Padding(2);
            btnCelda_1_5.Name = "btnCelda_1_5";
            btnCelda_1_5.Size = new Size(79, 56);
            btnCelda_1_5.TabIndex = 18;
            btnCelda_1_5.UseVisualStyleBackColor = true;
            btnCelda_1_5.Click += btnCelda_Click;
            //
            // btnCelda_1_6
            //
            btnCelda_1_6.Dock = DockStyle.Fill;
            btnCelda_1_6.Location = new Point(703, 62);
            btnCelda_1_6.Margin = new Padding(2);
            btnCelda_1_6.Name = "btnCelda_1_6";
            btnCelda_1_6.Size = new Size(79, 56);
            btnCelda_1_6.TabIndex = 19;
            btnCelda_1_6.UseVisualStyleBackColor = true;
            btnCelda_1_6.Click += btnCelda_Click;
            //
            // btnCelda_1_7
            //
            btnCelda_1_7.Dock = DockStyle.Fill;
            btnCelda_1_7.Location = new Point(786, 62);
            btnCelda_1_7.Margin = new Padding(2);
            btnCelda_1_7.Name = "btnCelda_1_7";
            btnCelda_1_7.Size = new Size(79, 56);
            btnCelda_1_7.TabIndex = 20;
            btnCelda_1_7.UseVisualStyleBackColor = true;
            btnCelda_1_7.Click += btnCelda_Click;
            //
            // btnCelda_1_8
            //
            btnCelda_1_8.Dock = DockStyle.Fill;
            btnCelda_1_8.Location = new Point(869, 62);
            btnCelda_1_8.Margin = new Padding(2);
            btnCelda_1_8.Name = "btnCelda_1_8";
            btnCelda_1_8.Size = new Size(79, 56);
            btnCelda_1_8.TabIndex = 21;
            btnCelda_1_8.UseVisualStyleBackColor = true;
            btnCelda_1_8.Click += btnCelda_Click;
            //
            // lblCarril2
            //
            lblCarril2.Dock = DockStyle.Fill;
            lblCarril2.Location = new Point(4, 121);
            lblCarril2.Name = "lblCarril2";
            lblCarril2.Size = new Size(124, 59);
            lblCarril2.TabIndex = 22;
            lblCarril2.Text = "Subsistema";
            lblCarril2.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pbNucleo2
            //
            pbNucleo2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pbNucleo2.Location = new Point(135, 139);
            pbNucleo2.Name = "pbNucleo2";
            pbNucleo2.Size = new Size(64, 23);
            pbNucleo2.TabIndex = 23;
            pbNucleo2.Value = 100;
            //
            // btnCelda_2_0
            //
            btnCelda_2_0.Dock = DockStyle.Fill;
            btnCelda_2_0.Location = new Point(205, 122);
            btnCelda_2_0.Margin = new Padding(2);
            btnCelda_2_0.Name = "btnCelda_2_0";
            btnCelda_2_0.Size = new Size(79, 56);
            btnCelda_2_0.TabIndex = 24;
            btnCelda_2_0.UseVisualStyleBackColor = true;
            btnCelda_2_0.Click += btnCelda_Click;
            //
            // btnCelda_2_1
            //
            btnCelda_2_1.Dock = DockStyle.Fill;
            btnCelda_2_1.Location = new Point(288, 122);
            btnCelda_2_1.Margin = new Padding(2);
            btnCelda_2_1.Name = "btnCelda_2_1";
            btnCelda_2_1.Size = new Size(79, 56);
            btnCelda_2_1.TabIndex = 25;
            btnCelda_2_1.UseVisualStyleBackColor = true;
            btnCelda_2_1.Click += btnCelda_Click;
            //
            // btnCelda_2_2
            //
            btnCelda_2_2.Dock = DockStyle.Fill;
            btnCelda_2_2.Location = new Point(371, 122);
            btnCelda_2_2.Margin = new Padding(2);
            btnCelda_2_2.Name = "btnCelda_2_2";
            btnCelda_2_2.Size = new Size(79, 56);
            btnCelda_2_2.TabIndex = 26;
            btnCelda_2_2.UseVisualStyleBackColor = true;
            btnCelda_2_2.Click += btnCelda_Click;
            //
            // btnCelda_2_3
            //
            btnCelda_2_3.Dock = DockStyle.Fill;
            btnCelda_2_3.Location = new Point(454, 122);
            btnCelda_2_3.Margin = new Padding(2);
            btnCelda_2_3.Name = "btnCelda_2_3";
            btnCelda_2_3.Size = new Size(79, 56);
            btnCelda_2_3.TabIndex = 27;
            btnCelda_2_3.UseVisualStyleBackColor = true;
            btnCelda_2_3.Click += btnCelda_Click;
            //
            // btnCelda_2_4
            //
            btnCelda_2_4.Dock = DockStyle.Fill;
            btnCelda_2_4.Location = new Point(537, 122);
            btnCelda_2_4.Margin = new Padding(2);
            btnCelda_2_4.Name = "btnCelda_2_4";
            btnCelda_2_4.Size = new Size(79, 56);
            btnCelda_2_4.TabIndex = 28;
            btnCelda_2_4.UseVisualStyleBackColor = true;
            btnCelda_2_4.Click += btnCelda_Click;
            //
            // btnCelda_2_5
            //
            btnCelda_2_5.Dock = DockStyle.Fill;
            btnCelda_2_5.Location = new Point(620, 122);
            btnCelda_2_5.Margin = new Padding(2);
            btnCelda_2_5.Name = "btnCelda_2_5";
            btnCelda_2_5.Size = new Size(79, 56);
            btnCelda_2_5.TabIndex = 29;
            btnCelda_2_5.UseVisualStyleBackColor = true;
            btnCelda_2_5.Click += btnCelda_Click;
            //
            // btnCelda_2_6
            //
            btnCelda_2_6.Dock = DockStyle.Fill;
            btnCelda_2_6.Location = new Point(703, 122);
            btnCelda_2_6.Margin = new Padding(2);
            btnCelda_2_6.Name = "btnCelda_2_6";
            btnCelda_2_6.Size = new Size(79, 56);
            btnCelda_2_6.TabIndex = 30;
            btnCelda_2_6.UseVisualStyleBackColor = true;
            btnCelda_2_6.Click += btnCelda_Click;
            //
            // btnCelda_2_7
            //
            btnCelda_2_7.Dock = DockStyle.Fill;
            btnCelda_2_7.Location = new Point(786, 122);
            btnCelda_2_7.Margin = new Padding(2);
            btnCelda_2_7.Name = "btnCelda_2_7";
            btnCelda_2_7.Size = new Size(79, 56);
            btnCelda_2_7.TabIndex = 31;
            btnCelda_2_7.UseVisualStyleBackColor = true;
            btnCelda_2_7.Click += btnCelda_Click;
            //
            // btnCelda_2_8
            //
            btnCelda_2_8.Dock = DockStyle.Fill;
            btnCelda_2_8.Location = new Point(869, 122);
            btnCelda_2_8.Margin = new Padding(2);
            btnCelda_2_8.Name = "btnCelda_2_8";
            btnCelda_2_8.Size = new Size(79, 56);
            btnCelda_2_8.TabIndex = 32;
            btnCelda_2_8.UseVisualStyleBackColor = true;
            btnCelda_2_8.Click += btnCelda_Click;
            //
            // lblCarril3
            //
            lblCarril3.Dock = DockStyle.Fill;
            lblCarril3.Location = new Point(4, 181);
            lblCarril3.Name = "lblCarril3";
            lblCarril3.Size = new Size(124, 59);
            lblCarril3.TabIndex = 33;
            lblCarril3.Text = "Subsistema";
            lblCarril3.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pbNucleo3
            //
            pbNucleo3.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pbNucleo3.Location = new Point(135, 199);
            pbNucleo3.Name = "pbNucleo3";
            pbNucleo3.Size = new Size(64, 23);
            pbNucleo3.TabIndex = 34;
            pbNucleo3.Value = 100;
            //
            // btnCelda_3_0
            //
            btnCelda_3_0.Dock = DockStyle.Fill;
            btnCelda_3_0.Location = new Point(205, 182);
            btnCelda_3_0.Margin = new Padding(2);
            btnCelda_3_0.Name = "btnCelda_3_0";
            btnCelda_3_0.Size = new Size(79, 56);
            btnCelda_3_0.TabIndex = 35;
            btnCelda_3_0.UseVisualStyleBackColor = true;
            btnCelda_3_0.Click += btnCelda_Click;
            //
            // btnCelda_3_1
            //
            btnCelda_3_1.Dock = DockStyle.Fill;
            btnCelda_3_1.Location = new Point(288, 182);
            btnCelda_3_1.Margin = new Padding(2);
            btnCelda_3_1.Name = "btnCelda_3_1";
            btnCelda_3_1.Size = new Size(79, 56);
            btnCelda_3_1.TabIndex = 36;
            btnCelda_3_1.UseVisualStyleBackColor = true;
            btnCelda_3_1.Click += btnCelda_Click;
            //
            // btnCelda_3_2
            //
            btnCelda_3_2.Dock = DockStyle.Fill;
            btnCelda_3_2.Location = new Point(371, 182);
            btnCelda_3_2.Margin = new Padding(2);
            btnCelda_3_2.Name = "btnCelda_3_2";
            btnCelda_3_2.Size = new Size(79, 56);
            btnCelda_3_2.TabIndex = 37;
            btnCelda_3_2.UseVisualStyleBackColor = true;
            btnCelda_3_2.Click += btnCelda_Click;
            //
            // btnCelda_3_3
            //
            btnCelda_3_3.Dock = DockStyle.Fill;
            btnCelda_3_3.Location = new Point(454, 182);
            btnCelda_3_3.Margin = new Padding(2);
            btnCelda_3_3.Name = "btnCelda_3_3";
            btnCelda_3_3.Size = new Size(79, 56);
            btnCelda_3_3.TabIndex = 38;
            btnCelda_3_3.UseVisualStyleBackColor = true;
            btnCelda_3_3.Click += btnCelda_Click;
            //
            // btnCelda_3_4
            //
            btnCelda_3_4.Dock = DockStyle.Fill;
            btnCelda_3_4.Location = new Point(537, 182);
            btnCelda_3_4.Margin = new Padding(2);
            btnCelda_3_4.Name = "btnCelda_3_4";
            btnCelda_3_4.Size = new Size(79, 56);
            btnCelda_3_4.TabIndex = 39;
            btnCelda_3_4.UseVisualStyleBackColor = true;
            btnCelda_3_4.Click += btnCelda_Click;
            //
            // btnCelda_3_5
            //
            btnCelda_3_5.Dock = DockStyle.Fill;
            btnCelda_3_5.Location = new Point(620, 182);
            btnCelda_3_5.Margin = new Padding(2);
            btnCelda_3_5.Name = "btnCelda_3_5";
            btnCelda_3_5.Size = new Size(79, 56);
            btnCelda_3_5.TabIndex = 40;
            btnCelda_3_5.UseVisualStyleBackColor = true;
            btnCelda_3_5.Click += btnCelda_Click;
            //
            // btnCelda_3_6
            //
            btnCelda_3_6.Dock = DockStyle.Fill;
            btnCelda_3_6.Location = new Point(703, 182);
            btnCelda_3_6.Margin = new Padding(2);
            btnCelda_3_6.Name = "btnCelda_3_6";
            btnCelda_3_6.Size = new Size(79, 56);
            btnCelda_3_6.TabIndex = 41;
            btnCelda_3_6.UseVisualStyleBackColor = true;
            btnCelda_3_6.Click += btnCelda_Click;
            //
            // btnCelda_3_7
            //
            btnCelda_3_7.Dock = DockStyle.Fill;
            btnCelda_3_7.Location = new Point(786, 182);
            btnCelda_3_7.Margin = new Padding(2);
            btnCelda_3_7.Name = "btnCelda_3_7";
            btnCelda_3_7.Size = new Size(79, 56);
            btnCelda_3_7.TabIndex = 42;
            btnCelda_3_7.UseVisualStyleBackColor = true;
            btnCelda_3_7.Click += btnCelda_Click;
            //
            // btnCelda_3_8
            //
            btnCelda_3_8.Dock = DockStyle.Fill;
            btnCelda_3_8.Location = new Point(869, 182);
            btnCelda_3_8.Margin = new Padding(2);
            btnCelda_3_8.Name = "btnCelda_3_8";
            btnCelda_3_8.Size = new Size(79, 56);
            btnCelda_3_8.TabIndex = 43;
            btnCelda_3_8.UseVisualStyleBackColor = true;
            btnCelda_3_8.Click += btnCelda_Click;
            //
            // lblCarril4
            //
            lblCarril4.Dock = DockStyle.Fill;
            lblCarril4.Location = new Point(4, 241);
            lblCarril4.Name = "lblCarril4";
            lblCarril4.Size = new Size(124, 59);
            lblCarril4.TabIndex = 44;
            lblCarril4.Text = "Subsistema";
            lblCarril4.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pbNucleo4
            //
            pbNucleo4.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pbNucleo4.Location = new Point(135, 259);
            pbNucleo4.Name = "pbNucleo4";
            pbNucleo4.Size = new Size(64, 23);
            pbNucleo4.TabIndex = 45;
            pbNucleo4.Value = 100;
            //
            // btnCelda_4_0
            //
            btnCelda_4_0.Dock = DockStyle.Fill;
            btnCelda_4_0.Location = new Point(205, 242);
            btnCelda_4_0.Margin = new Padding(2);
            btnCelda_4_0.Name = "btnCelda_4_0";
            btnCelda_4_0.Size = new Size(79, 56);
            btnCelda_4_0.TabIndex = 46;
            btnCelda_4_0.UseVisualStyleBackColor = true;
            btnCelda_4_0.Click += btnCelda_Click;
            //
            // btnCelda_4_1
            //
            btnCelda_4_1.Dock = DockStyle.Fill;
            btnCelda_4_1.Location = new Point(288, 242);
            btnCelda_4_1.Margin = new Padding(2);
            btnCelda_4_1.Name = "btnCelda_4_1";
            btnCelda_4_1.Size = new Size(79, 56);
            btnCelda_4_1.TabIndex = 47;
            btnCelda_4_1.UseVisualStyleBackColor = true;
            btnCelda_4_1.Click += btnCelda_Click;
            //
            // btnCelda_4_2
            //
            btnCelda_4_2.Dock = DockStyle.Fill;
            btnCelda_4_2.Location = new Point(371, 242);
            btnCelda_4_2.Margin = new Padding(2);
            btnCelda_4_2.Name = "btnCelda_4_2";
            btnCelda_4_2.Size = new Size(79, 56);
            btnCelda_4_2.TabIndex = 48;
            btnCelda_4_2.UseVisualStyleBackColor = true;
            btnCelda_4_2.Click += btnCelda_Click;
            //
            // btnCelda_4_3
            //
            btnCelda_4_3.Dock = DockStyle.Fill;
            btnCelda_4_3.Location = new Point(454, 242);
            btnCelda_4_3.Margin = new Padding(2);
            btnCelda_4_3.Name = "btnCelda_4_3";
            btnCelda_4_3.Size = new Size(79, 56);
            btnCelda_4_3.TabIndex = 49;
            btnCelda_4_3.UseVisualStyleBackColor = true;
            btnCelda_4_3.Click += btnCelda_Click;
            //
            // btnCelda_4_4
            //
            btnCelda_4_4.Dock = DockStyle.Fill;
            btnCelda_4_4.Location = new Point(537, 242);
            btnCelda_4_4.Margin = new Padding(2);
            btnCelda_4_4.Name = "btnCelda_4_4";
            btnCelda_4_4.Size = new Size(79, 56);
            btnCelda_4_4.TabIndex = 50;
            btnCelda_4_4.UseVisualStyleBackColor = true;
            btnCelda_4_4.Click += btnCelda_Click;
            //
            // btnCelda_4_5
            //
            btnCelda_4_5.Dock = DockStyle.Fill;
            btnCelda_4_5.Location = new Point(620, 242);
            btnCelda_4_5.Margin = new Padding(2);
            btnCelda_4_5.Name = "btnCelda_4_5";
            btnCelda_4_5.Size = new Size(79, 56);
            btnCelda_4_5.TabIndex = 51;
            btnCelda_4_5.UseVisualStyleBackColor = true;
            btnCelda_4_5.Click += btnCelda_Click;
            //
            // btnCelda_4_6
            //
            btnCelda_4_6.Dock = DockStyle.Fill;
            btnCelda_4_6.Location = new Point(703, 242);
            btnCelda_4_6.Margin = new Padding(2);
            btnCelda_4_6.Name = "btnCelda_4_6";
            btnCelda_4_6.Size = new Size(79, 56);
            btnCelda_4_6.TabIndex = 52;
            btnCelda_4_6.UseVisualStyleBackColor = true;
            btnCelda_4_6.Click += btnCelda_Click;
            //
            // btnCelda_4_7
            //
            btnCelda_4_7.Dock = DockStyle.Fill;
            btnCelda_4_7.Location = new Point(786, 242);
            btnCelda_4_7.Margin = new Padding(2);
            btnCelda_4_7.Name = "btnCelda_4_7";
            btnCelda_4_7.Size = new Size(79, 56);
            btnCelda_4_7.TabIndex = 53;
            btnCelda_4_7.UseVisualStyleBackColor = true;
            btnCelda_4_7.Click += btnCelda_Click;
            //
            // btnCelda_4_8
            //
            btnCelda_4_8.Dock = DockStyle.Fill;
            btnCelda_4_8.Location = new Point(869, 242);
            btnCelda_4_8.Margin = new Padding(2);
            btnCelda_4_8.Name = "btnCelda_4_8";
            btnCelda_4_8.Size = new Size(79, 56);
            btnCelda_4_8.TabIndex = 54;
            btnCelda_4_8.UseVisualStyleBackColor = true;
            btnCelda_4_8.Click += btnCelda_Click;
            //
            // lblCarril5
            //
            lblCarril5.Dock = DockStyle.Fill;
            lblCarril5.Location = new Point(4, 301);
            lblCarril5.Name = "lblCarril5";
            lblCarril5.Size = new Size(124, 59);
            lblCarril5.TabIndex = 55;
            lblCarril5.Text = "Subsistema";
            lblCarril5.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pbNucleo5
            //
            pbNucleo5.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pbNucleo5.Location = new Point(135, 319);
            pbNucleo5.Name = "pbNucleo5";
            pbNucleo5.Size = new Size(64, 23);
            pbNucleo5.TabIndex = 56;
            pbNucleo5.Value = 100;
            //
            // btnCelda_5_0
            //
            btnCelda_5_0.Dock = DockStyle.Fill;
            btnCelda_5_0.Location = new Point(205, 302);
            btnCelda_5_0.Margin = new Padding(2);
            btnCelda_5_0.Name = "btnCelda_5_0";
            btnCelda_5_0.Size = new Size(79, 56);
            btnCelda_5_0.TabIndex = 57;
            btnCelda_5_0.UseVisualStyleBackColor = true;
            btnCelda_5_0.Click += btnCelda_Click;
            //
            // btnCelda_5_1
            //
            btnCelda_5_1.Dock = DockStyle.Fill;
            btnCelda_5_1.Location = new Point(288, 302);
            btnCelda_5_1.Margin = new Padding(2);
            btnCelda_5_1.Name = "btnCelda_5_1";
            btnCelda_5_1.Size = new Size(79, 56);
            btnCelda_5_1.TabIndex = 58;
            btnCelda_5_1.UseVisualStyleBackColor = true;
            btnCelda_5_1.Click += btnCelda_Click;
            //
            // btnCelda_5_2
            //
            btnCelda_5_2.Dock = DockStyle.Fill;
            btnCelda_5_2.Location = new Point(371, 302);
            btnCelda_5_2.Margin = new Padding(2);
            btnCelda_5_2.Name = "btnCelda_5_2";
            btnCelda_5_2.Size = new Size(79, 56);
            btnCelda_5_2.TabIndex = 59;
            btnCelda_5_2.UseVisualStyleBackColor = true;
            btnCelda_5_2.Click += btnCelda_Click;
            //
            // btnCelda_5_3
            //
            btnCelda_5_3.Dock = DockStyle.Fill;
            btnCelda_5_3.Location = new Point(454, 302);
            btnCelda_5_3.Margin = new Padding(2);
            btnCelda_5_3.Name = "btnCelda_5_3";
            btnCelda_5_3.Size = new Size(79, 56);
            btnCelda_5_3.TabIndex = 60;
            btnCelda_5_3.UseVisualStyleBackColor = true;
            btnCelda_5_3.Click += btnCelda_Click;
            //
            // btnCelda_5_4
            //
            btnCelda_5_4.Dock = DockStyle.Fill;
            btnCelda_5_4.Location = new Point(537, 302);
            btnCelda_5_4.Margin = new Padding(2);
            btnCelda_5_4.Name = "btnCelda_5_4";
            btnCelda_5_4.Size = new Size(79, 56);
            btnCelda_5_4.TabIndex = 61;
            btnCelda_5_4.UseVisualStyleBackColor = true;
            btnCelda_5_4.Click += btnCelda_Click;
            //
            // btnCelda_5_5
            //
            btnCelda_5_5.Dock = DockStyle.Fill;
            btnCelda_5_5.Location = new Point(620, 302);
            btnCelda_5_5.Margin = new Padding(2);
            btnCelda_5_5.Name = "btnCelda_5_5";
            btnCelda_5_5.Size = new Size(79, 56);
            btnCelda_5_5.TabIndex = 62;
            btnCelda_5_5.UseVisualStyleBackColor = true;
            btnCelda_5_5.Click += btnCelda_Click;
            //
            // btnCelda_5_6
            //
            btnCelda_5_6.Dock = DockStyle.Fill;
            btnCelda_5_6.Location = new Point(703, 302);
            btnCelda_5_6.Margin = new Padding(2);
            btnCelda_5_6.Name = "btnCelda_5_6";
            btnCelda_5_6.Size = new Size(79, 56);
            btnCelda_5_6.TabIndex = 63;
            btnCelda_5_6.UseVisualStyleBackColor = true;
            btnCelda_5_6.Click += btnCelda_Click;
            //
            // btnCelda_5_7
            //
            btnCelda_5_7.Dock = DockStyle.Fill;
            btnCelda_5_7.Location = new Point(786, 302);
            btnCelda_5_7.Margin = new Padding(2);
            btnCelda_5_7.Name = "btnCelda_5_7";
            btnCelda_5_7.Size = new Size(79, 56);
            btnCelda_5_7.TabIndex = 64;
            btnCelda_5_7.UseVisualStyleBackColor = true;
            btnCelda_5_7.Click += btnCelda_Click;
            //
            // btnCelda_5_8
            //
            btnCelda_5_8.Dock = DockStyle.Fill;
            btnCelda_5_8.Location = new Point(869, 302);
            btnCelda_5_8.Margin = new Padding(2);
            btnCelda_5_8.Name = "btnCelda_5_8";
            btnCelda_5_8.Size = new Size(79, 56);
            btnCelda_5_8.TabIndex = 65;
            btnCelda_5_8.UseVisualStyleBackColor = true;
            btnCelda_5_8.Click += btnCelda_Click;
            //
            // grpComandante
            //
            grpComandante.Controls.Add(lblEstadoHeroe);
            grpComandante.Controls.Add(lblVidaHeroe);
            grpComandante.Controls.Add(pbVidaHeroe);
            grpComandante.Controls.Add(lblDanioHeroe);
            grpComandante.Controls.Add(lblCadena);
            grpComandante.Controls.Add(lstCadena);
            grpComandante.Controls.Add(btnSubirDefensa);
            grpComandante.Controls.Add(btnBajarDefensa);
            grpComandante.Location = new Point(972, 58);
            grpComandante.Name = "grpComandante";
            grpComandante.Size = new Size(306, 200);
            grpComandante.TabIndex = 10;
            grpComandante.TabStop = false;
            grpComandante.Text = "Comandante  [State]";
            //
            // lblEstadoHeroe
            //
            lblEstadoHeroe.AutoSize = true;
            lblEstadoHeroe.Location = new Point(10, 24);
            lblEstadoHeroe.Name = "lblEstadoHeroe";
            lblEstadoHeroe.Size = new Size(45, 15);
            lblEstadoHeroe.TabIndex = 0;
            lblEstadoHeroe.Text = "Estado:";
            //
            // lblVidaHeroe
            //
            lblVidaHeroe.AutoSize = true;
            lblVidaHeroe.Location = new Point(10, 44);
            lblVidaHeroe.Name = "lblVidaHeroe";
            lblVidaHeroe.Size = new Size(32, 15);
            lblVidaHeroe.TabIndex = 1;
            lblVidaHeroe.Text = "Vida:";
            //
            // pbVidaHeroe
            //
            pbVidaHeroe.Location = new Point(10, 62);
            pbVidaHeroe.Name = "pbVidaHeroe";
            pbVidaHeroe.Size = new Size(286, 18);
            pbVidaHeroe.TabIndex = 2;
            //
            // lblDanioHeroe
            //
            lblDanioHeroe.AutoSize = true;
            lblDanioHeroe.Location = new Point(170, 24);
            lblDanioHeroe.Name = "lblDanioHeroe";
            lblDanioHeroe.Size = new Size(38, 15);
            lblDanioHeroe.TabIndex = 3;
            lblDanioHeroe.Text = "Daño:";
            //
            // lblCadena
            //
            lblCadena.AutoSize = true;
            lblCadena.Location = new Point(10, 86);
            lblCadena.Name = "lblCadena";
            lblCadena.Size = new Size(165, 15);
            lblCadena.TabIndex = 4;
            lblCadena.Text = "Cadena de defensas  [Chain]:";
            //
            // lstCadena
            //
            lstCadena.FormattingEnabled = true;
            lstCadena.IntegralHeight = false;
            lstCadena.Location = new Point(10, 104);
            lstCadena.Name = "lstCadena";
            lstCadena.Size = new Size(240, 88);
            lstCadena.TabIndex = 5;
            //
            // btnSubirDefensa
            //
            btnSubirDefensa.Location = new Point(256, 104);
            btnSubirDefensa.Name = "btnSubirDefensa";
            btnSubirDefensa.Size = new Size(40, 42);
            btnSubirDefensa.TabIndex = 6;
            btnSubirDefensa.Text = "↑";
            btnSubirDefensa.UseVisualStyleBackColor = true;
            btnSubirDefensa.Click += btnSubirDefensa_Click;
            //
            // btnBajarDefensa
            //
            btnBajarDefensa.Location = new Point(256, 150);
            btnBajarDefensa.Name = "btnBajarDefensa";
            btnBajarDefensa.Size = new Size(40, 42);
            btnBajarDefensa.TabIndex = 7;
            btnBajarDefensa.Text = "↓";
            btnBajarDefensa.UseVisualStyleBackColor = true;
            btnBajarDefensa.Click += btnBajarDefensa_Click;
            //
            // grpRecorrido
            //
            grpRecorrido.Controls.Add(lstRecorrido);
            grpRecorrido.Location = new Point(490, 530);
            grpRecorrido.Name = "grpRecorrido";
            grpRecorrido.Size = new Size(282, 144);
            grpRecorrido.TabIndex = 11;
            grpRecorrido.TabStop = false;
            grpRecorrido.Text = "Recorrido del último golpe";
            //
            // lstRecorrido
            //
            lstRecorrido.Dock = DockStyle.Fill;
            lstRecorrido.FormattingEnabled = true;
            lstRecorrido.IntegralHeight = false;
            lstRecorrido.Location = new Point(3, 19);
            lstRecorrido.Name = "lstRecorrido";
            lstRecorrido.Size = new Size(276, 122);
            lstRecorrido.TabIndex = 0;
            //
            // grpReserva
            //
            grpReserva.Controls.Add(lstReserva);
            grpReserva.Controls.Add(lblDetalleReserva);
            grpReserva.Location = new Point(972, 58);
            grpReserva.Name = "grpReserva";
            grpReserva.Size = new Size(306, 200);
            grpReserva.TabIndex = 6;
            grpReserva.TabStop = false;
            grpReserva.Text = "Reserva - listas para desplegar";
            //
            // lstReserva
            //
            lstReserva.FormattingEnabled = true;
            lstReserva.IntegralHeight = false;
            lstReserva.Location = new Point(10, 22);
            lstReserva.Name = "lstReserva";
            lstReserva.Size = new Size(286, 100);
            lstReserva.TabIndex = 0;
            lstReserva.SelectedIndexChanged += lstReserva_SelectedIndexChanged;
            //
            // lblDetalleReserva
            //
            lblDetalleReserva.Location = new Point(10, 128);
            lblDetalleReserva.Name = "lblDetalleReserva";
            lblDetalleReserva.Size = new Size(286, 64);
            lblDetalleReserva.TabIndex = 1;
            lblDetalleReserva.Text = "Reserva vacía.";
            //
            // grpComandos
            //
            grpComandos.Controls.Add(btnDeshacer);
            grpComandos.Controls.Add(btnRehacer);
            grpComandos.Controls.Add(lstComandos);
            grpComandos.Controls.Add(lblReparar);
            grpComandos.Controls.Add(cmbCarrilReparar);
            grpComandos.Controls.Add(btnReparar);
            grpComandos.Controls.Add(btnDisparo);
            grpComandos.Location = new Point(972, 264);
            grpComandos.Name = "grpComandos";
            grpComandos.Size = new Size(306, 260);
            grpComandos.TabIndex = 7;
            grpComandos.TabStop = false;
            grpComandos.Text = "Acciones del jugador  [Command]";
            //
            // btnDeshacer
            //
            btnDeshacer.Location = new Point(10, 22);
            btnDeshacer.Name = "btnDeshacer";
            btnDeshacer.Size = new Size(138, 28);
            btnDeshacer.TabIndex = 0;
            btnDeshacer.Text = "↶ Deshacer";
            btnDeshacer.UseVisualStyleBackColor = true;
            btnDeshacer.Click += btnDeshacer_Click;
            //
            // btnRehacer
            //
            btnRehacer.Location = new Point(158, 22);
            btnRehacer.Name = "btnRehacer";
            btnRehacer.Size = new Size(138, 28);
            btnRehacer.TabIndex = 1;
            btnRehacer.Text = "Rehacer ↷";
            btnRehacer.UseVisualStyleBackColor = true;
            btnRehacer.Click += btnRehacer_Click;
            //
            // lstComandos
            //
            lstComandos.FormattingEnabled = true;
            lstComandos.HorizontalScrollbar = true;
            lstComandos.IntegralHeight = false;
            lstComandos.Location = new Point(10, 56);
            lstComandos.Name = "lstComandos";
            lstComandos.Size = new Size(286, 100);
            lstComandos.TabIndex = 2;
            //
            // lblReparar
            //
            lblReparar.AutoSize = true;
            lblReparar.Location = new Point(10, 164);
            lblReparar.Name = "lblReparar";
            lblReparar.Size = new Size(180, 15);
            lblReparar.TabIndex = 3;
            lblReparar.Text = "Reparar núcleo (entre oleadas):";
            //
            // cmbCarrilReparar
            //
            cmbCarrilReparar.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCarrilReparar.FormattingEnabled = true;
            cmbCarrilReparar.Location = new Point(10, 185);
            cmbCarrilReparar.Name = "cmbCarrilReparar";
            cmbCarrilReparar.Size = new Size(138, 23);
            cmbCarrilReparar.TabIndex = 4;
            //
            // btnReparar
            //
            btnReparar.Location = new Point(158, 182);
            btnReparar.Name = "btnReparar";
            btnReparar.Size = new Size(138, 28);
            btnReparar.TabIndex = 5;
            btnReparar.Text = "Reparar (60 cr)";
            btnReparar.UseVisualStyleBackColor = true;
            btnReparar.Click += btnReparar_Click;
            //
            // btnDisparo
            //
            btnDisparo.Location = new Point(10, 220);
            btnDisparo.Name = "btnDisparo";
            btnDisparo.Size = new Size(286, 28);
            btnDisparo.TabIndex = 6;
            btnDisparo.Text = "Disparo orbital al enemigo elegido (40 cr)";
            btnDisparo.UseVisualStyleBackColor = true;
            btnDisparo.Click += btnDisparo_Click;
            //
            // grpHistorial
            //
            grpHistorial.Controls.Add(txtHistorial);
            grpHistorial.Location = new Point(12, 530);
            grpHistorial.Name = "grpHistorial";
            grpHistorial.Size = new Size(470, 144);
            grpHistorial.TabIndex = 8;
            grpHistorial.TabStop = false;
            grpHistorial.Text = "Historial de turnos  [Template Method]";
            //
            // txtHistorial
            //
            txtHistorial.Dock = DockStyle.Fill;
            txtHistorial.Font = new Font("Consolas", 8.25F);
            txtHistorial.Location = new Point(3, 19);
            txtHistorial.Multiline = true;
            txtHistorial.Name = "txtHistorial";
            txtHistorial.ReadOnly = true;
            txtHistorial.ScrollBars = ScrollBars.Vertical;
            txtHistorial.Size = new Size(464, 122);
            txtHistorial.TabIndex = 0;
            //
            // grpRegistro
            //
            grpRegistro.Controls.Add(txtRegistro);
            grpRegistro.Location = new Point(780, 530);
            grpRegistro.Name = "grpRegistro";
            grpRegistro.Size = new Size(498, 144);
            grpRegistro.TabIndex = 9;
            grpRegistro.TabStop = false;
            grpRegistro.Text = "Registro de la estación";
            //
            // txtRegistro
            //
            txtRegistro.Dock = DockStyle.Fill;
            txtRegistro.Font = new Font("Consolas", 8.25F);
            txtRegistro.Location = new Point(3, 19);
            txtRegistro.Multiline = true;
            txtRegistro.Name = "txtRegistro";
            txtRegistro.ReadOnly = true;
            txtRegistro.ScrollBars = ScrollBars.Vertical;
            txtRegistro.Size = new Size(492, 122);
            txtRegistro.TabIndex = 0;
            //
            // timerTick
            //
            timerTick.Interval = 600;
            timerTick.Tick += timerTick_Tick;
            //
            // FormPrincipal
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1290, 684);
            Controls.Add(menuPrincipal);
            Controls.Add(lblEstado);
            Controls.Add(grpOleada);
            Controls.Add(grpEstrategia);
            Controls.Add(lblLeyenda);
            Controls.Add(tablaTablero);
            Controls.Add(grpComandante);
            Controls.Add(grpRecorrido);
            Controls.Add(grpReserva);
            Controls.Add(grpComandos);
            Controls.Add(grpHistorial);
            Controls.Add(grpRegistro);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuPrincipal;
            MaximizeBox = false;
            Name = "FormPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kepler-7 - Defensa de la estación";
            menuPrincipal.ResumeLayout(false);
            menuPrincipal.PerformLayout();
            grpOleada.ResumeLayout(false);
            grpOleada.PerformLayout();
            grpEstrategia.ResumeLayout(false);
            tablaTablero.ResumeLayout(false);
            grpComandante.ResumeLayout(false);
            grpComandante.PerformLayout();
            grpRecorrido.ResumeLayout(false);
            grpReserva.ResumeLayout(false);
            grpComandos.ResumeLayout(false);
            grpComandos.PerformLayout();
            grpHistorial.ResumeLayout(false);
            grpHistorial.PerformLayout();
            grpRegistro.ResumeLayout(false);
            grpRegistro.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuPrincipal;
        private ToolStripMenuItem menuAstillero;
        private ToolStripMenuItem menuLaboratorio;
        private ToolStripMenuItem menuInventario;
        private ToolStripMenuItem menuTripulacion;
        private ToolStripMenuItem menuControlCentral;
        private ToolStripMenuItem menuHub;
        private ToolStripMenuItem menuReiniciar;
        private Label lblEstado;
        private GroupBox grpOleada;
        private Button btnIniciarOleada;
        private Label lblVelocidad;
        private ComboBox cmbVelocidad;
        private GroupBox grpEstrategia;
        private ComboBox cmbIA;
        private Button btnIAUno;
        private Button btnIATodos;
        private Label lblInfoIA;
        private Label lblElegido;
        private Label lblLeyenda;
        private TableLayoutPanel tablaTablero;
        private Label lblCarril0;
        private ProgressBar pbNucleo0;
        private Button btnCelda_0_0;
        private Button btnCelda_0_1;
        private Button btnCelda_0_2;
        private Button btnCelda_0_3;
        private Button btnCelda_0_4;
        private Button btnCelda_0_5;
        private Button btnCelda_0_6;
        private Button btnCelda_0_7;
        private Button btnCelda_0_8;
        private Label lblCarril1;
        private ProgressBar pbNucleo1;
        private Button btnCelda_1_0;
        private Button btnCelda_1_1;
        private Button btnCelda_1_2;
        private Button btnCelda_1_3;
        private Button btnCelda_1_4;
        private Button btnCelda_1_5;
        private Button btnCelda_1_6;
        private Button btnCelda_1_7;
        private Button btnCelda_1_8;
        private Label lblCarril2;
        private ProgressBar pbNucleo2;
        private Button btnCelda_2_0;
        private Button btnCelda_2_1;
        private Button btnCelda_2_2;
        private Button btnCelda_2_3;
        private Button btnCelda_2_4;
        private Button btnCelda_2_5;
        private Button btnCelda_2_6;
        private Button btnCelda_2_7;
        private Button btnCelda_2_8;
        private Label lblCarril3;
        private ProgressBar pbNucleo3;
        private Button btnCelda_3_0;
        private Button btnCelda_3_1;
        private Button btnCelda_3_2;
        private Button btnCelda_3_3;
        private Button btnCelda_3_4;
        private Button btnCelda_3_5;
        private Button btnCelda_3_6;
        private Button btnCelda_3_7;
        private Button btnCelda_3_8;
        private Label lblCarril4;
        private ProgressBar pbNucleo4;
        private Button btnCelda_4_0;
        private Button btnCelda_4_1;
        private Button btnCelda_4_2;
        private Button btnCelda_4_3;
        private Button btnCelda_4_4;
        private Button btnCelda_4_5;
        private Button btnCelda_4_6;
        private Button btnCelda_4_7;
        private Button btnCelda_4_8;
        private Label lblCarril5;
        private ProgressBar pbNucleo5;
        private Button btnCelda_5_0;
        private Button btnCelda_5_1;
        private Button btnCelda_5_2;
        private Button btnCelda_5_3;
        private Button btnCelda_5_4;
        private Button btnCelda_5_5;
        private Button btnCelda_5_6;
        private Button btnCelda_5_7;
        private Button btnCelda_5_8;
        private GroupBox grpComandante;
        private Label lblEstadoHeroe;
        private Label lblVidaHeroe;
        private ProgressBar pbVidaHeroe;
        private Label lblDanioHeroe;
        private Label lblCadena;
        private ListBox lstCadena;
        private Button btnSubirDefensa;
        private Button btnBajarDefensa;
        private GroupBox grpRecorrido;
        private ListBox lstRecorrido;
        private GroupBox grpReserva;
        private ListBox lstReserva;
        private Label lblDetalleReserva;
        private GroupBox grpComandos;
        private Button btnDeshacer;
        private Button btnRehacer;
        private ListBox lstComandos;
        private Label lblReparar;
        private ComboBox cmbCarrilReparar;
        private Button btnReparar;
        private Button btnDisparo;
        private GroupBox grpHistorial;
        private TextBox txtHistorial;
        private GroupBox grpRegistro;
        private TextBox txtRegistro;
        private System.Windows.Forms.Timer timerTick;
    }
}

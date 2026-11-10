namespace Kepler7.Game
{
    partial class FormHub
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
            grpProgreso = new GroupBox();
            lblProgreso = new Label();
            pbNivel = new ProgressBar();
            btnMision = new Button();
            lblMensaje = new Label();
            lblLogros = new Label();
            lstLogros = new ListBox();
            grpBoveda = new GroupBox();
            lblBoveda = new Label();
            lstLogBoveda = new ListBox();
            grpGuardado = new GroupBox();
            lblDonde = new Label();
            cmbAlmacenamiento = new ComboBox();
            btnGuardar = new Button();
            btnCargar = new Button();
            lblGuardado = new Label();
            btnCerrar = new Button();
            grpTienda = new GroupBox();
            lblTienda = new Label();
            cmbMejoras = new ComboBox();
            btnComprar = new Button();
            lblDetalleMejora = new Label();
            lblResultadoTienda = new Label();
            lblPendientes = new Label();
            grpProgreso.SuspendLayout();
            grpBoveda.SuspendLayout();
            grpTienda.SuspendLayout();
            grpGuardado.SuspendLayout();
            SuspendLayout();
            //
            // grpProgreso
            //
            grpProgreso.Controls.Add(lblProgreso);
            grpProgreso.Controls.Add(pbNivel);
            grpProgreso.Controls.Add(btnMision);
            grpProgreso.Controls.Add(lblMensaje);
            grpProgreso.Controls.Add(lblLogros);
            grpProgreso.Controls.Add(lstLogros);
            grpProgreso.Location = new Point(12, 12);
            grpProgreso.Name = "grpProgreso";
            grpProgreso.Size = new Size(556, 186);
            grpProgreso.TabIndex = 0;
            grpProgreso.TabStop = false;
            grpProgreso.Text = "Progreso del jugador (XP, niveles, monedas y logros)";
            //
            // lblProgreso
            //
            lblProgreso.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblProgreso.Location = new Point(10, 22);
            lblProgreso.Name = "lblProgreso";
            lblProgreso.Size = new Size(320, 52);
            lblProgreso.TabIndex = 0;
            lblProgreso.Text = "Nivel 1";
            //
            // pbNivel
            //
            pbNivel.Location = new Point(10, 78);
            pbNivel.Maximum = 100;
            pbNivel.Name = "pbNivel";
            pbNivel.Size = new Size(320, 16);
            pbNivel.TabIndex = 1;
            //
            // btnMision
            //
            btnMision.Location = new Point(10, 100);
            btnMision.Name = "btnMision";
            btnMision.Size = new Size(320, 30);
            btnMision.TabIndex = 2;
            btnMision.Text = "Completar misión";
            btnMision.UseVisualStyleBackColor = true;
            btnMision.Click += btnMision_Click;
            //
            // lblMensaje
            //
            lblMensaje.Location = new Point(10, 136);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new Size(320, 44);
            lblMensaje.TabIndex = 3;
            //
            // lblLogros
            //
            lblLogros.AutoSize = true;
            lblLogros.Location = new Point(340, 22);
            lblLogros.Name = "lblLogros";
            lblLogros.Size = new Size(44, 15);
            lblLogros.TabIndex = 4;
            lblLogros.Text = "Logros:";
            //
            // lstLogros
            //
            lstLogros.FormattingEnabled = true;
            lstLogros.HorizontalScrollbar = true;
            lstLogros.IntegralHeight = false;
            lstLogros.Location = new Point(340, 42);
            lstLogros.Name = "lstLogros";
            lstLogros.SelectionMode = SelectionMode.None;
            lstLogros.Size = new Size(206, 136);
            lstLogros.TabIndex = 5;
            //
            // grpBoveda
            //
            grpBoveda.Controls.Add(lblBoveda);
            grpBoveda.Controls.Add(lstLogBoveda);
            grpBoveda.Location = new Point(12, 204);
            grpBoveda.Name = "grpBoveda";
            grpBoveda.Size = new Size(556, 206);
            grpBoveda.TabIndex = 1;
            grpBoveda.TabStop = false;
            grpBoveda.Text = "Bóveda de monedas  [Proxy]";
            //
            // lblBoveda
            //
            lblBoveda.Location = new Point(10, 22);
            lblBoveda.Name = "lblBoveda";
            lblBoveda.Size = new Size(536, 20);
            lblBoveda.TabIndex = 0;
            lblBoveda.Text = "Saldo: 0 monedas.";
            //
            // lstLogBoveda
            //
            lstLogBoveda.FormattingEnabled = true;
            lstLogBoveda.HorizontalScrollbar = true;
            lstLogBoveda.IntegralHeight = false;
            lstLogBoveda.Location = new Point(10, 46);
            lstLogBoveda.Name = "lstLogBoveda";
            lstLogBoveda.SelectionMode = SelectionMode.None;
            lstLogBoveda.Size = new Size(536, 150);
            lstLogBoveda.TabIndex = 1;
            //
            // grpTienda
            //
            grpTienda.Controls.Add(lblTienda);
            grpTienda.Controls.Add(cmbMejoras);
            grpTienda.Controls.Add(btnComprar);
            grpTienda.Controls.Add(lblDetalleMejora);
            grpTienda.Controls.Add(lblResultadoTienda);
            grpTienda.Controls.Add(lblPendientes);
            grpTienda.Location = new Point(12, 416);
            grpTienda.Name = "grpTienda";
            grpTienda.Size = new Size(556, 144);
            grpTienda.TabIndex = 2;
            grpTienda.TabStop = false;
            grpTienda.Text = "Tienda de mejoras  [se paga a través del Proxy]";
            //
            // lblTienda
            //
            lblTienda.Location = new Point(10, 22);
            lblTienda.Name = "lblTienda";
            lblTienda.Size = new Size(536, 18);
            lblTienda.TabIndex = 0;
            lblTienda.Text = "Lo que compres se aplica cuando empieces una partida nueva (menú Reiniciar partida).";
            //
            // cmbMejoras
            //
            cmbMejoras.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMejoras.FormattingEnabled = true;
            cmbMejoras.Location = new Point(10, 44);
            cmbMejoras.Name = "cmbMejoras";
            cmbMejoras.Size = new Size(300, 23);
            cmbMejoras.TabIndex = 1;
            cmbMejoras.SelectedIndexChanged += cmbMejoras_SelectedIndexChanged;
            //
            // btnComprar
            //
            btnComprar.Location = new Point(318, 42);
            btnComprar.Name = "btnComprar";
            btnComprar.Size = new Size(228, 28);
            btnComprar.TabIndex = 2;
            btnComprar.Text = "Comprar mejora";
            btnComprar.UseVisualStyleBackColor = true;
            btnComprar.Click += btnComprar_Click;
            //
            // lblDetalleMejora
            //
            lblDetalleMejora.Location = new Point(10, 74);
            lblDetalleMejora.Name = "lblDetalleMejora";
            lblDetalleMejora.Size = new Size(536, 18);
            lblDetalleMejora.TabIndex = 3;
            //
            // lblResultadoTienda
            //
            lblResultadoTienda.Location = new Point(10, 96);
            lblResultadoTienda.Name = "lblResultadoTienda";
            lblResultadoTienda.Size = new Size(536, 18);
            lblResultadoTienda.TabIndex = 4;
            //
            // lblPendientes
            //
            lblPendientes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPendientes.Location = new Point(10, 118);
            lblPendientes.Name = "lblPendientes";
            lblPendientes.Size = new Size(536, 18);
            lblPendientes.TabIndex = 5;
            //
            // grpGuardado
            //
            grpGuardado.Controls.Add(lblDonde);
            grpGuardado.Controls.Add(cmbAlmacenamiento);
            grpGuardado.Controls.Add(btnGuardar);
            grpGuardado.Controls.Add(btnCargar);
            grpGuardado.Controls.Add(lblGuardado);
            grpGuardado.Location = new Point(12, 566);
            grpGuardado.Name = "grpGuardado";
            grpGuardado.Size = new Size(556, 108);
            grpGuardado.TabIndex = 3;
            grpGuardado.TabStop = false;
            grpGuardado.Text = "Guardado JSON del progreso, la Bóveda y la partida  [Bridge]";
            //
            // lblDonde
            //
            lblDonde.AutoSize = true;
            lblDonde.Location = new Point(10, 28);
            lblDonde.Name = "lblDonde";
            lblDonde.Size = new Size(87, 15);
            lblDonde.TabIndex = 0;
            lblDonde.Text = "Dónde guardar:";
            //
            // cmbAlmacenamiento
            //
            cmbAlmacenamiento.DisplayMember = "Nombre";
            cmbAlmacenamiento.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAlmacenamiento.FormattingEnabled = true;
            cmbAlmacenamiento.Location = new Point(106, 24);
            cmbAlmacenamiento.Name = "cmbAlmacenamiento";
            cmbAlmacenamiento.Size = new Size(150, 23);
            cmbAlmacenamiento.TabIndex = 1;
            cmbAlmacenamiento.SelectedIndexChanged += cmbAlmacenamiento_SelectedIndexChanged;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(266, 22);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(136, 28);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "Guardar todo";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCargar
            //
            btnCargar.Location = new Point(410, 22);
            btnCargar.Name = "btnCargar";
            btnCargar.Size = new Size(136, 28);
            btnCargar.TabIndex = 3;
            btnCargar.Text = "Cargar progreso";
            btnCargar.UseVisualStyleBackColor = true;
            btnCargar.Click += btnCargar_Click;
            //
            // lblGuardado
            //
            lblGuardado.Location = new Point(10, 56);
            lblGuardado.Name = "lblGuardado";
            lblGuardado.Size = new Size(536, 44);
            lblGuardado.TabIndex = 4;
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(468, 682);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 28);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FormHub
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(580, 722);
            Controls.Add(grpProgreso);
            Controls.Add(grpBoveda);
            Controls.Add(grpTienda);
            Controls.Add(grpGuardado);
            Controls.Add(btnCerrar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormHub";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Hub y progreso  [Proxy + Bridge]";
            grpProgreso.ResumeLayout(false);
            grpProgreso.PerformLayout();
            grpBoveda.ResumeLayout(false);
            grpTienda.ResumeLayout(false);
            grpGuardado.ResumeLayout(false);
            grpGuardado.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpProgreso;
        private Label lblProgreso;
        private ProgressBar pbNivel;
        private Button btnMision;
        private Label lblMensaje;
        private Label lblLogros;
        private ListBox lstLogros;
        private GroupBox grpBoveda;
        private Label lblBoveda;
        private ListBox lstLogBoveda;
        private GroupBox grpGuardado;
        private Label lblDonde;
        private ComboBox cmbAlmacenamiento;
        private Button btnGuardar;
        private Button btnCargar;
        private Label lblGuardado;
        private Button btnCerrar;
        private GroupBox grpTienda;
        private Label lblTienda;
        private ComboBox cmbMejoras;
        private Button btnComprar;
        private Label lblDetalleMejora;
        private Label lblResultadoTienda;
        private Label lblPendientes;
    }
}

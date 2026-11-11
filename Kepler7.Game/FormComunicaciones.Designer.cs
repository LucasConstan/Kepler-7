namespace Kepler7.Game
{
    partial class FormComunicaciones
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
            lblExplicacion = new Label();
            lblTipo = new Label();
            cmbTipo = new ComboBox();
            lblCifrado = new Label();
            cmbCifrado = new ComboBox();
            lblCombinacion = new Label();
            lblContenido = new Label();
            txtContenido = new TextBox();
            btnEnviar = new Button();
            lblResultado = new Label();
            txtResultado = new TextBox();
            lblHistorial = new Label();
            lstHistorial = new ListBox();
            btnCerrar = new Button();
            SuspendLayout();
            //
            // lblExplicacion
            //
            lblExplicacion.Location = new Point(12, 9);
            lblExplicacion.Name = "lblExplicacion";
            lblExplicacion.Size = new Size(716, 34);
            lblExplicacion.TabIndex = 0;
            lblExplicacion.Text = "Elegí el tipo de mensaje y el cifrado por separado: cualquier tipo se combina con cualquier cifrado en el momento, sin una clase para cada combinación.";
            //
            // lblTipo
            //
            lblTipo.AutoSize = true;
            lblTipo.Location = new Point(12, 52);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(97, 15);
            lblTipo.TabIndex = 1;
            lblTipo.Text = "Tipo de mensaje:";
            //
            // cmbTipo
            //
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.Location = new Point(120, 48);
            cmbTipo.Name = "cmbTipo";
            cmbTipo.Size = new Size(150, 23);
            cmbTipo.TabIndex = 2;
            cmbTipo.SelectedIndexChanged += cmbTipo_SelectedIndexChanged;
            //
            // lblCifrado
            //
            lblCifrado.AutoSize = true;
            lblCifrado.Location = new Point(292, 52);
            lblCifrado.Name = "lblCifrado";
            lblCifrado.Size = new Size(48, 15);
            lblCifrado.TabIndex = 3;
            lblCifrado.Text = "Cifrado:";
            //
            // cmbCifrado
            //
            cmbCifrado.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCifrado.Location = new Point(348, 48);
            cmbCifrado.Name = "cmbCifrado";
            cmbCifrado.Size = new Size(150, 23);
            cmbCifrado.TabIndex = 4;
            cmbCifrado.SelectedIndexChanged += cmbCifrado_SelectedIndexChanged;
            //
            // lblCombinacion
            //
            lblCombinacion.ForeColor = Color.SteelBlue;
            lblCombinacion.Location = new Point(12, 78);
            lblCombinacion.Name = "lblCombinacion";
            lblCombinacion.Size = new Size(716, 34);
            lblCombinacion.TabIndex = 5;
            lblCombinacion.Text = "Combinación actual:";
            //
            // lblContenido
            //
            lblContenido.AutoSize = true;
            lblContenido.Location = new Point(12, 116);
            lblContenido.Name = "lblContenido";
            lblContenido.Size = new Size(64, 15);
            lblContenido.TabIndex = 6;
            lblContenido.Text = "Contenido:";
            //
            // txtContenido
            //
            txtContenido.Location = new Point(12, 134);
            txtContenido.Multiline = true;
            txtContenido.Name = "txtContenido";
            txtContenido.Size = new Size(590, 46);
            txtContenido.TabIndex = 7;
            //
            // btnEnviar
            //
            btnEnviar.Location = new Point(612, 134);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(116, 46);
            btnEnviar.TabIndex = 8;
            btnEnviar.Text = "Enviar a la Tierra";
            btnEnviar.UseVisualStyleBackColor = true;
            btnEnviar.Click += btnEnviar_Click;
            //
            // lblResultado
            //
            lblResultado.AutoSize = true;
            lblResultado.Location = new Point(12, 190);
            lblResultado.Name = "lblResultado";
            lblResultado.Size = new Size(133, 15);
            lblResultado.TabIndex = 9;
            lblResultado.Text = "Mensaje y cifrado:";
            //
            // txtResultado
            //
            txtResultado.Font = new Font("Consolas", 8.25F);
            txtResultado.Location = new Point(12, 208);
            txtResultado.Multiline = true;
            txtResultado.Name = "txtResultado";
            txtResultado.ReadOnly = true;
            txtResultado.ScrollBars = ScrollBars.Vertical;
            txtResultado.Size = new Size(480, 270);
            txtResultado.TabIndex = 10;
            //
            // lblHistorial
            //
            lblHistorial.AutoSize = true;
            lblHistorial.Location = new Point(502, 190);
            lblHistorial.Name = "lblHistorial";
            lblHistorial.Size = new Size(118, 15);
            lblHistorial.TabIndex = 11;
            lblHistorial.Text = "Mensajes enviados:";
            //
            // lstHistorial
            //
            lstHistorial.FormattingEnabled = true;
            lstHistorial.IntegralHeight = false;
            lstHistorial.Location = new Point(502, 208);
            lstHistorial.Name = "lstHistorial";
            lstHistorial.Size = new Size(226, 270);
            lstHistorial.TabIndex = 12;
            lstHistorial.SelectedIndexChanged += lstHistorial_SelectedIndexChanged;
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(628, 488);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 28);
            btnCerrar.TabIndex = 13;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FormComunicaciones
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(740, 528);
            Controls.Add(lblExplicacion);
            Controls.Add(lblTipo);
            Controls.Add(cmbTipo);
            Controls.Add(lblCifrado);
            Controls.Add(cmbCifrado);
            Controls.Add(lblCombinacion);
            Controls.Add(lblContenido);
            Controls.Add(txtContenido);
            Controls.Add(btnEnviar);
            Controls.Add(lblResultado);
            Controls.Add(txtResultado);
            Controls.Add(lblHistorial);
            Controls.Add(lstHistorial);
            Controls.Add(btnCerrar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormComunicaciones";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Comunicaciones con la Tierra  [Bridge]";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblExplicacion;
        private Label lblTipo;
        private ComboBox cmbTipo;
        private Label lblCifrado;
        private ComboBox cmbCifrado;
        private Label lblCombinacion;
        private Label lblContenido;
        private TextBox txtContenido;
        private Button btnEnviar;
        private Label lblResultado;
        private TextBox txtResultado;
        private Label lblHistorial;
        private ListBox lstHistorial;
        private Button btnCerrar;
    }
}

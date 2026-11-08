namespace Kepler7.Game
{
    partial class FormTripulacion
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
            lblCreditos = new Label();
            grpEnviar = new GroupBox();
            cmbCarril = new ComboBox();
            btnEnviar = new Button();
            lblResultado = new Label();
            grpCarriles = new GroupBox();
            lstCarriles = new ListBox();
            grpComparacion = new GroupBox();
            lblComparacion = new Label();
            btnCerrar = new Button();
            grpEnviar.SuspendLayout();
            grpCarriles.SuspendLayout();
            grpComparacion.SuspendLayout();
            SuspendLayout();
            //
            // lblCreditos
            //
            lblCreditos.AutoSize = true;
            lblCreditos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCreditos.Location = new Point(12, 9);
            lblCreditos.Name = "lblCreditos";
            lblCreditos.Size = new Size(66, 15);
            lblCreditos.TabIndex = 0;
            lblCreditos.Text = "Créditos: 0";
            //
            // grpEnviar
            //
            grpEnviar.Controls.Add(cmbCarril);
            grpEnviar.Controls.Add(btnEnviar);
            grpEnviar.Controls.Add(lblResultado);
            grpEnviar.Location = new Point(12, 32);
            grpEnviar.Name = "grpEnviar";
            grpEnviar.Size = new Size(456, 90);
            grpEnviar.TabIndex = 1;
            grpEnviar.TabStop = false;
            grpEnviar.Text = "Enviar tripulantes al núcleo de un subsistema";
            //
            // cmbCarril
            //
            cmbCarril.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCarril.FormattingEnabled = true;
            cmbCarril.Location = new Point(10, 28);
            cmbCarril.Name = "cmbCarril";
            cmbCarril.Size = new Size(200, 23);
            cmbCarril.TabIndex = 0;
            //
            // btnEnviar
            //
            btnEnviar.Location = new Point(220, 26);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(226, 28);
            btnEnviar.TabIndex = 1;
            btnEnviar.Text = "Enviar tripulantes";
            btnEnviar.UseVisualStyleBackColor = true;
            btnEnviar.Click += btnEnviar_Click;
            //
            // lblResultado
            //
            lblResultado.Location = new Point(10, 60);
            lblResultado.Name = "lblResultado";
            lblResultado.Size = new Size(436, 22);
            lblResultado.TabIndex = 2;
            //
            // grpCarriles
            //
            grpCarriles.Controls.Add(lstCarriles);
            grpCarriles.Location = new Point(12, 128);
            grpCarriles.Name = "grpCarriles";
            grpCarriles.Size = new Size(456, 140);
            grpCarriles.TabIndex = 2;
            grpCarriles.TabStop = false;
            grpCarriles.Text = "Tripulantes por subsistema (son el escudo del núcleo)";
            //
            // lstCarriles
            //
            lstCarriles.FormattingEnabled = true;
            lstCarriles.IntegralHeight = false;
            lstCarriles.Location = new Point(10, 22);
            lstCarriles.Name = "lstCarriles";
            lstCarriles.SelectionMode = SelectionMode.None;
            lstCarriles.Size = new Size(436, 108);
            lstCarriles.TabIndex = 0;
            //
            // grpComparacion
            //
            grpComparacion.Controls.Add(lblComparacion);
            grpComparacion.Location = new Point(12, 274);
            grpComparacion.Name = "grpComparacion";
            grpComparacion.Size = new Size(456, 130);
            grpComparacion.TabIndex = 3;
            grpComparacion.TabStop = false;
            grpComparacion.Text = "Comparación con y sin Flyweight";
            //
            // lblComparacion
            //
            lblComparacion.Location = new Point(10, 20);
            lblComparacion.Name = "lblComparacion";
            lblComparacion.Size = new Size(436, 102);
            lblComparacion.TabIndex = 0;
            lblComparacion.Text = "Datos de la comparación";
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(368, 412);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 28);
            btnCerrar.TabIndex = 4;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FormTripulacion
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(480, 452);
            Controls.Add(lblCreditos);
            Controls.Add(grpEnviar);
            Controls.Add(grpCarriles);
            Controls.Add(grpComparacion);
            Controls.Add(btnCerrar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormTripulacion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Biblioteca de tripulantes  [Flyweight]";
            grpEnviar.ResumeLayout(false);
            grpCarriles.ResumeLayout(false);
            grpComparacion.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCreditos;
        private GroupBox grpEnviar;
        private ComboBox cmbCarril;
        private Button btnEnviar;
        private Label lblResultado;
        private GroupBox grpCarriles;
        private ListBox lstCarriles;
        private GroupBox grpComparacion;
        private Label lblComparacion;
        private Button btnCerrar;
    }
}

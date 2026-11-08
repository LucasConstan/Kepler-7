namespace Kepler7.Game
{
    partial class FormAstillero
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
            lblReceta = new Label();
            cmbRecetas = new ComboBox();
            lblDescripcion = new Label();
            btnEnsamblar = new Button();
            lblComponentes = new Label();
            lstComponentes = new ListBox();
            lblResultado = new Label();
            btnCerrar = new Button();
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
            // lblReceta
            //
            lblReceta.AutoSize = true;
            lblReceta.Location = new Point(12, 36);
            lblReceta.Name = "lblReceta";
            lblReceta.Size = new Size(152, 15);
            lblReceta.TabIndex = 1;
            lblReceta.Text = "Receta (builder concreto):";
            //
            // cmbRecetas
            //
            cmbRecetas.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRecetas.FormattingEnabled = true;
            cmbRecetas.Location = new Point(12, 56);
            cmbRecetas.Name = "cmbRecetas";
            cmbRecetas.Size = new Size(406, 23);
            cmbRecetas.TabIndex = 2;
            cmbRecetas.SelectedIndexChanged += cmbRecetas_SelectedIndexChanged;
            //
            // lblDescripcion
            //
            lblDescripcion.Location = new Point(12, 86);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(406, 36);
            lblDescripcion.TabIndex = 3;
            lblDescripcion.Text = "Descripción de la receta";
            //
            // btnEnsamblar
            //
            btnEnsamblar.Location = new Point(12, 126);
            btnEnsamblar.Name = "btnEnsamblar";
            btnEnsamblar.Size = new Size(406, 32);
            btnEnsamblar.TabIndex = 4;
            btnEnsamblar.Text = "Ensamblar nave";
            btnEnsamblar.UseVisualStyleBackColor = true;
            btnEnsamblar.Click += btnEnsamblar_Click;
            //
            // lblComponentes
            //
            lblComponentes.AutoSize = true;
            lblComponentes.Location = new Point(12, 168);
            lblComponentes.Name = "lblComponentes";
            lblComponentes.Size = new Size(275, 15);
            lblComponentes.TabIndex = 5;
            lblComponentes.Text = "Piezas que colocó el director, en orden (última nave):";
            //
            // lstComponentes
            //
            lstComponentes.FormattingEnabled = true;
            lstComponentes.IntegralHeight = false;
            lstComponentes.Location = new Point(12, 188);
            lstComponentes.Name = "lstComponentes";
            lstComponentes.Size = new Size(406, 110);
            lstComponentes.TabIndex = 6;
            //
            // lblResultado
            //
            lblResultado.Location = new Point(12, 304);
            lblResultado.Name = "lblResultado";
            lblResultado.Size = new Size(406, 22);
            lblResultado.TabIndex = 7;
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(318, 332);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 28);
            btnCerrar.TabIndex = 8;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FormAstillero
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(430, 372);
            Controls.Add(lblCreditos);
            Controls.Add(lblReceta);
            Controls.Add(cmbRecetas);
            Controls.Add(lblDescripcion);
            Controls.Add(btnEnsamblar);
            Controls.Add(lblComponentes);
            Controls.Add(lstComponentes);
            Controls.Add(lblResultado);
            Controls.Add(btnCerrar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAstillero";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Astillero - Nave de apoyo  [Builder]";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCreditos;
        private Label lblReceta;
        private ComboBox cmbRecetas;
        private Label lblDescripcion;
        private Button btnEnsamblar;
        private Label lblComponentes;
        private ListBox lstComponentes;
        private Label lblResultado;
        private Button btnCerrar;
    }
}

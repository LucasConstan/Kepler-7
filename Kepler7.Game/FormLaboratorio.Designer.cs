namespace Kepler7.Game
{
    partial class FormLaboratorio
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
            grpPrototipos = new GroupBox();
            lstPrototipos = new ListBox();
            btnClonar = new Button();
            grpEquipar = new GroupBox();
            lstClones = new ListBox();
            lblEquipo = new Label();
            cmbEquipo = new ComboBox();
            btnEquipar = new Button();
            lblDetalle = new Label();
            btnCerrar = new Button();
            grpPrototipos.SuspendLayout();
            grpEquipar.SuspendLayout();
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
            // grpPrototipos
            //
            grpPrototipos.Controls.Add(lstPrototipos);
            grpPrototipos.Controls.Add(btnClonar);
            grpPrototipos.Location = new Point(12, 32);
            grpPrototipos.Name = "grpPrototipos";
            grpPrototipos.Size = new Size(536, 190);
            grpPrototipos.TabIndex = 1;
            grpPrototipos.TabStop = false;
            grpPrototipos.Text = "Prototipos registrados en el laboratorio";
            //
            // lstPrototipos
            //
            lstPrototipos.FormattingEnabled = true;
            lstPrototipos.IntegralHeight = false;
            lstPrototipos.Location = new Point(10, 22);
            lstPrototipos.Name = "lstPrototipos";
            lstPrototipos.Size = new Size(516, 120);
            lstPrototipos.TabIndex = 0;
            lstPrototipos.Format += lstPrototipos_Format;
            //
            // btnClonar
            //
            btnClonar.Location = new Point(10, 148);
            btnClonar.Name = "btnClonar";
            btnClonar.Size = new Size(516, 32);
            btnClonar.TabIndex = 1;
            btnClonar.Text = "Clonar prototipo seleccionado";
            btnClonar.UseVisualStyleBackColor = true;
            btnClonar.Click += btnClonar_Click;
            //
            // grpEquipar
            //
            grpEquipar.Controls.Add(lstClones);
            grpEquipar.Controls.Add(lblEquipo);
            grpEquipar.Controls.Add(cmbEquipo);
            grpEquipar.Controls.Add(btnEquipar);
            grpEquipar.Controls.Add(lblDetalle);
            grpEquipar.Location = new Point(12, 228);
            grpEquipar.Name = "grpEquipar";
            grpEquipar.Size = new Size(536, 214);
            grpEquipar.TabIndex = 2;
            grpEquipar.TabStop = false;
            grpEquipar.Text = "Equipar un clon de la reserva";
            //
            // lstClones
            //
            lstClones.FormattingEnabled = true;
            lstClones.IntegralHeight = false;
            lstClones.Location = new Point(10, 22);
            lstClones.Name = "lstClones";
            lstClones.Size = new Size(516, 90);
            lstClones.TabIndex = 0;
            lstClones.Format += lstClones_Format;
            //
            // lblEquipo
            //
            lblEquipo.AutoSize = true;
            lblEquipo.Location = new Point(10, 123);
            lblEquipo.Name = "lblEquipo";
            lblEquipo.Size = new Size(47, 15);
            lblEquipo.TabIndex = 1;
            lblEquipo.Text = "Equipo:";
            //
            // cmbEquipo
            //
            cmbEquipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEquipo.FormattingEnabled = true;
            cmbEquipo.Location = new Point(64, 120);
            cmbEquipo.Name = "cmbEquipo";
            cmbEquipo.Size = new Size(316, 23);
            cmbEquipo.TabIndex = 2;
            //
            // btnEquipar
            //
            btnEquipar.Location = new Point(390, 118);
            btnEquipar.Name = "btnEquipar";
            btnEquipar.Size = new Size(136, 28);
            btnEquipar.TabIndex = 3;
            btnEquipar.Text = "Equipar clon";
            btnEquipar.UseVisualStyleBackColor = true;
            btnEquipar.Click += btnEquipar_Click;
            //
            // lblDetalle
            //
            lblDetalle.Location = new Point(10, 154);
            lblDetalle.Name = "lblDetalle";
            lblDetalle.Size = new Size(516, 52);
            lblDetalle.TabIndex = 4;
            lblDetalle.Text = "El clon tiene su propia lista de equipo: equiparlo no cambia al prototipo original.";
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(448, 450);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 28);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FormLaboratorio
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(560, 490);
            Controls.Add(lblCreditos);
            Controls.Add(grpPrototipos);
            Controls.Add(grpEquipar);
            Controls.Add(btnCerrar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLaboratorio";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Laboratorio de clonación  [Prototype]";
            grpPrototipos.ResumeLayout(false);
            grpEquipar.ResumeLayout(false);
            grpEquipar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCreditos;
        private GroupBox grpPrototipos;
        private ListBox lstPrototipos;
        private Button btnClonar;
        private GroupBox grpEquipar;
        private ListBox lstClones;
        private Label lblEquipo;
        private ComboBox cmbEquipo;
        private Button btnEquipar;
        private Label lblDetalle;
        private Button btnCerrar;
    }
}

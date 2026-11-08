namespace Kepler7.Game
{
    partial class FormControlCentral
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
            btnIniciarTurno = new Button();
            btnEmergencia = new Button();
            btnApagar = new Button();
            txtPasos = new TextBox();
            btnCerrar = new Button();
            SuspendLayout();
            //
            // lblExplicacion
            //
            lblExplicacion.Location = new Point(12, 9);
            lblExplicacion.Name = "lblExplicacion";
            lblExplicacion.Size = new Size(456, 34);
            lblExplicacion.TabIndex = 0;
            lblExplicacion.Text = "Las tres operaciones de la fachada coordinan a los 6 subsistemas de la estación. Abajo se ve la secuencia de pasos de cada una.";
            //
            // btnIniciarTurno
            //
            btnIniciarTurno.Location = new Point(12, 48);
            btnIniciarTurno.Name = "btnIniciarTurno";
            btnIniciarTurno.Size = new Size(146, 32);
            btnIniciarTurno.TabIndex = 1;
            btnIniciarTurno.Text = "Iniciar turno";
            btnIniciarTurno.UseVisualStyleBackColor = true;
            btnIniciarTurno.Click += btnIniciarTurno_Click;
            //
            // btnEmergencia
            //
            btnEmergencia.Location = new Point(167, 48);
            btnEmergencia.Name = "btnEmergencia";
            btnEmergencia.Size = new Size(146, 32);
            btnEmergencia.TabIndex = 2;
            btnEmergencia.Text = "Emergencia";
            btnEmergencia.UseVisualStyleBackColor = true;
            btnEmergencia.Click += btnEmergencia_Click;
            //
            // btnApagar
            //
            btnApagar.Location = new Point(322, 48);
            btnApagar.Name = "btnApagar";
            btnApagar.Size = new Size(146, 32);
            btnApagar.TabIndex = 3;
            btnApagar.Text = "Apagar estación";
            btnApagar.UseVisualStyleBackColor = true;
            btnApagar.Click += btnApagar_Click;
            //
            // txtPasos
            //
            txtPasos.Font = new Font("Consolas", 8.25F);
            txtPasos.Location = new Point(12, 88);
            txtPasos.Multiline = true;
            txtPasos.Name = "txtPasos";
            txtPasos.ReadOnly = true;
            txtPasos.ScrollBars = ScrollBars.Vertical;
            txtPasos.Size = new Size(456, 240);
            txtPasos.TabIndex = 4;
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(368, 338);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 28);
            btnCerrar.TabIndex = 5;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FormControlCentral
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(480, 378);
            Controls.Add(lblExplicacion);
            Controls.Add(btnIniciarTurno);
            Controls.Add(btnEmergencia);
            Controls.Add(btnApagar);
            Controls.Add(txtPasos);
            Controls.Add(btnCerrar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormControlCentral";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Control central  [Facade]";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblExplicacion;
        private Button btnIniciarTurno;
        private Button btnEmergencia;
        private Button btnApagar;
        private TextBox txtPasos;
        private Button btnCerrar;
    }
}

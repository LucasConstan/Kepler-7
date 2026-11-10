namespace Kepler7.Game
{
    partial class FormResumen
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitulo = new Label();
            lblMotivo = new Label();
            grpResumen = new GroupBox();
            lblDatos = new Label();
            grpRecompensa = new GroupBox();
            lblRecompensa = new Label();
            btnJugarDeNuevo = new Button();
            btnCerrar = new Button();
            timerConteo = new System.Windows.Forms.Timer(components);
            grpResumen.SuspendLayout();
            grpRecompensa.SuspendLayout();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(420, 44);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Fin de la partida";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblMotivo
            //
            lblMotivo.Font = new Font("Segoe UI", 10F);
            lblMotivo.Location = new Point(12, 58);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.Size = new Size(420, 40);
            lblMotivo.TabIndex = 1;
            lblMotivo.Text = "Motivo";
            lblMotivo.TextAlign = ContentAlignment.TopCenter;
            //
            // grpResumen
            //
            grpResumen.Controls.Add(lblDatos);
            grpResumen.Location = new Point(12, 102);
            grpResumen.Name = "grpResumen";
            grpResumen.Size = new Size(420, 170);
            grpResumen.TabIndex = 2;
            grpResumen.TabStop = false;
            grpResumen.Text = "Resumen de la partida";
            //
            // lblDatos
            //
            lblDatos.Font = new Font("Segoe UI", 10F);
            lblDatos.Location = new Point(12, 24);
            lblDatos.Name = "lblDatos";
            lblDatos.Size = new Size(396, 138);
            lblDatos.TabIndex = 0;
            //
            // grpRecompensa
            //
            grpRecompensa.Controls.Add(lblRecompensa);
            grpRecompensa.Location = new Point(12, 280);
            grpRecompensa.Name = "grpRecompensa";
            grpRecompensa.Size = new Size(420, 126);
            grpRecompensa.TabIndex = 3;
            grpRecompensa.TabStop = false;
            grpRecompensa.Text = "Recompensa en el Hub";
            //
            // lblRecompensa
            //
            lblRecompensa.Location = new Point(12, 22);
            lblRecompensa.Name = "lblRecompensa";
            lblRecompensa.Size = new Size(396, 96);
            lblRecompensa.TabIndex = 0;
            //
            // btnJugarDeNuevo
            //
            btnJugarDeNuevo.DialogResult = DialogResult.Retry;
            btnJugarDeNuevo.Location = new Point(12, 416);
            btnJugarDeNuevo.Name = "btnJugarDeNuevo";
            btnJugarDeNuevo.Size = new Size(205, 32);
            btnJugarDeNuevo.TabIndex = 4;
            btnJugarDeNuevo.Text = "Jugar de nuevo";
            btnJugarDeNuevo.UseVisualStyleBackColor = true;
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(227, 416);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(205, 32);
            btnCerrar.TabIndex = 5;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // timerConteo
            //
            timerConteo.Interval = 40;
            timerConteo.Tick += timerConteo_Tick;
            //
            // FormResumen
            //
            AcceptButton = btnJugarDeNuevo;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(444, 460);
            Controls.Add(lblTitulo);
            Controls.Add(lblMotivo);
            Controls.Add(grpResumen);
            Controls.Add(grpRecompensa);
            Controls.Add(btnJugarDeNuevo);
            Controls.Add(btnCerrar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormResumen";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Fin de la partida";
            Shown += FormResumen_Shown;
            grpResumen.ResumeLayout(false);
            grpRecompensa.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private Label lblMotivo;
        private GroupBox grpResumen;
        private Label lblDatos;
        private GroupBox grpRecompensa;
        private Label lblRecompensa;
        private Button btnJugarDeNuevo;
        private Button btnCerrar;
        private System.Windows.Forms.Timer timerConteo;
    }
}

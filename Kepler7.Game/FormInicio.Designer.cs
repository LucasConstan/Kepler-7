namespace Kepler7.Game
{
    partial class FormInicio
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
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            btnNueva = new Button();
            btnContinuar = new Button();
            lblGuardada = new Label();
            btnComoJugar = new Button();
            btnSalir = new Button();
            lblIntegrantes = new Label();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.Gold;
            lblTitulo.Location = new Point(12, 24);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(436, 60);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "KEPLER-7";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblSubtitulo
            //
            lblSubtitulo.Font = new Font("Segoe UI", 11F);
            lblSubtitulo.ForeColor = Color.Gainsboro;
            lblSubtitulo.Location = new Point(12, 86);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(436, 24);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Defensa de la estación espacial por carriles";
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnNueva
            //
            btnNueva.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNueva.Location = new Point(130, 132);
            btnNueva.Name = "btnNueva";
            btnNueva.Size = new Size(200, 36);
            btnNueva.TabIndex = 2;
            btnNueva.Text = "Nueva partida";
            btnNueva.UseVisualStyleBackColor = true;
            btnNueva.Click += btnNueva_Click;
            //
            // btnContinuar
            //
            btnContinuar.Font = new Font("Segoe UI", 10F);
            btnContinuar.Location = new Point(130, 176);
            btnContinuar.Name = "btnContinuar";
            btnContinuar.Size = new Size(200, 36);
            btnContinuar.TabIndex = 3;
            btnContinuar.Text = "Continuar partida";
            btnContinuar.UseVisualStyleBackColor = true;
            btnContinuar.Click += btnContinuar_Click;
            //
            // lblGuardada
            //
            lblGuardada.Font = new Font("Segoe UI", 8.5F);
            lblGuardada.ForeColor = Color.Silver;
            lblGuardada.Location = new Point(12, 214);
            lblGuardada.Name = "lblGuardada";
            lblGuardada.Size = new Size(436, 18);
            lblGuardada.TabIndex = 4;
            lblGuardada.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnComoJugar
            //
            btnComoJugar.Font = new Font("Segoe UI", 10F);
            btnComoJugar.Location = new Point(130, 244);
            btnComoJugar.Name = "btnComoJugar";
            btnComoJugar.Size = new Size(200, 36);
            btnComoJugar.TabIndex = 5;
            btnComoJugar.Text = "Cómo jugar";
            btnComoJugar.UseVisualStyleBackColor = true;
            btnComoJugar.Click += btnComoJugar_Click;
            //
            // btnSalir
            //
            btnSalir.DialogResult = DialogResult.Cancel;
            btnSalir.Font = new Font("Segoe UI", 10F);
            btnSalir.Location = new Point(130, 288);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(200, 36);
            btnSalir.TabIndex = 6;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            //
            // lblIntegrantes
            //
            lblIntegrantes.Font = new Font("Segoe UI", 9F);
            lblIntegrantes.ForeColor = Color.White;
            lblIntegrantes.Location = new Point(12, 344);
            lblIntegrantes.Name = "lblIntegrantes";
            lblIntegrantes.Size = new Size(436, 48);
            lblIntegrantes.TabIndex = 7;
            lblIntegrantes.Text = "Integrantes";
            lblIntegrantes.TextAlign = ContentAlignment.MiddleCenter;
            //
            // FormInicio
            //
            AcceptButton = btnNueva;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 44, 56);
            CancelButton = btnSalir;
            ClientSize = new Size(460, 408);
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(btnNueva);
            Controls.Add(btnContinuar);
            Controls.Add(lblGuardada);
            Controls.Add(btnComoJugar);
            Controls.Add(btnSalir);
            Controls.Add(lblIntegrantes);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormInicio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kepler-7";
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Button btnNueva;
        private Button btnContinuar;
        private Label lblGuardada;
        private Button btnComoJugar;
        private Button btnSalir;
        private Label lblIntegrantes;
    }
}

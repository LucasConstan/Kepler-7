namespace Kepler7.Game
{
    partial class FormAviso
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
            lblTexto = new Label();
            timerAnimacion = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.Gold;
            lblTitulo.Location = new Point(12, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(296, 20);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "★ Logro desbloqueado";
            //
            // lblTexto
            //
            lblTexto.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTexto.ForeColor = Color.White;
            lblTexto.Location = new Point(12, 32);
            lblTexto.Name = "lblTexto";
            lblTexto.Size = new Size(296, 28);
            lblTexto.TabIndex = 1;
            lblTexto.Text = "Nombre del logro";
            //
            // timerAnimacion
            //
            timerAnimacion.Interval = 30;
            timerAnimacion.Tick += timerAnimacion_Tick;
            //
            // FormAviso
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 44, 56);
            ClientSize = new Size(320, 70);
            Controls.Add(lblTitulo);
            Controls.Add(lblTexto);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormAviso";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Logro desbloqueado";
            FormClosed += FormAviso_FormClosed;
            Shown += FormAviso_Shown;
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private Label lblTexto;
        private System.Windows.Forms.Timer timerAnimacion;
    }
}

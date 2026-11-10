namespace Kepler7.Game
{
    partial class FormComoJugar
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
            lblPaso = new Label();
            lblTitulo = new Label();
            lblTexto = new Label();
            btnAnterior = new Button();
            btnSiguiente = new Button();
            btnCerrar = new Button();
            SuspendLayout();
            //
            // lblPaso
            //
            lblPaso.ForeColor = Color.DimGray;
            lblPaso.Location = new Point(14, 12);
            lblPaso.Name = "lblPaso";
            lblPaso.Size = new Size(200, 18);
            lblPaso.TabIndex = 0;
            lblPaso.Text = "Paso 1 de 7";
            //
            // lblTitulo
            //
            lblTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitulo.Location = new Point(12, 32);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(556, 34);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Título";
            //
            // lblTexto
            //
            lblTexto.Font = new Font("Segoe UI", 10F);
            lblTexto.Location = new Point(14, 74);
            lblTexto.Name = "lblTexto";
            lblTexto.Size = new Size(554, 300);
            lblTexto.TabIndex = 2;
            lblTexto.Text = "Texto";
            //
            // btnAnterior
            //
            btnAnterior.Location = new Point(12, 388);
            btnAnterior.Name = "btnAnterior";
            btnAnterior.Size = new Size(120, 32);
            btnAnterior.TabIndex = 3;
            btnAnterior.Text = "Anterior";
            btnAnterior.UseVisualStyleBackColor = true;
            btnAnterior.Click += btnAnterior_Click;
            //
            // btnSiguiente
            //
            btnSiguiente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSiguiente.Location = new Point(138, 388);
            btnSiguiente.Name = "btnSiguiente";
            btnSiguiente.Size = new Size(120, 32);
            btnSiguiente.TabIndex = 4;
            btnSiguiente.Text = "Siguiente";
            btnSiguiente.UseVisualStyleBackColor = true;
            btnSiguiente.Click += btnSiguiente_Click;
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(448, 388);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(120, 32);
            btnCerrar.TabIndex = 5;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FormComoJugar
            //
            AcceptButton = btnSiguiente;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(580, 432);
            Controls.Add(lblPaso);
            Controls.Add(lblTitulo);
            Controls.Add(lblTexto);
            Controls.Add(btnAnterior);
            Controls.Add(btnSiguiente);
            Controls.Add(btnCerrar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormComoJugar";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cómo jugar Kepler-7";
            ResumeLayout(false);
        }

        #endregion

        private Label lblPaso;
        private Label lblTitulo;
        private Label lblTexto;
        private Button btnAnterior;
        private Button btnSiguiente;
        private Button btnCerrar;
    }
}

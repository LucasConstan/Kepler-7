namespace Kepler7.Game
{
    partial class FormInventario
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
            dgvItems = new DataGridView();
            colNombre = new DataGridViewTextBoxColumn();
            colTipo = new DataGridViewTextBoxColumn();
            colEfecto = new DataGridViewTextBoxColumn();
            colValor = new DataGridViewTextBoxColumn();
            btnEquipar = new Button();
            btnVender = new Button();
            lblValorTotal = new Label();
            grpHeroe = new GroupBox();
            lblHeroe = new Label();
            btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            grpHeroe.SuspendLayout();
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
            // dgvItems
            //
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.AllowUserToResizeRows = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Columns.AddRange(new DataGridViewColumn[] { colNombre, colTipo, colEfecto, colValor });
            dgvItems.Location = new Point(12, 32);
            dgvItems.MultiSelect = false;
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersVisible = false;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Size = new Size(596, 190);
            dgvItems.TabIndex = 1;
            //
            // colNombre
            //
            colNombre.FillWeight = 32F;
            colNombre.HeaderText = "Ítem";
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            //
            // colTipo
            //
            colTipo.FillWeight = 16F;
            colTipo.HeaderText = "Tipo";
            colTipo.Name = "colTipo";
            colTipo.ReadOnly = true;
            //
            // colEfecto
            //
            colEfecto.FillWeight = 40F;
            colEfecto.HeaderText = "Efecto";
            colEfecto.Name = "colEfecto";
            colEfecto.ReadOnly = true;
            //
            // colValor
            //
            colValor.FillWeight = 12F;
            colValor.HeaderText = "Valor";
            colValor.Name = "colValor";
            colValor.ReadOnly = true;
            //
            // btnEquipar
            //
            btnEquipar.Location = new Point(12, 230);
            btnEquipar.Name = "btnEquipar";
            btnEquipar.Size = new Size(140, 30);
            btnEquipar.TabIndex = 2;
            btnEquipar.Text = "Equipar";
            btnEquipar.UseVisualStyleBackColor = true;
            btnEquipar.Click += btnEquipar_Click;
            //
            // btnVender
            //
            btnVender.Location = new Point(160, 230);
            btnVender.Name = "btnVender";
            btnVender.Size = new Size(140, 30);
            btnVender.TabIndex = 3;
            btnVender.Text = "Vender";
            btnVender.UseVisualStyleBackColor = true;
            btnVender.Click += btnVender_Click;
            //
            // lblValorTotal
            //
            lblValorTotal.AutoSize = true;
            lblValorTotal.Location = new Point(320, 237);
            lblValorTotal.Name = "lblValorTotal";
            lblValorTotal.Size = new Size(160, 15);
            lblValorTotal.TabIndex = 4;
            lblValorTotal.Text = "Valor total del inventario: 0 cr";
            //
            // grpHeroe
            //
            grpHeroe.Controls.Add(lblHeroe);
            grpHeroe.Location = new Point(12, 268);
            grpHeroe.Name = "grpHeroe";
            grpHeroe.Size = new Size(596, 100);
            grpHeroe.TabIndex = 5;
            grpHeroe.TabStop = false;
            grpHeroe.Text = "Comandante";
            //
            // lblHeroe
            //
            lblHeroe.Location = new Point(10, 20);
            lblHeroe.Name = "lblHeroe";
            lblHeroe.Size = new Size(576, 72);
            lblHeroe.TabIndex = 0;
            lblHeroe.Text = "Datos del Comandante";
            //
            // btnCerrar
            //
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Location = new Point(508, 376);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(100, 28);
            btnCerrar.TabIndex = 6;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            //
            // FormInventario
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(620, 414);
            Controls.Add(lblCreditos);
            Controls.Add(dgvItems);
            Controls.Add(btnEquipar);
            Controls.Add(btnVender);
            Controls.Add(lblValorTotal);
            Controls.Add(grpHeroe);
            Controls.Add(btnCerrar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormInventario";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Inventario  [Visitor]";
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            grpHeroe.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCreditos;
        private DataGridView dgvItems;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colTipo;
        private DataGridViewTextBoxColumn colEfecto;
        private DataGridViewTextBoxColumn colValor;
        private Button btnEquipar;
        private Button btnVender;
        private Label lblValorTotal;
        private GroupBox grpHeroe;
        private Label lblHeroe;
        private Button btnCerrar;
    }
}

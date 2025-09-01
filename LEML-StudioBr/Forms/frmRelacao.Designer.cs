namespace LEML_StudioBr.Forms
{
    partial class frmRelacao
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnCancelar = new Button();
            btnConfirmar = new Button();
            label2 = new Label();
            label1 = new Label();
            label3 = new Label();
            cboOrigem = new ComboBox();
            cboDestino = new ComboBox();
            cboTipo = new ComboBox();
            label4 = new Label();
            cboCardSource = new ComboBox();
            cboCardTarget = new ComboBox();
            SuspendLayout();
            // 
            // btnCancelar
            // 
            btnCancelar.Image = Properties.Resources.cancel_btn;
            btnCancelar.ImageAlign = ContentAlignment.MiddleLeft;
            btnCancelar.Location = new Point(12, 221);
            btnCancelar.Margin = new Padding(0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(148, 64);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnConfirmar
            // 
            btnConfirmar.Image = Properties.Resources.ok_btn;
            btnConfirmar.ImageAlign = ContentAlignment.MiddleLeft;
            btnConfirmar.Location = new Point(515, 223);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(156, 62);
            btnConfirmar.TabIndex = 6;
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.UseVisualStyleBackColor = true;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 89);
            label2.Name = "label2";
            label2.Size = new Size(63, 20);
            label2.TabIndex = 8;
            label2.Text = "Destino";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 33);
            label1.Name = "label1";
            label1.Size = new Size(61, 20);
            label1.TabIndex = 7;
            label1.Text = "Origem";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 143);
            label3.Name = "label3";
            label3.Size = new Size(40, 20);
            label3.TabIndex = 9;
            label3.Text = "Tipo";
            // 
            // cboOrigem
            // 
            cboOrigem.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOrigem.FormattingEnabled = true;
            cboOrigem.Location = new Point(103, 30);
            cboOrigem.Name = "cboOrigem";
            cboOrigem.Size = new Size(396, 28);
            cboOrigem.TabIndex = 10;
            // 
            // cboDestino
            // 
            cboDestino.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDestino.FormattingEnabled = true;
            cboDestino.Location = new Point(103, 81);
            cboDestino.Name = "cboDestino";
            cboDestino.Size = new Size(396, 28);
            cboDestino.TabIndex = 11;
            // 
            // cboTipo
            // 
            cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTipo.FormattingEnabled = true;
            cboTipo.Items.AddRange(new object[] { "Ação", "Condição" });
            cboTipo.Location = new Point(103, 136);
            cboTipo.Name = "cboTipo";
            cboTipo.Size = new Size(396, 28);
            cboTipo.TabIndex = 12;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(515, 7);
            label4.Name = "label4";
            label4.Size = new Size(104, 20);
            label4.TabIndex = 13;
            label4.Text = "Cardinalidade";
            // 
            // cboCardSource
            // 
            cboCardSource.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCardSource.FormattingEnabled = true;
            cboCardSource.Items.AddRange(new object[] { "0", "1", "N" });
            cboCardSource.Location = new Point(541, 30);
            cboCardSource.Name = "cboCardSource";
            cboCardSource.Size = new Size(50, 28);
            cboCardSource.TabIndex = 14;
            // 
            // cboCardTarget
            // 
            cboCardTarget.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCardTarget.FormattingEnabled = true;
            cboCardTarget.Items.AddRange(new object[] { "0", "1", "N" });
            cboCardTarget.Location = new Point(541, 81);
            cboCardTarget.Name = "cboCardTarget";
            cboCardTarget.Size = new Size(50, 28);
            cboCardTarget.TabIndex = 15;
            // 
            // frmRelacao
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(683, 297);
            ControlBox = false;
            Controls.Add(cboCardTarget);
            Controls.Add(cboCardSource);
            Controls.Add(label4);
            Controls.Add(cboTipo);
            Controls.Add(cboDestino);
            Controls.Add(cboOrigem);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnCancelar);
            Controls.Add(btnConfirmar);
            Name = "frmRelacao";
            Text = "Relação entre os Elementos";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCancelar;
        private Button btnConfirmar;
        private Label label2;
        private Label label1;
        private Label label3;
        private ComboBox cboOrigem;
        private ComboBox cboDestino;
        private ComboBox cboTipo;
        private Label label4;
        private ComboBox cboCardSource;
        private ComboBox cboCardTarget;
    }
}
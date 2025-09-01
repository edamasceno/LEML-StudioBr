namespace LEML_StudioBr.Forms
{
    partial class frmInfoElemento
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
            btnConfirmar = new Button();
            btnCancelar = new Button();
            label1 = new Label();
            label2 = new Label();
            txtWhat = new TextBox();
            txtHow = new TextBox();
            SuspendLayout();
            // 
            // btnConfirmar
            // 
            btnConfirmar.Image = Properties.Resources.ok_btn;
            btnConfirmar.ImageAlign = ContentAlignment.MiddleLeft;
            btnConfirmar.Location = new Point(431, 162);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(156, 62);
            btnConfirmar.TabIndex = 4;
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.UseVisualStyleBackColor = true;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Image = Properties.Resources.cancel_btn;
            btnCancelar.ImageAlign = ContentAlignment.MiddleLeft;
            btnCancelar.Location = new Point(12, 162);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(148, 64);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(17, 19);
            label1.Name = "label1";
            label1.Size = new Size(91, 20);
            label1.TabIndex = 2;
            label1.Text = "O que Fazer";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(17, 90);
            label2.Name = "label2";
            label2.Size = new Size(91, 20);
            label2.TabIndex = 3;
            label2.Text = "Como Fazer";
            // 
            // txtWhat
            // 
            txtWhat.Location = new Point(131, 19);
            txtWhat.MaxLength = 50;
            txtWhat.Name = "txtWhat";
            txtWhat.Size = new Size(438, 27);
            txtWhat.TabIndex = 1;
            txtWhat.Enter += txtWhat_Enter;
            txtWhat.Leave += txtWhat_Leave;
            // 
            // txtHow
            // 
            txtHow.Location = new Point(131, 87);
            txtHow.MaxLength = 50;
            txtHow.Name = "txtHow";
            txtHow.Size = new Size(438, 27);
            txtHow.TabIndex = 2;
            txtHow.Enter += txtWhat_Enter;
            txtHow.Leave += txtWhat_Leave;
            // 
            // frmInfoElemento
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(599, 236);
            ControlBox = false;
            Controls.Add(txtHow);
            Controls.Add(txtWhat);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnCancelar);
            Controls.Add(btnConfirmar);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "frmInfoElemento";
            RightToLeftLayout = true;
            ShowInTaskbar = false;
            Text = "Informação";
            Load += frmInfoElemento_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnConfirmar;
        private Button btnCancelar;
        private Label label1;
        private Label label2;
        private TextBox txtWhat;
        private TextBox txtHow;
    }
}
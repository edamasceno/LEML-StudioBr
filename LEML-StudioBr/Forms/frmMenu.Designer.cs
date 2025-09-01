namespace LEML_StudioBr
{
    partial class frmMenu
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMenu));
            BarraOpcoes = new SplitContainer();
            groupBox2 = new GroupBox();
            btnAgent = new Button();
            btnStarEnd = new Button();
            btnFeedBack = new Button();
            btnPratica = new Button();
            btnAtividade = new Button();
            btnDialog = new Button();
            btnInfo = new Button();
            groupBox3 = new GroupBox();
            btnAmb_Simulado = new Button();
            btnAmb_ssincrono = new Button();
            btnAmb_Sincrono = new Button();
            btnAmb_SalaFisica = new Button();
            pictureBox1 = new PictureBox();
            menuStrip1 = new MenuStrip();
            arquivoToolStripMenuItem = new ToolStripMenuItem();
            novoToolStripMenuItem = new ToolStripMenuItem();
            abirToolStripMenuItem = new ToolStripMenuItem();
            salvarToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            imprimirToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            exportarToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            fecharToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            info = new ToolStripStatusLabel();
            toolStripSplitButtonZoomIn = new ToolStripSplitButton();
            toolStripStatusLBLZoom = new ToolStripStatusLabel();
            toolStripSplitButtonZoomOut = new ToolStripSplitButton();
            Contexto = new ContextMenuStrip(components);
            toolStripMenuItem_Apagar = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripMenuItem_Ligar = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)BarraOpcoes).BeginInit();
            BarraOpcoes.Panel1.SuspendLayout();
            BarraOpcoes.Panel2.SuspendLayout();
            BarraOpcoes.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            Contexto.SuspendLayout();
            SuspendLayout();
            // 
            // BarraOpcoes
            // 
            BarraOpcoes.Dock = DockStyle.Fill;
            BarraOpcoes.Location = new Point(0, 28);
            BarraOpcoes.Name = "BarraOpcoes";
            // 
            // BarraOpcoes.Panel1
            // 
            BarraOpcoes.Panel1.Controls.Add(groupBox2);
            BarraOpcoes.Panel1.Controls.Add(groupBox3);
            // 
            // BarraOpcoes.Panel2
            // 
            BarraOpcoes.Panel2.Controls.Add(pictureBox1);
            BarraOpcoes.Size = new Size(1729, 1027);
            BarraOpcoes.SplitterDistance = 110;
            BarraOpcoes.TabIndex = 2;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnAgent);
            groupBox2.Controls.Add(btnStarEnd);
            groupBox2.Controls.Add(btnFeedBack);
            groupBox2.Controls.Add(btnPratica);
            groupBox2.Controls.Add(btnAtividade);
            groupBox2.Controls.Add(btnDialog);
            groupBox2.Controls.Add(btnInfo);
            groupBox2.Location = new Point(9, 241);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(90, 757);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Artefato";
            // 
            // btnAgent
            // 
            btnAgent.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAgent.Image = Properties.Resources.Agent_logo;
            btnAgent.ImageAlign = ContentAlignment.TopCenter;
            btnAgent.Location = new Point(5, 602);
            btnAgent.Name = "btnAgent";
            btnAgent.Size = new Size(80, 90);
            btnAgent.TabIndex = 11;
            btnAgent.Text = "Agente IA";
            btnAgent.TextAlign = ContentAlignment.BottomCenter;
            btnAgent.UseVisualStyleBackColor = true;
            btnAgent.Click += btnAgent_Click;
            // 
            // btnStarEnd
            // 
            btnStarEnd.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnStarEnd.Image = Properties.Resources.StarEnd;
            btnStarEnd.ImageAlign = ContentAlignment.TopCenter;
            btnStarEnd.Location = new Point(6, 26);
            btnStarEnd.Name = "btnStarEnd";
            btnStarEnd.Size = new Size(80, 90);
            btnStarEnd.TabIndex = 10;
            btnStarEnd.Text = "Inicio/Fim";
            btnStarEnd.TextAlign = ContentAlignment.BottomCenter;
            btnStarEnd.UseVisualStyleBackColor = true;
            btnStarEnd.Click += btnStarEnd_Click;
            // 
            // btnFeedBack
            // 
            btnFeedBack.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnFeedBack.Image = Properties.Resources.Feedback_logo;
            btnFeedBack.ImageAlign = ContentAlignment.TopCenter;
            btnFeedBack.Location = new Point(5, 506);
            btnFeedBack.Name = "btnFeedBack";
            btnFeedBack.Size = new Size(80, 90);
            btnFeedBack.TabIndex = 9;
            btnFeedBack.Text = "Feedback";
            btnFeedBack.TextAlign = ContentAlignment.BottomCenter;
            btnFeedBack.UseVisualStyleBackColor = true;
            btnFeedBack.Click += btnFeedBack_Click;
            // 
            // btnPratica
            // 
            btnPratica.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPratica.Image = Properties.Resources.Pratice_logo;
            btnPratica.ImageAlign = ContentAlignment.TopCenter;
            btnPratica.Location = new Point(6, 410);
            btnPratica.Name = "btnPratica";
            btnPratica.Size = new Size(80, 90);
            btnPratica.TabIndex = 8;
            btnPratica.Text = "Prática";
            btnPratica.TextAlign = ContentAlignment.BottomCenter;
            btnPratica.UseVisualStyleBackColor = true;
            btnPratica.Click += button3_Click;
            // 
            // btnAtividade
            // 
            btnAtividade.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAtividade.Image = Properties.Resources.Activity_logo;
            btnAtividade.ImageAlign = ContentAlignment.TopCenter;
            btnAtividade.Location = new Point(5, 314);
            btnAtividade.Name = "btnAtividade";
            btnAtividade.Size = new Size(80, 90);
            btnAtividade.TabIndex = 7;
            btnAtividade.Text = "Atividade";
            btnAtividade.TextAlign = ContentAlignment.BottomCenter;
            btnAtividade.UseVisualStyleBackColor = true;
            btnAtividade.Click += button2_Click;
            // 
            // btnDialog
            // 
            btnDialog.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDialog.Image = Properties.Resources.Dialog_logo;
            btnDialog.ImageAlign = ContentAlignment.TopCenter;
            btnDialog.Location = new Point(6, 218);
            btnDialog.Name = "btnDialog";
            btnDialog.Size = new Size(80, 90);
            btnDialog.TabIndex = 6;
            btnDialog.Text = "Diálogo";
            btnDialog.TextAlign = ContentAlignment.BottomCenter;
            btnDialog.UseVisualStyleBackColor = true;
            btnDialog.Click += btnDialog_Click;
            // 
            // btnInfo
            // 
            btnInfo.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnInfo.Image = Properties.Resources.Information_logo;
            btnInfo.ImageAlign = ContentAlignment.TopCenter;
            btnInfo.Location = new Point(6, 122);
            btnInfo.Name = "btnInfo";
            btnInfo.Size = new Size(80, 90);
            btnInfo.TabIndex = 5;
            btnInfo.Text = "Informa";
            btnInfo.TextAlign = ContentAlignment.BottomCenter;
            btnInfo.UseVisualStyleBackColor = true;
            btnInfo.Click += btnInfo_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btnAmb_Simulado);
            groupBox3.Controls.Add(btnAmb_ssincrono);
            groupBox3.Controls.Add(btnAmb_Sincrono);
            groupBox3.Controls.Add(btnAmb_SalaFisica);
            groupBox3.Location = new Point(3, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(101, 234);
            groupBox3.TabIndex = 8;
            groupBox3.TabStop = false;
            groupBox3.Text = "Ambiente";
            // 
            // btnAmb_Simulado
            // 
            btnAmb_Simulado.BackColor = Color.MediumSlateBlue;
            btnAmb_Simulado.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAmb_Simulado.Location = new Point(8, 180);
            btnAmb_Simulado.Name = "btnAmb_Simulado";
            btnAmb_Simulado.Size = new Size(85, 52);
            btnAmb_Simulado.TabIndex = 8;
            btnAmb_Simulado.Text = "Simulado Experiencial";
            btnAmb_Simulado.UseVisualStyleBackColor = false;
            btnAmb_Simulado.Click += btnAmb_Simulado_Click;
            // 
            // btnAmb_ssincrono
            // 
            btnAmb_ssincrono.BackColor = Color.SkyBlue;
            btnAmb_ssincrono.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAmb_ssincrono.Location = new Point(6, 130);
            btnAmb_ssincrono.Name = "btnAmb_ssincrono";
            btnAmb_ssincrono.Size = new Size(85, 44);
            btnAmb_ssincrono.TabIndex = 7;
            btnAmb_ssincrono.Text = "Online Assíncrono";
            btnAmb_ssincrono.UseVisualStyleBackColor = false;
            btnAmb_ssincrono.Click += btnAmb_ssincrono_Click;
            // 
            // btnAmb_Sincrono
            // 
            btnAmb_Sincrono.BackColor = Color.Aquamarine;
            btnAmb_Sincrono.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAmb_Sincrono.Location = new Point(6, 79);
            btnAmb_Sincrono.Name = "btnAmb_Sincrono";
            btnAmb_Sincrono.Size = new Size(85, 45);
            btnAmb_Sincrono.TabIndex = 6;
            btnAmb_Sincrono.Text = "Online Sincrono";
            btnAmb_Sincrono.UseVisualStyleBackColor = false;
            btnAmb_Sincrono.Click += btnAmb_Sincrono_Click;
            // 
            // btnAmb_SalaFisica
            // 
            btnAmb_SalaFisica.BackColor = Color.Cornsilk;
            btnAmb_SalaFisica.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAmb_SalaFisica.Location = new Point(6, 27);
            btnAmb_SalaFisica.Name = "btnAmb_SalaFisica";
            btnAmb_SalaFisica.Size = new Size(85, 46);
            btnAmb_SalaFisica.TabIndex = 5;
            btnAmb_SalaFisica.Text = "Sala Física Presencial";
            btnAmb_SalaFisica.UseVisualStyleBackColor = false;
            btnAmb_SalaFisica.Click += btnAmb_SalaFisica_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1615, 1027);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseClick += pictureBox1_MouseClick;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.PreviewKeyDown += pictureBox1_PreviewKeyDown;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { arquivoToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1729, 28);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // arquivoToolStripMenuItem
            // 
            arquivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { novoToolStripMenuItem, abirToolStripMenuItem, salvarToolStripMenuItem, toolStripMenuItem2, imprimirToolStripMenuItem, toolStripMenuItem3, exportarToolStripMenuItem, toolStripMenuItem1, fecharToolStripMenuItem });
            arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            arquivoToolStripMenuItem.Size = new Size(75, 24);
            arquivoToolStripMenuItem.Text = "&Arquivo";
            // 
            // novoToolStripMenuItem
            // 
            novoToolStripMenuItem.Name = "novoToolStripMenuItem";
            novoToolStripMenuItem.Size = new Size(224, 26);
            novoToolStripMenuItem.Text = "&Novo";
            novoToolStripMenuItem.Click += novoToolStripMenuItem_Click;
            // 
            // abirToolStripMenuItem
            // 
            abirToolStripMenuItem.Name = "abirToolStripMenuItem";
            abirToolStripMenuItem.Size = new Size(224, 26);
            abirToolStripMenuItem.Text = "&Abir";
            abirToolStripMenuItem.Click += abirToolStripMenuItem_Click;
            // 
            // salvarToolStripMenuItem
            // 
            salvarToolStripMenuItem.Name = "salvarToolStripMenuItem";
            salvarToolStripMenuItem.Size = new Size(224, 26);
            salvarToolStripMenuItem.Text = "&Salvar";
            salvarToolStripMenuItem.Click += salvarToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(221, 6);
            // 
            // imprimirToolStripMenuItem
            // 
            imprimirToolStripMenuItem.Name = "imprimirToolStripMenuItem";
            imprimirToolStripMenuItem.Size = new Size(224, 26);
            imprimirToolStripMenuItem.Text = "&Imprimir";
            imprimirToolStripMenuItem.Click += imprimirToolStripMenuItem_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(221, 6);
            // 
            // exportarToolStripMenuItem
            // 
            exportarToolStripMenuItem.Name = "exportarToolStripMenuItem";
            exportarToolStripMenuItem.Size = new Size(224, 26);
            exportarToolStripMenuItem.Text = "&Exportar";
            exportarToolStripMenuItem.Click += exportarToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(221, 6);
            // 
            // fecharToolStripMenuItem
            // 
            fecharToolStripMenuItem.Name = "fecharToolStripMenuItem";
            fecharToolStripMenuItem.Size = new Size(224, 26);
            fecharToolStripMenuItem.Text = "&Fechar";
            fecharToolStripMenuItem.Click += fecharToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, info, toolStripSplitButtonZoomIn, toolStripStatusLBLZoom, toolStripSplitButtonZoomOut });
            statusStrip1.Location = new Point(0, 1029);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1729, 26);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            statusStrip1.ItemClicked += statusStrip1_ItemClicked;
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(45, 20);
            toolStripStatusLabel1.Text = "INFO:";
            // 
            // info
            // 
            info.Name = "info";
            info.Size = new Size(1574, 20);
            info.Spring = true;
            info.Text = "...";
            info.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripSplitButtonZoomIn
            // 
            toolStripSplitButtonZoomIn.AutoToolTip = false;
            toolStripSplitButtonZoomIn.BackgroundImageLayout = ImageLayout.None;
            toolStripSplitButtonZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripSplitButtonZoomIn.DropDownButtonWidth = 0;
            toolStripSplitButtonZoomIn.Image = Properties.Resources.Zoom_In;
            toolStripSplitButtonZoomIn.ImageTransparentColor = Color.Magenta;
            toolStripSplitButtonZoomIn.Name = "toolStripSplitButtonZoomIn";
            toolStripSplitButtonZoomIn.Size = new Size(25, 24);
            toolStripSplitButtonZoomIn.TextImageRelation = TextImageRelation.Overlay;
            toolStripSplitButtonZoomIn.ButtonClick += toolStripSplitButtonZoomIn_ButtonClick;
            // 
            // toolStripStatusLBLZoom
            // 
            toolStripStatusLBLZoom.Name = "toolStripStatusLBLZoom";
            toolStripStatusLBLZoom.Size = new Size(45, 20);
            toolStripStatusLBLZoom.Text = "100%";
            toolStripStatusLBLZoom.DoubleClick += toolStripStatusLBLZoom_DoubleClick;
            // 
            // toolStripSplitButtonZoomOut
            // 
            toolStripSplitButtonZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripSplitButtonZoomOut.DropDownButtonWidth = 0;
            toolStripSplitButtonZoomOut.Image = Properties.Resources.Zoom_Out;
            toolStripSplitButtonZoomOut.ImageTransparentColor = Color.Magenta;
            toolStripSplitButtonZoomOut.Name = "toolStripSplitButtonZoomOut";
            toolStripSplitButtonZoomOut.Size = new Size(25, 24);
            toolStripSplitButtonZoomOut.Text = "toolStripSplitButtonZoomOut";
            toolStripSplitButtonZoomOut.ButtonClick += toolStripSplitButtonZoomOut_ButtonClick;
            // 
            // Contexto
            // 
            Contexto.ImageScalingSize = new Size(20, 20);
            Contexto.Items.AddRange(new ToolStripItem[] { toolStripMenuItem_Apagar, toolStripSeparator1, toolStripMenuItem_Ligar });
            Contexto.Name = "Contexto";
            Contexto.ShowImageMargin = false;
            Contexto.Size = new Size(112, 58);
            Contexto.Text = "Opções";
            // 
            // toolStripMenuItem_Apagar
            // 
            toolStripMenuItem_Apagar.Name = "toolStripMenuItem_Apagar";
            toolStripMenuItem_Apagar.Size = new Size(111, 24);
            toolStripMenuItem_Apagar.Text = "Apagar";
            toolStripMenuItem_Apagar.Click += toolStripMenuItem_Apagar_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(108, 6);
            // 
            // toolStripMenuItem_Ligar
            // 
            toolStripMenuItem_Ligar.Name = "toolStripMenuItem_Ligar";
            toolStripMenuItem_Ligar.Size = new Size(111, 24);
            toolStripMenuItem_Ligar.Text = "Conexão";
            toolStripMenuItem_Ligar.Click += toolStripMenuItem_Ligar_Click;
            
            // frmMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1729, 1055);
            Controls.Add(statusStrip1);
            Controls.Add(BarraOpcoes);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "frmMenu";
            Text = "LEML-StudioBr";
            WindowState = FormWindowState.Maximized;
            BarraOpcoes.Panel1.ResumeLayout(false);
            BarraOpcoes.Panel2.ResumeLayout(false);
            BarraOpcoes.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)BarraOpcoes).EndInit();
            BarraOpcoes.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            Contexto.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer BarraOpcoes;
        private PictureBox pictureBox1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem arquivoToolStripMenuItem;
        private ToolStripMenuItem novoToolStripMenuItem;
        private ToolStripMenuItem salvarToolStripMenuItem;
        private ToolStripMenuItem exportarToolStripMenuItem;
        private ToolStripMenuItem fecharToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel info;
        private GroupBox groupBox2;
        private Button btnFeedBack;
        private Button btnPratica;
        private Button btnAtividade;
        private Button btnDialog;
        private Button btnInfo;
        private GroupBox groupBox3;
        private Button btnAmb_ssincrono;
        private Button btnAmb_Sincrono;
        private Button btnAmb_SalaFisica;
        private Button btnAmb_Simulado;
        private ToolStripMenuItem imprimirToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ContextMenuStrip Contexto;
        private ToolStripMenuItem toolStripMenuItem_Apagar;
        private Button btnStarEnd;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem toolStripMenuItem_Ligar;
        private ToolStripSplitButton toolStripSplitButtonZoomIn;
        private ToolStripSplitButton toolStripSplitButtonZoomOut;
        private ToolStripStatusLabel toolStripStatusLBLZoom;
        private ToolStripMenuItem abirToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem3;
        private Button btnAgent;
    }
}

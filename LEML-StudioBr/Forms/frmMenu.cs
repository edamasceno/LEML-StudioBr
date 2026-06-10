using LEML_StudioBr.Forms;
using LEML_StudioBr.Objetos;
using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Xml.Schema;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout; 

namespace LEML_StudioBr

{
    [Serializable]
    public class ProjectData
    {
        public List<Box> Boxes { get; set; } = new List<Box>();
        public List<Connection> Connections { get; set; } = new List<Connection>();
    }

    public partial class frmMenu : Form
    {
        private Canvas _canvas;
        private Box? _selectedBox;
        private int tipoAmbiente = 0;

        private Box _firstSelectedBox = null;
        private Box _secondSelectedBox = null;
        private string _selectedRelationshipType = "";

        private Point clickPoint = Point.Empty;

        // Variáveis para controle de zoom
        private float zoomLevel = 1.0f;
        private const float ZOOM_INCREMENT = 0.2f;
        private const float MIN_ZOOM = 0.5f;
        private const float MAX_ZOOM = 3.0f;
        private PointF zoomCenter = PointF.Empty;

        private bool isPanning = false;
        private Point panStartPoint = Point.Empty;
        private PointF panOffset = PointF.Empty;

        
        private bool _modoBorracha = false;
        private Box _boxParaDesconectar = null;

        // Variáveis para Seleção Múltipla e Arrasto em Grupo
        private List<Box> _caixasSelecionadas = new List<Box>();
        private bool _desenhandoRetangulo = false;
        private Rectangle _retanguloSelecao = Rectangle.Empty;
        private Point _pontoInicioRetangulo = Point.Empty;
        private bool _arrastandoGrupo = false;
        private Point _ultimoPontoDrag = Point.Empty;


        public frmMenu()
        {
            if (PdfSharp.Fonts.GlobalFontSettings.FontResolver == null)
            {
                PdfSharp.Fonts.GlobalFontSettings.FontResolver = new CustomFontResolver();
            }
            _canvas = new Canvas();
            InitializeComponent();

            BarraOpcoes.Panel1.AutoScroll = true;

            ToolStripMenuItem toolStripMenuItem_Editar = new ToolStripMenuItem("Editar");
            toolStripMenuItem_Editar.Click += toolStripMenuItem_Editar_Click;
            Contexto.Items.Insert(0, toolStripMenuItem_Editar);


            ToolStripMenuItem modelosMenu = new ToolStripMenuItem("Modelos Pedagógicos");

            // Cria as opções do menu dropdown
            ToolStripMenuItem optInvertida = new ToolStripMenuItem("Sala de Aula Invertida", null, CarregarTemplateInvertida);
            ToolStripMenuItem optPbl = new ToolStripMenuItem("Aprendizagem Baseada em Problemas (PBL)", null, CarregarTemplatePBL);

            // Adiciona as opções dentro do menu principal
            modelosMenu.DropDownItems.Add(optInvertida);
            modelosMenu.DropDownItems.Add(optPbl);

            // Injeta o novo menu na barra superior do seu formulário
            menuStrip1.Items.Add(modelosMenu);

            GroupBox groupBoxFerramentas = new GroupBox();
            groupBoxFerramentas.Name = "groupBoxFerramentas";
            groupBoxFerramentas.Text = "Ferramentas";
            groupBoxFerramentas.Size = new Size(90, 260);

           
            groupBoxFerramentas.Location = new Point(9, 760);

          
            Button btnBorracha = new Button();
            btnBorracha.Name = "btnBorracha";
            btnBorracha.Size = new Size(60, 60);
            btnBorracha.Location = new Point(15, 25); 

           
            btnBorracha.BackColor = Color.Transparent;
            btnBorracha.FlatStyle = FlatStyle.Flat;
            btnBorracha.FlatAppearance.BorderSize = 0;
            btnBorracha.Cursor = Cursors.Hand;
            btnBorracha.Image = Properties.Resources.borracha; 
            btnBorracha.ImageAlign = ContentAlignment.MiddleCenter;
            btnBorracha.Text = "";

            ToolTip dicaBorracha = new ToolTip();
            dicaBorracha.SetToolTip(btnBorracha, "Ferramenta Borracha: Apagar conexões");

           
            btnBorracha.Click += new EventHandler(this.btnBorracha_Click);

           
            groupBoxFerramentas.Controls.Add(btnBorracha);
            BarraOpcoes.Panel1.Controls.Add(groupBoxFerramentas);

            
            groupBoxFerramentas.BringToFront();

            
            Button btnPdf = new Button();
            btnPdf.Name = "btnPdf";
            btnPdf.Size = new Size(60, 60);
            btnPdf.Location = new Point(15, 95); 

            btnPdf.BackColor = Color.Transparent;
            btnPdf.FlatStyle = FlatStyle.Flat;
            btnPdf.FlatAppearance.BorderSize = 0;
            btnPdf.Cursor = Cursors.Hand;

            
            btnPdf.Image = Properties.Resources.impressora; 
            btnPdf.Text = ""; 
            btnPdf.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnPdf.ForeColor = Color.DarkRed;

            ToolTip dicaPdf = new ToolTip();
            dicaPdf.SetToolTip(btnPdf, "Gerar Plano de Aula em PDF");

            
            btnPdf.Click += (s, ev) => ExportToPdf();

         
            groupBoxFerramentas.Controls.Add(btnPdf);

            Button btnValidar = new Button();
            btnValidar.Name = "btnValidar";
            btnValidar.Size = new Size(60, 60);
            btnValidar.Location = new Point(15, 165);
            btnValidar.Image = Properties.Resources.verificado;
            btnValidar.BackColor = Color.Transparent; 
            btnValidar.FlatStyle = FlatStyle.Flat;
            btnValidar.FlatAppearance.BorderSize = 0;
            btnValidar.Cursor = Cursors.Hand;
            btnValidar.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            btnValidar.Text = ""; 

            ToolTip dicaValidar = new ToolTip();
            dicaValidar.SetToolTip(btnValidar, "Analisar a coerência do Design Instrucional");

            
            btnValidar.Click += (s, ev) => ValidarModeloPedagogico(true);

            groupBoxFerramentas.Controls.Add(btnValidar);


            pictureBox1.MouseUp += pictureBox1_MouseUp;

            ConfigurePictureBoxWithScroll();
            pictureBox1.Focus();
           
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                bool interceptado = false;

                if (!string.IsNullOrEmpty(_selectedRelationshipType))
                {
                    CancelConnectionCreation();
                    interceptado = true;
                }

                if (_modoBorracha)
                {
                    _boxParaDesconectar?.Unselect();
                    _modoBorracha = false;
                    _boxParaDesconectar = null;
                    pictureBox1.Cursor = Cursors.Default;
                    info.Text = "Ação cancelada.";
                    pictureBox1.Invalidate();
                    interceptado = true;
                }

                if (interceptado) return true;
            }
            // ==========================================
            // === NOVO: INTERCEPTA A TECLA DELETE ===
            // ==========================================
            else if (keyData == Keys.Delete)
            {
                // Verifica se existe uma seleção múltipla (ou uma única caixa clicada)
                if (_caixasSelecionadas.Count > 0)
                {
                    var result = MessageBox.Show($"Deseja excluir permanentemente {_caixasSelecionadas.Count} elemento(s)?",
                                                 "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // Usa .ToList() para criar uma cópia de segurança e não quebrar o loop enquanto remove
                        foreach (var box in _caixasSelecionadas.ToList())
                        {
                            _canvas.RemoveBoxFromList(box);
                        }
                        LimparMultiSelecao();
                        pictureBox1.Invalidate();
                    }
                    return true; // Bloqueia a tecla para que não faça mais nada
                }
                // Fallback: Caso o professor tenha selecionado uma caixa com o botão direito (menu de contexto)
                else if (_selectedBox != null)
                {
                    var result = MessageBox.Show($"Deseja apagar o artefato '{_selectedBox.OriginalName}'?",
                                                 "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        _canvas.RemoveBoxFromList(_selectedBox);
                        _selectedBox = null;
                        pictureBox1.Invalidate();
                    }
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private Point GetRelativeCursorPos()
        {
            Point screenPosition = Cursor.Position;
            Point relativePosition = pictureBox1.PointToClient(screenPosition);
            return relativePosition;
        }

        private void fecharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Point relativePosition = GetRelativeCursorPos();

            if (tipoAmbiente == 0)
            {
                info.Text = ("Selecione um tipo de ambiente antes de adicionar ao plano.");
                return;
            }
            else if (tipoAmbiente <= 4) 
            {
                Box box = new ClassBox(relativePosition.X, relativePosition.Y, info.Text, this.tipoAmbiente);
                box.PositionX -= box.Width / 2;
                box.PositionY -= box.Height / 2;
                _canvas.AddBoxToList(box);
            }
            else if (tipoAmbiente == 15) 
            {
                Agente ag = (Agente)new Elemento(relativePosition.X, relativePosition.Y, info.Text, this.tipoAmbiente);
                ag.PositionX -= ag.Width / 2;
                ag.PositionY -= ag.Height / 2;

                using (frmInfoAgente frmInfoAgente = new frmInfoAgente(ag))
                {
                    if (frmInfoAgente.ShowDialog() == DialogResult.OK)
                    {
                        ag = (Agente)frmInfoAgente.Result;
                    }
                }
                _canvas.AddBoxToList(ag);
            }
            else 
            {
                Elemento ele = new Elemento(relativePosition.X, relativePosition.Y, info.Text, this.tipoAmbiente);
                ele.PositionX -= ele.Width / 2;
                ele.PositionY -= ele.Height / 2;
                if (tipoAmbiente != 20)
                {
                    using (frmInfoElemento frmInfoElemento = new frmInfoElemento(ele))
                    {
                        if (frmInfoElemento.ShowDialog() == DialogResult.OK)
                        {
                            ele = frmInfoElemento.Result;
                        }
                    }
                }
                if (tipoAmbiente == 20)
                {
                    ele.SetElemento("Início", "Fim");
                }
                _canvas.AddBoxToList(ele);
            }

            tipoAmbiente = 0; 
            info.Text = "";
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
           
            if (_modoBorracha || !string.IsNullOrEmpty(_selectedRelationshipType)) return;

            if (e.Button == MouseButtons.Left)
            {
                
                Box caixaClicada = null;
                var caixas = _canvas.GetBoxes();
                for (int i = caixas.Count - 1; i >= 0; i--)
                {
                    if (caixas[i].IsInCollision(e.X, e.Y) || caixas[i].IsInCollisionWithCorner(e.X, e.Y))
                    {
                        caixaClicada = caixas[i];
                        break;
                    }
                }

                if (caixaClicada != null)
                {
                    
                    if (_caixasSelecionadas.Contains(caixaClicada))
                    {
                        _arrastandoGrupo = true;
                        _ultimoPontoDrag = new Point(e.X, e.Y);
                    }
                    else
                    {
                        
                        LimparMultiSelecao();
                        _canvas.Select(e.X, e.Y);
                        _caixasSelecionadas.Add(caixaClicada);
                    }
                }
                else
                {
                  
                    LimparMultiSelecao();
                    _canvas.Select(e.X, e.Y); 

                    _desenhandoRetangulo = true;
                    _pontoInicioRetangulo = new Point(e.X, e.Y);
                    _retanguloSelecao = new Rectangle(e.X, e.Y, 0, 0);
                }
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_arrastandoGrupo)
            {
                
                float deltaX = (e.X - _ultimoPontoDrag.X) / zoomLevel;
                float deltaY = (e.Y - _ultimoPontoDrag.Y) / zoomLevel;

               
                foreach (var box in _caixasSelecionadas)
                {
                    box.PositionX += deltaX;
                    box.PositionY += deltaY;
                }

                _ultimoPontoDrag = new Point(e.X, e.Y);
                pictureBox1.Refresh();
            }
            else if (_desenhandoRetangulo)
            {
                
                int x = Math.Min(e.X, _pontoInicioRetangulo.X);
                int y = Math.Min(e.Y, _pontoInicioRetangulo.Y);
                int width = Math.Abs(e.X - _pontoInicioRetangulo.X);
                int height = Math.Abs(e.Y - _pontoInicioRetangulo.Y);

                _retanguloSelecao = new Rectangle(x, y, width, height);
                pictureBox1.Refresh();
            }
            else if (e.Button == MouseButtons.Left && !_desenhandoRetangulo && !_arrastandoGrupo)
            {
              
                _canvas.Move(e.X, e.Y);
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (_arrastandoGrupo)
            {
                _arrastandoGrupo = false;
            }
            else if (_desenhandoRetangulo)
            {
                _desenhandoRetangulo = false;

              
                Rectangle rectTeste = new Rectangle(
                    _retanguloSelecao.X - (int)panOffset.X,
                    _retanguloSelecao.Y - (int)panOffset.Y,
                    _retanguloSelecao.Width,
                    _retanguloSelecao.Height);

               
                foreach (var box in _canvas.GetBoxes())
                {
                    Rectangle boxRect = box.GetZoomedRectangle(zoomLevel);

                    if (rectTeste.IntersectsWith(boxRect) || rectTeste.Contains(boxRect))
                    {
                        _caixasSelecionadas.Add(box);
                        box.Select();
                    }
                }

                _retanguloSelecao = Rectangle.Empty; 
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
          
            if (_firstSelectedBox != null)
            {
                using (Pen highlightPen = new Pen(Color.Red, 2))
                {
                    Rectangle rect = _firstSelectedBox.GetZoomedRectangle(zoomLevel);
                    e.Graphics.DrawRectangle(highlightPen, rect);

                   
                    using (Font font = new Font("Arial", 10, FontStyle.Bold))
                    using (Brush brush = new SolidBrush(Color.Red))
                    {
                        e.Graphics.DrawString("Aguardando segunda seleção...", font, brush, new PointF(10, 10));
                    }
                }
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(pictureBox1.BackColor);

           
            e.Graphics.TranslateTransform(panOffset.X, panOffset.Y);

            if (zoomCenter == PointF.Empty)
            {
                zoomCenter = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            }

            e.Graphics.TranslateTransform(zoomCenter.X, zoomCenter.Y);
            e.Graphics.ScaleTransform(zoomLevel, zoomLevel);
            e.Graphics.TranslateTransform(-zoomCenter.X, -zoomCenter.Y);

           
            _canvas.Draw(e.Graphics, zoomLevel);


            e.Graphics.ResetTransform();

            if (_desenhandoRetangulo)
            {
              
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, Color.DodgerBlue)))
                {
                    e.Graphics.FillRectangle(brush, _retanguloSelecao);
                }

                using (Pen pen = new Pen(Color.DodgerBlue, 1.5f))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawRectangle(pen, _retanguloSelecao);
                }
            }

           
            if (_caixasSelecionadas.Count > 1)
            {
                
                e.Graphics.TranslateTransform(panOffset.X, panOffset.Y);
                if (zoomCenter == PointF.Empty) zoomCenter = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
                e.Graphics.TranslateTransform(zoomCenter.X, zoomCenter.Y);
                e.Graphics.ScaleTransform(zoomLevel, zoomLevel);
                e.Graphics.TranslateTransform(-zoomCenter.X, -zoomCenter.Y);

                using (Pen penHighlight = new Pen(Color.DodgerBlue, 3f))
                {
                    penHighlight.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    foreach (var box in _caixasSelecionadas)
                    {
                        Rectangle rect = box.GetZoomedRectangle(zoomLevel);
                        e.Graphics.DrawRectangle(penHighlight, rect);
                    }
                }
            }
        }

        private void btnAmb_SalaFisica_Click(object sender, EventArgs e)
        {
            info.Text = "Ambiente Sala Física";
            tipoAmbiente = 1;
        }

        private void btnAmb_Sincrono_Click(object sender, EventArgs e)
        {
            info.Text = "Ambiente Síncrono";
            tipoAmbiente = 2;
        }

        private void btnAmb_ssincrono_Click(object sender, EventArgs e)
        {
            info.Text = "Ambiente Assíncrono";
            tipoAmbiente = 3;
        }

        private void btnAmb_Simulado_Click(object sender, EventArgs e)
        {
            info.Text = "Ambiente Simulado";
            tipoAmbiente = 4;
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            info.Text = "Informação";
            tipoAmbiente = 10;
        }

        private void btnDialog_Click(object sender, EventArgs e)
        {
            info.Text = "Diálogo";
            tipoAmbiente = 11;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            info.Text = "Atividade";
            tipoAmbiente = 12;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            info.Text = "Prática";
            tipoAmbiente = 13;
        }

        private void btnFeedBack_Click(object sender, EventArgs e)
        {
            info.Text = "Feedback";
            tipoAmbiente = 14;
        }

        private void novoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._canvas.ClearAll();
            _selectedBox = null;
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (_modoBorracha && e.Button == MouseButtons.Left)
            {
                HandleBorrachaSelection(e.X, e.Y);
                return; 
            }

            
            if (!string.IsNullOrEmpty(_selectedRelationshipType) && pictureBox1.Cursor == Cursors.Cross)
            {
                HandleConnectionSelection(e.X, e.Y);
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                
                Point relativePosition = GetRelativeCursorPos();
                this.clickPoint = new Point(e.X, e.Y);
                _canvas.Select(e.X, e.Y);
                pictureBox1.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                
                var caixas = _canvas.GetBoxes();

                
                for (int i = caixas.Count - 1; i >= 0; i--)
                {
                    Box box = caixas[i];

                    if (box.IsInCollision(e.X, e.Y))
                    {
                        _selectedBox = box;
                        clickPoint = new Point(e.X, e.Y);

                        
                        Contexto.Show(pictureBox1, e.Location);
                        return;
                    }
                }
                _selectedBox = null;
                clickPoint = Point.Empty;
            }
        }

        private void toolStripMenuItem_Editar_Click(object sender, EventArgs e)
        {
            if (_selectedBox == null) return;

          
            if (_selectedBox is Elemento ele && ele.TipoAmbiente != 20)
            {
               
                string backupTop = ele.TopText;
                string backupBottom = ele.BottomText;

                using (frmInfoElemento frm = new frmInfoElemento(ele))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        
                        ele.TopText = frm.Result.TopText;
                        ele.BottomText = frm.Result.BottomText;
                    }
                    else
                    {
                        
                        ele.TopText = backupTop;
                        ele.BottomText = backupBottom;
                    }

                    pictureBox1.Invalidate(); 
                }
            }
            
            else if (_selectedBox is Agente ag)
            {
                
                string backupTop = ag.TopText;
                string backupBottom = ag.BottomText;

                using (frmInfoAgente frm = new frmInfoAgente(ag))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        Agente editado = (Agente)frm.Result;
                        ag.TopText = editado.TopText;
                        ag.BottomText = editado.BottomText;
                    }
                    else
                    {
                        
                        ag.TopText = backupTop;
                        ag.BottomText = backupBottom;
                    }

                    pictureBox1.Invalidate();
                }
            }
            else
            {
                MessageBox.Show("Este tipo de elemento não possui textos editáveis.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnBorracha_Click(object sender, EventArgs e)
        {
            
            _modoBorracha = !_modoBorracha;

            if (_modoBorracha)
            {
                
                ResetConnectionSelection();
                tipoAmbiente = 0;

                _boxParaDesconectar = null;
                pictureBox1.Cursor = Cursors.NoMove2D; 
                info.Text = "Borracha Ativa: Clique na primeira Box da conexão que deseja apagar.";
            }
            else
            {
                
                _boxParaDesconectar?.Unselect();
                _boxParaDesconectar = null;
                pictureBox1.Cursor = Cursors.Default;
                info.Text = "";
                pictureBox1.Invalidate();
            }
        }

        private void HandleBorrachaSelection(int x, int y)
        {
            Box boxClicada = _canvas.SelectRC(x, y);

            if (boxClicada == null)
            {
                MessageBox.Show("Selecione um elemento válido.", "Borracha",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_boxParaDesconectar == null)
            {
             
                _boxParaDesconectar = boxClicada;
                _boxParaDesconectar.Select();
                info.Text = $"Borracha: Box '{boxClicada.OriginalName}' selecionada. Agora clique na outra Box conectada.";
            }
            else
            {
                
                if (_boxParaDesconectar.Id != boxClicada.Id)
                {
                    _canvas.RemoveConnection(_boxParaDesconectar, boxClicada);
                    MessageBox.Show("Conexão removida com sucesso!", "Borracha",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                _boxParaDesconectar?.Unselect();
                _modoBorracha = false;
                _boxParaDesconectar = null;
                pictureBox1.Cursor = Cursors.Default;
                info.Text = "";

                pictureBox1.Invalidate(); 
            }
        }
       

        private void HandleConnectionSelection(int x, int y)
        {
            Box selectedBox = _canvas.SelectRC(x, y);

            if (selectedBox == null)
            {
                MessageBox.Show("Selecione um elemento válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_firstSelectedBox == null)
            {
                _firstSelectedBox = selectedBox;
                _firstSelectedBox.Select();

                MessageBox.Show($"Primeiro elemento selecionado: {_firstSelectedBox.OriginalName}\nAgora selecione o segundo elemento.",
                              "Selecionar Segundo Elemento", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (_firstSelectedBox == selectedBox)
                {
                    MessageBox.Show("Não é possível conectar um elemento com ele mesmo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _secondSelectedBox = selectedBox;
                CreateConnectionBetweenBoxes();
                ResetConnectionSelection();
            }

            pictureBox1.Invalidate();
        }


       
        private void CarregarTemplateInvertida(object sender, EventArgs e)
        {
            if (_canvas.GetBoxes().Count > 0)
            {
                var result = MessageBox.Show("Deseja limpar o diagrama atual para carregar o modelo?",
                    "Confirmar Modelo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
            }

            _canvas.ClearAll();

            
            ClassBox ambVirtual = new ClassBox(30, 80, "Espaço Virtual (Antes da Aula)", 3); // 3 = Assíncrono
            ambVirtual.Width = 460;
            ambVirtual.Height = 400;

            ClassBox ambPresencial = new ClassBox(520, 80, "Espaço Presencial (Durante a Aula)", 1); // 1 = Sala Física
            ambPresencial.Width = 750;
            ambPresencial.Height = 400;

            _canvas.AddBoxToList(ambVirtual);
            _canvas.AddBoxToList(ambPresencial);

            
            Elemento inicio = new Elemento(40, 220, "Início", 20);
            inicio.SetElemento("Início", "");

            Elemento infoPrevia = new Elemento(300, 220, "Informação", 10);
            infoPrevia.TopText = "Video aula e Leituras";
            infoPrevia.BottomText = "Estudo Prévio Individual";

            Elemento atividade = new Elemento(600, 220, "Atividade", 12);
            atividade.TopText = "Resolução de Problemas";
            atividade.BottomText = "Trabalho em Grupo";

            Elemento feedback = new Elemento(850, 220, "Feedback", 14);
            feedback.TopText = "Alinhamento de Dúvidas";
            feedback.BottomText = "Debate com o Professor";

            Elemento fim = new Elemento(1100, 220, "Fim", 20);
            fim.SetElemento("Fim", "");

            _canvas.AddBoxToList(inicio);
            _canvas.AddBoxToList(infoPrevia);
            _canvas.AddBoxToList(atividade);
            _canvas.AddBoxToList(feedback);
            _canvas.AddBoxToList(fim);

           
            _canvas.AddConnection(infoPrevia, inicio, "Ação", "source", "1", "1");
            _canvas.AddConnection(atividade, infoPrevia, "Ação", "source", "1", "1");
            _canvas.AddConnection(feedback, atividade, "Ação", "source", "1", "1");
            _canvas.AddConnection(fim, feedback, "Ação", "source", "1", "1");

            
            UpdatePictureBoxSize();
            pictureBox1.Invalidate();
        }

       
        private void CarregarTemplatePBL(object sender, EventArgs e)
        {
            if (_canvas.GetBoxes().Count > 0)
            {
                var result = MessageBox.Show("Deseja limpar o diagrama atual para carregar o modelo?",
                    "Confirmar Modelo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
            }

            _canvas.ClearAll();

            
            ClassBox ambSala = new ClassBox(50, 80, "Laboratório / Sala de Aula", 1);
            ambSala.Width = 1200; 
            ambSala.Height = 400;
            _canvas.AddBoxToList(ambSala);

            
            Elemento inicio = new Elemento(100, 220, "Início", 20);
            inicio.SetElemento("Início", "");

            Elemento dialogo = new Elemento(350, 220, "Diálogo", 11);
            dialogo.TopText = "Apresentação do Problema";
            dialogo.BottomText = "Brainstorming Inicial";

            Elemento pratica = new Elemento(600, 220, "Prática", 13);
            pratica.TopText = "Investigação e Pesquisa";
            pratica.BottomText = "Desenho da Solução";

            Elemento feedback = new Elemento(850, 220, "Feedback", 14);
            feedback.TopText = "Apresentação dos Resultados";
            feedback.BottomText = "Feedback da Banca/Docente";

            Elemento fim = new Elemento(1100, 220, "Fim", 20);
            fim.SetElemento("Fim", "");

            _canvas.AddBoxToList(inicio);
            _canvas.AddBoxToList(dialogo);
            _canvas.AddBoxToList(pratica);
            _canvas.AddBoxToList(feedback);
            _canvas.AddBoxToList(fim);

            
            _canvas.AddConnection(dialogo, inicio, "Ação", "source", "1", "1");
            _canvas.AddConnection(pratica, dialogo, "Ação", "source", "1", "1");
            _canvas.AddConnection(feedback, pratica, "Ação", "source", "1", "1");
            _canvas.AddConnection(fim, feedback, "Ação", "source", "1", "1");

            
            UpdatePictureBoxSize();
            pictureBox1.Invalidate();
        }



        private bool ValidarModeloPedagogico(bool mostrarMensagemSucesso = true)
        {
            List<string> mensagensErro = new List<string>();

           
            foreach (var box in _canvas.GetBoxes())
            {
                box.HasError = false;
            }

            var elementos = _canvas.GetBoxes().OfType<Elemento>().ToList();
            var conexoes = _canvas.Connections;

            if (elementos.Count == 0)
            {
                MessageBox.Show("O diagrama está vazio. Adicione elementos para validar.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            
            bool temInicioFim = elementos.Any(e => e.TipoAmbiente == 20);
            if (!temInicioFim)
            {
                mensagensErro.Add("• O diagrama não possui nenhum bloco de 'Início/Fim'. É obrigatório definir de onde a aula começa e onde ela termina.");
            }

            
            foreach (var ele in elementos)
            {
                if (ele.TipoAmbiente == 15) continue;

                bool temEntrada = conexoes.Any(c => c.SourceBox.Id == ele.Id);
                bool temSaida = conexoes.Any(c => c.TargetBox.Id == ele.Id);
                bool temConexao = temEntrada || temSaida;

                if (ele.TipoAmbiente == 20)
                {
                    if (!temConexao)
                    {
                        ele.HasError = true;
                        mensagensErro.Add($"• O bloco de limite '{ele.OriginalName}' está solto. Ele deve estar conectado a algum artefato da aula.");
                    }
                    continue;
                }

                
                if (!temConexao)
                {
                    ele.HasError = true;
                    mensagensErro.Add($"• O artefato '{ele.OriginalName}' ({ele.TopText}) está solto e não possui nenhuma ligação.");
                    continue;
                }

                
                if (!temEntrada)
                {
                    ele.HasError = true;
                    mensagensErro.Add($"• O fluxo inicia a partir de '{ele.OriginalName}' ({ele.TopText}), mas deve começar obrigatoriamente a partir de um bloco 'Início/Fim'.");
                }

                if (!temSaida)
                {
                    ele.HasError = true;
                    mensagensErro.Add($"• O fluxo encerra abruptamente em '{ele.OriginalName}' ({ele.TopText}), mas deve ser finalizado ligando-o a um bloco 'Início/Fim'.");
                }

                
                if (ele.TipoAmbiente == 12 || ele.TipoAmbiente == 13)
                {
                    bool temFeedback = conexoes.Any(c =>
                        (c.SourceBox.Id == ele.Id && c.TargetBox is Elemento t && t.TipoAmbiente == 14) ||
                        (c.TargetBox.Id == ele.Id && c.SourceBox is Elemento s && s.TipoAmbiente == 14)
                    );

                    if (!temFeedback)
                    {
                        ele.HasError = true;
                        mensagensErro.Add($"• O artefato '{ele.OriginalName}' ({ele.TopText}) não possui um 'Feedback' subsequente validando a ação do aluno.");
                    }
                }
            }

           
            pictureBox1.Invalidate();

          
            if (mensagensErro.Count > 0)
            {
                string aviso = "Não é possível prosseguir. Foram encontradas inconsistências no Design Instrucional:\n\n" + string.Join("\n\n", mensagensErro);
                MessageBox.Show(aviso, "Validação do Modelo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; 
            }
            else
            {
                if (mostrarMensagemSucesso)
                {
                    MessageBox.Show("Parabéns! O seu modelo pedagógico está consistente, possuindo início, fim e validações estruturais corretas.", "Validação do Modelo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return true; 
            }
        }

        public void ExportToPdf()
        {
            if (!ValidarModeloPedagogico(mostrarMensagemSucesso: false))
            {
                MessageBox.Show("Exportação cancelada. Por favor, corrija os erros apontados em vermelho no diagrama antes de gerar o PDF.",
                                "Bloqueio de Exportação", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return; 
            }
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Ficheiro PDF|*.pdf";
                sfd.Title = "Exportar Plano de Aula (PDF)";
                sfd.FileName = $"PlanoAula_LEML_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        
                        float zoomAntigo = zoomLevel;
                        PointF panAntigo = panOffset;

                        
                        zoomLevel = 1.0f;
                        panOffset = PointF.Empty;
                        UpdatePictureBoxSize();
                        pictureBox1.Update(); 

                        PdfDocument document = new PdfDocument();
                        document.Info.Title = "Plano de Aula - LEML-StudioBr";

                        PdfPage page = document.AddPage();
                        XGraphics gfx = XGraphics.FromPdfPage(page);

                        XFont fontTitle = new XFont("Segoe UI", 18, XFontStyleEx.Bold);
                        XFont fontSubtitle = new XFont("Segoe UI", 14, XFontStyleEx.Bold);
                        XFont fontNormal = new XFont("Segoe UI", 11, XFontStyleEx.Regular);

                        int currentY = 40;
                        gfx.DrawString("Plano de Aula - Design Instrucional (LEML)", fontTitle, XBrushes.DarkBlue,
                                       new XRect(0, currentY, page.Width, 20), XStringFormats.Center);
                        currentY += 50;

                       
                        using (Bitmap fullBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height))
                        {
                            pictureBox1.DrawToBitmap(fullBitmap, new Rectangle(0, 0, fullBitmap.Width, fullBitmap.Height));

                            
                            Rectangle bounds = CalculateContentBounds();
                            bounds.Inflate(40, 40); 

                            
                            bounds.Intersect(new Rectangle(0, 0, fullBitmap.Width, fullBitmap.Height));

                            if (bounds.Width > 0 && bounds.Height > 0)
                            {
                                
                                using (Bitmap croppedBitmap = fullBitmap.Clone(bounds, fullBitmap.PixelFormat))
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        croppedBitmap.Save(ms, ImageFormat.Png);
                                        ms.Position = 0;

                                        XImage xImage = XImage.FromStream(ms);

                                        double margin = 40;
                                        double maxWidth = page.Width - (margin * 2);
                                        double maxHeight = 350;

                                       
                                        double scaleX = maxWidth / xImage.PointWidth;
                                        double scaleY = maxHeight / xImage.PointHeight;
                                        double scale = Math.Min(scaleX, scaleY);
                                        if (scale > 1) scale = 1;

                                        double drawWidth = xImage.PointWidth * scale;
                                        double drawHeight = xImage.PointHeight * scale;
                                        double drawX = (page.Width - drawWidth) / 2;

                                        gfx.DrawImage(xImage, drawX, currentY, drawWidth, drawHeight);
                                        currentY += (int)drawHeight + 40;
                                    }
                                }
                            }
                        }

                      
                        zoomLevel = zoomAntigo;
                        panOffset = panAntigo;
                        UpdatePictureBoxSize();
                        pictureBox1.Invalidate();

                    
                        gfx.DrawString("Detalhamento das Atividades:", fontSubtitle, XBrushes.Black, 40, currentY);
                        currentY += 30;

                        var elementos = _canvas.GetBoxes().OfType<Elemento>().ToList();

                        if (elementos.Count == 0)
                        {
                            gfx.DrawString("Nenhum artefato adicionado ao modelo.", fontNormal, XBrushes.Gray, 40, currentY);
                        }
                        else
                        {
                            foreach (var ele in elementos)
                            {
                                if (currentY > page.Height - 80)
                                {
                                    page = document.AddPage();
                                    gfx = XGraphics.FromPdfPage(page);
                                    currentY = 40;
                                }

                                string tipoArtefato = ele.OriginalName;
                                string oQueFazer = string.IsNullOrEmpty(ele.TopText) ? "Não definido" : ele.TopText;
                                string comoFazer = string.IsNullOrEmpty(ele.BottomText) ? "Não definido" : ele.BottomText;

                                gfx.DrawString($"• {tipoArtefato}", fontSubtitle, XBrushes.SteelBlue, 40, currentY);
                                currentY += 20;

                                gfx.DrawString($"  O quê fazer: {oQueFazer}", fontNormal, XBrushes.Black, 40, currentY);
                                currentY += 15;
                                gfx.DrawString($"  Como fazer: {comoFazer}", fontNormal, XBrushes.Black, 40, currentY);

                                currentY += 30;
                            }
                        }

                        document.Save(sfd.FileName);
                        MessageBox.Show("Plano de Aula exportado com sucesso!", "PDF Gerado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        
                        Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocorreu um erro ao gerar o PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CreateConnectionBetweenBoxes()
        {
            try
            {
                string relationOrigin = "source";
                string srcCardinality = "1";
                string tgtCardinality = "1";

                _canvas.AddConnection(_firstSelectedBox, _secondSelectedBox,
                                    _selectedRelationshipType, relationOrigin,
                                    srcCardinality, tgtCardinality);

                MessageBox.Show($"Conexão {_selectedRelationshipType} criada entre:\n{_firstSelectedBox.OriginalName} e {_secondSelectedBox.OriginalName}",
                               "Conexão Criada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar conexão: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetConnectionSelection()
        {
            _firstSelectedBox?.Unselect();
            _secondSelectedBox?.Unselect();

            _firstSelectedBox = null;
            _secondSelectedBox = null;
            _selectedRelationshipType = "";

            pictureBox1.Cursor = Cursors.Default;
            pictureBox1.Invalidate();
        }

        private void CancelConnectionCreation()
        {
            ResetConnectionSelection();
            MessageBox.Show("Criação de conexão cancelada.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripMenuItem_Apagar_Click(object sender, EventArgs e)
        {
            if (_selectedBox != null)
            {
                var result = MessageBox.Show($"Deseja apagar a box '{_selectedBox.OriginalName}'?",
                                             "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _canvas.RemoveBoxFromList(_selectedBox);
                    _selectedBox = null;
                    clickPoint = Point.Empty;
                    pictureBox1.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Nenhuma box selecionada para apagar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnStarEnd_Click(object sender, EventArgs e)
        {
            info.Text = "Início/Fim";
            tipoAmbiente = 20;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }

        private void toolStripSplitButtonZoomIn_ButtonClick(object sender, EventArgs e) => ZoomIn();
        private void toolStripSplitButtonZoomOut_ButtonClick(object sender, EventArgs e) => ZoomOut();
        private void toolStripStatusLBLZoom_DoubleClick(object sender, EventArgs e) => ResetZoom();

        private void ZoomIn()
        {
            if (zoomLevel < MAX_ZOOM)
            {
                zoomLevel += ZOOM_INCREMENT;
                UpdateZoom();
            }
        }

        private void ZoomOut()
        {
            if (zoomLevel > MIN_ZOOM)
            {
                zoomLevel -= ZOOM_INCREMENT;
                UpdateZoom();
            }
        }

        private void ResetZoom()
        {
            zoomLevel = 1.0f;
            UpdateZoom();
        }

        private void UpdateZoom()
        {
            toolStripStatusLBLZoom.Text = $"{zoomLevel * 100:0}%";
            UpdatePictureBoxSize();
            pictureBox1.Invalidate();
        }

        private void ConfigurePictureBoxWithScroll()
        {
            UpdatePictureBoxSize();
        }

        private void UpdatePictureBoxSize()
        {
            int maxX = 0;
            int maxY = 0;

            foreach (var box in _canvas.GetBoxes())
            {
                Rectangle boxRect = box.GetZoomedRectangle(zoomLevel);
                maxX = Math.Max(maxX, boxRect.Right);
                maxY = Math.Max(maxY, boxRect.Bottom);
            }

            pictureBox1.Size = new Size(maxX + 100, maxY + 100);
        }

        public void ExportToJpg(bool exportVisibleAreaOnly = false, int quality = 95)
        {
            if (!ValidarModeloPedagogico(mostrarMensagemSucesso: false))
            {
                MessageBox.Show("Exportação cancelada. Por favor, corrija os erros apontados em vermelho no diagrama antes de gerar a imagem.",
                                "Bloqueio de Exportação", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return; 
            }
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Imagens JPEG|*.jpg";
                sfd.Title = "Exportar Diagrama LEML";
                sfd.FileName = $"Diagrama_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                       
                        float zoomAntigo = zoomLevel;
                        PointF panAntigo = panOffset;

                        
                        zoomLevel = 1.0f;
                        panOffset = PointF.Empty;
                        UpdatePictureBoxSize();
                        pictureBox1.Update(); 

                        
                        using (Bitmap fullBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height))
                        {
                            pictureBox1.DrawToBitmap(fullBitmap, new Rectangle(0, 0, fullBitmap.Width, fullBitmap.Height));

                           
                            Rectangle bounds = CalculateContentBounds();
                            bounds.Inflate(40, 40); 

                          
                            bounds.Intersect(new Rectangle(0, 0, fullBitmap.Width, fullBitmap.Height));

                            if (bounds.Width > 0 && bounds.Height > 0)
                            {
                                
                                using (Bitmap croppedBitmap = fullBitmap.Clone(bounds, fullBitmap.PixelFormat))
                                {
                                    SaveJpeg(croppedBitmap, sfd.FileName, quality);
                                }
                            }
                            else
                            {
                                SaveJpeg(fullBitmap, sfd.FileName, quality);
                            }
                        }

                       
                        zoomLevel = zoomAntigo;
                        panOffset = panAntigo;
                        UpdatePictureBoxSize();
                        pictureBox1.Invalidate();

                        MessageBox.Show("Imagem exportada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                       
                        Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocorreu um erro ao exportar a imagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveJpeg(Bitmap bitmap, string path, int quality)
        {
            var encoder = ImageCodecInfo.GetImageEncoders()
                .FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

            if (encoder != null)
            {
                var parameters = new EncoderParameters(1);
                parameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                bitmap.Save(path, encoder, parameters);
            }
            else
            {
                bitmap.Save(path, ImageFormat.Jpeg);
            }
        }

        private Rectangle CalculateContentBounds()
        {
            try
            {
                var boxes = _canvas?.GetBoxes();
                if (boxes == null || boxes.Count == 0)
                    return new Rectangle(0, 0, 800, 600);

                int minX = int.MaxValue, minY = int.MaxValue;
                int maxX = int.MinValue, maxY = int.MinValue;
                bool hasContent = false;

                foreach (Box box in boxes)
                {
                    if (box != null)
                    {
                        Rectangle boxRect = box.GetZoomedRectangle(zoomLevel);
                        minX = Math.Min(minX, boxRect.Left);
                        minY = Math.Min(minY, boxRect.Top);
                        maxX = Math.Max(maxX, boxRect.Right);
                        maxY = Math.Max(maxY, boxRect.Bottom);
                        hasContent = true;
                    }
                }

                if (!hasContent)
                    return new Rectangle(0, 0, 800, 600);

                int margin = (int)(50 * zoomLevel);
                return new Rectangle(
                    minX - margin,
                    minY - margin,
                    (maxX - minX) + 2 * margin,
                    (maxY - minY) + 2 * margin
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao calcular bounds: {ex.Message}");
                return new Rectangle(0, 0, 800, 600);
            }
        }

        private void imprimirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToPdf();
        }

        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToJpg(false, 95);
        }

        private void toolStripCBOConexao_Click(object sender, EventArgs e) { }

       
        private void pictureBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        private void btnAgent_Click(object sender, EventArgs e)
        {
            info.Text = "Agente de IA";
            tipoAmbiente = 15;
        }

        private void toolStripMenuItem_Ligar_Click(object sender, EventArgs e)
        {
            using (frmRelacao formRelacao = new frmRelacao(_canvas))
            {
                if (formRelacao.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Invalidate();
                }
            }
        }

        private void LimparMultiSelecao()
        {
            foreach (var box in _caixasSelecionadas)
            {
                box.Unselect();
            }
            _caixasSelecionadas.Clear();
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Arquivo LEML JSON|*.lemljson|Todos os arquivos|*.*";
                sfd.Title = "Salvar Diagrama";
                sfd.FileName = "Diagrama.lemljson";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SaveProjectToJson(sfd.FileName);
                }
            }
        }

        private void abirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Arquivo LEML JSON|*.lemljson|Todos os arquivos|*.*";
                ofd.Title = "Abrir Diagrama";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LoadProjectFromJson(ofd.FileName);
                }
            }
        }

        private void SaveProjectToJson(string filePath)
        {
            try
            {
                var boxes = _canvas.GetBoxes().Select(b => new
                {
                    b.Id,
                    b.PositionX,
                    b.PositionY,
                    b.Width,
                    b.Height,
                    b.OriginalName,
                    b.TipoAmbiente,
                    TopText = b.TopText ?? "Não Definido",
                    BottomText = b.BottomText ?? "Não Definido",
                    BoxType = b.GetType().Name,
                    Name = b.OriginalName
                }).ToList();

                var connections = _canvas.Connections.Select(c => new
                {
                    SourceId = c.SourceBox?.Id ?? Guid.Empty,
                    TargetId = c.TargetBox?.Id ?? Guid.Empty,
                    c.RelationshipType,
                    c.RelationOrigin,
                    SourceCardinality = c.SourceCardinality ?? "1",
                    TargetCardinality = c.TargetCardinality ?? "1"
                }).ToList();

                var projectData = new
                {
                    Boxes = boxes,
                    Connections = connections,
                    SaveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Version = "1.0"
                };

                string json = JsonConvert.SerializeObject(projectData, Formatting.Indented);
                File.WriteAllText(filePath, json);

                MessageBox.Show($"Salvo: {boxes.Count} elementos e {connections.Count} conexões",
                               "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProjectFromJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Arquivo não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string json = File.ReadAllText(filePath);
                var jObject = JObject.Parse(json);
                var boxesArray = jObject["Boxes"] as JArray;
                var connectionsArray = jObject["Connections"] as JArray;

                if (boxesArray == null || boxesArray.Count == 0)
                {
                    MessageBox.Show("Arquivo não contém elementos!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _canvas.ClearAll();
                var boxDictionary = new Dictionary<Guid, Box>();
                int loadedBoxes = 0;
                int loadedConnections = 0;

                foreach (var boxToken in boxesArray)
                {
                    try
                    {
                        Guid id = boxToken["Id"]?.Value<Guid>() ?? Guid.NewGuid();
                        float posX = boxToken["PositionX"]?.Value<float>() ?? 0;
                        float posY = boxToken["PositionY"]?.Value<float>() ?? 0;
                        float width = boxToken["Width"]?.Value<float>() ?? 100;
                        float height = boxToken["Height"]?.Value<float>() ?? 60;
                        string originalName = boxToken["OriginalName"]?.Value<string>() ?? "";
                        int tipoAmbiente = boxToken["TipoAmbiente"]?.Value<int>() ?? 0;
                        string topText = boxToken["TopText"]?.Value<string>() ?? "Não Definido";
                        string bottomText = boxToken["BottomText"]?.Value<string>() ?? "Não Definido";
                        string typeName = boxToken["Type"]?.Value<string>() ?? "";

                        Box box = null;

                        if (typeName.Contains("Elemento") || tipoAmbiente >= 10)
                        {
                            box = new Elemento((int)posX, (int)posY, originalName, tipoAmbiente);
                        }
                        else if (typeName.Contains("ClassBox") || (tipoAmbiente >= 1 && tipoAmbiente <= 4))
                        {
                            box = new ClassBox((int)posX, (int)posY, originalName, tipoAmbiente);
                        }

                        if (box != null)
                        {
                            box.Id = id;
                            box.TopText = topText;
                            box.BottomText = bottomText;
                            box.Width = width;
                            box.Height = height;

                            _canvas.AddBoxToList(box);
                            boxDictionary[id] = box;
                            loadedBoxes++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"✗ Erro ao carregar box: {ex.Message}");
                        continue;
                    }
                }

                if (connectionsArray != null)
                {
                    foreach (var connToken in connectionsArray)
                    {
                        try
                        {
                            Guid sourceId = connToken["SourceId"]?.Value<Guid>() ?? Guid.Empty;
                            Guid targetId = connToken["TargetId"]?.Value<Guid>() ?? Guid.Empty;
                            string relationshipType = connToken["RelationshipType"]?.Value<string>() ?? "Ação";
                            string relationOrigin = connToken["RelationOrigin"]?.Value<string>() ?? "source";
                            string sourceCardinality = connToken["SourceCardinality"]?.Value<string>() ?? "1";
                            string targetCardinality = connToken["TargetCardinality"]?.Value<string>() ?? "1";

                            if (boxDictionary.TryGetValue(sourceId, out Box sourceBox) &&
                                boxDictionary.TryGetValue(targetId, out Box targetBox))
                            {
                                _canvas.AddConnection(sourceBox, targetBox, relationshipType,
                                                    relationOrigin, sourceCardinality, targetCardinality);
                                loadedConnections++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"✗ Erro ao carregar conexão: {ex.Message}");
                        }
                    }
                }

                pictureBox1.Invalidate();
                UpdatePictureBoxSize();

                MessageBox.Show($"Projeto carregado com sucesso!\n{loadedBoxes} elementos e {loadedConnections} conexões carregados",
                               "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveImageProperties(JToken token)
        {
            if (token is JContainer container)
            {
                var propertiesToRemove = new List<string>
                {
                    "Icon", "HeaderImage", "BitmapProperty", "ImageProperty",
                    "IconBase64", "HeaderImageBase64", "IsSelected",
                    "icon", "headerImage", "image", "bitmap"
                };

                foreach (JToken child in container.Children().ToList())
                {
                    if (child is JProperty property)
                    {
                        if (propertiesToRemove.Contains(property.Name, StringComparer.OrdinalIgnoreCase))
                        {
                            property.Remove();
                        }
                        else
                        {
                            RemoveImageProperties(property.Value);
                        }
                    }
                    else
                    {
                        RemoveImageProperties(child);
                    }
                }
            }
        }
    }

   
    public class CustomFontResolver : PdfSharp.Fonts.IFontResolver
    {
        public string DefaultFontName => "Segoe UI";

        public byte[] GetFont(string faceName)
        {
         
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), faceName + ".ttf");

            if (File.Exists(fontPath))
            {
                return File.ReadAllBytes(fontPath);
            }

        
            return File.ReadAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf"));
        }

        public PdfSharp.Fonts.FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
         
            if (familyName.Equals("Segoe UI", StringComparison.CurrentCultureIgnoreCase))
            {
                if (isBold && isItalic) return new PdfSharp.Fonts.FontResolverInfo("segoez");
                if (isBold) return new PdfSharp.Fonts.FontResolverInfo("segoeb");
                if (isItalic) return new PdfSharp.Fonts.FontResolverInfo("segoei");
                return new PdfSharp.Fonts.FontResolverInfo("segoeui"); 
            }

           
            if (isBold) return new PdfSharp.Fonts.FontResolverInfo("arialbd");
            if (isItalic) return new PdfSharp.Fonts.FontResolverInfo("ariali");
            return new PdfSharp.Fonts.FontResolverInfo("arial");
        }
    }
}






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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LEML_StudioBr
{
    [Serializable]
    public class ProjectData
    {
        public List<Box> Boxes { get; set; } = new List<Box>();
        public List<Connection> Connections { get; set; } = new List<Connection>();
        //public DateTime SaveDate { get; set; } = DateTime.Now;
        //public string Version { get; set; } = "1.0";
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




        public frmMenu()
        {
            _canvas = new Canvas();
            InitializeComponent();

            ConfigurePictureBoxWithScroll();

            pictureBox1.Focus();

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
            else
            if (tipoAmbiente <= 4)
            {
                Box box = new ClassBox(relativePosition.X, relativePosition.Y, info.Text, this.tipoAmbiente);
                box.PositionX -= box.Width / 2;
                box.PositionY -= box.Height / 2;
                _canvas.AddBoxToList(box);

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
                            ele = frmInfoElemento.theEle;

                        }
                    }
                }
                if (tipoAmbiente == 20)
                {
                    ele.SetElemento("Início", "Fim");
                }
                _canvas.AddBoxToList(ele);
            }

            tipoAmbiente = 0; // Reset tipoAmbiente after adding a box
            info.Text = "";





        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _canvas.Select(e.X, e.Y);
                pictureBox1.Refresh();
            }
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _canvas.Move(e.X, e.Y);
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {



            // Desenha feedback visual para seleção de conexão
            if (_firstSelectedBox != null)
            {
                using (Pen highlightPen = new Pen(Color.Red, 2))
                {
                    Rectangle rect = _firstSelectedBox.GetZoomedRectangle(zoomLevel);
                    e.Graphics.DrawRectangle(highlightPen, rect);

                    // Desenha texto indicando que está aguardando segunda seleção
                    using (Font font = new Font("Arial", 10, FontStyle.Bold))
                    using (Brush brush = new SolidBrush(Color.Red))
                    {
                        e.Graphics.DrawString("Aguardando segunda seleção...", font, brush,
                                            new PointF(10, 10));
                    }
                }
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(pictureBox1.BackColor);

            // Aplica transformações de zoom e pan
            e.Graphics.TranslateTransform(panOffset.X, panOffset.Y);

            if (zoomCenter == PointF.Empty)
            {
                zoomCenter = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            }

            e.Graphics.TranslateTransform(zoomCenter.X, zoomCenter.Y);
            e.Graphics.ScaleTransform(zoomLevel, zoomLevel);
            e.Graphics.TranslateTransform(-zoomCenter.X, -zoomCenter.Y);

            // Desenha o conteúdo com zoom
            _canvas.Draw(e.Graphics, zoomLevel);




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
            // Se estamos no modo de seleção de conexão
            if (!string.IsNullOrEmpty(_selectedRelationshipType) && pictureBox1.Cursor == Cursors.Cross)
            {
                HandleConnectionSelection(e.X, e.Y);
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                // Sua lógica normal de clique esquerdo aqui...
                Point relativePosition = GetRelativeCursorPos();
                this.clickPoint = new Point(e.X, e.Y);
                _canvas.Select(e.X, e.Y);
                pictureBox1.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Verifica se clicou em alguma box
                foreach (Box box in _canvas.GetBoxes())
                {
                    if (box.IsInCollision(e.X, e.Y))
                    {
                        _selectedBox = box; // Armazena a box selecionada
                        clickPoint = new Point(e.X, e.Y); // Armazena o ponto do clique

                        // Mostra o menu de contexto
                        Contexto.Show(pictureBox1, e.Location);
                        return;
                    }
                }
                // Se não clicou em nenhuma box, limpa a seleção
                _selectedBox = null;
                clickPoint = Point.Empty;
            }
        }


        private void HandleConnectionSelection(int x, int y)
        {
            Box selectedBox = _canvas.SelectRC(x, y);

            if (selectedBox == null)
            {
                MessageBox.Show("Selecione um elemento válido.", "Aviso",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_firstSelectedBox == null)
            {
                // Primeira seleção
                _firstSelectedBox = selectedBox;
                _firstSelectedBox.Select(); // Destaca o primeiro elemento

                MessageBox.Show($"Primeiro elemento selecionado: {_firstSelectedBox.OriginalName}\nAgora selecione o segundo elemento.",
                              "Selecionar Segundo Elemento", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Segunda seleção - não pode ser o mesmo elemento
                if (_firstSelectedBox == selectedBox)
                {
                    MessageBox.Show("Não é possível conectar um elemento com ele mesmo.",
                                  "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _secondSelectedBox = selectedBox;

                // Cria a conexão
                CreateConnectionBetweenBoxes();

                // Reseta a seleção
                ResetConnectionSelection();
            }

            pictureBox1.Invalidate();
        }

        private void CreateConnectionBetweenBoxes()
        {
            try
            {
                // Define a origem da relação (pode ser ajustado conforme necessidade)
                string relationOrigin = "source";
                string srcCardinality = "1"; // Cardinalidade padrão
                string tgtCardinality = "1"; // Cardinalidade padrão

                // Adiciona a conexão ao canvas
                _canvas.AddConnection(_firstSelectedBox, _secondSelectedBox,
                                    _selectedRelationshipType, relationOrigin,
                                    srcCardinality, tgtCardinality);

                MessageBox.Show($"Conexão {_selectedRelationshipType} criada entre:\n" +
                               $"{_firstSelectedBox.OriginalName} e {_secondSelectedBox.OriginalName}",
                               "Conexão Criada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar conexão: {ex.Message}", "Erro",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("Criação de conexão cancelada.", "Info",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void toolStripMenuItem_Apagar_Click(object sender, EventArgs e)
        {


            if (_selectedBox != null)
            {
                var result = MessageBox.Show(
                    $"Deseja apagar a box '{_selectedBox.OriginalName}'?",
                    "Confirmar Exclusão",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _canvas.RemoveBoxFromList(_selectedBox);
                    _selectedBox = null;
                    clickPoint = Point.Empty;
                    pictureBox1.Refresh(); // Atualiza a tela
                }
            }
            else
            {
                MessageBox.Show("Nenhuma box selecionada para apagar.", "Aviso",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnStarEnd_Click(object sender, EventArgs e)
        {
            info.Text = "Início/Fim";
            tipoAmbiente = 20;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripSplitButtonZoomIn_ButtonClick(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void toolStripSplitButtonZoomOut_ButtonClick(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void toolStripStatusLBLZoom_DoubleClick(object sender, EventArgs e)
        {
            ResetZoom();
        }


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
            // Atualiza o label de zoom
            toolStripStatusLBLZoom.Text = $"{zoomLevel * 100:0}%";

            // Atualiza o tamanho do PictureBox para o scroll
            UpdatePictureBoxSize();

            // Força o redesenho do PictureBox
            pictureBox1.Invalidate();
        }


        private void ConfigurePictureBoxWithScroll()
        {
            // Ajusta o tamanho do PictureBox para o conteúdo
            UpdatePictureBoxSize();
        }

        private void UpdatePictureBoxSize()
        {
            // Calcula o tamanho necessário baseado no conteúdo com zoom
            int maxX = 0;
            int maxY = 0;

            foreach (var box in _canvas.GetBoxes())
            {
                Rectangle boxRect = box.GetZoomedRectangle(zoomLevel);
                maxX = Math.Max(maxX, boxRect.Right);
                maxY = Math.Max(maxY, boxRect.Bottom);
            }

            // Adiciona margem
            pictureBox1.Size = new Size(maxX + 100, maxY + 100);
        }


        public void ExportToJpg(bool exportVisibleAreaOnly = false, int quality = 95)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JPEG Images|*.jpg";
                sfd.Title = "Exportar Diagrama LEML";
                sfd.FileName = $"Diagrama_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bitmap;

                    if (exportVisibleAreaOnly)
                    {
                        // Exporta apenas a área visível
                        bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        pictureBox1.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                    }
                    else
                    {
                        // Exporta todo o conteúdo
                        Rectangle bounds = CalculateContentBounds();
                        bounds.Inflate(50, 50); // Margem

                        bitmap = new Bitmap(bounds.Width, bounds.Height);

                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.Clear(Color.White);
                            g.TranslateTransform(-bounds.X, -bounds.Y);
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            _canvas.Draw(g, zoomLevel);
                        }
                    }

                    // Salva com a qualidade especificada
                    SaveJpeg(bitmap, sfd.FileName, quality);
                    bitmap.Dispose();

                    MessageBox.Show("Exportação concluída com sucesso!", "Sucesso",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                // Adiciona margem
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


        }

        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JPEG Images|*.jpg";
                sfd.FileName = $"Diagrama_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Calcula a área que contém todo o conteúdo
                    Rectangle contentBounds = CalculateContentBounds();

                    // Cria bitmap do tamanho do conteúdo
                    using (Bitmap bitmap = new Bitmap(contentBounds.Width, contentBounds.Height))
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(Color.White);
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        // Ajusta a transformação para desenhar na posição correta
                        graphics.TranslateTransform(-contentBounds.X, -contentBounds.Y);

                        // Desenha o conteúdo
                        _canvas.Draw(graphics, zoomLevel);

                        // Salva a imagem
                        bitmap.Save(sfd.FileName, ImageFormat.Jpeg);
                    }

                    MessageBox.Show("Exportação concluída com sucesso!", "LEMLStudio.BR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        private void toolStripCBOConexao_Click(object sender, EventArgs e)
        {


        }



        private void pictureBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && !string.IsNullOrEmpty(_selectedRelationshipType))
            {
                CancelConnectionCreation();
            }
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
                    pictureBox1.Invalidate(); // ← Isso força o redesenho!
                }
            }
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
                // 1. Prepara boxes
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

                // 2. Prepara conexões
                var connections = _canvas.Connections.Select(c => new
                {
                    SourceId = c.SourceBox?.Id ?? Guid.Empty,
                    TargetId = c.TargetBox?.Id ?? Guid.Empty,
                    c.RelationshipType,
                    c.RelationOrigin,
                    SourceCardinality = c.SourceCardinality ?? "1",
                    TargetCardinality = c.TargetCardinality ?? "1"
                }).ToList();

                // 3. Cria objeto final
                var projectData = new
                {
                    Boxes = boxes,
                    Connections = connections,
                    SaveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Version = "1.0"
                };

                // 4. Converte para JSON e salva
                string json = JsonConvert.SerializeObject(projectData, Formatting.Indented);
                File.WriteAllText(filePath, json);

                MessageBox.Show($"Salvo: {boxes.Count} elementos e {connections.Count} conexões",
                               "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProjectFromJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Arquivo não encontrado!", "Erro",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string json = File.ReadAllText(filePath);
                var jObject = JObject.Parse(json);
                var boxesArray = jObject["Boxes"] as JArray;
                var connectionsArray = jObject["Connections"] as JArray;

                if (boxesArray == null || boxesArray.Count == 0)
                {
                    MessageBox.Show("Arquivo não contém elementos!", "Aviso",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _canvas.ClearAll();
                var boxDictionary = new Dictionary<Guid, Box>();
                int loadedBoxes = 0;
                int loadedConnections = 0;

                // Primeiro: carregar todas as boxes
                foreach (var boxToken in boxesArray)
                {
                    try
                    {
                        // Extrai os dados básicos - mantém como FLOAT
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

                        // Cria o objeto do tipo correto - usando FLOAT nas posições
                        if (typeName.Contains("Elemento") || tipoAmbiente >= 10)
                        {
                            box = new Elemento((int)posX, (int)posY, originalName, tipoAmbiente);
                        }
                        else if (typeName.Contains("ClassBox") || (tipoAmbiente >= 1 && tipoAmbiente <= 4))
                        {
                            box = new ClassBox((int)posX, (int)posY, originalName, tipoAmbiente);
                        }
                        else
                        {
                            // Se não for nenhum tipo específico, cria uma Box genérica
                            //box = new ClassBox((int)posX, (int)posY, originalName, tipoAmbiente);
                        }

                        // Define as propriedades adicionais
                        box.Id = id;
                        box.TopText = topText;
                        box.BottomText = bottomText;
                        box.Width = width;
                        box.Height = height;

                        _canvas.AddBoxToList(box);
                        boxDictionary[id] = box;
                        loadedBoxes++;

                        Debug.WriteLine($"✓ Box carregada: {box.OriginalName} " +
                                       $"(Tipo: {box.GetType().Name}, " +
                                       $"Pos: [{box.PositionX}, {box.PositionY}], " +
                                       $"Top: '{box.TopText}', Bottom: '{box.BottomText}')");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"✗ Erro ao carregar box: {ex.Message}");
                        continue;
                    }
                }

                // Segundo: carregar as conexões
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

                            // Encontra as boxes correspondentes
                            if (boxDictionary.TryGetValue(sourceId, out Box sourceBox) &&
                                boxDictionary.TryGetValue(targetId, out Box targetBox))
                            {
                                _canvas.AddConnection(sourceBox, targetBox, relationshipType,
                                                    relationOrigin, sourceCardinality, targetCardinality);
                                loadedConnections++;

                                Debug.WriteLine($"✓ Conexão carregada: {sourceBox.OriginalName} → {targetBox.OriginalName}, " +
                                               $"Tipo: {relationshipType}, " +
                                               $"Cardinalidades: {sourceCardinality} → {targetCardinality}");
                            }
                            else
                            {
                                Debug.WriteLine($"✗ Boxes não encontradas para conexão: {sourceId} → {targetId}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"✗ Erro ao carregar conexão: {ex.Message}");
                        }
                    }
                }

                // Atualiza a interface
                pictureBox1.Invalidate();
                UpdatePictureBoxSize();

                MessageBox.Show($"Projeto carregado com sucesso!\n" +
                               $"{loadedBoxes} elementos e {loadedConnections} conexões carregados",
                               "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar: {ex.Message}", "Erro",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

        private void RemoveImageProperties(JToken token)
        {
            if (token is JContainer container)
            {
                // Lista de propriedades para remover (adicionar todas as possíveis)
                var propertiesToRemove = new List<string>
        {
            "Icon", "HeaderImage", "BitmapProperty", "ImageProperty",
            "IconBase64", "HeaderImageBase64", "IsSelected",
            "icon", "headerImage", "image", "bitmap" // versões em minúsculo também
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
}


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;  


namespace LEML_StudioBr.Objetos
{
    [Serializable]
    public class Elemento : Box
    {
        // Novos campos para os itens adicionais
        public string TopText { get; set; }
        public string BottomText { get; set; }

        public Image Icon { get; set; }
        

        private int clsAmbiente = 0;

        // Construtores
        // Construtor padrão necessário para JSON
        public Elemento() : base(0, 0, "", 0)
        {
            // Valores padrão serão definidos via propriedades durante a desserialização
        }

        public override void RebuildImages()
        {
            // Reconstrua as imagens específicas do Elemento aqui, se necessário
            switch (clsAmbiente)
            {
                case 10: this.Icon = Properties.Resources.Information_logo; break;
                case 11: this.Icon = Properties.Resources.Dialog_logo; break;
                case 12: this.Icon = Properties.Resources.Activity_logo; break;
                case 13: this.Icon = Properties.Resources.Pratice_logo; break;
                case 14: this.Icon = Properties.Resources.Feedback_logo; break;
                case 15: this.Icon = Properties.Resources.Agent_logo; break;

                case 20: this.Icon = Properties.Resources.StarEnd; break;
                default: this.Icon = null; break;
            }
        }

        public Elemento(int x, int y, string name, int tpAmbiente)
            : base(x, y, name, tpAmbiente)
        {
            SetElemento("", "");
            BoxType = "Elemento";
            ColorBrush = Brushes.Transparent;
            this.clsAmbiente = tpAmbiente;
             
            RebuildImages();


        }



        // Método Draw sobrescrito para incluir os novos elementos
        public override void Draw(Graphics g, float zoomLevel = 1.0f)
        {
            g.TranslateTransform(PositionX * zoomLevel, PositionY * zoomLevel);

            int zoomedWidth = (int)(Width * zoomLevel);
            int zoomedHeight = (int)(Height * zoomLevel);

            // Desenha o retângulo base
            g.FillRectangle(ColorBrush, 0, 0, zoomedWidth, zoomedHeight);
            g.FillRectangle(Brushes.Black, zoomedWidth - 10 * zoomLevel, zoomedHeight - 10 * zoomLevel,
                          10 * zoomLevel, 10 * zoomLevel);

            // Ajusta tamanhos baseado no zoom
            float fontSize = 9 * zoomLevel;
            if (fontSize < 6) fontSize = 6;
            if (fontSize > 20) fontSize = 20;

            // Texto superior
            if (!string.IsNullOrEmpty(TopText))
            {
                using (Font topFont = new Font("Segoe UI", fontSize, FontStyle.Regular))
                {
                    g.DrawString(TopText, topFont, Brushes.Black,
                                zoomedWidth / 2, zoomedHeight * 0.05f, _formatCenter);
                }
            }

            // Nome principal
            float mainFontSize = 10 * zoomLevel;
            if (mainFontSize < 7) mainFontSize = 7;
            if (mainFontSize > 24) mainFontSize = 24;

            using (Font mainFont = new Font("Segoe UI", mainFontSize, FontStyle.Bold))
            {
                g.DrawString(Name, mainFont, Brushes.Black,
                            zoomedWidth / 2, zoomedHeight * 0.15f, _formatCenter);
            }

            // Linha
            g.DrawLine(Pens.Black, 0, zoomedHeight * 0.25f, zoomedWidth, zoomedHeight * 0.25f);

            // Ícone (com zoom)
            if (Icon != null)
            {
                int iconSize = (int)(Math.Max(Icon.Width, Icon.Height) * zoomLevel);
                int iconX = (zoomedWidth - iconSize) / 2;
                int iconY = (int)(zoomedHeight * 0.35f);

                g.DrawImage(Icon, iconX, iconY, iconSize, iconSize);
            }

            // Texto inferior
            if (!string.IsNullOrEmpty(BottomText))
            {
                using (Font bottomFont = new Font("Segoe UI", fontSize, FontStyle.Regular))
                {
                    g.DrawString(BottomText, bottomFont, Brushes.Black,
                                zoomedWidth / 2, zoomedHeight * 0.75f, _formatCenter);
                }
            }

            g.ResetTransform();
        }
        

        // Método para carregar ícone dos recursos
        public void LoadIconFromResources(string resourceName)
        {
            try
            {
                // Obtém o assembly atual
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();

                // Obtém todos os nomes de recursos
                var resourceNames = assembly.GetManifestResourceNames();

                // Encontra o recurso pelo nome
                var fullResourceName = resourceNames.FirstOrDefault(
                    name => name.EndsWith(resourceName));

                if (fullResourceName != null)
                {
                    using (var stream = assembly.GetManifestResourceStream(fullResourceName))
                    {
                        if (stream != null)
                        {
                            Icon = Image.FromStream(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar ícone: {ex.Message}");
            }
        }

        // Método para definir todos os três elementos de uma vez
        public void SetElemento(string topText, string bottomText)
        {
            this.TopText = topText;
            this.BottomText = bottomText;
        }

        // Método ToString sobrescrito para incluir informações adicionais
        public override string ToString()
        {
            return $"{base.ToString()} - O quê: {TopText} - Como: {BottomText}";
        }
    }
}
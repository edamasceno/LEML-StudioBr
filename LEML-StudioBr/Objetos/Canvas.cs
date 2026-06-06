using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json;
using System.Drawing.Drawing2D;

namespace LEML_StudioBr.Objetos
{
    [Serializable]
    public class Canvas
    {
        private List<Box> _boxes;
        private Selection? _selection;
        private ListManipulator<Box> _manipulator;

        // Lista de conexões serializável
        private List<CanvasConnection> _connections = new List<CanvasConnection>();

        private PictureBox _pictureBox;


        // Propriedades públicas para serialização
        [JsonProperty("Boxes")]
        public List<Box> Boxes => _boxes;

        [JsonProperty("Connections")]
        public List<CanvasConnection> Connections => _connections;


        public Canvas()
        {
            _boxes = new List<Box>();
            _selection = null;
            _manipulator = new ListManipulator<Box>(_boxes);
        }



        public void Draw(Graphics g, float zoomLevel = 1.0f)
        {
            if (_boxes == null || _boxes.Count == 0)
                return;

            // Desenha primeiro as Boxes normais (que não são Elemento)
            foreach (var box in _boxes.Where(b => !(b is Elemento)))
            {
                box.Draw(g, zoomLevel);
            }

            // Desenha Elementos
            foreach (var ele in _boxes.Where(ele => ele is Elemento))
            {
                ele.Draw(g, zoomLevel);
            }

            // Desenha as conexões após desenhar todas as boxes
            foreach (var connection in _connections)
            {
                if (connection.SourceBox != null && connection.TargetBox != null)
                {
                    CheckLinesRels(connection.SourceBox, connection.TargetBox,
                                 connection.RelationshipType, connection.RelationOrigin,
                                 g, connection.SourceCardinality, connection.TargetCardinality);
                }
            }
        }

        public Box? SelectRC(int x, int y)
        {
            Unselect();

            for (int i = _boxes.Count - 1; i >= 0; i--)
            {
                Box box = _boxes[i];
                if (box.IsInCollision(x, y))
                {
                    return box;
                }
            }

            return null;
        }

        public Box? SelectHeader(int x, int y)
        {
            Unselect();

            for (int i = _boxes.Count - 1; i >= 0; i--)
            {
                Box box = _boxes[i];
                if (box.IsInCollisionWithHeader(x, y))
                {
                    return box;
                }
            }

            return null;
        }

        public void Select(int x, int y)
        {
            Unselect();

            for (int i = _boxes.Count - 1; i >= 0; i--)
            {
                Box box = _boxes[i];
                if (box.IsInCollisionWithCorner(x, y))
                {
                    _selection = new ResizeSelection(box, x, y);
                    _manipulator.MoveToLast(i);
                    _selection.Select();
                    return;
                }
                else if (box.IsInCollision(x, y))
                {
                    _selection = new MoveSelection(box, x, y);
                    _manipulator.MoveToLast(i);
                    _selection.Select();
                    return;
                }
            }
        }

        public void Unselect()
        {
            if (_selection == null)
                return;

            _selection.Unselect();
            _selection = null;
        }

        public void Move(int x, int y)
        {
            if (_selection == null)
                return;

            _selection.Move(x, y);
        }

        public List<Box> GetBoxes() => _boxes;

        public void AddBoxToList(Box box)
        {
            _boxes.Add(box);
        }

        private void ReorderBoxes()
        {
            _boxes = _boxes.OrderBy(b => b is Elemento ? 1 : 0).ToList();
        }

        public void BringElementToFront(Elemento elemento)
        {
            if (_boxes.Contains(elemento))
            {
                _boxes.Remove(elemento);
                _boxes.Add(elemento);
            }
        }

        public void SendBoxToBack(Box box)
        {
            if (_boxes.Contains(box) && !(box is Elemento))
            {
                _boxes.Remove(box);
                _boxes.Insert(0, box);
            }
        }

        public void RemoveBoxFromList(Box box)
        {
            _boxes.Remove(box);
            // Remove também as conexões relacionadas a esta box
            _connections.RemoveAll(c => c.SourceBox == box || c.TargetBox == box);
        }

        public bool DoesBoxNameExist(string name)
        {
            return _boxes.Exists(box => box.OriginalName == name);
        }

        public void AddConnection(Box b1, Box b2, string rel, string relOrigin, string srcCardinality, string tgtCardinality)
        {
            var connection = new CanvasConnection
            {
                SourceBox = b1,
                TargetBox = b2,
                RelationshipType = rel,
                RelationOrigin = relOrigin,
                SourceCardinality = srcCardinality,
                TargetCardinality = tgtCardinality
            };

            _connections.Add(connection);
        }

        public void RemoveConnection(Box b1, Box b2)
        {
            _connections.RemoveAll(conn =>
                (conn.SourceBox == b1 && conn.TargetBox == b2) ||
                (conn.SourceBox == b2 && conn.TargetBox == b1));
        }

        public void ClearAll()
        {
            _boxes.Clear();
            _connections.Clear();
        }

        public void CheckLinesRels(Box b1, Box b2, string rel, string relOrigin, Graphics g, string srcCardinality, string tgtCardinality)
        {
            using (Pen p = new Pen(Color.Black, 2))
            {
                // Reseta as terminações para o padrão por segurança
                p.StartCap = LineCap.Flat;
                p.EndCap = LineCap.Flat;
              

                // Path para criar o losango de Agregação e Composição
                GraphicsPath diamondPath = new GraphicsPath();
                diamondPath.AddPolygon(new PointF[] {
                    new PointF(0, 0),
                    new PointF(-4, 4),
                    new PointF(-8, 0),
                    new PointF(-4, -4)
                });

                // Configura a caneta baseada na relação (Incluindo a Seta ou Losango)
                switch (rel)
                {
                    case "Ação":
                        p.Color = Color.Black;
                        p.DashStyle = DashStyle.Solid;
                        p.CustomEndCap = new AdjustableArrowCap(5, 5, true); // Seta preenchida
                        break;

                    case "Condição":
                        p.Color = Color.Black;
                        p.DashStyle = DashStyle.Dash;
                        p.CustomEndCap = new AdjustableArrowCap(5, 5, false); // Seta vazada
                        break;

                    case "Associação":
                        p.Color = Color.Blue;
                        p.DashStyle = DashStyle.Solid;
                        p.EndCap = LineCap.ArrowAnchor; // Seta simples
                        break;

                    case "Agregação":
                        p.Color = Color.Green;
                        p.DashStyle = DashStyle.Dash;
                        p.CustomEndCap = new CustomLineCap(null, diamondPath) { BaseInset = 8 }; // Losango vazado
                        break;

                    case "Composição":
                        p.Color = Color.Red;
                        p.DashStyle = DashStyle.Solid;
                        p.Width = 3;
                        var filledDiamond = new CustomLineCap(diamondPath, null) { BaseInset = 8 };
                        filledDiamond.SetStrokeCaps(LineCap.Round, LineCap.Round);
                        p.CustomEndCap = filledDiamond; // Losango preenchido
                        break;

                    case "Generalização":
                        p.Color = Color.Purple;
                        p.DashStyle = DashStyle.DashDot;
                        GraphicsPath arrowPath = new GraphicsPath();
                        arrowPath.AddPolygon(new PointF[] { new PointF(0, 0), new PointF(-6, 4), new PointF(-6, -4) });
                        p.CustomEndCap = new CustomLineCap(null, arrowPath) { BaseInset = 6 }; // Seta triangular vazada
                        break;
                }

                // Encontra um elemento para desenhar as linhas
                Elemento element = _boxes.FirstOrDefault(b => b is Elemento) as Elemento;
                if (element == null) return;

                // Controle da origem da relação: Inverte a seta (Caneta) se o fluxo for do Destino para a Origem
                Pen drawPen = p;
                Pen reversePen = null;

                if (relOrigin == "target")
                {
                    reversePen = (Pen)p.Clone();
                    reversePen.CustomStartCap = p.CustomEndCap;
                    reversePen.CustomEndCap = p.CustomStartCap;
                    reversePen.StartCap = p.EndCap;
                    reversePen.EndCap = p.StartCap;
                    drawPen = reversePen;
                }

                // Lógica de colisão e desenho da linha com DrawLines
                if (b1.PositionX < b2.PositionX && b1.PositionX + b1.Width <= b2.PositionX)
                {
                    element.DrawLineB1LeftB2(b1, b2, g, drawPen);
                }
                else if (b1.PositionX > b2.PositionX && b2.PositionX + b2.Width <= b1.PositionX)
                {
                    element.DrawLineB1RightB2(b1, b2, g, drawPen);
                }
                else if (b1.PositionY < b2.PositionY)
                {
                    element.DrawLineB1OverB2(b1, b2, g, drawPen);
                }
                else if (b1.PositionY > b2.PositionY)
                {
                    element.DrawLineB1UnderB2(b1, b2, g, drawPen);
                }

                // Libera a caneta secundária caso tenha sido alocada
                if (reversePen != null)
                {
                    reversePen.Dispose();
                }

                // Desenha as Multiplicidades da mesma maneira
                element.DrawMultiplicity(b1, b2, g, srcCardinality, "source");
                element.DrawMultiplicity(b1, b2, g, tgtCardinality, "target");
            }
        }

        // Método para reconstruir referências após carregar do JSON
        public void RebuildReferences()
        {
            // Para cada conexão, reconstrói as referências às boxes
            foreach (var connection in _connections)
            {
                if (connection.SourceBox != null && connection.TargetBox != null)
                {
                    // Encontra as boxes correspondentes na lista atual
                    connection.SourceBox = _boxes.FirstOrDefault(b => b.Id == connection.SourceBox.Id);
                    connection.TargetBox = _boxes.FirstOrDefault(b => b.Id == connection.TargetBox.Id);
                }
            }
        }

        private void DeleteSelectedBox()
        {
            foreach (Box box in _boxes.ToList()) // Usar ToList() para evitar modificação durante iteração
            {
                if (box.IsSelected)
                {
                    var result = MessageBox.Show(
                        $"Deseja apagar a box '{box.OriginalName}'?",
                        "Confirmar Exclusão",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        RemoveBoxFromList(box);
                    }
                }
            }
        }
    }

    // Classe CanvasConnection para representar as conexões (nome diferente para evitar conflito)
    [Serializable]
    public class CanvasConnection
    {
        public Box SourceBox { get; set; }
        public Box TargetBox { get; set; }
        public string RelationshipType { get; set; }
        public string RelationOrigin { get; set; }
        public string SourceCardinality { get; set; }
        public string TargetCardinality { get; set; }

        public CanvasConnection()
        {
            SourceCardinality = "1"; // ← VALOR PADRÃO
            TargetCardinality = "1"; // ← VALOR PADRÃO
            RelationOrigin = "source";
        }
    }
}
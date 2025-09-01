using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;



namespace LEML_StudioBr.Objetos
{

    [Serializable]
    public abstract class Box
    {
        public Guid Id { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string TopText { get; set; }
        public string BottomText { get; set; }
        public string OriginalName { get; set; }
        public int TipoAmbiente { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }

        [JsonIgnore]
        public Image Icon { get; set; }

        [JsonIgnore]
        public Image HeaderImage { get; set; }

        [JsonIgnore]
        public Bitmap BitmapProperty { get; set; }

        [JsonIgnore]
        public Image ImageProperty { get; set; }

        // Adicione qualquer outra propriedade de imagem que possa existir
        [JsonIgnore]
        public object AnyImageProperty { get; set; }


        protected int MinWidth => 200;
        protected int MinHeight => 200;
        protected int MaxWidth => 800;
        protected int MaxHeight => 800;

        public Brush ColorBrush;
        public string BoxType { get; set; }
        public string Name { get; set; }

        public StringFormat _formatCenter;



        // Construtor padrão necessário para JSON
        public Box()
        {
            Id = Guid.NewGuid();
            // Inicialize outras propriedades com valores padrão se necessário
            Width = 100;
            Height = 60;
            TopText = "Não Definido";
            BottomText = "Não Definido";
        }

       public Box(int x, int y, string name, int tpAmbiente)
        {
            PositionX = x;
            PositionY = y;
            Id = Guid.NewGuid();
            Width = 200;
            Height = 200;
            this.TipoAmbiente = tpAmbiente; 
            BoxType = "Box";
            switch (tpAmbiente)
            {
                case 1: //Ambiente Síncrono
                    ColorBrush = Brushes.Cornsilk;
                    break;
                case 2: //Ambiente Síncrono
                    ColorBrush = Brushes.Aquamarine;
                    break;
                case 3: //Ambiente Assíncrono
                    ColorBrush = Brushes.SkyBlue;
                    break;
                case 4: //Ambiente Simulado
                    ColorBrush = Brushes.MediumSlateBlue;
                    break;
            }         

          
            OriginalName = name;

            
            _formatCenter = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            
        }

        public virtual void RebuildImages()
        {
            // As subclasses podem implementar isso para reconstruir imagens
        }

        public Box(int positionX, int positionY, int width, int height, string originalName, string boxType, string colorName)
        {
            PositionX = positionX;
            PositionY = positionY;
            Width = width;
            Height = height;
            OriginalName = originalName;
            BoxType = boxType;
            Name = originalName;
            ColorBrush = new SolidBrush(Color.FromName(colorName));
            
            _formatCenter = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            
        }

        public virtual void Select()
        {
            //ColorBrush = Brushes.LightBlue;
            Cursor.Current = Cursors.UpArrow;
        }

        public virtual void Unselect()
        {
            Cursor.Current = Cursors.Default;
            //ColorBrush = Brushes.LightSkyBlue;
            Name = OriginalName;
        }

        public void Move(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }

        public void Resize(int w, int h)
        {
            if (w < MinWidth)
                w = MinWidth;

            if (h < MinHeight)
                h = MinHeight;

            if (w > MaxWidth)
                w = MaxWidth;

            if (h > MaxHeight)
                h = MaxHeight;

           

            Width = w;
            Height = h;
   
        }

        private string GetColor()
        {
            SolidBrush brush = (SolidBrush)ColorBrush;
            return brush.Color.Name;
        }

        public virtual void Draw(Graphics g , float zoomLevel = 1.0f)
        {

            g.TranslateTransform(PositionX * zoomLevel, PositionY * zoomLevel);

            // Aplica zoom nas dimensões
            int zoomedWidth = (int)(Width * zoomLevel);
            int zoomedHeight = (int)(Height * zoomLevel);






            g.FillRectangle(ColorBrush, 0, 0, zoomedWidth, zoomedHeight);
            g.FillRectangle(Brushes.Black, zoomedWidth - 10 * zoomLevel, zoomedHeight - 10 * zoomLevel,
                          10 * zoomLevel, 10 * zoomLevel);

            // Ajusta o tamanho da fonte baseado no zoom
            float fontSize = 10 * zoomLevel;
            if (fontSize < 6) fontSize = 6; // Tamanho mínimo
            if (fontSize > 24) fontSize = 24; // Tamanho máximo

            using (Font zoomFont = new Font("Segoe UI", fontSize, FontStyle.Bold))
            {
                g.DrawString(Name, zoomFont, Brushes.Black, zoomedWidth / 2, zoomedHeight * 0.1f, _formatCenter);
            }

            g.DrawLine(Pens.Black, 0, zoomedHeight * 0.2f, zoomedWidth, zoomedHeight * 0.2f);
            g.ResetTransform();
       
            // Set coords to begin in top-left corner of the box
            g.TranslateTransform(PositionX, PositionY);

            // Draw Box
            g.FillRectangle(ColorBrush, 0, 0, Width, Height);
            g.FillRectangle(Brushes.Black, Width - 10, Height - 10, 10, 10);

            // Name of the box (Class/Interface Name)
            g.DrawString(Name, new Font("Segoe UI", 10, FontStyle.Bold), Brushes.Black, Width / 2, Height * 0.1f, _formatCenter);

            // Line under the name
            g.DrawLine(Pens.Black, 0, Height * 0.2f, Width, Height * 0.2f);

            if (IsSelected)
            {
                using (Pen redPen = new Pen(Color.Red, 2))
                {
                    g.DrawLine(redPen, 5, 5, Width - 5, Height - 5);
                    g.DrawLine(redPen, Width - 5, 5, 5, Height - 5);
                }
            }


            // Reset coords
            g.ResetTransform();
        }

        public bool IsInCollision(int x, int y)
        {
            return x > PositionX && x <= PositionX + Width
                && y > PositionY && y <= PositionY + Height;
        }

        public bool IsInCollisionWithHeader(int x, int y)
        {
            return x > PositionX && x <= PositionX + Width
                && y > PositionY && y <= PositionY + Height * 0.2f;
        }

        public bool IsInCollisionWithCorner(int x, int y)
        {
            return x > PositionX + Width - 10 && x <= PositionX + Width
                && y > PositionY + Height - 10 && y <= PositionY + Height;
        }

        public void UpdateBoxName() => Name = OriginalName;

       public override string ToString()
        {
            return $"{OriginalName}";
        }
        public void DrawLineB1RightB2(Box b1, Box b2, Graphics g, Pen p)
        {
            int distanceX = (int) (b1.PositionX - (b2.PositionX + b2.Width));
            Point p1 = new Point((int) b1.PositionX, (int) (b1.PositionY + b1.Height / 2));
            Point p2 = new Point((int) (b2.PositionX + b2.Width), (int) (b2.PositionY + b2.Height / 2));

            Point pt1 = new Point((int) (b2.PositionX + (distanceX / 2)), (int) (b2.PositionY + (b2.Height / 2)));
            Point pt2 = new Point((int) (b1.PositionX - (distanceX / 2) + b1.Width), (int) (b1.PositionY + (b1.Height / 2)));

            g.DrawLine(p, p1, pt2);
            g.DrawLine(p, pt2, pt1);
            g.DrawLine(p, p2, pt1);

        }

        public void DrawLineB1LeftB2(Box b1, Box b2, Graphics g, Pen p)
        {
            int distanceX = (int) (b1.PositionX - b2.PositionX + b2.Width);
            Point p1 = new Point((int)(b1.PositionX + b1.Width), (int) (b1.PositionY + b1.Height / 2));
            Point p2 = new Point((int) b2.PositionX, (int)(b2.PositionY + b2.Height / 2));

            Point pt1 = new Point((int)((int)b2.PositionX + (distanceX / 2)), (int)((int)(b2.PositionY + (b2.Height / 2))));
            Point pt2 = new Point((int)((int)b1.PositionX - (distanceX / 2) + (int)b1.Width), (int)((int)(b1.PositionY + (b1.Height / 2))));

            g.DrawLine(p, p1, pt2);
            g.DrawLine(p, pt2, pt1);
            g.DrawLine(p, p2, pt1); 
        }

        public void DrawLineB1OverB2(Box b1, Box b2, Graphics g, Pen p)
        {
            int distanceY = (int) b2.PositionY - (int) (b1.PositionY - b2.Height);

            Point p1 = new Point((int)( b1.PositionX + b1.Width / 2), (int) (b1.PositionY + b1.Height));
            Point p2 = new Point((int) (b2.PositionX + b2.Width / 2), (int) b2.PositionY);

            Point pt1 = new Point((int) (b1.PositionX + b1.Width / 2), (int) (b1.PositionY + b1.Height + (distanceY / 2)));
            Point pt2 = new Point((int) (b2.PositionX + b2.Width / 2), (int) (b2.PositionY - distanceY / 2));

            g.DrawLine(p, p1, pt1);
            g.DrawLine(p, pt2, pt1);
            g.DrawLine(p, pt2, p2);
        }

        public void DrawLineB1UnderB2(Box b1, Box b2, Graphics g, Pen p)
        {
            int distanceY = (int) b1.PositionY - (int)(b1.Height - b2.PositionY);

            Point p1 = new Point((int) (b1.PositionX + b1.Width / 2), (int) b1.PositionY);
            Point p2 = new Point((int) (b2.PositionX + b2.Width / 2), (int)(b2.PositionY + b2.Height));

            Point pt1 = new Point((int) (b1.PositionX + b1.Width / 2), (int)(b1.PositionY - distanceY / 2));
            Point pt2 = new Point((int) (b2.PositionX + b2.Width / 2), (int)(b2.PositionY + b2.Height + (distanceY / 2)));


            g.DrawLine(p, p1, pt1);
            g.DrawLine(p, pt2, pt1);
            g.DrawLine(p, pt2, p2);
        }
        public void DrawLineThroughPoint(Box b1, Box b2, List<Point> points, Graphics g, Pen p)
        {

        }
        public void DrawAssociation(Box b1, Box b2, Graphics g, Pen p, string rel, string relOrigin)
        {
            if (relOrigin == "source")
            {
                if (b1.PositionX < b2.PositionX && b1.PositionX + b1.Width <= b2.PositionX)
                {
                    Point[] points =
                    {
                        new Point((int) (b1.PositionX + b1.Width) ,(int) (b1.PositionY + b1.Height / 2)),
                        new Point((int) (b1.PositionX + b1.Width + 15), (int) ( b1.PositionY + b1.Height / 2 + 8)),
                        new Point((int) (b1.PositionX + b1.Width + 15), (int) (b1.PositionY + b1.Height / 2 -8))
                    };

                    g.DrawLine(p, points[0], points[1]);
                    g.DrawLine(p, points[0], points[2]);
                }
                else if (b1.PositionX > b2.PositionX && b2.PositionX + b2.Width <= b1.PositionX)
                {
                    Point[] points =
                    {
                        new Point((int) b1.PositionX, (int) (b1.PositionY + b1.Height / 2)),
                        new Point((int) b1.PositionX - 15, (int) (b1.PositionY + b1.Height / 2 + 8)),
                        new Point((int) b1.PositionX - 15, (int) (b1.PositionY + b1.Height / 2 -8))
                    };
                    
                    g.DrawLine(p, points[0], points[1]);
                    g.DrawLine(p, points[0], points[2]);
                }
                else if (b1.PositionY < b2.PositionY)
                {
                    Point[] points =
                    {
                        new Point((int)(b1.PositionX + b1.Width / 2),(int)(b1.PositionY + b1.Height)),
                        new Point((int) (b1.PositionX + b1.Width / 2 + 8), (int) (b1.PositionY + b1.Height + 15)),
                        new Point((int) (b1.PositionX + b1.Width / 2 - 8), (int) (b1.PositionY + b1.Height +15))
                    };
                   
                    g.DrawLine(p, points[0], points[1]);
                    g.DrawLine(p, points[0], points[2]);
                }
                else if (b1.PositionY > b2.PositionY)
                {
                    Point[] points =
                    {
                        new Point((int)(b1.PositionX + b1.Width / 2),(int)(b1.PositionY)),
                        new Point((int)(b1.PositionX + b1.Width / 2 + 8), (int) b1.PositionY - 15), //top
                        new Point((int)(b1.PositionX + b1.Width / 2 - 8), (int) b1.PositionY -15) //bottom
					};
                    
                    g.DrawLine(p, points[0], points[1]);
                    g.DrawLine(p, points[0], points[2]);
                }
            }
            else if (relOrigin == "target")
            {
                if (b2.PositionX < b1.PositionX && b2.PositionX + b2.Width <= b1.PositionX)
                {
                    Point[] points =
                    {
                        new Point((int)((int)(b2.PositionX + b2.Width)),(int)(b2.PositionY + b2.Height / 2)),
                        new Point((int) (b2.PositionX + b2.Width + 15), (int)(b2.PositionY + b2.Height / 2 + 8)), //top
                        new Point((int) (b2.PositionX + b2.Width + 15), (int)(b2.PositionY + b2.Height / 2 -8)) //bottom
					};
                    
                    g.DrawLine(p, points[0], points[1]);
                    g.DrawLine(p, points[0], points[2]);
                }
                else if (b2.PositionX > b1.PositionX && b1.PositionX + b1.Width <= b2.PositionX)
                {
                    Point[] points =
                    {
                        new Point((int) b2.PositionX, (int) (b2.PositionY + b2.Height / 2)),
                        new Point((int) b2.PositionX - 15, (int) (b2.PositionY + b2.Height / 2 + 8)),
                        new Point((int) b2.PositionX - 15, (int) (b2.PositionY + b2.Height / 2 -8))
                    };
                    
                }
                else if (b2.PositionY < b1.PositionY)
                {
                    Point[] points =
                    {
                        new Point((int) (b2.PositionX + b2.Width / 2), (int)(b2.PositionY + b2.Height)),
                        new Point((int) (b2.PositionX + b2.Width / 2 + 8), (int) (b2.PositionY + b2.Height + 15)),
                        new Point((int)((int)(b2.PositionX + b2.Width / 2 - 8)),(int)(b2.PositionY + b2.Height + 15))
                    };
                     
                    g.DrawLine(p, points[0], points[1]);
                    g.DrawLine(p, points[0], points[2]);
                }
                else if (b2.PositionY > b1.PositionY)
                {
                    Point[] points =
                    {
                        new Point((int)(b2.PositionX + b2.Width / 2),(int)(b2.PositionY)),
                        new Point((int)(b2.PositionX + b2.Width / 2 + 8),(int)(b2.PositionY - 15)),
                        new Point((int)((int)(b2.PositionX + b2.Width / 2 - 8)),(int)(b2.PositionY - 15))
                    };
                    /*
					Point rotatePoint = new Point(b1.PositionX + b1.Width / 2, b1.PositionY + b1.Height);

					double angle = Math.Atan2(rotatePoint.Y - points[0].Y, rotatePoint.X - points[0].X);
					angle += (90 * (Math.PI / 180));

					Point rotPt1 = RotatePoint(points[1], points[0], angle);
					Point rotPt2 = RotatePoint(points[2], points[0], angle);

					g.DrawLine(p, rotPt1, points[0]);
					g.DrawLine(p, rotPt2, points[0]);
					*/
                    g.DrawLine(p, points[0], points[1]);
                    g.DrawLine(p, points[0], points[2]);
                }
            }
        }
       


        private Point RotatePoint(Point pt, Point pivot, double angle)
        {
            int x = pivot.X + (int)((pt.X - pivot.X) * Math.Cos(angle) - (pt.Y - pivot.Y) * Math.Sin(angle));
            int y = pivot.Y + (int)((pt.X - pivot.X) * Math.Sin(angle) + (pt.Y - pivot.Y) * Math.Cos(angle));
            return new Point(x, y);
        }

        public void DrawMultiplicity(Box b1, Box b2, Graphics g, string cardinality, string origin)
        {
            if (origin == "source")
            {
                if (b1.PositionX < b2.PositionX && b1.PositionX + b1.Width <= b2.PositionX)
                {
                    Point pt = new Point((int)(b1.PositionX + b1.Width + 50), (int)(b1.PositionY + b1.Height / 2 - 50));
                    g.DrawString(cardinality, new Font("Segoe UI", 16), Brushes.Black, pt);
                }
                else if (b1.PositionX > b2.PositionX && b2.PositionX + b2.Width <= b1.PositionX)
                {
                    Point pt = new Point((int)((int)(b1.PositionX - 50)), (int)(b1.PositionY + b1.Height / 2 - 50));
                    g.DrawString(cardinality, new Font("Segoe UI", 16), Brushes.Black, pt);
                }
                else if (b1.PositionY < b2.PositionY)
                {
                    Point pt = new Point((int)(b1.PositionX + b1.Width / 2 + 30), (int)(b1.PositionY + b1.Height + 30));
                    g.DrawString(cardinality, new Font("Segoe UI", 16), Brushes.Black, pt);
                }
                else if (b1.PositionY > b2.PositionY)
                {
                    Point pt = new Point((int)(b1.PositionX + b1.Width / 2 + 30), (int)(b1.PositionY - 30));
                    g.DrawString(cardinality, new Font("Segoe UI", 16), Brushes.Black, pt);
                }
            }
            else if (origin == "target")
            {
                if (b2.PositionX < b1.PositionX && b2.PositionX + b2.Width <= b1.PositionX)
                {
                    Point pt = new Point((int)(b2.PositionX + b2.Width + 50), (int)(b2.PositionY + b2.Height / 2 - 50));
                    g.DrawString(cardinality, new Font("Segoe UI", 16), Brushes.Black, pt);
                }
                else if (b2.PositionX > b1.PositionX && b1.PositionX + b1.Width <= b2.PositionX)
                {
                    Point pt = new Point((int)((int)(b2.PositionX - 50)), (int)(b2.PositionY + b2.Height / 2 - 50));
                    g.DrawString(cardinality, new Font("Segoe UI", 16), Brushes.Black, pt);
                }
                else if (b2.PositionY < b1.PositionY)
                {
                    Point pt = new Point((int)(b2.PositionX + b2.Width / 2 + 30), (int)(b2.PositionY + b2.Height + 30));
                    g.DrawString(cardinality, new Font("Segoe UI", 16), Brushes.Black, pt);
                }
                else if (b2.PositionY > b1.PositionY)
                {
                    Point pt = new Point((int)(b2.PositionX + b2.Width / 2 + 30), (int)(b2.PositionY - 30));
                    g.DrawString(cardinality, new Font("Segoe UI", 16), Brushes.Black, pt);
                }
            }
        }


        // Na classe Box, adicione esta propriedade
        public event EventHandler<MouseEventArgs> BoxClicked;

        // Método para invocar o evento
        protected virtual void OnBoxClicked(MouseEventArgs e)
        {
            BoxClicked?.Invoke(this, e);
        }

        private void Box_BoxClicked(object sender, MouseEventArgs e)
        {
            var clickedBox = sender as Box;

            if (e.Button == MouseButtons.Left)
            {
                 
            }
        }


        public virtual void ApplyZoom(float zoomLevel)
        {
            // As propriedades Position, Width e Height já são usadas no draw
            // O zoom será aplicado durante o desenho
        }

        // Método para obter o retângulo com zoom aplicado
        public virtual Rectangle GetZoomedRectangle(float zoomLevel)
        {
            return new Rectangle(
                (int)(PositionX * zoomLevel),
                (int)(PositionY * zoomLevel),
                (int)(Width * zoomLevel),
                (int)(Height * zoomLevel)
            );
        }



    }

    [Serializable]
    public class Connection
    {
        public Guid SourceId { get; set; }
        public Guid TargetId { get; set; }
        public string RelationshipType { get; set; }
        public string RelationOrigin { get; set; }
        public string SourceCardinality { get; set; }
        public string TargetCardinality { get; set; }

        [JsonIgnore]
        public Box SourceBox { get; set; }

        [JsonIgnore]
        public Box TargetBox { get; set; }
    }




}

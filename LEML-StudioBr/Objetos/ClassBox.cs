using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;


namespace LEML_StudioBr.Objetos
{
    [Serializable]
    public class ClassBox:Box
    {
        //public override string BoxType { get; set; } = "Class";

        public ClassBox() : base()
        {
            // Valores padrão
        }


        public ClassBox(int x, int y, string name, int tipo) : base(x, y, name, tipo)
        {
            PositionX = x;
            PositionY = y;

            Width = 200;
            Height = 200;
            //ColorBrush = Brushes.LightSkyBlue;
            Name = name;

            _formatCenter = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
        }

              
    }
}

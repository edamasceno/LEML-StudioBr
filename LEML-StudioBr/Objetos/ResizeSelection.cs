using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEML_StudioBr.Objetos
{
    public class ResizeSelection : Selection
    {
        public ResizeSelection(Box box, int x, int y)
            : base(box, x, y)
        { }

        public override void Move(int x, int y)
        {
            int dx = (int) _selectedBox.Width - _relativeX;
            int dy = (int) _selectedBox.Height - _relativeY;

            _selectedBox.Resize((int)(x - _selectedBox.PositionX + dx), (int)(y - _selectedBox.PositionY + dy));

            _relativeX = (int) _selectedBox.Width - dx;
            _relativeY = (int) _selectedBox.Height - dy;
        }
    }
}

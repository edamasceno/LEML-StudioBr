using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEML_StudioBr.Objetos
{
    public abstract class Selection
    {
        protected Box _selectedBox;
        protected int _relativeX;
        protected int _relativeY;

        public Selection(Box box, int x, int y)
        {
            _selectedBox = box;
            _relativeX = (int)(x - box.PositionX);
            _relativeY = (int)(y - box.PositionY);
        }

        public void Select()
        {
            _selectedBox.Select();
        }

        public void Unselect()
        {
            _selectedBox.Unselect();
        }

        public abstract void Move(int x, int y);
    }
}

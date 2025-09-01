using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEML_StudioBr.Objetos
{
    public class MoveSelection : Selection
    {
        public MoveSelection(Box box, int x, int y)
            : base(box, x, y)
        { }

        public override void Move(int x, int y)
        {
            _selectedBox.Move(x - _relativeX, y - _relativeY);
        }
    }
}

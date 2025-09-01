using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEML_StudioBr.Objetos
{
    public  class ListManipulator<T>
    {
        private List<T> _list { get; set; }

        public ListManipulator(List<T> list)
        {
            _list = list;
        }

        public void MoveToLast(int index)
        {
            if (index == _list.Count - 1) return;

            T selectedItem = _list[index];
            _list.RemoveAt(index);

            int lastIndex = _list.Count;
            _list.Insert(lastIndex, selectedItem);
        }
    }
}

using LEML_StudioBr.Objetos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LEML_StudioBr.Forms
{

    public partial class frmRelacao : Form
    {
        public Canvas theCanvas;
        public frmRelacao(Canvas c)
        {
            InitializeComponent();
            foreach (var Ele in c.GetBoxes())
            {
                if (Ele is Elemento)
                {
;                    string itemText = Ele.ToString();
                    cboOrigem.Items.Add(itemText);
                    cboDestino.Items.Add(itemText);
                }
            }
            theCanvas = c;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            Elemento el1 = null;
            Elemento el2 = null;    

            foreach (var Ele in theCanvas.GetBoxes())
            {
                if (Ele is Elemento)
                {
                    if (Ele.ToString() == cboOrigem.Text)
                        el1 = (Elemento)Ele;
                    if (Ele.ToString() == cboDestino.Text)
                        el2 = (Elemento)Ele;
                }
            }
            theCanvas.AddConnection(el2, el1, cboTipo.Text, "source", cboCardTarget.Text, cboCardSource.Text);
            this.Close();    
            

        }
    }
}

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
    public partial class frmInfoAgente : frmInfoElemento
    {
        public frmInfoAgente(Agente Ag) : base(Ag)
        {
            InitializeComponent();
        }

        protected override void LoadElemento(Elemento Ele)
        {
            base.LoadElemento(Ele);
            
            Agente Ag = (Agente)Ele;
            //txtWhat.Text = Ag.TopText;
            //txtHow.Text = Ag.BottomText;
        }

        protected override void SaveElemento()
        {
            base.SaveElemento();

            theEle.TopText = txtWhat.Text;
            theEle.BottomText = txtHow.Text;
        }

        protected override void txtWhat_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.LightBlue;
        }

        protected override void txtWhat_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.LightGray;
        }

        protected override void frmInfoElemento_Load(object sender, EventArgs e)
        {

        }

        //protected override void btnCancelar_Click(object sender, EventArgs e)
        //{
        //    theEle.SetElemento("", "");

        //    this.DialogResult = DialogResult.Cancel;
        //    this.Close();
        //}

        //protected override void btnConfirmar_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtWhat.Text))
        //    {
        //        MessageBox.Show("O campo 'O que?' não pode estar vazio.", "Atenção",
        //                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        txtWhat.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(txtHow.Text))
        //    {
        //        MessageBox.Show("O campo 'Como?' não pode estar vazio.", "Atenção",
        //                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        txtHow.Focus();
        //        return;
        //    }

        //    // Atribui às propriedades
        //    theEle.SetElemento(txtWhat.Text, txtHow.Text);

        //    this.DialogResult = DialogResult.OK;
        //    this.Close();
        //}
    }
}

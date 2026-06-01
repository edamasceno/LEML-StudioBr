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
    public partial class frmInfoElemento : Form
    {
        protected Elemento theEle;

        public Elemento Result => theEle;

        public frmInfoElemento(Elemento Ele)
        {
            InitializeComponent();
            theEle = Ele;
            LoadElemento(Ele);
            //txtWhat.Text = Ele.TopText;
            //txtHow.Text = Ele.BottomText;
        }

        protected virtual void LoadElemento(Elemento Ele)
        {
            txtWhat.Text = Ele.TopText;
            txtHow.Text = Ele.BottomText;
        }

        protected virtual void SaveElemento()
        {
            theEle.TopText = txtWhat.Text;
            theEle.BottomText = txtHow.Text;
        }

        protected virtual void txtWhat_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.LightYellow;
        }

        protected virtual void txtWhat_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.White;
        }

        protected virtual void frmInfoElemento_Load(object sender, EventArgs e)
        {
            
        }

        protected virtual void btnCancelar_Click(object sender, EventArgs e)
        {
            theEle.SetElemento("", "");
            
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected virtual void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWhat.Text))
            {
                MessageBox.Show("O campo 'O que?' não pode estar vazio.", "Atenção",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtWhat.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtHow.Text))
            {
                MessageBox.Show("O campo 'Como?' não pode estar vazio.", "Atenção",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHow.Focus();
                return;
            }

            // Atribui às propriedades
            theEle.SetElemento(txtWhat.Text, txtHow.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

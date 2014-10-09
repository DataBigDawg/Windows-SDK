using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accela.Mobile.CustomWizard
{
    public partial class frmUserInput : Form
    {
        public frmUserInput()
        {
            InitializeComponent();
        }

        public string AppId
        {
            get;
            private set;
        }

        public string AppSecret
        {
            get;
            private set;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AppId = txtAppId.Text.Trim();
            AppSecret = txtAppSecret.Text.Trim();

            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAppId.Text.Trim()) ||
                string.IsNullOrEmpty(txtAppSecret.Text.Trim()))
            {
                btnOK.Enabled = false;
            }
            else
            {
                btnOK.Enabled = true;
            }
        }

        public void SetReplacementsDictionary(IDictionary<string, string> dic)
        {
            txtReplacementsDictionary.Visible = true;

            foreach (var item in dic)
            {
                txtReplacementsDictionary.Text += string.Format("{0}:{1}", item.Key, item.Value);
                txtReplacementsDictionary.Text += Environment.NewLine;
            }
        }

    }
}

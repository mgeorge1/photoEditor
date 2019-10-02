using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace photoEditor1
{
    public partial class ProgressDialogBox : Form
    {
        public int ProgressValue
        {
            set { progressBar.Value = value; }
        }

        public ProgressDialogBox()
        {
            InitializeComponent();
            CenterToParent();
        }
        public event EventHandler<EventArgs> Canceled;

        public void ProgressDialogBoxCancelButton_Click(object sender, EventArgs e)
        {
            EventHandler<EventArgs> ea = Canceled;

            if (ea != null)
                ea(this, e);
        }

        private void ProgressDialogBox_Load(object sender, EventArgs e)
        {

        }
    }
}

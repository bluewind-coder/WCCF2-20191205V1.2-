using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormChatClient
{
    public partial class CheckForm : Form
    {
        public CheckForm()
        {
            InitializeComponent();
        }

        private void CheckForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}

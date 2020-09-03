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
    public partial class Login : Form
    {
        string man = "blue";
        string woman = "jude";

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            login();
        }

        private void login()
        {
            if (txtPWD.Text.Equals("8295468") || txtPWD.Text.Equals("13968427"))
            {
                if (txtPWD.Text.Equals("8295468"))
                {

                    Form1 form1 = new Form1(woman);
                    form1.Show();
                }

                if (txtPWD.Text.Equals("13968427"))
                {
                    Form1 form1 = new Form1(man);
                    form1.Show();
                }

            }
            else
            {
                CheckForm checkForm = new CheckForm();
                checkForm.Show();
            }
            this.Hide();
        }

        private void txtPWD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                // When the user presses both the 'Alt' key and 'F' key,
                // KeyPreview is set to False, and a message appears.
                // This message is only displayed when KeyPreview is set to False.
                //this.KeyPreview = true;
                //MessageBox.Show("KeyPreview is False, and this is from the CONTROL.");
                e.SuppressKeyPress = true;//回车事件已经处理完不再响应了
                login();
            }
        }
    }
}

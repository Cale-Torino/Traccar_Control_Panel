using System;
using System.Windows.Forms;

namespace Traccar_Control_Panel
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Close form
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Open link to https://tutorials.techrad.co.za/
            System.Diagnostics.Process.Start("https://tutorials.techrad.co.za/");
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nppTranslateCS.Forms
{
    public partial class frmBingCredentials : Form
    {

        public TranslateSettingsController controller;
 
        public frmBingCredentials()
        {
            InitializeComponent();
        }

        public void setController(TranslateSettingsController controller)
        {
            this.controller = controller;
        }

        public void setBINGClientID(String id)
        {
            this.textBox1.Text = id;
        }

        public void setBINGClientSecret(String secret)
        {
            this.textBox2.Text = secret;
        }

        public String getBINGClientID()
        {
            return this.textBox1.Text;
        }

        public String getBINGClientSecret()
        {
            return this.textBox2.Text;
        }

        private void frmBingCredentials_Load(object sender, EventArgs e)
        {
            controller.onLoad(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void frmBingCredentials_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.onClose(this);
            e.Cancel = true;
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String link = e.Link.LinkData as string;
#if DEBUG
            MessageBox.Show(link);
#endif
            System.Diagnostics.Process.Start("IExplore.exe", link); 
        
        }
    }
}

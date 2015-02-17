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
            //System.Diagnostics.Process.Start("IExplore.exe", link); 
            //TODO fix perm why above does not work.

            System.Diagnostics.Process.Start("http://blogs.msdn.com/b/translation/p/gettingstarted1.aspx");
        
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                //controller.updateEngine(TranslateSettingsModel.Engine.MYMEMORY);
                this.groupBoxMyMemory.Visible = true;
                this.groupBoxBINGSettings.Visible = false;
            }
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                //controller.updateEngine(TranslateSettingsModel.Engine.BING);
                this.groupBoxMyMemory.Visible = false;
                this.groupBoxBINGSettings.Visible = true;
            }
        }

        public int getSelectedEngineIndex()
        {
            if(this.radioButton1.Checked)
            {
                return 0;
            }
            else if(this.radioButton2.Checked)
            {
                return 1;
            }
            return 0;
        }

        public void setEngineSelection(int index)
        {
            switch(index)
            {
                case 0:
                    radioButton1.Checked = true;
                    break;
                case 1:
                    radioButton2.Checked = true;
                    break;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String link = e.Link.LinkData as string;

            //System.Diagnostics.Process.Start("IExplore.exe", link); 
            //TODO fix perm why above does not work.

            System.Diagnostics.Process.Start("http://mymemory.translated.net/doc/spec.php");
        }

        public String getEmail()
        {
            return this.textBoxEmail.Text;
        }

        public void setEmail(String email)
        {
            this.textBoxEmail.Text = email;
        }
    }
}

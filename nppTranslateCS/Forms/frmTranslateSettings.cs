using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.UI;

namespace nppTranslateCS.Forms
{
    public partial class frmTranslateSettings : Form
    {
        public TranslateSettings bingSettings;
 
        public frmTranslateSettings(TranslateSettings bs)
        {
            bingSettings = bs;
            InitializeComponent();  
        }

        private void TranslateSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            bingSettings.setClientCredentials( new Pair(this.textBox1.Text,this.textBox2.Text) );

            int fromIndex = this.from.SelectedIndex;
            int toIndex = this.to.SelectedIndex;

            string fromCode = "";
            string toCode = "en";
            if(fromIndex > 0)
            {
                fromCode = (string) bingSettings.getAllLanguages().ToArray()[fromIndex-1].First;
            }

            if (toIndex > -1)
            {
                toCode = (string)bingSettings.getAllLanguages().ToArray()[toIndex].First;
            }

            bingSettings.setLanguagePreference(new Pair(fromCode, toCode));
            

            e.Cancel = true;
            this.Hide();
        }

        private void frmBingSettings_Load(object sender, EventArgs e)
        {
            Pair clientCred = this.bingSettings.getClientCredentials();

            if (clientCred != null)
            {
                this.textBox1.Text = (string)clientCred.First;
                this.textBox2.Text = (string)clientCred.Second;
            }
            populateLanguages();
        }

        private void populateLanguages()
        {
            string preferredFrom = (string) bingSettings.getLanguagePreference().First;
            string preferredTo = (string) bingSettings.getLanguagePreference().Second;

            int fromIndex = 0, toIndex = -1;

            int i = 0;

            foreach (Pair codeNamePair in bingSettings.getAllLanguages())
            {
                this.from.Items.Add(codeNamePair.Second);
                this.to.Items.Add(codeNamePair.Second);

                if (fromIndex == 0)
                {
                    if(codeNamePair.First.Equals(preferredFrom))
                        fromIndex = i+1;
                }

                if (toIndex == -1)
                {
                    if(codeNamePair.First.Equals(preferredTo))
                        toIndex = i;
                }

                i++;
            }

            this.from.SelectedIndex = fromIndex;
            this.to.SelectedIndex = toIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("IExplore.exe", e.Link.LinkData as string); 
        }
    }
}

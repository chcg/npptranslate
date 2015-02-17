﻿using System;
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
        public TranslateSettingsController controller;
 
        public frmTranslateSettings()
        {
            InitializeComponent();  
        }

        public void setController(TranslateSettingsController controller)
        {
            this.controller = controller;
        }

        private void TranslateSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.onClose(this);
            e.Cancel = true;
            this.Hide();
        }

        private void TranslateSettings_Load(object sender, EventArgs e)
        {
            controller.onLoad(this);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        public void addFromLanguage(String lang)
        {
            this.from.Items.Add(lang);
            
        }

        public void addToLanguage(String lang)
        {
            if (lang.Equals("AUTO"))
                return;
            this.to.Items.Add(lang);
        }

        public void clearLanguages()
        {
            this.from.Items.Clear();
            this.to.Items.Clear();
        }


        public Pair getPreferredLanguages()
        {
            if ((from.SelectedIndex + to.SelectedIndex) >= 0)//both are properly selected
                return new Pair(from.SelectedItem.ToString(), to.SelectedItem.ToString());
            else
                return new Pair();

        }

        public void setPreferredLanguages(Pair fromTo)
        {
            int fromIndex = -1, toIndex = -1;

            int i = 0;

            foreach (String fromLanguage in this.from.Items)
            {
                if ((fromIndex < 0) && (fromLanguage.Equals(fromTo.First)))
                {
                    fromIndex = i;
                    break;
                }
                i++;
            }

            i = 0;

            foreach (String toLanguage in this.to.Items)
            {
                if ((toIndex < 0) && (toLanguage.Equals(fromTo.Second)))
                {
                    toIndex = i;
                    break;
                }
                i++;
            }

            this.from.SelectedIndex = fromIndex;
            this.to.SelectedIndex = toIndex;
        }
    }
}

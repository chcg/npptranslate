﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NppPluginNET;
using System.Web.UI;
using nppTranslateCS.Forms;
using System.Windows.Forms;

namespace nppTranslateCS
{
    public class TranslateSettingsController
    {
        TranslateSettingsModel model;
        frmTranslateSettings frmTrSettings;
        frmBingCredentials frmBingSettings;
        string dataSourcePath;


        public TranslateSettingsController(string dataSourcePath)
        {
            this.dataSourcePath = dataSourcePath;
        }

        public void setModel(TranslateSettingsModel model)
        {
            this.model = model;
        }

        public void setTranslateSettingsForm(frmTranslateSettings form)
        {
            this.frmTrSettings = form;
        }

        public void setBingSettingsForm(frmBingCredentials form)
        {
            this.frmBingSettings = form;
        }

        public void loadModel()
        {
            StringBuilder clientCred = new StringBuilder(255);
            Win32.GetPrivateProfileString("BING", "ClientIDAndSecret", "", clientCred, 255, dataSourcePath);
            
            if(clientCred.ToString().Contains(";"))
            {
                model.setClientCredentials(new Pair(clientCred.ToString().Split(';')[0], clientCred.ToString().Split(';')[1]));
            }


            StringBuilder allLangs = new StringBuilder(1023);
            Win32.GetPrivateProfileString("TRANSLATE", "ALLLANGUAGES", "", allLangs, 1023, dataSourcePath);
            
            if(allLangs.ToString().Contains(";"))
            {
                List<Pair> allLanguages = new List<Pair>();

                string[] allLangPair = allLangs.ToString().Split(';');
            
                foreach (string codeNamePair in allLangPair)
                {
                    string[] pair = codeNamePair.Split(':');
                    allLanguages.Add(new Pair(pair[0], pair[1]));
                }
                model.setAllLanguages(allLanguages);
            }

            StringBuilder langPref = new StringBuilder(255);
            Win32.GetPrivateProfileString("TRANSLATE", "LANGUAGEPREF", "", langPref, 255, dataSourcePath);

            if(langPref.ToString().Contains(":"))
            {
                string[] pref = langPref.ToString().Split(':');
                model.setLanguagePreference(new Pair(pref[0], pref[1]));
            }


        }

        public void persistModel()
        {
            Win32.WritePrivateProfileString("BING", "ClientIDAndSecret", model.getClientCredentials().First + ";" + model.getClientCredentials().Second, dataSourcePath);

            List<string> writableStringList = new List<string>();

            foreach (Pair codeNamePair in model.getAllLanguages())
            {
                writableStringList.Add(codeNamePair.First + ":" + codeNamePair.Second);
            }

            string writableString = string.Join(";", writableStringList.ToArray());

            Win32.WritePrivateProfileString("TRANSLATE", "ALLLANGUAGES", writableString, dataSourcePath);

            string langPrefStr = model.getLanguagePreference().First + ":" + model.getLanguagePreference().Second;

            Win32.WritePrivateProfileString("TRANSLATE", "LANGUAGEPREF", langPrefStr, dataSourcePath);
        }



        public void onClose(Form frm)
        {
            if (frm.GetType().Equals(typeof(frmBingCredentials)))
            {
                updateModel(this.frmBingSettings);
            }
            else if (frm.GetType().Equals(typeof(frmTranslateSettings)))
            {
                updateModel(this.frmTrSettings);
            }
        }


        private void updateModel(frmBingCredentials frm)
        {
            this.model.setClientCredentials(new Pair(frm.getBINGClientID(), frm.getBINGClientSecret()));

        }

        private void updateModel(frmTranslateSettings frm)
        {
            this.model.setLanguagePreference(
                new Pair(getLanguageCode((string)frm.getPreferredLanguages().First), 
                    getLanguageCode((string)frm.getPreferredLanguages().Second)));
        }

        private void populateBINGCredentials()
        {
            this.frmBingSettings.setBINGClientID((string)this.model.getClientCredentials().First);
            this.frmBingSettings.setBINGClientSecret((string)this.model.getClientCredentials().Second);
        }

        public void onLoad(Form frm)
        {
            if (frm.GetType().Equals(typeof(frmBingCredentials)))
            {
                updateView(this.frmBingSettings);
            }
            else if (frm.GetType().Equals(typeof(frmTranslateSettings)))
            {
                updateView(this.frmTrSettings);
            }
        }

        private void updateView(frmBingCredentials frm)
        {
            populateBINGCredentials();
        }

        private void updateView(frmTranslateSettings frm)
        {
            
            foreach(Pair lang in model.getAllLanguages())
            {
                frm.addFromLanguage((string)lang.Second);
                frm.addToLanguage((string)lang.Second);
                
            }
            Pair langDescriptionPair = new Pair(getLanguageDescription((string)model.getLanguagePreference().First), getLanguageDescription((string)model.getLanguagePreference().Second) );
            frm.setPreferredLanguages(langDescriptionPair);
        }

        private String getLanguageDescription(String langCode)
        {
            foreach(Pair lang in model.getAllLanguages())
            {
                if (lang.First.Equals(langCode))
                    return (string)lang.Second;
            }
            return null;
        }

        private String getLanguageCode(String langDescription)
        {
            foreach (Pair lang in model.getAllLanguages())
            {
                if (lang.Second.Equals(langDescription))
                    return (string)lang.First;
            }
            return null;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NppPluginNET;
using System.Web.UI;

namespace nppTranslateCS
{
    public class TranslateSettingsModel
    {
        Pair clientCredentials = new Pair();
        List<Pair> allLanguages = new List<Pair>();
        Pair languagePreference = new Pair();

        public Pair getClientCredentials()
        {
            return clientCredentials;
        }

        public void setClientCredentials(Pair clientCredentials)
        {
            this.clientCredentials = clientCredentials;
        }

        
        public List<Pair> getAllLanguages()
        {
            return allLanguages;
        }

        public void setAllLanguages(List<Pair> allLanguages)
        {
            this.allLanguages = allLanguages;
        }

        public Pair getLanguagePreference()
        {
            return languagePreference;
        }

        public void setLanguagePreference(Pair pref)
        {
            this.languagePreference = pref;
        }

    }
}

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
        public enum Engine {BING, MYMEMORY};

        Engine engine = Engine.MYMEMORY; //Default engine

        Pair clientCredentials = new Pair("","");
        List<Pair> allLanguages = new List<Pair>();
        Pair languagePreference = new Pair("","");
        public String email { get; set; }

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

        public Engine getEngine()
        {
            return engine;
        }

        public void setEngine(Engine eng)
        {
            engine = eng;
        }

    }
}

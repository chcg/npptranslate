using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Windows.Forms;
using nppTranslateCS.BingTranslate;
using System.Web.UI;

namespace nppTranslateCS
{
    class TrOD : ITranslateEngine
    {
        private TranslateSettingsModel settings;
        private ITranslateEngine engine;

        public TrOD(TranslateSettingsModel inParam)
        {
            settings = inParam;

        }

        public string Translate(string from, string to, string text)
        {
            updateEngineBasedOnPreference();

            if(Util.isStringEmpty(from) || Util.isStringEmpty(to))
            {
                throw new InvalidLanguagePreferenceException();
            }
            if(engine.ToString().Equals(TranslateSettingsModel.Engine.MYMEMORY.ToString()) && from.Equals("AUTO"))
            {
                throw new InvalidLanguagePreferenceException();
            }

            Main.writeLog("Fetching translation with translation params: ");
            Main.writeLog(" * from: " + from);
            Main.writeLog(" * to: " + to);
            Main.writeLog(" * text: " + text);

            
            String result = engine.Translate(from, to, text);
            Main.writeLog("Returning translation result: " + result);
            return result;
        }

        public List<Pair> GetSupportedLanguages()
        {
            Main.writeLog("Fetching languages...");

            updateEngineBasedOnPreference();
            return engine.GetSupportedLanguages();
        }

        private void updateEngineBasedOnPreference()
        {
            switch(settings.getEngine())
            {
                case TranslateSettingsModel.Engine.MYMEMORY:
                    engine = new MyMemoryTranslateEngine(settings);
                    break;
                case TranslateSettingsModel.Engine.BING:
                    engine = new BINGTranslateEngine(settings);
                    break;
            }

            Main.writeLog("Current Engine: " +engine.ToString());
        }

        public Pair getDefaultLanguagePreference()
        {
            return engine.getDefaultLanguagePreference();
        }
    }

}
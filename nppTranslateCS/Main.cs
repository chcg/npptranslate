using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NppPluginNET;
using System.Collections.Generic;
using System.Diagnostics;
using nppTranslateCS.Forms;
using System.Web.UI;
using Microsoft.VisualBasic.Logging;
using System.Reflection;
using System.Globalization;
using System.Threading;

namespace nppTranslateCS
{
    class Main
    {
        #region " Fields "
#if DEBUG
        internal const string PluginName = "TranslateCSDebug";
#else
        internal const string PluginName = "Translate";
#endif

        static String pluginVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        static string iniFilePath = null;
        static string logDirectoryPath = null;
        //static string logFilePath = null;
        static int idMyDlg = -1;
        static frmTranslateSettings dlgTrSettings = new frmTranslateSettings();
        static frmBingCredentials dlgBingSettings = new frmBingCredentials();
        static TranslateSettingsModel trSettingsModel = new TranslateSettingsModel();
        static TranslateSettingsController translateSettingsController;
        static TrOD translateEngine;


        static Main()
        {

            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            String iniDirectoryFilePath = Path.Combine(sbIniFilePath.ToString(), PluginName);
            if (!Directory.Exists(iniDirectoryFilePath)) Directory.CreateDirectory(iniDirectoryFilePath);
            iniFilePath = Path.Combine(iniDirectoryFilePath, "Translate" + ".ini");

            //Create directory for logging if not exisits
            
            String[] configPathArray = iniDirectoryFilePath.Split(new char[]{'\\'});
            configPathArray.SetValue("logs", configPathArray.Length - 2);//change ...config/translate => logs/translate
            logDirectoryPath = String.Join("\\", configPathArray);

            if (!Directory.Exists(logDirectoryPath)) Directory.CreateDirectory(logDirectoryPath);
            //logFilePath = Path.Combine(logDirectoryPath, "Translate" + ".log");

            

            //It is gaurunteed to have directory created after this
            try
            {
                initializeTraceListner();
                writeLog("################ Translate plugin (Version: " + pluginVersion + ") initializing...");

                if (!File.Exists(iniFilePath))
                {
                    FileStream fs = File.Create(iniFilePath);
                    fs.Dispose();
                }
                else
                {
                    //File exists, check for migration from older version
                    migrateIfRequired();
                }
                //Here, It is gaurunteed to have a settings file either crerated or preexisting or migrated(blank)
                
                translateSettingsController = new TranslateSettingsController(iniFilePath);
                translateSettingsController.setBingSettingsForm(dlgBingSettings);
                translateSettingsController.setTranslateSettingsForm(dlgTrSettings);
                

                translateSettingsController.setModel(trSettingsModel);
                dlgBingSettings.setController(translateSettingsController);
                dlgTrSettings.setController(translateSettingsController);

                translateSettingsController.loadModel();

                translateEngine = new TrOD(trSettingsModel);
                
                logSystemInfo();

            }
            catch (Exception ex)
            {
                handleException(ex);
            }

            
        }

        private static void logSystemInfo()
        {
            bool is64BitProcess = (IntPtr.Size == 8);
            bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

            writeLog("OSVersion: " + Environment.OSVersion.ToString());
            writeLog("Is64Bit: " + is64BitOperatingSystem.ToString());

            writeLog("Default Language Info:");
            logCultureInfo(CultureInfo.InstalledUICulture);
            writeLog("Current Culture Info:");
            logCultureInfo(Thread.CurrentThread.CurrentCulture);

            
        }

        private static void logCultureInfo(CultureInfo ci)
        {
            writeLog(String.Format(" * Name: {0}", ci.Name));
            writeLog(String.Format(" * Display Name: {0}", ci.DisplayName));
            writeLog(String.Format(" * English Name: {0}", ci.EnglishName));
            writeLog(String.Format(" * 2-letter ISO Name: {0}", ci.TwoLetterISOLanguageName));
            writeLog(String.Format(" * 3-letter ISO Name: {0}", ci.ThreeLetterISOLanguageName));
            writeLog(String.Format(" * 3-letter Win32 API Name: {0}", ci.ThreeLetterWindowsLanguageName));
        }

        private static void logEncodingInfo()
        {
            StringBuilder bufferEncoding = new StringBuilder();
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETBUFFERENCODING, Win32.MAX_PATH, bufferEncoding);

            StringBuilder currentNativeLangEncoding = new StringBuilder();
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETCURRENTNATIVELANGENCODING, Win32.MAX_PATH, currentNativeLangEncoding);

            writeLog("bufferEncoding: " + bufferEncoding.ToString());
            writeLog("currentNativeLangEncoding: " + currentNativeLangEncoding.ToString());

        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        internal static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }
        

        #endregion

        #region " StartUp/CleanUp "
        internal static void CommandMenuInit()
        {
            
            PluginBase.SetCommand(0, "Translate Selected", TranslateText, new ShortcutKey(true, true, false, Keys.Z));
            PluginBase.SetCommand(1, "Translate Selected-Swapped Preference", TranslateText_Reverse, new ShortcutKey(true, true, true, Keys.Z));
            PluginBase.SetCommand(2, "Translate CamelCase/underscore_case", TranslateCodeString, new ShortcutKey(true, true, false, Keys.X));
            PluginBase.SetCommand(3, "BING Credentials", setBINGCredentials); 
            PluginBase.SetCommand(4, "Settings", setLanguagePreference);
            PluginBase.SetCommand(5, "About", AboutDlg);
            PluginBase.SetCommand(6, "Help", LaunchHelp);
            idMyDlg = 6;
        }

        internal static void PluginCleanUp()
        {
            writeLog("################ Translate plugin cleaning up"); 
            translateSettingsController.persistModel();
        }

        #endregion

        
        internal static IntPtr GetCurrentEditHandle()
        {
            IntPtr curScintilla = PluginBase.GetCurrentScintilla();
            return curScintilla;
        }


        internal static void BEGINFUN(String txt)
        {
            writeLog("BEGIN -- " + txt);
        }

        internal static void ENDFUN(String txt)
        {
            writeLog("END -- " + txt);
        }


        internal static String getSelectedText()
        {
            BEGINFUN("getSelectedText");

            try
            {
                IntPtr editHandle = GetCurrentEditHandle();

                int cpMin = (int)Win32.SendMessage(editHandle, SciMsg.SCI_GETSELECTIONSTART, 0, 0);
                int cpMax = (int)Win32.SendMessage(editHandle, SciMsg.SCI_GETSELECTIONEND, 0, 0);

                Sci_TextRange tr = new Sci_TextRange(cpMin, cpMax, cpMax - cpMin + 1);
            
                Win32.SendMessage(editHandle, SciMsg.SCI_GETTEXTRANGE, 0, tr.NativePointer);

                string selected = tr.lpstrText;
                if (selected.Length < 1)
                {
                    return "";
                }

                writeLog("Selected text range: " + selected);

                logEncodingInfo();

                Encoding w1252 = Encoding.GetEncoding(1252);
                string converted = Encoding.UTF8.GetString(w1252.GetBytes(selected));

                writeLog("Final selected text after conversion: " + converted);

                ENDFUN("getSelectedText");

                return converted;

            }
            catch (Exception ex)
            {
                handleException(ex);
                return "";
            }

            
        }


        internal static void TranslateText()
        {
            BEGINFUN("TranslateText");

            try
            {
                string text = getSelectedText();

                if (text.Length == 0)
                    return;

                Pair langPref = getLanguagePreference();

                //readProxySettings();

                String result = translateEngine.Translate((string)langPref.First, (string)langPref.Second, text);

                showTranslationResults((string)langPref.First, (string)langPref.Second, result);


            }
            catch (Exception ex)
            {
                handleException(ex);
                return ;
            }

            ENDFUN("TranslateText");

        }

        internal static void setBINGCredentials()
        {
            try
            {
                    dlgBingSettings.ShowDialog();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
        }

        internal static void setLanguagePreference()
        {
            try
            {
                if (initLanguages())
                {
                    dlgTrSettings.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
    
        }

        internal static Pair getLanguagePreference()
        {
            if (initLanguages())
            {
               return trSettingsModel.getLanguagePreference();
            }        
            return null;   
        }


        internal static Boolean initLanguages()
        {
            try
            {
                if (trSettingsModel.getAllLanguages().Count == 0)
                {
                    List<Pair> fetchedList = new List<Pair>();
                    fetchedList.AddRange(translateEngine.GetSupportedLanguages());
                    fetchedList.Add(new Pair("AUTO","AUTO"));
                    trSettingsModel.setAllLanguages(fetchedList);
                    trSettingsModel.setLanguagePreference(new Pair("AUTO", "en"));
                }
                return true;
            }
            catch (Exception ex)
            {
                handleException(ex);
                return false;
            }
    
        }


        internal static void AboutDlg()
        {
            string aboutText = "Translate Plugin For Notepad++\n\nVersion: " + pluginVersion + "\nAuthor: Shaleen Mishra\nContact: shaleen.mishra@gmail.com";
            MessageBox.Show(System.Windows.Forms.Control.FromHandle(GetCurrentEditHandle()), aboutText, "Translate", MessageBoxButtons.OK);
        }


        internal static void LaunchHelp()
        {
            Process.Start("IExplore.exe", "https://sourceforge.net/apps/mediawiki/npptranslate/index.php?title=Main_Page");
        }



        internal static void TranslateText_Reverse() 
        {
            try
            {
                string text = getSelectedText();

                if (text.Length == 0)
                    return;

                Pair langPref = getLanguagePreference();

                if(langPref.First.Equals("AUTO"))
                {
                    MessageBox.Show(System.Windows.Forms.Control.FromHandle(GetCurrentEditHandle()), "This feature is not available for auto-detect settings!\nChange configuration file to a valid source language code and Retry.", "Translate Error!", MessageBoxButtons.OK);
                    return;
                }
                //readProxySettings();

                String result = translateEngine.Translate((String)langPref.Second, (string)langPref.First, text);

                showTranslationResults((string)langPref.Second, (string)langPref.First, result);
            }
            catch (Exception ex)
            {
                handleException(ex);
                return;
            }


        }

        internal static string DecoupleMixedCase(string inStr)
        {
            try
            {
                bool hasLower = false, hasUpper = false;
                int len = inStr.Length;
                StringBuilder outStr = new StringBuilder();

                for (int i = 0; i < len; i++)
                {
                    if (!Char.IsLower(inStr[i]))
                    {
                        hasUpper = true;
                        outStr.Append(" ");
                    }
                    else
                        hasLower = true;

                    outStr.Append(inStr[i]);
                }

                if (!(hasUpper && hasLower))
                    return inStr;
                else
                    return outStr.ToString();
            }
            catch (Exception ex)
            {
                handleException(ex);
                return inStr;
            }
        }

        internal static string replaceUndescores(string inStr)
        {
            return inStr.Replace('_',' ');
        }

        internal static void TranslateCodeString()
        {
            try
            {
                string selectedText = getSelectedText();

                if (selectedText.Equals(""))
                    return;

                string processedText = DecoupleMixedCase(replaceUndescores(selectedText));

                Pair fromTo = getLanguagePreference();

                //readProxySettings();

                string result = translateEngine.Translate((string)fromTo.First, (string)fromTo.Second, processedText);

                showTranslationResults((string)fromTo.First, (string)fromTo.Second, result);
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
        }




        internal static void CopyTranslatedTextDataToClipBoard(string strData)
        {
            Clipboard.SetText(strData);
        }

        internal static void showTranslationResults(string from, string to, string transResult)
        {
            BEGINFUN("showTranslationResults");

            try
            {
                StringBuilder transDisplay = new StringBuilder();
                transDisplay.Append(from);
                transDisplay.Append(" ==> ");
                transDisplay.Append(to);

                transDisplay.Append("\n\n**********************************************************\n\n");

                transDisplay.Append(transResult);

                transDisplay.Append("\n\n**********************************************************\n\n");

                transDisplay.Append("Do you want to copy translated text to clipboard?\n");

                DialogResult selection = MessageBox.Show(System.Windows.Forms.Control.FromHandle(GetCurrentEditHandle()), transDisplay.ToString(), "Translate", MessageBoxButtons.YesNo);
                if (selection.Equals(DialogResult.Yes))
                {
                    CopyTranslatedTextDataToClipBoard(transResult);

                }
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
            ENDFUN("showTranslationResults");
        }

        internal static void handleException(Exception e) 
        {
            if (e.GetType().Equals(typeof(BINGClientCredentialException)))
            {
                MessageBox.Show("Client ID and Client Secret must be provided in order to use Translate functionality");
                //MessageBox.Show(e.StackTrace);
                //settingsDialogue.ShowDialog();
            }
            else
            {
                MessageBox.Show("Unable to translate. Please check BING credentials and language preference.");
#if DEBUG
                MessageBox.Show(e.Message);
                MessageBox.Show(e.StackTrace);
#endif
            }
        }

        
        internal static void migrateIfRequired()
        {
            BEGINFUN("migrateIfRequired");

            //No direct way to get current Version in < 2.0.0.0, ge it indirectly;

            String strInstalledVersion = "n/a";
            StringBuilder installedVersion = new StringBuilder(255);
            Win32.GetPrivateProfileString("VERSION", "version", "", installedVersion, 255, iniFilePath);

            if(installedVersion.ToString().Length>0)
            {
                writeLog("Installed version (" + installedVersion.ToString() + ") is 2.1 or later, doing nothing");
                //Has version infor, i.e. is 2.1 or later
                strInstalledVersion = installedVersion.ToString();
            }

#if DEBUG
            //MessageBox.Show("Existing installed version: "+strInstalledVersion);
#endif
            if (strInstalledVersion.Equals("n/a"))
            {
                writeLog("No version info available, i.e. Legacy installation, creating a new config file..");
                System.IO.File.WriteAllText(iniFilePath, string.Empty);
                Win32.WritePrivateProfileString("VERSION", "version", pluginVersion, iniFilePath);

            }

            ENDFUN("migrateIfRequired");
        }

        internal static void initializeTraceListner()
        {
            FileLogTraceListener listner = new FileLogTraceListener();
            listner.BaseFileName = "Translate.log";
            listner.TraceOutputOptions = TraceOptions.DateTime;
            listner.DiskSpaceExhaustedBehavior = DiskSpaceExhaustedOption.ThrowException;
            listner.Location= LogFileLocation.Custom;
            listner.CustomLocation = logDirectoryPath;
           //.listner.MaxFileSize = 1024;
            listner.LogFileCreationSchedule = LogFileCreationScheduleOption.Daily;
            listner.AutoFlush = true;

            Trace.Listeners.Add(listner);
            
        }

        internal static void writeLog(String text)
        {
            Trace.TraceInformation(text);
        }

    }
}
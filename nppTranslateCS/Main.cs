﻿using System;
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

namespace nppTranslateCS
{
    class Main
    {
        #region " Fields "
        internal const string PluginName = "Translate";
        static string iniFilePath = null;
        static int idMyDlg = -1;
        static frmTranslateSettings settingsDialogue;
        static TranslateSettings settings;
        static TrOD translateEngine;


        static Main()
        {
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            String iniDirectoryFilePath = Path.Combine(sbIniFilePath.ToString(), PluginName);
            if (!Directory.Exists(iniDirectoryFilePath)) Directory.CreateDirectory(iniDirectoryFilePath);
            iniFilePath = Path.Combine(iniDirectoryFilePath, "Translate" + ".ini");

            try
            {

                if (!File.Exists(iniFilePath))
                {
                    FileStream fs = File.Create(iniFilePath);
                    fs.Dispose();
                }
                else
                {
                    cleanUpOldFile();
                }

                settings = new TranslateSettings(iniFilePath);

                settingsDialogue = new frmTranslateSettings(settings);

                translateEngine = new TrOD(settings);


            }
            catch (Exception ex)
            {
                handleException(ex);
            }
        }

        

        #endregion

        #region " StartUp/CleanUp "
        internal static void CommandMenuInit()
        {
            
            PluginBase.SetCommand(0, "Translate Selected", TranslateText, new ShortcutKey(true, true, false, Keys.Z));
            PluginBase.SetCommand(1, "Translate Selected-Swapped Preference", TranslateText_Reverse, new ShortcutKey(true, true, true, Keys.Z));
            PluginBase.SetCommand(2, "Translate CamelCase/underscore_case", TranslateCodeString, new ShortcutKey(true, true, false, Keys.X));
            PluginBase.SetCommand(3, "Settings", editConfiguration);
            PluginBase.SetCommand(4, "About", AboutDlg);
            PluginBase.SetCommand(5, "Help", LaunchHelp);
            idMyDlg = 5;
        }

        internal static void PluginCleanUp()
        {
            settings.persistSettings();
        }

        #endregion

        
        internal static IntPtr GetCurrentEditHandle()
        {
            IntPtr curScintilla = PluginBase.GetCurrentScintilla();
            return curScintilla;
        }


        internal static String getSelectedText()
        {
            try
            {
                IntPtr editHandle = GetCurrentEditHandle();

                int cpMin = (int)Win32.SendMessage(editHandle, SciMsg.SCI_GETSELECTIONSTART, 0, 0);
                int cpMax = (int)Win32.SendMessage(editHandle, SciMsg.SCI_GETSELECTIONEND, 0, 0);

                Sci_TextRange tr = new Sci_TextRange(cpMin, cpMax, cpMax - cpMin + 1);
            
                Win32.SendMessage(editHandle, SciMsg.SCI_GETTEXTRANGE, 0, tr.NativePointer);

                string selected = tr.lpstrText;

                Encoding w1252 = Encoding.GetEncoding(1252);
                string converted = Encoding.UTF8.GetString(w1252.GetBytes(selected));
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
            try
            {
                string text = getSelectedText();

                if (text.Length == 0)
                    return;

                KeyValuePair<string, string> langPref = readLanguageConfiguration();

                //readProxySettings();

                String result = translateEngine.Translate(langPref.Key, langPref.Value, text);

                showTranslationResults(langPref.Key, langPref.Value, result);

            }
            catch (Exception ex)
            {
                handleException(ex);
                return ;
            }

        }

        internal static void editConfiguration()
        {
            try
            {
                if (loadInitConfiguration())
                {
                    settingsDialogue.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
    
        }

        internal static KeyValuePair<string, string> readLanguageConfiguration()
        {
            try
            {
                if (loadInitConfiguration())
                {
                    string from = (string) settings.getLanguagePreference().First;
                    string to = (string)settings.getLanguagePreference().Second;

                    return new KeyValuePair<string, string>(from, to);
                }        
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
            return new KeyValuePair<string, string>();
    

        }


        internal static Boolean loadInitConfiguration()
        {
            try
            {
                if (settings.getAllLanguages().Count == 0)
                {
                    List<Pair> fetchedList = new List<Pair>();
                    fetchedList.AddRange(translateEngine.GetSupportedLanguages());
                    settings.setAllLanguages(fetchedList);

                    settings.setLanguagePreference(new Pair("", "en"));
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
            string aboutText = "Translate Plugin For Notepad++\n\nVersion: 2.0.0.0\nAuthor: Shaleen Mishra\nContact: shaleen.mishra@gmail.com";
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

                KeyValuePair<string, string> langPref = readLanguageConfiguration();

                if(langPref.Key.Equals(""))
                {
                    MessageBox.Show(System.Windows.Forms.Control.FromHandle(GetCurrentEditHandle()), "This feature is not available for auto-detect settings!\nChange configuration file to a valid source language code and Retry.", "Translate Error!", MessageBoxButtons.OK);
                    return;
                }
                //readProxySettings();

                String result = translateEngine.Translate(langPref.Value, langPref.Key, text);

                showTranslationResults(langPref.Value, langPref.Key, result);
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

                KeyValuePair<string, string> fromTo = readLanguageConfiguration();

                //readProxySettings();

                string result = translateEngine.Translate(fromTo.Key, fromTo.Value, processedText);

                showTranslationResults(fromTo.Key, fromTo.Value, result);
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
            try
            {
                StringBuilder transDisplay = new StringBuilder();
                transDisplay.Append(from.Equals("") ? "[Auto Detect]" : from);
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
        }

        internal static void handleException(Exception e) 
        {
            if (e.GetType().Equals(typeof(BINGClientCredentialException)))
            {
                MessageBox.Show("Client ID and Client Secret must be provided in order to use Translate functionality");
                //MessageBox.Show(e.StackTrace);
                settingsDialogue.ShowDialog();
            }
            else
            {
                MessageBox.Show(e.Message);
                //MessageBox.Show(e.StackTrace);
            }
        }

        internal static void cleanUpOldFile()
        {
                StringBuilder oldSourceTag = new StringBuilder(255);
                Win32.GetPrivateProfileString("SOURCE", "code", "", oldSourceTag, 255, iniFilePath);

                StringBuilder oldDestTag = new StringBuilder(255);
                Win32.GetPrivateProfileString("DESTINATION", "code", "", oldDestTag, 255, iniFilePath);

                if(oldSourceTag.Append(oldDestTag).ToString().Length > 1)
                {
                    System.IO.File.WriteAllText(iniFilePath, string.Empty);
                }

        }

    }
}
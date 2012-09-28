//this file is part of notepad++
//Copyright (C)2003 Don HO <donho@altern.org>
//
//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either
//version 2 of the License, or (at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

#include <tchar.h>
#include "TranslatePlugin.h"
#include "menuCmdID.h"
#include <cassert>
#include "iTrOD.h"

#include <Shlwapi.h>
#include <fstream>
#include <ctype.h>
#include <stdlib.h> 
#include "MSGS.h"
#include "UTILS.h"

using namespace std;

extern FuncItem funcItem[nbFunc];
extern NppData nppData;

HANDLE g_hModule;

map<wstring, wstring> languageList;

//TCHAR g_TranslateIniPath[MAX_PATH];
TCHAR g_configPath[MAX_PATH];

wstring g_proxyAddress;
int g_proxyPort;

BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  reasonForCall, 
                       LPVOID lpReserved )
{
	g_hModule = hModule;

    switch (reasonForCall)
    {
      case DLL_PROCESS_ATTACH:
        pluginInit(hModule);

        break;

      case DLL_PROCESS_DETACH:
		commandMenuCleanUp();
        pluginCleanUp();
        break;

      case DLL_THREAD_ATTACH:
        break;

      case DLL_THREAD_DETACH:
        break;
    }

    return TRUE;
}


extern "C" __declspec(dllexport) void setInfo(NppData notpadPlusData)
{
	nppData = notpadPlusData;
	commandMenuInit();
}

extern "C" __declspec(dllexport) const TCHAR * getName()
{
	return NPP_PLUGIN_NAME;
}

extern "C" __declspec(dllexport) FuncItem * getFuncsArray(int *nbF)
{
	*nbF = nbFunc;
	return funcItem;
}


extern "C" __declspec(dllexport) void beNotified(SCNotification *notifyCode)
{
}


//
extern "C" __declspec(dllexport) LRESULT messageProc(UINT Message, WPARAM wParam, LPARAM lParam)
{/*
	if (Message == WM_MOVE)
	{
		::MessageBox(NULL, "move", "", MB_OK);
	}
*/
	return TRUE;
}

#ifdef UNICODE
extern "C" __declspec(dllexport) BOOL isUnicode()
{
    return TRUE;
}
#endif //UNICODE


/***********************************Translate Plugin Specific Code******************************/


FuncItem funcItem[nbFunc];

NppData nppData;


void pluginInit(HANDLE hModule)
{
	
}

void pluginCleanUp()
{
}


void commandMenuInit()
{
	ShortcutKey* sk = new ShortcutKey;
    sk->_isAlt = true;
    sk->_isCtrl = true;
    sk->_isShift = false;
    sk->_key = 'Z';

	ShortcutKey* sk_x = new ShortcutKey;
    sk_x->_isAlt = true;
    sk_x->_isCtrl = true;
    sk_x->_isShift = true;
    sk_x->_key = 'Z';

	ShortcutKey* sk_code = new ShortcutKey;
    sk_code->_isAlt = true;
    sk_code->_isCtrl = true;
    sk_code->_isShift = false;
    sk_code->_key = 'X';

    setCommand(0, TEXT("Translate Selected"), TranslateText, sk, false);
	setCommand(1, TEXT("Translate Selected-Swapped Preference"), TranslateText_Reverse, sk_x, false);
	setCommand(2, TEXT("Translate CamelCase/underscore_case"), TranslateCodeString, sk_code, false);
    setCommand(3, TEXT("Change Language Preference"), editConfiguration, NULL, false);
	setCommand(4, TEXT("About"), AboutDlg, NULL, false);
	setCommand(5, TEXT("Help"), LaunchHelp, NULL, false);
}


void commandMenuCleanUp()
{
	delete funcItem[0]._pShKey;
	delete funcItem[1]._pShKey;
	delete funcItem[2]._pShKey;
}


bool setCommand(size_t index, TCHAR *cmdName, PFUNCPLUGINCMD pFunc, ShortcutKey *sk, bool check0nInit) 
{
    if (index >= nbFunc)
        return false;

    if (!pFunc)
        return false;

    lstrcpy(funcItem[index]._itemName, cmdName);
    funcItem[index]._pFunc = pFunc;
	funcItem[index]._pShKey = sk;
	funcItem[index]._init2Check = check0nInit;

    return true;
}


void getSelectedText(wstring& outText)
{
	HWND editHandle = GetCurrentEditHandle();

	assert(editHandle != NULL);

	if (editHandle != NULL)
	{
		struct TextRange tr;

		tr.chrg.cpMin = (long)SendMessage(editHandle, SCI_GETSELECTIONSTART, 0, 0);
		tr.chrg.cpMax = (long)SendMessage(editHandle, SCI_GETSELECTIONEND, 0, 0);

		if( tr.chrg.cpMax > 0 && (tr.chrg.cpMax > tr.chrg.cpMin))
		{
			string buf(tr.chrg.cpMax - tr.chrg.cpMin + 1, 0);
			
			tr.lpstrText = &buf[0];
			SendMessage(editHandle, SCI_GETTEXTRANGE, 0, (LPARAM)&tr);

			int sizeStr = MultiByteToWideChar(CP_UTF8, 0, tr.lpstrText, -1, NULL, 0);

			if(sizeStr>2000)
			{
				MessageBox(nppData._nppHandle, __TEXT("Please select a shorter text!"), __TEXT("Translate Error!"), 0);
				return;
			}

			wchar_t* wStringRaw = new wchar_t[sizeStr];

			if(!wStringRaw)
			{
				delete []wStringRaw;
			}

			MultiByteToWideChar(CP_UTF8, 0, tr.lpstrText, -1, wStringRaw, sizeStr);
			outText.assign(wStringRaw);

			delete []wStringRaw;
		}
		else
		{
			MessageBox(nppData._nppHandle, __TEXT("Please select some text first !!"), __TEXT("Translate Error!"), 0);
			return;
		}
	}

}


void TranslateText()
{
	wstring text;
    getSelectedText(text);

	if(text.empty())
		return;

	wstring outText;
	wstring error;

	wchar_t from[10];
	wchar_t to[10];

	readLanguageConfiguration(from, to);

	readProxySettings();

	TrOD::Translate(text,outText,from,to, g_proxyAddress, g_proxyPort, error);
	
	if(error.empty())
	{
		wstring tempStr = outText;
		tempStr.append(L"\nDo you want to copy translated text to clipboard?\n");

		if(::MessageBox(nppData._nppHandle, tempStr.c_str(), TEXT("Translate"), MB_YESNO) == IDYES)
		{
			CopyTranslatedTextDataToClipBoard(outText);

		}
		
	}
	else 
	{
	
		wstring errorMsg(MSGS::TRANSLATE_ERROR_MESSAGE);
		errorMsg.append(error);

		::MessageBox(nppData._nppHandle, errorMsg.c_str(), TEXT("nppTranslate Error!"), MB_OK);
	}

}


void editConfiguration()
{
	int retVal = 0;
	wstring translateConfigFilePath(L"");

	retVal = setupConfigurationFile(translateConfigFilePath);
	
	if(!retVal)
	{
		if (!::SendMessage(nppData._nppHandle, NPPM_SWITCHTOFILE, 0, (LPARAM)translateConfigFilePath.c_str()))
		{
			::SendMessage(nppData._nppHandle, NPPM_DOOPEN, 0, (LPARAM)translateConfigFilePath.c_str());
		}
	}

}

int readLanguageConfiguration(wchar_t* from, wchar_t* to)
{
	int retVal = 0;
	wstring translateConfigFilePath(L"");
	
	setupConfigurationFile(translateConfigFilePath);

	DWORD res_s = GetPrivateProfileString(TEXT("SOURCE"), TEXT("code"), L"", from, 10, translateConfigFilePath.c_str());
	DWORD res_d = GetPrivateProfileString(TEXT("DESTINATION"), TEXT("code"), L"en", to, 10, translateConfigFilePath.c_str());

	return retVal;

}

//Reads proxy configuration from plugin manager ini and sets global proxy config vars
//no Param
void readProxySettings()
{
	wchar_t sProxyAddress[32];
	wchar_t sProxyPort[8];
	TCHAR pluginManagerIniPath[MAX_PATH];

	::_tcscpy(pluginManagerIniPath,g_configPath);

	::_tcscat(pluginManagerIniPath,TEXT("\\PluginManager.ini"));

	DWORD res_s = GetPrivateProfileString(TEXT("Settings"), TEXT("Proxy"), L"", sProxyAddress, 32, pluginManagerIniPath);
	DWORD res_d = GetPrivateProfileString(TEXT("Settings"), TEXT("ProxyPort"), L"", sProxyPort, 8, pluginManagerIniPath);

	g_proxyAddress.clear();
	g_proxyAddress.append(sProxyAddress);

	g_proxyPort = _wtoi(sProxyPort);	

}

int setupConfigurationFile(wstring& sTranslateConfigFilePath)
{
		int retVal = 0;
		TCHAR path[MAX_PATH];

		::SendMessage(nppData._nppHandle, NPPM_GETPLUGINSCONFIGDIR, MAX_PATH, reinterpret_cast<LPARAM>(g_configPath));
		
		::_tcscpy(path,g_configPath);
		::_tcscat(path,TEXT("\\Translate"));

		//CreateIfNotExist
		if (PathFileExists(path) == FALSE) ::CreateDirectory(path, NULL);

		//buildFilePath
    	::_tcscat(path,TEXT("\\Translate.ini"));

		if (PathFileExists(path) == FALSE)
		{
		//Create File

			//Get Data
			wstring error;

			readProxySettings();

			TrOD::GetSupportedLanguages(languageList, g_proxyAddress, g_proxyPort, error);

			if(!error.empty())
			{
				retVal = -1;
				return retVal;
			}

			wofstream myfile (path);

			if (myfile.is_open())
			{
					myfile << ";The list of all available language codes is given at the bottom of this file.\n";
					myfile << ";Pick the codes of your choice and replace the Active SOURCE and DESTINATION tag with new codes.\n";
					myfile << ";For example, if you want to change the translation preference to English - French, change the file as follows:\n";
					myfile << "\n;[SOURCE]";
					myfile << "\n;code=en";
					myfile << "\n\n;[DESTINATION]";
					myfile << "\n;code=fr";
					myfile << "\n;NOTE: For Auto-Detection Of the source lanugage, leave SOURCE code blank (i.e. code=)";

					myfile << "\n\n\n;***************************Active Configuration****************************";
					myfile << "\n[SOURCE]";
					myfile << "\ncode=";
					myfile << "\n\n[DESTINATION]";
					myfile << "\ncode=en";
					myfile << "\n;***************************************************************************";

					myfile << "\n\n\n;**********************************LANGUAGE CODES*********************************\n";

					map<wstring,wstring>::iterator it;

					int i = 0;
					for(it=languageList.begin(); it != languageList.end(); it++)
					{
						if(i%5 !=0)
							myfile << it->second.c_str() << "-->"<< it->first.c_str()<<"\t\t\t";
						else
							myfile <<"\n\n;";
						i++;
					}
					myfile << "\n\n\n;*********************************************************************************\n";
					myfile.close();
			}
			else 
			{
				retVal = -1;
				return retVal;
			}		
		}

		sTranslateConfigFilePath.append(path);

		return retVal;
}



HWND GetCurrentEditHandle()
{
	int currentEdit = 0;

	SendMessage(nppData._nppHandle, NPPM_GETCURRENTSCINTILLA, 0, (LPARAM)&currentEdit);

	return (currentEdit == 0) ? nppData._scintillaMainHandle : nppData._scintillaSecondHandle;
}


void AboutDlg()
{

#ifdef MYDEBUGFROMREL
	wstring aboutText(L"DEBUG MODE");
#else
	wstring aboutText(L"Translate Plugin For Notepad++\n\nVersion: 0.4.0.0\nAuthor: Shaleen Mishra\nContact: shaleen.mishra@gmail.com");
#endif

	::MessageBox(nppData._nppHandle, aboutText.c_str(), TEXT("Translate"), MB_OK);
}


void LaunchHelp()
{
	ShellExecute(NULL, L"open", L"https://sourceforge.net/apps/mediawiki/npptranslate/index.php?title=Main_Page", NULL, NULL, SW_SHOWNORMAL);
}



void TranslateText_Reverse()
{
	wstring text;
    getSelectedText(text);

	if(text.empty())
		return;

	wstring outText;
	wstring error;

	wchar_t from[10];
	wchar_t to[10];

	readLanguageConfiguration(from, to);
	
	readProxySettings();

	if(from[0] == '\0')
	{
		::MessageBox(nppData._nppHandle, TEXT("This feature is not available for auto-detect settings!\nChange configuration file to a valid source language code and Retry."), TEXT("Translate Error!"), MB_OK);
		return;
	}

	//Send opposite language settings
	TrOD::Translate(text,outText,to,from,g_proxyAddress,g_proxyPort,error);
	
	if(error.empty())
	{
		wstring tempStr = outText;
		tempStr.append(L"\nDo you want to copy translated text to clipboard?\n");

		if(::MessageBox(nppData._nppHandle, tempStr.c_str(), TEXT("Translate"), MB_YESNO) == IDYES)
		{
			CopyTranslatedTextDataToClipBoard(outText);

		}

		//::MessageBox(nppData._nppHandle, outText.c_str(), TEXT("Translate"), MB_OK);
	}
	else 
	{
		wstring w_error (error.begin(), error.end());
		::MessageBox(nppData._nppHandle, w_error.c_str(), TEXT("Translate Error!"), MB_OK);
	}
}

void DecoupleMixedCase(const wstring& in, wstring& out)
{
	bool hasLower,hasUpper;
	int len = in.length();
	out.assign(L"");

	for(int i = 0; i<len; i++)
	{
		if(!islower(in[i]))
		{
			hasUpper= true;
			out.append(L" ");
		}
		else
			hasLower= true;

		out.append(1,in[i]);
	}

	if(!(hasUpper && hasLower))
		out.assign(in);

}

void replaceUndescores(const wstring& in, wstring& out)
{
	int len = in.length();
	out.assign(L"");

	for(int i = 0; i<len; i++)
	{
		if(in[i]=='_')
		{
			out.append(L" ");
		}
		else
			out.append(1,in[i]);
	}

}

void TranslateCodeString()
{
	wstring text, textModUnd, textModMix;

    getSelectedText(text);

	if(text.empty())
		return;
	
	replaceUndescores(text,textModUnd);
	DecoupleMixedCase(textModUnd,textModMix);

	wstring outText;
	wstring error;

	wchar_t from[10];
	wchar_t to[10];

	readLanguageConfiguration(from, to);
		
	readProxySettings();

	TrOD::Translate(textModMix,outText,from,to, g_proxyAddress,g_proxyPort, error);
	
	if(error.empty())
	{
		wstring tempStr = outText;
		tempStr.append(L"\nDo you want to copy translated text to clipboard?\n");

		if(::MessageBox(nppData._nppHandle, tempStr.c_str(), TEXT("Translate"), MB_YESNO) == IDYES)
		{
			CopyTranslatedTextDataToClipBoard(outText);

		}
		//::MessageBox(nppData._nppHandle, outText.c_str(), TEXT("Translate"), MB_OK);
	}
	else 
	{
		wstring w_error (error.begin(), error.end());
		::MessageBox(nppData._nppHandle, w_error.c_str(), TEXT("Translate Error!"), MB_OK);
	}

}




void CopyTranslatedTextDataToClipBoard(const std::wstring& wStrData)
{
	wstring wstrActualTranData = filterTranslatedTextFromOutput(wStrData);

	LPWSTR cwdBuffer = (LPWSTR)(wstrActualTranData.c_str());

    // Get the current working directory:
    //if( (cwdBuffer = _wgetcwd( NULL, 0 )) == NULL )
    //    return;// 1;

    DWORD len = wcslen(cwdBuffer);
    HGLOBAL hdst;
    LPWSTR dst;

    // Allocate string for cwd
    hdst = GlobalAlloc(GMEM_MOVEABLE | GMEM_DDESHARE, (len + 1) * sizeof(WCHAR));
    dst = (LPWSTR)GlobalLock(hdst);
    memcpy(dst, cwdBuffer, len * sizeof(WCHAR));
    dst[len] = 0;
    GlobalUnlock(hdst);

    // Set clipboard data
    if (!OpenClipboard(NULL)) return;// GetLastError();
    EmptyClipboard();
    if (!SetClipboardData(CF_UNICODETEXT, hdst)) return;// GetLastError();
    CloseClipboard();

}

wstring filterTranslatedTextFromOutput(const std::wstring& wStrData)
{
	wstring retVal;
	
	int posBegin = wStrData.find_first_of(L"\n\n") + 2;
	int posEnd = wStrData.find_last_of(L"\n\n") - 2;

	retVal  = wStrData.substr(posBegin, posEnd - posBegin + 1);

	return retVal;
}


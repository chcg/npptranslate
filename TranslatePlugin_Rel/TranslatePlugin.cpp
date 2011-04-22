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

extern FuncItem funcItem[nbFunc];
extern NppData nppData;

HANDLE g_hModule;

map<wstring, wstring> languageList;

TCHAR g_iniPath[MAX_PATH];


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


// Here you can process the Npp Messages 
// I will make the messages accessible little by little, according to the need of plugin development.
// Please let me know if you need to access to some messages :
// http://sourceforge.net/forum/forum.php?forum_id=482781
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

    setCommand(0, TEXT("Translate Selected"), TranslateText, sk, false);
    setCommand(1, TEXT("Change Language Preference"), editConfiguration, NULL, false);
	setCommand(2, TEXT("About"), AboutDlg, NULL, false);
}


void commandMenuCleanUp()
{
	delete funcItem[0]._pShKey;
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

			int sizeStr = MultiByteToWideChar(CP_ACP, 0, tr.lpstrText, -1, NULL, 0);

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

			MultiByteToWideChar(CP_ACP, 0, tr.lpstrText, -1, wStringRaw, sizeStr);
			outText.assign(wStringRaw);

			delete []wStringRaw;
		}
		else
		{
			MessageBox(nppData._nppHandle, __TEXT("Please select some text first!!"), __TEXT("Translate Error!"), 0);
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
	string error;

	wchar_t from[10];
	wchar_t to[10];

	getConfiguration(from, to);

	TrOD::Translate(text,outText,from,to, error);
	
	if(error.empty())
	{
		::MessageBox(NULL, outText.c_str(), TEXT("Translate"), MB_OK);
	}
	else 
	{
		wstring w_error (error.begin(), error.end());
		::MessageBox(NULL, w_error.c_str(), TEXT("Translate Error!"), MB_OK);
	}

}


void editConfiguration()
{
	int retVal = 0;

	retVal = setupConfigurationFile();
	
	if(!retVal)
	{
		if (!::SendMessage(nppData._nppHandle, NPPM_SWITCHTOFILE, 0, (LPARAM)g_iniPath))
		{
			::SendMessage(nppData._nppHandle, NPPM_DOOPEN, 0, (LPARAM)g_iniPath);
		}
	}

}

int getConfiguration(wchar_t* from, wchar_t* to)
{
	int retVal = 0;
	
	setupConfigurationFile();

	DWORD res_s = GetPrivateProfileString(TEXT("SOURCE"), TEXT("code"), L"", from, 10, g_iniPath);
	DWORD res_d = GetPrivateProfileString(TEXT("DESTINATION"), TEXT("code"), L"en", to, 10, g_iniPath);

	return retVal;

}

int setupConfigurationFile()
{
		int retVal = 0;
		//Get the path
		TCHAR path[MAX_PATH];
		::SendMessage(nppData._nppHandle, NPPM_GETPLUGINSCONFIGDIR, MAX_PATH, reinterpret_cast<LPARAM>(path));
		::_tcscat(path,TEXT("\\Translate"));

		//CreateIfNotExist
		if (PathFileExists(path) == FALSE) ::CreateDirectory(path, NULL);

		//getFilePath
		::_tcscpy(g_iniPath,path);
    	::_tcscat(g_iniPath,TEXT("\\Translate.ini"));

		if (PathFileExists(g_iniPath) == FALSE)
		{
		//Create File

			//Get Data
			string error;
			TrOD::GetSupportedLanguages(languageList, error);

			if(!error.empty())
			{
				retVal = -1;
				return retVal;
			}

			wofstream myfile (g_iniPath);

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
					myfile << "\ncode=es";
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

	wstring aboutText(L"Translate Plugin For Notepad++\n\nVersion: 0.0.0.1\nAuthor: Shaleen Mishra\nContact: shaleen.mishra@gmail.com");

	::MessageBox(NULL, aboutText.c_str(), TEXT("Translate"), MB_OK);
}
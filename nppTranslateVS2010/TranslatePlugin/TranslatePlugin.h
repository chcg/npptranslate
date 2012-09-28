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

#ifndef PLUGINDEFINITION_H
#define PLUGINDEFINITION_H

//
// All difinitions of plugin interface
//
#include "PluginInterface.h"
#include <iostream>



const TCHAR NPP_PLUGIN_NAME[] = TEXT("Translate");


const int nbFunc = 6;

void pluginInit(HANDLE hModule);

void pluginCleanUp();

void commandMenuInit();

void commandMenuCleanUp();

bool setCommand(size_t index, TCHAR *cmdName, PFUNCPLUGINCMD pFunc, ShortcutKey *sk = NULL, bool check0nInit = false);


//
// Plugin command functions
//

void TranslateText();
void TranslateText_Reverse();
HWND GetCurrentEditHandle();
void getSelectedText(std::wstring& outText);
void AboutDlg();
void LaunchHelp();
void DecoupleMixedCase(const std::wstring& in, std::wstring& out);
void replaceUndescores(const std::wstring& in, std::wstring& out);

void editConfiguration();
int setupConfigurationFile(std::wstring&);
int readLanguageConfiguration(wchar_t* from, wchar_t* to);
void TranslateCodeString();
void readProxySettings();

void CopyTranslatedTextDataToClipBoard(const std::wstring& strData);

std::wstring filterTranslatedTextFromOutput(const std::wstring& wStrData);

#endif //PLUGINDEFINITION_H

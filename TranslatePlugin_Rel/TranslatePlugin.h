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
using namespace std;


const TCHAR NPP_PLUGIN_NAME[] = TEXT("Translate");


const int nbFunc = 3;

void pluginInit(HANDLE hModule);

void pluginCleanUp();

void commandMenuInit();

void commandMenuCleanUp();

bool setCommand(size_t index, TCHAR *cmdName, PFUNCPLUGINCMD pFunc, ShortcutKey *sk = NULL, bool check0nInit = false);


//
// Plugin command functions
//

void TranslateText();
HWND GetCurrentEditHandle();
void getSelectedText(wstring& outText);
void AboutDlg();

void editConfiguration();
int setupConfigurationFile();
int getConfiguration(wchar_t* from, wchar_t* to);

#endif //PLUGINDEFINITION_H

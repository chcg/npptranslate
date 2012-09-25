#include "Utils.h"
#include "Windows.h"

using namespace std;

wstring UTILS::s2ws(const string& s)
{
    int len;
    int slength = (int)s.length() + 1;
    len = MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, 0, 0); 
    wchar_t* buf = new wchar_t[len];
    MultiByteToWideChar(CP_ACP, 0, s.c_str(), slength, buf, len);
    std::wstring r(buf);
    delete[] buf;
    return r;

}

string UTILS::ws2s(const wstring& ws)
{

	int len;
	int wslength = lstrlenW( ws.c_str()) + 1; // Convert all UNICODE characters
	len = WideCharToMultiByte( CP_ACP, // ANSI Code Page
	0,
	ws.c_str(), // wide-character string to be converted
	wslength,
	NULL, 0, // No output buffer since we are calculating length
	NULL, NULL ); // Unrepresented char replacement - Use Default 
	char* buf = new char[ len ]; // nUserNameLen includes the NULL character
	WideCharToMultiByte( CP_ACP, // ANSI Code Page
	0, // No special handling of unmapped chars
	ws.c_str(), // wide-character string to be converted
	wslength,
	buf, 
	len,
	NULL, NULL ); // Unrepresented char replacement - Use Default
    std::string r(buf);
    delete[] buf;
    return r;
}
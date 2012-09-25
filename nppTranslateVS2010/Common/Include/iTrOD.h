#include <map>

namespace TrOD
{
	void Translate (std::wstring& in, std::wstring& out, const std::wstring& sourceLanguage, const std::wstring& destinationLang, const std::wstring& proxyAddress, const int& proxyPort, std::wstring& error);
	void GetSupportedLanguages (std::map<std::wstring,std::wstring>& langList, const std::wstring& proxyAddress, const int& proxyPort, std::wstring& error) ;
}
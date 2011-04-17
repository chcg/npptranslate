#include <map>

namespace TrOD
{
	void Translate (std::wstring& in, std::wstring& out, const std::wstring& sourceLanguage, const std::wstring& destinationLang, std::string& error);
	void GetSupportedLanguages (std::map<std::wstring,std::wstring>& langList, std::string& error) ;
}
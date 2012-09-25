#include <map>
#include <vector>


class TranslateOnDemand
{
public:
	TranslateOnDemand();
	void getTranslation(std::wstring& in, std::wstring& out, std::wstring& error);
	void setLanguagePref(const std::wstring& from, const std::wstring& to, std::wstring& error);
	void getAvailableLanguageList(std::map<std::wstring,std::wstring>&, std::wstring& error);
	int validate(const std::wstring&, std::wstring&);
	void setProxyAddress(const std::wstring& value);
	void setProxyPort(const int& value){this->m_proxyPort = value;}



private:

	int validateLanguageCode(std::wstring& testString, std::wstring error);
	void getAvailableCodes(std::vector<std::wstring>& output, std::wstring& error);
	void autoDetect(std::wstring& text, std::wstring& detectResult, std::wstring& error);
	void getLanguageForCodes(std::vector<std::wstring>& , std::vector<std::wstring>& , std::wstring& error);
	void setProxyInformation(msTransAPI& transObj);
	std::string buildSoapErrorString(const int& errorCode, soap* pSoap);
	

	std::wstring m_SourceLang;
	std::wstring m_DestLanguage;
	std::wstring m_AppId;
	std::wstring m_DefaultLocale;
	std::string m_proxyAddress;
	int m_proxyPort;
};
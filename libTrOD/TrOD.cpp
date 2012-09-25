#include "soapmsTransAPIProxy.h"
#include "TrOD.h"
#include "ExceptionHandler.h"
#include "MSGS.h"
#include "Utils.h"
#include "appId.h"


TranslateOnDemand::TranslateOnDemand():m_AppId(APP_ID),m_DefaultLocale(L"en")
{
}

void TranslateOnDemand::setLanguagePref(const wstring& from, const wstring& to, wstring& error)
{
	m_SourceLang = from;
	m_DestLanguage = to;
}

void TranslateOnDemand::setProxyAddress(const wstring& value)
{
	this->m_proxyAddress = UTILS::ws2s(value);
}

int TranslateOnDemand::validate(const wstring& text, wstring& error)
{
	int retVal = 0;

	if(text.size() > 2000)
	{
		error.assign(L"Please select a shorter text.");
		retVal = -1;
	}

	return retVal;
}

void TranslateOnDemand::getTranslation(wstring& request, wstring& response, wstring& error)
{
	if(m_SourceLang.empty())
	{
		wstring detectResult;

		TRYCATCHRETHROW(autoDetect(request, detectResult, error);)

		if(error.empty() && !detectResult.empty())
		{
			m_SourceLang = detectResult;
		}
		else 
		{
			//error.assign(L"Could Not Detect Source Language.");
			//return;
			THROWWITHTRACE("Could Not Detect Source Language.");
		}
	}

	msTransAPI myTrans;
	setProxyInformation(myTrans);
	
	_ns3__Translate trRequest;
	_ns3__TranslateResponse trResponse;

	trRequest.appId	= &m_AppId;

	trRequest.from = &m_SourceLang;
	
	trRequest.to = &m_DestLanguage;

	trRequest.text = &request;

	int res = 0;
	
	TRYCATCHRETHROW(res = myTrans.__ns1__Translate(&trRequest, &trResponse);)
	
	if(res == SOAP_OK)
	{
		wstring retVal;
		
		retVal.append(L"(");
		retVal.append(m_SourceLang);
		retVal.append(L") --> ");
		retVal.append(L"(");
		retVal.append(m_DestLanguage);
		retVal.append(L")");
		retVal.append(L"\n\n");

		retVal.append(*trResponse.TranslateResult);

		response = retVal;
	}
	else
	{
		string errStr = buildSoapErrorString(res, trResponse.soap);
		THROWWITHTRACE(errStr) //To have begin trace in stack trace 
	}
	
}

void TranslateOnDemand::getAvailableCodes(vector<wstring>& output, wstring & error)
{
	msTransAPI myTrans;
	setProxyInformation(myTrans);

	_ns3__GetLanguagesForTranslate request;
	_ns3__GetLanguagesForTranslateResponse response;

	request.appId = &m_AppId;
	int res;

	TRYCATCHRETHROW(res = myTrans.__ns1__GetLanguagesForTranslate(&request, &response);)

	if(res == SOAP_OK)
	{
		ns5__ArrayOfstring* result = response.GetLanguagesForTranslateResult;
		
		int size = (result)->__sizestring;
				
		for(int i =0; i <size; i++)
		{
			output.push_back((result->string)[i]);
		}
	}
	else
	{
		string errStr = buildSoapErrorString(res, response.soap);
		THROWWITHTRACE(errStr) //To have begin trace in stack trace 
	}

}


void TranslateOnDemand::getLanguageForCodes(vector<wstring>& codes, vector<wstring>& languages, wstring& error)
{
	msTransAPI myTrans;
	setProxyInformation(myTrans);
	
	_ns3__GetLanguageNames request;
	_ns3__GetLanguageNamesResponse response;

	wstring* codesForInput = new wstring[codes.size()];

	for(int i=0; i<codes.size(); i++)
	{
		codesForInput[i] = codes[i];
	}

	ns5__ArrayOfstring arrayOfCodes;;
	
	arrayOfCodes.__sizestring = codes.size();
	arrayOfCodes.string = codesForInput;

	request.languageCodes = &arrayOfCodes;
	request.appId = &m_AppId;
	request.locale = &m_DefaultLocale;
	
	int res;

	TRYCATCHRETHROW(res = myTrans.__ns1__GetLanguageNames(&request, &response);)

	if (res == SOAP_OK)
	{

		ns5__ArrayOfstring* result = response.GetLanguageNamesResult;
		
		int size = (result)->__sizestring;
				
		for(int i =0; i <size; i++)
		{
			languages.push_back((result->string)[i]);
		}
		delete []codesForInput;
	}
	else
	{
		delete []codesForInput;

		string errStr = buildSoapErrorString(res, response.soap);
		THROWWITHTRACE(errStr) //To have begin trace in stack trace  
	}
}


void TranslateOnDemand::getAvailableLanguageList(map<wstring,wstring>& out, wstring& error)
{
	TranslateOnDemand myTrOD(*this);

	vector<wstring> oCodes;
	vector<wstring> oLangs;;

	TRYCATCHRETHROW(myTrOD.getAvailableCodes(oCodes,error);)

	TRYCATCHRETHROW(myTrOD.getLanguageForCodes(oCodes, oLangs,error);)

	if(oCodes.size() != oLangs.size())
	{
		//error.assign(L"Mismatch between number of codes and number of languages.");
		//return;
		THROWWITHTRACE("Mismatch between number of codes and number of languages.") //To have begin trace in stack trace  
	}
	
	int size = oCodes.size();
				
	for(int i =0; i <size; i++)
	{
		out.insert(make_pair( oCodes[i], oLangs[i] ));
	}
}


void TranslateOnDemand::autoDetect(wstring& text, wstring& detectResult, wstring& error)
{

	msTransAPI myTrans;
	setProxyInformation(myTrans);
	
	_ns3__Detect request;
	_ns3__DetectResponse response;

	request.appId = &m_AppId;
	request.text = &text;

	int res;
	TRYCATCHRETHROW( res = myTrans.__ns1__Detect(&request, &response);)

	if(res == SOAP_OK)
	{
		if(response.DetectResult != NULL)
		{
			int validateRet;

			TRYCATCHRETHROW(validateRet = validateLanguageCode(*(response.DetectResult), error);)

			if(!validateRet)
			{
				detectResult = *(response.DetectResult);
			}
			else
			{
				THROWWITHTRACE("Could not autodetect source language!") //To have begin trace in stack trace  
			}
		}
		else
		{
			THROWWITHTRACE("Could not autodetect source language!") //To have begin trace in stack trace  
		}
	}
	else 
	{
		string errStr = buildSoapErrorString(res, response.soap);
		THROWWITHTRACE(errStr) //To have begin trace in stack trace  
	}

}


int TranslateOnDemand::validateLanguageCode(wstring& testString, wstring error)
{
	int retVal = 0;
	map<wstring,wstring> langList;

	TRYCATCHRETHROW(getAvailableLanguageList(langList, error);)

	if(langList.find(testString) == langList.end())
	{
		retVal = -1;
	}

	return retVal;
}

void TranslateOnDemand::setProxyInformation(msTransAPI& transObj)
{
	if( (!this->m_proxyAddress.empty()) && (this->m_proxyPort > 0) )
	{
		/*char mbsProxyAddress[128];
		memset(mbsProxyAddress, '\0',128*sizeof(char));
		wcstombs(mbsProxyAddress,this->m_proxyAddress.c_str(), sizeof(char)*128);
		*/
		transObj.soap->proxy_host =  (this->m_proxyAddress).c_str();//mbsProxyAddress;
		transObj.soap->proxy_port = this->m_proxyPort;
	}
}

string TranslateOnDemand::buildSoapErrorString(const int& errorCode, soap* pSoap)
{
	char bufSoapError[64];
	string strSoapError("");
	sprintf(bufSoapError,(UTILS::ws2s(MSGS::SOAP_ERROR)).c_str(), errorCode);
	strSoapError.append(bufSoapError);

	if(pSoap)
	{
		if( (pSoap)->fault )
		{
			strSoapError.append( (pSoap)->fault->faultstring);
		}
	}

	return strSoapError;
}







#include "iTrOD.h"

namespace TrOD
{

	void Translate(wstring& in, wstring& out, const wstring& from, const std::wstring& to, const std::wstring& proxyAddress, const int& proxyPort, wstring& error)
	{
		wchar_t params[256];
		memset(params,'\0',sizeof(wchar_t)*256);
		wsprintf(params,MSGS::TRANSLATION_PARAMS,from.c_str(), to.c_str(), proxyAddress.c_str(), proxyPort);

		TranslateOnDemand myTrOD;

		if(!myTrOD.validate(in, error))
		{
			//try without proxy settings first
			myTrOD.setLanguagePref(from, to, error);
			TRYCATCH( myTrOD.getTranslation(in, out, error); )

			if(exceptionhandler::instance()->anyErrors())
			{
				//try again with proxy
				exceptionhandler::instance()->clearTrace();
				myTrOD.setProxyAddress(proxyAddress);
				myTrOD.setProxyPort(proxyPort);

				TRYCATCH( myTrOD.getTranslation(in, out, error); )
			}
		}
		
		//still errors

		if(exceptionhandler::instance()->anyErrors())
		{
			error.append(params);
			error.append( UTILS::s2ws((exceptionhandler::instance())->getStackTrace()) ) ;
		}
		
	}


	void GetSupportedLanguages (std::map<wstring,wstring>& langList, const std::wstring& proxyAddress, const int& proxyPort, wstring& error)
	{
		wchar_t params[256];
		memset(params,'\0',sizeof(wchar_t)*256);
		wsprintf(params,MSGS::LAGUAGE_LOOKUP_PARAMS, proxyAddress, proxyPort);

		TranslateOnDemand myTrOD;
		//try without proxy first
	
		TRYCATCH(myTrOD.getAvailableLanguageList(langList, error);)

		if(exceptionhandler::instance()->anyErrors())
		{
			//try again with proxy info
			exceptionhandler::instance()->clearTrace();
			myTrOD.setProxyAddress(proxyAddress);
			myTrOD.setProxyPort(proxyPort);
			TRYCATCH(myTrOD.getAvailableLanguageList(langList, error);)
		}

		//still errors

		if(exceptionhandler::instance()->anyErrors())
		{
			error.append(params);
			error.append( UTILS::s2ws((exceptionhandler::instance())->getStackTrace()) ) ;
		}

	}

}


/* soapmsTransAPIProxy.h
   Generated by gSOAP 2.8.0 from wsName.h
   Copyright(C) 2000-2010, Robert van Engelen, Genivia Inc. All Rights Reserved.
   The generated code is released under one of the following licenses:
   GPL, the gSOAP public license, or Genivia's license for commercial use.
*/

#ifndef soapmsTransAPIProxy_H
#define soapmsTransAPIProxy_H
#include "soapH.h"
class msTransAPI
{   public:
	/// Runtime engine context allocated in constructor
	struct soap *soap;
	/// Endpoint URL of service 'msTransAPI' (change as needed)
	const char *endpoint;
	/// Constructor allocates soap engine context, sets default endpoint URL, and sets namespace mapping table
	msTransAPI()
	{ soap = soap_new(); endpoint = "http://api.microsofttranslator.com/V2/soap.svc"; if (soap && !soap->namespaces) { static const struct Namespace namespaces[] = 
{
	{"SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/", "http://www.w3.org/*/soap-envelope", NULL},
	{"SOAP-ENC", "http://schemas.xmlsoap.org/soap/encoding/", "http://www.w3.org/*/soap-encoding", NULL},
	{"xsi", "http://www.w3.org/2001/XMLSchema-instance", "http://www.w3.org/*/XMLSchema-instance", NULL},
	{"xsd", "http://www.w3.org/2001/XMLSchema", "http://www.w3.org/*/XMLSchema", NULL},
	{"ns5", "http://schemas.microsoft.com/2003/10/Serialization/Arrays", NULL, NULL},
	{"ns6", "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2", NULL, NULL},
	{"ns4", "http://schemas.microsoft.com/2003/10/Serialization/", NULL, NULL},
	{"ns1", "http://tempuri.org/", NULL, NULL},
	{"ns3", "http://api.microsofttranslator.com/V2", NULL, NULL},
	{NULL, NULL, NULL, NULL}
};
	soap->namespaces = namespaces; } };
	/// Destructor frees deserialized data and soap engine context
	virtual ~msTransAPI() { if (soap) { soap_destroy(soap); soap_end(soap); soap_free(soap); } };
	/// Invoke 'AddTranslation' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__AddTranslation(_ns3__AddTranslation *ns3__AddTranslation, _ns3__AddTranslationResponse *ns3__AddTranslationResponse) { return soap ? soap_call___ns1__AddTranslation(soap, endpoint, NULL, ns3__AddTranslation, ns3__AddTranslationResponse) : SOAP_EOM; };
	/// Invoke 'BreakSentences' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__BreakSentences(_ns3__BreakSentences *ns3__BreakSentences, _ns3__BreakSentencesResponse *ns3__BreakSentencesResponse) { return soap ? soap_call___ns1__BreakSentences(soap, endpoint, NULL, ns3__BreakSentences, ns3__BreakSentencesResponse) : SOAP_EOM; };
	/// Invoke 'Detect' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__Detect(_ns3__Detect *ns3__Detect, _ns3__DetectResponse *ns3__DetectResponse) { return soap ? soap_call___ns1__Detect(soap, endpoint, NULL, ns3__Detect, ns3__DetectResponse) : SOAP_EOM; };
	/// Invoke 'DetectArray' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__DetectArray(_ns3__DetectArray *ns3__DetectArray, _ns3__DetectArrayResponse *ns3__DetectArrayResponse) { return soap ? soap_call___ns1__DetectArray(soap, endpoint, NULL, ns3__DetectArray, ns3__DetectArrayResponse) : SOAP_EOM; };
	/// Invoke 'GetAppIdToken' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__GetAppIdToken(_ns3__GetAppIdToken *ns3__GetAppIdToken, _ns3__GetAppIdTokenResponse *ns3__GetAppIdTokenResponse) { return soap ? soap_call___ns1__GetAppIdToken(soap, endpoint, NULL, ns3__GetAppIdToken, ns3__GetAppIdTokenResponse) : SOAP_EOM; };
	/// Invoke 'GetLanguageNames' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__GetLanguageNames(_ns3__GetLanguageNames *ns3__GetLanguageNames, _ns3__GetLanguageNamesResponse *ns3__GetLanguageNamesResponse) { return soap ? soap_call___ns1__GetLanguageNames(soap, endpoint, NULL, ns3__GetLanguageNames, ns3__GetLanguageNamesResponse) : SOAP_EOM; };
	/// Invoke 'GetLanguagesForSpeak' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__GetLanguagesForSpeak(_ns3__GetLanguagesForSpeak *ns3__GetLanguagesForSpeak, _ns3__GetLanguagesForSpeakResponse *ns3__GetLanguagesForSpeakResponse) { return soap ? soap_call___ns1__GetLanguagesForSpeak(soap, endpoint, NULL, ns3__GetLanguagesForSpeak, ns3__GetLanguagesForSpeakResponse) : SOAP_EOM; };
	/// Invoke 'GetLanguagesForTranslate' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__GetLanguagesForTranslate(_ns3__GetLanguagesForTranslate *ns3__GetLanguagesForTranslate, _ns3__GetLanguagesForTranslateResponse *ns3__GetLanguagesForTranslateResponse) { return soap ? soap_call___ns1__GetLanguagesForTranslate(soap, endpoint, NULL, ns3__GetLanguagesForTranslate, ns3__GetLanguagesForTranslateResponse) : SOAP_EOM; };
	/// Invoke 'GetTranslations' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__GetTranslations(_ns3__GetTranslations *ns3__GetTranslations, _ns3__GetTranslationsResponse *ns3__GetTranslationsResponse) { return soap ? soap_call___ns1__GetTranslations(soap, endpoint, NULL, ns3__GetTranslations, ns3__GetTranslationsResponse) : SOAP_EOM; };
	/// Invoke 'Translate' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__Translate(_ns3__Translate *ns3__Translate, _ns3__TranslateResponse *ns3__TranslateResponse) { return soap ? soap_call___ns1__Translate(soap, endpoint, NULL, ns3__Translate, ns3__TranslateResponse) : SOAP_EOM; };
	/// Invoke 'AddTranslationArray' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__AddTranslationArray(_ns3__AddTranslationArray *ns3__AddTranslationArray, _ns3__AddTranslationArrayResponse *ns3__AddTranslationArrayResponse) { return soap ? soap_call___ns1__AddTranslationArray(soap, endpoint, NULL, ns3__AddTranslationArray, ns3__AddTranslationArrayResponse) : SOAP_EOM; };
	/// Invoke 'GetTranslationsArray' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__GetTranslationsArray(_ns3__GetTranslationsArray *ns3__GetTranslationsArray, _ns3__GetTranslationsArrayResponse *ns3__GetTranslationsArrayResponse) { return soap ? soap_call___ns1__GetTranslationsArray(soap, endpoint, NULL, ns3__GetTranslationsArray, ns3__GetTranslationsArrayResponse) : SOAP_EOM; };
	/// Invoke 'Speak' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__Speak(_ns3__Speak *ns3__Speak, _ns3__SpeakResponse *ns3__SpeakResponse) { return soap ? soap_call___ns1__Speak(soap, endpoint, NULL, ns3__Speak, ns3__SpeakResponse) : SOAP_EOM; };
	/// Invoke 'TranslateArray' of service 'msTransAPI' and return error code (or SOAP_OK)
	virtual int __ns1__TranslateArray(_ns3__TranslateArray *ns3__TranslateArray, _ns3__TranslateArrayResponse *ns3__TranslateArrayResponse) { return soap ? soap_call___ns1__TranslateArray(soap, endpoint, NULL, ns3__TranslateArray, ns3__TranslateArrayResponse) : SOAP_EOM; };
};
#endif

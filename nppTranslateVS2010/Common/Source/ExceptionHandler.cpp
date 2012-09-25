#include "ExceptionHandler.h"
#include "MSGS.h"
#include "Utils.h"

traceInfo::traceInfo(string file, string function, int line, string errorText)
{
	m_file = file;
	m_function = function;
	m_line = line;
	m_errorText = errorText;
}

string traceInfo::getTraceAsString()
{
	char retVal[512];
	memset(retVal,'\0',sizeof(char)*512);
	//sprintf(retVal,"Line[%d], Function[%s], File[%s], Error[%s]\n", m_line, m_function.c_str(), m_file.c_str(), m_errorText.c_str() );
	sprintf(retVal,"Line[%d], Function[%s], Error[%s]\n", m_line, m_function.c_str(), m_errorText.c_str() );
	return string(retVal);
}


exceptionhandler* exceptionhandler::single = NULL;

exceptionhandler::exceptionhandler()
{

}

exceptionhandler* exceptionhandler::instance()
{ 
	if(!single)
	{
		single = new exceptionhandler();
	}
	return single;
}


string exceptionhandler::getStackTrace()
{
	string retVal(UTILS::ws2s(MSGS::STACK_TRACE));

	while(!m_stackTrace.empty())
	{
		traceInfo myTrace = m_stackTrace.back();
		m_stackTrace.pop_back();
		
		retVal = retVal + myTrace.getTraceAsString() +"\n";
	}

	return retVal;
}

void exceptionhandler::clearTrace()
{
	m_stackTrace.clear();
}

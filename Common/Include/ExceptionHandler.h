#ifndef EXCEPTIONHANDLER_H
#define EXCEPTIONHANDLER_H

#include <string>
#include <vector>
using namespace std;


class traceInfo
{
public:
    traceInfo(string, string, int, string);
	string getTraceAsString();
private:
	string m_file;
	string m_function;
	int m_line;
	string m_errorText;
};


class exceptionhandler
{
private:
	exceptionhandler();
	static exceptionhandler* single;

public:
	static exceptionhandler* instance();
	void addTrace(traceInfo trace){ m_stackTrace.push_back(trace); }
	string getStackTrace();
	bool anyErrors(){ return !(m_stackTrace.empty()) ;}
	void clearTrace();

private:
	vector<traceInfo> m_stackTrace;
};

#define TRYCATCHRETHROW(instruction) try \
	{ \
		instruction \
	} \
	catch(exception& e) \
	{ \
		(exceptionhandler::instance())->addTrace( traceInfo( __FILE__, __FUNCTION__, __LINE__, e.what() ) ); \
		throw e; \
	} \
	catch(string& s) \
	{ \
		(exceptionhandler::instance())->addTrace( traceInfo( __FILE__, __FUNCTION__, __LINE__, s ) ); \
		throw s; \
	} \
	catch(...) \
	{ \
		(exceptionhandler::instance())->addTrace( traceInfo( __FILE__, __FUNCTION__, __LINE__, "Unknown Error" ) ); \
		throw; \
	} \

#define TRYCATCH(instruction) try \
	{ \
		instruction \
	} \
	catch(exception& e) \
	{ \
		(exceptionhandler::instance())->addTrace( traceInfo( __FILE__, __FUNCTION__, __LINE__, e.what() ) ); \
	} \
	catch(string& s) \
	{ \
		(exceptionhandler::instance())->addTrace( traceInfo( __FILE__, __FUNCTION__, __LINE__, s ) ); \
	} \
	catch(...) \
	{ \
		(exceptionhandler::instance())->addTrace( traceInfo( __FILE__, __FUNCTION__, __LINE__, "Unknown Error" ) ); \
	} \

#define THROWWITHTRACE(toThrow) \
	TRYCATCHRETHROW(throw toThrow;)

#endif
#ifndef UTILS_H
#define UTILS_H

#include <xstring>
#include <string>

class UTILS
{
public:
	static std::wstring s2ws(const std::string& s);
	static std::string ws2s(const std::wstring& s);

};

#endif
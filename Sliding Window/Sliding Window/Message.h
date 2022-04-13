#pragma once
#include <string>
class Message
{
public:
	int x = 0;
	bool syn = 0;
	bool ack = 0;
	int f = 0;
	std::string text="";
	bool final = 0;
	Message();
};


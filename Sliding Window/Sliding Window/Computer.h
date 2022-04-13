#pragma once
#include<string>
#include <thread>
class Computer
{
public:
	std::string message;
	std::thread thread_comp;
    Computer(){
		message = "";
	}
};


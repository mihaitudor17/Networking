#include <iostream>
#include"Message.h"
#include<random>
#include <mutex>
#include<chrono>
#include"Computer.h"
std::mutex mtx;
Message message;
int len = -1;
int Random() {
    std::random_device rd;
    std::mt19937 gen(rd());
    int limit = len / 2 + 1;
    std::uniform_int_distribution<int> distribution(0, limit);
    return distribution(gen);
}
void send(Computer& computer)
{
    if (message.syn == 0)
    {
        std::cout << "Source " << "x=" << message.x << " " << "syn=" << message.syn << " " << "ack=" << message.ack << " " << "f=" << message.f << " " << computer.message << std::endl;
        mtx.lock();
        message.syn = 1;
        mtx.unlock();
    }
    while (message.x <= computer.message.length())
    {
        if (message.ack == 0)
        {
            if (message.f == 0)
            {
                mtx.lock();
                message.ack = 1;
                mtx.unlock();
            }
            else 
            {
                mtx.lock();
                message.text = computer.message.substr(message.x - 1, message.f);
                message.x += message.f - 1;
                std::cout << "Source " << "x=" << message.x << " " << "syn=" << message.syn << " " << "ack=" << message.ack << " " << "f=" << message.f << " " << message.text << std::endl;
                message.ack = 1;
                mtx.unlock();
            }
        }
        else
        {
            std::this_thread::sleep_for(std::chrono::milliseconds(100));
        }
    }
    mtx.lock();
    message.final = 1;
    mtx.unlock();
}
void recv(Computer& computer)
{
    while(message.final!=1)
    if (message.ack == 1)
    {
        mtx.lock();
        message.f = Random();
        if(message.f!=0)
        message.x += 1;
        computer.message += message.text;
        message.text = "";
        std::cout << "Destin " << "x=" << message.x << " " << "syn=" << message.syn << " " << "ack=" << message.ack << " " << "f=" << message.f << " " << computer.message << std::endl;
        message.ack = 0;
        mtx.unlock();
    }
    else
    {
        std::this_thread::sleep_for(std::chrono::milliseconds(100));
    }
    mtx.lock();
    if (message.text != "")
        computer.message += message.text;
    message.syn = 0;
    mtx.unlock();
    std::cout << "Destin " << "x=" << message.x << " " << "syn=" << message.syn << " " << "ack=" << message.ack << " " << "f=" << message.f << " " <<"fin=" << message.final << " " << computer.message << std::endl;
}
int main()
{
    std::string text;
    std::cout << "Introduceti mesaj ";
    std::cin >> text;
    len = text.length();
    Message message;
    Computer source,dest;
    source.message = text;
    source.thread_comp = std::thread(send, std::ref(source));
    dest.thread_comp = std::thread(recv, std::ref(dest));
    source.thread_comp.join();
    dest.thread_comp.join();
    return 0;
}
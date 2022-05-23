using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Diffie_Hellman_Key_Exchange
{
    class Server
    {
        private TcpListener tcpServer;
        private int y, a, b, key;
        public int n, g;
        public int Y { get => y; set => y = value; }
        public int B { get => b; set => b = value; }
        public int A { get => a;  set { a = value; key = (int)((long)Math.Pow(a, y) % n); } }
        public int Key { get => key; set => key = value; }
        public TcpListener TcpServer { get => tcpServer; set => tcpServer = value; }
        public static int Random(int max, int min = 0)
        {
            Random rd = new Random();
            return rd.Next(min, max);
        }
        public Server(int n, int g, TcpListener tcpServer)
        {
            this.n = n;
            this.g = g;
            this.tcpServer = tcpServer;
            y = Random(10);
            b = (int)((long)Math.Pow(g, y) % n);
        }

    }
}

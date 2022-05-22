using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Diffie_Hellman_Key_Exchange
{
    class Client
    {
        private int x, a, b, key;
        public int n, g;
        public int X { get => x; set => x = value; }
        public int A { get => a; set => a = value; }
        public int B { get => b; set { b = value; key =(int)Math.Pow(b,x) % n; } }
        public int Key { get => key; set => key = value; }
        public static int Random(int max, int min = 0)
        {
            Random rd = new Random();
            return rd.Next(min, max);
        }
        public Client(int n, int g) 
        {
            this.n = n;
            this.g = g;
            x = Random(10); 
            a = (int)Math.Pow(g, x) % n;
        }

    }
}

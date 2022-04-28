using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    
    class TCP_Client
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        static void Connect(String server, String message)
        {

            try
            {
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();
                string key;
                if (message.Length<6)
                    key = RandomString(message.Length);
                else
                    key = RandomString(6);
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(key);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Key: {0}", key);

                Thread.Sleep(100);

                message=Crypt(message,key);
                data = System.Text.Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);

                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
        public static string Crypt(string message,string key)
        {
            char[] temp=key.ToCharArray();
            Array.Sort(temp);
            string keyCopy = new string(temp);
            List<int> pos=new List<int>();
            Dictionary<char, int> hash = new Dictionary<char, int>();
            int find=-1;
            foreach (char c in key)
            {
                if (hash.ContainsKey(c))
                {
                    hash[c] += 1;
                    find = keyCopy.IndexOf(c);
                    for (int j = 0; j < hash[c]; j++)
                        find = keyCopy.IndexOf(c, find + 1);
                }
                else
                {
                    hash.Add(c, 0);
                    find = keyCopy.IndexOf(c);
                }

                pos.Add(find);
            }
            List<List<char>> list = new List<List<char>>();
            List<char> temp1 = new List<char>();
            int i = 0;
            foreach(char c in message)
            {
                temp1.Add(c);
                if ((i+1)%key.Length==0)
                {  
                    list.Add(temp1);
                    temp1=new List<char>();
                    temp1.Clear();
                }
                i++;
            }
            if(temp1.Count>0)
                list.Add(temp1);
            while (list[list.Count - 1].Count < key.Length)
            {
                list[list.Count - 1].Add('\0');
            }
            message = "";
            for(int k=0;k<key.Length;k++)
            {
                int index = pos.FindIndex(a => a == k);
                for (int j = 0; j < list.Count; j++)
                    message += list[j][index].ToString();
            }
            System.Console.Write("Crypted message: ");
            System.Console.WriteLine( message);
            return (message);
        }
        public static void Main()
        {
            string localAddr = "127.0.0.1";
            bool ok = true;
            while(ok)
            {
                System.Console.Write("Please input the message: ");
                string message = System.Console.ReadLine();
                System.Console.WriteLine("Message: "+message);
                Connect(localAddr, message);
                if (message == "Close connection")
                    ok = false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class TCP_Server
    {
        public static void Connect()
        {
            string key=null;
            string message=null;
            TcpListener server = null;
            bool ok = true;
            try
            {

                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");


                server = new TcpListener(localAddr, port);

                server.Start();

                Byte[] bytes = new Byte[256];
                String data = null;

                while (ok)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    NetworkStream stream = client.GetStream();

                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        if (key != null)
                        {

                            message = data.ToString();
                            System.Console.WriteLine("Key: " + key);
                            System.Console.WriteLine("Crypted message: " + message);
                            message = Decrypt(key, message);
                            System.Console.WriteLine("Decrypted message: " + message);
                            if (message == "Close connection")
                                ok = false;
                            key = null;
                            message = null;
                        }
                        else
                            key= data;
                    }
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
        }
        public static string Decrypt(string key,string message)
        {
            char[] temp = key.ToCharArray();
            string decrypt = "";
            Array.Sort(temp);
            string keyCopy = new string(temp);
            List<int> pos = new List<int>();
            Dictionary<char, int> hash = new Dictionary<char, int>();
            int find = -1;
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
            List<List<char>>list = new List<List<char>>();
            for(int i = 0; i < message.Length/key.Length; i++)
            {
                List<char> c = new List<char>();
                for (int j = 0; j < key.Length; j++)
                    c.Add('\0');
                list.Add(c);
            }
            for (int i=0;i<key.Length;i++)
            {
                int index = pos.FindIndex(a => a == i);
                string temp1 = message.Substring(i * (message.Length / key.Length), (message.Length / key.Length));
                for (int j = 0; j < message.Length / key.Length; j++)
                    list[j][index] = temp1[j];
            }
            for (int i = 0; i < list.Count; i++)
                for (int j = 0; j < list[i].Count; j++)
                    decrypt += list[i][j];
            return decrypt;
        }
        public static void Main()
        {

            Connect();
        }
    }
}

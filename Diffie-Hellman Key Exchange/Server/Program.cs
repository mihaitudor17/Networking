using Diffie_Hellman_Key_Exchange;
using System.Net.Sockets;
TcpListener connect(string ip_addr = "127.0.0.1", Int32 port = 13000)
{
    TcpListener server = new TcpListener(System.Net.IPAddress.Parse(ip_addr), port);
    server.Start();
    return server;
}
void send(TcpListener tcp, string message)
{
    TcpClient client = tcp.AcceptTcpClient();
    NetworkStream stream = client.GetStream();
    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
    stream.Write(data, 0, data.Length);
    Console.WriteLine("Sent: {0}", message);
    Thread.Sleep(1);
    stream.Close();
    client.Close();
}
string receive(TcpListener tcp)
{
    TcpClient client = tcp.AcceptTcpClient();
    NetworkStream stream = client.GetStream();
    int i;
    String data = null;
    Byte[] bytes = new Byte[256];
    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
    {
        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
    }
    Console.WriteLine("Received: {0}", data);
    stream.Close();
    client.Close();
    return data.ToString();
}
string caesarDecrypt(string message,int key)
{
    List<char> letters=new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    string decMessage = "";
    foreach (char x in message)
    {
        char c;
        bool flag = false;
        if (Char.IsUpper(x))
        {
            flag = true;
            c = Char.ToLower(x);
        }
        else
            c = x;
        if (letters.Contains(c))
        {
            int index = letters.IndexOf(c) - key % letters.Count();
            if (index < 0)
                index = letters.Count() + index;
            if (flag)
                decMessage += Char.ToUpper(letters[index]);
            else
                decMessage += letters[index];
        }
        else
        {
            if (flag)
                decMessage += Char.ToUpper(c);
            else
                decMessage += c;
        }
    }
    return decMessage;
}
TcpListener tcp = connect();
    try
    {
        
        bool ok = true;
        int n = Int32.Parse(receive(tcp));
        int g = Int32.Parse(receive(tcp));
        int a = Int32.Parse(receive(tcp));
        Server server = new Server(n, g, tcp);
        server.A = a;
        send(server.TcpServer, server.B.ToString());
        Console.WriteLine("Key: "+server.Key);
    while (ok)
        {
            string message = caesarDecrypt(receive(server.TcpServer),server.Key);
            
            Console.WriteLine("Decrypted message: "+message);
            if (message == "Close connection")
                ok = false;
        }
}
    catch (SocketException e)
    {
        Console.WriteLine("SocketException: {0}", e);
    }
    finally
    {
        tcp.Stop();
    }

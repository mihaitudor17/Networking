using Diffie_Hellman_Key_Exchange;
using System.Net.Sockets;

TcpClient connect(string ip_addr="127.0.0.1",Int32 port=13000)
{
    TcpClient client = new TcpClient(ip_addr, port);
    return client;
}
void send(string message)
{
    TcpClient client = connect();
    NetworkStream stream = client.GetStream();
    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
    stream.Write(data, 0, data.Length);
    Console.WriteLine("Sent: {0}", message);
    Thread.Sleep(1);
    stream.Close();
    client.Close();
}
string receive()
{
    TcpClient client=connect();
    NetworkStream stream = client.GetStream();
    int i;
    String data = null;
    Byte[] bytes = new Byte[256];
    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
    {
        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
    }
    Console.WriteLine("Received: {0}", data);
    return data.ToString();
    stream.Close();
    client.Close();
}
string caesarCrypt(string message,int key)
{
    List<char> letters = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    string cryptMessage = "";
    foreach (char c in message)
    {
        if (letters.Contains(c))
        {
            int index = letters.IndexOf(c) + key % letters.Count();
            if (index > letters.Count-1)
                index = index-letters.Count();
            cryptMessage += letters[index];
        }
        else
            cryptMessage += c;
    }
    return cryptMessage;
}

    int n, g;
    Console.WriteLine("Insert prime number: ");
    n = Int32.Parse(Console.ReadLine());
    Console.WriteLine("Insert primitive root: ");
    g = Int32.Parse(Console.ReadLine());
    try
    {
        Client client = new Client(n, g);
        send(n.ToString());
        send(g.ToString()); 
        send(client.A.ToString());
        client.B=Int32.Parse(receive());
        Console.WriteLine("Key: "+client.Key);
        bool ok = true;
        while (ok)
        {
            System.Console.Write("Please input the message: ");
            string message = System.Console.ReadLine();
            send(caesarCrypt(message,client.Key));
            if (message == "Close connection")
                ok = false;
        }
    }
    catch (ArgumentNullException e)
    {
        Console.WriteLine("ArgumentNullException: {0}", e);
    }
    catch (SocketException e)
    {
        Console.WriteLine("SocketException: {0}", e);
    }



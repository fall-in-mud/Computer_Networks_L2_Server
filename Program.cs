using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


class SimpleServer
{
    public static void Main()
    {
        UInt16 port = 2003;
        IPAddress address = IPAddress.Parse("127.0.0.1");
        byte[] messageBytes = new byte[1000];
        string messageText;
        string answerText;
        try
        {
            TcpListener server = new(address, port);
            server.Start();
            TcpClient client = server.AcceptTcpClient();
            while (true)
            {
                NetworkStream clientStream = client.GetStream();
                int n = clientStream.Read(messageBytes, 0, messageBytes.Length);
                messageText = System.Text.Encoding.ASCII.GetString(messageBytes, 0, n);
                byte[] answerBytes = new byte[n];
                for (int i = 0; i < n; i++)
                {
                    answerBytes[i] = messageBytes[n - i - 1];
                }
                answerText = System.Text.Encoding.ASCII.GetString(answerBytes, 0, n);
                clientStream.Write(answerBytes, 0, n);
            }
        }
        catch
        {

        }
    }
}
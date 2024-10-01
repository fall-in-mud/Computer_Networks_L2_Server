using System.Net;
using System.Net.Sockets;
using Server;
class Program
{
    public static void Main()
    {
        Console.WriteLine();
    }
}

namespace Server
{
    class SimpleServer
    {
        public void Launch(ushort port)
        {
            byte[] recievedBytes = new byte[1024];
            string recievedText;
            string answerText;
            TcpListener server = new(IPAddress.Any, port);
            try
            {
                server.Start();
                Console.WriteLine($"Broadcast IP address of the server: {IPAddress.Broadcast}");
                while (true)
                {
                    Console.WriteLine("Waiting for client...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connection establshed");

                    NetworkStream clientStream = client.GetStream();
                    int n = clientStream.Read(recievedBytes, 0, recievedBytes.Length);
                    recievedText = System.Text.Encoding.ASCII.GetString(recievedBytes, 0, n);
                    Console.WriteLine($"Recieved message: {recievedText}");

                    byte[] answerBytes = new byte[n];
                    answerText = ReverseMessage(recievedText);
                    answerBytes = System.Text.Encoding.ASCII.GetBytes(answerText);
                    clientStream.Write(answerBytes, 0, n);
                    Console.WriteLine($"Sent message: {answerText}\n");

                    client.Close();
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong...");
            }
            finally
            {
                server.Stop();
            }
        }

        public static string ReverseMessage(string message)
        {
            char[] charArray = message.ToCharArray();
            Array.Reverse(charArray);
            return new(charArray);
        }
    }

    class SimpleClient
    {
        readonly ushort port = 2003;
        public void SendMessage(string messageText, string ip, ushort port)
        {
            try
            {
                TcpClient server = new(ip, port);
                Console.WriteLine("Connection established");

                byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(messageText);
                NetworkStream clientStream = server.GetStream();
                clientStream.Write(messageBytes, 0, messageBytes.Length);
                Console.WriteLine($"Message sent: {messageText}");

                byte[] buff = new byte[1024];
                int n = clientStream.Read(buff, 0, buff.Length);
                string recievedText = System.Text.Encoding.ASCII.GetString(buff, 0, n);
                Console.WriteLine($"Recieved message: {recievedText}");

                clientStream.Close();
                server.Close();
            }
            catch
            {
                Console.WriteLine("Something went wrong...");
            }

        }
    }
}
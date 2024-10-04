using Server;
using System.Net;
using System.Net.Sockets;
class Program
{
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            if (args[0] == "server" && args.Length == 2)
            {
                SimpleServer server = new();
                server.Launch(Convert.ToUInt16(args[1]));
            }
            else if (args[0] == "client" && args.Length == 4)
            {
                SimpleClient client = new();
                client.SendMessage(args[1], args[2], Convert.ToUInt16(args[3]));
            }
            else
            {
                Console.WriteLine("Wrong arguments");
            }
        }
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
            TcpListener listener = new(IPAddress.Any, port);
            try
            {
                listener.Start();
                while (true)
                {
                    Console.WriteLine("Waiting for client...");
                    TcpClient client = listener.AcceptTcpClient();
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
            catch( Exception e )
            {
                Console.WriteLine($"Something went wrong:\n{e}");
            }
            finally
            {
                listener.Stop();
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
        public void SendMessage(string messageText, string ip, ushort port)
        {
            try
            {
                TcpClient client = new(ip, port);
                Console.WriteLine("Connection established");

                byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(messageText);
                NetworkStream clientStream = client.GetStream();
                clientStream.Write(messageBytes, 0, messageBytes.Length);
                Console.WriteLine($"Sent message: {messageText}");

                byte[] buff = new byte[1024];
                int n = clientStream.Read(buff, 0, buff.Length);
                string recievedText = System.Text.Encoding.ASCII.GetString(buff, 0, n);
                Console.WriteLine($"Recieved message: {recievedText}");

                clientStream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong:\n{ex}");
            }

        }
    }
}
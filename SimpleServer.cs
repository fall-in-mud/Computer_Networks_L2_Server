using System.Net;
using System.Net.Sockets;

namespace SimpleServer
{
    class SimpleServer
    {
        public static void Main()
        {
            ushort port = 2003;
            IPAddress address = IPAddress.Parse("127.0.0.1");
            byte[] messageBytes = new byte[1000];
            string messageText;
            string answerText;
            TcpListener server = new(address, port);
            try
            {
                server.Start();
                while (true)
                {
                    Console.WriteLine("Waiting for client...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connection establshed");

                    NetworkStream clientStream = client.GetStream();
                    int n = clientStream.Read(messageBytes, 0, messageBytes.Length);
                    messageText = System.Text.Encoding.ASCII.GetString(messageBytes, 0, n);
                    Console.WriteLine($"Recieved message: {messageText}");

                    byte[] answerBytes = new byte[n];
                    for (int i = 0; i < n; i++)
                    {
                        answerBytes[i] = messageBytes[n - i - 1];
                    }
                    answerText = System.Text.Encoding.ASCII.GetString(answerBytes, 0, n);
                    clientStream.Write(answerBytes, 0, n);
                    Console.WriteLine($"Sent message: {answerText}");

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
    }
}
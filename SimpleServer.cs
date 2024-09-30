using System.Net;
using System.Net.Sockets;

namespace SimpleServer
{
    class SimpleServer
    {
        public static void Main()
        {
            
            //ushort port = 2003;
            Console.Write("Enter port number: ");
            ushort port = Convert.ToUInt16(Console.ReadLine());
            //int.Parse(port);
            IPAddress address = IPAddress.Parse("127.0.0.1");
            byte[] messageBytes = new byte[1000];
            string recievedText;
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
                    recievedText = System.Text.Encoding.ASCII.GetString(messageBytes, 0, n);
                    Console.WriteLine($"Recieved message: {recievedText}");

                    byte[] answerBytes = new byte[n];
                    /*for (int i = 0; i < n; i++)
                    {
                        answerBytes[i] = messageBytes[n - i - 1];
                    }*/

                    //answerText = System.Text.Encoding.ASCII.GetString(answerBytes, 0, n);
                    answerText = ReverseMessage(recievedText);
                    answerBytes = System.Text.Encoding.ASCII.GetBytes(answerText);
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
        
        public static string ReverseMessage(string message)
        {
            //string  = "woman loving woman wlw";
            char[] charArray = message.ToCharArray();
            Array.Reverse(charArray);
            //string sentMessage = (char)charArray;
            return new(charArray);
            //Console.WriteLine(charArray);
        }
    }
}


using SGP.XUtility;
using System;
using System.Net.Sockets;
using System.Text;

public class Program
{
    private static void Main(string[] args)
    {

        Console.Title = "TcpByteSender";
        Console.WriteLine("IP: ");
        string ip = Console.ReadLine();
        Console.WriteLine("Port: ");
        int port = int.Parse(Console.ReadLine());


        using (TcpClient client = new TcpClient(ip, port))
        {
            Console.WriteLine("Connected to " + ip + ":" + port.ToString());
            using (NetworkStream nwStream = client.GetStream())
            {
                while (true)
                {
                    Console.Write("HANDLER>");
                    string textToSend = Console.ReadLine();
                    if (textToSend == "exit")
                    {
                        break;
                    }
                    else
                    {
                        int byteCount = Encoding.UTF8.GetByteCount(textToSend);
                        byte[] bytesToSend = new byte[byteCount + 4];
                        XConvert.ToByteArray(byteCount, bytesToSend, 0, XByteOrder.LittleEndian);
                        Encoding.UTF8.GetBytes(textToSend, 0, textToSend.Length, bytesToSend, 4);
                        nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        nwStream.Flush();
                        Console.WriteLine("Text Sended");
                    }
                }
            }
        }
    }
}

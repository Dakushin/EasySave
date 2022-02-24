using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestClient;

public class Client
{
    private const int Port = 6000;

    private static Socket _clientSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    
    static void Main()
    {
        LoopConnect();
        SendLoop();
        Console.ReadLine();
    }

    private static void SendLoop()
    {
        while (true)
        {
            string? request;
            do
            {
                Console.Write("Enter a request: ");
                request = Console.ReadLine();
            } while (string.IsNullOrEmpty(request));

            var buffer = Encoding.ASCII.GetBytes(request);
            _clientSocket.Send(buffer);

            var receivedBuffer = new byte[1024];
            var received = _clientSocket.Receive(receivedBuffer);
            var data = new byte[received];
            
            Array.Copy(receivedBuffer, data, received);
            
            Console.WriteLine($"Received: {Encoding.ASCII.GetString(data)}");
        }
    }

    private static void LoopConnect()
    {
        var attempts = 0;
        
        while (!_clientSocket.Connected)
        {
            try
            {
                attempts++;
                _clientSocket.Connect(GetLocalIpAddress(), Port);
            }
            catch (SocketException e)
            {
                Console.Clear();
                Console.WriteLine($"Connection attempts : {attempts}");
            }
        }
        
        Console.WriteLine("Connected");
    }
    
    private static IPAddress GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip;
            }
        }
        
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}
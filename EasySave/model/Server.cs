using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using EasySave.view.wpf.core;

namespace EasySave.model;

public class Server
{
    private const int Port = 6000;

    private static readonly Server Instance = new();
    
    private readonly Socket _serverSocket;
    private readonly List<Socket> _clientsSockets;
    private static readonly byte[] Buffer = new byte[1024];

    private Server()
    {
        _clientsSockets = new List<Socket>();
        
        _serverSocket = SetupServer();
        _serverSocket.BeginAccept(AcceptCallback, null);
    }

    public static Server GetInstance()
    {
        return Instance;
    }

    private Socket SetupServer()
    {
        var ipAddress = GetLocalIpAddress();
        var localEndPoint = new IPEndPoint(ipAddress, Port);
        
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      
        socket.Bind(localEndPoint);
        socket.Listen(3);

        return socket;
    }

    private void AcceptCallback(IAsyncResult asyncResult)
    {
        var client = _serverSocket.EndAccept(asyncResult);
        
        NotifyInUi($"{properties.Resources.Client_Connected}. {GetSocketEndPoint(client)}");
        
        client.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, ReceiveCallback, client);
        _clientsSockets.Add(client);

        _serverSocket.BeginAccept(AcceptCallback, null);
    }

    private void ReceiveCallback(IAsyncResult asyncResult)
    {
        if (asyncResult.AsyncState is not Socket client) return;
        
        var received = client.EndReceive(asyncResult);
        var dataBuffer = new byte[received];
        Array.Copy(Buffer, dataBuffer, received);

        var message = Encoding.ASCII.GetString(dataBuffer);
        
        var response = message switch
        {
            "test" => Encoding.ASCII.GetBytes("todo"),
            _ => Encoding.ASCII.GetBytes("invalid")
        };

        client.BeginSend(response, 0, response.Length, SocketFlags.None, SendCallback, client);

        client.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, ReceiveCallback, client);
    }

    private void SendCallback(IAsyncResult asyncResult)
    {
        if (asyncResult.AsyncState is not Socket client) return;

        client.EndSend(asyncResult);
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
    
    private static void NotifyInUi(string message)
    {
        Application.Current.Dispatcher.Invoke(() => ViewModelBase.NotifyInfo(message));
    }
    
    private static string GetSocketEndPoint(Socket socket)
    {
        var socketEndPoint = (socket.RemoteEndPoint ?? socket.LocalEndPoint) as IPEndPoint;
        var socketIp = socketEndPoint?.Address.MapToIPv4();
        var socketPort = socketEndPoint?.Port;

        return $"Ip [{socketIp}] - Port [{socketPort}]";
    }
}


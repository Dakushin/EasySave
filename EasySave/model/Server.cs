using System.Net.Sockets;
using System.Net;

namespace EasySave.model;

public class Server
{
    private Socket serversocket;
    private static Server _instance;
    private static int port = 4900;
    private Server()
    {
        OpenSocket();

    }
    public static Server GetInstance()
    { 
        if(_instance == null)
        {
            _instance = new Server();
        }
        return _instance;
    }

    private void OpenSocket()
    {
        var ipAdresse = IPAddress.Parse("127.0.0.1");
        var localEndPoint = new IPEndPoint(ipAdresse, port);
        serversocket = new Socket(ipAdresse.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        serversocket.Bind(localEndPoint);
        serversocket.Listen(2);
    }


}


using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using EasySave.view.wpf.core;

namespace EasySave.model;

public class Server
{
    private const int Port = 6000;
    
    // request methods
    private const string RequestMethodGetAllBackups = "get_all_backups";
    private const string RequestMethodExecuteAllBackups = "execute_all_backups";
    private const string RequestMethodExecuteBackup = "execute_backup";
    private const string RequestMethodResumeBackup = "resume_backup";
    private const string RequestMethodPauseBackup = "pause_backup";
    private const string RequestMethodStopBackup = "stop_backup";


    // responses
    private const string ResponseInvalidRequest = "invalid_request";

    
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
        
        try {
            NotifyInUi($"{properties.Resources.Client_Connected}. {GetSocketEndPoint(client)}");
            
            client.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, ReceiveCallback, client);
            _clientsSockets.Add(client);

            _serverSocket.BeginAccept(AcceptCallback, null);
        }
        catch (SocketException)
        {
            DisconnectClient(client);
        }
    }

    private void ReceiveCallback(IAsyncResult asyncResult)
    {
        if (asyncResult.AsyncState is not Socket client) return;

        try
        {
            var received = client.EndReceive(asyncResult);
            var dataBuffer = new byte[received];
            Array.Copy(Buffer, dataBuffer, received);

            var request = Encoding.ASCII.GetString(dataBuffer);
        
            var response = Encoding.ASCII.GetBytes(HandleRequest(request));

            client.BeginSend(response, 0, response.Length, SocketFlags.None, SendCallback, client);

            client.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, ReceiveCallback, client);
        }
        catch (SocketException)
        {
            DisconnectClient(client);
        }
    }

    private static string HandleRequest(string request)
    {
        try
        {
            var split = request.Split(':');

            var nbParameters = split.Length - 1; // the method doesn't count as a parameter
            var method = split[0];
            var parameters = new string[nbParameters];

            for (var i = 0; i < nbParameters; i++)
            {
                parameters[i] = split[i + 1];
            }
        
            var response = method switch
            {
                RequestMethodGetAllBackups => OnGetAllBackups(),
                RequestMethodExecuteAllBackups => OnExecuteAllBackups(),
                RequestMethodExecuteBackup => OnExecuteBackup(parameters[0]),
                RequestMethodResumeBackup => OnResumeBackup(parameters[0]),
                RequestMethodPauseBackup => OnPauseBackup(parameters[0]),
                RequestMethodStopBackup => OnStopBackup(parameters[0]),
                _ => ResponseInvalidRequest
            };

            return response;
        }
        catch (Exception)
        {
            return "Exception occured in the server while handling the request. Please check the parameters.";
        }
    }

    private static string OnGetAllBackups()
    {
        return "OnGetAllBackups";
    }
    
    private static string OnExecuteAllBackups()
    {
        return "OnExecuteAllBackups";
    }
    
    private static string OnExecuteBackup(string backupName)
    {
        return "OnExecuteBackup";
    }
    
    private static string OnResumeBackup(string backupName)
    {
        return "OnResumeBackup";
    }
    
    private static string OnPauseBackup(string backupName)
    {
        return "OnPauseBackup";
    }
    
    private static string OnStopBackup(string backupName)
    {
        return "OnStopBackup";
    }

    private void SendCallback(IAsyncResult asyncResult)
    {
        if (asyncResult.AsyncState is not Socket client) return;

        try
        {
            client.EndSend(asyncResult);
        }
        catch (SocketException)
        {
            DisconnectClient(client);
        }
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

    private void DisconnectClient(Socket client)
    {
        NotifyInUi($"{properties.Resources.Client_Disconnected}. {GetSocketEndPoint(client)}");

        _clientsSockets.Remove(client);
        client.Shutdown(SocketShutdown.Both);
        client.Disconnect(false);
    }
}


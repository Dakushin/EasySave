using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using EasySave.properties;
using EasySave.view.wpf.core;
using EasySave.viewmodel;

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
    private const string ResponseInvalidBackupName = "invalid_backup_name";
    private const string ResponseSuccessGetAllBackups = "success_get_all_backups";
    private const string ResponseSuccessExecuteBackup = "success_execute_backup";
    private const string ResponseSuccessExecuteAllBackup = "success_execute_all_backups";
    private const string ResponseSuccessResumeBackup = "success_resume_backup";
    private const string ResponseSuccessPauseBackup = "success_pause_backup";
    private const string ResponseSuccessStopBackup = "success_stop_backup";

    private const char Separator = '$';


    private static readonly Server Instance = new();
    private static readonly byte[] Buffer = new byte[1024];
    private readonly List<Socket> _clientsSockets;

    private readonly Socket _serverSocket;
    private BackupsViewModel _backupsViewModel;

    private Server()
    {
        _clientsSockets = new List<Socket>();

        Task.Run(ScanForRequestServerIp);

        _serverSocket = SetupServer();
        _serverSocket.BeginAccept(AcceptCallback, null);
    }

    public static Server GetInstance()
    {
        return Instance;
    }

    private static void ScanForRequestServerIp()
    {
        var udpClient = new UdpClient(8888);
        var response = Encoding.ASCII.GetBytes("ip");

        while (true)
        {
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            udpClient.Receive(ref clientEndPoint);
            udpClient.Send(response, response.Length, clientEndPoint);
        }
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

        try
        {
            NotifyInUi($"{Resources.Client_Connected}. {GetSocketEndPoint(client)}");

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

    private string HandleRequest(string request)
    {
        try
        {
            var split = request.Split(Separator);

            var nbParameters = split.Length - 1; // the method doesn't count as a parameter
            var method = split[0];
            var parameters = new string[nbParameters];

            for (var i = 0; i < nbParameters; i++) parameters[i] = split[i + 1];

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

    private string OnGetAllBackups()
    {
        return _backupsViewModel.Backups.Aggregate(ResponseSuccessGetAllBackups, (acc, backup) =>
            $"{acc}{Separator}{backup.Name}-{backup.SourcePath}-{backup.TargetPath}-{backup.BackupStrategyName}-{backup.Crypted}-{backup.Progression}"
        );
    }

    private string OnExecuteAllBackups()
    {
        _backupsViewModel.ExecAllBackup();

        return ResponseSuccessExecuteAllBackup;
    }

    private string OnExecuteBackup(string backupName)
    {
        var backup = Model.GetInstance().FindbyName(backupName);

        if (backup != null)
        {
            _backupsViewModel.ExecuteBackup(backup);
            return ResponseSuccessExecuteBackup;
        }

        return ResponseInvalidBackupName;
    }

    private string OnResumeBackup(string backupName)
    {
        var backup = Model.GetInstance().FindbyName(backupName);

        if (backup != null)
        {
            _backupsViewModel.ResumeBackup(backup);
            return ResponseSuccessResumeBackup;
        }

        return ResponseInvalidBackupName;
    }

    private string OnPauseBackup(string backupName)
    {
        var backup = Model.GetInstance().FindbyName(backupName);

        if (backup != null)
        {
            _backupsViewModel.PauseBackup(backup);
            return ResponseSuccessPauseBackup;
        }

        return ResponseInvalidBackupName;
    }

    private string OnStopBackup(string backupName)
    {
        var backup = Model.GetInstance().FindbyName(backupName);

        if (backup != null)
        {
            _backupsViewModel.CancelBackup(backup);
            return ResponseSuccessStopBackup;
        }

        return ResponseInvalidBackupName;
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
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
        
        socket.Connect("8.8.8.8", 65530);

        if (socket.LocalEndPoint is not IPEndPoint endPoint)
        {
            throw new Exception("Couldn't resolve the local network IP address");
        }
        
        return endPoint.Address;
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
        NotifyInUi($"{Resources.Client_Disconnected}. {GetSocketEndPoint(client)}");

        _clientsSockets.Remove(client);
        client.Shutdown(SocketShutdown.Both);
        client.Disconnect(false);
    }

    public void SetBackupsViewModel(BackupsViewModel backupsViewModel)
    {
        _backupsViewModel = backupsViewModel;
    }
}
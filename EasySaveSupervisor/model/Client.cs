using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using EasySaveSupervisor.view.core;
using EasySaveSupervisor.viewmodel;

namespace EasySaveSupervisor.model;

public class Client
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

    private static readonly Client Instance = new();
    private readonly Socket _clientSocket;
    private BackupsViewModel _backupsViewModel;

    private Client()
    {
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        Task.Run(() =>
        {
            LoopConnect();

            while (true)
            {
                Application.Current.Dispatcher.Invoke(() =>_backupsViewModel.GetAllBackups());
                Thread.Sleep(1000);
            }
        });
    }

    public static Client GetInstance()
    {
        return Instance;
    }

    public void SetBackupsViewModel(BackupsViewModel backupsViewModel)
    {
        _backupsViewModel = backupsViewModel;
    }

    public Backup[] GetAllBackups()
    {
        var response = SendRequest(RequestMethodGetAllBackups);

        var split = response.Split(Separator);
        if (split[0].Equals(ResponseSuccessGetAllBackups))
        {
            var backups = new Backup[split.Length - 1];
            
            for (int i = 1; i < split.Length; i++)
            {
                var backupInformation = split[i].Split("-");

                backups[i - 1] = new Backup(
                    backupInformation[0],
                    backupInformation[1],
                    backupInformation[2],
                    backupInformation[3],
                    bool.Parse(backupInformation[4]),
                    int.Parse(backupInformation[5])
                );
            }

            return backups;
        }

        NotifyInUi("ERREUR !!!");
        return Array.Empty<Backup>();
    }

    public void ExecuteAllBackups()
    {
        var response = SendRequest(RequestMethodExecuteAllBackups);
        
        NotifyInUi(response);
    }
    
    public void ExecuteBackup(Backup backup)
    {
        var response = SendRequest(RequestMethodExecuteBackup, backup.Name);
        
        NotifyInUi(response);
    }
    
    public void ResumeBackup(Backup backup)
    {
        var response = SendRequest(RequestMethodResumeBackup, backup.Name);
        
        NotifyInUi(response);
    }
    
    public void PauseBackup(Backup backup)
    {
        var response = SendRequest(RequestMethodPauseBackup, backup.Name);
        
        NotifyInUi(response);
    }
    
    public void CancelBackup(Backup backup)
    {
        var response = SendRequest(RequestMethodStopBackup, backup.Name);
        
        NotifyInUi(response);
    }

    private string SendRequest(string request, params string[] parameters)
    {
        request = parameters.Aggregate(request, (acc, parameter) => 
            $"{acc}{Separator}{parameter}"
        );
            
        var buffer = Encoding.ASCII.GetBytes(request);
        _clientSocket.Send(buffer);

        var receivedBuffer = new byte[1024];
        var received = _clientSocket.Receive(receivedBuffer);
        var data = new byte[received];

        Array.Copy(receivedBuffer, data, received);

        return Encoding.ASCII.GetString(data);
    }

    private void LoopConnect()
    {
        var attempts = 0;

        while (!_clientSocket.Connected)
            try
            {
                attempts++;
                _clientSocket.Connect(GetLocalIpAddress(), Port);
            }
            catch (SocketException e)
            {
                NotifyInUi($"Connection attempts : {attempts}");
            }

        NotifyInUi("Connected");
    }

    private static IPAddress GetLocalIpAddress()
    {
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            return endPoint.Address;
        }
        //
        // var host = Dns.GetHostEntry(Dns.GetHostName());
        // foreach (var ip in host.AddressList)
        //     if (ip.AddressFamily == AddressFamily.InterNetwork)
        //         return ip;
        //
        // throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    private static void NotifyInUi(string message)
    {
        Application.Current.Dispatcher.Invoke(() => ViewModelBase.NotifyInfo(message));
    }
}
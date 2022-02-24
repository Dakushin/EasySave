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

    private static readonly Client Instance = new();
    private readonly Socket _clientSocket;
    private BackupsViewModel _backupsViewModel; 
    
    private Client()
    {
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        Task.Run(() =>
        {
            LoopConnect();
            SendLoop();
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
    
    private void SendLoop()
    {
        while (true)
        {
            string? request;
            do
            {
                NotifyInUi("Enter a request: ");
                request = Console.ReadLine();
            } while (string.IsNullOrEmpty(request));

            var buffer = Encoding.ASCII.GetBytes(request);
            _clientSocket.Send(buffer);

            var receivedBuffer = new byte[1024];
            var received = _clientSocket.Receive(receivedBuffer);
            var data = new byte[received];
            
            Array.Copy(receivedBuffer, data, received);
            
            NotifyInUi($"Received: {Encoding.ASCII.GetString(data)}");
        }
    }

    private void LoopConnect()
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
                NotifyInUi($"Connection attempts : {attempts}");
            }
        }
        
        NotifyInUi("Connected");
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
}
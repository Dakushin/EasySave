﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using EasySaveSupervisor.properties;
using EasySaveSupervisor.view.core;
using EasySaveSupervisor.viewmodel;

namespace EasySaveSupervisor.model;

/**
 * A singleton client which uses a socket to send request to the server.
 */
public class Client
{
    private const int Port = 6000;

    // client requests
    private const string RequestMethodGetAllBackups = "get_all_backups";
    private const string RequestMethodExecuteAllBackups = "execute_all_backups";
    private const string RequestMethodExecuteBackup = "execute_backup";
    private const string RequestMethodResumeBackup = "resume_backup";
    private const string RequestMethodPauseBackup = "pause_backup";
    private const string RequestMethodStopBackup = "stop_backup";


    // server responses
    private const string ResponseSuccessGetAllBackups = "success_get_all_backups";
    
    /**
     * separator for the custom messaging protocol
     * nevertheless we should have used an existing protocol link json.
     *
     * here is the protocol : method$param1$param2$param_n
     *
     * for instance : 'execute_backup$hello_backup'. Or 'get_all_backups'.
     */
    private const char Separator = '$';

    private static readonly Client Instance = new();
    private Socket _clientSocket;
    private BackupsViewModel _backupsViewModel;

    /**
     * Starts the client socket in a new thread.
     * Looks for a server whenever it is disconnected, otherwise communicates with it. 
     */
    private Client()
    {
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        Task.Run(() =>
        {
            while (true)
            {
                LoopConnect();

                while (_clientSocket.Connected)
                {
                    Application.Current.Dispatcher.Invoke(() => _backupsViewModel.GetAllBackups());
                    Thread.Sleep(1000);
                }
                
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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

        return Array.Empty<Backup>();
    }

    public void ExecuteAllBackups()
    {
        SendRequest(RequestMethodExecuteAllBackups);
    }
    
    public void ExecuteBackup(Backup backup)
    {
        SendRequest(RequestMethodExecuteBackup, backup.Name);
    }
    
    public void ResumeBackup(Backup backup)
    {
        SendRequest(RequestMethodResumeBackup, backup.Name);
    }
    
    public void PauseBackup(Backup backup)
    {
        SendRequest(RequestMethodPauseBackup, backup.Name);
    }
    
    public void CancelBackup(Backup backup)
    {
        SendRequest(RequestMethodStopBackup, backup.Name);
    }

    private string SendRequest(string request, params string[] parameters)
    {
        try
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
        catch (SocketException)
        {
            var res = $"{Resources.Client_Disconnected}. {GetSocketEndPoint(_clientSocket)}";
            _clientSocket.Shutdown(SocketShutdown.Both);
            _clientSocket.Disconnect(false);
            return res;
        }
    }

    private void LoopConnect()
    {
        var attempts = 0;

        while (!_clientSocket.Connected)
            try
            {
                attempts++;
                _clientSocket.Connect(GetServerIpAddress(), Port);
            }
            catch (SocketException)
            {
                NotifyInUi($"{Resources.Connection_Attempt} : {attempts}");
            }

        NotifyInUi(Resources.Connected);
    }

    /**
     * Find the server ipv4 address by network discovering using UDP broadcast address.
     */
    private static IPAddress GetServerIpAddress()
    {
        var client = new UdpClient();
        var request = Encoding.ASCII.GetBytes("get_ip");
        var serverEndPoint = new IPEndPoint(IPAddress.Any, 0);

        client.EnableBroadcast = true;
        client.Client.ReceiveTimeout = 3000;
        client.Send(request, request.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

        client.Receive(ref serverEndPoint);

        var serverIp = serverEndPoint.Address;
        client.Close();

        return serverIp;
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
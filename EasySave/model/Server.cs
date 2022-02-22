using System.Net.Sockets;

namespace EasySave.model
{
    public class Server
    {
        static public Socket serversocket;
        Server _instance;
        private Server()
        { 
        
        }
        public Server GetInstance()
        {
            if(_instance == null)
            {
                _instance = new Server();
            }
            return _instance;
        }

        private void OpenSocket()
        {

        }


    }
}

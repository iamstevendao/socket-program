using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadServerForm
{
    public class HandleClient
    {
        private TcpClient clientSocket;

        private string clNo;

        public void StartClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            Thread ctThread = new Thread(DoChat);
            ctThread.Start();
        }

        public void Chat()
        {
            Byte[] sendBytes = null;
            string serverResponse = null;
            NetworkStream networkStream = clientSocket.GetStream();
            serverResponse = "Server to clinet(" + clNo + "): " + "hello world";
            sendBytes = Encoding.ASCII.GetBytes(serverResponse);
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            networkStream.Flush();
        }

        private void DoChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;
            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, 10000);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine(" <<<<<<< " + "RECEIVED: client-" + clNo + ": " + dataFromClient);
                    rCount = Convert.ToString(requestCount);
                    serverResponse = "Server to clinet(" + clNo + "): " + rCount;
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    for (int i = 0; i < 5; i++)
                    {
                        networkStream.Write(sendBytes, 0, sendBytes.Length);
                        networkStream.Flush();
                    }
                    Console.WriteLine(" >>>>>>> SENT: " + serverResponse + "\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                }
            }
        }
    }
}

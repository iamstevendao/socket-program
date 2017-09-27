using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiThreadServerForm
{
    public partial class ServerForm : Form
    {
        List<TcpClient> clients = new List<TcpClient>();

        private System.Windows.Forms.Timer timer1;
        private void InitTimer()
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(Timer1_Tick);
            timer1.Interval = 2000; // in miliseconds
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            foreach (TcpClient tcp in clients.ToList())
            {
                if (tcp.Client.Poll(0, SelectMode.SelectRead))
                {
                    byte[] buff = new byte[1];
                    if (tcp.Client.Receive(buff, SocketFlags.Peek) == 0)
                        clients.Remove(tcp);
                }
            }
            SetLable("Connected: " + clients.Count.ToString());
        }

        public ServerForm()
        {
            InitializeComponent();
        }


        private void BtnSend_Click(object sender, EventArgs e)
        {
            Byte[] sendBytes = null;
            string serverResponse = "hello world";
            foreach (TcpClient client in clients)
            {
                NetworkStream networkStream = client.GetStream();
                sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                networkStream.Write(sendBytes, 0, sendBytes.Length);
                networkStream.Flush();
            }
        }

        private void ServerForm_Load(object sender, EventArgs e)        {            InitTimer();            Thread listening = new Thread(Listening);            listening.Start();        }
        
        private void Listening()
        {            TcpListener serverSocket = new TcpListener(8888);
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            while (true)
            {
                clientSocket = serverSocket.AcceptTcpClient();
                clients.Add(clientSocket);
                SetLable("Connected: " + clients.Count.ToString());
            }
        }

        delegate void SetTextCallback(string text);

        private void SetLable(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lbStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLable);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lbStatus.Text = text;
            }
        }
    }
}

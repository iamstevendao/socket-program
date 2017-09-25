using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiThreadServerForm
{
    public partial class ServerForm : Form
    {
        List<TcpClient> clients = new List<TcpClient>();
        public ServerForm()
        {
            InitializeComponent();
        }


        private void btnSend_Click(object sender, EventArgs e)
        {

        }

        private void ServerForm_Load(object sender, EventArgs e)        {            TcpListener serverSocket = new TcpListener(8888);
            TcpClient clientSocket = default(TcpClient);

            int counter = 0;
            serverSocket.Start();
            SetLable(">>> Server Started");
            counter = 0;
            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                HandleClient client = new HandleClient();
                client.StartClient(clientSocket, Convert.ToString(counter));
            }
            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();        }

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

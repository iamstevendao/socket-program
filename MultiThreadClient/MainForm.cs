using System;using System.Net.Sockets;using System.Threading;
using System.Windows.Forms;namespace MultiThreadClient{    public partial class MainForm : Form    {        private TcpClient clientSocket = new TcpClient();        private NetworkStream serverStream;

        private const string SERVER = "127.0.0.1";
        private const int PORT = 8888;
        public MainForm()        {            InitializeComponent();        }
        public void UpdateLog(string mess)        {            txtLog.Text = txtLog.Text + Environment.NewLine + " >> " + mess;        }

        private void BtnSend_Click(object sender, EventArgs e)        {            NetworkStream serverStream = clientSocket.GetStream();            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("This is a message from Client$");            serverStream.Write(outStream, 0, outStream.Length);            serverStream.Flush();            byte[] inStream = new byte[10025];            serverStream.Read(inStream, 0, 10000);            string returndata = System.Text.Encoding.ASCII.GetString(inStream);            UpdateLog("Data from Server : " + returndata);
            //KeepListening();

            var childSocketThread = new Thread(() => KeepListening());            childSocketThread.Start();        }

        private void KeepListening()
        {
                NetworkStream serverStream = clientSocket.GetStream();
                //byte[] outStream = System.Text.Encoding.ASCII.GetBytes("This is a message from Client$");
                //serverStream.Write(outStream, 0, outStream.Length);
                //serverStream.Flush();
                byte[] inStream = new byte[10025];
                serverStream.Read(inStream, 0, 10000);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                UpdateLog("Data from Server : " + returndata);
          
        }

        private void MainForm_Load(object sender, EventArgs e)        {            UpdateLog("Client Started");            clientSocket.Connect(SERVER, PORT);            lbStatus.Text = "Client Socket Program - Server Connected ...";        }    }}
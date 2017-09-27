using System;using System.Net.Sockets;using System.Threading;
using System.Windows.Forms;namespace MultiThreadClient{    public partial class MainForm : Form    {        private TcpClient clientSocket = new TcpClient();
        Thread childSocketThread;        private NetworkStream serverStream;

        private const string SERVER = "127.0.0.1";
        private const int PORT = 8888;
        public MainForm()        {            InitializeComponent();        }
        public void UpdateLog(string mess)        {            SetText(txtLog.Text + Environment.NewLine + " >> " + mess);        }

        private void BtnSend_Click(object sender, EventArgs e)        {            NetworkStream serverStream = clientSocket.GetStream();            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("This is a message from Client$");            serverStream.Write(outStream, 0, outStream.Length);            serverStream.Flush();            byte[] inStream = new byte[10025];            serverStream.Read(inStream, 0, 10000);            string returndata = System.Text.Encoding.ASCII.GetString(inStream);            UpdateLog("Data from Server : " + returndata);
            //KeepListening();
        }

        private void KeepListening()
        {
            while (true)
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
          
        }
        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtLog.Text = text;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)        {            UpdateLog("Client Started");            clientSocket.Connect(SERVER, PORT);            childSocketThread = new Thread(KeepListening);            childSocketThread.Start();            lbStatus.Text = "Client Socket Program - Server Connected ...";        }        private void MainForm_Closing(object sender, EventArgs e)        {            childSocketThread.Abort();            clientSocket.GetStream().Close();            clientSocket.Close();        }    }}
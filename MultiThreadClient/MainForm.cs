﻿using System;
using System.Windows.Forms;

        private const string SERVER = "127.0.0.1";
        private const int PORT = 8888;
        public MainForm()


        private void BtnSend_Click(object sender, EventArgs e)
            //KeepListening();

            var childSocketThread = new Thread(() => KeepListening());

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

        private void MainForm_Load(object sender, EventArgs e)
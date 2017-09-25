using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SynchronousSocketClient
{

    public static void StartClient()
    {
        // Data buffer for incoming data.  
        byte[] bytes = new byte[1024];
        List<Socket> senders = new List<Socket>();
        const int NUMBER_OF_CLIENT = 3;

        // Connect to a remote device.  
        try
        {
            // Establish the remote endpoint for the socket.  
            // This example uses port 11000 on the local computer.  
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
             IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.  
       //     for (int i = 0; i < NUMBER_OF_CLIENT; i++)
       //     {
                Socket sender = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
               // senders.Add(sender);
          //  }
            while (true)
            //    foreach (Socket sender in senders)
           //     {
                    // Connect the socket to the remote endpoint. Catch any errors.  
                    try
                    {
                        sender.Connect(remoteEP);

                        Console.WriteLine("Socket connected to {0}",
                            sender.RemoteEndPoint.ToString());

                        // Encode the data string into a byte array.  
                        byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                     //   while (true)
                     //   {
                            // Send the data through the socket.  
                            int bytesSent = sender.Send(msg);

                            // Receive the response from the remote device.  
                            int bytesRec = sender.Receive(bytes);
                            Console.WriteLine("Echoed test = {0}",
                                Encoding.ASCII.GetString(bytes, 0, bytesRec));
                           // Thread.Sleep(3000);
                       // }
                        // Release the socket.  
                        // sender.Shutdown(SocketShutdown.Both);
                        //  sender.Close();

                    }
                    catch (ArgumentNullException ane)
                    {
                        Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    }
                    catch (SocketException se)
                    {
                        Console.WriteLine("SocketException : {0}", se.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    }
           //     }
        
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static int Main(String[] args)
    {

        Console.WriteLine("******THIS IS SYNC CLIENT*******\n");

        StartClient();
        return 0;
    }
}
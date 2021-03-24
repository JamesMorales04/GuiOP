using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GuiOP
{
    /*
         * File sistem port: 8082 
         * GUI port: 8081
         * Kernel port: 8080
         * 
         */
    class Communication
    {
        MainWindow window;
        private string data = null;
        Messages messages;
        public Communication(MainWindow w)
        {
            window = w;
        }
        public void sendMessage(string msg, int port)
        {
            WriteToConsole(msg);
            msg += "<EOF>";
            byte[] msgAux = Encoding.ASCII.GetBytes(msg);
            StartClient(msgAux, port);
            Console.WriteLine(data);
        }
        private void StartClient(byte[] msg, int port)
        {
            byte[] bytes = new byte[1024];
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sender.Connect(remoteEP);
                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
                    int bytesSent = sender.Send(msg);
                    //int bytesRec = sender.Receive(bytes);
                    //messages.Actions(Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void StartListening(int port)
        {
            byte[] bytes = new Byte[1024];
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket handler = listener.Accept();
                    data = null;
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    messages.Actions(data);
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void SetterMessages(Messages messages)
        {
            this.messages = messages;
        }
        public void UpdateParentApp(string text)
        {
            string newText = text == "stopped" ? "Start" : "Stop";

            window.Dispatcher.Invoke(() => {
                window.ParentAppButton.Content = newText;
            });

        }
        public void SetAppPid(string app, string pid)
        {
            switch (app)
            {
                case "APP1":
                    SetApp1Pid(pid);
                    break;
                case "APP2":
                    SetApp2Pid(pid);
                    break;
                case "APP3":
                    SetApp3Pid(pid);
                    break;
            }
        }
        public void SetApp1Pid(string text)
        {
            window.Dispatcher.Invoke(() => {
                if (text == "")
                {
                    window.App1Button.Content = "Start";
                    window.App1PID.Content = "PID: ";
                }
                else
                {
                    window.App1Button.Content = "Stop";
                    window.App1PID.Content = "PID: " + text;
                }
            });
        }
        public void SetApp2Pid(string text)
        {
            window.Dispatcher.Invoke(() => {
                if (text == "")
                {
                    window.App2Button.Content = "Start";
                    window.App2PID.Content = "PID: ";
                }
                else
                {
                    window.App2Button.Content = "Stop";
                    window.App2PID.Content = "PID: " + text;
                }
            });
        }
        public void SetApp3Pid(string text)
        {
            window.Dispatcher.Invoke(() => {
                if (text == "")
                {
                    window.App2Button.Content = "Start";
                    window.App3PID.Content = "PID: ";
                }
                else
                {
                    window.App2Button.Content = "Stop";
                    window.App3PID.Content = "PID: " + text;
                }
            });
        }
        public void WriteToConsole(String text)
        {

            window.Dispatcher.Invoke(() => {
                window.transactionBox.Text += GetTime() + text + Environment.NewLine;
                window.transactionBox.ScrollToEnd();
            });
        }
        public string GetTime()
        {
            return "[" + DateTime.Now.Hour.ToString("D2") + ":" + DateTime.Now.Minute.ToString("D2") + ":" + DateTime.Now.Second.ToString("D2") + "] ";
        }
    }
}

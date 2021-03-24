using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace GuiOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int port = 8080;
        Messages messages;
        Communication comms;
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        readonly System.Timers.Timer timer1;
        readonly DriveInfo dDrive;

        public MainWindow()
        {
            InitializeComponent();
            comms = new Communication(this);
            messages = new Messages(comms);

            comms.SetterMessages(messages);

            Thread listener = new Thread(() => comms.StartListening(8081));
            listener.Start();

            //Statistics
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            dDrive = new DriveInfo("C");
            //Timer
            timer1 = new System.Timers.Timer();
            timer1.Elapsed += new ElapsedEventHandler(systemStatus);
            timer1.Elapsed += new ElapsedEventHandler(folders);

            timer1.Interval = 1000;
            timer1.Start();

            systemStatus(null, EventArgs.Empty);
        }

        private void systemStatus(object sender, EventArgs e)
        {
            string system = "CPU USAGE: " + Math.Round(cpuCounter.NextValue(), 2) + "%" + Environment.NewLine;
            system += "FREE RAM: " + ramCounter.NextValue() + "MB" + Environment.NewLine;
            system += "FREE HDD (C:): " + dDrive.AvailableFreeSpace / 1073741824 + "GB";

            Dispatcher.Invoke(() => {
                statusBox.Text = system;
            });
        }

        private void folders(object sender, EventArgs e)
        {
            string[] dirs = Directory.GetDirectories(@"\", "p*", SearchOption.TopDirectoryOnly);

            Dispatcher.Invoke(() => {
                listViewButton.Items.Clear();
            });
            for(int i=0; i<dirs.Length; i++)
            {
                Dispatcher.Invoke(() => {
                    Button button = new Button();
                    button.Content = dirs[i].Replace("\\", "");

                    button.Click += new RoutedEventHandler(folder_Click);

                    listViewButton.Items.Add(button);
                });
            }
        }

        private void folder_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            //comms.WriteToConsole("{cmd:delete,src:GUI,dest:GestorArc,msg:\"" + btn.Content + "\"}");
            comms.sendMessage("{cmd:delete,src:GUI,dest:GestorArc,msg:\"" + btn.Content + "\"}", port);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)ParentAppButton.Content == "Stop")
            {
                comms.sendMessage("{cmd:stop,src:GUI,dest:kernel,msg:\"halt\"}", port);
            }
            else
            {
                comms.sendMessage("{cmd:start,src:GUI,dest:kernel,msg:\"start\"}", port);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if ((string)App1Button.Content == "Stop")
            {
                App1PID.Content = "PID: ";
                App1Button.Content = "Start";
                comms.sendMessage("{cmd:halt,src:GUI,dest:APP,msg:\"APP1\"}", port);
            }
            else
            {
                comms.sendMessage("{cmd:start,src:GUI,dest:APP,msg:\"APP1\"}", port);
            }
            
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if ((string)App2Button.Content == "Stop")
            {
                App2PID.Content = "PID: ";
                App2Button.Content = "Start";
                comms.sendMessage("{cmd:halt,src:GUI,dest:APP,msg:\"APP2\"}", port);
            }
            else
            {
                comms.sendMessage("{cmd:start,src:GUI,dest:APP,msg:\"APP2\"}", port);
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if ((string)App3Button.Content == "Stop")
            {
                App3PID.Content = "PID: ";
                App3Button.Content = "Start";
                comms.sendMessage("{cmd:halt,src:GUI,dest:APP,msg:\"APP3\"}", port);
            }
            else
            {
                comms.sendMessage("{cmd:start,src:GUI,dest:APP,msg:\"APP3\"}", port);
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (folderInputBox.Text == "")
            {
                comms.WriteToConsole("Empty folder name");
            }
            else
            {
                //comms.WriteToConsole("{cmd:create,src:GUI,dest:GestorArc,msg:\"" + folderInputBox.Text + "\"}");
                comms.sendMessage("{cmd:create,src:GUI,dest:GestorArc,msg:\"" + folderInputBox.Text + "\"}", port);
                folderInputBox.Text = "";
            }
        }
    }
}

using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuiOP
{
    public partial class Form1 : Form
    {
        ProcessHandler processHandler;
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        readonly Timer timer1;
        readonly DriveInfo dDrive;

        public Form1()
        {
            InitializeComponent();
            //Statistics
            processHandler = new ProcessHandler(this);
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            dDrive = new DriveInfo("C");
            //Timer
            timer1 = new Timer();
            timer1.Tick += new EventHandler(systemStatus);
            timer1.Interval = 1000;
            timer1.Start();
            systemStatus(null, EventArgs.Empty);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            processHandler.App1_start();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            processHandler.App2_start();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            processHandler.App3_start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            processHandler.Explorer();
        }
        private void systemStatus(object sender, EventArgs e)
        {
            string system = "CPU USAGE: " + Math.Round(cpuCounter.NextValue(), 2) + "%" + Environment.NewLine;
            system += "FREE RAM: " + ramCounter.NextValue() + "MB" + Environment.NewLine;
            system += "FREE HDD (C:): " + dDrive.AvailableFreeSpace / 1073741824 + "GB" + Environment.NewLine;
            textBox2.Text = system;
        }
    }
}

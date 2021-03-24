using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuiOP
{
    class ProcessHandler
    {

        private Process app1;
        private Process app2;
        private Process app3;
        private Messages messages;
        private Comms comms;
        public Form1 form;



        public ProcessHandler(Form1 form)
        {

            this.form = form;
            this.comms = new Comms();
            this.messages = new Messages(comms);

            comms.setterMessages(messages);

            Thread listener = new Thread(() => comms.StartListening(8081));
            listener.Start();

            Set_apps();
        }

        private void Set_apps()
        {
            app1 = new Process();
            app1.StartInfo.FileName = "IExplore.exe";
            app1.StartInfo.Arguments = "https://www.solruc.online";
            app1.EnableRaisingEvents = true;
            app1.Exited += new EventHandler(App1_Exited);

            app2 = new Process();
            app2.StartInfo.FileName = "notepad.exe";
            app2.StartInfo.Arguments = "D:/Downloads/trolling.txt";
            app2.EnableRaisingEvents = true;
            app2.Exited += new EventHandler(App2_Exited);

            app3 = new Process();
            app3.StartInfo.FileName = "cmd.exe";
            app3.StartInfo.Arguments = "D:/Downloads/trolling.txt";
            app3.EnableRaisingEvents = true;
            app3.Exited += new EventHandler(App3_Exited);
        }
        public void App1_start()
        {
            Process[] pname = Process.GetProcessesByName("IExplore");
            if (pname.Length == 0)
            {
                app1.Start();
                form.label8.BeginInvoke(new MethodInvoker(() =>
                {
                    form.label8.Visible = true;
                    form.label8.Text = app1.Id.ToString();
                    form.button1.Text = "Stop";
                }));
                Write("start","App1 has been started");
            }
            else
            {
                pname[0].Kill();
            }
        }
        private void App1_Exited(object sender, System.EventArgs e)
        {
            form.button1.BeginInvoke(new MethodInvoker(() =>
            {
                form.button1.Text = "Run";
                form.label8.Visible = false;
            }));
            Write("stop","App1 has been stopped");
        }
        public void App2_start()
        {
            Process[] pname = Process.GetProcessesByName("notepad");
            if (pname.Length == 0)
            {
                app2.Start();
                form.label8.BeginInvoke(new MethodInvoker(() =>
                {
                    form.label9.Visible = true;
                    form.label9.Text = app2.Id.ToString();
                    form.button2.Text = "Stop";
                }));
                Write("start","App2 has been started");
            }
            else
            {
                pname[0].Kill();
            }
        }
        private void App2_Exited(object sender, System.EventArgs e)
        {
            form.button2.BeginInvoke(new MethodInvoker(() =>
            {
                form.button2.Text = "Run";
                form.label9.Visible = false;
            }));
            Write("stop","App2 has been stopped");
        }
        public void App3_start()
        {
            Process[] pname = Process.GetProcessesByName("cmd");
            if (pname.Length == 0)
            {
                app3.Start();
                form.label10.BeginInvoke(new MethodInvoker(() =>
                {
                    form.label10.Visible = true;
                    form.label10.Text = app3.Id.ToString();
                    form.button3.Text = "Stop";
                }));
                Write("start","App3 has been started");
            }
            else
            {
                pname[0].Kill();
            }
        }
        private void App3_Exited(object sender, System.EventArgs e)
        {
            form.button3.BeginInvoke(new MethodInvoker(() =>
            {
                form.button3.Text = "Run";
                form.label10.Visible = false;
            }));
            Write("stop","App3 has been stopped");
        }
        public void Explorer()
        {
            Process.Start("Explorer.exe");
        }
        public void Write(string cmd, string text)
        {
            comms.sendMessage(cmd,"APP",text,8080);
            comms.sendMessage(cmd,"GestorArc",text, 8080);

            form.textBox1.BeginInvoke(new MethodInvoker(() =>
            {
                form.textBox1.AppendText(GetTime() + text + Environment.NewLine);
            }));
        }
        public string GetTime()
        {
            return "[" + DateTime.Now.Hour.ToString("D2") + ":" + DateTime.Now.Minute.ToString("D2") + ":" + DateTime.Now.Second.ToString("D2") + "] ";
        }
    }
}

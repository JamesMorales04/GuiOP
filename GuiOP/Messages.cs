using System;
using System.Collections.Generic;
using System.Linq;
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
    class Messages
    {
        private Communication comunication;

        public Messages(Communication comunication)
        {
            this.comunication = comunication;
        }

        public void Actions(string msg)
        {
            msg = msg.Replace("<EOF>", "");
            comunication.WriteToConsole(msg);
            string[] msgClean = msg.Replace("{", "").Replace("}", "").Split(',');

            if (msgClean.Length > 2)
            {
                longMessage(msgClean, msg);
            }
            else
            {
                shortMessage(msgClean);
            }

        }
        private void longMessage(string[] msgClean, string rawMsg)
        {
            switch (msgClean[0].Split(':')[1])
            {
                case "info":
                    HandleInfo(msgClean);
                    break;
                case "send":
                    HandleSend(msgClean);
                    break;
                case "halt":
                    HandleHalt(msgClean);
                    break;
                case "stop":
                    HandleStop();
                    break;
            }
        }
        public void HandleStop()
        {
            Environment.Exit(0);
        }
        public void HandleHalt(string[] msgClean)
        {
            string message = msgClean[3].Replace("\"", "").Split(':')[1];
            comunication.SetAppPid(message, "");
        }
        public void HandleInfo(string[] msgClean)
        {
            string message = msgClean[3].Replace("-", "").Replace("\"", "").Split(':')[1];
            string[] appInfo = message.Split('>');

            comunication.SetAppPid(appInfo[0], appInfo[1]);
        }
        public void HandleSend(string[] msgClean)
        {
            string message = msgClean[3].Replace("-", "").Replace("\"", "").Split(':')[1];
            string[] action = message.Split('>');

            switch (action[0])
            {
                case "Error":
                    comunication.WriteToConsole("Error: "+action[1]);
                    break;
                case "App":
                    comunication.UpdateParentApp(action[1]);
                    break;
            }

        }
        private void shortMessage(string[] msgClean)
        {
            switch (msgClean[1].Replace("\"", "").Split(':')[1])
            {
                case "OK":
                    break;
                case "0":
                    break;
                case "Err":
                    break;
                default:
                    break;
            }
        }
        public string Response()
        {
            var seed = Environment.TickCount;
            var random = new Random(seed);

            var value = random.Next(0, 2);
            string msg = "";

            switch (value)
            {
                case 0:
                    msg = "{codterm:0,msg:\"OK\"}<EOF>";
                    break;
                case 1:
                    msg = "{codterm:1,msg:\"0\"}<EOF>";
                    break;
                case 2:
                    msg = "{codterm:2,msg:\"Err\"}<EOF>";
                    break;
            }
            return msg;
        }

    }
}

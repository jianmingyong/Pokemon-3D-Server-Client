using System;
using System.Windows.Forms;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Modules;
using Pokemon_3D_Server_Core.Network;
using Pokemon_3D_Server_Core.Packages;

namespace Pokemon_3D_Server_Client_GUI
{
    /// <summary>
    /// Class containing Main GUI Element
    /// </summary>
    public partial class Main : Form
    {
        private delegate void QueueMessage_AddMessage_Safe(object myObject, MessageEventArgs myArgs);
        private bool ApplicationRestart = false;

        /// <summary>
        /// GUI Component Start Point
        /// </summary>
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // Add Handler
            QueueMessage.AddMessage += QueueMessage_AddMessage;
            RestartTrigger.RestartSwitch += Restart;

            // Setup Settings.
            Core.Setting.Setup();

            // Setup Settings
            Core.Setting.ApplicationDirectory = Environment.CurrentDirectory;
            if (Core.Setting.Load())
            {
                Core.Setting.Save();
            }
            else
            {
                Core.Setting.Save();
                Environment.Exit(0);
            }

            // Setup Server
            Core.Server.Start();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Core.Server.SendToAllPlayer(new Package(Package.PackageTypes.ServerClose, ApplicationRestart ? Core.Setting.Token("SERVER_RESTART") : Core.Setting.Token("SERVER_CLOSE"), null));
            Core.Setting.Save();
            Core.Logger.Add("Main.cs: Application closed successfully!", Logger.LogTypes.Info);

            if (ApplicationRestart)
            {
                Application.Restart();
                Application.ExitThread();
            }
        }

        private void QueueMessage_AddMessage(object myObject, MessageEventArgs myArgs)
        {
            try
            {
                if (Main_Logger.InvokeRequired)
                {
                    BeginInvoke(new QueueMessage_AddMessage_Safe(QueueMessage_AddMessage), myObject, myArgs);
                }
                else
                {
                    Main_Logger.AppendText(myArgs.OutputMessage + Functions.vbNewLine);
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        private void Restart(object myObject, EventArgs myArgs)
        {
            ApplicationRestart = true;
            Application.Exit();
        }
    }
}
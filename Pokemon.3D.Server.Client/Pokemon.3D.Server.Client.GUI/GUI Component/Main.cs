using System;
using System.Windows.Forms;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Modules;
using Pokemon_3D_Server_Core.Network;
using Pokemon_3D_Server_Core.Packages;
using System.Threading;
using System.Collections.Generic;

namespace Pokemon_3D_Server_Client_GUI
{
    /// <summary>
    /// Class containing Main GUI Element
    /// </summary>
    public partial class Main : Form
    {
        private delegate void QueueMessage_AddMessage_Safe(object myObject, MessageEventArgs myArgs);
        private delegate void UpdateGUI_Safe(object myObject);

        private bool ApplicationRestart = false;

        private List<System.Threading.Timer> TimerCollection = new List<System.Threading.Timer>();

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

            // Timer Test
            System.Threading.Timer Timer = new System.Threading.Timer(new TimerCallback(UpdatePlayerList), null, 0, 100);
            TimerCollection.Add(Timer);
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

        private void UpdatePlayerList(object obj)
        {
            try
            {
                if (Main_CurrentPlayerOnline.InvokeRequired)
                {
                    BeginInvoke(new UpdateGUI_Safe(UpdatePlayerList), "");
                }
                else
                {
                    List<string> ListOfPlayer = new List<string>();

                    for (int i = 0; i < Core.Player.Count; i++)
                    {
                        ListOfPlayer.Add(Core.Player[i].ToString());
                    }

                    Main_CurrentPlayerOnline.DataSource = null;
                    Main_CurrentPlayerOnline.DataSource = Core.Player.Count > 0 ? ListOfPlayer : null;
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        private void Main_Command_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Alt && !e.Control && !e.Shift && e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(Main_Command.Text))
                {
                    PackageHandler.HandleChatCommand(new Package(Package.PackageTypes.ChatMessage, Main_Command.Text, null));
                }
                Main_Command.Clear();
            }
        }
    }
}
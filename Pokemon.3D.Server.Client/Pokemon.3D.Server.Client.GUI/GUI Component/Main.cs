using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.Event;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Modules;
using Pokemon_3D_Server_Core.Packages;

namespace Pokemon_3D_Server_Client_GUI
{
    /// <summary>
    /// Class containing Main GUI Element
    /// </summary>
    public partial class Main : Form
    {
        private delegate void ClientEvent_Safe(object myObject, ClientEventArgs myArgs);
        private delegate void UpdatePlayerList_Safe(object myObject);

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
            ClientEvent.Update += ClientEvent_Update;

            // Setup
            Core.Setting.ApplicationDirectory = Environment.CurrentDirectory;
            Core.Logger.Setup();
            Core.Setting.Setup();

            // Setup Settings
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
            Core.Setting.NoPingKickTime = 15;
            Core.Server.Start();

            // UpdatePlayerList
            System.Threading.Timer Timer = new System.Threading.Timer(new TimerCallback(UpdatePlayerList), null, 0, 10);
            TimerCollection.Add(Timer);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < Core.Player.Count; i++)
            {
                Core.Server.SentToPlayer(new Package(Package.PackageTypes.ServerClose, ApplicationRestart ? Core.Setting.Token("SERVER_RESTART") : Core.Setting.Token("SERVER_CLOSE"), Core.Player[i].Network.Client),false);
            }
            Core.Server.Stop(true);
            Core.Setting.Save();

            Core.Logger.Add("Main.cs: Application closed successfully!", Logger.LogTypes.Info);

            if (ApplicationRestart)
            {
                Application.Restart();
                Application.ExitThread();
            }
        }

        private void ClientEvent_Update(object myObject, ClientEventArgs myArgs)
        {
            try
            {
                if (Main_Logger.InvokeRequired)
                {
                    BeginInvoke(new ClientEvent_Safe(ClientEvent_Update), myObject, myArgs);
                }
                else
                {
                    if (myArgs.Type == ClientEvent.Types.Logger)
                    {
                        Main_Logger.AppendText(myArgs.Output + Functions.vbNewLine);

                        if (Main_Logger.Lines.Length > 1000)
                        {
                            Main_Logger.Lines = Main_Logger.Lines.Skip(1).ToArray();
                        }
                    }
                    else if (myArgs.Type == ClientEvent.Types.Restart)
                    {
                        ApplicationRestart = true;
                        Application.Exit();
                    }
                    else if (myArgs.Type == ClientEvent.Types.Stop)
                    {
                        ApplicationRestart = false;
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        private void UpdatePlayerList(object obj)
        {
            try
            {
                if (Main_CurrentPlayerOnline.InvokeRequired)
                {
                    BeginInvoke(new UpdatePlayerList_Safe(UpdatePlayerList), "");
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
                if (!string.IsNullOrWhiteSpace(Main_Command.Text.Trim()))
                {
                    PackageHandler.HandleChatCommand(new Package(Package.PackageTypes.ChatMessage, Main_Command.Text, null));
                }
                Main_Command.Clear();
            }
        }
    }
}
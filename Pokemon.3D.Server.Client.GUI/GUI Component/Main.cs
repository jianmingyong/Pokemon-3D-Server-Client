using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;

namespace Pokemon_3D_Server_Client_GUI
{
    /// <summary>
    /// Class containing Main GUI Element
    /// </summary>
    public partial class Main : Form
    {
        private delegate void LoggerEvent_Safe(string Args);
        private delegate void ClientEvent_Safe(ClientEvent.Types Type);
        private delegate void PlayerEvent_Safe(PlayerEvent.Types Type, string Args);

        private bool ApplicationRestart = false;
        private bool ApplicationUpdate = false;

        private List<string> LoggerLog = new List<string>();
        private bool ScrollTextBox = true;

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
            LoggerEvent.Update += LoggerEvent_Update;
            ClientEvent.Update += ClientEvent_Update;
            PlayerEvent.Update += PlayerEvent_Update;
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Core.Start(Environment.CurrentDirectory);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < Core.Player.Count; i++)
            {
                Core.Player.SentToPlayer(new Package(Package.PackageTypes.ServerClose, ApplicationRestart ? Core.Setting.Token("SERVER_RESTART") : ApplicationUpdate ? Core.Setting.Token("SERVER_UPDATE") : Core.Setting.Token("SERVER_CLOSE"), Core.Player[i].Network.Client));
            }

            Core.Setting.Save();
            Core.Dispose();

            if (ApplicationRestart)
            {
                while ((DateTime.Now - Core.Setting.StartTime).TotalSeconds < 30)
                {
                    Thread.Sleep(1000);
                }

                Application.Restart();
                Application.ExitThread();
            }

            if (ApplicationUpdate)
            {
                ApplicationUpdate = false;
                Functions.Run(Core.Setting.ApplicationDirectory + "\\Pokemon.3D.Server.Client.Updater.exe", Core.Setting.ApplicationDirectory.Replace(" ", "%20"), false);
            }
        }

        #region Client Events
        private void LoggerEvent_Update(string Args)
        {
            try
            {
                if (Main_Logger.InvokeRequired)
                {
                    BeginInvoke(new LoggerEvent_Safe(LoggerEvent_Update), Args);
                }
                else
                {
                    if (ScrollTextBox)
                    {
                        if (LoggerLog.Count > 0)
                        {
                            for (int i = 0; i < LoggerLog.Count; i++)
                            {
                                Main_Logger.AppendText(LoggerLog[i] + Environment.NewLine);
                            }
                            LoggerLog.RemoveRange(0, LoggerLog.Count);
                        }

                        Main_Logger.AppendText(Args + Environment.NewLine);

                        if (Main_Logger.Lines.Count() > 1000)
                        {
                            Main_Logger.Text.Remove(0, Main_Logger.Lines[0].Count());
                        }
                    }
                    else
                    {
                        LoggerLog.Add(Args);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        private void ClientEvent_Update(ClientEvent.Types Type)
        {
            try
            {
                if (Main_Logger.InvokeRequired)
                {
                    BeginInvoke(new ClientEvent_Safe(ClientEvent_Update), Type);
                }
                else
                {
                    if (Type == ClientEvent.Types.Restart)
                    {
                        ApplicationRestart = true;
                        Application.Exit();
                    }
                    else if (Type == ClientEvent.Types.Stop)
                    {
                        Application.Exit();
                    }
                    else if (Type == ClientEvent.Types.Update)
                    {
                        ApplicationUpdate = true;
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        private void PlayerEvent_Update(PlayerEvent.Types Type, string Args)
        {
            try
            {
                if (Main_CurrentPlayerOnline.InvokeRequired)
                {
                    BeginInvoke(new PlayerEvent_Safe(PlayerEvent_Update), Type, Args);
                }
                else
                {
                    if (Type == PlayerEvent.Types.Add)
                    {
                        Main_CurrentPlayerOnline.Items.Add(Args.GetSplit(1, ","));
                    }
                    else if (Type == PlayerEvent.Types.Remove)
                    {
                        for (int i = 0; i < Main_CurrentPlayerOnline.Items.Count; i++)
                        {
                            if (Main_CurrentPlayerOnline.Items[i].ToString().Contains("ID: " + Args.GetSplit(0, ",").Toint()))
                            {
                                Main_CurrentPlayerOnline.Items.RemoveAt(i);
                            }
                        }
                    }
                    else if (Type == PlayerEvent.Types.Update)
                    {
                        for (int i = 0; i < Main_CurrentPlayerOnline.Items.Count; i++)
                        {
                            if (Main_CurrentPlayerOnline.Items[i].ToString().Contains("ID: " + Args.GetSplit(0, ",").Toint()))
                            {
                                Main_CurrentPlayerOnline.Items[i] = Args.GetSplit(1, ",");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }
        #endregion Client Events

        #region Unhandled Exception Catcher
        private void Application_ThreadException(object sender, ThreadExceptionEventArgs ex)
        {
            ex.Exception.CatchError();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            ((Exception)ex.ExceptionObject).CatchError();
        }
        #endregion Unhandled Exception Catcher

        #region Logger Events
        private void Main_Logger_TextChanged(object sender, EventArgs e)
        {
            if (ScrollTextBox)
            {
                Main_Logger.SelectionStart = Main_Logger.TextLength;
                Main_Logger.ScrollToCaret();
            }
        }

        private void Main_Logger_Leave(object sender, EventArgs e)
        {
            ScrollTextBox = true;

            Main_Logger.SelectionStart = Main_Logger.TextLength;
            Main_Logger.ScrollToCaret();
        }

        private void Main_Logger_Enter(object sender, EventArgs e)
        {
            ScrollTextBox = false;
        }
        #endregion Logger Events

        private void Main_Command_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Alt && !e.Control && !e.Shift && e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(Main_Command.Text.Trim()))
                {
                    Core.Command.HandleAllCommand(new Package(Package.PackageTypes.ChatMessage, Main_Command.Text, null));
                }
                Main_Command.Clear();
            }
        }

        private void About_Button_Click(object sender, EventArgs e)
        {
            About About = new About();
            About.Show();
        }

        private void applicationSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationSettings ApplicationSettings = new ApplicationSettings();
            ApplicationSettings.Show();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Downloader;
using Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Servers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings;

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

            if (Core.Setting.MainEntryPoint != Setting.MainEntryPointType.Rcon)
            {
                toolStripSeparator6.Visible = false;
                RCON_Main_Button.Visible = false;
                toolStripSeparator1.Visible = false;
                toolStripLabel1.Visible = false;
                RCON_IPAddress.Visible = false;
                toolStripSeparator2.Visible = false;
                toolStripLabel2.Visible = false;
                RCON_Port.Visible = false;
                toolStripSeparator3.Visible = false;
                toolStripLabel3.Visible = false;
                RCON_Password.Visible = false;
                toolStripSeparator4.Visible = false;
                RCON_Connect.Visible = false;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Core.Setting.MainEntryPoint == Setting.MainEntryPointType.jianmingyong_Server)
            {
                for (int i = 0; i < Core.Player.Count; i++)
                {
                    Core.Player.SentToPlayer(new Package(Package.PackageTypes.ServerClose, ApplicationRestart ? Core.Setting.Token("SERVER_RESTART") : ApplicationUpdate ? Core.Setting.Token("SERVER_UPDATE") : Core.Setting.Token("SERVER_CLOSE"), Core.Player[i].Network.Client));
                }
            }
            else if (Core.Setting.MainEntryPoint == Setting.MainEntryPointType.Rcon)
            {
                if (Core.RCONGUIListener != null)
                {
                    Core.RCONGUIListener.Dispose();
                }
            }

            Core.Setting.Save();
            Core.Dispose();

            if (ApplicationRestart)
            {
                Application.Restart();
                Application.ExitThread();
            }
            else if (ApplicationUpdate)
            {
                ApplicationUpdate = false;
                Functions.Run(Core.Setting.ApplicationDirectory + "\\Pokemon.3D.Server.Client.Updater.exe", Core.Setting.ApplicationDirectory.Replace(" ", "%20"), false);
            }
        }

        private void Main_Command_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Alt && !e.Control && !e.Shift && e.KeyCode == Keys.Enter)
            {
                if (Core.Setting.MainEntryPoint == Setting.MainEntryPointType.jianmingyong_Server)
                {
                    if (!string.IsNullOrWhiteSpace(Main_Command.Text.Trim()) && Main_Command.Text.StartsWith("/"))
                    {
                        Core.Command.HandleAllCommand(new Package(Package.PackageTypes.ChatMessage, Main_Command.Text, null));
                    }
                    else
                    {
                        Core.Command.HandleAllCommand(new Package(Package.PackageTypes.ChatMessage, "/say " + Main_Command.Text, null));
                    }
                }
                else if (Core.Setting.MainEntryPoint == Setting.MainEntryPointType.Rcon)
                {
                    if (Core.RCONGUIListener != null && !string.IsNullOrWhiteSpace(Main_Command.Text.Trim()) && Main_Command.Text.StartsWith("/"))
                    {
                        Core.RCONGUIListener.SentToServer(new Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages.Package(Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages.Package.PackageTypes.Logger, Main_Command.Text, null));
                    }
                    else if (Core.RCONGUIListener != null && !string.IsNullOrWhiteSpace(Main_Command.Text.Trim()))
                    {
                        Core.RCONGUIListener.SentToServer(new Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages.Package(Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages.Package.PackageTypes.Logger, "/say " + Main_Command.Text, null));
                    }
                }

                Main_Command.Clear();
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
                        if (!Main_CurrentPlayerOnline.Items.Contains("ID: " + Args.GetSplit(0, ",")))
                            Main_CurrentPlayerOnline.Items.Add(Args.GetSplit(1, ","));
                    }
                    else if (Type == PlayerEvent.Types.Remove)
                    {
                        for (int i = 0; i < Main_CurrentPlayerOnline.Items.Count; i++)
                        {
                            if (Main_CurrentPlayerOnline.Items[i].ToString().Contains("ID: " + Args.GetSplit(0, ",")))
                                Main_CurrentPlayerOnline.Items.RemoveAt(i);
                        }
                    }
                    else if (Type == PlayerEvent.Types.Update)
                    {
                        for (int i = 0; i < Main_CurrentPlayerOnline.Items.Count; i++)
                        {
                            if (Main_CurrentPlayerOnline.Items[i].ToString().Contains("ID: " + Args.GetSplit(0, ",")))
                                Main_CurrentPlayerOnline.Items[i] = Args.GetSplit(1, ",");
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

            if (LoggerLog.Count > 0)
            {
                for (int i = 0; i < LoggerLog.Count; i++)
                {
                    Main_Logger.AppendText(LoggerLog[i] + Environment.NewLine);
                }
                LoggerLog.RemoveRange(0, LoggerLog.Count);
            }

            Main_Logger.SelectionStart = Main_Logger.TextLength;
            Main_Logger.ScrollToCaret();
        }

        private void Main_Logger_Enter(object sender, EventArgs e)
        {
            ScrollTextBox = false;
        }
        #endregion Logger Events

        #region Menu Bar Buttons
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

        private void RCON_Connect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(RCON_IPAddress.Text) || string.IsNullOrEmpty(RCON_Password.Text) || string.IsNullOrEmpty(RCON_Port.Text))
            {
                Core.Logger.Log("Please make sure that the IP Address, Password and Port is not blank.", Logger.LogTypes.Info);
            }
            else
            {
                if (Core.RCONGUIListener != null)
                {
                    Core.RCONGUIListener.Dispose();
                    Core.RCONGUIListener = new Listener(RCON_IPAddress.Text, RCON_Password.Text, RCON_Port.Text.ToInt());
                }
                else
                {
                    Core.RCONGUIListener = new Listener(RCON_IPAddress.Text, RCON_Password.Text, RCON_Port.Text.ToInt());
                }
            }
        }

        // Get All Logs
        private void getLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Downloader Downloader = new Downloader();
            Downloader.Show();

            Core.RCONGUIDownloadQueue.DownloadType = DownloadFile.FileType.Logger;
            Core.RCONGUIListener.SentToServer(new Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages.Package(Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages.Package.PackageTypes.GetAllLogs, "", null));
        }

        private void getCrashLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Downloader Downloader = new Downloader();
            Downloader.Show();

            Core.RCONGUIDownloadQueue.DownloadType = DownloadFile.FileType.CrashLog;
            Core.RCONGUIListener.SentToServer(new Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages.Package(Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages.Package.PackageTypes.GetAllCrashLogs, "", null));
        }
        #endregion Menu Bar Buttons

        #region Context Menu for Logger
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main_Logger.Copy();
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main_Logger.SelectAll();
            Main_Logger.Copy();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main_Logger.SelectAll();
        }
        #endregion Context Menu for Logger

        #region Context Menu for Player List
        private void Main_CurrentPlayerOnlineRC_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Main_CurrentPlayerOnline.Items.Count == 0)
            {
                e.Cancel = true;
            }
        }

        private void Main_CurrentPlayerOnline_MouseDown(object sender, MouseEventArgs e)
        {
            if (Main_CurrentPlayerOnline.IndexFromPoint(e.X,e.Y) < 0)
            {
                Main_CurrentPlayerOnline.SelectedIndex = Main_CurrentPlayerOnline.Items.Count - 1;
            }
            else
            {
                Main_CurrentPlayerOnline.SelectedIndex = Main_CurrentPlayerOnline.IndexFromPoint(e.X, e.Y);
            }
        }

        private void KickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Player Player = Core.Player.GetPlayer(Regex.Match(Main_CurrentPlayerOnline.Items[Main_CurrentPlayerOnline.SelectedIndex].ToString(), @"ID: (\d+) \|.+").Groups[1].Value.ToInt());

            Main_Command.Text = $"/kick {Player.Name} <Insert Reason here>";
        }
        #endregion Context Menu for Player List
    }
}
using System;
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
        private delegate void ClientEvent_Safe(ClientEvent.Types Type, object Args);
        private delegate void UpdatePlayerList_Safe(object myObject);

        private bool ApplicationRestart = false;
        private bool ApplicationUpdate = false;

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
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

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
                Application.Restart();
                Application.ExitThread();
            }

            if (ApplicationUpdate)
            {
                
            }
        }

        private void ClientEvent_Update(ClientEvent.Types Type, object Args)
        {
            try
            {
                if (Main_Logger.InvokeRequired)
                {
                    BeginInvoke(new ClientEvent_Safe(ClientEvent_Update), Type, Args);
                }
                else
                {
                    if (Type == ClientEvent.Types.Logger)
                    {
                        Main_Logger.AppendText(Args + Functions.vbNewLine);

                        if (Main_Logger.Lines.Length > 1000)
                        {
                            Main_Logger.Lines = Main_Logger.Lines.Skip(1).ToArray();
                        }
                    }
                    else if (Type == ClientEvent.Types.Restart)
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

        private void Application_ThreadException(object sender, ThreadExceptionEventArgs ex)
        {
            ex.Exception.CatchError();
            ClientEvent.Invoke(ClientEvent.Types.Restart, null);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            (ex.ExceptionObject as Exception).CatchError();
            ClientEvent.Invoke(ClientEvent.Types.Restart, null);
        }

        private void Main_Command_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Alt && !e.Control && !e.Shift && e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(Main_Command.Text.Trim()))
                {
                    // Core.Package.HandleChatCommand(new Package(Package.PackageTypes.ChatMessage, Main_Command.Text, null));
                }
                Main_Command.Clear();
            }
        }

        private void About_Button_Click(object sender, EventArgs e)
        {
            About About = new About();
            About.Show();
        }
    }
}
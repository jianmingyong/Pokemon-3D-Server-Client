using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Settings;
using Pokemon_3D_Server_Core.Modules;

namespace Pokemon_3D_Server_Client_GUI
{
    /// <summary>
    /// Class containing Main GUI Element
    /// </summary>
    public partial class Main : Form
    {
        /// <summary>
        /// Delegate Event for Queue Message.
        /// </summary>
        public delegate void QueueMessage_AddMessage_Safe(object myObject, MessageEventArgs myArgs);

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
    }
}

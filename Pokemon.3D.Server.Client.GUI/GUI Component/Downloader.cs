using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Pokemon_3D_Server_Core;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Downloader;

namespace Pokemon_3D_Server_Client_GUI
{
    /// <summary>
    /// Class containing Downloader Stuff
    /// </summary>
    public partial class Downloader : Form
    {
        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();
        private delegate void Update_Safe(List<DownloadFile> Download);

        /// <summary>
        /// New Downloader
        /// </summary>
        public Downloader()
        {
            InitializeComponent();

            Thread Thread = new Thread(new ThreadStart(UpdateGUI)) { IsBackground = true };
            Thread.Start();
            ThreadCollection.Add(Thread);
        }

        private void UpdateGUI()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                try
                {
                    Update(Core.RCONGUIDownloadQueue);
                }
                catch (Exception) { }

                sw.Stop();
                if (sw.ElapsedMilliseconds < 1000)
                {
                    Thread.Sleep(1000 - sw.ElapsedMilliseconds.ToString().ToInt());
                }
                sw.Restart();
            } while (true);
        }

        private void Update(List<DownloadFile> Download)
        {
            if (objectListView1.InvokeRequired)
            {
                BeginInvoke(new Update_Safe(Update), Download);
            }
            else
            {
                objectListView1.UpdateObjects(Download);
            }
        }

        private void Downloader_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < Core.RCONGUIDownloadQueue.Count; i++)
            {
                Core.RCONGUIDownloadQueue.Dispose();
            }
        }
    }
}

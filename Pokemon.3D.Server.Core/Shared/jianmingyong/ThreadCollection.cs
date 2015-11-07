using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Amib.Threading;

namespace Pokemon_3D_Server_Core.Shared.jianmingyong
{
    /// <summary>
    /// Class containing Thread
    /// </summary>
    public class ThreadCollection : List<Thread>, IDisposable
    {
        /// <summary>
        /// Add a new thread into the collection.
        /// </summary>
        /// <param name="Thread">Thread to add.</param>
        public void Add(ThreadStart ThreadStart)
        {
            Thread Thread = new Thread(ThreadStart) { IsBackground = true };
            Thread.Start();
            this.Add(Thread);
        }

        private void RemoveAll()
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].IsAlive)
                {
                    this[i].Abort();
                }
            }
        }

        /// <summary>
        /// Dispose all running threads.
        /// </summary>
        public void Dispose()
        {
            this.RemoveAll();
        }
    }
}

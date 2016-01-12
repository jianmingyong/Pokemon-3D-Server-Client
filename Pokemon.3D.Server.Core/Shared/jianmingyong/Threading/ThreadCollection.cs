using System;
using System.Collections.Generic;
using System.Threading;

namespace Pokemon_3D_Server_Core.Shared.jianmingyong.Threading
{
    /// <summary>
    /// Class containing Thread
    /// </summary>
    public class ThreadCollection : List<Thread>, IDisposable
    {
        /// <summary>
        /// Add a new thread into the collection.
        /// </summary>
        /// <param name="ThreadStart">Thread to add.</param>
        public void Add(ThreadStart ThreadStart)
        {
            Thread Thread = new Thread(ThreadStart) { IsBackground = true };
            Thread.Start();
            Add(Thread);
        }

        /// <summary>
        /// Dispose all running threads.
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].IsAlive)
                {
                    this[i].Abort();
                }
            }
        }
    }
}

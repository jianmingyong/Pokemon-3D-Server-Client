using System;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Rcon.Players
{
    /// <summary>
    /// Class containing Rcon Player Data
    /// </summary>
    public class Player : IDisposable
    {
        /// <summary>
        /// Get/Set Rcon Player ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Get/Set Rcon Neteork.
        /// </summary>
        public Networking Network { get; set; }

        /// <summary>
        /// New Rcon Player
        /// </summary>
        /// <param name="p">Package Data</param>
        /// <param name="ID">ID.</param>
        public Player(Package p, int ID)
        {
            this.ID = ID;
            Network = new Networking(p.Client);

            Core.Player.SentToPlayer(new Package(Package.PackageTypes.ID, ID.ToString(), p.Client));
        }

        /// <summary>
        /// Dispose Rcon Player.
        /// </summary>
        public void Dispose()
        {
            Network.Dispose();
        }
    }
}

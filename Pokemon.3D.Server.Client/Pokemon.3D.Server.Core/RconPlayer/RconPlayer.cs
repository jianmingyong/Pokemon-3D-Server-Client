using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Network;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;

namespace Pokemon_3D_Server_Core.Rcon.Players
{
    /// <summary>
    /// Class containing Rcon Player Data
    /// </summary>
    public class RconPlayer : IDisposable
    {
        /// <summary>
        /// Get/Set Rcon Player ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Get/Set List of Players data.
        /// </summary>
        public List<Player> Player { get; set; } = new List<Player>();

        /// <summary>
        /// Get/Set Rcon Neteork.
        /// </summary>
        public RconNetworking Network { get; set; }

        /// <summary>
        /// New Rcon Player
        /// </summary>
        /// <param name="p">Package Data</param>
        /// <param name="ID">ID.</param>
        public RconPlayer(Package p, int ID)
        {
            this.ID = ID;
            Network = new RconNetworking(p.Client);

            Core.Server.SentToPlayer(new Package(Package.PackageTypes.ID, ID.ToString(), p.Client));
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

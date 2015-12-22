using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.RCON_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data;

namespace Pokemon_3D_Server_Core.RCON_Client_Listener.Players
{
    /// <summary>
    /// Class containing Player infomation
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Get/Set Player ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Get/Set Player Client
        /// </summary>
        public Networking Network { get; set; }

        /// <summary>
        /// New Player (Update Player)
        /// </summary>
        /// <param name="p">Package</param>
        /// <param name="ID">Player ID</param>
        public Player(Package p, int ID)
        {
            this.ID = ID;
            Network = new Networking(p.Client);

            Core.RCONPlayer.Add(this);
            Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.Authentication, Package.AuthenticationStatus.AccessGranted.ToString(), p.Client));

            Core.Logger.Log($"RCON Player (ID: {ID.ToString()}) have connected.", Logger.LogTypes.Info, p.Client);

            for (int i = 0; i < Core.Player.Count; i++)
            {
                Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.AddPlayer, $"{Core.Player[i].ID},{Core.Player[i].ToString()}", p.Client));
            }
        }
    }
}
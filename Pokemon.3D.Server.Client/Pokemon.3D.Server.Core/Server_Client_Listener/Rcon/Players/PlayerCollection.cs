using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Rcon.Players
{
    /// <summary>
    /// Class containing a collection of Rcon players.
    /// </summary>
    public class PlayerCollection : List<Player>
    {
        private static readonly object Lock = new object();

        /// <summary>
        /// Add New Player to the collection.
        /// </summary>
        /// <param name="p">Package Data</param>
        public void Add(Package p)
        {
            if (p.IsValid && p.IsFullPackageData())
            {
                int ID = GetNextValidID();
                Player Player = new Player(p, ID);
            }
        }

        /// <summary>
        /// Remove Player by ID
        /// </summary>
        /// <param name="ID">Player ID</param>
        /// <param name="Reason">Reason</param>
        public void Remove(int ID, string Reason)
        {
            Player Player = GetPlayer(ID);
            Player.Network.IsActive = false;

            Player.Network.SentToPlayer(new Package(Package.PackageTypes.RCON_KICKED, Reason, Player.Network.Client));

            Player.Dispose();
            Core.RconPlayer.Remove(Player);
        }

        /// <summary>
        /// Remove Player by Client
        /// </summary>
        /// <param name="Client">Client</param>
        /// <param name="Reason">Reason</param>
        public void Remove(TcpClient Client, string Reason)
        {
            Player Player = GetPlayer(Client);
            Player.Network.IsActive = false;

            Player.Network.SentToPlayer(new Package(Package.PackageTypes.RCON_KICKED, Reason, Player.Network.Client));

            Player.Dispose();
            Core.RconPlayer.Remove(Player);
        }

        /// <summary>
        /// Check if the player is in the collection.
        /// </summary>
        /// <param name="ID">ID of the player.</param>
        public bool HasPlayer(int ID)
        {
            if ((from Player p in Core.RconPlayer where p.ID == ID select p).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the player is in the collection.
        /// </summary>
        /// <param name="Client">TcpClient of the player.</param>
        public bool HasPlayer(TcpClient Client)
        {
            if ((from Player p in Core.Player where p.Network.Client == Client select p).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the player in the collection.
        /// </summary>
        /// <param name="ID">ID of the player.</param>
        public Player GetPlayer(int ID)
        {
            return (from Player p in Core.RconPlayer where p.ID == ID select p).FirstOrDefault();
        }

        /// <summary>
        /// Get the player in the collection.
        /// </summary>
        /// <param name="Client">TcpClient of the player.</param>
        public Player GetPlayer(TcpClient Client)
        {
            return (from Player p in Core.RconPlayer where p.Network.Client == Client select p).FirstOrDefault();
        }

        private int GetNextValidID()
        {
            if (Count == 0)
            {
                return 0;
            }
            else
            {
                int ValidID = 0;
                List<Player> ListOfPlayer = (from Player p in Core.RconPlayer orderby p.ID ascending select p).ToList();

                for (int i = 0; i < ListOfPlayer.Count; i++)
                {
                    if (ValidID == ListOfPlayer[i].ID)
                    {
                        ValidID++;
                    }
                    else
                    {
                        return ValidID;
                    }
                }
                return ValidID;
            }
        }

        /// <summary>
        /// Sent Package Data to Player
        /// </summary>
        /// <param name="p">Package</param>
        public void SentToPlayer(Package p)
        {

            if (Core.RconPlayer.HasPlayer(p.Client))
            {
                GetPlayer(p.Client).Network.SentToPlayer(p);
            }
            else
            {
                lock (Lock)
                {
                    try
                    {
                        StreamWriter Writer = new StreamWriter(p.Client.GetStream());
                        Writer.WriteLine(p.ToString());
                        Writer.Flush();
                        Core.Logger.Log("Sent: " + p.ToString(), Logger.LogTypes.Debug, p.Client);
                    }
                    catch (Exception)
                    {
                        Core.Logger.Log("StreamWriter failed to send package data.", Logger.LogTypes.Debug, p.Client);
                    }
                }
            }
        }

        /// <summary>
        /// Sent Package Data to All Player
        /// </summary>
        /// <param name="p">Package</param>
        public void SendToAllPlayer(Package p)
        {
            for (int i = 0; i < Count; i++)
            {
                if (p.Client == null || Core.RconPlayer[i].Network.Client != p.Client)
                {
                    Core.RconPlayer[i].Network.SentToPlayer(p);
                }
            }
        }
    }
}

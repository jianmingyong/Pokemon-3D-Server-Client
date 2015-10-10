using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Pokemon_3D_Server_Core.Packages;

namespace Pokemon_3D_Server_Core.Rcon.Players
{
    /// <summary>
    /// Class containing a collection of Rcon players.
    /// </summary>
    public class RconPlayerCollection : List<RconPlayer>
    {
        /// <summary>
        /// Add New Rcon Player to the collection.
        /// </summary>
        /// <param name="p">Package Data</param>
        public void Add(Package p)
        {
            if (p.IsValid)
            {
                int ID = GetNextValidID();
                RconPlayer Player = new RconPlayer(p, ID);
            }
        }

        /// <summary>
        /// Remove Rcon Player by ID
        /// </summary>
        /// <param name="ID">Rcon Player ID</param>
        /// <param name="Reason">Reason</param>
        public void Remove(int ID, string Reason)
        {
            RconPlayer Player = GetPlayer(ID);
            Player.Network.IsActive = false;

            Player.Dispose();
            Core.RconPlayer.Remove(Player);
        }

        /// <summary>
        /// Get the rcon player in the collection.
        /// </summary>
        /// <param name="ID">ID of the rcon player.</param>
        public RconPlayer GetPlayer(int ID)
        {
            return (from RconPlayer p in this where p.ID == ID select p).FirstOrDefault();
        }

        /// <summary>
        /// Check if the rcon player is in the collection.
        /// </summary>
        /// <param name="Client">TcpClient of the rcon player.</param>
        public bool HasPlayer(TcpClient Client)
        {
            if ((from RconPlayer p in this where p.Network.Client == Client select p).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the rcon player in the collection.
        /// </summary>
        /// <param name="Client">TcpClient of the rcon player.</param>
        public RconPlayer GetPlayer(TcpClient Client)
        {
            return (from RconPlayer p in this where p.Network.Client == Client select p).FirstOrDefault();
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
                List<RconPlayer> ListOfPlayer = (from RconPlayer p in this orderby p.ID ascending select p).ToList();

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
    }
}

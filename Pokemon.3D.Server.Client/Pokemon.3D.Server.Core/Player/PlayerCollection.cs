using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Pokemon_3D_Server_Core.Players
{
    /// <summary>
    /// Class containing a collection of players.
    /// </summary>
    public class PlayerCollection : List<Player>
    {
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
                if (Player.isGameJoltPlayer)
                {
                    Settings.OnlineSettingListData.Add(new OnlineSetting(Player.Name, Player.GameJoltID));
                }
                // Update Player List - WIP
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
            
            if (Player.isGameJoltPlayer)
            {
                ServerClient.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Settings.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "left the server."), null));
                QueueMessage.Add(Settings.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "left the server with the following reason: " + Reason), MessageEventArgs.LogType.Info);

                OnlineSetting OnlineSetting = (from OnlineSetting p in Settings.OnlineSettingListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault();
                OnlineSetting.Save();
                Settings.OnlineSettingListData.Remove(OnlineSetting);
            }
            else
            {
                ServerClient.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Settings.Token("SERVER_NOGAMEJOLT", Player.Name, "left the server."), null));
                QueueMessage.Add(Settings.Token("SERVER_NOGAMEJOLT", Player.Name, "left the server with the following reason: " + Reason), MessageEventArgs.LogType.Info);
            }

            ServerClient.SendToAllPlayer(new Package(Package.PackageTypes.DestroyPlayer, Player.ID.ToString(), null));

            if (Reason != Settings.Token("SERVER_PLAYERLEFT"))
            {
                ServerClient.SentToPlayer(new Package(Package.PackageTypes.Kicked, Reason, Player.Client.Client));
            }

            // Update Player List - WIP
        }

        /// <summary>
        /// Check if the player is in the collection.
        /// </summary>
        /// <param name="ID">ID of the player.</param>
        public bool HasPlayer(int ID)
        {
            if ((from Player p in this where p.ID == ID select p).Count() > 0)
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
        /// <param name="Name">Name of the player.</param>
        public bool HasPlayer(string Name)
        {
            if ((from Player p in this where p.Name == Name select p).Count() > 0)
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
            if ((from Player p in this where p.Client.Client == Client select p).Count() > 0)
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
            return (from Player p in this where p.ID == ID select p).FirstOrDefault();
        }

        /// <summary>
        /// Get the player in the collection.
        /// </summary>
        /// <param name="Name">Name of the player.</param>
        public Player GetPlayer(string Name)
        {
            return (from Player p in this where p.Name == Name select p).FirstOrDefault();
        }

        /// <summary>
        /// Get the player in the collection.
        /// </summary>
        /// <param name="Client">TcpClient of the player.</param>
        public Player GetPlayer(TcpClient Client)
        {
            return (from Player p in this where p.Client.Client == Client select p).FirstOrDefault();
        }

        /// <summary>
        /// Get the next valid id that is not in use.
        /// </summary>
        public int GetNextValidID()
        {
            if (Count == 0)
            {
                return 0;
            }
            else
            {
                int ValidID = 0;
                List<Player> ListOfPlayer = (from Player p in this orderby p.ID ascending select p).ToList();

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

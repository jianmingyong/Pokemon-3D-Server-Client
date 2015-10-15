using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Packages
{
    /// <summary>
    /// Class containing Package Data Handler.
    /// </summary>
    public class Package
    {
        /* Package data:
        ProtcolVersion|PackageType|Origin|DataItemsCount|Offset1|Offset2|Offset3...|Data1Data2Data3
        The package contains:
            - Its protocol version.
            - The PackageType, defining the type of the package
            - The Origin, indicating which computer sent this package.
            - The DataItemsCount tells the package how many data items it contains.
            - A list of offsets that separate the data.
            - A list of data items, that aren't separated.
        */

        /// <summary>
        /// Get Protocol Version.
        /// </summary>
        public string ProtocolVersion { get; } = Core.Setting.ProtocolVersion;

        /// <summary>
        /// Get/Set Package Type.
        /// </summary>
        public int PackageType { get; set; } = (int)PackageTypes.Unknown;

        /// <summary>
        /// Get/Set Origin.
        /// </summary>
        public int Origin { get; set; } = -1;

        /// <summary>
        /// Get DataItems Count.
        /// </summary>
        public int DataItemsCount { get { return DataItems.Count; } }

        /// <summary>
        /// Get/Set DataItems.
        /// </summary>
        public List<string> DataItems { get; set; } = new List<string>();

        /// <summary>
        /// Get/Set Is Valid?
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Get/Set Client.
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// A collection of Package Type.
        /// </summary>
        public enum PackageTypes
        {
            /// <summary>
            /// Package Type: RCON Kick
            /// <para>Set: {Origin = -1 | DataItem[0] = Reason}</para>
            /// </summary>
            RCON_KICKED = -6,

            /// <summary>
            /// Package Type: RCON Command
            /// <para>Get: {Origin = ID | DataItem[0] = Command}</para>
            /// </summary>
            RCON_Command = -5,

            /// <summary>
            /// Package Type: RCON Ping
            /// <para>Get: {Origin = ID | DataItem[0] = null}</para>
            /// </summary>
            RCON_Ping = -4,

            /// <summary>
            /// Package Type: RCON Identification
            /// <para>Set: {Origin = -1 | DataItem[0] = ID}</para>
            /// </summary>
            RCON_ID = -3,

            /// <summary>
            /// Package Type: RCON Login Token
            /// <para>Get: {Origin = -1 | DataItem[0] = Md5 Hash Password | DataItem[1] = Sha1 Hash Password | DataItem[2] = Sha256 Hash Password}</para>
            /// <para>Set: {Origin = -1 | DataItem[0] = Authentication Fail or Success.}</para>
            /// </summary>
            RCON_Authentication = -2,

            /// <summary>
            /// Package Type: Unknown Data
            /// </summary>
            Unknown = -1,

            /// <summary>
            /// Package Type: Game Data
            /// <para>Join: {Origin = PlayerID | DataItem[] = FullPackageData[] | To other players}</para>  
            /// <para>Update: {Origin = PlayerID | DataItem[] = PartialPackageData[] | To other players}</para>
            /// </summary>
            GameData = 0,

            /// <summary>
            /// Private Message
            /// <para>Global: {Origin = -1 | DataItem[0] = Message | To the player}</para>
            /// <para>Own: {Origin = PlayerID | DataItem[0] = PlayerID, DataItem[1] = Message | To yourself}</para>
            /// <para>Client: {Origin = PlayerID | DataItem[0] = Message | To client}</para>
            /// </summary>
            PrivateMessage = 2,

            /// <summary>
            /// Chat Message
            /// <para>Global: {Origin = -1 | DataItem[0] = Message | To all players}</para>
            /// <para>Player: {Origin = PlayerID | DataItem[0] = Message | To all players}</para>
            /// </summary>
            ChatMessage = 3,

            /// <summary>
            /// Kick
            /// <para>{Origin = -1 | DataItem[0] = Reason | To player}</para>
            /// </summary>
            Kicked = 4,

            /// <summary>
            /// ID
            /// <para>{Origin = -1 | DataItem[0] = PlayerID | To own}</para>
            /// </summary>
            ID = 7,

            /// <summary>
            /// Create Player
            /// <para>{Origin = -1 | DataItem[0] = PlayerID | To other players}</para>
            /// </summary>
            CreatePlayer = 8,

            /// <summary>
            /// Destroy Player
            /// <para>{Origin = -1 | DataItem[0] = PlayerID | To other players}</para>
            /// </summary>
            DestroyPlayer = 9,

            /// <summary>
            /// Server Close
            /// <para>{Origin = -1 | DataItem[0] = Reason | To all players}</para>
            /// </summary>
            ServerClose = 10,

            /// <summary>
            /// Server Message
            /// <para>{Origin = -1 | DataItem[0] = Message | To all players}</para>
            /// </summary>
            ServerMessage = 11,

            /// <summary>
            /// World Data
            /// <para>{Origin = -1 | DataItem[0] = Season, DataItem[1] = Weather, DataItem[2] = Time | To all players}</para>
            /// </summary>
            WorldData = 12,

            /// <summary>
            /// Ping (Get Only)
            /// </summary>
            Ping = 13,

            /// <summary>
            /// Gamestate Message (Get Only)
            /// </summary>
            GamestateMessage = 14,

            /// <summary>
            /// Trade Request
            /// <para>{Origin = PlayerID | DataItem = null | To trade player}</para>
            /// </summary>
            TradeRequest = 30,

            /// <summary>
            /// Trade Join
            /// <para>{Origin = PlayerID | DataItem = null | To trade player}</para>
            /// </summary>
            TradeJoin = 31,

            /// <summary>
            /// Trade Quit
            /// <para>{Origin = PlayerID | DataItem = null | To trade player}</para>
            /// </summary>
            TradeQuit = 32,

            /// <summary>
            /// Trade Offer
            /// <para>{Origin = PlayerID | DataItem[0] = PokemonData | To trade player}</para>
            /// </summary>
            TradeOffer = 33,

            /// <summary>
            /// Trade Start
            /// <para>{Origin = PlayerID | DataItem = null | To trade player}</para>
            /// </summary>
            TradeStart = 34,

            /// <summary>
            /// Battle Request
            /// <para>{Origin = PlayerID | DataItem = null | To battle player}</para>
            /// </summary>
            BattleRequest = 50,

            /// <summary>
            /// Battle Join
            /// <para>{Origin = PlayerID | DataItem = null | To battle player}</para>
            /// </summary>
            BattleJoin = 51,

            /// <summary>
            /// Battle Quit
            /// <para>{Origin = PlayerID | DataItem = null | To battle player}</para>
            /// </summary>
            BattleQuit = 52,

            /// <summary>
            /// Battle Offer
            /// <para>{Origin = PlayerID | DataItem[0] = PokemonData | To battle player}</para>
            /// </summary>
            BattleOffer = 53,

            /// <summary>
            /// Battle Start
            /// <para>{Origin = PlayerID | DataItem = null | To battle player}</para>
            /// </summary>
            BattleStart = 54,

            /// <summary>
            /// Battle Client Data
            /// <para>{Origin = PlayerID | DataItem[0] = ClientData | To battle player}</para>
            /// </summary>
            BattleClientData = 55,

            /// <summary>
            /// Battle Host Data
            /// <para>{Origin = PlayerID | DataItem[0] = HostData | To battle player}</para>
            /// </summary>
            BattleHostData = 56,

            /// <summary>
            /// Battle Pokemon Data
            /// <para>{Origin = PlayerID | DataItem[0] = PokemonData | To battle player}</para>
            /// </summary>
            BattlePokemonData = 57,

            /// <summary>
            /// Server Info Data
            /// <para>{Origin = -1 | DataItem[] = Server Info | To listening client}</para>
            /// </summary>
            ServerInfoData = 98,

            /// <summary>
            /// Server Data Request (Read only)
            /// </summary>
            ServerDataRequest = 99,
        }

        /// <summary>
        /// Full Package Data
        /// </summary>
        /// <param name="FullData">Full Package Data</param>
        /// <param name="Client">TcpClient of the player</param>
        public Package(string FullData, TcpClient Client)
        {
            try
            {
                this.Client = Client;

                if (!FullData.Contains("|"))
                {
                    Core.Logger.Log("Package.cs: Package is incomplete.", Logger.LogTypes.Debug, Client);
                    IsValid = false;
                    return;
                }

                List<string> bits = FullData.Split("|".ToCharArray()).ToList();

                if (bits.Count >= 5)
                {
                    // Protocol Version
                    if (!string.Equals(Core.Setting.ProtocolVersion, bits[0], StringComparison.OrdinalIgnoreCase))
                    {
                        Core.Logger.Log("Package.cs: Package does not contains valid Protocol Version.", Logger.LogTypes.Debug, Client);
                        IsValid = false;
                        return;
                    }

                    // Package Type
                    try
                    {
                        PackageType = int.Parse(bits[1]);
                    }
                    catch (Exception)
                    {
                        Core.Logger.Log("Package.cs: Package does not contains valid Package Type.", Logger.LogTypes.Debug, Client);
                        IsValid = false;
                        return;
                    }

                    // Origin
                    try
                    {
                        Origin = int.Parse(bits[2]);
                    }
                    catch (Exception)
                    {
                        Core.Logger.Log("Package.cs: Package does not contains valid Origin.", Logger.LogTypes.Debug, Client);
                        IsValid = false;
                        return;
                    }

                    // DataItemsCount
                    int DataItemsCount = 0;
                    try
                    {
                        DataItemsCount = bits[3].Toint();
                    }
                    catch (Exception)
                    {
                        Core.Logger.Log("Package.cs: Package does not contains valid DataItemsCount.", Logger.LogTypes.Debug, Client);
                        IsValid = false;
                        return;
                    }

                    List<int> OffsetList = new List<int>();

                    // Count from 4th item to second last item. Those are the offsets.
                    for (int i = 4; i < DataItemsCount + 4; i++)
                    {
                        try
                        {
                            OffsetList.Add(bits[i].Toint());
                        }
                        catch (Exception)
                        {
                            Core.Logger.Log("Package.cs: Package does not contains valid Offset.", Logger.LogTypes.Debug, Client);
                            IsValid = false;
                            return;
                        }
                    }

                    // Set the datastring, its the last item in the list. If it contained any separators, they will get readded here:
                    string dataString = null;
                    for (int i = DataItemsCount + 4; i < bits.Count; i++)
                    {
                        if (i > DataItemsCount + 4)
                        {
                            dataString += "|";
                        }
                        dataString += bits[i];
                    }

                    // Cutting the data:
                    for (int i = 0; i < OffsetList.Count; i++)
                    {
                        int cOffset = OffsetList[i];
                        int length = dataString.Length - cOffset;
                        if (i < OffsetList.Count - 1)
                        {
                            length = OffsetList[i + 1] - cOffset;
                        }
                        DataItems.Add(dataString.Substring(cOffset, length));
                    }
                    IsValid = true;
                }
                else
                {
                    Core.Logger.Log("Package.cs: Package is incomplete.", Logger.LogTypes.Debug, Client);
                    IsValid = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                Core.Logger.Log("Package.cs: " + ex.Message, Logger.LogTypes.Debug, Client);
                IsValid = false;
                return;
            }
        }

        /// <summary>
        /// Creating a new Package
        /// </summary>
        /// <param name="PackageType">Package Type</param>
        /// <param name="Origin">Origin</param>
        /// <param name="DataItems">DataItems</param>
        /// <param name="Client">Client</param>
        public Package(PackageTypes PackageType, int Origin, List<string> DataItems, TcpClient Client)
        {
            this.PackageType = (int)PackageType;
            this.Origin = Origin;
            this.DataItems = DataItems;
            this.Client = Client;
            IsValid = true;
        }

        /// <summary>
        /// Creating a new Package
        /// </summary>
        /// <param name="PackageType">Package Type</param>
        /// <param name="DataItems">DataItems</param>
        /// <param name="Client">Client</param>
        public Package(PackageTypes PackageType, List<string> DataItems, TcpClient Client)
        {
            this.PackageType = (int)PackageType;
            this.DataItems = DataItems;
            this.Client = Client;
            IsValid = true;
        }

        /// <summary>
        /// Creating a new Package
        /// </summary>
        /// <param name="PackageType">Package Type</param>
        /// <param name="Origin">Origin</param>
        /// <param name="DataItems">DataItems</param>
        /// <param name="Client">Client</param>
        public Package(PackageTypes PackageType, int Origin, string DataItems, TcpClient Client)
        {
            this.PackageType = (int)PackageType;
            this.Origin = Origin;
            this.DataItems = new List<string> { DataItems };
            this.Client = Client;
            IsValid = true;
        }

        /// <summary>
        /// Creating a new Package
        /// </summary>
        /// <param name="PackageType">Package Type</param>
        /// <param name="DataItems">DataItems</param>
        /// <param name="Client">Client</param>
        public Package(PackageTypes PackageType, string DataItems, TcpClient Client)
        {
            this.PackageType = (int)PackageType;
            this.DataItems = new List<string> { DataItems };
            this.Client = Client;
            IsValid = true;
        }

        /// <summary>
        /// Handle the package
        /// </summary>
        public void Handle()
        {
            Core.Package.Handle(this);
        }

        /// <summary>
        /// Check if the package data is full or partial data.
        /// </summary>
        public bool IsFullPackageData()
        {
            if (string.IsNullOrWhiteSpace(DataItems[0]))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Get Full Package Data
        /// </summary>
        public override string ToString()
        {
            string outputStr = ProtocolVersion + "|" + PackageType.ToString() + "|" + Origin.ToString() + "|" + DataItemsCount.ToString();

            int CurrentIndex = 0;
            string data = null;

            foreach (string dataItem in DataItems)
            {
                outputStr += "|" + CurrentIndex.ToString();
                data += dataItem;
                CurrentIndex += dataItem.Length;
            }

            outputStr += "|" + data;

            return outputStr;
        }
    }
}
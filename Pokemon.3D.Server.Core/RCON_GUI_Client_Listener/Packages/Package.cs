using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;

namespace Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages
{
    /// <summary>
    /// Class containing Package Data Handler.
    /// </summary>
    public class Package
    {
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
        /// Get Package Handler Object.
        /// </summary>
        public PackageHandler PackageHandler { get; } = new PackageHandler();

        /// <summary>
        /// A collection of Package Type.
        /// </summary>
        public enum PackageTypes
        {
            /// <summary>
            /// Unknown Data
            /// </summary>
            Unknown = -1,

            /// <summary>
            /// Authentication Package
            /// <para>Get: DataItems[0] = MD5Hash | DataItems[1] = SHA1Hash | DataItems[2] = SHA256Hash</para>
            /// <para>Set: DataItems[0] = Result.ToBool</para>
            /// </summary>
            Authentication,

            /// <summary>
            /// Ping Package
            /// <para>Get: DataItems[0] = Null</para>
            /// </summary>
            Ping,

            /// <summary>
            /// Kick Package
            /// <para>Set: DataItems[0] = Reason</para>
            /// </summary>
            Kicked,

            /// <summary>
            /// ID Package
            /// <para>Set: DataItems[0] = Player ID</para>
            /// </summary>
            ID,

            #region Client Event Player Data
            /// <summary>
            /// Add Player
            /// <para>Get: DataItems[] = Player Data.</para>
            /// </summary>
            AddPlayer,

            /// <summary>
            /// Update Player
            /// <para>Get: DataItems[] = Player Data.</para>
            /// </summary>
            UpdatePlayer,

            /// <summary>
            /// Remove Player
            /// <para>Get: DataItems[] = Player Data.</para>
            /// </summary>
            RemovePlayer,
            #endregion Client Event Player Data

            #region Client Event Logger
            /// <summary>
            /// Logger Message
            /// <para>Get: DataItems[0] = Command.</para>
            /// <para>Set: DataItems[0] = Message.</para>
            /// </summary>
            Logger,
            #endregion
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
                    Core.Logger.Log("Package is incomplete.", Logger.LogTypes.Debug, Client);
                    IsValid = false;
                    return;
                }

                List<string> bits = FullData.Split("|".ToCharArray()).ToList();

                if (bits.Count >= 5)
                {
                    // Protocol Version
                    if (!string.Equals(Core.Setting.ProtocolVersion, bits[0], StringComparison.OrdinalIgnoreCase))
                    {
                        Core.Logger.Log("Package does not contains valid Protocol Version.", Logger.LogTypes.Debug, Client);
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
                        Core.Logger.Log("Package does not contains valid Package Type.", Logger.LogTypes.Debug, Client);
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
                        Core.Logger.Log("Package does not contains valid Origin.", Logger.LogTypes.Debug, Client);
                        IsValid = false;
                        return;
                    }

                    // DataItemsCount
                    int DataItemsCount = 0;
                    try
                    {
                        DataItemsCount = bits[3].ToInt();
                    }
                    catch (Exception)
                    {
                        Core.Logger.Log("Package does not contains valid DataItemsCount.", Logger.LogTypes.Debug, Client);
                        IsValid = false;
                        return;
                    }

                    List<int> OffsetList = new List<int>();

                    // Count from 4th item to second last item. Those are the offsets.
                    for (int i = 4; i < DataItemsCount + 4; i++)
                    {
                        try
                        {
                            OffsetList.Add(bits[i].ToInt());
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
                    Core.Logger.Log("Package is incomplete.", Logger.LogTypes.Debug, Client);
                    IsValid = false;
                }
            }
            catch (Exception ex)
            {
                Core.Logger.Log(ex.Message, Logger.LogTypes.Debug, Client);
                IsValid = false;
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
        /// Handle the package.
        /// </summary>
        public void Handle()
        {
            PackageHandler.Handle(this);
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
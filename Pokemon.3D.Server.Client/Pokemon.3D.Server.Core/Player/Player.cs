using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Modules;
using Pokemon_3D_Server_Core.Network;
using Pokemon_3D_Server_Core.Packages;

namespace Pokemon_3D_Server_Core.Players
{
    /// <summary>
    /// Class containing Player infomation
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Get/Set Player DataItem[0]
        /// </summary>
        public string GameMode { get; set; }

        private int _isGameJoltPlayer;
        /// <summary>
        /// Get/Set Player DataItem[1]
        /// </summary>
        public bool isGameJoltPlayer
        {
            get
            {
                return _isGameJoltPlayer.Tobool();
            }
            set
            {
                _isGameJoltPlayer = value.Tobool();
            }
        }

        /// <summary>
        /// Get/Set Player DataItem[2]
        /// </summary>
        public int GameJoltID { get; set; }

        /// <summary>
        /// Get/Set Player DataItem[3]
        /// </summary>
        public string DecimalSeparator { get; set; }

        /// <summary>
        /// Get/Set Player DataItem[4]
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get/Set Player DataItem[5]
        /// </summary>
        public string LevelFile { get; set; }

        /// <summary>
        /// Get/Set Player DataItem[6]
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Get/Set Player Position X
        /// </summary>
        public double Position_X
        {
            get
            {
                return Position.GetSplit(0).Todouble();
            }
            set
            {
                Position = value.ToString().ConvertStringCulture(this) + "|" + Position_Y.ToString().ConvertStringCulture(this) + "|" + Position_Z.ToString().ConvertStringCulture(this);
            }
        }

        /// <summary>
        /// Get/Set Player Position Y
        /// </summary>
        public double Position_Y
        {
            get
            {
                return Position.GetSplit(1).Todouble();
            }
            set
            {
                Position = Position_X.ToString().ConvertStringCulture(this) + "|" + value.ToString().ConvertStringCulture(this) + "|" + Position_Z.ToString().ConvertStringCulture(this);
            }
        }

        /// <summary>
        /// Get/Set Player Position Z
        /// </summary>
        public double Position_Z
        {
            get
            {
                return Position.GetSplit(2).Todouble();
            }
            set
            {
                Position = Position_X.ToString().ConvertStringCulture(this) + "|" + Position_Y.ToString().ConvertStringCulture(this) + "|" + value.ToString().ConvertStringCulture(this);
            }
        }

        /// <summary>
        /// Get/Set Player DataItem[7]
        /// </summary>
        public int Facing { get; set; }

        private int _Moving;
        /// <summary>
        /// Get/Set Player DataItem[8]
        /// </summary>
        public bool Moving
        {
            get
            {
                return _Moving.Tobool();
            }
            set
            {
                _Moving = value.Tobool();
            }
        }

        /// <summary>
        /// Get/Set Player DataItem[9]
        /// </summary>
        public string Skin { get; set; }

        /// <summary>
        /// Get/Set Player DataItem[10]
        /// </summary>
        public int BusyType { get; set; }

        private int _PokemonVisible;
        /// <summary>
        /// Get/Set Player DataItem[11]
        /// </summary>
        public bool PokemonVisible
        {
            get
            {
                return _PokemonVisible.Tobool();
            }
            set
            {
                _PokemonVisible = value.Tobool();
            }
        }

        /// <summary>
        /// Get/Set Player DataItem[12]
        /// </summary>
        public string PokemonPosition { get; set; }

        /// <summary>
        /// Get/Set Player PokemonPosition X
        /// </summary>
        public double PokemonPosition_X
        {
            get
            {
                return PokemonPosition.GetSplit(0).Todouble();
            }
            set
            {
                PokemonPosition = value.ToString().ConvertStringCulture(this) + "|" + PokemonPosition_Y.ToString().ConvertStringCulture(this) + "|" + PokemonPosition_Z.ToString().ConvertStringCulture(this);
            }
        }

        /// <summary>
        /// Get/Set Player PokemonPosition Y
        /// </summary>
        public double PokemonPosition_Y
        {
            get
            {
                return PokemonPosition.GetSplit(1).Todouble();
            }
            set
            {
                PokemonPosition = PokemonPosition_X.ToString().ConvertStringCulture(this) + "|" + value.ToString().ConvertStringCulture(this) + "|" + PokemonPosition_Z.ToString().ConvertStringCulture(this);
            }
        }

        /// <summary>
        /// Get/Set Player PokemonPosition Z
        /// </summary>
        public double PokemonPosition_Z
        {
            get
            {
                return PokemonPosition.GetSplit(2).Todouble();
            }
            set
            {
                PokemonPosition = PokemonPosition_X.ToString().ConvertStringCulture(this) + "|" + PokemonPosition_Y.ToString().ConvertStringCulture(this) + "|" + value.ToString().ConvertStringCulture(this);
            }
        }

        /// <summary>
        /// Get/Set Player DataItem[13]
        /// </summary>
        public string PokemonSkin { get; set; }

        /// <summary>
        /// Get/Set Player DataItem[14]
        /// </summary>
        public int PokemonFacing { get; set; }

        /// <summary>
        /// Get/Set Player ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Get/Set Player Client
        /// </summary>
        public Networking Network { get; set; }

        /// <summary>
        /// Get/Set Player Last Chat Message
        /// </summary>
        public string LastChatMessage { get; set; }

        /// <summary>
        /// Get/Set Player Last Chat Time
        /// </summary>
        public DateTime LastChatTime { get; set; }

        /// <summary>
        /// Get/Set Player Last Valid Game Data
        /// </summary>
        public List<string> LastValidGameData { get; set; }

        /// <summary>
        /// Get/Set Player Last Movement Position [Direction | Position Vector]
        /// </summary>
        public string LastMovementPosition { get; set; }

        /// <summary>
        /// Get/Set Player Last Pokemon Movement Position [Direction | Position Vector]
        /// </summary>
        public string LastPokemonMovementPosition { get; set; }

        /// <summary>
        /// A Collection of Busy Type
        /// </summary>
        public enum BusyTypes
        {
            /// <summary>
            /// Not Busy
            /// </summary>
            NotBusy,

            /// <summary>
            /// Battling
            /// </summary>
            Battling,

            /// <summary>
            /// Chatting
            /// </summary>
            Chatting,

            /// <summary>
            /// Inactive - AFK
            /// </summary>
            Inactive,
        }

        /// <summary>
        /// A Collection of Operator Type
        /// </summary>
        public enum OperatorTypes
        {
            /// <summary>
            /// Normal Player
            /// </summary>
            Player,

            /// <summary>
            /// Player with Chat Moderator ability
            /// </summary>
            ChatModerator,

            /// <summary>
            /// Player with Server Moderator ability
            /// </summary>
            ServerModerator,

            /// <summary>
            /// Player with Global Moderator ability
            /// </summary>
            GlobalModerator,

            /// <summary>
            /// Player with Administrator ability
            /// </summary>
            Administrator,

            /// <summary>
            /// Player with Administrator ability and Debugging ability
            /// </summary>
            Creator,
        }

        /// <summary>
        /// Ner Player (Update Player - Not send to server)
        /// </summary>
        /// <param name="p">Package</param>
        public Player(Package p)
        {
            Update(p, false);
        }

        /// <summary>
        /// New Player (Update Player)
        /// </summary>
        /// <param name="p">Package</param>
        /// <param name="ID">Player ID</param>
        public Player(Package p, int ID)
        {
            this.ID = ID;
            Network = new Networking(p.Client);

        }

        /// <summary>
        /// Update Player Objects
        /// </summary>
        /// <param name="p">Package Data</param>
        /// <param name="SentToServer">Sent data to server?</param>
        public void Update(Package p, bool SentToServer)
        {
            Network.LastValidMovement = DateTime.Now;

            if (p.IsFullPackageData())
            {
                GameMode = p.DataItems[0];
                isGameJoltPlayer = p.DataItems[1].Toint().Tobool();
                GameJoltID = isGameJoltPlayer ? p.DataItems[2].Toint() : -1;
                DecimalSeparator = p.DataItems[3];
                Name = p.DataItems[4];
                LevelFile = p.DataItems[5];
                Position = p.DataItems[6];
                Facing = p.DataItems[7].Toint();
                Moving = p.DataItems[8].Toint().Tobool();
                Skin = p.DataItems[9];
                BusyType = p.DataItems[10].Toint();
                PokemonVisible = p.DataItems[11].Toint().Tobool();
                PokemonPosition = p.DataItems[12];
                PokemonSkin = p.DataItems[13];
                PokemonFacing = p.DataItems[14].Toint();

                LastValidGameData = new List<string> { LevelFile, Position, Facing.ToString(), Moving.ToString(), Skin, BusyType.ToString(), PokemonVisible.ToString(), PokemonPosition, PokemonSkin, PokemonFacing.ToString() };
            }
            else
            {
                LastValidGameData = new List<string> { LevelFile, Position, Facing.ToString(), Moving.ToString(), Skin, BusyType.ToString(), PokemonVisible.ToString(), PokemonPosition, PokemonSkin, PokemonFacing.ToString() };

                if (!string.IsNullOrWhiteSpace(p.DataItems[5]) && p.DataItems[5].SplitCount() == 1)
                {
                    LevelFile = p.DataItems[5];
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[6]) && p.DataItems[6].SplitCount() == 3)
                {
                    Position = p.DataItems[6];
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[7]) && p.DataItems[7].SplitCount() == 1)
                {
                    Facing = p.DataItems[7].Toint();
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[8]) && p.DataItems[8].SplitCount() == 1)
                {
                    Moving = p.DataItems[8].Toint().Tobool();
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[9]) && p.DataItems[9].SplitCount() <= 2)
                {
                    Skin = p.DataItems[9];
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[10]) && p.DataItems[10].SplitCount() == 1)
                {
                    BusyType = p.DataItems[10].Toint();
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[11]) && p.DataItems[11].SplitCount() == 1)
                {
                    PokemonVisible = p.DataItems[11].Toint().Tobool();
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[12]) && p.DataItems[12].SplitCount() == 3)
                {
                    PokemonPosition = p.DataItems[12];
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[13]) && p.DataItems[13].SplitCount() <= 2)
                {
                    PokemonSkin = p.DataItems[13];
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[14]) && p.DataItems[14].SplitCount() == 1)
                {
                    PokemonFacing = p.DataItems[14].Toint();
                }
            }

            // Sent To Server
        }

        /// <summary>
        /// Get the current player status.
        /// </summary>
        public string GetPlayerBusyType()
        {
            switch (BusyType)
            {
                case (int)BusyTypes.NotBusy:
                    return "";
                case (int)BusyTypes.Battling:
                    return " - Battling";
                case (int)BusyTypes.Chatting:
                    return " - Chatting";
                case (int)BusyTypes.Inactive:
                    return " - Inactive";
                default:
                    return "";
            }
        }

        private List<string> GenerateGameData(bool FullPackageData)
        {
            List<string> ReturnList;

            if (FullPackageData)
            {
                ReturnList = new List<string>
                {
                    GameMode,
                    isGameJoltPlayer.Tobool().ToString(),
                    isGameJoltPlayer ? GameJoltID.ToString() : "",
                    DecimalSeparator,
                    Name,
                    LevelFile,
                    Position.ConvertStringCulture(this),
                    Facing.ToString(),
                    Moving.Tobool().ToString(),
                    Skin,
                    BusyType.ToString(),
                    PokemonVisible.Tobool().ToString(),
                    PokemonPosition.ConvertStringCulture(this),
                    PokemonSkin,
                    PokemonFacing.ToString()
                };
            }
            else
            {
                ReturnList = new List<string>
                {
                    "",
                    "",
                    "",
                    "",
                    Name,
                    LastValidGameData[0] == LevelFile ? "" : LevelFile,
                    LastValidGameData[1] == Position ? "" : Position.ConvertStringCulture(this),
                    LastValidGameData[2] == Facing.ToString() ? "" : Facing.ToString(),
                    LastValidGameData[3] == Moving.Tobool().ToString() ? "" : Moving.Tobool().ToString(),
                    LastValidGameData[4] == Skin ? "" : Skin,
                    LastValidGameData[5] == BusyType.ToString() ? "" : BusyType.ToString(),
                    LastValidGameData[6] == PokemonVisible.Tobool().ToString() ? "" : PokemonVisible.Tobool().ToString(),
                    LastValidGameData[7] == PokemonPosition ? "" : PokemonPosition.ConvertStringCulture(this),
                    LastValidGameData[8] == PokemonSkin ? "" : PokemonSkin,
                    LastValidGameData[9] == PokemonFacing.ToString() ? "" : PokemonFacing.ToString()
                };
            }

            return ReturnList;
        }

        private List<string> CatchUp(string LastPosition, bool IsPlayerData = true)
        {
            double LastPositionX = LastPosition.GetSplit(0).Todouble();
            double LastPositionY = LastPosition.GetSplit(1).Todouble();
            double LastPositionZ = LastPosition.GetSplit(2).Todouble();

            List<string> Positions = new List<string>();

            if (IsPlayerData)
            {
                if (Position_X < LastPositionX)
                {
                    // Going Left
                    if (LastMovementPosition != null && LastMovementPosition.GetSplit(0) == "Left" && LastMovementPosition.GetSplit(1).Todouble() == Position_X.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionX; i >= LastPositionX - 1; i -= "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                    i.ToString().ConvertStringCulture(this),
                                    LastPositionY.ToString(),
                                    LastPositionZ.ToString()
                                    ));
                        }
                        LastMovementPosition = "Left|" + Position_X.Floor();
                        Position_X = LastPositionX - 1;
                        return Positions;
                    }
                }
                else if (Position_X > LastPositionX)
                {
                    // Going Right
                    if (LastMovementPosition != null && LastMovementPosition.GetSplit(0) == "Right" && LastMovementPosition.GetSplit(1).Todouble() == Position_X.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionX; i <= LastPositionX + 1; i += "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                    i.ToString().ConvertStringCulture(this),
                                    LastPositionY.ToString(),
                                    LastPositionZ.ToString()
                                    ));
                        }
                        LastMovementPosition = "Right|" + Position_X.Floor();
                        Position_X = LastPositionX + 1;
                        return Positions;
                    }
                }
                else if (Position_Y > LastPositionY)
                {
                    // Going Up
                    if (LastMovementPosition != null && LastMovementPosition.GetSplit(0) == "Up" && LastMovementPosition.GetSplit(1).Todouble() == Position_Y.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionY; i <= LastPositionY + 1; i += "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                LastPositionX.ToString(),
                                i.ToString().ConvertStringCulture(this),
                                LastPositionZ.ToString()
                                ));
                        }
                        LastMovementPosition = "Up|" + Position_Y.Floor();
                        Position_Y = LastPositionY + 1;
                        return Positions;
                    }
                }
                else if (Position_Y < LastPositionY)
                {
                    // Going Down
                    if (LastMovementPosition != null && LastMovementPosition.GetSplit(0) == "Down" && LastMovementPosition.GetSplit(1).Todouble() == Position_Y.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionY; i >= LastPositionY - 1; i -= "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                LastPositionX.ToString(),
                                i.ToString().ConvertStringCulture(this),
                                LastPositionZ.ToString()
                                ));
                        }
                        LastMovementPosition = "Down|" + Position_Y.Floor();
                        Position_Y = LastPositionY - 1;
                        return Positions;
                    }
                }
                else if (Position_Z < LastPositionZ)
                {
                    // Going Front
                    if (LastMovementPosition != null && LastMovementPosition.GetSplit(0) == "Front" && LastMovementPosition.GetSplit(1).Todouble() == Position_Z.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionZ; i >= LastPositionZ - 1; i -= "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                LastPositionX.ToString(),
                                LastPositionY.ToString(),
                                i.ToString().ConvertStringCulture(this)
                                ));
                        }
                        LastMovementPosition = "Front|" + Position_Z.Floor();
                        Position_Z = LastPositionZ - 1;
                        return Positions;
                    }
                }
                else if (Position_Z > LastPositionZ)
                {
                    // Going Back
                    if (LastMovementPosition != null && LastMovementPosition.GetSplit(0) == "Back" && LastMovementPosition.GetSplit(1).Todouble() == Position_Z.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionZ; i <= LastPositionZ + 1; i += "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                LastPositionX.ToString(),
                                LastPositionY.ToString(),
                                i.ToString().ConvertStringCulture(this)
                                ));
                        }
                        LastMovementPosition = "Back|" + Position_Z.Floor();
                        Position_Z = LastPositionZ + 1;
                        return Positions;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (PokemonPosition_X < LastPositionX)
                {
                    // Going Left
                    if (LastPokemonMovementPosition != null && LastPokemonMovementPosition.GetSplit(0) == "Left" && LastPokemonMovementPosition.GetSplit(1).Todouble() == PokemonPosition_X.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionX; i >= LastPositionX - 1; i -= "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                    i.ToString().ConvertStringCulture(this),
                                    LastPositionY.ToString(),
                                    LastPositionZ.ToString()
                                    ));
                        }
                        LastPokemonMovementPosition = "Left|" + PokemonPosition_X.Floor();
                        PokemonPosition_X = LastPositionX - 1;
                        return Positions;
                    }
                }
                else if (PokemonPosition_X > LastPositionX)
                {
                    // Going Right
                    if (LastPokemonMovementPosition != null && LastPokemonMovementPosition.GetSplit(0) == "Right" && LastPokemonMovementPosition.GetSplit(1).Todouble() == PokemonPosition_X.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionX; i <= LastPositionX + 1; i += "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                    i.ToString().ConvertStringCulture(this),
                                    LastPositionY.ToString(),
                                    LastPositionZ.ToString()
                                    ));
                        }
                        LastPokemonMovementPosition = "Right|" + PokemonPosition_X.Floor();
                        PokemonPosition_X = LastPositionX + 1;
                        return Positions;
                    }
                }
                else if (Position_Y > LastPositionY)
                {
                    // Going Up
                    if (LastPokemonMovementPosition != null && LastPokemonMovementPosition.GetSplit(0) == "Up" && LastPokemonMovementPosition.GetSplit(1).Todouble() == PokemonPosition_Y.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionY; i <= LastPositionY + 1; i += "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                LastPositionX.ToString(),
                                i.ToString().ConvertStringCulture(this),
                                LastPositionZ.ToString()
                                ));
                        }
                        LastPokemonMovementPosition = "Up|" + PokemonPosition_Y.Floor();
                        PokemonPosition_Y = LastPositionY + 1;
                        return Positions;
                    }
                }
                else if (Position_Y < LastPositionY)
                {
                    // Going Down
                    if (LastPokemonMovementPosition != null && LastPokemonMovementPosition.GetSplit(0) == "Down" && LastPokemonMovementPosition.GetSplit(1).Todouble() == PokemonPosition_Y.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionY; i >= LastPositionY - 1; i -= "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                LastPositionX.ToString(),
                                i.ToString().ConvertStringCulture(this),
                                LastPositionZ.ToString()
                                ));
                        }
                        LastPokemonMovementPosition = "Down|" + PokemonPosition_Y.Floor();
                        PokemonPosition_Y = LastPositionY - 1;
                        return Positions;
                    }
                }
                else if (Position_Z < LastPositionZ)
                {
                    // Going Front
                    if (LastPokemonMovementPosition != null && LastPokemonMovementPosition.GetSplit(0) == "Front" && LastPokemonMovementPosition.GetSplit(1).Todouble() == PokemonPosition_Z.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionZ; i >= LastPositionZ - 1; i -= "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                LastPositionX.ToString(),
                                LastPositionY.ToString(),
                                i.ToString().ConvertStringCulture(this)
                                ));
                        }
                        LastPokemonMovementPosition = "Front|" + PokemonPosition_Z.Floor();
                        PokemonPosition_Z = LastPositionZ - 1;
                        return Positions;
                    }
                }
                else if (Position_Z > LastPositionZ)
                {
                    // Going Back
                    if (LastPokemonMovementPosition != null && LastPokemonMovementPosition.GetSplit(0) == "Back" && LastPokemonMovementPosition.GetSplit(1).Todouble() == PokemonPosition_Z.Floor())
                    {
                        return null;
                    }
                    else
                    {
                        for (double i = LastPositionZ; i <= LastPositionZ + 1; i += "0.01".Todouble())
                        {
                            Positions.Add(
                                string.Format(@"{0}|{1}|{2}",
                                LastPositionX.ToString(),
                                LastPositionY.ToString(),
                                i.ToString().ConvertStringCulture(this)
                                ));
                        }
                        LastPokemonMovementPosition = "Back|" + PokemonPosition_Z.Floor();
                        PokemonPosition_Z = LastPositionZ + 1;
                        return Positions;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
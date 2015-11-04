﻿using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Players
{
    /// <summary>
    /// Class containing Player infomation
    /// </summary>
    public class Player
    {
        #region Player Data
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
                return Position.GetSplit(0).ToDouble();
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
                return Position.GetSplit(1).ToDouble();
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
                return Position.GetSplit(2).ToDouble();
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
                return PokemonPosition.GetSplit(0).ToDouble();
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
                return PokemonPosition.GetSplit(1).ToDouble();
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
                return PokemonPosition.GetSplit(2).ToDouble();
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
        /// Get/Set Player Last Valid Game Data
        /// </summary>
        public List<string> LastValidGameData { get; set; } = new List<string>();
        #endregion Player Data

        /// <summary>
        /// Get/Set Player ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Get/Set Player Client
        /// </summary>
        public Networking Network { get; set; }

        #region Chat Channel System
        /* 
            Chat Channel System:
            Default:
            1. All
            2. Server Chat
            3. General Chat
            4. Trade Chat
            5. PvP Chat

            Custom:
            6. German Lounge
        */
        /// <summary>
        /// Get/Set Player Current Chat Channel
        /// </summary>
        public string CC_CurrentChatChannel { get; set; } = "All";

        /// <summary>
        /// Get/Set Player Last Chat Message
        /// </summary>
        public string CC_LastChatMessage { get; set; }

        /// <summary>
        /// Get/Set Player Last Chat Time
        /// </summary>
        public DateTime CC_LastChatTime { get; set; } = DateTime.Now;
        #endregion Chat Channel System

        #region PvP System
        /// <summary>
        /// Get/Set Current Player PvP Status.
        /// </summary>
        public PvPTypes PvP_Status { get; set; } = PvPTypes.Nothing;

        /// <summary>
        /// Get/Set PvP Host?
        /// </summary>
        public bool PvP_Host { get; set; } = false;

        /// <summary>
        /// Get/Set PvP Rule Set.
        /// </summary>
        public PvPRules PvP_Rules { get; set; } = PvPRules.Default;

        /// <summary>
        /// Get/Set PvP Opponent ID
        /// </summary>
        public int PvP_OpponentID { get; set; } = -1;

        /// <summary>
        /// Get/Set PvP Participants.
        /// </summary>
        public List<string> PvP_Pokemon { get; set; } = new List<string>();

        /// <summary>
        /// PvP Types
        /// </summary>
        public enum PvPTypes
        {
            /// <summary>
            /// PvP Types: Lobby
            /// </summary>
            Lobby,

            /// <summary>
            /// PvP Types: Battling
            /// </summary>
            Battling,

            /// <summary>
            /// PvP Types: Nothing
            /// </summary>
            Nothing,
        }

        /// <summary>
        /// PvP Rules
        /// </summary>
        public enum PvPRules
        {
            /// <summary>
            /// PvP Rules: Default
            /// </summary>
            Default,
        }
        #endregion PvP System

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
            /// GameJolt Player
            /// </summary>
            GameJoltPlayer,

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
            Update(p, false);

            if (isGameJoltPlayer)
            {
                Core.Setting.OnlineSettingListData.Add(new OnlineSetting(Name, GameJoltID));
            }
            Core.Player.Add(this);

            Core.Player.SentToPlayer(new Package(Package.PackageTypes.ID, ID.ToString(), p.Client));

            for (int i = 0; i < Core.Player.Count; i++)
            {
                if (Core.Player[i].ID != ID)
                {
                    Core.Player.SentToPlayer(new Package(Package.PackageTypes.CreatePlayer, Core.Player[i].ID.ToString(), p.Client));
                    Core.Player.SentToPlayer(new Package(Package.PackageTypes.GameData, Core.Player[i].ID, Core.Player[i].GenerateGameData(true), p.Client));
                }
            }

            Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.CreatePlayer, ID.ToString(), null));
            Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.GameData, ID, GenerateGameData(true), null));

            if (Core.Setting.WelcomeMessage != null)
            {
                Core.Player.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.WelcomeMessage, p.Client));
            }

            Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, isGameJoltPlayer ?
                Core.Setting.Token("SERVER_GAMEJOLT", Name, GameJoltID.ToString(), "join the game!") :
                Core.Setting.Token("SERVER_NOGAMEJOLT", Name, "join the game!"), null));
            Core.Logger.Log(isGameJoltPlayer ?
                Core.Setting.Token("SERVER_GAMEJOLT", Name, GameJoltID.ToString(), "join the game!") :
                Core.Setting.Token("SERVER_NOGAMEJOLT", Name, "join the game!"), Logger.LogTypes.Info, p.Client);

            if (Core.Listener.TimeLeft() != null)
            {
                Core.Player.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_RESTARTWARNING", Core.Listener.TimeLeft()), p.Client));
            }
        }

        /// <summary>
        /// Update Player Objects
        /// </summary>
        /// <param name="p">Package Data</param>
        /// <param name="SentToServer">Sent data to server?</param>
        public void Update(Package p, bool SentToServer)
        {

            if (Network != null)
            {
                Network.LastValidMovement = DateTime.Now;
            }

            if (p.IsFullPackageData())
            {
                GameMode = p.DataItems[0];
                isGameJoltPlayer = p.DataItems[1].ToInt().Tobool();
                GameJoltID = isGameJoltPlayer ? p.DataItems[2].ToInt() : -1;
                DecimalSeparator = p.DataItems[3];
                Name = p.DataItems[4];
                LevelFile = p.DataItems[5];
                Position = p.DataItems[6];
                Facing = p.DataItems[7].ToInt();
                Moving = p.DataItems[8].ToInt().Tobool();
                Skin = p.DataItems[9];
                BusyType = p.DataItems[10].ToInt();
                PokemonVisible = p.DataItems[11].ToInt().Tobool();
                PokemonPosition = p.DataItems[12];
                PokemonSkin = p.DataItems[13];
                PokemonFacing = p.DataItems[14].ToInt();

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
                    Facing = p.DataItems[7].ToInt();
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[8]) && p.DataItems[8].SplitCount() == 1)
                {
                    Moving = p.DataItems[8].ToInt().Tobool();
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[9]) && p.DataItems[9].SplitCount() <= 2)
                {
                    Skin = p.DataItems[9];
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[10]) && p.DataItems[10].SplitCount() == 1)
                {
                    BusyType = p.DataItems[10].ToInt();
                }
                if (!string.IsNullOrWhiteSpace(p.DataItems[11]) && p.DataItems[11].SplitCount() == 1)
                {
                    PokemonVisible = p.DataItems[11].ToInt().Tobool();
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
                    PokemonFacing = p.DataItems[14].ToInt();
                }
            }

            // Sent To Server
            if (SentToServer)
            {
                Core.RCONPlayer.SendToAllPlayer(new RCON_Client_Listener.Packages.Package(RCON_Client_Listener.Packages.Package.PackageTypes.UpdatePlayer, $"{ID},{ToString()}", null));
                PlayerEvent.Invoke(PlayerEvent.Types.Update, $"{ID},{ToString()}");

                if (p.IsFullPackageData())
                {
                    Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.GameData, ID, GenerateGameData(true), null));
                }
                else
                {
                    Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.GameData, ID, GenerateGameData(false), null));
                }
            }
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
                    return "- Battling";
                case (int)BusyTypes.Chatting:
                    return "- Chatting";
                case (int)BusyTypes.Inactive:
                    return "- Inactive";
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
                    "",
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

        /// <summary>
        /// Get Player Status. GUI use only.
        /// </summary>
        public override string ToString()
        {
            return isGameJoltPlayer ? string.Format("ID: {3} | {0} ({1}) {2}", Name, GameJoltID.ToString(), GetPlayerBusyType(), ID.ToString()) : string.Format("ID: {2} | {0} {1}", Name, GetPlayerBusyType(), ID.ToString());
        }

        /// <summary>
        /// Check PvP Pokemon.
        /// </summary>
        public bool DoPvPValidation()
        {
            // {"Pokemon"[6]}{"Experience"[112709]}{"Gender"[0]}{"EggSteps"[0]}{"Item"[146]}{"ItemData"[]}{"NickName"[]}{"Level"[100]}{"OT"[28377]}{"Ability"[66]}{"Status"[]}{"Nature"[4]}{"CatchLocation"[]}{"CatchTrainer"[jianmingyong]}{"CatchBall"[5]}{"CatchMethod"[]}{"Friendship"[255]}{"isShiny"[1]}{"Attack1"[488,20,20]}{"Attack2"[424,15,15]}{"Attack3"[481,15,15]}{"Attack4"[53,15,15]}{"HP"[274]}{"EVs"[10,35,20,30,12,26]}{"IVs"[6,27,12,21,12,18]}{"AdditionalData"[]}{"IDValue"[P3ewPpDfvzR]}
            // |{"Pokemon"[250]}{"Experience"[116214]}{"Gender"[2]}{"EggSteps"[0]}{"Item"[146]}{"ItemData"[]}{"NickName"[]}{"Level"[45]}{"OT"[116016]}{"Ability"[46]}{"Status"[]}{"Nature"[1]}{"CatchLocation"[]}{"CatchTrainer"[jianmingyong]}{"CatchBall"[2]}{"CatchMethod"[caught at]}{"Friendship"[130]}{"isShiny"[0]}{"Attack1"[326,20,20]}{"Attack2"[19,15,15]}{"Attack3"[221,5,5]}{"Attack4"[126,5,5]}{"HP"[157]}{"EVs"[3,3,0,0,0,0]}{"IVs"[16,4,31,23,1,2]}{"AdditionalData"[]}{"IDValue"[bE8d1hMZbsB]}
            // |{"Pokemon"[3]}{"Experience"[49534]}{"Gender"[0]}{"EggSteps"[0]}{"Item"[0]}{"ItemData"[]}{"NickName"[]}{"Level"[100]}{"OT"[28377]}{"Ability"[65]}{"Status"[]}{"Nature"[6]}{"CatchLocation"[Daycare]}{"CatchTrainer"[jianmingyong]}{"CatchBall"[5]}{"CatchMethod"[obtained at]}{"Friendship"[255]}{"isShiny"[1]}{"Attack1"[15,30,30]}{"Attack2"[75,25,25]}{"Attack3"[70,15,15]}{"Attack4"[431,20,20]}{"HP"[298]}{"EVs"[1,19,5,18,6,9]}{"IVs"[28,5,31,26,0,14]}{"AdditionalData"[]}{"IDValue"[HSYxV1MHrWV]}

            // Check for invalid Pokemon.
            List<int> InvalidPokemonID = new List<int> { };

            for (int i = 0; i < PvP_Pokemon.Count; i++)
            {
                
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Global
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
        public Networking Client { get; set; }

        /// <summary>
        /// Get/Set Player Last Chat Message
        /// </summary>
        public string LastChatMessage { get; set; }

        /// <summary>
        /// Get/Set Player Last Chat Time
        /// </summary>
        public DateTime LastChatTime { get; set; }

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


    }
}

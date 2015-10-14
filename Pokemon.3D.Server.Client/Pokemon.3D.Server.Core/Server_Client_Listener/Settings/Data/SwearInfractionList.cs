using System;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data
{
    /// <summary>
    /// Class containing SwearInfractionList data
    /// </summary>
    public class SwearInfractionList : CommonData
    {
        /// <summary>
        /// Get/Set SwearInfraction Points
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Get/Set SwearInfraction Mute Amount
        /// </summary>
        public int Muted { get; set; }

        /// <summary>
        /// Get/Set SwearInfraction StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// New SwearInfractionList
        /// </summary>
        /// <param name="Name">Player Name</param>
        /// <param name="GameJoltID">Player GameJolt ID</param>
        /// <param name="Points">SwearInfraction Points</param>
        /// <param name="Muted">SwearInfraction Points</param>
        /// <param name="StartTime">SwearInfraction StartTime</param>
        public SwearInfractionList(string Name, int GameJoltID, int Points, int Muted, DateTime StartTime)
        {
            this.Name = Name;
            this.GameJoltID = GameJoltID;
            this.Points = Points;
            this.Muted = Muted;
            this.StartTime = StartTime;
        }
    }
}
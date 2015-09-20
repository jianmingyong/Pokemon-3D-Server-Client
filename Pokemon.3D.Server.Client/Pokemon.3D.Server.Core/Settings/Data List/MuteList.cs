using System;

namespace Pokemon_3D_Server_Core.Settings
{
    /// <summary>
    /// Class containing MuteList data
    /// </summary>
    public class MuteList : CommonList
    {
        /// <summary>
        /// New MuteList
        /// </summary>
        /// <param name="Name">Player Name</param>
        /// <param name="GameJoltID">Player GameJolt ID</param>
        /// <param name="Reason">Player Mute Reason</param>
        /// <param name="StartTime">Player Mute StartTime</param>
        /// <param name="Duration">Player Mute Duration</param>
        public MuteList(string Name, int GameJoltID, string Reason, DateTime StartTime, int Duration)
        {
            this.Name = Name;
            this.GameJoltID = GameJoltID;
            this.Reason = Reason;
            this.StartTime = StartTime;
            this.Duration = Duration;
        }
    }
}

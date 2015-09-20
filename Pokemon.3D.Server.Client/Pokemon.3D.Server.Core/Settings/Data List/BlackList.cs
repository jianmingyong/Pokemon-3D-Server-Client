using System;

namespace Pokemon_3D_Server_Core.Settings
{
    /// <summary>
    /// Class containing BlackList data
    /// </summary>
    public class BlackList : CommonList
    {
        /// <summary>
        /// New BlackList
        /// </summary>
        /// <param name="Name">Player Name</param>
        /// <param name="GameJoltID">Player GameJolt ID</param>
        /// <param name="Reason">Player Ban Reason</param>
        /// <param name="StartTime">Player Ban Start Time</param>
        /// <param name="Duration">Player Ban Duration</param>
        public BlackList(string Name, int GameJoltID, string Reason, DateTime StartTime, int Duration)
        {
            this.Name = Name;
            this.GameJoltID = GameJoltID;
            this.Reason = Reason;
            this.StartTime = StartTime;
            this.Duration = Duration;
        }
    }
}

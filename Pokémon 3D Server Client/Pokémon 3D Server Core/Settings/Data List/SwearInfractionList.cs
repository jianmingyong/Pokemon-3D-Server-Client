using System;

namespace Global
{
    /// <summary>
    /// Class containing SwearInfractionList data
    /// </summary>
    public class SwearInfractionList : CommonList
    {
        /// <summary>
        /// New SwearInfractionList
        /// </summary>
        /// <param name="Name">Player Name</param>
        /// <param name="GameJoltID">Player GameJolt ID</param>
        /// <param name="Points">Player Infraction Points</param>
        /// <param name="Muted">Player Mute Points</param>
        /// <param name="StartTime">Player Infraction StartTime</param>
        public SwearInfractionList(string Name,int GameJoltID,int Points,int Muted,DateTime StartTime)
        {
            this.Name = Name;
            this.GameJoltID = GameJoltID;
            this.Points = Points;
            this.Muted = Muted;
            this.StartTime = StartTime;
        }
    }
}

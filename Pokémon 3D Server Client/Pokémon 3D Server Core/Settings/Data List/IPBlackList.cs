using System;

namespace Global
{
    /// <summary>
    /// Class containing IPBlackList data
    /// </summary>
    public class IPBlackList : CommonList
    {
        /// <summary>
        /// New IPBlackList
        /// </summary>
        /// <param name="IPAddress">Player IPAddress</param>
        /// <param name="Reason">Player Ban Reason</param>
        /// <param name="StartTime">Player Ban StartTime</param>
        /// <param name="Duration">Player Ban Duration</param>
        public IPBlackList(string IPAddress, string Reason, DateTime StartTime, int Duration)
        {
            this.IPAddress = IPAddress;
            this.Reason = Reason;
            this.StartTime = StartTime;
            this.Duration = Duration;
        }
    }
}

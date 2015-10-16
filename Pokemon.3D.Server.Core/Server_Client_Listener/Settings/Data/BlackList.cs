using System;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data
{
    /// <summary>
    /// Class containing BlackList.
    /// </summary>
    public class BlackList : CommonData
    {
        /// <summary>
        /// Get/Set BlackList StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Get/Set BlackList Duration
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Get BlackList Remaining Time
        /// </summary>
        public string RemainingTime
        {
            get
            {
                string ReturnText = null;

                if (Duration > 0)
                {
                    if (StartTime.AddSeconds(Duration) > DateTime.Now)
                    {
                        TimeSpan RemainingTime = StartTime.AddSeconds(Duration) - DateTime.Now;

                        if (RemainingTime.Days > 1)
                        {
                            ReturnText = RemainingTime.Days.ToString() + " Days";
                        }
                        else if (RemainingTime.Days == 1)
                        {
                            ReturnText = "1 Day";
                        }
                        else
                        {
                            if (RemainingTime.Hours == 1)
                            {
                                ReturnText += "1 Hour ";
                            }
                            else if (RemainingTime.Hours > 1)
                            {
                                ReturnText += RemainingTime.Hours.ToString() + " Hour ";
                            }

                            if (RemainingTime.Minutes == 1)
                            {
                                ReturnText += "1 Minute ";
                            }
                            else if (RemainingTime.Minutes > 1)
                            {
                                ReturnText += RemainingTime.Minutes.ToString() + " Minutes ";
                            }
                            else if (RemainingTime.Minutes == 0 && RemainingTime.TotalSeconds > 60)
                            {
                                ReturnText += "0 Minute ";
                            }

                            if (RemainingTime.Seconds == 1)
                            {
                                ReturnText += "1 Second";
                            }
                            else if (RemainingTime.Seconds > 1)
                            {
                                ReturnText += RemainingTime.Seconds.ToString() + " Seconds";
                            }
                            else if (RemainingTime.Seconds == 0)
                            {
                                ReturnText += "0 Second";
                            }
                        }
                    }
                    else
                    {
                        ReturnText = null;
                    }
                }
                else
                {
                    ReturnText = "Permanent";
                }
                return ReturnText;
            }
        }

        /// <summary>
        /// New BlackList
        /// </summary>
        /// <param name="Name">Player Name</param>
        /// <param name="GameJoltID">Player GameJolt ID</param>
        /// <param name="Reason">BlackList Reason</param>
        /// <param name="StartTime">BlackList Start Time</param>
        /// <param name="Duration">BlackList Duration</param>
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
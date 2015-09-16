using System;
using System.Net;

namespace Global
{
    /// <summary>
    /// Class containing Common List Property
    /// </summary>
    public abstract class CommonList
    {
        /// <summary>
        /// Get/Set Player Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get/Set Player Gamejolt ID
        /// </summary>
        public int GameJoltID { get; set; }

        private IPAddress _IPAddress;
        /// <summary>
        /// Get/Set Player IPAddress
        /// </summary>
        public string IPAddress
        {
            get
            {
                return _IPAddress.ToString();
            }
            set
            {
                try
                {
                    _IPAddress = System.Net.IPAddress.Parse(value);
                }
                catch (Exception ex)
                {
                    ex.CatchError();
                }
            }
        }

        /// <summary>
        /// Get/Set Player Ban Reason
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Get/Set Player Ban Start Time
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Get/Set Player Ban Duration
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Get/Set Player Operator Level
        /// </summary>
        public int OperatorLevel { get; set; }

        /// <summary>
        /// Get/Set Player Infraction Points
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Get/Set Player Muted
        /// </summary>
        public int Muted { get; set; }

        /// <summary>
        /// Get Player Ban Remaining Time
        /// </summary>
        public string RemainingTime
        {
            get
            {
                TimeSpan RemainingTime;
                string ReturnText = null;

                if (Duration > 0)
                {
                    if (StartTime.AddSeconds(Duration) > DateTime.Now)
                    {
                        RemainingTime = StartTime.AddSeconds(Duration) - DateTime.Now;

                        if (RemainingTime.Days > 0)
                        {
                            if (RemainingTime.Days == 1)
                            {
                                ReturnText = "1 Day";
                            }
                            else
                            {
                                ReturnText = RemainingTime.Days.ToString() + " Day";
                            }
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
                            else if (RemainingTime.Seconds == 0 && RemainingTime.TotalSeconds > 0)
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
    }
}

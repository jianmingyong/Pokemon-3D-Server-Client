using System;
using System.Net;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data
{
    /// <summary>
    /// Class containing IPBlackList.
    /// </summary>
    public class IPBlackList : CommonData
    {
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
        /// Get/Set IPBlackList StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Get/Set IPBlackList Duration
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Get IPBlackList Remaining Time
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
        /// New IPBlackList
        /// </summary>
        /// <param name="IPAddress">Player IPAddress</param>
        /// <param name="Reason">IPBlackList Reason</param>
        /// <param name="StartTime">IPBlackList Start Time</param>
        /// <param name="Duration">IPBlackList Duration</param>
        public IPBlackList(string IPAddress, string Reason, DateTime StartTime, int Duration)
        {
            this.IPAddress = IPAddress;
            this.Reason = Reason;
            this.StartTime = StartTime;
            this.Duration = Duration;
        }
    }
}
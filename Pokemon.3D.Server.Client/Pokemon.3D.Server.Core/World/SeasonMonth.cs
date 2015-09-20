using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Modules;

namespace Pokemon_3D_Server_Core.Worlds
{
    /// <summary>
    /// Class containing Advanced Season
    /// </summary>
    public class SeasonMonth
    {
        /// <summary>
        /// Get/Set Season Data
        /// </summary>
        public string SeasonData { get; set; }

        /// <summary>
        /// Get/Set Season List
        /// </summary>
        public List<int> SeasonList { get; set; }

        /// <summary>
        /// New SeasonMonth
        /// </summary>
        public SeasonMonth()
        {
            SeasonData = null;
            SeasonList = new List<int> { -2 };
        }

        /// <summary>
        /// New SeasonMonth
        /// </summary>
        /// <param name="Data">SeasonMonth Data</param>
        public SeasonMonth(string Data)
        {
            if (Data.SplitCount() >= 12)
            {
                SeasonData = Data;
                SeasonList = GetSeason();
            }
        }

        private List<int> GetSeason()
        {
            var ReturnList = new List<int>();
            if (SeasonData.GetSplit(DateTime.Now.Month - 1).SplitCount(",") == 1)
            {
                try
                {
                    ReturnList.Add(SeasonData.GetSplit(DateTime.Now.Month - 1).Toint());
                }
                catch (Exception)
                {
                    ReturnList.Add(-2);
                }
            }
            else
            {
                try
                {
                    foreach (string Season in SeasonData.GetSplit(DateTime.Now.Month - 1).Split(",".ToCharArray()))
                    {
                        ReturnList.Add(Season.Toint());
                    }
                }
                catch (Exception)
                {
                    ReturnList.Add(-2);
                }
            }
            return ReturnList;
        }
    }
}

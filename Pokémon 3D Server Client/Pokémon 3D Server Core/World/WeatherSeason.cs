using System;
using System.Collections.Generic;

namespace Global
{
    /// <summary>
    /// Class containing Advanced Weather
    /// </summary>
    public class WeatherSeason
    {
        /// <summary>
        /// Get/Set Weather Data
        /// </summary>
        public string WeatherData { get; set; }

        /// <summary>
        /// Get/Set Weather List
        /// </summary>
        public List<int> WeatherList { get; set; }

        /// <summary>
        /// New WeatherSeason
        /// </summary>
        public WeatherSeason()
        {
            WeatherData = null;
            WeatherList = new List<int> { -2 };
        }

        /// <summary>
        /// New WeatherSeason
        /// </summary>
        public WeatherSeason(string Data)
        {
            if (Data.SplitCount() >= 12)
            {
                WeatherData = Data;
                //WeatherList = GetWeather();
            }
        }

        //private List<int> GetWeather()
        //{
        //    var ReturnList = new List<int>();
        //    if (WeatherData.GetSplit().SplitCount(",") == 1)
        //    {
        //        try
        //        {
        //            ReturnList.Add(WeatherData.GetSplit(DateTime.Now.Month - 1).Toint());
        //        }
        //        catch (Exception)
        //        {
        //            ReturnList.Add(-2);
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            foreach (string Season in SeasonData.GetSplit(DateTime.Now.Month - 1).Split(",".ToCharArray()))
        //            {
        //                ReturnList.Add(Season.Toint());
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            ReturnList.Add(-2);
        //        }
        //    }
        //    return ReturnList;
        //}
    }
}

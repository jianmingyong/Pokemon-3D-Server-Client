﻿using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Modules;

namespace Pokemon_3D_Server_Core.Worlds
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
            if (Data.SplitCount() >= 4)
            {
                WeatherData = Data;
                WeatherList = GetWeather();
            }
        }

        private List<int> GetWeather()
        {
            var ReturnList = new List<int>();
            if (WeatherData.SplitCount(",") == 1)
            {
                try
                {
                    ReturnList.Add(WeatherData.GetSplit(DateTime.Now.Month - 1).Toint());
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
                    foreach (string Weather in WeatherData.GetSplit(DateTime.Now.Month - 1).Split(",".ToCharArray()))
                    {
                        ReturnList.Add(Weather.Toint());
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
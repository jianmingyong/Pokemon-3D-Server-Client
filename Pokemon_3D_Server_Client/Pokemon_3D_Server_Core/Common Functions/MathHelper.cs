using System;
using System.Globalization;

namespace Global
{
    /// <summary>
    /// Class containing Math functions
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Get Current Culture Number Decimal Separator.
        /// </summary>
        public static readonly string CurrentCulture = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        /// <summary>
        /// Convert Math Value to current culture.
        /// </summary>
        /// <param name="value">The value to convert in string.</param>
        /// <param name="Player">The Player Culture.</param>
        public static string ConvertStringCulture(this string value, Player Player = null)
        {
            if (Player == null)
            {
                return value.Replace(".", CurrentCulture).Replace(",", CurrentCulture);
            }
            else
            {
                return value.Replace(".", Player.DecimalSeparator).Replace(",", Player.DecimalSeparator);
            }
        }

        /// <summary>
        /// Convert string to byte type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static byte Tobyte(this string value)
        {
            try
            {
                return byte.Parse(value.ConvertStringCulture());
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert string to decimal type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static decimal Todecimal(this string value)
        {
            try
            {
                return decimal.Parse(value.ConvertStringCulture());
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert string to double type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static double Todouble(this string value)
        {
            try
            {
                return double.Parse(value.ConvertStringCulture());
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert string to float type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static float Tofloat(this string value)
        {
            try
            {
                return float.Parse(value.ConvertStringCulture());
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert string to int type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static int Toint(this string value)
        {
            try
            {
                return int.Parse(value.ConvertStringCulture());
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert string to long type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static long Tolong(this string value)
        {
            try
            {
                return long.Parse(value.ConvertStringCulture());
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert string to sbyte type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static sbyte Tosbyte(this string value)
        {
            try
            {
                return sbyte.Parse(value.ConvertStringCulture());
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert string to short type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static short Toshort(this string value)
        {
            try
            {
                return short.Parse(value.ConvertStringCulture());
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert string to uint type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static uint Touint(this string value)
        {
            try
            {
                return uint.Parse(value.ConvertStringCulture());
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert string to ulong type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static ulong Toulong(this string value)
        {
            try
            {
                return ulong.Parse(value.ConvertStringCulture());
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert string to ushort type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static ushort Toushort(this string value)
        {
            try
            {
                return ushort.Parse(value.ConvertStringCulture());
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert integer to bool type.
        /// </summary>
        /// <param name="value">The int value to convert.</param>
        public static bool Tobool(this int value)
        {
            if (value <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Convert boolean to integer type.
        /// </summary>
        /// <param name="value">The bool value to convert.</param>
        public static int Tobool(this bool value)
        {
            if (value)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns></returns>
        public static int Random(this int minValue,int maxValue)
        {
            try
            {
                return new Random().Next(minValue, maxValue);
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }
    }
}

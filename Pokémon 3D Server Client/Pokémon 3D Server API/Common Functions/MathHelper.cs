using System;
using System.Globalization;

namespace Global
{
    /// <summary>
    /// Public Module MathHelper
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Convert string to byte type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static byte Tobyte(this string value)
        {
            try
            {
                return byte.Parse(value, CultureInfo.InvariantCulture);
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
                return decimal.Parse(value, CultureInfo.InvariantCulture);
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
                return double.Parse(value, CultureInfo.InvariantCulture);
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
                return float.Parse(value, CultureInfo.InvariantCulture);
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
                return int.Parse(value, CultureInfo.InvariantCulture);
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
                return long.Parse(value, CultureInfo.InvariantCulture);
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
                return sbyte.Parse(value, CultureInfo.InvariantCulture);
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
                return short.Parse(value, CultureInfo.InvariantCulture);
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
                return uint.Parse(value, CultureInfo.InvariantCulture);
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
                return ulong.Parse(value, CultureInfo.InvariantCulture);
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
                return ushort.Parse(value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
            return 0;
        }

        /// <summary>
        /// Convert string to bool type.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static bool Tobool(this string value)
        {
            if (value.Toint() <= 0)
            {
                return false;
            }
            else
            {
                return true;
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

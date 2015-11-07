using System;
using System.Globalization;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;

namespace Pokemon_3D_Server_Core.Shared.jianmingyong.Modules
{
    /// <summary>
    /// Class containing math functions.
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
        /// Converts the string representation of a number to its <see cref="byte"/> equivalent.
        /// </summary>
        /// <param name="value">A string that contains a number to convert. The string is interpreted using the <see cref="NumberStyles.Integer"/> style.</param>
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        public static byte ToByte(this string value, byte DefaultValue = 0)
        {
            try
            {
                return byte.Parse(value.ConvertStringCulture());
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="decimal"/> equivalent.
        /// </summary>
        /// <param name="value">The string representation of the number to convert.</param>
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        public static decimal ToDecimal(this string value, decimal DefaultValue = 0)
        {
            try
            {
                return decimal.Parse(value.ConvertStringCulture());
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its double-precision floating-point number equivalent.
        /// </summary>
        /// <param name="value">A string that contains a number to convert.</param>
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        public static double ToDouble(this string value, double DefaultValue = 0.0)
        {
            try
            {
                return double.Parse(value.ConvertStringCulture());
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its single-precision floating-point number equivalent.
        /// </summary>
        /// <param name="value">A string that contains a number to convert.</param>
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        public static float ToFloat(this string value, float DefaultValue = 0)
        {
            try
            {
                return float.Parse(value.ConvertStringCulture());
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="value">A string containing a number to convert.</param>
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        public static int ToInt(this string value, int DefaultValue = 0)
        {
            try
            {
                return int.Parse(value.ConvertStringCulture());
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its 64-bit signed integer equivalent.
        /// </summary>
        /// <param name="value">A string containing a number to convert.</param>
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        public static long ToLong(this string value, long DefaultValue = 0)
        {
            try
            {
                return long.Parse(value.ConvertStringCulture());
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its 8-bit signed integer equivalent.
        /// </summary>
        /// <param name="value">A string that represents a number to convert. The string is interpreted using the <see cref="NumberStyles.Integer"/> style.</param>
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        public static sbyte ToSbyte(this string value, sbyte DefaultValue = 0)
        {
            try
            {
                return sbyte.Parse(value.ConvertStringCulture());
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its 16-bit signed integer equivalent.
        /// </summary>
        /// <param name="value">A string containing a number to convert.</param>
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        public static short ToShort(this string value, short DefaultValue = 0)
        {
            try
            {
                return short.Parse(value.ConvertStringCulture());
            }
            catch (Exception)
            {
                return DefaultValue;
            }
            
        }

        /// <summary>
        /// Converts the string representation of a number to its 32-bit unsigned integer equivalent.
        /// </summary>
        /// <param name="value">A string representing the number to convert.</param>
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        public static uint ToUint(this string value, uint DefaultValue = 0)
        {
            try
            {
                return uint.Parse(value.ConvertStringCulture());
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its 64-bit unsigned integer equivalent.
        /// </summary>
        /// <param name="value">A string that represents the number to convert.</param>
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        public static ulong ToUlong(this string value, ulong DefaultValue = 0)
        {
            try
            {
                return ulong.Parse(value.ConvertStringCulture());
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its 16-bit unsigned integer equivalent.
        /// </summary>
        /// <param name="value">A string that represents the number to convert.</param>
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        public static ushort ToUshort(this string value, ushort DefaultValue = 0)
        {
            try
            {
                return ushort.Parse(value.ConvertStringCulture());
            }
            catch (Exception)
            {
                return DefaultValue;
            }
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
        /// <param name="DefaultValue">The default value returned if the conversion fails.</param>
        /// <returns></returns>
        public static int Random(int minValue, int maxValue, int DefaultValue = 0)
        {
            try
            {
                return new Random().Next(minValue, maxValue);
            }
            catch (Exception)
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Clamp the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static byte Clamp(this byte Value, byte minValue, byte maxValue)
        {
            if (Value < minValue)
            {
                return minValue;
            }
            else if (Value > maxValue)
            {
                return maxValue;
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// Clamp the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static decimal Clamp(this decimal Value, decimal minValue, decimal maxValue)
        {
            if (Value < minValue)
            {
                return minValue;
            }
            else if (Value > maxValue)
            {
                return maxValue;
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// Clamp the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static double Clamp(this double Value, double minValue, double maxValue)
        {
            if (Value < minValue)
            {
                return minValue;
            }
            else if (Value > maxValue)
            {
                return maxValue;
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// Clamp the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static float Clamp(this float Value, float minValue, float maxValue)
        {
            if (Value < minValue)
            {
                return minValue;
            }
            else if (Value > maxValue)
            {
                return maxValue;
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// Clamp the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static int Clamp(this int Value, int minValue, int maxValue)
        {
            if (Value < minValue)
            {
                return minValue;
            }
            else if (Value > maxValue)
            {
                return maxValue;
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// Clamp the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static long Clamp(this long Value, long minValue, long maxValue)
        {
            if (Value < minValue)
            {
                return minValue;
            }
            else if (Value > maxValue)
            {
                return maxValue;
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// Clamp the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static sbyte Clamp(this sbyte Value, sbyte minValue, sbyte maxValue)
        {
            if (Value < minValue)
            {
                return minValue;
            }
            else if (Value > maxValue)
            {
                return maxValue;
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// Clamp the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static short Clamp(this short Value, short minValue, short maxValue)
        {
            if (Value < minValue)
            {
                return minValue;
            }
            else if (Value > maxValue)
            {
                return maxValue;
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// Clamp the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static uint Clamp(this uint Value, uint minValue, uint maxValue)
        {
            if (Value < minValue)
            {
                return minValue;
            }
            else if (Value > maxValue)
            {
                return maxValue;
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// Clamp the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static ulong Clamp(this ulong Value, ulong minValue, ulong maxValue)
        {
            if (Value < minValue)
            {
                return minValue;
            }
            else if (Value > maxValue)
            {
                return maxValue;
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// Clamp the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to clamp.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static ushort Clamp(this ushort Value, ushort minValue, ushort maxValue)
        {
            if (Value < minValue)
            {
                return minValue;
            }
            else if (Value > maxValue)
            {
                return maxValue;
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// RollOver the value between the minValue and the maxValue.
        /// </summary>
        /// <param name="Value">The value to rollover.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public static int RollOver(this int Value, int minValue, int maxValue)
        {
            int Diff = maxValue - minValue + 1;
            int NewValue = Value;

            if (Value > maxValue)
            {
                while (NewValue > maxValue)
                {
                    NewValue -= Diff;
                }
            }
            else if (Value < minValue)
            {
                while (NewValue < maxValue)
                {
                    NewValue += Diff;
                }
            }

            return NewValue;
        }
    }
}
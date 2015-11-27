using System;
using System.Globalization;

namespace Pokemon_3D_Server_Core.Shared.jianmingyong.Modules
{
    /// <summary>
    /// Class containing all basic math functions.
    /// </summary>
    public static class Math
    {
        /// <summary>
        /// Represents the natural logarithmic base, specified by the constant, e.
        /// </summary>
        public static readonly double E = System.Math.E.ToString().ToDouble();

        /// <summary>
        /// Represents the ratio of the circumference of a circle to its diameter, specified by the constant, π.
        /// </summary>
        public static readonly double PI = System.Math.PI.ToString().ToDouble();

        /// <summary>
        /// Returns the absolute value of a Decimal number.
        /// </summary>
        /// <param name="value">A number that is greater than or equal to Decimal.MinValue, but less than or equal to Decimal.MaxValue.</param>
        public static decimal Abs(this decimal value)
        {
            return System.Math.Abs(value.ToString().ToDecimal());
        }

        /// <summary>
        /// Returns the absolute value of a double-precision floating-point number.
        /// </summary>
        /// <param name="value">A number that is greater than or equal to Double.MinValue, but less than or equal to Double.MaxValue.</param>
        public static double Abs(this double value)
        {
            return System.Math.Abs(value.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the absolute value of a single-precision floating-point number.
        /// </summary>
        /// <param name="value">A number that is greater than or equal to Single.MinValue, but less than or equal to Single.MaxValue.</param>
        public static float Abs(this float value)
        {
            return System.Math.Abs(value.ToString().ToFloat());
        }

        /// <summary>
        /// Returns the absolute value of a 32-bit signed integer.
        /// </summary>
        /// <param name="value">A number that is greater than Int32.MinValue, but less than or equal to Int32.MaxValue.</param>
        public static int Abs(this int value)
        {
            return System.Math.Abs(value.ToString().ToInt());
        }

        /// <summary>
        /// Returns the absolute value of a 64-bit signed integer.
        /// </summary>
        /// <param name="value">A number that is greater than Int64.MinValue, but less than or equal to Int64.MaxValue.</param>
        public static long Abs(this long value)
        {
            return System.Math.Abs(value.ToString().ToLong());
        }

        /// <summary>
        /// Returns the absolute value of an 8-bit signed integer.
        /// </summary>
        /// <param name="value">A number that is greater than SByte.MinValue, but less than or equal to SByte.MaxValue.</param>
        public static sbyte Abs(this sbyte value)
        {
            return System.Math.Abs(value.ToString().ToSbyte());
        }

        /// <summary>
        /// Returns the absolute value of a 16-bit signed integer.
        /// </summary>
        /// <param name="value">A number that is greater than Int16.MinValue, but less than or equal to Int16.MaxValue.</param>
        public static short Abs(this short value)
        {
            return System.Math.Abs(value.ToString().ToShort());
        }

        /// <summary>
        /// Returns the angle whose cosine is the specified number.
        /// </summary>
        /// <param name="d">A number representing a cosine, where d must be greater than or equal to -1, but less than or equal to 1.</param>
        public static double Acos(this double d)
        {
            return System.Math.Acos(d.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the angle whose sine is the specified number.
        /// </summary>
        /// <param name="d">A number representing a sine, where d must be greater than or equal to -1, but less than or equal to 1.</param>
        public static double Asin(this double d)
        {
            return System.Math.Asin(d.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the angle whose tangent is the specified number.
        /// </summary>
        /// <param name="d">A number representing a tangent.</param>
        public static double Atan(this double d)
        {
            return System.Math.Atan(d.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the angle whose tangent is the quotient of two specified numbers.
        /// </summary>
        /// <param name="y">The y coordinate of a point.</param>
        /// <param name="x">The x coordinate of a point.</param>
        public static double Atan2(this double y, double x)
        {
            return System.Math.Atan2(y.ToString().ToDouble(), x.ToString().ToDouble());
        }

        /// <summary>
        /// Produces the full product of two 32-bit numbers.
        /// </summary>
        /// <param name="a">The first number to multiply.</param>
        /// <param name="b">The second number to multiply.</param>
        public static long BigMul(this int a, int b)
        {
            return System.Math.BigMul(a.ToString().ToInt(), b.ToString().ToInt());
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified decimal number.
        /// </summary>
        /// <param name="d">A decimal number.</param>
        public static decimal Ceiling(this decimal d)
        {
            return System.Math.Ceiling(d.ToString().ToDecimal());
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified double-precision floating-point number.
        /// </summary>
        /// <param name="a">A double-precision floating-point number.</param>
        public static double Ceiling(this double a)
        {
            return System.Math.Ceiling(a.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        /// <param name="d">An angle, measured in radians.</param>
        public static double Cos(this double d)
        {
            return System.Math.Cos(d.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the hyperbolic cosine of the specified angle.
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        public static double Cosh(this double value)
        {
            return System.Math.Cosh(value.ToString().ToDouble());
        }

        /// <summary>
        /// Calculates the quotient of two 32-bit signed integers and also returns the remainder in an output parameter.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <param name="result">The remainder.</param>
        public static int DivRem(this int a, int b, out int result)
        {
            System.Math.DivRem(a.ToString().ToInt(), b.ToString().ToInt(), out result);
            return result.ToString().ToInt();
        }

        /// <summary>
        /// Calculates the quotient of two 64-bit signed integers and also returns the remainder in an output parameter.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <param name="result">The remainder.</param>
        public static long DivRem(this long a, long b, out long result)
        {
            System.Math.DivRem(long.Parse(a.ToString(), CultureInfo.InvariantCulture), long.Parse(b.ToString(), CultureInfo.InvariantCulture), out result);
            return result.ToString().ToLong();
        }

        /// <summary>
        /// Returns e raised to the specified power.
        /// </summary>
        /// <param name="d">A number specifying a power.</param>
        public static double Exp(this double d)
        {
            return System.Math.Exp(d.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the largest integer less than or equal to the specified decimal number.
        /// </summary>
        /// <param name="d">A decimal number.</param>
        public static decimal Floor(this decimal d)
        {
            return System.Math.Floor(d.ToString().ToDecimal());
        }

        /// <summary>
        /// Returns the largest integer less than or equal to the specified double-precision floating-point number.
        /// </summary>
        /// <param name="d">A double-precision floating-point number.</param>
        public static double Floor(this double d)
        {
            return System.Math.Floor(d.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the remainder resulting from the division of a specified number by another specified number.
        /// </summary>
        /// <param name="x">A dividend.</param>
        /// <param name="y">A divisor.</param>
        public static double IEEERemainder(this double x, double y)
        {
            return System.Math.IEEERemainder(x.ToString().ToDouble(), y.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the natural (base e) logarithm of a specified number.
        /// </summary>
        /// <param name="d">The number whose logarithm is to be found.</param>
        public static double Log(this double d)
        {
            return System.Math.Log(d.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the logarithm of a specified number in a specified base.
        /// </summary>
        /// <param name="a">The number whose logarithm is to be found.</param>
        /// <param name="newBase">The base of the logarithm.</param>
        public static double Log(this double a, double newBase)
        {
            return System.Math.Log(a.ToString().ToDouble(), newBase.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the base 10 logarithm of a specified number.
        /// </summary>
        /// <param name="d">The number whose logarithm is to be found.</param>
        public static double Log10(this double d)
        {
            return System.Math.Log10(d.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the larger of two 8-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
        public static byte Max(this byte val1, byte val2)
        {
            return System.Math.Max(val1.ToString().ToByte(), val2.ToString().ToByte());
        }

        /// <summary>
        /// Returns the larger of two decimal numbers.
        /// </summary>
        /// <param name="val1">The first of two decimal numbers to compare.</param>
        /// <param name="val2">The second of two decimal numbers to compare.</param>
        public static decimal Max(this decimal val1, decimal val2)
        {
            return System.Math.Max(val1.ToString().ToDecimal(), val2.ToString().ToDecimal());
        }

        /// <summary>
        /// Returns the larger of two double-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
        public static double Max(this double val1, double val2)
        {
            return System.Math.Max(val1.ToString().ToDouble(), val2.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the larger of two single-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
        public static float Max(this float val1, float val2)
        {
            return System.Math.Max(val1.ToString().ToFloat(), val2.ToString().ToFloat());
        }

        /// <summary>
        /// Returns the larger of two 32-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 32-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 32-bit signed integers to compare.</param>
        public static int Max(this int val1, int val2)
        {
            return System.Math.Max(val1.ToString().ToInt(), val2.ToString().ToInt());
        }

        /// <summary>
        /// Returns the larger of two 64-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 64-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 64-bit signed integers to compare.</param>
        public static long Max(this long val1, long val2)
        {
            return System.Math.Max(val1.ToString().ToLong(), val2.ToString().ToLong());
        }

        /// <summary>
        /// Returns the larger of two 8-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 8-bit signed integers to compare.</param>
        public static sbyte Max(this sbyte val1, sbyte val2)
        {
            return System.Math.Max(val1.ToString().ToSbyte(), val2.ToString().ToSbyte());
        }

        /// <summary>
        /// Returns the larger of two 16-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 16-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 16-bit signed integers to compare.</param>
        public static short Max(this short val1, short val2)
        {
            return System.Math.Max(val1.ToString().ToShort(), val2.ToString().ToShort());
        }

        /// <summary>
        /// Returns the larger of two 32-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 32-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 32-bit unsigned integers to compare.</param>
        public static uint Max(this uint val1, uint val2)
        {
            return System.Math.Max(val1.ToString().ToUint(), val2.ToString().ToUint());
        }

        /// <summary>
        /// Returns the larger of two 64-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 64-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 64-bit unsigned integers to compare.</param>
        public static ulong Max(this ulong val1, ulong val2)
        {
            return System.Math.Max(val1.ToString().ToUlong(), val2.ToString().ToUlong());
        }

        /// <summary>
        /// Returns the larger of two 16-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 16-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 16-bit unsigned integers to compare.</param>
        public static ushort Max(this ushort val1, ushort val2)
        {
            return System.Math.Max(val1.ToString().ToUshort(), val2.ToString().ToUshort());
        }

        /// <summary>
        /// Returns the smaller of two 8-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
        public static byte Min(this byte val1, byte val2)
        {
            return System.Math.Min(val1.ToString().ToByte(), val2.ToString().ToByte());
        }

        /// <summary>
        /// Returns the smaller of two decimal numbers.
        /// </summary>
        /// <param name="val1">The first of two decimal numbers to compare.</param>
        /// <param name="val2">The second of two decimal numbers to compare.</param>
        public static decimal Min(this decimal val1, decimal val2)
        {
            return System.Math.Min(val1.ToString().ToDecimal(), val2.ToString().ToDecimal());
        }

        /// <summary>
        /// Returns the smaller of two double-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
        public static double Min(this double val1, double val2)
        {
            return System.Math.Min(val1.ToString().ToDouble(), val2.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the smaller of two single-precision floating-point numbers.
        /// </summary>
        /// <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
        /// <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
        public static float Min(this float val1, float val2)
        {
            return System.Math.Min(val1.ToString().ToFloat(), val2.ToString().ToFloat());
        }

        /// <summary>
        /// Returns the smaller of two 32-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 32-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 32-bit signed integers to compare.</param>
        public static int Min(this int val1, int val2)
        {
            return System.Math.Min(val1.ToString().ToInt(), val2.ToString().ToInt());
        }

        /// <summary>
        /// Returns the smaller of two 64-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 64-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 64-bit signed integers to compare.</param>
        public static long Min(this long val1, long val2)
        {
            return System.Math.Min(val1.ToString().ToLong(), val2.ToString().ToLong());
        }

        /// <summary>
        /// Returns the smaller of two 8-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 8-bit signed integers to compare.</param>
        public static sbyte Min(this sbyte val1, sbyte val2)
        {
            return System.Math.Min(val1.ToString().ToSbyte(), val2.ToString().ToSbyte());
        }

        /// <summary>
        /// Returns the smaller of two 16-bit signed integers.
        /// </summary>
        /// <param name="val1">The first of two 16-bit signed integers to compare.</param>
        /// <param name="val2">The second of two 16-bit signed integers to compare.</param>
        public static short Min(this short val1, short val2)
        {
            return System.Math.Min(val1.ToString().ToShort(), val2.ToString().ToShort());
        }

        /// <summary>
        /// Returns the smaller of two 32-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 32-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 32-bit unsigned integers to compare.</param>
        public static uint Min(this uint val1, uint val2)
        {
            return System.Math.Min(val1.ToString().ToUint(), val2.ToString().ToUint());
        }

        /// <summary>
        /// Returns the smaller of two 64-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 64-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 64-bit unsigned integers to compare.</param>
        public static ulong Min(this ulong val1, ulong val2)
        {
            return System.Math.Min(val1.ToString().ToUlong(), val2.ToString().ToUlong());
        }

        /// <summary>
        /// Returns the smaller of two 16-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 16-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 16-bit unsigned integers to compare.</param>
        public static ushort Min(this ushort val1, ushort val2)
        {
            return System.Math.Min(val1.ToString().ToUshort(), val2.ToString().ToUshort());
        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// </summary>
        /// <param name="x">A double-precision floating-point number to be raised to a power.</param>
        /// <param name="y">A double-precision floating-point number that specifies a power.</param>
        public static double Pow(this double x, double y)
        {
            return System.Math.Pow(x.ToString().ToDouble(), y.ToString().ToDouble());
        }

        /// <summary>
        /// Rounds a decimal value to the nearest integral value.
        /// </summary>
        /// <param name="d">A decimal number to be rounded.</param>
        public static decimal Round(this decimal d)
        {
            return System.Math.Round(d.ToString().ToDecimal());
        }

        /// <summary>
        /// Rounds a double-precision floating-point value to the nearest integral value.
        /// </summary>
        /// <param name="d">A double-precision floating-point number to be rounded.</param>
        public static double Round(this double d)
        {
            return System.Math.Round(d.ToString().ToDouble());
        }

        /// <summary>
        /// Rounds a decimal value to a specified number of fractional digits.
        /// </summary>
        /// <param name="d">A decimal number to be rounded.</param>
        /// <param name="decimals">The number of decimal places in the return value.</param>
        public static decimal Round(this decimal d, int decimals)
        {
            return System.Math.Round(d.ToString().ToDecimal(), decimals.ToString().ToInt());
        }

        /// <summary>
        /// Rounds a decimal value to the nearest integer. A parameter specifies how to round the value if it is midway between two numbers.
        /// </summary>
        /// <param name="d">A decimal number to be rounded.</param>
        /// <param name="mode">Specification for how to round d if it is midway between two other numbers.</param>
        public static decimal Round(this decimal d, MidpointRounding mode)
        {
            return System.Math.Round(d.ToString().ToDecimal(), mode);
        }

        /// <summary>
        /// Rounds a double-precision floating-point value to a specified number of fractional digits.
        /// </summary>
        /// <param name="value">A double-precision floating-point number to be rounded.</param>
        /// <param name="digits">The number of decimal places in the return value.</param>
        public static double Round(this double value, int digits)
        {
            return System.Math.Round(value.ToString().ToDouble(), digits.ToString().ToInt());
        }

        /// <summary>
        /// Rounds a double-precision floating-point value to the nearest integer. A parameter specifies how to round the value if it is midway between two numbers.
        /// </summary>
        /// <param name="value">A double-precision floating-point number to be rounded.</param>
        /// <param name="mode">Specification for how to round d if it is midway between two other numbers.</param>
        public static double Round(this double value, MidpointRounding mode)
        {
            return System.Math.Round(value.ToString().ToDouble(), mode);
        }

        /// <summary>
        /// Rounds a decimal value to a specified number of fractional digits. A parameter specifies how to round the value if it is midway between two numbers.
        /// </summary>
        /// <param name="d">A decimal number to be rounded.</param>
        /// <param name="decimals">The number of decimal places in the return value.</param>
        /// <param name="mode">Specification for how to round d if it is midway between two other numbers.</param>
        public static decimal Round(this decimal d, int decimals, MidpointRounding mode)
        {
            return System.Math.Round(d.ToString().ToDecimal(), decimals.ToString().ToInt(), mode);
        }

        /// <summary>
        /// Rounds a decimal value to a specified number of fractional digits. A parameter specifies how to round the value if it is midway between two numbers.
        /// </summary>
        /// <param name="value">A double-precision floating-point number to be rounded.</param>
        /// <param name="digits">The number of fractional digits in the return value.</param>
        /// <param name="mode">Specification for how to round d if it is midway between two other numbers.</param>
        public static double Round(this double value, int digits, MidpointRounding mode)
        {
            return System.Math.Round(value.ToString().ToDouble(), digits.ToString().ToInt(), mode);
        }

        /// <summary>
        /// Returns a value indicating the sign of a decimal number.
        /// </summary>
        /// <param name="value">A signed decimal number.</param>
        public static int Sign(this decimal value)
        {
            return System.Math.Sign(value.ToString().ToDecimal());
        }

        /// <summary>
        /// Returns a value indicating the sign of a double-precision floating-point number.
        /// </summary>
        /// <param name="value">A signed number.</param>
        public static int Sign(this double value)
        {
            return System.Math.Sign(value.ToString().ToDouble());
        }

        /// <summary>
        /// Returns a value indicating the sign of a single-precision floating-point number.
        /// </summary>
        /// <param name="value">A signed number.</param>
        public static int Sign(this float value)
        {
            return System.Math.Sign(value.ToString().ToFloat());
        }

        /// <summary>
        /// Returns a value indicating the sign of a 32-bit signed integer.
        /// </summary>
        /// <param name="value">A signed number.</param>
        public static int Sign(this int value)
        {
            return System.Math.Sign(value.ToString().ToInt());
        }

        /// <summary>
        /// Returns a value indicating the sign of a 64-bit signed integer.
        /// </summary>
        /// <param name="value">A signed number.</param>
        public static int Sign(this long value)
        {
            return System.Math.Sign(value.ToString().ToLong());
        }

        /// <summary>
        /// Returns a value indicating the sign of an 8-bit signed integer.
        /// </summary>
        /// <param name="value">A signed number.</param>
        public static int Sign(this sbyte value)
        {
            return System.Math.Sign(value.ToString().ToSbyte());
        }

        /// <summary>
        /// Returns a value indicating the sign of a 16-bit signed integer.
        /// </summary>
        /// <param name="value">A signed number.</param>
        public static int Sign(this short value)
        {
            return System.Math.Sign(value.ToString().ToShort());
        }

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        /// <param name="a">An angle, measured in radians.</param>
        public static double Sin(this double a)
        {
            return System.Math.Sin(a.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the hyperbolic sine of the specified angle.
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        public static double Sinh(this double value)
        {
            return System.Math.Sinh(value.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the square root of a specified number.
        /// </summary>
        /// <param name="d">The number whose square root is to be found.</param>
        public static double Sqrt(this double d)
        {
            return System.Math.Sqrt(d.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the tangent of the specified angle.
        /// </summary>
        /// <param name="a">An angle, measured in radians.</param>
        public static double Tan(this double a)
        {
            return System.Math.Tan(a.ToString().ToDouble());
        }

        /// <summary>
        /// Returns the hyperbolic tangent of the specified angle.
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        public static double Tanh(this double value)
        {
            return System.Math.Tanh(value.ToString().ToDouble());
        }

        /// <summary>
        /// Calculates the integral part of a specified decimal number.
        /// </summary>
        /// <param name="d">A number to truncate.</param>
        public static decimal Truncate(this decimal d)
        {
            return System.Math.Truncate(d.ToString().ToDecimal());
        }

        /// <summary>
        /// Calculates the integral part of a specified double-precision floating-point number.
        /// </summary>
        /// <param name="d">A number to truncate.</param>
        public static double Truncate(this double d)
        {
            return System.Math.Truncate(d.ToString().ToDouble());
        }
    }
}
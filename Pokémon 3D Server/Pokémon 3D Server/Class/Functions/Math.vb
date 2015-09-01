Imports System.Runtime.CompilerServices

Public Module Math

    ''' <summary>
    ''' Represents the natural logarithmic base, specified by the constant, e.
    ''' </summary>
    Public ReadOnly Property E As Double = Double.Parse("2.7182818284590452354".Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator))

    ''' <summary>
    ''' Represents the ratio of the circumference of a circle to its diameter, specified by the constant, π.
    ''' </summary>
    Public ReadOnly Property PI As Double = Double.Parse("3.14159265358979323846".Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator))

    ''' <summary>
    ''' Returns the absolute value of a Decimal number.
    ''' </summary>
    ''' <param name="value">A number that is greater than or equal to Decimal.MinValue, but less than or equal to Decimal.MaxValue.</param>
    <Extension>
    Public Function Abs(ByVal value As Decimal) As Decimal
        Return System.Math.Abs(Decimal.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the absolute value of a double-precision floating-point number.
    ''' </summary>
    ''' <param name="value">A number that is greater than or equal to Double.MinValue, but less than or equal to Double.MaxValue.</param>
    <Extension>
    Public Function Abs(ByVal value As Double) As Double
        Return System.Math.Abs(Double.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the absolute value of a 16-bit signed integer.
    ''' </summary>
    ''' <param name="value">A number that is greater than Int16.MinValue, but less than or equal to Int16.MaxValue.</param>
    <Extension>
    Public Function Abs(ByVal value As Short) As Short
        Return System.Math.Abs(Short.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the absolute value of a 32-bit signed integer.
    ''' </summary>
    ''' <param name="value">A number that is greater than Int32.MinValue, but less than or equal to Int32.MaxValue.</param>
    <Extension>
    Public Function Abs(ByVal value As Integer) As Integer
        Return System.Math.Abs(Integer.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the absolute value of a 64-bit signed integer.
    ''' </summary>
    ''' <param name="value">A number that is greater than Int64.MinValue, but less than or equal to Int64.MaxValue.</param>
    <Extension>
    Public Function Abs(ByVal value As Long) As Long
        Return System.Math.Abs(Long.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the absolute value of an 8-bit signed integer.
    ''' </summary>
    ''' <param name="value">A number that is greater than SByte.MinValue, but less than or equal to SByte.MaxValue.</param>
    <Extension>
    Public Function Abs(ByVal value As SByte) As SByte
        Return System.Math.Abs(SByte.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the absolute value of a single-precision floating-point number.
    ''' </summary>
    ''' <param name="value">A number that is greater than or equal to Single.MinValue, but less than or equal to Single.MaxValue.</param>
    <Extension>
    Public Function Abs(ByVal value As Single) As Single
        Return System.Math.Abs(Single.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the angle whose cosine is the specified number.
    ''' </summary>
    ''' <param name="d">A number representing a cosine, where d must be greater than or equal to -1, but less than or equal to 1.</param>
    <Extension>
    Public Function Acos(ByVal d As Double) As Double
        Return System.Math.Acos(Double.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the angle whose sine is the specified number.
    ''' </summary>
    ''' <param name="d">A number representing a sine, where d must be greater than or equal to -1, but less than or equal to 1.</param>
    <Extension>
    Public Function Asin(ByVal d As Double) As Double
        Return System.Math.Asin(Double.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the angle whose tangent is the specified number.
    ''' </summary>
    ''' <param name="d">A number representing a tangent.</param>
    <Extension>
    Public Function Atan(ByVal d As Double) As Double
        Return System.Math.Atan(Double.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the angle whose tangent is the quotient of two specified numbers.
    ''' </summary>
    ''' <param name="y">The y coordinate of a point.</param>
    ''' <param name="x">The x coordinate of a point.</param>
    <Extension>
    Public Function Atan2(ByVal y As Double, ByVal x As Double) As Double
        Return System.Math.Atan2(Double.Parse(y.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Double.Parse(x.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Produces the full product of two 32-bit numbers.
    ''' </summary>
    ''' <param name="a">The first number to multiply.</param>
    ''' <param name="b">The second number to multiply.</param>
    <Extension>
    Public Function BigMul(ByVal a As Integer, ByVal b As Integer) As Long
        Return System.Math.BigMul(Integer.Parse(a.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Integer.Parse(b.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smallest integral value that is greater than or equal to the specified decimal number.
    ''' </summary>
    ''' <param name="d">A decimal number.</param>
    <Extension>
    Public Function Ceiling(ByVal d As Decimal) As Decimal
        Return System.Math.Ceiling(Decimal.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smallest integral value that is greater than or equal to the specified double-precision floating-point number.
    ''' </summary>
    ''' <param name="a">A double-precision floating-point number.</param>
    <Extension>
    Public Function Ceiling(ByVal a As Double) As Double
        Return System.Math.Ceiling(Double.Parse(a.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the cosine of the specified angle.
    ''' </summary>
    ''' <param name="d">An angle, measured in radians.</param>
    <Extension>
    Public Function Cos(ByVal d As Double) As Double
        Return System.Math.Cos(Double.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the hyperbolic cosine of the specified angle.
    ''' </summary>
    ''' <param name="value">An angle, measured in radians.</param>
    <Extension>
    Public Function Cosh(ByVal value As Double) As Double
        Return System.Math.Cosh(Double.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Calculates the quotient of two 32-bit signed integers and also returns the remainder in an output parameter.
    ''' </summary>
    ''' <param name="a">The dividend.</param>
    ''' <param name="b">The divisor.</param>
    ''' <param name="result">The remainder.</param>
    <Extension>
    Public Function DivRem(ByVal a As Integer, ByVal b As Integer, ByRef result As Integer) As Integer
        Return System.Math.DivRem(Integer.Parse(a.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Integer.Parse(b.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Integer.Parse(result.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Calculates the quotient of two 64-bit signed integers and also returns the remainder in an output parameter.
    ''' </summary>
    ''' <param name="a">The dividend.</param>
    ''' <param name="b">The divisor.</param>
    ''' <param name="result">The remainder.</param>
    <Extension>
    Public Function DivRem(ByVal a As Long, ByVal b As Long, ByRef result As Long) As Long
        Return System.Math.DivRem(Long.Parse(a.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Long.Parse(b.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Long.Parse(result.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns e raised to the specified power.
    ''' </summary>
    ''' <param name="d">A number specifying a power.</param>
    <Extension>
    Public Function Exp(ByVal d As Double) As Double
        Return System.Math.Exp(Double.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the largest integer less than or equal to the specified decimal number.
    ''' </summary>
    ''' <param name="d">A decimal number.</param>
    <Extension>
    Public Function Floor(ByVal d As Decimal) As Decimal
        Return System.Math.Floor(Decimal.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the largest integer less than or equal to the specified double-precision floating-point number.
    ''' </summary>
    ''' <param name="d">A double-precision floating-point number.</param>
    <Extension>
    Public Function Floor(ByVal d As Double) As Double
        Return System.Math.Floor(Double.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the remainder resulting from the division of a specified number by another specified number.
    ''' </summary>
    ''' <param name="x">A dividend.</param>
    ''' <param name="y">A divisor.</param>
    <Extension>
    Public Function IEEERemainder(ByVal x As Double, ByVal y As Double) As Double
        Return System.Math.IEEERemainder(Double.Parse(x.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Double.Parse(y.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the natural (base e) logarithm of a specified number.
    ''' </summary>
    ''' <param name="d">The number whose logarithm is to be found.</param>
    <Extension>
    Public Function Log(ByVal d As Double) As Double
        Return System.Math.Log(Double.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the logarithm of a specified number in a specified base.
    ''' </summary>
    ''' <param name="a">The number whose logarithm is to be found.</param>
    ''' <param name="newBase">The base of the logarithm.</param>
    <Extension>
    Public Function Log(ByVal a As Double, ByVal newBase As Double) As Double
        Return System.Math.Log(Double.Parse(a.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Double.Parse(newBase.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the base 10 logarithm of a specified number.
    ''' </summary>
    ''' <param name="d">A number whose logarithm is to be found.</param>
    <Extension>
    Public Function Log10(ByVal d As Double) As Double
        Return System.Math.Log10(Double.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the larger of two 8-bit unsigned integers.
    ''' </summary>
    ''' <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
    ''' <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
    <Extension>
    Public Function Max(ByVal val1 As Byte, ByVal val2 As Byte) As Byte
        Return System.Math.Max(Byte.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Byte.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the larger of two decimal numbers.
    ''' </summary>
    ''' <param name="val1">The first of two decimal numbers to compare.</param>
    ''' <param name="val2">The second of two decimal numbers to compare.</param>
    <Extension>
    Public Function Max(ByVal val1 As Decimal, ByVal val2 As Decimal) As Decimal
        Return System.Math.Max(Decimal.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Decimal.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the larger of two double-precision floating-point numbers.
    ''' </summary>
    ''' <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
    ''' <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
    <Extension>
    Public Function Max(ByVal val1 As Double, ByVal val2 As Double) As Double
        Return System.Math.Max(Double.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Double.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the larger of two 16-bit signed integers.
    ''' </summary>
    ''' <param name="val1">The first of two 16-bit signed integers to compare.</param>
    ''' <param name="val2">The second of two 16-bit signed integers to compare.</param>
    <Extension>
    Public Function Max(ByVal val1 As Short, ByVal val2 As Short) As Short
        Return System.Math.Max(Short.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Short.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the larger of two 32-bit signed integers.
    ''' </summary>
    ''' <param name="val1">The first of two 32-bit signed integers to compare.</param>
    ''' <param name="val2">The second of two 32-bit signed integers to compare.</param>
    <Extension>
    Public Function Max(ByVal val1 As Integer, ByVal val2 As Integer) As Integer
        Return System.Math.Max(Integer.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Integer.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the larger of two 64-bit signed integers.
    ''' </summary>
    ''' <param name="val1">The first of two 64-bit signed integers to compare.</param>
    ''' <param name="val2">The second of two 64-bit signed integers to compare.</param>
    <Extension>
    Public Function Max(ByVal val1 As Long, ByVal val2 As Long) As Long
        Return System.Math.Max(Long.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Long.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the larger of two 8-bit signed integers.
    ''' </summary>
    ''' <param name="val1">The first of two 8-bit signed integers to compare.</param>
    ''' <param name="val2">The second of two 8-bit signed integers to compare.</param>
    <Extension>
    Public Function Max(ByVal val1 As SByte, ByVal val2 As SByte) As SByte
        Return System.Math.Max(SByte.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), SByte.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the larger of two single-precision floating-point numbers.
    ''' </summary>
    ''' <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
    ''' <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
    <Extension>
    Public Function Max(ByVal val1 As Single, ByVal val2 As Single) As Single
        Return System.Math.Max(Single.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Single.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the larger of two 16-bit unsigned integers.
    ''' </summary>
    ''' <param name="val1">The first of two 16-bit unsigned integers to compare.</param>
    ''' <param name="val2">The second of two 16-bit unsigned integers to compare.</param>
    <Extension>
    Public Function Max(ByVal val1 As UShort, ByVal val2 As UShort) As UShort
        Return System.Math.Max(UShort.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), UShort.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the larger of two 32-bit unsigned integers.
    ''' </summary>
    ''' <param name="val1">The first of two 32-bit unsigned integers to compare.</param>
    ''' <param name="val2">The second of two 32-bit unsigned integers to compare.</param>
    <Extension>
    Public Function Max(ByVal val1 As UInteger, ByVal val2 As UInteger) As UInteger
        Return System.Math.Max(UInteger.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), UInteger.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the larger of two 64-bit unsigned integers.
    ''' </summary>
    ''' <param name="val1">The first of two 64-bit unsigned integers to compare.</param>
    ''' <param name="val2">The second of two 64-bit unsigned integers to compare.</param>
    <Extension>
    Public Function Max(ByVal val1 As ULong, ByVal val2 As ULong) As ULong
        Return System.Math.Max(ULong.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), ULong.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smaller of two 8-bit unsigned integers.
    ''' </summary>
    ''' <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
    ''' <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
    <Extension>
    Public Function Min(ByVal val1 As Byte, ByVal val2 As Byte) As Byte
        Return System.Math.Min(Byte.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Byte.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smaller of two decimal numbers.
    ''' </summary>
    ''' <param name="val1">The first of two decimal numbers to compare.</param>
    ''' <param name="val2">The second of two decimal numbers to compare.</param>
    <Extension>
    Public Function Min(ByVal val1 As Decimal, ByVal val2 As Decimal) As Decimal
        Return System.Math.Min(Decimal.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Decimal.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smaller of two double-precision floating-point numbers.
    ''' </summary>
    ''' <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
    ''' <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
    <Extension>
    Public Function Min(ByVal val1 As Double, ByVal val2 As Double) As Double
        Return System.Math.Min(Double.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Double.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smaller of two 16-bit signed integers.
    ''' </summary>
    ''' <param name="val1">The first of two 16-bit signed integers to compare.</param>
    ''' <param name="val2">The second of two 16-bit signed integers to compare.</param>
    <Extension>
    Public Function Min(ByVal val1 As Short, ByVal val2 As Short) As Short
        Return System.Math.Min(Short.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Short.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smaller of two 32-bit signed integers.
    ''' </summary>
    ''' <param name="val1">The first of two 32-bit signed integers to compare.</param>
    ''' <param name="val2">The second of two 32-bit signed integers to compare.</param>
    <Extension>
    Public Function Min(ByVal val1 As Integer, ByVal val2 As Integer) As Integer
        Return System.Math.Min(Integer.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Integer.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smaller of two 64-bit signed integers.
    ''' </summary>
    ''' <param name="val1">The first of two 64-bit signed integers to compare.</param>
    ''' <param name="val2">The second of two 64-bit signed integers to compare.</param>
    <Extension>
    Public Function Min(ByVal val1 As Long, ByVal val2 As Long) As Long
        Return System.Math.Min(Long.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Long.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smaller of two 8-bit signed integers.
    ''' </summary>
    ''' <param name="val1">The first of two 8-bit signed integers to compare.</param>
    ''' <param name="val2">The second of two 8-bit signed integers to compare.</param>
    <Extension>
    Public Function Min(ByVal val1 As SByte, ByVal val2 As SByte) As SByte
        Return System.Math.Min(SByte.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), SByte.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smaller of two single-precision floating-point numbers.
    ''' </summary>
    ''' <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
    ''' <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
    <Extension>
    Public Function Min(ByVal val1 As Single, ByVal val2 As Single) As Single
        Return System.Math.Min(Single.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Single.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smaller of two 16-bit unsigned integers.
    ''' </summary>
    ''' <param name="val1">The first of two 16-bit unsigned integers to compare.</param>
    ''' <param name="val2">The second of two 16-bit unsigned integers to compare.</param>
    <Extension>
    Public Function Min(ByVal val1 As UShort, ByVal val2 As UShort) As UShort
        Return System.Math.Min(UShort.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), UShort.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smaller of two 32-bit unsigned integers.
    ''' </summary>
    ''' <param name="val1">The first of two 32-bit unsigned integers to compare.</param>
    ''' <param name="val2">The second of two 32-bit unsigned integers to compare.</param>
    <Extension>
    Public Function Min(ByVal val1 As UInteger, ByVal val2 As UInteger) As UInteger
        Return System.Math.Min(UInteger.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), UInteger.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the smaller of two 64-bit unsigned integers.
    ''' </summary>
    ''' <param name="val1">The first of two 64-bit unsigned integers to compare.</param>
    ''' <param name="val2">The second of two 64-bit unsigned integers to compare.</param>
    <Extension>
    Public Function Min(ByVal val1 As ULong, ByVal val2 As ULong) As ULong
        Return System.Math.Min(ULong.Parse(val1.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), ULong.Parse(val2.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns a specified number raised to the specified power.
    ''' </summary>
    ''' <param name="x">A double-precision floating-point number to be raised to a power.</param>
    ''' <param name="y">A double-precision floating-point number that specifies a power.</param>
    <Extension>
    Public Function Pow(ByVal x As Double, ByVal y As Double) As Double
        Return System.Math.Pow(Double.Parse(x.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Double.Parse(y.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Rounds a decimal value to the nearest integral value.
    ''' </summary>
    ''' <param name="d">A decimal number to be rounded.</param>
    <Extension>
    Public Function Round(ByVal d As Decimal) As Decimal
        Return System.Math.Round(Decimal.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Rounds a double-precision floating-point value to the nearest integral value.
    ''' </summary>
    ''' <param name="d">A double-precision floating-point number to be rounded.</param>
    <Extension>
    Public Function Round(ByVal d As Double) As Double
        Return System.Math.Round(Double.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Rounds a decimal value to a specified number of fractional digits.
    ''' </summary>
    ''' <param name="d">A decimal number to be rounded.</param>
    ''' <param name="decimals">The number of decimal places in the return value.</param>
    <Extension>
    Public Function Round(ByVal d As Decimal, ByVal decimals As Integer) As Decimal
        Return System.Math.Round(Decimal.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Integer.Parse(decimals.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Rounds a decimal value to the nearest integer. A parameter specifies how to round the value if it is midway between two numbers.
    ''' </summary>
    ''' <param name="d">A decimal number to be rounded.</param>
    ''' <param name="mode">Specification for how to round d if it is midway between two other numbers.</param>
    <Extension>
    Public Function Round(ByVal d As Decimal, ByVal mode As MidpointRounding) As Decimal
        Return System.Math.Round(Decimal.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), mode)
    End Function

    ''' <summary>
    ''' Rounds a double-precision floating-point value to a specified number of fractional digits.
    ''' </summary>
    ''' <param name="value">A double-precision floating-point number to be rounded.</param>
    ''' <param name="digits">The number of fractional digits in the return value.</param>
    <Extension>
    Public Function Round(ByVal value As Double, ByVal digits As Integer) As Double
        Return System.Math.Round(Double.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Integer.Parse(digits.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Rounds a double-precision floating-point value to the nearest integer. A parameter specifies how to round the value if it is midway between two numbers.
    ''' </summary>
    ''' <param name="value">A double-precision floating-point number to be rounded.</param>
    ''' <param name="mode">Specification for how to round value if it is midway between two other numbers.</param>
    <Extension>
    Public Function Round(ByVal value As Double, ByVal mode As MidpointRounding) As Double
        Return System.Math.Round(Double.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), mode)
    End Function

    ''' <summary>
    ''' Rounds a decimal value to a specified number of fractional digits. A parameter specifies how to round the value if it is midway between two numbers.
    ''' </summary>
    ''' <param name="d">A decimal number to be rounded.</param>
    ''' <param name="decimals">The number of decimal places in the return value.</param>
    ''' <param name="mode">Specification for how to round d if it is midway between two other numbers.</param>
    <Extension>
    Public Function Round(ByVal d As Decimal, ByVal decimals As Integer, ByVal mode As MidpointRounding) As Decimal
        Return System.Math.Round(Decimal.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Integer.Parse(decimals.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), mode)
    End Function

    ''' <summary>
    ''' Rounds a double-precision floating-point value to a specified number of fractional digits. A parameter specifies how to round the value if it is midway between two numbers.
    ''' </summary>
    ''' <param name="value">A double-precision floating-point number to be rounded.</param>
    ''' <param name="digits">The number of fractional digits in the return value.</param>
    ''' <param name="mode">Specification for how to round value if it is midway between two other numbers.</param>
    <Extension>
    Public Function Round(ByVal value As Double, ByVal digits As Integer, ByVal mode As MidpointRounding) As Double
        Return System.Math.Round(Double.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), Integer.Parse(digits.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)), mode)
    End Function

    ''' <summary>
    ''' Returns a value indicating the sign of a decimal number.
    ''' </summary>
    ''' <param name="value">A signed decimal number.</param>
    <Extension>
    Public Function Sign(ByVal value As Decimal) As Integer
        Return System.Math.Sign(Decimal.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns a value indicating the sign of a double-precision floating-point number.
    ''' </summary>
    ''' <param name="value">A signed number.</param>
    <Extension>
    Public Function Sign(ByVal value As Double) As Integer
        Return System.Math.Sign(Double.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns a value indicating the sign of a 16-bit signed integer.
    ''' </summary>
    ''' <param name="value">A signed number.</param>
    <Extension>
    Public Function Sign(ByVal value As Short) As Integer
        Return System.Math.Sign(Short.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns a value indicating the sign of a 32-bit signed integer.
    ''' </summary>
    ''' <param name="value">A signed number.</param>
    <Extension>
    Public Function Sign(ByVal value As Integer) As Integer
        Return System.Math.Sign(Integer.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns a value indicating the sign of a 64-bit signed integer.
    ''' </summary>
    ''' <param name="value">A signed number.</param>
    <Extension>
    Public Function Sign(ByVal value As Long) As Integer
        Return System.Math.Sign(Long.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns a value indicating the sign of an 8-bit signed integer.
    ''' </summary>
    ''' <param name="value">A signed number.</param>
    <Extension>
    Public Function Sign(ByVal value As SByte) As Integer
        Return System.Math.Sign(SByte.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns a value indicating the sign of a single-precision floating-point number.
    ''' </summary>
    ''' <param name="value">A signed number.</param>
    <Extension>
    Public Function Sign(ByVal value As Single) As Integer
        Return System.Math.Sign(Single.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the sine of the specified angle.
    ''' </summary>
    ''' <param name="a">An angle, measured in radians.</param>
    <Extension>
    Public Function Sin(ByVal a As Double) As Double
        Return System.Math.Sin(Double.Parse(a.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the hyperbolic sine of the specified angle.
    ''' </summary>
    ''' <param name="value">An angle, measured in radians.</param>
    <Extension>
    Public Function Sinh(ByVal value As Double) As Double
        Return System.Math.Sinh(Double.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the square root of a specified number.
    ''' </summary>
    ''' <param name="d">The number whose square root is to be found. </param>
    <Extension>
    Public Function Sqrt(ByVal d As Double) As Double
        Return System.Math.Sqrt(Double.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the tangent of the specified angle.
    ''' </summary>
    ''' <param name="a">An angle, measured in radians.</param>
    <Extension>
    Public Function Tan(ByVal a As Double) As Double
        Return System.Math.Tan(Double.Parse(a.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Returns the hyperbolic tangent of the specified angle.
    ''' </summary>
    ''' <param name="value">An angle, measured in radians.</param>
    <Extension>
    Public Function Tanh(ByVal value As Double) As Double
        Return System.Math.Tanh(Double.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Calculates the integral part of a specified decimal number.
    ''' </summary>
    ''' <param name="d">A number to truncate.</param>
    <Extension>
    Public Function Truncate(ByVal d As Decimal) As Decimal
        Return System.Math.Truncate(Decimal.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

    ''' <summary>
    ''' Calculates the integral part of a specified double-precision floating-point number.
    ''' </summary>
    ''' <param name="d">A number to truncate.</param>
    <Extension>
    Public Function Truncate(ByVal d As Double) As Double
        Return System.Math.Truncate(Double.Parse(d.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)))
    End Function

End Module
Imports System.Runtime.CompilerServices

Public Module MathHelper

    <Extension>
    Public Function ToDecimal(ByVal value As String) As Decimal
        Return Decimal.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator))
    End Function

    <Extension>
    Public Function ToDouble(ByVal value As String) As Double
        Return Double.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator))
    End Function

    <Extension>
    Public Function ToShort(ByVal value As String) As Short
        Return Short.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator))
    End Function

    <Extension>
    Public Function ToInteger(ByVal value As String) As Integer
        Return Integer.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator))
    End Function

    <Extension>
    Public Function ToLong(ByVal value As String) As Long
        Return Long.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator))
    End Function

    <Extension>
    Public Function ToSByte(ByVal value As String) As SByte
        Return SByte.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator))
    End Function

    <Extension>
    Public Function ToSingle(ByVal value As String) As Single
        Return Single.Parse(value.ToString.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator).Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator))
    End Function

    <Extension>
    Public Function ToString(ByVal value As Double, ByVal DecimalSeperator As String) As String
        Return value.ToString.Replace(".", DecimalSeperator).Replace(",", DecimalSeperator)
    End Function

    ''' <summary>
    ''' Returns a random integer that is within a specified range.
    ''' </summary>
    ''' <param name="minValue">The inclusive lower bound of the random number returned.</param>
    ''' <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
    <Extension>
    Public Function Random(ByVal minValue As Integer, ByVal maxValue As Integer) As Integer
        Return New Random().Next(minValue, maxValue)
    End Function
End Module

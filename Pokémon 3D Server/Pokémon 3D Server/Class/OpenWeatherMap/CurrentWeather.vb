Namespace OpenWeatherMap
    Public Class CurrentWeather
        Public Property coord As coord
        Public Property weather As weather
        Public Property base As String
        Public Property main As main
        Public Property wind As wind
        Public Property clouds As clouds
        Public Property rain As rain
        Public Property snow As snow
        Public Property dt As Long
        Public Property sys As sys
        Public Property id As Integer
        Public Property name As String
        Public Property cod As Integer

        Public Sub New() : End Sub
    End Class

    Public Class coord
        Public Property lon As Double
        Public Property lat As Double
    End Class

    Public Class weather
        Public Property id As Integer
        Public Property main As String
        Public Property description As String
        Public Property icon As String
    End Class

    Public Class main
        Public Property temp As Double
        Public Property pressure As Integer
        Public Property humidity As Integer
        Public Property temp_min As Double
        Public Property temp_max As Double
        Public Property sea_level As Integer
        Public Property grnd_level As Integer
    End Class

    Public Class wind
        Public Property speed As Double
        Public Property deg As Integer
    End Class

    Public Class clouds
        Public Property all As Integer
    End Class

    Public Class rain
        Public Property threeh As Integer
    End Class

    Public Class snow
        Public Property threeh As Integer
    End Class

    Public Class sys
        Public Property type As Integer
        Public Property id As Integer
        Public Property message As Double
        Public Property country As String
        Public Property sunrise As Long
        Public Property sunset As Long
    End Class
End Namespace
Imports System.IO
Imports System.Net
Imports System.Runtime.CompilerServices

Public Module Functions

    Private Sub PlaySystemSound()
        My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
    End Sub

    <Extension>
    Public Sub CatchError(ByVal ex As Exception)
        PlaySystemSound()
        Dim CoreArchitecture As String
        Dim HelpLink As String
        Dim InnerException As String
        Dim InnerStackTrace As String
        If Environment.Is64BitOperatingSystem = True Then
            CoreArchitecture = "64 Bit"
        Else
            CoreArchitecture = "32 Bit"
        End If
        If String.IsNullOrWhiteSpace(ex.HelpLink) Then
            HelpLink = "No helplink available."
        Else
            HelpLink = ex.HelpLink
        End If
        If ex.InnerException IsNot Nothing Then
            InnerException = ex.InnerException.Message
            InnerStackTrace = ex.InnerException.StackTrace & vbNewLine & ex.StackTrace
        Else
            InnerException = "Nothing"
            InnerStackTrace = ex.StackTrace
        End If
        Dim ErrorLog As String =
"[CODE]
Pokémon 3D Server Client Crash Log v" & Main.Setting.ApplicationVersion & "
--------------------------------------------------

System specifications:

Operating system: " & My.Computer.Info.OSFullName & "[" & My.Computer.Info.OSVersion & "]
Core architecture: " & CoreArchitecture & "
System time: " & My.Computer.Clock.LocalTime.ToString & "
System language: " & Globalization.CultureInfo.CurrentCulture.EnglishName.ToString & "
Physical memory: " & Round(My.Computer.Info.AvailablePhysicalMemory / 1073741824, 2).ToString & " GB / " & Round(My.Computer.Info.TotalPhysicalMemory / 1073741824, 2).ToString & " GB" & "
Logical processors: " & Environment.ProcessorCount.ToString & "

--------------------------------------------------
            
Error information:

Message: " & ex.Message & "
InnerException: " & InnerException & "
HelpLink: " & HelpLink & "
Source: " & ex.Source & "

--------------------------------------------------

CallStack:

" & InnerStackTrace & "

--------------------------------------------------

You should report this error if it is reproduceable or you could not solve it by yourself.

Go To: http://pokemon3d.net/forum/threads/8234/ to report this crash there.
[/CODE]"

        If Not Directory.Exists(Main.Setting.ApplicationDirectory & "\CrashLogs") Then
            Directory.CreateDirectory(Main.Setting.ApplicationDirectory & "\CrashLogs")
        End If

        Dim ErrorTime As Date = Date.Now
        Dim RandomIndetifier As Integer = Random(0, Integer.MaxValue)
        Try
            File.WriteAllText(Main.Setting.ApplicationDirectory & "\CrashLogs\Crash_" & Main.Setting.StartTime.Day.ToString & "-" & Main.Setting.StartTime.Month.ToString & "-" & Main.Setting.StartTime.Year.ToString & "_" & Main.Setting.StartTime.Hour.ToString & "." & Main.Setting.StartTime.Minute.ToString & "." & Main.Setting.StartTime.Second.ToString & "." & RandomIndetifier & ".dat", ErrorLog)
            Main.Main.QueueMessage(ex.Message & vbNewLine & "Error Log saved at: " & Main.Setting.ApplicationDirectory & "\CrashLogs\Crash_" & Main.Setting.StartTime.Day.ToString & "-" & Main.Setting.StartTime.Month.ToString & "-" & Main.Setting.StartTime.Year.ToString & "_" & Main.Setting.StartTime.Hour.ToString & "." & Main.Setting.StartTime.Minute.ToString & "." & Main.Setting.StartTime.Second.ToString & "." & RandomIndetifier & ".dat", Main.LogType.Warning)
        Catch exa As Exception
            Main.Main.QueueMessage(ex.Message, Main.LogType.Warning)
        End Try
    End Sub

    <Extension>
    Public Sub ReturnMessage(ByVal Message As String, Optional ByVal Style As MsgBoxStyle = MsgBoxStyle.Information, Optional ByVal Title As String = Nothing)
        PlaySystemSound()
        MsgBox(Message, Style, Title)
    End Sub

    <Extension>
    Public Function SplitCount(ByVal FullString As String) As Integer
        If FullString.Contains("|") Then
            Return FullString.Split("|"c).Count
        Else
            Return 1
        End If
    End Function

    <Extension>
    Public Function GetSplit(ByVal fullString As String, ByVal valueIndex As Integer) As String
        If Not fullString = Nothing AndAlso fullString.Contains("|") Then
            Dim SelectedString() As String = fullString.Split("|"c)
            Dim SelectedIndex As Integer = 0

            If valueIndex = -1 OrElse valueIndex > SelectedString.Count - 1 Then
                SelectedIndex = SelectedString.Count - 1
            Else
                SelectedIndex = valueIndex
            End If

            Return SelectedString(SelectedIndex)
        Else
            Return fullString
        End If
    End Function

    <Extension>
    Public Function GetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal seperator As String) As String
        If Not fullString = Nothing AndAlso fullString.Contains(seperator) Then
            Dim SelectedString() As String = fullString.Split(CChar(seperator))
            Dim SelectedIndex As Integer = 0

            If valueIndex = -1 OrElse valueIndex > SelectedString.Count - 1 Then
                SelectedIndex = SelectedString.Count - 1
            Else
                SelectedIndex = valueIndex
            End If

            Return SelectedString(SelectedIndex)
        Else
            Return fullString
        End If
    End Function

    <Extension>
    Public Function GetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal seperator As String, ByVal [Default] As String) As String
        If Not fullString = Nothing AndAlso fullString.Contains(seperator) Then
            Dim SelectedString() As String = fullString.Split(CChar(seperator))
            Dim SelectedIndex As Integer = 0

            If valueIndex = -1 OrElse valueIndex > SelectedString.Count - 1 Then
                SelectedIndex = SelectedString.Count - 1
            Else
                SelectedIndex = valueIndex
            End If

            Return SelectedString(SelectedIndex)
        Else
            Return [Default]
        End If
    End Function

    Public Function GetPublicIP() As String
        Dim Client As New WebClient
        Try
            Return Client.DownloadString("https://api.ipify.org")
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function GetPrivateIP() As String
        Dim host As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())

        For Each address As IPAddress In host.AddressList
            If address.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                Return address.ToString
            End If
        Next
        Return Nothing
    End Function
End Module
Imports System.IO

Public Class PlayerMute

    Public Shared Function Load(ByVal PlayerIndex As Integer) As List(Of String)
        Dim Setting As New List(Of String)
        Dim PlayerMuteList As New List(Of String)
        Dim FileLocation As String = Nothing

        If Directory.Exists(Settings.ApplicationDirectory + "\UserMute") Then
            Directory.CreateDirectory(Settings.ApplicationDirectory + "\UserMute")
        End If

        If Player.isGameJoltPlayer(PlayerIndex) Then
            FileLocation = Settings.ApplicationDirectory + "\UserMute\" + Player.Name(PlayerIndex) + "_" + Player.GameJoltID(PlayerIndex).ToString + ".dat"
        Else
            FileLocation = Settings.ApplicationDirectory + "\UserMute\" + Player.Name(PlayerIndex) + ".dat"
        End If

        If File.Exists(FileLocation) Then
            Dim Flag As Boolean = False
            Dim Flag2 As Boolean = False
            Dim FlagData As String = Nothing
            For Each Lines As String In File.ReadAllLines(FileLocation)
                If Lines.Contains("<!--") Or Lines.Contains("/*") Then
                    Flag = True
                End If
                If Flag = True Then
                    If Lines.Contains("-->") Or Lines.Contains("*/") Then
                        Flag = False
                    End If
                End If
                If Not Flag And Not Lines.Contains("-->") Then
                    If Lines.Contains("MuteListData|") Then
                        Flag2 = True
                        FlagData = "MuteListData"
                    End If

                    If Flag2 Then
                        If String.Equals(FlagData, "MuteListData", StringComparison.OrdinalIgnoreCase) And Not Lines.Contains("MuteListData|") Then
                            If Not (String.IsNullOrEmpty(Lines) Or String.IsNullOrWhiteSpace(Lines)) Then
                                PlayerMuteList.Add(Lines)
                            End If
                        End If
                    End If
                End If
            Next

            If PlayerMuteList.Count > 0 Then
                Return PlayerMuteList
            Else
                Return Nothing
            End If
        Else
            Setting.Add("<!-- Pokémon 3D Server Client User Setting File -->")
            Setting.Add("<!-- MuteList Data -->")
            Setting.Add("<!-- Name | GameJolt ID | Mute Reason | Mute Start Time | Mute Time -->")
            Setting.Add("MuteListData|")
            File.WriteAllLines(FileLocation, Setting)
            Return Nothing
        End If
    End Function

    Public Shared Sub Add(ByVal PlayerIndex As Integer, ByVal MutePlayerIndex As Integer, Optional ByVal MuteReason As String = Nothing, Optional ByVal Duration As Integer = Integer.MaxValue)
        Dim Setting As New List(Of String)
        Dim FileLocation As String = Nothing

        If Player.isGameJoltPlayer(PlayerIndex) Then
            FileLocation = Settings.ApplicationDirectory + "\UserMute\" + Player.Name(PlayerIndex) + "_" + Player.GameJoltID(PlayerIndex).ToString + ".dat"
        Else
            FileLocation = Settings.ApplicationDirectory + "\UserMute\" + Player.Name(PlayerIndex) + ".dat"
        End If

        If File.Exists(FileLocation) Then
            Setting.Add("<!-- Pokémon 3D Server Client User Setting File -->")
            Setting.Add("<!-- MuteList Data -->")
            Setting.Add("<!-- Name | GameJolt ID | Mute Reason | Mute Start Time | Mute Time -->")
            Setting.Add("MuteListData|")
            If Load(PlayerIndex).Count > 0 Then
                Setting.AddRange(Load(PlayerIndex))
            End If
            If Player.isGameJoltPlayer(MutePlayerIndex) Then
                Setting.Add(Player.Name(MutePlayerIndex) + "|" + Player.GameJoltID(MutePlayerIndex).ToString + "|" + MuteReason + "|" + DateTime.Now.ToString + "|" + Duration.ToString)
            Else
                Setting.Add(Player.Name(MutePlayerIndex) + "|Nothing|" + MuteReason + "|" + DateTime.Now.ToString + "|" + Duration.ToString)
            End If

            File.WriteAllLines(FileLocation, Setting)
        End If
    End Sub

    Public Shared Sub Remove(ByVal PlayerIndex As Integer, ByVal MutePlayerIndex As Integer)
        Dim Setting As New List(Of String)
        Dim FileLocation As String = Nothing

        If Player.isGameJoltPlayer(PlayerIndex) Then
            FileLocation = Settings.ApplicationDirectory + "\UserMute\" + Player.Name(PlayerIndex) + "_" + Player.GameJoltID(PlayerIndex).ToString + ".dat"
        Else
            FileLocation = Settings.ApplicationDirectory + "\UserMute\" + Player.Name(PlayerIndex) + ".dat"
        End If

        If File.Exists(FileLocation) Then
            Setting.Add("<!-- Pokémon 3D Server Client User Setting File -->")
            Setting.Add("<!-- MuteList Data -->")
            Setting.Add("<!-- Name | GameJolt ID | Mute Reason | Mute Start Time | Mute Time -->")
            Setting.Add("MuteListData|")
            If Load(PlayerIndex).Count > 0 Then
                Setting.AddRange(Load(PlayerIndex))
            End If

            For Each Players As String In Setting
                If Player.isGameJoltPlayer(MutePlayerIndex) Then
                    If String.Equals(Functions.GetSplit(Players, 0, "|"), Player.Name(MutePlayerIndex), StringComparison.Ordinal) And String.Equals(Functions.GetSplit(Players, 1, "|"), Player.GameJoltID(MutePlayerIndex).ToString, StringComparison.OrdinalIgnoreCase) Then
                        Setting.Remove(Players)
                    End If
                Else
                    If String.Equals(Functions.GetSplit(Players, 0, "|"), Player.Name(MutePlayerIndex), StringComparison.Ordinal) And String.Equals(Functions.GetSplit(Players, 1, "|"), "Nothing", StringComparison.OrdinalIgnoreCase) Then
                        Setting.Remove(Players)
                    End If
                End If
            Next

            File.WriteAllLines(FileLocation, Setting)
        End If
    End Sub
End Class

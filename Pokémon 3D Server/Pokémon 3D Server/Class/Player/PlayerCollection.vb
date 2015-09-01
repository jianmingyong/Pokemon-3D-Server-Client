Imports System.Net.Sockets

Public Class PlayerCollection
    Inherits List(Of Player)

    Public Overloads Sub Add(ByVal p As Package)
        If p.IsValid AndAlso p.IsFullPackageData Then
            Dim ID As Integer = GetNextValidID()
            Dim Player As Player = New Player(p, ID)
            If Player.isGameJoltPlayer Then
                Main.Setting.OnlineSettingListData.Add(New OnlineSetting(Player.Name, Player.GameJoltID))
            End If
            Main.Main.UpdatePlayerList(Main.Operation.Add, Player)
        End If
    End Sub

    Public Overloads Sub Remove(ByVal ID As Integer, ByVal Reason As String)
        Dim Name As String = GetPlayer(ID).Name
        Dim GameJoltID As Integer = GetPlayer(ID).GameJoltID
        Dim OnlineSetting As OnlineSetting = (From p As OnlineSetting In Main.Setting.OnlineSettingListData Select p Where p.Name = Name AndAlso p.GameJoltID = GameJoltID).FirstOrDefault

        GetPlayer(ID).Remove(Reason)
        If OnlineSetting IsNot Nothing Then
            OnlineSetting.Save()
            Main.Setting.OnlineSettingListData.Remove(OnlineSetting)
        End If
        Main.Main.UpdatePlayerList(Main.Operation.Remove, GetPlayer(ID))
    End Sub

    Public Overloads Sub Remove(ByVal Name As String, ByVal Reason As String)
        Dim GameJoltID As Integer = GetPlayer(Name).GameJoltID
        Dim OnlineSetting As OnlineSetting = (From p As OnlineSetting In Main.Setting.OnlineSettingListData Select p Where p.Name = Name AndAlso p.GameJoltID = GameJoltID).FirstOrDefault

        GetPlayer(Name).Remove(Reason)
        If OnlineSetting IsNot Nothing Then
            OnlineSetting.Save()
            Main.Setting.OnlineSettingListData.Remove(OnlineSetting)
        End If
        Main.Main.UpdatePlayerList(Main.Operation.Remove, GetPlayer(Name))
    End Sub

    Public Function HasPlayer(ByVal ID As Integer) As Boolean
        If (From p As Player In Me Select p Where p.PlayerID = ID).FirstOrDefault IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function HasPlayer(ByVal Name As String) As Boolean
        If (From p As Player In Me Select p Where p.Name = Name).FirstOrDefault IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function HasPlayer(ByVal Client As TcpClient) As Boolean
        If (From p As Player In Me Select p Where p.PlayerClient Is Client).FirstOrDefault IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetPlayer(ID As Integer) As Player
        Return (From p As Player In Me Select p Where p.PlayerID = ID).FirstOrDefault
    End Function

    Public Function GetPlayer(Name As String) As Player
        Return (From p As Player In Me Select p Where p.Name = Name).FirstOrDefault
    End Function

    Public Function GetPlayer(Client As TcpClient) As Player
        Return (From p As Player In Me Select p Where p.PlayerClient Is Client).FirstOrDefault
    End Function

    Public Function GetNextValidID() As Integer
        If Me.Count = 0 Then
            Return 0
        Else
            Dim ValidID As Integer = 0
            Dim ListOfPlayer As List(Of Player) = (From p As Player In Me Order By p.PlayerID Ascending).ToList
            For i As Integer = 0 To ListOfPlayer.Count - 1
                If ValidID = ListOfPlayer(i).PlayerID Then
                    ValidID += 1
                Else
                    Return ValidID
                End If
            Next
            Return ValidID
        End If
    End Function
End Class
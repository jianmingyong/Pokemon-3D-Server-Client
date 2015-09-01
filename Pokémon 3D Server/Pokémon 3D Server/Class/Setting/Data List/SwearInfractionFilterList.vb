Public Class SwearInfractionFilterList
    Public Property Word As String
    Public Property CaseSensitive As Boolean
    Public Property Regex As String

    Public Sub New(ByVal Word As String, ByVal CaseSensitive As Boolean)
        Me.Word = Word
        Me.CaseSensitive = CaseSensitive
        Regex = "\b" & GenerateRegex() & "\b"
    End Sub

    Private Function GenerateRegex() As String
        Dim Word() As Char = Me.Word.ToCharArray
        Dim Regex As String = Nothing

        For Each Letter As Char In Word
            Select Case Letter
                Case "A"c, "a"c
                    If CaseSensitive Then
                        If Letter = "A"c Then
                            Regex &= GenerateListofRegex("4", "A", "À", "Á", "Â", "Ã", "Ä", "Å", "Ā", "Ă", "Ą", "ƛ", "Ǎ", "Ǟ", "Ǡ", "Ǻ", "Ȁ", "Ȃ", "Ȧ", "Ⱥ", "Ʌ")
                        ElseIf Letter = "a"c
                            Regex &= GenerateListofRegex("@", "a", "ª", "à", "á", "â", "ã", "ä", "å", "ā", "ă", "ą", "ǎ", "ǟ", "ǡ", "ǻ", "ȁ", "ȃ", "ȧ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("@", "a", "ª", "à", "á", "â", "ã", "ä", "å", "ā", "ă", "ą", "ǎ", "ǟ", "ǡ", "ǻ", "ȁ", "ȃ", "ȧ", "4", "A", "À", "Á", "Â", "Ã", "Ä", "Å", "Ā", "Ă", "Ą", "ƛ", "Ǎ", "Ǟ", "Ǡ", "Ǻ", "Ȁ", "Ȃ", "Ȧ", "Ⱥ", "Ʌ")
                    End If
                Case "B"c, "b"c
                    If CaseSensitive Then
                        If Letter = "B"c Then
                            Regex &= GenerateListofRegex("B", "ß", "Ɓ", "Ƃ", "Ƀ")
                        ElseIf Letter = "b"c
                            Regex &= GenerateListofRegex("b", "Þ", "þ", "ƀ", "ƃ", "Ƅ", "ƅ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("b", "Þ", "þ", "ƀ", "ƃ", "Ƅ", "ƅ", "B", "ß", "Ɓ", "Ƃ", "Ƀ")
                    End If
                Case "C"c, "c"c
                    If CaseSensitive Then
                        If Letter = "C"c Then
                            Regex &= GenerateListofRegex("C", "{", "Ç", "Ć", "Ĉ", "Ċ", "Č", "Ƈ", "Ȼ")
                        ElseIf Letter = "c"c
                            Regex &= GenerateListofRegex("<", "c", "¢", "©", "ç", "ć", "ĉ", "ċ", "č", "ƈ", "ȼ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("<", "c", "¢", "©", "ç", "ć", "ĉ", "ċ", "č", "ƈ", "ȼ", "C", "{", "Ç", "Ć", "Ĉ", "Ċ", "Č", "Ƈ", "Ȼ")
                    End If
                Case "D"c, "d"c
                    If CaseSensitive Then
                        If Letter = "D"c Then
                            Regex &= GenerateListofRegex("D", "Ð", "Þ", "þ", "Ď", "Đ", "Ɗ", "ƿ", "Ƿ")
                        ElseIf Letter = "d"c
                            Regex &= GenerateListofRegex("d", "ď", "đ", "Ƌ", "ƌ", "ȡ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("d", "ď", "đ", "Ƌ", "ƌ", "ȡ", "D", "Ð", "Þ", "þ", "Ď", "Đ", "Ɗ", "ƿ", "Ƿ")
                    End If
                Case "E"c, "e"c
                    If CaseSensitive Then
                        If Letter = "E"c Then
                            Regex &= GenerateListofRegex("3", "E", "£", "È", "É", "Ê", "Ë", "Ē", "Ĕ", "Ė", "Ę", "Ě", "Ǝ", "Ɛ", "Ʃ", "Ƹ", "ƹ", "Ǯ", "ǯ", "Ȅ", "Ȇ", "Ȝ", "ȝ", "Ȩ", "Ɇ")
                        ElseIf Letter = "e"c
                            Regex &= GenerateListofRegex("e", "è", "é", "ê", "ë", "ē", "ĕ", "ė", "ę", "ě", "Ə", "ǝ", "ȅ", "ȇ", "ȩ", "ɇ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("e", "è", "é", "ê", "ë", "ē", "ĕ", "ė", "ę", "ě", "Ə", "ǝ", "ȅ", "ȇ", "ȩ", "ɇ", "3", "E", "£", "È", "É", "Ê", "Ë", "Ē", "Ĕ", "Ė", "Ę", "Ě", "Ǝ", "Ɛ", "Ʃ", "Ƹ", "ƹ", "Ǯ", "ǯ", "Ȅ", "Ȇ", "Ȝ", "ȝ", "Ȩ", "Ɇ")
                    End If
                Case "F"c, "f"c
                    If CaseSensitive Then
                        If Letter = "F"c Then
                            Regex &= GenerateListofRegex("F", "Ƒ")
                        ElseIf Letter = "f"c
                            Regex &= GenerateListofRegex("f", "ƒ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("f", "ƒ", "F", "Ƒ")
                    End If
                Case "G"c, "g"c
                    If CaseSensitive Then
                        If Letter = "G"c Then
                            Regex &= GenerateListofRegex("G", "Ĝ", "Ğ", "Ġ", "Ģ", "Ɠ", "Ǥ", "Ǧ", "Ǵ")
                        ElseIf Letter = "g"c
                            Regex &= GenerateListofRegex("9", "g", "ĝ", "ğ", "ġ", "ģ", "ǥ", "ǧ", "ǵ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("9", "g", "ĝ", "ğ", "ġ", "ģ", "ǥ", "ǧ", "ǵ", "G", "Ĝ", "Ğ", "Ġ", "Ģ", "Ɠ", "Ǥ", "Ǧ", "Ǵ")
                    End If
                Case "H"c, "h"c
                    If CaseSensitive Then
                        If Letter = "H"c Then
                            Regex &= GenerateListofRegex("H", "Ĥ", "Ħ", "Ƕ", "Ȟ")
                        ElseIf Letter = "h"c
                            Regex &= GenerateListofRegex("h", "ĥ", "ħ", "ȟ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("h", "ĥ", "ħ", "ȟ", "H", "Ĥ", "Ħ", "Ƕ", "Ȟ")
                    End If
                Case "I"c, "i"c
                    If CaseSensitive Then
                        If Letter = "I"c Then
                            Regex &= GenerateListofRegex("1", "I", "Ì", "Í", "Î", "Ï", "Ĩ", "Ī", "Ĭ", "Į", "İ", "Ɩ", "Ɨ", "Ǐ", "Ȉ", "Ȋ")
                        ElseIf Letter = "i"c
                            Regex &= GenerateListofRegex("!", "i", "¡", "ì", "í", "î", "ï", "ĩ", "ī", "ĭ", "į", "ı", "ǃ", "ǀ", "ǐ", "ȉ", "ȋ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("!", "i", "¡", "ì", "í", "î", "ï", "ĩ", "ī", "ĭ", "į", "ı", "ǃ", "ǀ", "ǐ", "ȉ", "ȋ", "1", "I", "Ì", "Í", "Î", "Ï", "Ĩ", "Ī", "Ĭ", "Į", "İ", "Ɩ", "Ɨ", "Ǐ", "Ȉ", "Ȋ")
                    End If
                Case "J"c, "j"c
                    If CaseSensitive Then
                        If Letter = "J"c Then
                            Regex &= GenerateListofRegex("J", "Ĵ", "Ɉ")
                        ElseIf Letter = "j"c
                            Regex &= GenerateListofRegex("j", "ĵ", "ǰ", "ɉ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("j", "ĵ", "ǰ", "ɉ", "J", "Ĵ", "Ɉ")
                    End If
                Case "K"c, "k"c
                    If CaseSensitive Then
                        If Letter = "K"c Then
                            Regex &= GenerateListofRegex("K", "Ķ", "Ƙ", "Ǩ")
                        ElseIf Letter = "k"c
                            Regex &= GenerateListofRegex("k", "ķ", "ĸ", "ƙ", "ǩ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("k", "ķ", "ĸ", "ƙ", "ǩ", "K", "Ķ", "Ƙ", "Ǩ")
                    End If
                Case "L"c, "l"c
                    If CaseSensitive Then
                        If Letter = "L"c Then
                            Regex &= GenerateListofRegex("L", "Ĺ", "Ļ", "Ľ", "Ŀ", "Ł", "Ƚ")
                        ElseIf Letter = "l"c
                            Regex &= GenerateListofRegex("1", "l", "¦", "ĺ", "ļ", "ľ", "ŀ", "ł", "ƚ", "ƪ", "ǀ", "ȴ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("1", "l", "¦", "ĺ", "ļ", "ľ", "ŀ", "ł", "ƚ", "ƪ", "ǀ", "ȴ", "L", "Ĺ", "Ļ", "Ľ", "Ŀ", "Ł", "Ƚ")
                    End If
                Case "M"c, "m"c
                    If CaseSensitive Then
                        If Letter = "M"c Then
                            Regex &= GenerateListofRegex("M")
                        ElseIf Letter = "m"c
                            Regex &= GenerateListofRegex("m")
                        End If
                    Else
                        Regex &= GenerateListofRegex("m", "M")
                    End If
                Case "N"c, "n"c
                    If CaseSensitive Then
                        If Letter = "N"c Then
                            Regex &= GenerateListofRegex("N", "Ñ", "Ń", "Ņ", "Ň", "Ɲ", "Ǹ")
                        ElseIf Letter = "n"c
                            Regex &= GenerateListofRegex("n", "ñ", "ń", "ņ", "ň", "ŉ", "Ŋ", "ŋ", "ƞ", "ǹ", "Ƞ", "ȵ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("n", "ñ", "ń", "ņ", "ň", "ŉ", "Ŋ", "ŋ", "ƞ", "ǹ", "Ƞ", "ȵ", "N", "Ñ", "Ń", "Ņ", "Ň", "Ɲ", "Ǹ")
                    End If
                Case "O"c, "o"c
                    If CaseSensitive Then
                        If Letter = "O"c Then
                            Regex &= GenerateListofRegex("0", "O", "Ò", "Ó", "Ô", "Õ", "Ö", "Ø", "Ō", "Ŏ", "Ő", "Ɵ", "Ơ", "Ǒ", "Ǫ", "Ǭ", "Ǿ", "Ȍ", "Ȏ", "Ȫ", "Ȭ", "Ȯ", "Ȱ")
                        ElseIf Letter = "o"c
                            Regex &= GenerateListofRegex("o", "¤", "°", "º", "ð", "ò", "ó", "ô", "õ", "ö", "ø", "ō", "ŏ", "ő", "ơ", "ǒ", "ǫ", "ǭ", "ǿ", "ȍ", "ȏ", "ȫ", "ȭ", "ȯ", "ȱ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("o", "¤", "°", "º", "ð", "ò", "ó", "ô", "õ", "ö", "ø", "ō", "ŏ", "ő", "ơ", "ǒ", "ǫ", "ǭ", "ǿ", "ȍ", "ȏ", "ȫ", "ȭ", "ȯ", "ȱ", "0", "O", "Ò", "Ó", "Ô", "Õ", "Ö", "Ø", "Ō", "Ŏ", "Ő", "Ɵ", "Ơ", "Ǒ", "Ǫ", "Ǭ", "Ǿ", "Ȍ", "Ȏ", "Ȫ", "Ȭ", "Ȯ", "Ȱ")
                    End If
                Case "P"c, "p"c
                    If CaseSensitive Then
                        If Letter = "P"c Then
                            Regex &= GenerateListofRegex("P", "Ƥ")
                        ElseIf Letter = "p"c
                            Regex &= GenerateListofRegex("p", "Þ", "þ", "ƥ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("p", "Þ", "þ", "ƥ", "P", "Ƥ")
                    End If
                Case "Q"c, "q"c
                    If CaseSensitive Then
                        If Letter = "Q"c Then
                            Regex &= GenerateListofRegex("Q", "ƍ", "Ɋ", "Ǫ", "Ǭ")
                        ElseIf Letter = "q"c
                            Regex &= GenerateListofRegex("q", "ɋ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("p", "Þ", "þ", "ƥ", "P", "Ƥ")
                    End If
                Case "R"c, "r"c
                    If CaseSensitive Then
                        If Letter = "R"c Then
                            Regex &= GenerateListofRegex("R", "®", "Ŕ", "Ŗ", "Ř", "Ʀ", "Ȑ", "Ȓ", "Ɍ")
                        ElseIf Letter = "r"c
                            Regex &= GenerateListofRegex("r", "ŕ", "ŗ", "ř", "ȑ", "ȓ", "ɍ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("r", "ŕ", "ŗ", "ř", "ȑ", "ȓ", "ɍ", "R", "®", "Ŕ", "Ŗ", "Ř", "Ʀ", "Ȑ", "Ȓ", "Ɍ")
                    End If
                Case "S"c, "s"c
                    If CaseSensitive Then
                        If Letter = "S"c Then
                            Regex &= GenerateListofRegex("\$", "5", "S", "§", "Ś", "Ŝ", "Ş", "Š", "Ƨ", "Ƽ", "Ș")
                        ElseIf Letter = "s"c
                            Regex &= GenerateListofRegex("s", "ś", "ŝ", "ş", "š", "ƨ", "ƽ", "ș", "ȿ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("s", "ś", "ŝ", "ş", "š", "ƨ", "ƽ", "ș", "ȿ", "\$", "5", "S", "§", "Ś", "Ŝ", "Ş", "Š", "Ƨ", "Ƽ", "Ș")
                    End If
                Case "T"c, "t"c
                    If CaseSensitive Then
                        If Letter = "T"c Then
                            Regex &= GenerateListofRegex("T", "Ţ", "Ť", "Ŧ", "Ƭ", "Ʈ", "Ț", "Ⱦ")
                        ElseIf Letter = "t"c
                            Regex &= GenerateListofRegex("t", "ţ", "ť", "ŧ", "ƫ", "ƭ", "ț", "ȶ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("t", "ţ", "ť", "ŧ", "ƫ", "ƭ", "ț", "ȶ", "T", "Ţ", "Ť", "Ŧ", "Ƭ", "Ʈ", "Ț", "Ⱦ")
                    End If
                Case "U"c, "u"c
                    If CaseSensitive Then
                        If Letter = "U"c Then
                            Regex &= GenerateListofRegex("U", "Ù", "Ú", "Û", "Ü", "Ũ", "Ū", "Ŭ", "Ů", "Ű", "Ų", "Ư", "Ʊ", "Ʋ", "Ǔ", "Ǖ", "Ǘ", "Ǚ", "Ǜ", "Ȕ", "Ȗ", "Ʉ")
                        ElseIf Letter = "u"c
                            Regex &= GenerateListofRegex("u", "µ", "ù", "ú", "û", "ü", "ũ", "ū", "ŭ", "ů", "ű", "ų", "ư", "ǔ", "ǖ", "ǘ", "ǚ", "ǜ", "ȕ", "ȗ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("u", "µ", "ù", "ú", "û", "ü", "ũ", "ū", "ŭ", "ů", "ű", "ų", "ư", "ǔ", "ǖ", "ǘ", "ǚ", "ǜ", "ȕ", "ȗ", "U", "Ù", "Ú", "Û", "Ü", "Ũ", "Ū", "Ŭ", "Ů", "Ű", "Ų", "Ư", "Ʊ", "Ʋ", "Ǔ", "Ǖ", "Ǘ", "Ǚ", "Ǜ", "Ȕ", "Ȗ", "Ʉ")
                    End If
                Case "V"c, "v"c
                    If CaseSensitive Then
                        If Letter = "V"c Then
                            Regex &= GenerateListofRegex("V", "Ɣ")
                        ElseIf Letter = "v"c
                            Regex &= GenerateListofRegex("v")
                        End If
                    Else
                        Regex &= GenerateListofRegex("v", "V", "Ɣ")
                    End If
                Case "W"c, "w"c
                    If CaseSensitive Then
                        If Letter = "W"c Then
                            Regex &= GenerateListofRegex("W", "Ŵ", "Ɯ")
                        ElseIf Letter = "w"c
                            Regex &= GenerateListofRegex("w", "ŵ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("w", "ŵ", "W", "Ŵ", "Ɯ")
                    End If
                Case "X"c, "x"c
                    If CaseSensitive Then
                        If Letter = "X"c Then
                            Regex &= GenerateListofRegex("X")
                        ElseIf Letter = "x"c
                            Regex &= GenerateListofRegex("x", "×")
                        End If
                    Else
                        Regex &= GenerateListofRegex("x", "×", "X")
                    End If
                Case "Y"c, "y"c
                    If CaseSensitive Then
                        If Letter = "Y"c Then
                            Regex &= GenerateListofRegex("Y", "¥", "Ý", "Ŷ", "Ÿ", "Ƴ", "Ȳ", "Ɏ")
                        ElseIf Letter = "y"c
                            Regex &= GenerateListofRegex("y", "ý", "ÿ", "ŷ", "ƴ", "ȳ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("y", "ý", "ÿ", "ŷ", "ƴ", "ȳ", "Y", "¥", "Ý", "Ŷ", "Ÿ", "Ƴ", "Ȳ", "Ɏ")
                    End If
                Case "Z"c, "z"c
                    If CaseSensitive Then
                        If Letter = "Z"c Then
                            Regex &= GenerateListofRegex("Z", "Ź", "Ż", "Ž", "Ƶ", "Ȥ")
                        ElseIf Letter = "z"c
                            Regex &= GenerateListofRegex("z", "ź", "ż", "ž", "ƶ", "ȥ", "ɀ")
                        End If
                    Else
                        Regex &= GenerateListofRegex("z", "ź", "ż", "ž", "ƶ", "ȥ", "ɀ", "Z", "Ź", "Ż", "Ž", "Ƶ", "Ȥ")
                    End If
                Case " "c
                    Regex &= GenerateListofRegex("\s")
            End Select
        Next
        Return Regex
    End Function

    Private Function GenerateListofRegex(ParamArray ByVal Letters() As String) As String
        Dim [Return] As String = "("
        For Each letter As String In Letters
            [Return] &= letter & "|"
        Next
        Return [Return] & ")"
    End Function
End Class

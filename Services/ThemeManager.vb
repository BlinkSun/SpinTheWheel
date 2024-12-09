Imports System.IO
Imports System.Text.Json

Namespace Services

    Public Class ThemeManager
        Private Shared ReadOnly ThemeFilePath As String = "theme.json"

        ''' <summary>
        ''' Loads the theme from a JSON file and updates application resources.
        ''' </summary>
        Public Shared Sub LoadTheme()
            If File.Exists(ThemeFilePath) Then
                Dim json As String = File.ReadAllText(ThemeFilePath)
                Dim theme As Dictionary(Of String, String) = JsonSerializer.Deserialize(Of Dictionary(Of String, String))(json)

                For Each keyValue In theme
                    If Application.Current.Resources.Contains(keyValue.Key) Then
                        Dim color = CType(ColorConverter.ConvertFromString(keyValue.Value), Color)
                        Application.Current.Resources(keyValue.Key) = color
                    End If
                Next
            End If
        End Sub

        ''' <summary>
        ''' Saves the theme to a JSON file.
        ''' </summary>
        Public Shared Sub SaveTheme(theme As Dictionary(Of String, String))
            Dim json As String = JsonSerializer.Serialize(theme, New JsonSerializerOptions With {.WriteIndented = True})
            File.WriteAllText(ThemeFilePath, json)
        End Sub
    End Class

End Namespace

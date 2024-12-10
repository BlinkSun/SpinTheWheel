Imports SpinTheWheel.Services
Imports SpinTheWheel.Views

Namespace ViewModels

    Public Class ThemeMananerViewModel
        Inherits ViewModelBase

        Public Property PrimaryColor As Color
        Public Property SecondaryColor As Color
        Public Property HoverColor As Color
        Public Property PressedColor As Color
        Public Property BackgroundLight As Color
        Public Property TextColor As Color
        Public Property TextColorInverse As Color
        Public Property DisabledBackground As Color
        Public Property DisabledBorder As Color
        Public Property DisabledForeground As Color


        Public ReadOnly Property SaveCommand As RelayCommand
        Public ReadOnly Property CancelCommand As RelayCommand

        Public Sub New()
            ' Initialize colors from current resources
            PrimaryColor = CType(Application.Current.Resources("PrimaryColor"), Color)
            SecondaryColor = CType(Application.Current.Resources("SecondaryColor"), Color)
            HoverColor = CType(Application.Current.Resources("HoverColor"), Color)
            PressedColor = CType(Application.Current.Resources("PressedColor"), Color)
            BackgroundLight = CType(Application.Current.Resources("BackgroundLight"), Color)
            TextColor = CType(Application.Current.Resources("TextColor"), Color)
            TextColorInverse = CType(Application.Current.Resources("TextColorInverse"), Color)
            DisabledBackground = CType(Application.Current.Resources("DisabledBackground"), Color)
            DisabledBorder = CType(Application.Current.Resources("DisabledBorder"), Color)
            DisabledForeground = CType(Application.Current.Resources("DisabledForeground"), Color)

            ' Initialize commands
            SaveCommand = New RelayCommand(AddressOf SaveTheme)
            CancelCommand = New RelayCommand(AddressOf Cancel)
        End Sub

        ''' <summary>
        ''' Saves the theme and updates the application resources.
        ''' </summary>
        Private Sub SaveTheme()
            ' Save colors to JSON
            Dim theme As New Dictionary(Of String, String) From {
                {"PrimaryColor", PrimaryColor.ToString()},
                {"SecondaryColor", SecondaryColor.ToString()},
                {"HoverColor", HoverColor.ToString()},
                {"PressedColor", PressedColor.ToString()},
                {"BackgroundLight", BackgroundLight.ToString()},
                {"TextColor", TextColor.ToString()},
                {"TextColorInverse", TextColorInverse.ToString()},
                {"DisabledBackground", DisabledBackground.ToString()},
                {"DisabledBorder", DisabledBorder.ToString()},
                {"DisabledForeground", DisabledForeground.ToString()}
            }

            ThemeManager.SaveTheme(theme)

            ' Update application resources
            For Each key In theme.Keys
                If Application.Current.Resources.Contains(key) Then
                    Application.Current.Resources(key) = CType(ColorConverter.ConvertFromString(theme(key)), Color)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Cancels the changes and closes the window.
        ''' </summary>
        Private Sub Cancel()
            ' Close the window
            For Each win As Window In Application.Current.Windows
                If TypeOf win Is ThemeManagerWindow Then
                    win.Close()
                    Exit For
                End If
            Next
        End Sub

    End Class

End Namespace

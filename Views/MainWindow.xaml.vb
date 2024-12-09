Namespace Views

    Partial Public Class MainWindow
        Inherits Window

        Private IsFullscreen As Boolean = False
        Private WindowStateCache As WindowState = WindowState.Normal

        Private Sub Window_KeyDown(sender As Object, e As KeyEventArgs)
            If e.Key = Key.F11 Then
                ToggleFullscreen()
            ElseIf e.Key = Key.F8 Then
                OpenTestControlsWindow()
            End If
        End Sub

        Private Sub ToggleFullscreen()
            If IsFullscreen Then
                WindowStyle = WindowStyle.SingleBorderWindow
                ResizeMode = ResizeMode.CanResize
                WindowState = WindowStateCache
                Topmost = False
            Else
                WindowStateCache = WindowState
                WindowStyle = WindowStyle.None
                ResizeMode = ResizeMode.NoResize
                WindowState = WindowState.Maximized
                Topmost = True
            End If
            IsFullscreen = Not IsFullscreen
        End Sub

        Private Sub OpenTestControlsWindow()
            Dim testControlsWindow As New TestControlsWindow With {
                .Owner = Me
            }
            testControlsWindow.ShowDialog()
        End Sub

    End Class

End Namespace

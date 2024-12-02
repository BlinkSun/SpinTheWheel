Namespace Services

    ''' <summary>
    ''' Service responsible for displaying error messages in a consistent manner across the application.
    ''' </summary>
    Public Class ErrorService

        ''' <summary>
        ''' Displays an error message in a message box.
        ''' </summary>
        ''' <param name="errorMessage">The message describing the error.</param>
        ''' <param name="title">The title of the error message box.</param>
        Public Shared Sub ShowError(errorMessage As String, Optional title As String = "Error")
            Try
                MessageBox.Show(errorMessage, title, MessageBoxButton.OK, MessageBoxImage.Error)
            Catch ex As Exception
                ' If MessageBox fails, write to Debug output as a fallback
                Debug.WriteLine($"Failed to display error message: {errorMessage}. Exception: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Displays a warning message in a message box.
        ''' </summary>
        ''' <param name="warningMessage">The message describing the warning.</param>
        ''' <param name="title">The title of the warning message box.</param>
        Public Shared Sub ShowWarning(warningMessage As String, Optional title As String = "Warning")
            Try
                MessageBox.Show(warningMessage, title, MessageBoxButton.OK, MessageBoxImage.Warning)
            Catch ex As Exception
                Debug.WriteLine($"Failed to display warning message: {warningMessage}. Exception: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Displays an informational message in a message box.
        ''' </summary>
        ''' <param name="infoMessage">The informational message to display.</param>
        ''' <param name="title">The title of the informational message box.</param>
        Public Shared Sub ShowInfo(infoMessage As String, Optional title As String = "Information")
            Try
                MessageBox.Show(infoMessage, title, MessageBoxButton.OK, MessageBoxImage.Information)
            Catch ex As Exception
                Debug.WriteLine($"Failed to display informational message: {infoMessage}. Exception: {ex.Message}")
            End Try
        End Sub

    End Class

End Namespace

Namespace Services

    ''' <summary>
    ''' Service responsible for displaying error messages in a consistent manner across the application.
    ''' </summary>
    Public Class ErrorService
        Private Shared ReadOnly MainWindow As Window = Application.Current.MainWindow

        ''' <summary>
        ''' Displays an error message in a message box.
        ''' If the MessageBox fails, logs the error to the Event Viewer.
        ''' </summary>
        ''' <param name="errorMessage">The message describing the error.</param>
        ''' <param name="title">The title of the error message box.</param>
        Public Shared Sub ShowError(errorMessage As String, Optional title As String = "Error")
            Try
                MessageBox.Show(MainWindow, errorMessage, title, MessageBoxButton.OK, MessageBoxImage.Error)
            Catch ex As Exception
                LogToEventViewer($"Failed to display error message: {errorMessage}. Exception: {ex.Message}", EventLogEntryType.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Displays a warning message in a message box.
        ''' If the MessageBox fails, logs the warning to the Event Viewer.
        ''' </summary>
        ''' <param name="warningMessage">The message describing the warning.</param>
        ''' <param name="title">The title of the warning message box.</param>
        Public Shared Sub ShowWarning(warningMessage As String, Optional title As String = "Warning")
            Try
                MessageBox.Show(MainWindow, warningMessage, title, MessageBoxButton.OK, MessageBoxImage.Warning)
            Catch ex As Exception
                LogToEventViewer($"Failed to display warning message: {warningMessage}. Exception: {ex.Message}", EventLogEntryType.Warning)
            End Try
        End Sub

        ''' <summary>
        ''' Displays an informational message in a message box.
        ''' If the MessageBox fails, logs the information to the Event Viewer.
        ''' </summary>
        ''' <param name="infoMessage">The informational message to display.</param>
        ''' <param name="title">The title of the informational message box.</param>
        Public Shared Sub ShowInfo(infoMessage As String, Optional title As String = "Information")
            Try
                MessageBox.Show(MainWindow, infoMessage, title, MessageBoxButton.OK, MessageBoxImage.Information)
            Catch ex As Exception
                LogToEventViewer($"Failed to display informational message: {infoMessage}. Exception: {ex.Message}", EventLogEntryType.Information)
            End Try
        End Sub

        ''' <summary>
        ''' Logs a message to the Event Viewer.
        ''' </summary>
        ''' <param name="message">The message to log.</param>
        ''' <param name="type">The type of event to log (Error, Warning, Information).</param>
        Public Shared Sub LogToEventViewer(message As String, type As EventLogEntryType)
            Dim source As String = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "").Replace(".dll", "")
            Dim logName As String = "Application"

            Try
                If Not EventLog.SourceExists(source) Then
                    EventLog.CreateEventSource(source, logName)
                End If

                Using eventLog As New EventLog(logName)
                    eventLog.Source = source
                    eventLog.WriteEntry(message, type)
                End Using
            Catch ex As Exception
                ' In case logging to the Event Viewer fails, fallback to Debug output
                Debug.WriteLine($"Failed to write to Event Viewer: {message}. Exception: {ex.Message}")
            End Try
        End Sub

    End Class

End Namespace
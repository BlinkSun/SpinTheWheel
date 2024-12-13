Imports SpinTheWheel.Services

Namespace ViewModels

    ''' <summary>
    ''' Locator to provide instances of ViewModels for binding in XAML.
    ''' </summary>
    Public Class ViewModelLocator

        Private ReadOnly databaseService As DatabaseService
        Public ReadOnly Property MainViewModel As MainViewModel
        Public ReadOnly Property ParticipantsViewModel As ParticipantsViewModel
        Public ReadOnly Property ThemeManagerViewModel As ThemeManagerViewModel
        Public Sub New()
            Try
                databaseService = Application.DatabaseService
                MainViewModel = New MainViewModel(databaseService)
                ParticipantsViewModel = New ParticipantsViewModel(databaseService)
                ThemeManagerViewModel = New ThemeManagerViewModel()
            Catch ex As Exception
                ErrorService.LogToEventViewer($"Error in ViewModelLocator: {ex.Message}", EventLogEntryType.Error)
                Throw
            End Try
        End Sub

    End Class

End Namespace
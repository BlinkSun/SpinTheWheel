Imports SpinTheWheel.Services

Namespace ViewModels

    ''' <summary>
    ''' Locator to provide instances of ViewModels for binding in XAML.
    ''' </summary>
    Public Class ViewModelLocator

        ''' <summary>
        ''' Singleton instance of the DatabaseService.
        ''' </summary>
        Private ReadOnly databaseService As DatabaseService

        ''' <summary>
        ''' Provides the MainViewModel instance.
        ''' </summary>
        Public ReadOnly Property MainViewModel As MainViewModel

        ''' <summary>
        ''' Provides the ParticipantsViewModel instance.
        ''' </summary>
        Public ReadOnly Property ParticipantsViewModel As ParticipantsViewModel

        ''' <summary>
        ''' Initializes a new instance of the ViewModelLocator.
        ''' </summary>
        Public Sub New()
            Try
                ' Initialize DatabaseService and ViewModels
                databaseService = Application.DatabaseService
                MainViewModel = New MainViewModel(databaseService)
                ParticipantsViewModel = New ParticipantsViewModel(databaseService)
            Catch ex As Exception
                ' Log or rethrow exception for debugging
                Debug.WriteLine($"Error in ViewModelLocator: {ex.Message}")
                Throw
            End Try
        End Sub
    End Class
End Namespace
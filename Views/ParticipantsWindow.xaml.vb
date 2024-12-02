Imports SpinTheWheel.ViewModels

Partial Public Class ParticipantsWindow
    Private Sub ParticipantsWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim viewModel As ParticipantsViewModel = TryCast(DataContext, ParticipantsViewModel)
        viewModel?.RefreshParticipants()
    End Sub
End Class

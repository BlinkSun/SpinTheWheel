Imports SpinTheWheel.ViewModels

Namespace Views

    Partial Public Class ParticipantsWindow
        Inherits Window

        Private Sub ParticipantsWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            Dim viewModel As ParticipantsViewModel = TryCast(DataContext, ParticipantsViewModel)
            viewModel?.LoadParticipants()
        End Sub

    End Class

End Namespace

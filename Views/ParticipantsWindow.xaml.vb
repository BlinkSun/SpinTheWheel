Imports SpinTheWheel.ViewModels

Namespace Views

    Partial Public Class ParticipantsWindow
        Inherits Window

        Private Sub ParticipantsWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            Dim viewModel As ParticipantsViewModel = TryCast(DataContext, ParticipantsViewModel)
            viewModel?.GetParticipants()
        End Sub

        Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)
            Me.Close()
        End Sub
    End Class

End Namespace

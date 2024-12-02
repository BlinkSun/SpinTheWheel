Imports System.Collections.ObjectModel
Imports SpinTheWheel.Models
Imports SpinTheWheel.Services

Namespace ViewModels

    ''' <summary>
    ''' ViewModel for managing participants.
    ''' </summary>
    Public Class ParticipantsViewModel
        Inherits ViewModelBase

        ''' <summary>
        ''' Instance of the database service for accessing participants.
        ''' </summary>
        Private ReadOnly databaseService As DatabaseService

        ''' <summary>
        ''' Collection of participants displayed in the UI.
        ''' </summary>
        Public Property Participants As ObservableCollection(Of Participant)

        ''' <summary>
        ''' Currently selected participant in the UI.
        ''' </summary>
        Private selectedParticipantValue As Participant
        Public Property SelectedParticipant As Participant
            Get
                Return selectedParticipantValue
            End Get
            Set(value As Participant)
                selectedParticipantValue = value
                OnPropertyChanged(NameOf(SelectedParticipant))
            End Set
        End Property

        ''' <summary>
        ''' Command to add a new participant.
        ''' </summary>
        Public ReadOnly Property AddCommand As ICommand

        ''' <summary>
        ''' Command to delete the selected participant.
        ''' </summary>
        Public ReadOnly Property DeleteCommand As ICommand

        ''' <summary>
        ''' Command to save changes made to participants.
        ''' </summary>
        Public ReadOnly Property SaveCommand As ICommand

        ''' <summary>
        ''' Command to refresh list of participants.
        ''' </summary>
        Public ReadOnly Property RefreshCommand As ICommand

        ''' <summary>
        ''' Constructor for ParticipantsViewModel.
        ''' </summary>
        ''' <param name="dbService">Instance of the database service.</param>
        Public Sub New(dbService As DatabaseService)
            ArgumentNullException.ThrowIfNull(dbService)

            databaseService = dbService
            Participants = New ObservableCollection(Of Participant)()

            AddCommand = New RelayCommand(AddressOf AddParticipant)
            DeleteCommand = New RelayCommand(AddressOf DeleteParticipant, Function() SelectedParticipant IsNot Nothing)
            SaveCommand = New RelayCommand(AddressOf SaveParticipants)
            RefreshCommand = New RelayCommand(AddressOf RefreshParticipants)

            ' Load participants at initialization
            'RefreshParticipants()
        End Sub

        ''' <summary>
        ''' Refreshes the participants list from the database.
        ''' </summary>
        Public Sub RefreshParticipants()
            Try
                Participants.Clear()
                Dim participantsFromDb = databaseService.GetParticipants()
                For Each participant In participantsFromDb
                    Participants.Add(participant)
                Next
            Catch ex As Exception
                ErrorService.ShowError($"Error refreshing participants: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Adds a new participant via a prompt.
        ''' </summary>
        Private Sub AddParticipant()
            Try
                Dim newName As String = InputBox("Enter the name of the new participant:", "Add Participant")
                If Not String.IsNullOrWhiteSpace(newName) Then
                    Dim newParticipant = New Participant() With {
                        .Name = newName,
                        .Done = False
                    }
                    databaseService.AddParticipant(newParticipant)
                    Participants.Add(newParticipant)
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Error adding a new participant: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Deletes the selected participant.
        ''' </summary>
        Private Sub DeleteParticipant()
            Try
                If SelectedParticipant IsNot Nothing Then
                    databaseService.DeleteParticipant(SelectedParticipant.Id)
                    Participants.Remove(SelectedParticipant)
                    SelectedParticipant = Nothing
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Error deleting participant: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Saves the current state of participants to the database.
        ''' </summary>
        Private Sub SaveParticipants()
            Try
                For Each participant In Participants
                    databaseService.UpdateParticipant(participant)
                Next
                ErrorService.ShowInfo("Participants saved successfully.")
            Catch ex As Exception
                ErrorService.ShowError($"Error saving participants: {ex.Message}")
            End Try
        End Sub

    End Class

End Namespace

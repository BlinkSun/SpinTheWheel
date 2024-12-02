Imports System.Collections.ObjectModel
Imports SpinTheWheel.Models
Imports SpinTheWheel.Services

Namespace ViewModels

    ''' <summary>
    ''' ViewModel for managing participants.
    ''' </summary>
    Public Class ParticipantsViewModel
        Inherits ViewModelBase

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
        ''' Commands for UI interactions.
        ''' </summary>
        Public ReadOnly Property AddCommand As ICommand
        Public ReadOnly Property DeleteCommand As ICommand
        Public ReadOnly Property SaveCommand As ICommand

        ''' <summary>
        ''' Constructor for ParticipantsViewModel.
        ''' </summary>
        ''' <param name="dbService">Database service instance.</param>
        Public Sub New(dbService As DatabaseService)
            databaseService = dbService
            Participants = New ObservableCollection(Of Participant)(dbService.GetParticipants())

            AddCommand = New RelayCommand(AddressOf AddParticipant)
            DeleteCommand = New RelayCommand(AddressOf DeleteParticipant, Function() SelectedParticipant IsNot Nothing)
            SaveCommand = New RelayCommand(AddressOf SaveParticipants)
        End Sub

        ''' <summary>
        ''' Loads participants from the database.
        ''' </summary>
        Public Sub LoadParticipants()
            Try
                Participants.Clear()
                For Each participant In databaseService.GetParticipants()
                    Participants.Add(participant)
                Next
            Catch ex As Exception
                MessageBox.Show($"Error loading participants: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Adds a new participant via a prompt.
        ''' </summary>
        Private Sub AddParticipant()
            Dim newName As String = InputBox("Enter the name of the new participant:", "Add Participant")
            If Not String.IsNullOrWhiteSpace(newName) Then
                Dim newParticipant = New Participant() With {.Name = newName, .Done = False}
                databaseService.AddParticipant(newParticipant)
                Participants.Add(newParticipant)
            End If
        End Sub

        ''' <summary>
        ''' Deletes the selected participant.
        ''' </summary>
        Private Sub DeleteParticipant()
            If SelectedParticipant IsNot Nothing Then
                databaseService.DeleteParticipant(SelectedParticipant.Id)
                Participants.Remove(SelectedParticipant)
                SelectedParticipant = Nothing
            End If
        End Sub

        ''' <summary>
        ''' Saves the current state of participants to the database.
        ''' </summary>
        Private Sub SaveParticipants()
            For Each participant In Participants
                databaseService.UpdateParticipant(participant)
            Next
        End Sub

    End Class

End Namespace
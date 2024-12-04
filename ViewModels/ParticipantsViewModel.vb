Imports System.Collections.ObjectModel
Imports SpinTheWheel.Models
Imports SpinTheWheel.Services

Namespace ViewModels

    Public Class ParticipantsViewModel
        Inherits ViewModelBase

        Private ReadOnly databaseService As DatabaseService
        Public Property Participants As ObservableCollection(Of Participant)

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

        Public ReadOnly Property AddCommand As ICommand
        Public ReadOnly Property DeleteCommand As ICommand
        Public ReadOnly Property SaveCommand As ICommand
        Public ReadOnly Property RefreshCommand As ICommand
        Public ReadOnly Property OKCommand As ICommand
        Public ReadOnly Property CancelCommand As ICommand

        Public Sub New(dbService As DatabaseService)
            ArgumentNullException.ThrowIfNull(dbService)

            databaseService = dbService
            Participants = New ObservableCollection(Of Participant)()

            AddCommand = New RelayCommand(AddressOf AddParticipant)
            DeleteCommand = New RelayCommand(AddressOf DeleteParticipant, Function() SelectedParticipant IsNot Nothing)
            SaveCommand = New RelayCommand(AddressOf SaveParticipants)
            RefreshCommand = New RelayCommand(AddressOf RefreshParticipants)
            ApplyFilterCommand = New RelayCommand(AddressOf ApplyFilter)
            GoToPageCommand = New RelayCommand(Of Integer)(AddressOf GoToPage)
            OKCommand = New RelayCommand(AddressOf OnOKClicked)
            CancelCommand = New RelayCommand(AddressOf OnCancelClicked)

            ' Load participants at initialization
            'RefreshParticipants()
        End Sub
        Private Sub OnOKClicked()
            ' Logique pour le bouton OK
            Console.WriteLine("OK clicked")
        End Sub

        Private Sub OnCancelClicked()
            ' Logique pour le bouton Cancel
            Console.WriteLine("Cancel clicked")
        End Sub
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


        Public Property FilterText As String
        Public Property CurrentPage As Integer = 1
        Public Property ItemsPerPage As Integer = 10
        Public Property PagedParticipants As ObservableCollection(Of Participant)
        Public Property PageNumbers As ObservableCollection(Of Integer)
        Public ReadOnly Property ApplyFilterCommand As ICommand
        Public ReadOnly Property GoToPageCommand As ICommand
        Private Sub ApplyFilter()
            Dim filteredList = If(String.IsNullOrWhiteSpace(FilterText),
                                  Participants,
                                  Participants.Where(Function(p) p.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase)).ToList())
            UpdatePagination(filteredList)
        End Sub
        Private Sub UpdatePagination(participants As IEnumerable(Of Participant))
            Dim totalItems = participants.Count()
            Dim totalPages = Math.Ceiling(totalItems / ItemsPerPage)
            PageNumbers = New ObservableCollection(Of Integer)(Enumerable.Range(1, totalPages))

            ' Charger la page actuelle
            PagedParticipants = New ObservableCollection(Of Participant)(participants.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage))
            OnPropertyChanged(NameOf(PagedParticipants))
            OnPropertyChanged(NameOf(PageNumbers))
        End Sub
        Private Sub GoToPage(pageNumber As Integer)
            If pageNumber <> CurrentPage Then
                CurrentPage = pageNumber
                ApplyFilter() ' Réapplique la pagination
            End If
        End Sub

    End Class

End Namespace

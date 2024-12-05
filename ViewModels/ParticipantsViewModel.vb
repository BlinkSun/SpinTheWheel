Imports System.Collections.ObjectModel
Imports System.IO
Imports SpinTheWheel.Models
Imports SpinTheWheel.Services

Namespace ViewModels

    Public Class ParticipantsViewModel
        Inherits ViewModelBase

        Private ReadOnly databaseService As DatabaseService

        Public Property FilterText As String
            Get
                Return filterTextValue
            End Get
            Set(value As String)
                filterTextValue = value
                OnPropertyChanged(NameOf(FilterText))
            End Set
        End Property
        Private filterTextValue As String

        Public Property Participants As ObservableCollection(Of Participant)
            Get
                Return participantsValue
            End Get
            Set(value As ObservableCollection(Of Participant))
                participantsValue = value
                OnPropertyChanged(NameOf(Participants))
            End Set
        End Property
        Private participantsValue As ObservableCollection(Of Participant)

        Public Property FilteredParticipants As ObservableCollection(Of Participant)
            Get
                Return filteredParticipantsValue
            End Get
            Set(value As ObservableCollection(Of Participant))
                filteredParticipantsValue = value
                OnPropertyChanged(NameOf(FilteredParticipants))
            End Set
        End Property
        Private filteredParticipantsValue As ObservableCollection(Of Participant)

        Public Property IsUpdateModalPopupVisible As Boolean
            Get
                Return isUpdateModalPopupVisibleValue
            End Get
            Set(value As Boolean)
                isUpdateModalPopupVisibleValue = value
                OnPropertyChanged(NameOf(IsUpdateModalPopupVisible))
            End Set
        End Property
        Private isUpdateModalPopupVisibleValue As Boolean

        Public Property IsAddModalPopupVisible As Boolean
            Get
                Return isAddModalPopupVisibleValue
            End Get
            Set(value As Boolean)
                isAddModalPopupVisibleValue = value
                OnPropertyChanged(NameOf(IsAddModalPopupVisible))
            End Set
        End Property
        Private isAddModalPopupVisibleValue As Boolean

        Public Property IsDeleteModalPopupVisible As Boolean
            Get
                Return isDeleteModalPopupVisibleValue
            End Get
            Set(value As Boolean)
                isDeleteModalPopupVisibleValue = value
                OnPropertyChanged(NameOf(IsDeleteModalPopupVisible))
            End Set
        End Property
        Private isDeleteModalPopupVisibleValue As Boolean

        Public Property SelectedParticipantDescription As String
            Get
                Return selectedParticipantDescriptionValue
            End Get
            Set(value As String)
                selectedParticipantDescriptionValue = value
                OnPropertyChanged(NameOf(SelectedParticipantDescription))
            End Set
        End Property
        Private selectedParticipantDescriptionValue As String
        Public Property SelectedParticipant As Participant
            Get
                Return selectedParticipantValue
            End Get
            Set(value As Participant)
                selectedParticipantValue = value
                OnPropertyChanged(NameOf(SelectedParticipant))
                If SelectedParticipant IsNot Nothing Then
                    If SelectedParticipant.Done Then
                        SelectedParticipantDescription = $"Ce participant a déjà été tiré au sort. Il est le gagant du tirage numéro {SelectedParticipant.Order}."
                    Else
                        SelectedParticipantDescription = "Ce participant fait toujours parti du tirage. Il n'a pas encore été tiré au sort."
                    End If
                End If
            End Set
        End Property
        Private selectedParticipantValue As Participant

        Public ReadOnly Property AddCommand As RelayCommand
        Public ReadOnly Property UpdateCommand As RelayCommand(Of Participant)
        Public ReadOnly Property DeleteCommand As RelayCommand(Of Participant)
        Public ReadOnly Property RefreshCommand As RelayCommand
        Public ReadOnly Property OnAddCommand As RelayCommand(Of String)
        Public ReadOnly Property OnUpdateCommand As RelayCommand(Of String)
        Public ReadOnly Property OnDeleteCommand As RelayCommand(Of Participant)
        Public ReadOnly Property OnCancelCommand As RelayCommand
        Public ReadOnly Property ApplyFilterCommand As RelayCommand
        Public ReadOnly Property ImportParticipantsCommand As RelayCommand

        Public Sub New(dbService As DatabaseService)
            ArgumentNullException.ThrowIfNull(dbService)

            databaseService = dbService
            Participants = New ObservableCollection(Of Participant)()

            AddCommand = New RelayCommand(AddressOf AddParticipant)
            DeleteCommand = New RelayCommand(Of Participant)(AddressOf DeleteParticipant, Function() SelectedParticipant IsNot Nothing)
            UpdateCommand = New RelayCommand(Of Participant)(AddressOf UpdateParticipant, Function() SelectedParticipant IsNot Nothing)
            RefreshCommand = New RelayCommand(AddressOf LoadParticipants)
            OnAddCommand = New RelayCommand(Of String)(AddressOf OnAddClicked)
            OnUpdateCommand = New RelayCommand(Of String)(AddressOf OnUpdateClicked)
            OnDeleteCommand = New RelayCommand(Of Participant)(AddressOf OnDeleteClicked)
            OnCancelCommand = New RelayCommand(AddressOf OnCancelClicked)
            ApplyFilterCommand = New RelayCommand(AddressOf ApplyFilter)
            ImportParticipantsCommand = New RelayCommand(AddressOf ImportParticipants)

            'LoadParticipants()
        End Sub

        Public Sub LoadParticipants()
            Try
                Participants.Clear()
                Dim participantsFromDb = databaseService.GetParticipants()
                For Each participant In participantsFromDb
                    Participants.Add(participant)
                Next
                ApplyFilter()
            Catch ex As Exception
                ErrorService.ShowError($"Erreur lors du chargement des participants : {ex.Message}")
            End Try
        End Sub
        Private Sub AddParticipant()
            IsAddModalPopupVisible = True
        End Sub
        Private Sub DeleteParticipant()
            IsDeleteModalPopupVisible = True
        End Sub
        Private Sub UpdateParticipant()
            IsUpdateModalPopupVisible = True
        End Sub
        Private Sub OnAddClicked(name As String)
            Try
                If Not String.IsNullOrWhiteSpace(name) Then
                    Dim newParticipant = New Participant() With {
                        .Name = name
                    }
                    databaseService.AddParticipant(newParticipant)
                    Participants.Add(newParticipant)
                    ApplyFilter()
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Error adding a new participant: {ex.Message}")
            End Try
        End Sub
        Private Sub OnUpdateClicked(name As String)
            Try
                If SelectedParticipant IsNot Nothing Then
                    SelectedParticipant.Name = name
                    databaseService.UpdateParticipant(SelectedParticipant)
                    ApplyFilter()
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Erreur lors de la mise à jour du participant : {ex.Message}")
            End Try
        End Sub
        Private Sub OnDeleteClicked()
            Try
                If SelectedParticipant IsNot Nothing Then
                    databaseService.DeleteParticipant(SelectedParticipant)
                    Participants.Remove(SelectedParticipant)
                    SelectedParticipant = Nothing
                    ApplyFilter()
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Erreur lors de la suppression du participant : {ex.Message}")
            End Try
        End Sub
        Private Sub OnCancelClicked()
            Console.WriteLine("Cancel clicked")
        End Sub
        Private Sub ApplyFilter()
            Dim filteredList As List(Of Participant) = If(String.IsNullOrWhiteSpace(FilterText),
                                  Participants.ToList,
                                  Participants.Where(Function(p) p.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase)).ToList())
            FilteredParticipants = New ObservableCollection(Of Participant)(filteredList)
            OnPropertyChanged(NameOf(FilteredParticipants))
        End Sub
        Private Sub ImportParticipants()
            Try
                Dim openFileDialog As New Microsoft.Win32.OpenFileDialog With {
                    .Title = "Sélectionner un fichier de participants",
                    .Filter = "Fichiers texte (*.txt)|*.txt",
                    .Multiselect = False
                }

                If openFileDialog.ShowDialog() = True Then
                    Dim filePath As String = openFileDialog.FileName
                    If Not IO.File.Exists(filePath) Then
                        Throw New FileNotFoundException("Le fichier sélectionné est introuvable.")
                    End If
                    Using fileStream = IO.File.Open(filePath, FileMode.Open, FileAccess.Read)
                        ' Fichier accessible, on ferme le flux immédiatement
                    End Using
                    Dim lines As String() = File.ReadAllLines(filePath)
                    If lines.Length = 0 Then
                        Throw New InvalidDataException("Le fichier est vide.")
                    End If
                    Dim invalidLines As New List(Of String)()
                    For Each line In lines
                        If Not String.IsNullOrWhiteSpace(line) Then
                            Dim trimmedLine = line.Trim()
                            Dim parts = trimmedLine.Split(" "c, StringSplitOptions.RemoveEmptyEntries)
                            If parts.Length < 2 Then
                                invalidLines.Add(trimmedLine)
                                Continue For
                            End If
                            Dim fullName = String.Join(" ", parts)
                            Dim participant As New Participant() With {
                                .Name = fullName
                            }
                            databaseService.AddParticipant(participant)
                        End If
                    Next

                    If invalidLines.Count > 0 Then
                        ErrorService.ShowWarning($"{invalidLines.Count} ligne(s) ont été ignorées en raison d'un format invalide :{Environment.NewLine}{String.Join(Environment.NewLine, invalidLines.Take(5))}...", "Importation partielle")
                    Else
                        ErrorService.ShowInfo("Tous les participants ont été importés avec succès !", "Importation réussie")
                    End If

                    LoadParticipants()
                End If
            Catch ex As FileNotFoundException
                ErrorService.ShowError($"Erreur : {ex.Message}", "Fichier introuvable")
            Catch ex As IOException
                ErrorService.ShowError($"Erreur : Le fichier est déjà utilisé par une autre application ou est inaccessible.{Environment.NewLine}{ex.Message}", "Fichier inaccessible")
            Catch ex As InvalidDataException
                ErrorService.ShowError($"Erreur : {ex.Message}", "Format de fichier invalide")
            Catch ex As Exception
                ErrorService.ShowError($"Erreur inattendue : {ex.Message}", "Erreur")
            End Try
        End Sub

    End Class

End Namespace

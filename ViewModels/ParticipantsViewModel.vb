Imports System.Collections.ObjectModel
Imports System.IO
Imports SpinTheWheel.Models
Imports SpinTheWheel.Services

Namespace ViewModels

    ''' <summary>
    ''' ViewModel for managing participants in the application.
    ''' Provides functionality for filtering, adding, updating, and importing participants.
    ''' </summary>
    Public Class ParticipantsViewModel
        Inherits ViewModelBase

        ''' <summary>
        ''' Service for accessing the database.
        ''' </summary>
        Private ReadOnly databaseService As DatabaseService

        ''' <summary>
        ''' Gets or sets the text used for filtering participants.
        ''' </summary>
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

        ''' <summary>
        ''' Gets or sets the collection of all participants.
        ''' </summary>
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

        ''' <summary>
        ''' Gets or sets the filtered collection of participants based on <see cref="FilterText"/>.
        ''' </summary>
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

        ''' <summary>
        ''' Gets or sets the description of the currently selected participant.
        ''' </summary>
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

        ''' <summary>
        ''' Gets or sets the currently selected participant.
        ''' </summary>
        Public Property SelectedParticipant As Participant
            Get
                Return selectedParticipantValue
            End Get
            Set(value As Participant)
                selectedParticipantValue = value
                OnPropertyChanged(NameOf(SelectedParticipant))
                UpdateParticipantDescription()
            End Set
        End Property
        Private selectedParticipantValue As Participant

        ''' <summary>
        ''' Command to add a new participant.
        ''' </summary>
        Public ReadOnly Property AddCommand As RelayCommand

        ''' <summary>
        ''' Command to update the selected participant.
        ''' </summary>
        Public ReadOnly Property UpdateCommand As RelayCommand(Of Participant)

        ''' <summary>
        ''' Command to reset all participants to their default state.
        ''' </summary>
        Public ReadOnly Property ResetCommand As RelayCommand

        ''' <summary>
        ''' Command to mark the selected participant as done.
        ''' </summary>
        Public ReadOnly Property UpdateDoneCommand As RelayCommand(Of Participant)

        ''' <summary>
        ''' Command to delete the selected participant.
        ''' </summary>
        Public ReadOnly Property DeleteCommand As RelayCommand(Of Participant)

        ''' <summary>
        ''' Command to clear all participants.
        ''' </summary>
        Public ReadOnly Property ClearCommand As RelayCommand

        ''' <summary>
        ''' Command to apply the filter to the participant list.
        ''' </summary>
        Public ReadOnly Property ApplyFilterCommand As RelayCommand

        ''' <summary>
        ''' Command to import participants from a file.
        ''' </summary>
        Public ReadOnly Property ImportParticipantsCommand As RelayCommand

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ParticipantsViewModel"/> class.
        ''' </summary>
        ''' <param name="dbService">The database service used to manage participants.</param>
        Public Sub New(dbService As DatabaseService)
            ArgumentNullException.ThrowIfNull(dbService)

            databaseService = dbService
            Participants = New ObservableCollection(Of Participant)()

            AddCommand = New RelayCommand(AddressOf AddParticipant)
            UpdateCommand = New RelayCommand(Of Participant)(AddressOf UpdateParticipant, Function() SelectedParticipant IsNot Nothing)
            ResetCommand = New RelayCommand(AddressOf ResetParticipants)
            UpdateDoneCommand = New RelayCommand(Of Participant)(AddressOf UpdateParticipantDone, Function() SelectedParticipant IsNot Nothing)
            DeleteCommand = New RelayCommand(Of Participant)(AddressOf DeleteParticipant, Function() SelectedParticipant IsNot Nothing)
            ClearCommand = New RelayCommand(AddressOf ClearParticipants)
            ApplyFilterCommand = New RelayCommand(AddressOf ApplyFilter)
            ImportParticipantsCommand = New RelayCommand(AddressOf ImportParticipants)
        End Sub

        ''' <summary>
        ''' Retrieves the list of participants from the database and updates the <see cref="Participants"/> collection.
        ''' </summary>
        Public Sub GetParticipants()
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
        ''' <summary>
        ''' Adds a new participant.
        ''' </summary>
        Private Sub AddParticipant()
            Try
                SelectedParticipant = Nothing
                Dim name As String = InputBox("Entrez le nom de ce nouveau participant :", "Ajouter Participant")
                If Not String.IsNullOrWhiteSpace(name) Then
                    Dim newParticipant = New Participant() With {
                        .Name = name
                    }
                    databaseService.AddParticipant(newParticipant)
                    Participants.Add(newParticipant)
                    SelectedParticipant = newParticipant
                    ApplyFilter()
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Erreur lors de la création d'un nouveau participant: {ex.Message}")
            End Try
        End Sub
        ''' <summary>
        ''' Updates the selected participant's information.
        ''' </summary>
        Private Sub UpdateParticipant()
            Try
                If SelectedParticipant IsNot Nothing Then
                    Dim newName As String = InputBox("Entrez un nouveau nom pour ce participant :", "Modifier Participant", SelectedParticipant.Name)
                    If Not String.IsNullOrWhiteSpace(newName) Then
                        SelectedParticipant.Name = newName
                        databaseService.UpdateParticipant(SelectedParticipant)
                        ApplyFilter()
                    End If
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Erreur lors de la mise à jour du participant : {ex.Message}")
            End Try
        End Sub
        ''' <summary>
        ''' Marks the selected participant as done and updates the database.
        ''' </summary>
        Private Sub UpdateParticipantDone()
            Try
                If SelectedParticipant IsNot Nothing Then
                    If SelectedParticipant.Done Then
                        Dim order As Integer = databaseService.GetNextOrderValue()
                        SelectedParticipant.Order = order
                    Else
                        SelectedParticipant.Order = -1
                    End If
                    databaseService.UpdateParticipant(SelectedParticipant)
                    UpdateParticipantDescription()
                    ApplyFilter()
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Erreur lors de la mise à jour du participant : {ex.Message}")
            End Try
        End Sub
        ''' <summary>
        ''' Updates the description of the currently selected participant.
        ''' </summary>
        Private Sub UpdateParticipantDescription()
            If SelectedParticipant IsNot Nothing Then
                If SelectedParticipant.Done Then
                    SelectedParticipantDescription = $"Ce participant a déjà été tiré au sort. Il est le gagant du tirage numéro {SelectedParticipant.Order}."
                Else
                    SelectedParticipantDescription = "Ce participant fait toujours parti du tirage. Il n'a pas encore été tiré au sort."
                End If
            End If
        End Sub
        ''' <summary>
        ''' Reset the done flag of the selected participant.
        ''' </summary>
        Private Sub ResetParticipants()
            Try
                If MessageBox.Show("Tous les participants seront réinitialisés à l'état non tiré. Voulez-vous continuer ?", "Réinitialiser le tirage.", MessageBoxButton.YesNo) = MessageBoxResult.Yes Then
                    SelectedParticipant = Nothing
                    databaseService.ResetParticipants()
                    GetParticipants()
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Erreur lors de la mise à jour de tous les participants : {ex.Message}")
            End Try
        End Sub
        ''' <summary>
        ''' Delete the selected participant.
        ''' </summary>
        Private Sub DeleteParticipant()
            Try
                If SelectedParticipant IsNot Nothing Then
                    If MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce participant ?", "Suppression d'un participant", MessageBoxButton.YesNo) = MessageBoxResult.Yes Then
                        databaseService.DeleteParticipant(SelectedParticipant)
                        Participants.Remove(SelectedParticipant)
                        SelectedParticipant = Nothing
                        ApplyFilter()
                    End If
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Erreur lors de la suppression du participant : {ex.Message}")
            End Try
        End Sub
        ''' <summary>
        ''' Delete all participant.
        ''' </summary>
        Private Sub ClearParticipants()
            Try
                If MessageBox.Show("Êtes-vous sûr de vouloir effacer tous les participants de la liste ?" & vbCrLf & "Cette action est irréversible.", "Suppression de tous les participants", MessageBoxButton.YesNo) = MessageBoxResult.Yes Then
                    SelectedParticipant = Nothing
                    databaseService.ClearParticipants()
                    Participants.Clear()
                    ApplyFilter()
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Erreur lors de la suppression des participants : {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Apply filter to participants list.
        ''' </summary>
        Private Sub ApplyFilter()
            Dim filteredList As List(Of Participant) = If(String.IsNullOrWhiteSpace(FilterText),
                                  Participants.ToList,
                                  Participants.Where(Function(p) p.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase)).ToList())
            filteredList = filteredList.OrderByDescending(Function(p) p.Done).ThenBy(Function(p) p.Order).ToList()
            FilteredParticipants = New ObservableCollection(Of Participant)(filteredList)
            OnPropertyChanged(NameOf(FilteredParticipants))
        End Sub
        ''' <summary>
        ''' Import participants list from file.
        ''' </summary>
        Private Async Sub ImportParticipants()
            Try
                Dim openFileDialog As New Microsoft.Win32.OpenFileDialog With {
                    .Title = "Sélectionner un fichier de participants",
                    .Filter = "Fichiers texte (*.txt)|*.txt|Fichiers csv (*.csv)|*.csv|Tous les fichiers (*.*)|*.*",
                    .Multiselect = False
                }

                If openFileDialog.ShowDialog() = True Then
                    Dim filePath As String = openFileDialog.FileName
                    If Not IO.File.Exists(filePath) Then
                        Throw New FileNotFoundException("Le fichier sélectionné est introuvable.")
                    End If
                    Using fileStream = IO.File.Open(filePath, FileMode.Open, FileAccess.Read)
                        ' File not available
                    End Using

                    Mouse.OverrideCursor = Cursors.Wait

                    Dim result As (ValidParticipants As List(Of Participant), InvalidLines As List(Of String))

                    result = Await Task.Run(Function() ProcessFile(filePath))

                    For Each participant In result.ValidParticipants
                        databaseService.AddParticipant(participant)
                    Next

                    GetParticipants()

                    If result.InvalidLines.Count > 0 Then
                        ErrorService.ShowWarning($"{result.InvalidLines.Count} ligne(s) ont été ignorées en raison d'un format invalide :{Environment.NewLine}{String.Join(Environment.NewLine, result.InvalidLines.Take(5))}...", "Importation partielle")
                    Else
                        ErrorService.ShowInfo("Tous les participants ont été importés avec succès !", "Importation réussie")
                    End If
                End If
            Catch ex As FileNotFoundException
                ErrorService.ShowError($"Erreur : {ex.Message}", "Fichier introuvable")
            Catch ex As IOException
                ErrorService.ShowError($"Erreur : Le fichier est déjà utilisé par une autre application ou est inaccessible.{Environment.NewLine}{ex.Message}", "Fichier inaccessible")
            Catch ex As InvalidDataException
                ErrorService.ShowError($"Erreur : {ex.Message}", "Format de fichier invalide")
            Catch ex As Exception
                ErrorService.ShowError($"Erreur inattendue : {ex.Message}", "Erreur")
            Finally
                Mouse.OverrideCursor = Nothing
            End Try
        End Sub
        ''' <summary>
        ''' Processes the file and parses participants from it.
        ''' </summary>
        ''' <param name="filePath">The path to the file to process.</param>
        ''' <returns>A tuple containing a list of valid participants and invalid lines.</returns>
        Private Shared Function ProcessFile(filePath As String) As (ValidParticipants As List(Of Participant), InvalidLines As List(Of String))
            Dim validParticipants As New List(Of Participant)()
            Dim invalidLines As New List(Of String)()

            Dim lines As String() = File.ReadAllLines(filePath, Text.Encoding.UTF8)
            If lines.Length = 0 Then
                Throw New InvalidDataException("Le fichier est vide.")
            End If

            For Each line In lines
                If Not String.IsNullOrWhiteSpace(line) Then
                    Dim trimmedLine = line.Trim()
                    Dim parts = trimmedLine.Split(" "c, StringSplitOptions.RemoveEmptyEntries)
                    If parts.Length < 2 Then
                        invalidLines.Add(trimmedLine)
                        Continue For
                    End If
                    Dim fullName = String.Join(" ", parts)
                    validParticipants.Add(New Participant() With {.Name = fullName})
                End If
            Next

            Return (validParticipants, invalidLines)
        End Function

    End Class

End Namespace

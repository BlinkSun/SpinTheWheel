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
        Public ReadOnly Property UpdateAllCommand As RelayCommand
        Public ReadOnly Property DeleteCommand As RelayCommand(Of Participant)
        Public ReadOnly Property DeleteAllCommand As RelayCommand
        Public ReadOnly Property ApplyFilterCommand As RelayCommand
        Public ReadOnly Property ImportParticipantsCommand As RelayCommand

        Public Sub New(dbService As DatabaseService)
            ArgumentNullException.ThrowIfNull(dbService)

            databaseService = dbService
            Participants = New ObservableCollection(Of Participant)()

            AddCommand = New RelayCommand(AddressOf AddParticipant)
            UpdateCommand = New RelayCommand(Of Participant)(AddressOf UpdateParticipant, Function() SelectedParticipant IsNot Nothing)
            UpdateAllCommand = New RelayCommand(AddressOf UpdateParticipants)
            DeleteCommand = New RelayCommand(Of Participant)(AddressOf DeleteParticipant, Function() SelectedParticipant IsNot Nothing)
            DeleteAllCommand = New RelayCommand(AddressOf DeleteParticipants)
            ApplyFilterCommand = New RelayCommand(AddressOf ApplyFilter)
            ImportParticipantsCommand = New RelayCommand(AddressOf ImportParticipants)

            'GetParticipants()
        End Sub

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

        Private Sub AddParticipant()
            Try
                Dim name As String = InputBox("Entrez le nom de ce nouveau participant :", "Ajouter Participant")
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
        Private Sub UpdateParticipants()
            Try
                If MessageBox.Show("Tous les participants seront réinitialisés à l'état non pigé. Voulez-vous continuer ?", "Réinitialiser le tirage.", MessageBoxButton.YesNo) = MessageBoxResult.Yes Then
                    SelectedParticipant = Nothing
                    databaseService.UpdateParticipants()
                    GetParticipants()
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Erreur lors de la suppression des participants : {ex.Message}")
            End Try
        End Sub
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
        Private Sub DeleteParticipants()
            Try
                If MessageBox.Show("Êtes-vous sûr de vouloir effacer tous les participants de la liste ?" & vbCrLf & "Cette action est irréversible.", "Suppression de tous les participants", MessageBoxButton.YesNo) = MessageBoxResult.Yes Then
                    SelectedParticipant = Nothing
                    databaseService.DeleteParticipants()
                    GetParticipants()
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Erreur lors de la suppression du participant : {ex.Message}")
            End Try
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
                    .Filter = "Fichiers texte (*.txt)|*.txt|Fichiers csv (*.csv)|*.csv",
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
                    Dim lines As String() = File.ReadAllLines(filePath, Text.Encoding.UTF8)
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

                    GetParticipants()
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

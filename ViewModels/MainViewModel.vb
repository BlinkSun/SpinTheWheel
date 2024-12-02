Imports SpinTheWheel.Models
Imports SpinTheWheel.Services

Namespace ViewModels

    ''' <summary>
    ''' ViewModel for the main window, handling the spinning wheel and participant selection.
    ''' </summary>
    Public Class MainViewModel
        Inherits ViewModelBase

        Private ReadOnly databaseService As DatabaseService

        ''' <summary>
        ''' Command to spin the wheel.
        ''' </summary>
        Public ReadOnly Property SpinWheelCommand As ICommand

        ''' <summary>
        ''' Command to open the participants manager window.
        ''' </summary>
        Public ReadOnly Property OpenParticipantsManagerCommand As ICommand

        ''' <summary>
        ''' Selected participant to display on the UI.
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
        ''' Indicates whether the wheel is spinning.
        ''' </summary>
        Private isSpinningValue As Boolean
        Public Property IsSpinning As Boolean
            Get
                Return isSpinningValue
            End Get
            Set(value As Boolean)
                isSpinningValue = value
                OnPropertyChanged(NameOf(IsSpinning))
            End Set
        End Property

        ''' <summary>
        ''' Rotation angle of the wheel.
        ''' </summary>
        Private spinAngleValue As Double
        Public Property SpinAngle As Double
            Get
                Return spinAngleValue
            End Get
            Set(value As Double)
                spinAngleValue = value
                OnPropertyChanged(NameOf(SpinAngle))
            End Set
        End Property

        ''' <summary>
        ''' Constructor for MainViewModel.
        ''' </summary>
        ''' <param name="dbService">The shared DatabaseService instance.</param>
        Public Sub New(dbService As DatabaseService)
            databaseService = dbService
            SpinWheelCommand = New RelayCommand(AddressOf SpinWheel, Function() Not IsSpinning)
            OpenParticipantsManagerCommand = New RelayCommand(AddressOf OpenParticipantsManager)
            SelectedParticipant = Nothing
        End Sub

        ''' <summary>
        ''' Simulates spinning the wheel and selects a random participant.
        ''' </summary>
        Private Async Sub SpinWheel()
            IsSpinning = True

            ' Simulate spinning animation
            For i As Integer = 1 To 360 Step 10
                SpinAngle = SpinAngle + i
                Await Task.Delay(50)
            Next

            ' Select a random participant from the database
            Dim randomParticipant = databaseService.GetRandomParticipant()
            If randomParticipant IsNot Nothing Then
                SelectedParticipant = randomParticipant
            Else
                MessageBox.Show("No participants available.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning)
            End If

            IsSpinning = False
        End Sub

        ''' <summary>
        ''' Opens the participants manager window in modal.
        ''' </summary>
        Private Sub OpenParticipantsManager()
            Dim participantsWindow As New ParticipantsWindow With {
                .Owner = Application.Current.MainWindow
            }
            participantsWindow.ShowDialog()

            ' Reload participants after the manager window is closed
            SelectedParticipant = Nothing
        End Sub

    End Class

End Namespace
Imports System.Collections.ObjectModel
Imports SpinTheWheel.Models
Imports SpinTheWheel.Services

Namespace ViewModels

    ''' <summary>
    ''' ViewModel for the main window, handling the spinning wheel and participant selection.
    ''' </summary>
    Public Class MainViewModel
        Inherits ViewModelBase

        ''' <summary>
        ''' Instance of the database service for retrieving participants.
        ''' </summary>
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
        ''' The participant currently selected by the wheel.
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
        ''' Indicates whether the wheel is currently spinning.
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
        ''' Current rotation angle of the wheel.
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
        ''' Duration of the wheel spin animation in milliseconds.
        ''' Controlled by a Slider in the UI.
        ''' </summary>
        Private spinDurationValue As Integer = 2000 ' Default value: 2 seconds
        Public Property SpinDuration As Integer
            Get
                Return spinDurationValue
            End Get
            Set(value As Integer)
                spinDurationValue = value
                OnPropertyChanged(NameOf(SpinDuration))
            End Set
        End Property

        ''' <summary>
        ''' Number of segments in the wheel.
        ''' </summary>
        Private numberOfSegmentsValue As Integer = 6 ' Default: 6 segments
        Public Property NumberOfSegments As Integer
            Get
                Return numberOfSegmentsValue
            End Get
            Set(value As Integer)
                numberOfSegmentsValue = value
                GenerateSegments()
                OnPropertyChanged(NameOf(NumberOfSegments))
            End Set
        End Property

        ''' <summary>
        ''' Collection of segment angles for the wheel.
        ''' </summary>
        Public Property Segments As ObservableCollection(Of Double)

        ''' <summary>
        ''' Generates the segments based on the current number of segments.
        ''' </summary>
        Private Sub GenerateSegments()
            Segments.Clear()
            Dim anglePerSegment As Double = 360.0 / NumberOfSegments
            For i As Integer = 0 To NumberOfSegments - 1
                Segments.Add(i * anglePerSegment)
            Next
        End Sub

        ''' <summary>
        ''' Constructor for MainViewModel.
        ''' </summary>
        ''' <param name="dbService">The shared DatabaseService instance.</param>
        Public Sub New(dbService As DatabaseService)
            ArgumentNullException.ThrowIfNull(dbService)

            databaseService = dbService
            SpinWheelCommand = New RelayCommand(AddressOf SpinWheel, Function() Not IsSpinning)
            OpenParticipantsManagerCommand = New RelayCommand(AddressOf OpenParticipantsManager)
            Segments = New ObservableCollection(Of Double)()
            GenerateSegments()
        End Sub

        ''' <summary>
        ''' Spins the wheel with an animation and selects a random participant.
        ''' </summary>
        Private Async Sub SpinWheel()
            Try
                IsSpinning = True
                Dim random As New Random()
                Dim totalRotation As Integer = random.Next(1440, 2160) ' 4 to 6 full rotations
                Dim currentRotation As Integer = 0
                Dim stepDuration As Integer = SpinDuration \ totalRotation ' Adjust speed based on SpinDuration

                While currentRotation < totalRotation
                    currentRotation += 10
                    SpinAngle = currentRotation Mod 360
                    Await Task.Delay(stepDuration)
                End While

                ' Select a random participant after the animation
                Dim randomParticipant = databaseService.GetRandomParticipant()
                If randomParticipant IsNot Nothing Then
                    SelectedParticipant = randomParticipant
                Else
                    ErrorService.ShowWarning("No participants are available to select.")
                End If
            Catch ex As Exception
                ErrorService.ShowError($"An error occurred while spinning the wheel: {ex.Message}")
            Finally
                IsSpinning = False
            End Try
        End Sub

        ''' <summary>
        ''' Opens the participants manager window in modal and reloads participants afterward.
        ''' </summary>
        Private Sub OpenParticipantsManager()
            Try
                Dim participantsWindow As New ParticipantsWindow With {
                    .Owner = Application.Current.MainWindow
                }
                participantsWindow.ShowDialog()

                ' Clear the current selection after the manager window is closed
                SelectedParticipant = Nothing
            Catch ex As Exception
                ErrorService.ShowError($"An error occurred while opening the participants manager: {ex.Message}")
            End Try
        End Sub

    End Class

End Namespace

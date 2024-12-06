Imports System.Collections.ObjectModel
Imports SpinTheWheel.Models
Imports SpinTheWheel.Services
Imports SpinTheWheel.Views

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
        ''' Collection of segment angles for the wheel.
        ''' </summary>
        Public ReadOnly Property Segments As ObservableCollection(Of UIElement)
        ''' <summary>
        ''' Command to spin the wheel.
        ''' </summary>
        Public ReadOnly Property SpinWheelCommand As RelayCommand
        ''' <summary>
        ''' Command to open the participants manager window.
        ''' </summary>
        Public ReadOnly Property OpenParticipantsManagerCommand As RelayCommand

        ''' <summary>
        ''' The participant currently selected by the wheel.
        ''' </summary>
        Public Property SelectedParticipant As Participant
            Get
                Return selectedParticipantValue
            End Get
            Set(value As Participant)
                selectedParticipantValue = value
                OnPropertyChanged(NameOf(SelectedParticipant))
            End Set
        End Property
        Private selectedParticipantValue As Participant

        ''' <summary>
        ''' Indicates whether the wheel is currently spinning.
        ''' </summary>
        Public Property IsSpinning As Boolean
            Get
                Return isSpinningValue
            End Get
            Set(value As Boolean)
                isSpinningValue = value
                OnPropertyChanged(NameOf(IsSpinning))
            End Set
        End Property
        Private isSpinningValue As Boolean

        ''' <summary>
        ''' Current rotation angle of the wheel.
        ''' </summary>
        Public Property SpinAngle As Double
            Get
                Return spinAngleValue
            End Get
            Set(value As Double)
                spinAngleValue = value
                OnPropertyChanged(NameOf(SpinAngle))
            End Set
        End Property
        Private spinAngleValue As Double

        ''' <summary>
        ''' Duration of the wheel spin animation in milliseconds.
        ''' Controlled by a Slider in the UI.
        ''' </summary>
        Public Property SpinDuration As Integer
            Get
                Return spinDurationValue
            End Get
            Set(value As Integer)
                spinDurationValue = value
                OnPropertyChanged(NameOf(SpinDuration))
            End Set
        End Property
        Private spinDurationValue As Integer = 2000 ' Default value: 2 seconds

        ''' <summary>
        ''' Number of segments in the wheel.
        ''' </summary>
        Public Property SegmentCount As Integer
            Get
                Return segmentCountValue
            End Get
            Set(value As Integer)
                segmentCountValue = value
                GenerateSegments()
                OnPropertyChanged(NameOf(SegmentCount))
            End Set
        End Property
        Private segmentCountValue As Integer = 16 ' Default: 16 segments

        Public Property WinnerName As String
            Get
                Return winnerNameValue
            End Get
            Set(value As String)
                winnerNameValue = value
                OnPropertyChanged(NameOf(WinnerName))
            End Set
        End Property
        Private winnerNameValue As String

        Public Property IsWinnerVisible As Boolean
            Get
                Return isWinnerVisibleValue
            End Get
            Set(value As Boolean)
                isWinnerVisibleValue = value
                OnPropertyChanged(NameOf(IsWinnerVisible))
            End Set
        End Property
        Private isWinnerVisibleValue As Boolean

        ''' <summary>
        ''' Radius of the wheel.
        ''' </summary>
        Private Const Radius As Double = 300.0

        ''' <summary>
        ''' Constructor for MainViewModel.
        ''' </summary>
        ''' <param name="dbService">The shared DatabaseService instance.</param>
        Public Sub New(dbService As DatabaseService)
            ArgumentNullException.ThrowIfNull(dbService)

            databaseService = dbService
            Segments = New ObservableCollection(Of UIElement)()
            SpinWheelCommand = New RelayCommand(AddressOf SpinWheel, Function() Not IsSpinning)
            OpenParticipantsManagerCommand = New RelayCommand(AddressOf OpenParticipantsManager)

            GenerateSegments()
        End Sub

        ''' <summary>
        ''' Generates the segments based on the current number of segments.
        ''' </summary>
        Private Sub GenerateSegments()
            Segments.Clear()
            Dim centerX As Double = Radius
            Dim centerY As Double = Radius
            Dim angleStep As Double = 360.0 / SegmentCount

            For i As Integer = 0 To SegmentCount - 1
                Dim startAngle As Double = i * angleStep
                Dim endAngle As Double = startAngle + angleStep
                Dim midAngle As Double = startAngle + (angleStep / 2)

                Dim startX As Double = centerX + (Radius * Math.Cos(startAngle * Math.PI / 180.0))
                Dim startY As Double = centerY - (Radius * Math.Sin(startAngle * Math.PI / 180.0))
                Dim endX As Double = centerX + (Radius * Math.Cos(endAngle * Math.PI / 180.0))
                Dim endY As Double = centerY - (Radius * Math.Sin(endAngle * Math.PI / 180.0))
                Dim midX As Double = centerX + (Radius / 2 * Math.Cos(midAngle * Math.PI / 180.0))
                Dim midY As Double = centerY - (Radius / 2 * Math.Sin(midAngle * Math.PI / 180.0))

                Dim path As New Path() With {
                    .Fill = If(i Mod 2 = 0, New SolidColorBrush(ColorConverter.ConvertFromString("#00874e")), Brushes.White),
                    .Stroke = New SolidColorBrush(ColorConverter.ConvertFromString("#03673e")),
                    .StrokeThickness = 3
                }
                Dim figure As New PathFigure() With {
                    .StartPoint = New Point(centerX, centerY)
                }
                figure.Segments.Add(New LineSegment(New Point(startX, startY), True))
                figure.Segments.Add(New ArcSegment(New Point(endX, endY), New Size(Radius, Radius), 0, False, SweepDirection.Counterclockwise, True))

                Dim geometry As New PathGeometry()
                geometry.Figures.Add(figure)
                path.Data = geometry

                Segments.Add(path)
            Next
        End Sub

        ''' <summary>
        ''' Spins the wheel with an animation and selects a random participant.
        ''' </summary>
        Private Async Sub SpinWheel()
            Try
                IsSpinning = True
                Dim random As New Random()

                ' Configuration de la rotation
                Dim totalRotation As Double = random.Next(1440, 2160) ' 4 à 6 rotations complètes
                Dim currentRotation As Double = 0.0
                Dim velocity As Double = 20 ' Vitesse initiale
                Dim deceleration As Double = 0.98 ' Facteur de désaccélération
                Dim minVelocity As Double = 0.75 ' Vitesse minimale avant d'arrêter

                ' Phase 1 : Rotation constante
                Dim spinStartTime As DateTime = DateTime.Now
                While (DateTime.Now - spinStartTime).TotalMilliseconds < SpinDuration
                    currentRotation += velocity
                    SpinAngle = currentRotation Mod 360
                    Await Task.Delay(10)
                End While

                ' Phase 2 : Désaccélération progressive
                currentRotation = 0.0
                While currentRotation < totalRotation AndAlso velocity > minVelocity
                    currentRotation += velocity
                    SpinAngle = currentRotation Mod 360
                    velocity *= deceleration
                    Dim stepDuration As Integer = Math.Max(10, CInt(100 / velocity))
                    Await Task.Delay(stepDuration)
                End While

                ' Sélectionner un participant aléatoire après l'animation
                Dim randomParticipant = databaseService.GetRandomParticipant()
                If randomParticipant IsNot Nothing Then
                    WinnerName = randomParticipant.Name ' Met à jour le nom du gagnant
                    IsWinnerVisible = True ' Affiche le gagnant
                    Await Task.Delay(10000) ' Attend 10 secondes avant de masquer
                    IsWinnerVisible = False ' Cache le gagnant après l'animation
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
                    .Owner = Application.Current.MainWindow,
                    .WindowStartupLocation = WindowStartupLocation.CenterScreen
                }
                participantsWindow.ShowDialog()

                SelectedParticipant = Nothing
            Catch ex As Exception
                ErrorService.ShowError($"An error occurred while opening the participants manager: {ex.Message}")
            End Try
        End Sub

    End Class

End Namespace

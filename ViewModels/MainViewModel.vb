Imports System.Collections.ObjectModel
Imports System.Windows.Media.Animation
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
        ''' Command to reset the wheel.
        ''' </summary>
        Public ReadOnly Property NextSpinCommand As RelayCommand
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

        ''' <summary>
        ''' Gets or Sets ame of the winner.
        ''' </summary>
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

        ''' <summary>
        ''' Gets or Sets if is winner visible.
        ''' </summary>
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
        ''' Collection of confetti.
        ''' </summary>
        Public Property Confettis As ObservableCollection(Of UIElement)
            Get
                Return confettisValue
            End Get
            Set(value As ObservableCollection(Of UIElement))
                confettisValue = value
                OnPropertyChanged(NameOf(Confettis))
            End Set
        End Property
        Private confettisValue As ObservableCollection(Of UIElement)

        ''' <summary>
        ''' Radius of the wheel.
        ''' </summary>
        Public ReadOnly Property Radius As Double = 300.0
        ''' <summary>
        ''' Diameter of the wheel.
        ''' </summary>
        Public ReadOnly Property Diameter As Double = Radius * 2

        ''' <summary>
        ''' Constructor for MainViewModel.
        ''' </summary>
        ''' <param name="dbService">The shared DatabaseService instance.</param>
        Public Sub New(dbService As DatabaseService)
            ArgumentNullException.ThrowIfNull(dbService)

            databaseService = dbService
            Segments = New ObservableCollection(Of UIElement)()
            Confettis = New ObservableCollection(Of UIElement)()
            SpinWheelCommand = New RelayCommand(AddressOf SpinWheel, Function() Not IsSpinning)
            NextSpinCommand = New RelayCommand(AddressOf NextSpin)
            OpenParticipantsManagerCommand = New RelayCommand(AddressOf OpenParticipantsManager)

            GenerateSegments()
        End Sub

        ''' <summary>
        ''' Generates the segments for the wheel based on the current segment count.
        ''' Each segment is defined by its start and end angles, and alternates colors for visual distinction.
        ''' </summary>
        Private Sub GenerateSegments()
            Segments.Clear()
            Dim centerX As Double = Radius
            Dim centerY As Double = Radius
            Dim angleStep As Double = 360.0 / SegmentCount

            ' Loop through each segment and create its geometry
            For i As Integer = 0 To SegmentCount - 1
                Dim startAngle As Double = i * angleStep
                Dim endAngle As Double = startAngle + angleStep
                Dim midAngle As Double = startAngle + (angleStep / 2)

                ' Calculate points for segment start, end, and mid
                Dim startX As Double = centerX + (Radius * Math.Cos(startAngle * Math.PI / 180.0))
                Dim startY As Double = centerY - (Radius * Math.Sin(startAngle * Math.PI / 180.0))
                Dim endX As Double = centerX + (Radius * Math.Cos(endAngle * Math.PI / 180.0))
                Dim endY As Double = centerY - (Radius * Math.Sin(endAngle * Math.PI / 180.0))
                Dim midX As Double = centerX + (Radius / 2 * Math.Cos(midAngle * Math.PI / 180.0))
                Dim midY As Double = centerY - (Radius / 2 * Math.Sin(midAngle * Math.PI / 180.0))

                ' Create the segment path with alternating colors
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
        ''' Creates an animated explosion of confetti centered at a random point within the radius.
        ''' Each confetti piece is represented as an ellipse with random size, color, and movement.
        ''' </summary>
        ''' <param name="confettiCount">The number of confetti pieces to generate.</param>
        Private Sub CreateConfettiExplosion(confettiCount As Integer)
            Dim random As New Random()

            ' Generate a random center point for the explosion within the defined radius
            Dim centerX = random.Next(0, Diameter)
            Dim centerY = random.Next(0, Diameter)

            ' Generate the specified number of confetti pieces
            For i As Integer = 1 To confettiCount
                Dim storyboard As New Storyboard()

                ' Create a confetti ellipse with random dimensions and a random color
                Dim ellipse As New Ellipse With {
                    .Width = random.Next(5, 12), ' Random width between 5 and 12
                    .Height = random.Next(5, 12), ' Random height between 5 and 12
                    .Fill = New SolidColorBrush(Color.FromRgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256))),
                    .Opacity = 1.0 ' Fully visible initially
                }

                ' Configure horizontal movement animation (X-axis)
                Dim xAnimation As New DoubleAnimation() With {
                    .From = centerX,
                    .To = centerX + random.Next(-Radius, Radius), ' Random movement within the radius
                    .Duration = TimeSpan.FromSeconds(2) ' Animation duration
                }
                Storyboard.SetTarget(xAnimation, ellipse)
                Storyboard.SetTargetProperty(xAnimation, New PropertyPath("(Canvas.Left)", Array.Empty(Of Object)()))
                storyboard.Children.Add(xAnimation)

                ' Configure vertical movement animation (Y-axis)
                Dim yAnimation As New DoubleAnimation() With {
                    .From = centerY,
                    .To = centerY + random.Next(-Radius, Radius), ' Random movement within the radius
                    .Duration = TimeSpan.FromSeconds(2)
                }
                Storyboard.SetTarget(yAnimation, ellipse)
                Storyboard.SetTargetProperty(yAnimation, New PropertyPath("(Canvas.Top)", Array.Empty(Of Object)()))
                storyboard.Children.Add(yAnimation)

                ' Configure fade-out animation (opacity)
                Dim opacityAnimation As New DoubleAnimation() With {
                    .From = 1.0, ' Start fully visible
                    .To = 0.0, ' Fade to invisible
                    .Duration = TimeSpan.FromSeconds(2.2) ' Slightly longer than movement duration
                }
                Storyboard.SetTarget(opacityAnimation, ellipse)
                Storyboard.SetTargetProperty(opacityAnimation, New PropertyPath("Opacity", Array.Empty(Of Object)()))
                storyboard.Children.Add(opacityAnimation)

                ' Add the ellipse to the confetti canvas and start its animation
                Confettis.Add(ellipse)
                storyboard.Begin()
            Next
        End Sub

        ''' <summary>
        ''' Animates the spinning of the wheel and selects a random participant upon completion.
        ''' Includes a confetti explosion to celebrate the winner.
        ''' </summary>
        Private Async Sub SpinWheel()
            Try
                IsSpinning = True
                Dim random As New Random()

                ' Configure spin animation parameters
                Dim totalRotation As Double = random.Next(1440, 2160) ' 4 to 6 full rotations
                Dim currentRotation As Double = 0.0
                Dim velocity As Double = 20 ' Initial spinning speed
                Dim deceleration As Double = 0.98 ' Rate of slowing down
                Dim minVelocity As Double = 0.75 ' Minimum speed before stopping

                Dim confettiExplosion As Integer = 5 ' Number of confetti explosions
                Dim confettiByExplosion As Integer = 30 ' Number of confetti pieces per explosion

                ' Phase 1: Constant speed rotation
                Dim spinStartTime As DateTime = DateTime.Now
                While (DateTime.Now - spinStartTime).TotalMilliseconds < SpinDuration
                    currentRotation += velocity
                    SpinAngle = currentRotation Mod 360 ' Keep angle within 0–360
                    Await Task.Delay(10) ' Smooth animation frame rate
                End While

                ' Phase 2: Gradual deceleration
                currentRotation = 0.0
                While currentRotation < totalRotation AndAlso velocity > minVelocity
                    currentRotation += velocity
                    SpinAngle = currentRotation Mod 360
                    velocity *= deceleration ' Reduce speed progressively
                    Dim stepDuration As Integer = Math.Max(10, CInt(100 / velocity)) ' Adjust delay as speed decreases
                    Await Task.Delay(stepDuration)
                End While

                ' Select a random winner from the participants
                Dim winner As Participant = GetRandomWinner()

                If winner IsNot Nothing Then
                    WinnerName = winner.Name
                    IsWinnerVisible = True

                    ' Celebrate the winner with confetti explosions
                    While IsWinnerVisible
                        For j As Integer = 1 To confettiExplosion
                            CreateConfettiExplosion(confettiByExplosion)
                        Next
                        Await Task.Delay(2000) ' Wait before clearing and restarting confetti
                        Confettis.Clear()
                    End While
                Else
                    ErrorService.ShowWarning("Aucun participant n'est disponible pour le tirage.")
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Une erreur s'est produite lors du lancement de la roue : {ex.Message}")
            Finally
                IsSpinning = False
            End Try
        End Sub

        ''' <summary>
        ''' Resets the visibility of the winner section, preparing for the next spin.
        ''' </summary>
        Private Sub NextSpin()
            IsWinnerVisible = False
        End Sub

        ''' <summary>
        ''' Retrieves a random participant from the database and updates their status as "selected."
        ''' </summary>
        ''' <returns>The selected participant or <c>Nothing</c> if no participants are available.</returns>
        Private Function GetRandomWinner() As Participant
            Dim participant As Participant = Nothing
            Try
                participant = databaseService.GetRandomParticipant()
                If participant IsNot Nothing Then
                    participant.Done = True
                    Dim order As Integer = databaseService.GetNextOrderValue()
                    participant.Order = order
                    databaseService.UpdateParticipant(participant)
                End If
            Catch ex As Exception
                ErrorService.ShowError($"Une erreur s'est produite lors de la mise à jour du gagnant : {ex.Message}")
            End Try
            Return participant
        End Function

        ''' <summary>
        ''' Opens the participants manager window in modal and reloads participants afterward.
        ''' </summary>
        Private Sub OpenParticipantsManager()
            Try
                Dim participantsWindow As New ParticipantsWindow With {
                    .Owner = Application.Current.MainWindow,
                    .WindowStartupLocation = WindowStartupLocation.CenterOwner
                }
                participantsWindow.ShowDialog()

                SelectedParticipant = Nothing
            Catch ex As Exception
                ErrorService.ShowError($"Une erreur s'est produite lors de l'ouverture du gestionnaire de participants : {ex.Message}")
            End Try
        End Sub

    End Class

End Namespace

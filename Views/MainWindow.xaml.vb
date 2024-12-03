Imports System.Windows.Media.Animation

Namespace Views

    Partial Public Class MainWindow
        Inherits Window

        Private Sub OnLaunchConfettiClick(sender As Object, e As RoutedEventArgs)
            GenerateConfettiExplosion(400, 300) ' Centre de l'explosion
        End Sub

        ''' <summary>
        ''' Génère une explosion de confettis.
        ''' </summary>
        ''' <param name="centerX">Position X de l'explosion.</param>
        ''' <param name="centerY">Position Y de l'explosion.</param>
        Private Sub GenerateConfettiExplosion(centerX As Double, centerY As Double)
            Dim random As New Random()
            Dim confettiCount As Integer = 50 ' Nombre de confettis

            For i As Integer = 1 To confettiCount
                ' Créer un élément visuel pour le confetti
                Dim confetti As New Ellipse With {
                    .Width = random.Next(5, 15),
                    .Height = random.Next(5, 15),
                    .Fill = New SolidColorBrush(Color.FromRgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)))
                }

                ' Ajouter le confetti au Canvas
                ConfettiCanvas.Children.Add(confetti)

                ' Positionner le confetti au centre de l'explosion
                Canvas.SetLeft(confetti, centerX)
                Canvas.SetTop(confetti, centerY)

                ' Générer des destinations aléatoires
                Dim targetX As Double = centerX + random.Next(-300, 300)
                Dim targetY As Double = centerY + random.Next(-300, 300)

                ' Créer les animations
                Dim xAnimation As New DoubleAnimation(Canvas.GetLeft(confetti), targetX, New Duration(TimeSpan.FromSeconds(1))) With {
                    .EasingFunction = New CircleEase With {.EasingMode = EasingMode.EaseOut}
                }
                Dim yAnimation As New DoubleAnimation(Canvas.GetTop(confetti), targetY, New Duration(TimeSpan.FromSeconds(1))) With {
                    .EasingFunction = New CircleEase With {.EasingMode = EasingMode.EaseOut}
                }
                Dim fadeAnimation As New DoubleAnimation(1.0, 0.0, New Duration(TimeSpan.FromSeconds(1.5)))

                ' Appliquer les animations
                confetti.BeginAnimation(Canvas.LeftProperty, xAnimation)
                confetti.BeginAnimation(Canvas.TopProperty, yAnimation)
                confetti.BeginAnimation(UIElement.OpacityProperty, fadeAnimation)

                ' Supprimer le confetti après l'animation
                AddHandler fadeAnimation.Completed, Sub(s, args) ConfettiCanvas.Children.Remove(confetti)
            Next
        End Sub

    End Class

End Namespace

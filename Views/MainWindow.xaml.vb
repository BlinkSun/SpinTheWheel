Imports System.Windows.Media.Animation

Namespace Views

    Partial Public Class MainWindow
        Inherits Window

        Private Sub OnLaunchConfettiClick(sender As Object, e As RoutedEventArgs)
            Dim button As Button = DirectCast(sender, Button)
            Dim j = Convert.ToInt32(button.Tag)
            Dim random As New Random()
            For i As Integer = 0 To 10
                Select Case j
                    Case 0
                        GenerateConfettiExplosionImproved1(random.Next(0, mainCanvas.ActualWidth), random.Next(0, mainCanvas.ActualHeight))
                    Case 1
                        GenerateConfettiExplosionImproved2(random.Next(0, mainCanvas.ActualWidth), random.Next(0, mainCanvas.ActualHeight))
                    Case 2
                        GenerateConfettiExplosionImproved3(random.Next(0, mainCanvas.ActualWidth), random.Next(0, mainCanvas.ActualHeight))
                    Case 3
                        GenerateConfettiExplosionImproved4(random.Next(0, mainCanvas.ActualWidth), random.Next(0, mainCanvas.ActualHeight))
                    Case 4
                        GenerateConfettiExplosionImproved5(random.Next(0, mainCanvas.ActualWidth), random.Next(0, mainCanvas.ActualHeight))
                    Case 5
                        GenerateConfettiExplosionImproved6(random.Next(0, mainCanvas.ActualWidth), random.Next(0, mainCanvas.ActualHeight))
                    Case 6
                        GenerateConfettiExplosionImproved7(random.Next(0, mainCanvas.ActualWidth), random.Next(0, mainCanvas.ActualHeight))
                End Select
            Next
        End Sub

        Private Sub GenerateConfettiExplosionImproved1(centerX As Double, centerY As Double)
            Dim random As New Random()
            Dim confettiCount As Integer = 75 ' Augmenter le nombre de confettis pour un effet plus spectaculaire

            For i As Integer = 1 To confettiCount
                ' Créer un élément visuel pour le confetti
                Dim confetti As New Ellipse With {
                    .Width = random.Next(8, 20),
                    .Height = random.Next(8, 20),
                    .Fill = New SolidColorBrush(Color.FromRgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)))
                }

                ' Ajouter le confetti au Canvas
                mainCanvas.Children.Add(confetti)

                ' Positionner le confetti au centre de l'explosion
                Canvas.SetLeft(confetti, centerX)
                Canvas.SetTop(confetti, centerY)

                ' Générer des destinations aléatoires avec un effet de rotation
                Dim targetX As Double = centerX + random.Next(-400, 400)
                Dim targetY As Double = centerY + random.Next(-400, 400)

                ' Créer les animations
                Dim xAnimation As New DoubleAnimation(Canvas.GetLeft(confetti), targetX, New Duration(TimeSpan.FromSeconds(1.2))) With {
                    .EasingFunction = New QuinticEase With {.EasingMode = EasingMode.EaseOut}
                }
                Dim yAnimation As New DoubleAnimation(Canvas.GetTop(confetti), targetY, New Duration(TimeSpan.FromSeconds(1.2))) With {
                    .EasingFunction = New QuinticEase With {.EasingMode = EasingMode.EaseOut}
                }
                Dim fadeAnimation As New DoubleAnimation(1.0, 0.0, New Duration(TimeSpan.FromSeconds(1.8)))

                ' Appliquer les animations
                confetti.BeginAnimation(Canvas.LeftProperty, xAnimation)
                confetti.BeginAnimation(Canvas.TopProperty, yAnimation)
                confetti.BeginAnimation(UIElement.OpacityProperty, fadeAnimation)

                ' Supprimer le confetti après l'animation
                AddHandler fadeAnimation.Completed, Sub(s, args) mainCanvas.Children.Remove(confetti)
            Next
        End Sub
        Private Sub GenerateConfettiExplosionImproved2(centerX As Double, centerY As Double)
            Dim random As New Random()
            Dim confettiCount As Integer = 60 ' Confettis avec variations de taille et de forme

            For i As Integer = 1 To confettiCount
                ' Choisir aléatoirement entre Rectangle et Ellipse
                Dim confetti As Shape
                If random.Next(0, 2) = 0 Then
                    confetti = New Rectangle With {
                        .Width = random.Next(6, 15),
                        .Height = random.Next(6, 15)
                    }
                Else
                    confetti = New Ellipse With {
                        .Width = random.Next(6, 15),
                        .Height = random.Next(6, 15)
                    }
                End If
                confetti.Fill = New SolidColorBrush(Color.FromRgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)))

                ' Ajouter le confetti au Canvas
                mainCanvas.Children.Add(confetti)

                ' Positionner le confetti au centre de l'explosion
                Canvas.SetLeft(confetti, centerX)
                Canvas.SetTop(confetti, centerY)

                ' Générer des destinations aléatoires avec des rotations
                Dim targetX As Double = centerX + random.Next(-350, 350)
                Dim targetY As Double = centerY + random.Next(-350, 350)

                ' Créer les animations
                Dim xAnimation As New DoubleAnimation(Canvas.GetLeft(confetti), targetX, New Duration(TimeSpan.FromSeconds(1.3))) With {
                    .EasingFunction = New BackEase With {.EasingMode = EasingMode.EaseOut}
                }
                Dim yAnimation As New DoubleAnimation(Canvas.GetTop(confetti), targetY, New Duration(TimeSpan.FromSeconds(1.3))) With {
                    .EasingFunction = New BackEase With {.EasingMode = EasingMode.EaseOut}
                }
                Dim rotateTransform As New RotateTransform()
                confetti.RenderTransform = rotateTransform
                Dim rotateAnimation As New DoubleAnimation(0, random.Next(0, 360), New Duration(TimeSpan.FromSeconds(1.5)))
                rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation)
                Dim fadeAnimation As New DoubleAnimation(1.0, 0.0, New Duration(TimeSpan.FromSeconds(2.0)))

                ' Appliquer les animations
                confetti.BeginAnimation(Canvas.LeftProperty, xAnimation)
                confetti.BeginAnimation(Canvas.TopProperty, yAnimation)
                confetti.BeginAnimation(UIElement.OpacityProperty, fadeAnimation)

                ' Supprimer le confetti après l'animation
                AddHandler fadeAnimation.Completed, Sub(s, args) mainCanvas.Children.Remove(confetti)
            Next
        End Sub
        Private Sub GenerateConfettiExplosionImproved3(centerX As Double, centerY As Double)
            Dim random As New Random()
            Dim confettiCount As Integer = 80 ' Plus de confettis avec des variations de couleur et de scintillement

            For i As Integer = 1 To confettiCount
                ' Créer un élément visuel pour le confetti
                Dim confetti As New Ellipse With {
                    .Width = random.Next(5, 12),
                    .Height = random.Next(5, 12),
                    .Fill = New SolidColorBrush(Color.FromRgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)))
                }

                ' Ajouter le confetti au Canvas
                mainCanvas.Children.Add(confetti)

                ' Positionner le confetti au centre de l'explosion
                Canvas.SetLeft(confetti, centerX)
                Canvas.SetTop(confetti, centerY)

                ' Générer des destinations aléatoires avec une animation de scintillement
                Dim targetX As Double = centerX + random.Next(-500, 500)
                Dim targetY As Double = centerY + random.Next(-500, 500)

                ' Créer les animations
                Dim xAnimation As New DoubleAnimation(Canvas.GetLeft(confetti), targetX, New Duration(TimeSpan.FromSeconds(1.1))) With {
                    .EasingFunction = New ElasticEase With {.EasingMode = EasingMode.EaseOut, .Oscillations = 2, .Springiness = 3}
                }
                Dim yAnimation As New DoubleAnimation(Canvas.GetTop(confetti), targetY, New Duration(TimeSpan.FromSeconds(1.1))) With {
                    .EasingFunction = New ElasticEase With {.EasingMode = EasingMode.EaseOut, .Oscillations = 2, .Springiness = 3}
                }
                Dim fadeAnimation As New DoubleAnimation(1.0, 0.0, New Duration(TimeSpan.FromSeconds(2.2)))
                Dim flickerAnimation As New DoubleAnimation(1.0, 0.3, New Duration(TimeSpan.FromMilliseconds(300))) With {
                    .AutoReverse = True,
                    .RepeatBehavior = RepeatBehavior.Forever
                }

                ' Appliquer les animations
                confetti.BeginAnimation(Canvas.LeftProperty, xAnimation)
                confetti.BeginAnimation(Canvas.TopProperty, yAnimation)
                confetti.BeginAnimation(UIElement.OpacityProperty, fadeAnimation)
                confetti.BeginAnimation(UIElement.OpacityProperty, flickerAnimation)

                ' Supprimer le confetti après l'animation
                AddHandler fadeAnimation.Completed, Sub(s, args) mainCanvas.Children.Remove(confetti)
            Next
        End Sub
        Private Sub GenerateConfettiExplosionImproved4(centerX As Double, centerY As Double)
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
                mainCanvas.Children.Add(confetti)

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
                AddHandler fadeAnimation.Completed, Sub(s, args) mainCanvas.Children.Remove(confetti)
            Next
        End Sub
        Private Sub GenerateConfettiExplosionImproved5(centerX As Double, centerY As Double)
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
                mainCanvas.Children.Add(confetti)

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
                AddHandler fadeAnimation.Completed, Sub(s, args) mainCanvas.Children.Remove(confetti)
            Next
        End Sub
        Private Sub GenerateConfettiExplosionImproved6(centerX As Double, centerY As Double)
            Dim random As New Random()

            For i As Integer = 0 To 100
                Dim confetti As New Rectangle With {
            .Width = random.Next(5, 15),
            .Height = random.Next(5, 15),
            .Fill = New SolidColorBrush(Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)))
        }

                ' Position initiale
                Canvas.SetLeft(confetti, centerX)
                Canvas.SetTop(confetti, centerY)
                mainCanvas.Children.Add(confetti)

                ' Animation de chute
                Dim fallAnimation As New DoubleAnimation With {
            .From = Canvas.GetTop(confetti),
            .To = mainCanvas.ActualHeight,
            .Duration = TimeSpan.FromSeconds(random.Next(2, 5))
        }
                Dim sb As New Storyboard()
                Storyboard.SetTarget(fallAnimation, confetti)
                Storyboard.SetTargetProperty(fallAnimation, New PropertyPath("(Canvas.Top)"))
                sb.Children.Add(fallAnimation)

                ' Rotation aléatoire
                Dim rotateAnimation As New DoubleAnimation With {
            .From = 0,
            .To = 360,
            .Duration = TimeSpan.FromSeconds(random.Next(1, 3))
        }
                Storyboard.SetTarget(rotateAnimation, confetti)
                Storyboard.SetTargetProperty(rotateAnimation, New PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"))
                sb.Children.Add(rotateAnimation)

                ' Démarrer l'animation
                sb.Begin(mainCanvas)
            Next
        End Sub
        Private Sub GenerateConfettiExplosionImproved7(centerX As Double, centerY As Double)
            Dim random As New Random()
            Dim confettiCount As Integer = 100 ' Nombre de confettis pour une explosion spectaculaire

            For i As Integer = 1 To confettiCount
                ' Créer une forme aléatoire (ellipse ou rectangle) pour le confetti
                Dim confetti As Shape
                If random.Next(0, 2) = 0 Then
                    confetti = New Ellipse With {
                .Width = random.Next(6, 20),
                .Height = random.Next(6, 20)
            }
                Else
                    confetti = New Rectangle With {
                .Width = random.Next(6, 20),
                .Height = random.Next(6, 20)
            }
                End If

                ' Attribuer une couleur vive et aléatoire
                confetti.Fill = New SolidColorBrush(Color.FromRgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)))

                ' Ajouter le confetti au Canvas
                mainCanvas.Children.Add(confetti)

                ' Position initiale au centre de l'explosion
                Canvas.SetLeft(confetti, centerX)
                Canvas.SetTop(confetti, centerY)

                ' Générer des destinations aléatoires
                Dim targetX As Double = centerX + random.Next(-400, 400)
                Dim targetY As Double = centerY + random.Next(-400, 400)

                ' Ajouter une rotation
                Dim rotateTransform As New RotateTransform()
                confetti.RenderTransform = rotateTransform
                confetti.RenderTransformOrigin = New Point(0.5, 0.5)

                ' Animation de translation
                Dim xAnimation As New DoubleAnimation(Canvas.GetLeft(confetti), targetX, New Duration(TimeSpan.FromSeconds(1.5))) With {
            .EasingFunction = New ElasticEase With {.EasingMode = EasingMode.EaseOut, .Oscillations = 2, .Springiness = 2}
        }
                Dim yAnimation As New DoubleAnimation(Canvas.GetTop(confetti), targetY, New Duration(TimeSpan.FromSeconds(1.5))) With {
            .EasingFunction = New ElasticEase With {.EasingMode = EasingMode.EaseOut, .Oscillations = 2, .Springiness = 2}
        }

                ' Animation de rotation
                Dim rotateAnimation As New DoubleAnimation(0, random.Next(180, 720), New Duration(TimeSpan.FromSeconds(1.5)))

                ' Animation de scintillement
                Dim flickerAnimation As New DoubleAnimation(1.0, random.NextDouble() * 0.5, New Duration(TimeSpan.FromMilliseconds(300))) With {
            .AutoReverse = True,
            .RepeatBehavior = RepeatBehavior.Forever
        }

                ' Animation de disparition
                Dim fadeAnimation As New DoubleAnimation(1.0, 0.0, New Duration(TimeSpan.FromSeconds(2.0)))

                ' Appliquer les animations
                confetti.BeginAnimation(Canvas.LeftProperty, xAnimation)
                confetti.BeginAnimation(Canvas.TopProperty, yAnimation)
                confetti.BeginAnimation(UIElement.OpacityProperty, flickerAnimation)
                confetti.BeginAnimation(UIElement.OpacityProperty, fadeAnimation)
                rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation)

                ' Supprimer le confetti après la disparition
                AddHandler fadeAnimation.Completed, Sub(s, args) mainCanvas.Children.Remove(confetti)
            Next
        End Sub

        Private IsFullscreen As Boolean = False
        Private WindowStateCache As WindowState = WindowState.Normal
        Private Sub Window_KeyDown(sender As Object, e As KeyEventArgs)
            If e.Key = Key.F11 Then
                ToggleFullscreen()
            End If
        End Sub
        Private Sub ToggleFullscreen()
            If IsFullscreen Then
                WindowStyle = WindowStyle.SingleBorderWindow
                ResizeMode = ResizeMode.CanResize
                WindowState = WindowStateCache
                Topmost = False
            Else
                WindowStateCache = WindowState
                WindowStyle = WindowStyle.None
                ResizeMode = ResizeMode.NoResize
                WindowState = WindowState.Maximized
                Topmost = True
            End If

            IsFullscreen = Not IsFullscreen
        End Sub

    End Class

End Namespace

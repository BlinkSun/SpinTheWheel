Namespace Views

    Partial Public Class ColorPickerWindow
        Inherits Window

        Private isUpdating As Boolean = False
        Private selectedColorValue As Color

        Public Property SelectedColor As Color
            Get
                Return selectedColorValue
            End Get
            Set(value As Color)
                selectedColorValue = value
                UpdateUI()
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
            InitializeGradient()
            UpdateUI()
        End Sub

        Private Sub InitializeGradient()
            Dim gradientBrush As New LinearGradientBrush With {
                .StartPoint = New Point(0, 0),
                .EndPoint = New Point(1, 0)
            }

            ' Ajoute des couleurs clés pour représenter le spectre complet des teintes
            gradientBrush.GradientStops.Add(New GradientStop(Colors.Red, 0.0))
            gradientBrush.GradientStops.Add(New GradientStop(Colors.Yellow, 0.17))
            gradientBrush.GradientStops.Add(New GradientStop(Colors.Green, 0.33))
            gradientBrush.GradientStops.Add(New GradientStop(Colors.Cyan, 0.5))
            gradientBrush.GradientStops.Add(New GradientStop(Colors.Blue, 0.67))
            gradientBrush.GradientStops.Add(New GradientStop(Colors.Magenta, 0.83))
            gradientBrush.GradientStops.Add(New GradientStop(Colors.Red, 1.0))

            ' Superpose un gradient vertical pour la luminosité
            Dim overlayBrush As New LinearGradientBrush With {
                .StartPoint = New Point(0, 0),
                .EndPoint = New Point(0, 1)
            }
            overlayBrush.GradientStops.Add(New GradientStop(Colors.Transparent, 0.0)) ' Haut = Lumineux
            overlayBrush.GradientStops.Add(New GradientStop(Colors.Black, 1.0))       ' Bas = Sombre

            ' Combine les deux gradients via un VisualBrush
            Dim visualBrush As New DrawingBrush()
            Dim drawingGroup As New DrawingGroup()
            drawingGroup.Children.Add(New GeometryDrawing(gradientBrush, Nothing, New RectangleGeometry(New Rect(0, 0, 1, 1))))
            drawingGroup.Children.Add(New GeometryDrawing(overlayBrush, Nothing, New RectangleGeometry(New Rect(0, 0, 1, 1))))
            visualBrush.Drawing = drawingGroup

            GradientRectangle.Fill = visualBrush
        End Sub

        Private Sub UpdateUI()
            If isUpdating Then Return
            isUpdating = True

            SliderR.Value = selectedColorValue.R
            SliderG.Value = selectedColorValue.G
            SliderB.Value = selectedColorValue.B
            HexCodeTextBox.Text = $"#{selectedColorValue.R:X2}{selectedColorValue.G:X2}{selectedColorValue.B:X2}"

            Dim selectedBrush As New SolidColorBrush(selectedColorValue)
            SelectedColorRectangle.Fill = selectedBrush

            isUpdating = False
        End Sub

        Private Sub Slider_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))
            If isUpdating Then Return
            SelectedColor = Color.FromRgb(SliderR.Value, SliderG.Value, SliderB.Value)
        End Sub
        Private Sub PredefinedColor_Click(sender As Object, e As MouseButtonEventArgs)
            Dim border = TryCast(sender, Border)
            If border IsNot Nothing AndAlso border.Tag IsNot Nothing Then
                SelectedColor = CType(ColorConverter.ConvertFromString(border.Tag.ToString()), Color)
            End If
        End Sub
        Private Sub GradientRectangle_Click(sender As Object, e As MouseButtonEventArgs)
            ' Vérifie si le clic a lieu sur le Rectangle
            Dim rectangle As Rectangle = TryCast(sender, Rectangle)
            If rectangle Is Nothing Then
                Return
            End If

            ' Obtient la position du clic par rapport au Rectangle
            Dim clickPosition As Point = e.GetPosition(rectangle)

            ' Obtient la taille du Rectangle
            Dim rectWidth As Double = rectangle.ActualWidth
            Dim rectHeight As Double = rectangle.ActualHeight

            ' Normalise les coordonnées du clic (0.0 à 1.0)
            Dim normalizedX As Double = clickPosition.X / rectWidth
            Dim normalizedY As Double = clickPosition.Y / rectHeight

            ' Génère une couleur basée sur les coordonnées du clic
            Dim hue As Double = normalizedX * 360.0 ' Teinte en degrés (0-360)
            Dim brightness As Double = 1.0 - normalizedY ' Inverse pour que le haut soit clair

            ' Convertit la teinte en une couleur RGB
            Dim resultColor As Color = ColorFromHsb(hue, 1.0, brightness)

            ' Met à jour la couleur sélectionnée
            SelectedColor = resultColor
        End Sub

        Private Sub OKButton_Click(sender As Object, e As RoutedEventArgs)
            DialogResult = True
            Close()
        End Sub
        Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs)
            DialogResult = False
            Close()
        End Sub
        Private Shared Function ColorFromHsb(hue As Double, saturation As Double, brightness As Double) As Color
            Dim chroma As Double = brightness * saturation
            Dim x As Double = chroma * (1 - Math.Abs((hue / 60.0) Mod 2 - 1))
            Dim m As Double = brightness - chroma

            Dim r As Double = 0, g As Double = 0, b As Double = 0

            If hue >= 0 AndAlso hue < 60 Then
                r = chroma : g = x : b = 0
            ElseIf hue >= 60 AndAlso hue < 120 Then
                r = x : g = chroma : b = 0
            ElseIf hue >= 120 AndAlso hue < 180 Then
                r = 0 : g = chroma : b = x
            ElseIf hue >= 180 AndAlso hue < 240 Then
                r = 0 : g = x : b = chroma
            ElseIf hue >= 240 AndAlso hue < 300 Then
                r = x : g = 0 : b = chroma
            ElseIf hue >= 300 AndAlso hue < 360 Then
                r = chroma : g = 0 : b = x
            End If

            Return Color.FromRgb((r + m) * 255, (g + m) * 255, (b + m) * 255)
        End Function

    End Class

End Namespace
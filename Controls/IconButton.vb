Namespace Controls

    Public Class IconButton
        Inherits Button

        Public Shared ReadOnly IconProperty As DependencyProperty =
            DependencyProperty.Register("Icon", GetType(ImageSource), GetType(IconButton), New PropertyMetadata(Nothing))

        Public Property Icon As ImageSource
            Get
                Return CType(GetValue(IconProperty), ImageSource)
            End Get
            Set(value As ImageSource)
                SetValue(IconProperty, value)
            End Set
        End Property

        Public Shared ReadOnly IconSizeProperty As DependencyProperty =
            DependencyProperty.Register("IconSize", GetType(Double), GetType(IconButton), New PropertyMetadata(16.0))

        Public Property IconSize As Double
            Get
                Return CType(GetValue(IconSizeProperty), Double)
            End Get
            Set(value As Double)
                SetValue(IconSizeProperty, value)
            End Set
        End Property

    End Class

End Namespace

Namespace Views
    ''' <summary>
    ''' Interaction logic for HeaderControl.xaml
    ''' </summary>
    Partial Public Class HeaderControl
        Inherits UserControl

        ''' <summary>
        ''' Dependency Property for customizable header content.
        ''' </summary>
        Public Shared ReadOnly HeaderContentProperty As DependencyProperty = DependencyProperty.Register(
            NameOf(HeaderContent),
            GetType(Object),
            GetType(HeaderControl),
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Gets or sets the content displayed in the header.
        ''' </summary>
        Public Property HeaderContent As Object
            Get
                Return GetValue(HeaderContentProperty)
            End Get
            Set(value As Object)
                SetValue(HeaderContentProperty, value)
            End Set
        End Property

    End Class

End Namespace

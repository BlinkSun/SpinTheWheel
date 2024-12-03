Namespace Views
    ''' <summary>
    ''' Interaction logic for HeaderControl.xaml
    ''' </summary>
    Partial Public Class HeaderControl
        Inherits UserControl

        ''' <summary>
        ''' Dependency Property for customizable content in the header.
        ''' </summary>
        Public Shared ReadOnly HeaderContentProperty As DependencyProperty = DependencyProperty.Register(
            NameOf(Content),
            GetType(Object),
            GetType(HeaderControl),
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Gets or sets the content displayed in the header.
        ''' </summary>
        Public Overloads Property Content As Object
            Get
                Return GetValue(ContentProperty)
            End Get
            Set(value As Object)
                SetValue(ContentProperty, value)
            End Set
        End Property

    End Class

End Namespace

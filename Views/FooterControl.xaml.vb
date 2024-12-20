﻿Namespace Views
    ''' <summary>
    ''' Interaction logic for FooterControl.xaml
    ''' </summary>
    Partial Public Class FooterControl
        Inherits UserControl

        ''' <summary>
        ''' Dependency Property for customizable footer content.
        ''' </summary>
        Public Shared ReadOnly FooterContentProperty As DependencyProperty = DependencyProperty.Register(
            NameOf(FooterContent),
            GetType(Object),
            GetType(FooterControl),
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Gets or sets the content displayed in the footer.
        ''' </summary>
        Public Property FooterContent As Object
            Get
                Return GetValue(FooterContentProperty)
            End Get
            Set(value As Object)
                SetValue(FooterContentProperty, value)
            End Set
        End Property

    End Class

End Namespace

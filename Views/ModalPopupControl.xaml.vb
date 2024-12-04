Imports System.Collections.ObjectModel

Namespace Views

    Partial Public Class ModalPopupControl
        Inherits UserControl

        Private ReadOnly PopupInputValideColor As SolidColorBrush
        Private ReadOnly PopupInputInvalideColor As SolidColorBrush

        ' Constructor
        Public Sub New()
            InitializeComponent()
            Buttons = New ObservableCollection(Of UIElement)()
            PopupInputValideColor = PopupInput.BorderBrush
            PopupInputInvalideColor = New SolidColorBrush(Colors.Red)
        End Sub

        ' Dependency Properties
        ' Title Property
        Public Shared ReadOnly TitleProperty As DependencyProperty = DependencyProperty.Register(
            NameOf(Title), GetType(String), GetType(ModalPopupControl), New PropertyMetadata(String.Empty))

        Public Property Title As String
            Get
                Return CType(GetValue(TitleProperty), String)
            End Get
            Set(value As String)
                SetValue(TitleProperty, value)
            End Set
        End Property

        ' Message Property
        Public Shared ReadOnly MessageProperty As DependencyProperty = DependencyProperty.Register(
            NameOf(Message), GetType(String), GetType(ModalPopupControl), New PropertyMetadata(String.Empty))

        Public Property Message As String
            Get
                Return CType(GetValue(MessageProperty), String)
            End Get
            Set(value As String)
                SetValue(MessageProperty, value)
            End Set
        End Property

        ' UserInput Property
        Public Shared ReadOnly UserInputProperty As DependencyProperty = DependencyProperty.Register(
            NameOf(UserInput), GetType(String), GetType(ModalPopupControl), New PropertyMetadata(String.Empty))

        Public Property UserInput As String
            Get
                Return CType(GetValue(UserInputProperty), String)
            End Get
            Set(value As String)
                SetValue(UserInputProperty, value)
            End Set
        End Property

        ' IsInputVisible Property
        Public Shared ReadOnly IsInputVisibleProperty As DependencyProperty = DependencyProperty.Register(
            NameOf(IsInputVisible), GetType(Boolean), GetType(ModalPopupControl), New PropertyMetadata(False))

        Public Property IsInputVisible As Boolean
            Get
                Return GetValue(IsInputVisibleProperty)
            End Get
            Set(value As Boolean)
                SetValue(IsInputVisibleProperty, value)
            End Set
        End Property

        ' Buttons Property
        Public Shared ReadOnly ButtonsProperty As DependencyProperty = DependencyProperty.Register(
            NameOf(Buttons), GetType(ObservableCollection(Of UIElement)), GetType(ModalPopupControl), New PropertyMetadata(Nothing))

        Public Property Buttons As ObservableCollection(Of UIElement)
            Get
                Return CType(GetValue(ButtonsProperty), ObservableCollection(Of UIElement))
            End Get
            Set(value As ObservableCollection(Of UIElement))
                SetValue(ButtonsProperty, value)
            End Set
        End Property

        ' IsPopupVisible Property
        Public Shared ReadOnly IsPopupVisibleProperty As DependencyProperty = DependencyProperty.Register(
            NameOf(IsPopupVisible), GetType(Boolean), GetType(ModalPopupControl), New PropertyMetadata(False, AddressOf OnIsPopupVisibleChanged))

        Public Property IsPopupVisible As Boolean
            Get
                Return GetValue(IsPopupVisibleProperty)
            End Get
            Set(value As Boolean)
                SetValue(IsPopupVisibleProperty, value)
            End Set
        End Property

        ' IsModal Property
        Public Shared ReadOnly IsModalProperty As DependencyProperty = DependencyProperty.Register(
            NameOf(IsModal), GetType(Boolean), GetType(ModalPopupControl), New PropertyMetadata(False))

        Public Property IsModal As Boolean
            Get
                Return GetValue(IsModalProperty)
            End Get
            Set(value As Boolean)
                SetValue(IsModalProperty, value)
            End Set
        End Property

        ' DialogResult Property
        Public Shared ReadOnly DialogResultProperty As DependencyProperty = DependencyProperty.Register(
            NameOf(DialogResult), GetType(Boolean?), GetType(ModalPopupControl), New PropertyMetadata(Nothing))

        Public Property DialogResult As Boolean?
            Get
                Return GetValue(DialogResultProperty)
            End Get
            Set(value As Boolean?)
                SetValue(DialogResultProperty, value)
            End Set
        End Property

        ' Event Handlers
        Private Shared Sub OnIsPopupVisibleChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim control As ModalPopupControl = DirectCast(d, ModalPopupControl)
            If control.IsPopupVisible Then
                control.Focusable = True
                control.Focus()
                If control.IsInputVisible Then
                    control.PopupInput.Focus()
                End If
            End If
        End Sub

        Private Sub Backdrop_MouseDown(sender As Object, e As MouseButtonEventArgs)
            If Not IsModal Then
                IsPopupVisible = False
            End If
        End Sub

        Private Sub ModalPopupControl_PreviewKeyDown(sender As Object, e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Visibility = Visibility.Hidden
                e.Handled = True
            ElseIf e.Key = Key.Enter Then
                If IsInputVisible AndAlso String.IsNullOrWhiteSpace(UserInput) Then
                    PopupInput.BorderBrush = PopupInputInvalideColor
                Else
                    IsPopupVisible = False
                End If
                e.Handled = True
            End If
        End Sub

        Private Sub UserControl_Click(sender As Object, e As RoutedEventArgs)
            Dim originalElement = TryCast(e.OriginalSource, FrameworkElement)
            If originalElement IsNot Nothing Then
                Dim button = TryCast(originalElement, Button)
                If button IsNot Nothing Then
                    If button.IsDefault Then
                        If IsInputVisible Then
                            If String.IsNullOrEmpty(UserInput) Then
                                PopupInput.BorderBrush = PopupInputInvalideColor
                                e.Handled = True
                                Return
                            End If
                        End If
                        DialogResult = True
                    ElseIf button.IsCancel Then
                        DialogResult = False
                    End If
                End If
            End If
            IsPopupVisible = False
            e.Handled = True
        End Sub

        Private Sub PopupInput_TextChanged(sender As Object, e As TextChangedEventArgs)
            If Not String.IsNullOrEmpty(UserInput) Then
                PopupInput.BorderBrush = PopupInputValideColor
            End If
        End Sub

    End Class

End Namespace
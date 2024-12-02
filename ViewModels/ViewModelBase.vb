Imports System.ComponentModel

Namespace ViewModels

    ''' <summary>
    ''' Base class for ViewModels implementing INotifyPropertyChanged.
    ''' </summary>
    Public MustInherit Class ViewModelBase
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        ''' <summary>
        ''' Notifies UI of a property change.
        ''' </summary>
        ''' <param name="propertyName">The name of the property that changed.</param>
        Protected Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

    End Class

End Namespace
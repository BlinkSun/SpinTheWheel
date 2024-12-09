Imports System.ComponentModel

Namespace Models

    ''' <summary>
    ''' Represents a participant entity in the database.
    ''' </summary>
    Public Class Participant
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        ''' <summary>
        ''' Notifies UI of a property change.
        ''' </summary>
        ''' <param name="propertyName">The name of the property that changed.</param>
        Protected Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        ''' <summary>
        ''' Gets or sets the unique identifier for the participant.
        ''' </summary>
        Public Property Id As Integer
            Get
                Return idValue
            End Get
            Set(value As Integer)
                If Not idValue.Equals(value) Then
                    idValue = value
                    OnPropertyChanged(NameOf(Id))
                End If
            End Set
        End Property
        Private idValue As Integer

        ''' <summary>
        ''' Gets or sets the name of the participant.
        ''' </summary>
        Public Property Name As String
            Get
                Return nameValue
            End Get
            Set(value As String)
                If Not Equals(nameValue, value) Then
                    nameValue = value
                    OnPropertyChanged(NameOf(Name))
                End If
            End Set
        End Property
        Private nameValue As String

        ''' <summary>
        ''' Gets or sets whether the participant has been flagged as "done".
        ''' </summary>
        Public Property Done As Boolean
            Get
                Return doneValue
            End Get
            Set(value As Boolean)
                If Not doneValue.Equals(value) Then
                    doneValue = value
                    OnPropertyChanged(NameOf(Done))
                End If
            End Set
        End Property
        Private doneValue As Boolean = False

        ''' <summary>
        ''' Gets or sets the participant cardinal order has been flagged as "done".
        ''' </summary>
        Public Property Order As Integer
            Get
                Return orderValue
            End Get
            Set(value As Integer)
                If Not orderValue.Equals(value) Then
                    orderValue = value
                    OnPropertyChanged(NameOf(Order))
                End If
            End Set
        End Property
        Private orderValue As Integer = -1

    End Class

End Namespace

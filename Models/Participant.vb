Namespace Models

    ''' <summary>
    ''' Represents a participant entity in the database.
    ''' </summary>
    Public Class Participant

        ''' <summary>
        ''' Gets or sets the unique identifier for the participant.
        ''' </summary>
        Public Property Id As Integer

        ''' <summary>
        ''' Gets or sets the name of the participant.
        ''' </summary>
        Public Property Name As String

        ''' <summary>
        ''' Gets or sets whether the participant has been flagged as "done".
        ''' </summary>
        Public Property Done As Boolean = False

        ''' <summary>
        ''' Gets or sets the participant cardinal order has been flagged as "done".
        ''' </summary>
        Public Property Order As Integer = -1

    End Class

End Namespace

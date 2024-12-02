Namespace ViewModels

    ''' <summary>
    ''' Simplified implementation of ICommand.
    ''' </summary>
    Public Class RelayCommand
        Implements ICommand

        Private ReadOnly executeAction As Action
        Private ReadOnly canExecuteFunction As Func(Of Boolean)

        ''' <summary>
        ''' Event raised when the CanExecute status changes.
        ''' </summary>
        Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

        ''' <summary>
        ''' Initializes a new instance of the RelayCommand class.
        ''' </summary>
        ''' <param name="execute">The action to execute when the command is invoked.</param>
        ''' <param name="canExecute">A function that determines whether the command can execute.</param>
        Public Sub New(execute As Action, Optional canExecute As Func(Of Boolean) = Nothing)
            executeAction = execute
            canExecuteFunction = canExecute
        End Sub

        ''' <summary>
        ''' Determines whether the command can execute.
        ''' </summary>
        ''' <param name="parameter">The command parameter (ignored in this implementation).</param>
        ''' <returns>True if the command can execute; otherwise, false.</returns>
        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
            Return If(canExecuteFunction Is Nothing, True, canExecuteFunction.Invoke())
        End Function

        ''' <summary>
        ''' Executes the command.
        ''' </summary>
        ''' <param name="parameter">The command parameter (ignored in this implementation).</param>
        Public Sub Execute(parameter As Object) Implements ICommand.Execute
            executeAction.Invoke()
        End Sub

        ''' <summary>
        ''' Raises the CanExecuteChanged event.
        ''' </summary>
        Public Sub RaiseCanExecuteChanged()
            RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
        End Sub

    End Class

End Namespace
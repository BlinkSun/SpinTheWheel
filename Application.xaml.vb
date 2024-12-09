Imports SpinTheWheel.Services

''' <summary>
''' Interaction logic for Application.xaml
''' </summary>
Partial Public Class Application
    Inherits System.Windows.Application
    ''' <summary>
    ''' Initializes a new instance of the Application class.
    ''' Ensures the database is initialized before the application components are loaded.
    ''' </summary>
    Public Sub New()
        ' Initialize the database before loading application components
        DatabaseService.InitializeDatabase()
        InitializeComponent()
        ThemeManager.LoadTheme()
    End Sub

    ''' <summary>
    ''' Singleton instance of the DatabaseService.
    ''' </summary>
    Public Shared ReadOnly Property DatabaseService As New DatabaseService()

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

End Class
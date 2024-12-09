Imports System.Data.SQLite
Imports System.IO
Imports SpinTheWheel.Models

Namespace Services

    ''' <summary>
    ''' Service responsible for managing the SQLite database and performing CRUD operations.
    ''' </summary>
    Public Class DatabaseService

        ''' <summary>
        ''' Path to the SQLite database file.
        ''' </summary>
        Private ReadOnly databaseFilePath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database.db")

        ''' <summary>
        ''' Initializes the database. Creates the file and table if they do not exist.
        ''' </summary>
        Public Sub InitializeDatabase()
            Try
                ' Create database file if it doesn't exist
                If Not File.Exists(databaseFilePath) Then
                    SQLiteConnection.CreateFile(databaseFilePath)
                End If

                ' Ensure the Participant table exists
                Using connection As SQLiteConnection = GetConnection()
                    connection.Open()
                    Dim createTableQuery As String = "
                        CREATE TABLE IF NOT EXISTS Participant (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            Done INTEGER DEFAULT 0 NOT NULL,
                            ""Order"" INTEGER DEFAULT -1 NOT NULL
                        );"
                    Using command As New SQLiteCommand(createTableQuery, connection)
                        command.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                Throw New ApplicationException($"Erreur lors de l'initialisation de la base de données : {ex.Message}", ex)

            End Try
        End Sub

        ''' <summary>
        ''' Provides a connection to the SQLite database.
        ''' </summary>
        ''' <returns>A new instance of SQLiteConnection.</returns>
        Public Function GetConnection() As SQLiteConnection
            Return New SQLiteConnection($"Data Source={databaseFilePath};Version=3;")
        End Function

        ''' <summary>
        ''' Retrieves all participants from the database.
        ''' </summary>
        ''' <returns>A list of Participant objects.</returns>
        Public Function GetParticipants() As List(Of Participant)
            Dim participants As New List(Of Participant)()
            Try
                Using connection As SQLiteConnection = GetConnection()
                    connection.Open()
                    Dim selectQuery As String = "SELECT Id, Name, Done, ""Order"" FROM Participant ORDER BY Done DESC, ""Order"" ASC;"
                    Using command As New SQLiteCommand(selectQuery, connection)
                        Using reader As SQLiteDataReader = command.ExecuteReader()
                            While reader.Read()
                                participants.Add(New Participant() With {
                                    .Id = reader.GetInt32(0),
                                    .Name = reader.GetString(1),
                                    .Done = reader.GetBoolean(2),
                                    .Order = reader.GetInt32(3)
                                })
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                Throw New ApplicationException($"Erreur lors de la récupération des participants : {ex.Message}", ex)
            End Try
            Return participants
        End Function


        ''' <summary>
        ''' Adds a new participant to the database and updates its properties with the generated values.
        ''' </summary>
        ''' <param name="participant">The participant to add.</param>
        Public Sub AddParticipant(participant As Participant)
            Try
                Using connection As SQLiteConnection = GetConnection()
                    connection.Open()
                    Dim insertQuery As String = "INSERT INTO Participant (Name) VALUES (@Name);"

                    Using command As New SQLiteCommand(insertQuery, connection)
                        command.Parameters.AddWithValue("@Name", participant.Name)
                        command.ExecuteNonQuery()
                    End Using

                    Dim selectQuery As String = "SELECT last_insert_rowid();"
                    Using command As New SQLiteCommand(selectQuery, connection)
                        Dim newId As Long = Convert.ToInt32(command.ExecuteScalar())
                        participant.Id = newId
                    End Using
                End Using
            Catch ex As Exception
                Throw New ApplicationException($"Erreur lors de l'ajout du participant : {ex.Message}", ex)
            End Try
        End Sub


        ''' <summary>
        ''' Updates an existing participant in the database.
        ''' </summary>
        ''' <param name="participant">The participant to update.</param>
        Public Sub UpdateParticipant(participant As Participant)
            Try
                Using connection As SQLiteConnection = GetConnection()
                    connection.Open()
                    Dim updateQuery As String = "UPDATE Participant SET Name = @Name, Done = @Done, ""Order"" = @Order WHERE Id = @Id;"
                    Using command As New SQLiteCommand(updateQuery, connection)
                        command.Parameters.AddWithValue("@Id", participant.Id)
                        command.Parameters.AddWithValue("@Name", participant.Name)
                        command.Parameters.AddWithValue("@Done", participant.Done)
                        command.Parameters.AddWithValue("@Order", participant.Order)
                        command.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                Throw New ApplicationException($"Erreur lors de la mise à jour du participant : {ex.Message}", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Reset flags of all participants in the database.
        ''' </summary>
        Public Sub ResetParticipants()
            Try
                Using connection As SQLiteConnection = GetConnection()
                    connection.Open()
                    Dim updateQuery As String = "UPDATE Participant SET Done = 0, ""Order"" = -1;"
                    Using command As New SQLiteCommand(updateQuery, connection)
                        command.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                Throw New ApplicationException($"Erreur lors de la mise à jour du participant : {ex.Message}", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Deletes a participant from the database.
        ''' </summary>
        ''' <param name="participant">The participant to delete.</param>
        Public Sub DeleteParticipant(participant As Participant)
            Try
                Using connection As SQLiteConnection = GetConnection()
                    connection.Open()
                    Dim deleteQuery As String = "DELETE FROM Participant WHERE Id = @Id;"
                    Using command As New SQLiteCommand(deleteQuery, connection)
                        command.Parameters.AddWithValue("@Id", participant.Id)
                        command.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                Throw New ApplicationException($"Erreur lors de la suppression du participant : {ex.Message}", ex)

            End Try
        End Sub

        ''' <summary>
        ''' Deletes all participants from the database.
        ''' </summary>
        Public Sub ClearParticipants()
            Try
                Using connection As SQLiteConnection = GetConnection()
                    connection.Open()
                    Dim deleteQuery As String = "DELETE FROM Participant;"
                    Using command As New SQLiteCommand(deleteQuery, connection)
                        command.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                Throw New ApplicationException($"Erreur lors de la suppression de tous les participants : {ex.Message}", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Retrieves a random participant whose Done flag is false (0).
        ''' </summary>
        ''' <returns>A random Participant object, or Nothing if none are available.</returns>
        Public Function GetRandomParticipant() As Participant
            Try
                Using connection As SQLiteConnection = GetConnection()
                    connection.Open()
                    Dim randomQuery As String = " SELECT Id, Name, Done 
                                                    FROM Participant 
                                                    WHERE Done = 0 
                                                    ORDER BY RANDOM() 
                                                    LIMIT 1;"
                    Using command As New SQLiteCommand(randomQuery, connection)
                        Using reader As SQLiteDataReader = command.ExecuteReader()
                            If reader.Read() Then
                                Return New Participant() With {
                                    .Id = reader.GetInt32(0),
                                    .Name = reader.GetString(1),
                                    .Done = reader.GetBoolean(2)
                                }
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                Throw New ApplicationException($"Erreur lors de la récupération d'un participant aléatoire : {ex.Message}", ex)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Retrieves the next available number for the "Order" column.
        ''' </summary>
        ''' <returns>The next integer value for the "Order" column.</returns>
        Public Function GetNextOrderValue() As Integer
            Try
                Using connection As SQLiteConnection = GetConnection()
                    connection.Open()
                    Dim query As String = "SELECT COALESCE(MAX(""Order""), 0) FROM Participant WHERE Done = 1;"
                    Using command As New SQLiteCommand(query, connection)
                        Dim maxOrder As Integer = Convert.ToInt32(command.ExecuteScalar())
                        Return maxOrder + 1
                    End Using
                End Using
            Catch ex As Exception
                Throw New ApplicationException($"Erreur lors de la récupération de la prochaine valeur d'ordre : {ex.Message}", ex)
            End Try
        End Function

    End Class

End Namespace
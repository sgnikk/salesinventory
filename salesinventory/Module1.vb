Imports System.Data.SqlClient

Module Module1
    Public connectionStrings As String = "Data Source=192.168.1.100\SQLEXPRESS;Initial Catalog=salesinventory;Integrated Security=True"

    Public connection As New SqlConnection(connectionStrings)

    Public Sub OpenConnection()
        Try
            connection = New SqlConnection(connectionStrings)
            connection.Open()
            Console.WriteLine("Connection opened successfully.")
        Catch ex As Exception
            Console.WriteLine("Error opening connection: " & ex.Message)
        End Try
    End Sub

    Public Sub CloseConnection()
        Try
            If connection IsNot Nothing AndAlso connection.State = ConnectionState.Open Then
                connection.Close()
                Console.WriteLine("Connection closed successfully.")
            End If
        Catch ex As Exception
            Console.WriteLine("Error closing connection: " & ex.Message)
        End Try
    End Sub

End Module

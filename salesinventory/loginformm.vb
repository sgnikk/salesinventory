Imports System.Data.SqlClient
Public Class loginformm

    Public Property UserID As Integer
    Public Property LoggedInUsername As String

    Dim connection As New SqlConnection(Module1.connectionStrings)

    Private Sub loginformm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtpassword.UseSystemPasswordChar = True
    End Sub

    Public Function GetLoggedInUserID() As Integer
        Return UserID
    End Function
    Public Sub ValidateLogin(username As String, password As String)
        Dim query As String = "SELECT users_id, Username FROM tblusers WHERE Username COLLATE Latin1_General_CS_AS = @Username AND Password COLLATE Latin1_General_CS_AS = @Password;"
        Dim command As New SqlCommand(query, connection)
        command.Parameters.AddWithValue("@Username", username)
        command.Parameters.AddWithValue("@Password", password)

        connection.Open()
        Dim reader As SqlDataReader = command.ExecuteReader()
        If reader.Read() Then
            UserID = Convert.ToInt32(reader("users_id"))
            LoggedInUsername = reader("Username").ToString()
        Else
            UserID = -1
            LoggedInUsername = ""
        End If
        connection.Close()
    End Sub

    Private Sub Guna2CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox1.CheckedChanged
        txtpassword.UseSystemPasswordChar = Not Guna2CheckBox1.Checked
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        Dim username As String = txtusername.Text
        Dim password As String = txtpassword.Text

        ValidateLogin(username, password)

        If UserID <> -1 Then
            Form1.Show()
            Dim userID As Integer = Me.UserID
            Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
            Dim timee As String = DateTime.Now.ToString("HH:mm")
            connection.Open()

            Using cmdinsert As New SqlCommand("INSERT INTO tblaudit (users_id,actions,time,date) VALUES (@uID,@acts,@time,@date)", connection)
                cmdinsert.Parameters.AddWithValue("@uID", userID)
                cmdinsert.Parameters.AddWithValue("@acts", $"'{LoggedInUsername}' Has LogIn") ' Use the logged-in username here
                cmdinsert.Parameters.AddWithValue("@time", timee)
                cmdinsert.Parameters.AddWithValue("@date", datee)
                cmdinsert.ExecuteNonQuery()
                Form1.AuditTrail()
            End Using
            connection.Close()
        Else
            MessageBox.Show("Invalid username or password. Please try again.")
            txtusername.Clear()
            txtpassword.Clear()
            txtusername.Focus()
        End If
    End Sub

    Private Sub Guna2TextBox4_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox4.TextChanged

    End Sub

    Private Sub Guna2TextBox1_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox1.TextChanged

    End Sub
End Class
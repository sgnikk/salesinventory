Imports System.Data.SqlClient

Public Class addtype


    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub


    Public Sub insertaudit()
        Dim loggedInUsername As String = loginformm.LoggedInUsername
        Dim userID As Integer = loginformm.UserID
        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim timee As String = DateTime.Now.ToString("HH:mm")
        Dim action As String = $"{loggedInUsername} Added A New Medicine Type :'{txttype.Text} '"
        Dim auditInsertQuery As String = "INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)"
        Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection)
            auditInsertCommand.Parameters.AddWithValue("@uID", userID)
            auditInsertCommand.Parameters.AddWithValue("@acts", action)
            auditInsertCommand.Parameters.AddWithValue("@time", timee)
            auditInsertCommand.Parameters.AddWithValue("@date", datee)
            auditInsertCommand.ExecuteNonQuery()
            Form1.AuditTrail()
        End Using
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click

        If MedicineTypeExists(txttype.Text) Then
            MessageBox.Show("Medicine type with the same name already exists. Please enter a different name.")
            Return
        End If

        Try
            OpenConnection()
            Using cmdinsert As New SqlCommand("INSERT INTO tblmedtype (MEDICINE_TYPE) VALUES (@medtype)", connection)
                cmdinsert.Parameters.AddWithValue("@medtype", txttype.Text)
                cmdinsert.ExecuteNonQuery()
                MessageBox.Show("Added Successfully")
                Form1.displaymedtype()
                insertaudit()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub ValidateInput(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[0-9]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub

    Private Sub txttype_TextChanged(sender As Object, e As EventArgs) Handles txttype.TextChanged
        ValidateInput(txttype)
    End Sub

    Private Function MedicineTypeExists(medTypeName As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM tblmedtype WHERE MEDICINE_TYPE = @medTypeName"
        Dim count As Integer

        Using connection As New SqlConnection(Module1.connectionStrings)
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@medTypeName", medTypeName)
                connection.Open()
                count = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using

        Return count > 0
    End Function
End Class

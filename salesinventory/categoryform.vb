Imports System.Data.SqlClient

Public Class categoryform

    Dim connectionString As String = "Data Source=DESKTOP-1A0SD84\SQLEXPRESS;Initial Catalog=salesinventory;Integrated Security=True"

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        Dim insertQuery As String = "INSERT INTO tblcategory (CATEGORY_DESCRIPTION, CATEGORY_NAME) VALUES (@cdescription, @cname)"
        Using connection As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(insertQuery, connection)
                cmd.Parameters.AddWithValue("@cdescription", txtdescription.Text)
                cmd.Parameters.AddWithValue("@cname", txtname.Text)
                connection.Open()
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                If rowsAffected > 0 Then
                    MessageBox.Show("Category added successfully.")
                    txtdescription.Clear()
                    txtname.Clear()
                    Form1.DisplayCategory()
                Else
                    MessageBox.Show("Failed to add category.")
                End If
            End Using
        End Using
        auditcategory()
    End Sub

    Public Sub auditcategory()
        Dim loggedInUsername As String = loginformm.LoggedInUsername
        Dim userID As Integer = loginformm.UserID
        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim timee As String = DateTime.Now.ToString("HH:mm")
        connection.Open()
        Dim action As String = $"{loggedInUsername} Added A New Category Name :'{txtname.Text} Category Description:{txtdescription.Text} '"
        Dim auditInsertQuery As String = "INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)"
        Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection)
            auditInsertCommand.Parameters.AddWithValue("@uID", userID)
            auditInsertCommand.Parameters.AddWithValue("@acts", action)
            auditInsertCommand.Parameters.AddWithValue("@time", timee)
            auditInsertCommand.Parameters.AddWithValue("@date", datee)
            auditInsertCommand.ExecuteNonQuery()
        End Using
        connection.Open()
        Form1.AuditTrail()
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub

    Private Sub txtname_TextChanged(sender As Object, e As EventArgs) Handles txtname.TextChanged
        ValidateInput(txtname)
    End Sub

    Private Sub txtdescription_TextChanged(sender As Object, e As EventArgs) Handles txtdescription.TextChanged
        ValidateInput(txtdescription)
    End Sub

    Private Sub ValidateInput(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[0-9]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub

End Class

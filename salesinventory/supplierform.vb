Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class supplierform
    Dim connection As New SqlConnection(Module1.connectionStrings)

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub



    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click

        Dim query As String = "SELECT COUNT(*) FROM tblsupplier WHERE SUPPLIER_NAME = @sname"
        Using cmd As New SqlCommand(query, connection)
            connection.Open()
            cmd.Parameters.AddWithValue("@sname", txtsuppliername.Text)
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            connection.Close()

            If count > 0 Then
                MessageBox.Show("Supplier name already exists.")
                Return
            End If
        End Using

        ' Check if the contact number already exists
        query = "SELECT COUNT(*) FROM tblsupplier WHERE CONTACT = @contact"
        Using cmd As New SqlCommand(query, connection)
            connection.Open()
            cmd.Parameters.AddWithValue("@contact", txtcontact.Text)
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            connection.Close()

            If count > 0 Then
                MessageBox.Show("Contact number already exists.")
                Return
            End If
        End Using
        Using cmd As New SqlCommand("INSERT INTO tblsupplier (SUPPLIER_NAME, ADDRESS, CONTACT) VALUES (@sname, @address, @contact)", connection)
            connection.Open()
            cmd.Parameters.AddWithValue("@sname", txtsuppliername.Text)
            cmd.Parameters.AddWithValue("@address", txtaddress.Text)
            cmd.Parameters.AddWithValue("@contact", txtcontact.Text)
            cmd.ExecuteNonQuery()
            connection.Close()
            Form1.DisplaySupplier()
            INSERTAUDIT()
            MessageBox.Show("Successfully Added")
        End Using
    End Sub

    Private Sub txtsuppliername_TextChanged(sender As Object, e As EventArgs) Handles txtsuppliername.TextChanged
        ValidateInput(txtsuppliername)
    End Sub
    Private Sub ValidateInput(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[0-9]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub
    Private Sub ValidatesInput(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[^0-9\s-]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub

    Private Sub txtcontact_TextChanged(sender As Object, e As EventArgs) Handles txtcontact.TextChanged
        ValidateSInput(txtcontact)
    End Sub

    Private Sub supplierform_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub INSERTAUDIT()
        Dim loggedInUsername As String = loginformm.LoggedInUsername
        Dim userID As Integer = loginformm.UserID
        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim timee As String = DateTime.Now.ToString("HH:mm")
        Dim action As String = $"{loggedInUsername} Added A New Supplier, Supplier Name: {txtsuppliername.Text}'"
        connection.Open()
        Dim auditInsertQuery As String = "INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)"
        Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection)
            auditInsertCommand.Parameters.AddWithValue("@uID", userID)
            auditInsertCommand.Parameters.AddWithValue("@acts", action)
            auditInsertCommand.Parameters.AddWithValue("@time", timee)
            auditInsertCommand.Parameters.AddWithValue("@date", datee)
            auditInsertCommand.ExecuteNonQuery()
            Form1.AuditTrail()
        End Using
        connection.Close()
    End Sub
End Class
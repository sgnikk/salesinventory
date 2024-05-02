Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class supplierform
    Dim connection As New SqlConnection("Data Source=DESKTOP-1A0SD84\SQLEXPRESS;Initial Catalog=salesinventory;Integrated Security=True")

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        Using cmd As New SqlCommand("INSERT INTO tblsupplier (SUPPLIER_NAME, ADDRESS, CONTACT) VALUES (@sname, @address, @contact)", connection)
            connection.Open()
            cmd.Parameters.AddWithValue("@sname", txtsuppliername.Text)
            cmd.Parameters.AddWithValue("@address", txtaddress.Text)
            cmd.Parameters.AddWithValue("@contact", txtcontact.Text)
            cmd.ExecuteNonQuery()
            connection.Close()
            Form1.DisplaySupplier()
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
End Class
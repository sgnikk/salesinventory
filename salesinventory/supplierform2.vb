Imports System.Data.SqlClient
Public Class supplierform2
    Dim connection As New SqlConnection("Data Source=DESKTOP-1A0SD84\SQLEXPRESS;Initial Catalog=salesinventory;Integrated Security=True")

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub

    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        connection.Open()
        Dim transaction As SqlTransaction = connection.BeginTransaction
        Using cmdupdate As New SqlCommand("UPDATE tblsupplier SET SUPPLIER_NAME = @sname, ADDRESS = @address, CONTACT = @contact WHERE SUPPLIER_ID = @sid", connection, transaction)
            cmdupdate.Parameters.AddWithValue("@sname", txtsuppliername.Text)
            cmdupdate.Parameters.AddWithValue("@address", txtaddress.Text)
            cmdupdate.Parameters.AddWithValue("@contact", txtcontact.Text)
            cmdupdate.Parameters.AddWithValue("@sid", txtid.Text)
            cmdupdate.ExecuteNonQuery()
            MessageBox.Show("Updated Successfully")
        End Using
        transaction.Commit()
        connection.Close()
        Form1.DisplaySupplier()
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
        ValidatesInput(txtcontact)
    End Sub
End Class
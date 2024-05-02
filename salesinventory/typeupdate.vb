Imports System.Data.SqlClient
Public Class typeupdate
    Dim connection As New SqlConnection("Data Source=DESKTOP-1A0SD84\SQLEXPRESS;Initial Catalog=salesinventory;Integrated Security=True")
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub

    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        connection.Open()
        Dim transaction As SqlTransaction = connection.BeginTransaction
        Using cmdupdate As New SqlCommand("UPDATE tblmedtype SET MEDICINE_TYPE = @mtype WHERE MEDTYPE_ID = @mid", connection, transaction)
            cmdupdate.Parameters.AddWithValue("@mtype", txttype.Text)
            cmdupdate.Parameters.AddWithValue("@mid", txtid.Text)
            cmdupdate.ExecuteNonQuery()
        End Using
        transaction.Commit()
        connection.Close()
        Form1.displaymedtype()
        Form1.DisplayItem()
    End Sub

    Private Sub ValidateInput(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[0-9]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub

    Private Sub txttype_TextChanged(sender As Object, e As EventArgs) Handles txttype.TextChanged
        ValidateInput(txttype)
    End Sub
End Class
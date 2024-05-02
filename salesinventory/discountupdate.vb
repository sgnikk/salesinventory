Imports System.Data.SqlClient
Public Class discountupdate
    Dim connection As New SqlConnection(Module1.connectionStrings)
    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        connection.Open()
        Using cmdupdate As New SqlCommand("UPDATE tbldiscount SET discount_name = @dname, discount_amount = @damount WHERE discount_id = @did", connection)
            cmdupdate.Parameters.AddWithValue("@dname", txtdiscountname.Text)
            cmdupdate.Parameters.AddWithValue("@damount", txtdiscountamount.Text)
            cmdupdate.Parameters.AddWithValue("@did", txtids.Text)
            cmdupdate.ExecuteNonQuery()
            MessageBox.Show("Update Successfully")
            Form1.discount()
        End Using
        connection.Close()
    End Sub

    'to allow letters only
    Private Sub ValidateInputLetters(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[0-9]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub
    'to allow numbers only
    Private Sub ValidatesInputNumber(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[^0-9\s-%.]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub

    Private Sub txtdiscountname_TextChanged(sender As Object, e As EventArgs) Handles txtdiscountname.TextChanged
        ValidateInputLetters(txtdiscountname)
    End Sub

    Private Sub txtdiscountamount_TextChanged(sender As Object, e As EventArgs) Handles txtdiscountamount.TextChanged
        ValidatesInputNumber(txtdiscountamount)
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub

End Class
Imports System.Data.SqlClient

Public Class discountform

    Dim connection As New SqlConnection(Module1.connectionStrings)
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        connection.Open()
        Using cmdinsert As New SqlCommand("INSERT INTO tbldiscount (discount_name, discount_amount) values (@dname,@damount)", connection)
            cmdinsert.Parameters.AddWithValue("@dname", txtdiscountname.Text)
            cmdinsert.Parameters.AddWithValue("@damount", txtdiscountamount.Text)
            cmdinsert.ExecuteNonQuery()
            MessageBox.Show("Added Successfully")
            Form1.discount()
            insertaudit()
        End Using
        connection.Close()
    End Sub
    Public Sub insertaudit()
        Dim loggedInUsername As String = loginformm.LoggedInUsername
        Dim userID As Integer = loginformm.UserID
        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim timee As String = DateTime.Now.ToString("HH:mm")
        Dim action As String = $"{loggedInUsername} Added A New Discount, Discount Name:'{txtdiscountname.Text}, Discount Amount:{txtdiscountamount.Text} '"
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

        Dim textWithoutDots As String = textBox.Text.Replace(".", "")
        If textWithoutDots.Length > 0 AndAlso textWithoutDots.LastIndexOf(".") < textWithoutDots.Length - 1 Then
            textBox.Text = textWithoutDots.Substring(0, textWithoutDots.LastIndexOf(".")) + "." + textBox.Text.Substring(textWithoutDots.LastIndexOf(".") + 1)
        End If

        textBox.SelectionStart = textBox.Text.Length
    End Sub



    Private Sub txtdiscountname_TextChanged(sender As Object, e As EventArgs) Handles txtdiscountname.TextChanged
        ValidateInputLetters(txtdiscountname)
    End Sub

    Private Sub txtdiscountamount_TextChanged(sender As Object, e As EventArgs) Handles txtdiscountamount.TextChanged
        ValidatesInputNumber(txtdiscountamount)
    End Sub
End Class
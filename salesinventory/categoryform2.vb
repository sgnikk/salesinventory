Imports System.Data.SqlClient

Public Class categoryform2
    Dim connectionString As String = "Data Source=DESKTOP-1A0SD84\SQLEXPRESS;Initial Catalog=salesinventory;Integrated Security=True"

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


    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Using transaction As SqlTransaction = conn.BeginTransaction()
                Dim query As String = "UPDATE tblcategory SET CATEGORY_DESCRIPTION = @cdesc, CATEGORY_NAME = @cname WHERE CATEGORY_ID = @categoryID"
                Using cmdupate As New SqlCommand(query, conn, transaction)
                    cmdupate.Parameters.AddWithValue("@cdesc", txtdescription.Text)
                    cmdupate.Parameters.AddWithValue("@cname", txtname.Text)
                    cmdupate.Parameters.AddWithValue("@categoryID", txtid.Text) ' Assuming txtid contains the category ID
                    cmdupate.ExecuteNonQuery()

                End Using
                transaction.Commit()
                Form1.DisplayItem()
                Form1.DisplayCategory()
            End Using
        End Using
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub
End Class

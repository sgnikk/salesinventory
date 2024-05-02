Imports System.Data.SqlClient
Public Class itemsupdate
    Dim connectionString As String = Module1.connectionStrings

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub

    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        Dim discount As String
        If Guna2CheckBox1.Checked Then
            discount = "Discount"
        Else
            discount = "No Discount"
        End If

        Dim isexpirable As String
        If Guna2CheckBox2.Checked Then
            isexpirable = "Yex"
        Else
            isexpirable = "No"
        End If

        Using conn As New SqlConnection(connectionString)
            conn.Open()
            Using transaction As SqlTransaction = conn.BeginTransaction
                Dim query As String = "UPDATE tblitemm SET PRODUCT_NAME = @pname, DESCRIPTION = @desc,CATEGORY_ID = @cid, 
                MEDTYPE_ID = @mid, COSTPRICE_BYPIECE = @cprice, discount = @discount, expirable = @exp, sellingprice = @sprice  WHERE ITEM_ID = @itemID"
                Using cmdupate As New SqlCommand(query, conn, transaction)
                    cmdupate.Parameters.AddWithValue("@pname", txtproductname.Text)
                    cmdupate.Parameters.AddWithValue("@desc", txtdescription.Text)
                    cmdupate.Parameters.AddWithValue("@mid", cmdmedtype.SelectedValue)
                    cmdupate.Parameters.AddWithValue("@cid", cmbcategory.SelectedValue)
                    cmdupate.Parameters.AddWithValue("@cprice", txtcpricepiece.Text)
                    cmdupate.Parameters.AddWithValue("@itemID", txtid.Text)
                    cmdupate.Parameters.AddWithValue("@discount", discount)
                    cmdupate.Parameters.AddWithValue("@exp", isexpirable)
                    cmdupate.Parameters.AddWithValue("@sprice", txtsprice.Text)
                    cmdupate.ExecuteNonQuery()
                    Try
                        Dim rowsAffected As Integer = cmdupate.ExecuteNonQuery()
                        If rowsAffected > 0 Then
                            transaction.Commit()
                            Form1.DisplayItem()
                            MessageBox.Show("Updated successful!")
                        Else
                            transaction.Rollback()
                            MessageBox.Show("no matching records found.")
                        End If
                    Catch ex As Exception
                        transaction.Rollback()
                        MessageBox.Show("error: " & ex.Message)
                    End Try
                End Using
            End Using
        End Using
    End Sub

    Private Sub txtproductname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtproductname.KeyPress
        ValidateInputLetters(txtproductname)
    End Sub
    'to allow letters only
    Private Sub ValidateInputLetters(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[0-9]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub
    'to allow numbers only
    Private Sub ValidatesInputNumber(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[^0-9\s-]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub

    Private Sub txtproductname_TextChanged(sender As Object, e As EventArgs) Handles txtproductname.TextChanged
        ValidateInputLetters(txtproductname)
    End Sub

    Private Sub txtcpricepiece_TextChanged(sender As Object, e As EventArgs) Handles txtcpricepiece.TextChanged
        ValidatesInputNumber(txtcpricepiece)
    End Sub
End Class
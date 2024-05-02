Imports System.Data.SqlClient

Public Class vatmaintenance
    Dim connection As New SqlConnection(Module1.ConnectionStrings)
    Public vatID As Integer

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        ' Log action: Close button clicked
        Me.Close()
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        ' Check if there's already a row with active status set to 'True'
        Dim hasActiveRow As Boolean = False
        Using cmdCheckActive As New SqlCommand("SELECT COUNT(*) FROM tblvat WHERE active = 'True'", connection)
            connection.Open()
            Dim count As Integer = Convert.ToInt32(cmdCheckActive.ExecuteScalar())
            If count > 0 Then
                hasActiveRow = True
            End If
            connection.Close()
        End Using

        ' Check if the user is trying to insert 'True' as active when there's already an active row
        If hasActiveRow AndAlso Guna2CheckBox1.Checked Then
            MessageBox.Show("There's already a row with active status set to 'True'.")
            Return
        End If

        Dim activevat As String
        Dim oldVatAmount As String = ""
        Dim oldActiveStatus As String = ""

        ' Retrieve old VAT amount and old active status
        Dim selectQuery As String = "SELECT vat_amount, active FROM tblvat WHERE vatID = @ID"
        Using cmdSelect As New SqlCommand(selectQuery, connection)
            cmdSelect.Parameters.AddWithValue("@ID", txtvatID.Text)
            connection.Open()
            Dim reader As SqlDataReader = cmdSelect.ExecuteReader()
            If reader.Read() Then
                oldVatAmount = reader("vat_amount").ToString()
                oldActiveStatus = reader("active").ToString()
            End If
            reader.Close()
        End Using
        connection.Close()

        ' Check the previous active status
        If Guna2CheckBox1.Checked Then
            activevat = "True"
        Else
            activevat = "False"
        End If

        If String.IsNullOrWhiteSpace(txtvat.Text) Then
            MessageBox.Show("Input a Value")
            Return
        End If

        connection.Open()
        Using cmdinsert As New SqlCommand("UPDATE tblvat SET vat_amount = @vat, active = @act WHERE vatID = @ID", connection)
            cmdinsert.Parameters.AddWithValue("@vat", txtvat.Text)
            cmdinsert.Parameters.AddWithValue("@act", activevat)
            cmdinsert.Parameters.AddWithValue("@ID", txtvatID.Text)
            cmdinsert.ExecuteNonQuery()
            MessageBox.Show("Updated Successfully")
            Form1.populatevat()
        End Using
        connection.Close()

        ' Construct action message
        Dim actionMessage As String = ""

        ' Check if both VAT amount and active status have been updated
        If txtvat.Text <> oldVatAmount AndAlso activevat <> oldActiveStatus Then
            actionMessage = $"Updated the Vat Amount: '{oldVatAmount}' to '{txtvat.Text}' and the Vat Active Status: '{oldActiveStatus}' to '{activevat}' for the Vat ID: '{txtvatID.Text}'"
        ElseIf txtvat.Text <> oldVatAmount Then
            ' Construct the action message for VAT amount update
            actionMessage = $"Updated the Vat Amount: '{oldVatAmount}' to '{txtvat.Text}' for the Vat ID: '{txtvatID.Text}'"
        ElseIf activevat <> oldActiveStatus Then
            ' Construct the action message for active status update
            actionMessage = $"Updated the Vat Active Status: '{oldActiveStatus}' to '{activevat}' for the Vat ID: '{txtvatID.Text}'"
        End If

        ' Log action
        If actionMessage <> "" Then
            LogAction(actionMessage)
        End If
    End Sub


    Private Sub LogAction(action As String)
        ' Log user action
        Dim userID As Integer = loginform.UserID
        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim timee As String = DateTime.Now.ToString("HH:mm")

        Using cmdinsert As New SqlCommand("INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)", OpenConnection())
            cmdinsert.Parameters.AddWithValue("@uID", userID)
            cmdinsert.Parameters.AddWithValue("@acts", action)
            cmdinsert.Parameters.AddWithValue("@time", timee)
            cmdinsert.Parameters.AddWithValue("@date", datee)
            cmdinsert.ExecuteNonQuery()
            Form1.AuditTrail()
        End Using
    End Sub


    Private Sub ValidatesInputNumber(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[^0-9\s.-]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub
    Private Sub txtvat_TextChanged(sender As Object, e As EventArgs) Handles txtvat.TextChanged
        ValidatesInputNumber(txtvat)
    End Sub


End Class

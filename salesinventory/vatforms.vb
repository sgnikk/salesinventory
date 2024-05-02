Imports System.Data.SqlClient

Public Class vatforms
    Dim connection As New SqlConnection(Module1.connectionStrings)

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles btnclose.Click
        Me.Close()
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        Dim activevat As String
        Dim hasActiveRow As Boolean = False

        ' Check if there's already a row with active status set to 'True'
        Using cmdCheckActive As New SqlCommand("SELECT COUNT(*) FROM tblvat WHERE active = 'True'", connection)
            connection.Open()
            Dim count As Integer = Convert.ToInt32(cmdCheckActive.ExecuteScalar())
            If count > 0 Then
                hasActiveRow = True
            End If
            connection.Close()
        End Using

        ' If there's already an active row and the user is trying to insert 'True' as active, prevent insertion
        If hasActiveRow AndAlso Guna2CheckBox1.Checked Then
            MessageBox.Show("There's already a row with active status set to 'True'.")
            Return
        End If

        ' Set the active status based on checkbox state
        If Guna2CheckBox1.Checked Then
            activevat = "True"
        Else
            activevat = "False"
        End If

        ' Validate input VAT amount
        Dim vatAmount As Decimal
        If Not Decimal.TryParse(txtvat.Text, vatAmount) Then
            MessageBox.Show("Invalid VAT amount. Please input a valid decimal number.")
            Return
        End If

        ' Insert VAT data into database
        connection.Open()
        Using cmdinsert As New SqlCommand("INSERT INTO tblvat (vat_amount, active) VALUES (@vat, @act)", connection)
            cmdinsert.Parameters.AddWithValue("@vat", vatAmount)
            cmdinsert.Parameters.AddWithValue("@act", activevat)
            cmdinsert.ExecuteNonQuery()
            MessageBox.Show("Added Successfully")
            Form1.populatevat()
        End Using
        connection.Close()

        ' Log action: VAT added
        LogAction($"Vat added: '{vatAmount}', Active Status: '{activevat}'")
    End Sub

    Private Sub txtvat_TextChanged(sender As Object, e As EventArgs) Handles txtvat.TextChanged
        ValidatesInputNumber(txtvat)
    End Sub

    Private Sub ValidatesInputNumber(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[^0-9\s.-]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub

    Private Sub LogAction(action As String)
        ' Log user action
        Dim userID As Integer = loginformm.UserID
        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim timee As String = DateTime.Now.ToString("HH:mm")

        Using cmdinsert As New SqlCommand("INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)", connection)
            cmdinsert.Parameters.AddWithValue("@uID", userID)
            cmdinsert.Parameters.AddWithValue("@acts", action)
            cmdinsert.Parameters.AddWithValue("@time", timee)
            cmdinsert.Parameters.AddWithValue("@date", datee)
            cmdinsert.ExecuteNonQuery()
            Form1.AuditTrail()
        End Using
    End Sub

    Private Sub vatforms_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub txtvatID_TextChanged(sender As Object, e As EventArgs) Handles txtvatID.TextChanged

    End Sub
End Class

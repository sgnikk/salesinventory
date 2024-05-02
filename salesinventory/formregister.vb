Imports System.Data.SqlClient

Public Class formregister
    Dim connection As New SqlConnection(Module1.connectionStrings)
    Dim connectionstring As String = Module1.connectionStrings

    Private Sub formregister_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbuserlvl.Items.Add("Owner")
        cmbuserlvl.Items.Add("Staff")
        txtpassword.UseSystemPasswordChar = True
        btnupdate.Visible = False
    End Sub


    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        Dim username As String = txtusername.Text.Trim()
        connection.Open()
        Using cmdCheckUsername As New SqlCommand("SELECT COUNT(*) FROM tblusers WHERE username = @uname", connection)
            cmdCheckUsername.Parameters.AddWithValue("@uname", username)
            Dim count As Integer = Convert.ToInt32(cmdCheckUsername.ExecuteScalar())
            If count > 0 Then
                MessageBox.Show("Username already exists. Please choose a different username.")
                connection.Close()
                Return
            End If
        End Using

        Using cmdinsert As New SqlCommand("INSERT INTO tblusers (name, lastname, userlevel, username, password) VALUES (@namee, @lname, @ulevel, @uname, @pass)", connection)
            cmdinsert.Parameters.AddWithValue("@namee", txtfname.Text)
            cmdinsert.Parameters.AddWithValue("@lname", txtlname.Text)
            cmdinsert.Parameters.AddWithValue("@ulevel", cmbuserlvl.SelectedItem)
            cmdinsert.Parameters.AddWithValue("@uname", username)
            cmdinsert.Parameters.AddWithValue("@pass", txtpassword.Text)
            cmdinsert.ExecuteNonQuery()
            MessageBox.Show("Inserted Successfully")
            Form1.populateusers()
        End Using
        connection.Close()

        LogAction($"User added: Name - '{txtfname.Text}', User Level - '{cmbuserlvl.SelectedItem}', Username - '{username}'")
    End Sub


    Private Sub Guna2CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox1.CheckedChanged
        txtpassword.UseSystemPasswordChar = Not Guna2CheckBox1.Checked
    End Sub

    Private Sub btnupdate_Click(sender As Object, e As EventArgs) Handles btnupdate.Click
        Dim username As String = txtusername.Text.Trim()

        If cmbuserlvl.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a user level.")
            Return
        End If

        connection.Open()

        Dim oldName As String = ""
        Dim oldUserLevel As String = ""
        Dim oldUsername As String = ""
        Dim oldPassword As String = ""

        Using cmdGetOldValues As New SqlCommand("SELECT name, userlevel, username, password FROM tblusers WHERE users_id = @uID", connection)
            cmdGetOldValues.Parameters.AddWithValue("@uID", txtID.Text)
            Dim reader As SqlDataReader = cmdGetOldValues.ExecuteReader()

            If reader.Read() Then
                oldName = reader("name").ToString()
                oldUserLevel = reader("userlevel").ToString()
                oldUsername = reader("username").ToString()
                oldPassword = reader("password").ToString()
            End If

            reader.Close()
        End Using


        Using cmdupdate As New SqlCommand("UPDATE tblusers SET name = @namee, lastname = @lname, userlevel = @ulevel, username = @uname, password = @pass where users_id = @uID", connection)
            cmdupdate.Parameters.AddWithValue("@namee", txtfname.Text)
            cmdupdate.Parameters.AddWithValue("@lname", txtlname.Text)
            cmdupdate.Parameters.AddWithValue("@ulevel", cmbuserlvl.SelectedItem)
            cmdupdate.Parameters.AddWithValue("@uname", username)
            cmdupdate.Parameters.AddWithValue("@pass", txtpassword.Text)
            cmdupdate.Parameters.AddWithValue("@uID", txtID.Text)
            cmdupdate.ExecuteNonQuery()
            Form1.populateusers()
            MessageBox.Show("Updated Successfully")

            Dim userID As Integer = loginformm.UserID
            Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
            Dim timee As String = DateTime.Now.ToString("HH:mm")
            Dim action As String = "User data updated: " &
                               If(oldName <> txtfname.Text, $"Name changed from '{oldName}' to '{txtfname.Text}'", "") &
                               If(oldUserLevel <> cmbuserlvl.SelectedItem.ToString(), $"User level changed from '{oldUserLevel}' to '{cmbuserlvl.SelectedItem.ToString()}'", "") &
                               If(oldUsername <> username, $"Username changed from '{oldUsername}' to '{username}'", "") &
                               If(oldPassword <> txtpassword.Text, $"Password changed", "")
            Using cmdinsert As New SqlCommand("INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)", connection)
                cmdinsert.Parameters.AddWithValue("@uID", userID)
                cmdinsert.Parameters.AddWithValue("@acts", action)
                cmdinsert.Parameters.AddWithValue("@time", timee)
                cmdinsert.Parameters.AddWithValue("@date", datee)
                cmdinsert.ExecuteNonQuery()
                Form1.AuditTrail()
            End Using
        End Using

        connection.Close()
    End Sub



    Public Sub populateblanks()
        Dim queryString As String = "SELECT users_id, name AS 'Name', userlevel AS 'User Level', username AS 'Username', password AS 'Password' FROM tblusers WHERE users_id = @uID"
        Using connection As New SqlConnection(connectionstring)
            Dim command As New SqlCommand(queryString, connection)
            command.Parameters.AddWithValue("@uID", txtID.Text)

            connection.Open()

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    Dim name As String = reader("Name").ToString()
                    Dim username As String = reader("Username").ToString()
                    Dim password As String = reader("Password").ToString()
                    Dim level As String = reader("User Level").ToString()

                    txtfname.Text = name
                    txtusername.Text = username
                    txtpassword.Text = password

                    If level.ToLower() = "owner" Then
                        cmbuserlvl.SelectedItem = "Owner"
                    ElseIf level.ToLower() = "staff" Then
                        cmbuserlvl.SelectedItem = "Staff"
                    End If
                    btnsave.Visible = False
                    btnupdate.Visible = True
                Else
                    MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        End Using
    End Sub

    Private Sub LogAction(action As String)
        Dim userID As Integer = loginformm.UserID
        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim timee As String = DateTime.Now.ToString("HH:mm")
        connection.Open()

        Using cmdinsert As New SqlCommand("INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)", connection)
            cmdinsert.Parameters.AddWithValue("@uID", userID)
            cmdinsert.Parameters.AddWithValue("@acts", action)
            cmdinsert.Parameters.AddWithValue("@time", timee)
            cmdinsert.Parameters.AddWithValue("@date", datee)
            cmdinsert.ExecuteNonQuery()
            Form1.AuditTrail()
        End Using
        connection.Close()

        'Dim loggedInUsername As String = loginformm.LoggedInUsername
        'Dim userID As Integer = loginformm.UserID
        'Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
        'Dim timee As String = DateTime.Now.ToString("HH:mm")
        'Dim action As String = $"{loggedInUsername} Added A New User, UserName: {txtusername.Text}Name :'{txtlname.Text + txtfname.Text}'"
        'Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection)
        '    auditInsertCommand.Parameters.AddWithValue("@uID", userID)
        '    auditInsertCommand.Parameters.AddWithValue("@acts", action)
        '    auditInsertCommand.Parameters.AddWithValue("@time", timee)
        '    auditInsertCommand.Parameters.AddWithValue("@date", datee)
        '    auditInsertCommand.ExecuteNonQuery()
        '    Form1.AuditTrail()
        'End Using

    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Me.Close()
    End Sub

    Public Sub insertusers()
        Dim loggedInUsername As String = loginformm.LoggedInUsername
        Dim userID As Integer = loginformm.UserID
        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim timee As String = DateTime.Now.ToString("HH:mm")
        Dim action As String = $"{loggedInUsername} Added A New User, UserName: {txtusername.Text}Name :'{txtlname.Text + txtfname.Text}'"
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

End Class

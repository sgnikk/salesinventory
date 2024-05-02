Imports System.Data.SqlClient

Public Class itemform

    Dim connection As New SqlConnection(Module1.connectionStrings)
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub
    Private Sub itemform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbpopulatemedtype()
        cmbpopulatecategory()
        cmbcategory.SelectedValue = 0
        cmbmedtype.SelectedValue = 0
    End Sub
    Public Sub cmbpopulatecategory()
        Dim connectionString As String = connectionStrings
        Dim query As String = "SELECT CATEGORY_ID,CATEGORY_DESCRIPTION,CATEGORY_NAME FROM tblcategory"
        Dim adapter As New SqlDataAdapter(query, connectionString)
        Dim datatable As New DataTable
        adapter.Fill(datatable)
        cmbcategory.DataSource = datatable
        cmbcategory.DisplayMember = "CATEGORY_NAME"
        cmbcategory.ValueMember = "CATEGORY_ID"
    End Sub

    Public Sub cmbpopulatemedtype()
        Dim query As String = "SELECT MEDTYPE_ID, MEDICINE_TYPE FROM tblmedtype"
        Dim adapter As New SqlDataAdapter(query, connection)
        Dim medtypetable As New DataTable
        adapter.Fill(medtypetable)
        cmbmedtype.DataSource = medtypetable
        cmbmedtype.DisplayMember = "MEDICINE_TYPE"
        cmbmedtype.ValueMember = "MEDTYPE_ID"
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

    Private Function ProductExists(productName As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM tblitemm WHERE PRODUCT_NAME = @productName"
        Dim count As Integer

        Using connection As New SqlConnection(Module1.connectionStrings)
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@productName", productName)
                connection.Open()
                count = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using
        Return count > 0
    End Function

    Private Function BarCodeExists(productName As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM tblitemm WHERE BARCODE = @bcode"
        Dim count As Integer

        Using connection As New SqlConnection(Module1.connectionStrings)
            Using cmd As New SqlCommand(query, connection)
                cmd.Parameters.AddWithValue("@bcode", productName)
                connection.Open()
                count = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using
        Return count > 0
    End Function
    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        If ProductExists(txtproductname.Text) Then
            MessageBox.Show("Product with the same name already exists. Please enter a different name.")
            Return
        End If

        If BarCodeExists(itembarcode.Text) Then
            MessageBox.Show("Product with the same barcode already exists. Please enter a different barcode.")
            Return
        End If

        Dim discount As String
        If Guna2CheckBox1.Checked Then
            discount = "Discount"
        Else
            discount = "No Discount"
        End If

        Dim Isexpirable As String
        If Guna2CheckBox2.Checked Then
            Isexpirable = "Yes"
        Else
            Isexpirable = "No"
        End If

        connection.Open()
        Using cmdinsert As New SqlCommand("INSERT INTO tblitemm (PRODUCT_NAME,DESCRIPTION,CATEGORY_ID,MEDTYPE_ID,BARCODE,COSTPRICE_BYPIECE,discount,expirable,sellingprice)
        VALUES (@pname,@desc,@cid,@mid,@bcode,@cprice,@discount,@exp,@sprice)", connection)
            cmdinsert.Parameters.AddWithValue("@pname", txtproductname.Text)
            cmdinsert.Parameters.AddWithValue("@desc", txtdescription.Text)
            cmdinsert.Parameters.AddWithValue("@cid", cmbcategory.SelectedValue)
            cmdinsert.Parameters.AddWithValue("@mid", cmbmedtype.SelectedValue)
            cmdinsert.Parameters.AddWithValue("@bcode", itembarcode.Text)
            cmdinsert.Parameters.AddWithValue("@cprice", txtcpricepiece.Text)
            cmdinsert.Parameters.AddWithValue("@discount", discount)
            cmdinsert.Parameters.AddWithValue("@exp", Isexpirable)
            cmdinsert.Parameters.AddWithValue("@sprice", txtsprice.Text)
            cmdinsert.ExecuteNonQuery()
            MessageBox.Show("Added Succesfully")
            Form1.DisplayItem()
            INSERTITEM()
        End Using
        connection.Close()
    End Sub

    Private Sub txtcpricepiece_TextChanged(sender As Object, e As EventArgs) Handles txtcpricepiece.TextChanged
        ValidatesInputNumber(txtcpricepiece)
    End Sub

    Public Sub INSERTITEM()
        Dim loggedInUsername As String = loginformm.LoggedInUsername
        Dim userID As Integer = loginformm.UserID
        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim timee As String = DateTime.Now.ToString("HH:mm")
        Dim action As String = $"{loggedInUsername} Added A New Product, Product: {txtproductname.Text}'"
        connection.Open()
        Dim auditInsertQuery As String = "INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)"
        Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection)
            auditInsertCommand.Parameters.AddWithValue("@uID", userID)
            auditInsertCommand.Parameters.AddWithValue("@acts", action)
            auditInsertCommand.Parameters.AddWithValue("@time", timee)
            auditInsertCommand.Parameters.AddWithValue("@date", datee)
            auditInsertCommand.ExecuteNonQuery()
            Form1.AuditTrail()
        End Using
        connection.Close()
    End Sub

End Class
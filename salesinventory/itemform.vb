Imports System.Data.SqlClient

Public Class itemform

    Dim connection As New SqlConnection("Data Source=DESKTOP-1A0SD84\SQLEXPRESS;Initial Catalog=salesinventory;Integrated Security=True")
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
        Dim connectionString As String = "Data Source=DESKTOP-1A0SD84\SQLEXPRESS;Initial Catalog=salesinventory;Integrated Security=True"
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


    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        Dim discount As String
        If Guna2CheckBox1.Checked Then
            discount = "Discount"
        Else
            discount = "No Discount"
        End If
        connection.Open()
        Using cmdinsert As New SqlCommand("INSERT INTO tblitemm (PRODUCT_NAME,DESCRIPTION,CATEGORY_ID,MEDTYPE_ID,BARCODE,COSTPRICE_BYPIECE,discount)
        VALUES (@pname,@desc,@cid,@mid,@bcode,@cprice,@discount)", connection)
            cmdinsert.Parameters.AddWithValue("@pname", txtproductname.Text)
            cmdinsert.Parameters.AddWithValue("@desc", txtdescription.Text)
            cmdinsert.Parameters.AddWithValue("@cid", cmbcategory.SelectedValue)
            cmdinsert.Parameters.AddWithValue("@mid", cmbmedtype.SelectedValue)
            cmdinsert.Parameters.AddWithValue("@bcode", itembarcode.Text)
            cmdinsert.Parameters.AddWithValue("@cprice", txtcpricepiece.Text)
            cmdinsert.Parameters.AddWithValue("@discount", discount)
            cmdinsert.ExecuteNonQuery()
            MessageBox.Show("Added Succesfully")
            Form1.DisplayItem()
        End Using
        connection.Close()
    End Sub

    Private Sub txtcpricepiece_TextChanged(sender As Object, e As EventArgs) Handles txtcpricepiece.TextChanged
        ValidatesInputNumber(txtcpricepiece)
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
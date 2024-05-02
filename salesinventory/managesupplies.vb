Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class ManageSupplies

    Private ReadOnly connection As New SqlConnection(Module1.connectionStrings)

    Private Sub ManageSupplies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        populatevalues()
        'txtornumber.Text = GenerateORNumber()
        'txtornumber.Enabled = False
        expdate.MinDate = DateTime.Now
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub

    Private Sub txtqty_TextChanged(sender As Object, e As EventArgs) Handles txtqty.TextChanged
        ValidateInput(txtqty)
    End Sub

    Private Sub ValidateInput(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New Regex("[^0-9\s-]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        Try
            Dim qty As Decimal
            Dim price As Decimal

            If Decimal.TryParse(txtqty.Text, qty) AndAlso Decimal.TryParse(txtprice.Text, price) Then
                Dim amount As Decimal = qty * price
                connection.Open()

                Dim query As String = "INSERT INTO tblsupply (SUPPLIER_ID, ITEM_ID, PRICE, EXPIRATION_DATE, AMOUNT, QUANTITY, ornumber) " &
                                      "VALUES (@sID, @iID, @price, @expdate, @amount, @qty, @ornumber); SELECT SCOPE_IDENTITY();"

                Using cmdinsert As New SqlCommand(query, connection)
                    With cmdinsert.Parameters
                        .AddWithValue("@sID", cmbsuppliername.SelectedValue)
                        .AddWithValue("@iID", cmbproduct.SelectedValue)
                        .AddWithValue("@expdate", expdate.Value)
                        .AddWithValue("@qty", qty)
                        .AddWithValue("@price", price)
                        .AddWithValue("@amount", amount)
                        .AddWithValue("@ornumber", txtornumber.Text)
                    End With

                    Dim newSupplyID As Integer = Convert.ToInt32(cmdinsert.ExecuteScalar())
                    If newSupplyID > 0 Then
                        UpdateAndInsertInventory(cmbproduct.SelectedValue, qty)
                        AddTotalAmountRowToDataGridView(gridviewsupplied)
                        MessageBox.Show("Supply added successfully.")
                    Else
                        MessageBox.Show("Error: Could not retrieve the newly inserted supply ID.")
                    End If
                End Using

                connection.Close()
                displaysupplied()
            Else
                MessageBox.Show("Error: Please enter valid numeric values for quantity and price.")
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub UpdateAndInsertInventory(itemID As Integer, quantity As Decimal)
        Dim stockout As Integer = 0
        Using cmdselect As New SqlCommand("SELECT COUNT (*) FROM tblinventory where ITEM_ID = @iID", connection)
            cmdselect.Parameters.AddWithValue("@iID", itemID)
            If cmdselect.ExecuteScalar() > 0 Then
                Using cmdupdateinventory As New SqlCommand("UPDATE tblinventory SET STOCK_IN = STOCK_IN + @sIN, STOCK_AVAILABLE = STOCK_AVAILABLE + @sAVAIL WHERE ITEM_ID = @iID", connection)
                    cmdupdateinventory.Parameters.AddWithValue("@sIN", quantity)
                    cmdupdateinventory.Parameters.AddWithValue("@sAVAIL", quantity)
                    cmdupdateinventory.Parameters.AddWithValue("@iID", itemID)
                    cmdupdateinventory.ExecuteNonQuery()
                End Using
            Else
                Using cmdinventoryinsert As New SqlCommand("INSERT INTO tblinventory(ITEM_ID,STOCK_IN,STOCK_OUT,STOCK_AVAILABLE) VALUES (@itemID,@sIN,@sOUT,@sAVAIL)", connection)
                    cmdinventoryinsert.Parameters.AddWithValue("@itemID", itemID)
                    cmdinventoryinsert.Parameters.AddWithValue("@sOUT", stockout)
                    cmdinventoryinsert.Parameters.AddWithValue("@sIN", quantity)
                    cmdinventoryinsert.Parameters.AddWithValue("@sAVAIL", quantity)
                    cmdinventoryinsert.ExecuteNonQuery()
                    MessageBox.Show("Inventory record inserted successfully.")
                End Using
            End If
        End Using
    End Sub

    Public Sub displaysupplied()
        Dim querysupply As String = "
        SELECT 
            s.SUPPLIER_NAME AS 'Supplier Name',
            ts.SUPPLY_ID, 
            ts.PRICE AS 'Price',
            ts.EXPIRATION_DATE AS 'Expiration Date',
            ts.QUANTITY AS 'Quantity',
            ts.AMOUNT AS 'Amount',
            ts.ornumber AS 'OR Number'
        FROM 
            tblsupply ts
        LEFT JOIN 
            tblsupplier s ON ts.SUPPLIER_ID = s.SUPPLIER_ID
        LEFT JOIN
            tblitemm i ON ts.ITEM_ID = i.ITEM_ID"

        Dim adsupplied As New SqlDataAdapter(querysupply, connection)
        Dim suppliedtable As New DataTable
        adsupplied.Fill(suppliedtable)

        ' Bind the filtered DataTable to the DataGridView
        gridviewsupplied.DataSource = suppliedtable
    End Sub

    Private Sub FilterOldSupplies(dataTable As DataTable)
        If dataTable.Rows.Count > 0 Then
            ' Get the newest supply ID
            Dim newestSupplyID As Integer = Convert.ToInt32(dataTable.Rows(0)("SUPPLY_ID"))
            ' Remove rows with supply IDs not equal to the newest supply ID
            For i As Integer = dataTable.Rows.Count - 1 To 0 Step -1
                Dim supplyID As Integer = Convert.ToInt32(dataTable.Rows(i)("SUPPLY_ID"))
                If supplyID <> newestSupplyID Then
                    dataTable.Rows.RemoveAt(i)
                End If
            Next
        End If
    End Sub

    Private Sub AddTotalAmountRowToDataGridView(dataGridView As DataGridView)
        Dim table As DataTable = TryCast(dataGridView.DataSource, DataTable)
        If table IsNot Nothing Then
            dataGridView.Columns("Supplier Name").Visible = True
            Dim totalAmount As Decimal = 0
            For Each row As DataRow In table.Rows
                totalAmount += Convert.ToDecimal(row("Amount"))
            Next
            Dim totalRow As DataRow = table.NewRow()
            totalRow("Supplier Name") = "Total Amount"
            totalRow("Amount") = totalAmount
            table.Rows.Add(totalRow)
            dataGridView.DataSource = table
        End If
    End Sub

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        For Each row As DataGridViewRow In gridviewsupplied.Rows
            If row.Cells("SUPPLY_ID").Value IsNot DBNull.Value Then
                Dim supplyId As Integer = Convert.ToInt32(row.Cells("SUPPLY_ID").Value)
                Dim supplierName As String = row.Cells("Supplier Name").Value.ToString()
                Dim supplierID As Integer = GetSupplierID(supplierName)
                If supplierID <> -1 Then
                    InsertData(supplyId, supplierID)
                Else
                    MessageBox.Show("Error: Supplier ID not found.")
                End If
            End If
        Next
        MessageBox.Show("Data saved successfully.")
        gridviewsupplied.DataSource = Nothing
        displaysupplied()
        Form1.Displaysupplies()
    End Sub

    Private Function GetSupplierID(supplierName As String) As Integer
        Dim supplierID As Integer = -1
        Dim query As String = "SELECT SUPPLIER_ID FROM tblsupplier WHERE SUPPLIER_NAME = @SupplierName"
        Using connection As New SqlConnection(Module1.connectionStrings)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@SupplierName", supplierName)
                connection.Open()
                Dim result As Object = command.ExecuteScalar()
                If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                    supplierID = Convert.ToInt32(result)
                End If
            End Using
        End Using
        Return supplierID
    End Function

    Private Sub InsertData(supplyId As Integer, supplierID As Integer)
        Dim query As String = "INSERT INTO tblsupplied (SUPPLY_ID, DATE_SUPPLIED,SUPPLIER_ID) VALUES (@SupplyId, @DateSupplied, @SupplierID)"
        Using connection As New SqlConnection(Module1.connectionStrings)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@SupplyId", supplyId)
                command.Parameters.AddWithValue("@DateSupplied", DateTime.Now)
                command.Parameters.AddWithValue("@SupplierID", supplierID)
                connection.Open()
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub cmbproduct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbproduct.SelectedIndexChanged
        Dim selectedRow As DataRowView = TryCast(cmbproduct.SelectedItem, DataRowView)
        If selectedRow IsNot Nothing Then
            Dim costPriceByPiece As Decimal = Convert.ToDecimal(selectedRow("COSTPRICE_BYPIECE"))
            txtprice.Text = costPriceByPiece.ToString()
        End If
    End Sub

    'Public Shared Function GenerateORNumber() As String
    '    Dim currentDate As Date = Date.Now
    '    Dim formattedDate As String = currentDate.ToString("yyyyMMddHHmmss")
    '    Dim randomNumber As Integer = New Random().Next(100, 999)
    '    Dim orNumber As String = "OR" & formattedDate & randomNumber.ToString()
    '    Return orNumber
    'End Function

    Public Sub populatevalues()
        Dim supquery As String = "SELECT SUPPLIER_ID,SUPPLIER_NAME,ADDRESS,CONTACT FROM tblsupplier"
        Dim supadapter As New SqlDataAdapter(supquery, connection)
        Dim suptable As New DataTable
        supadapter.Fill(suptable)
        cmbsuppliername.DataSource = suptable
        cmbsuppliername.DisplayMember = "SUPPLIER_NAME"
        cmbsuppliername.ValueMember = "SUPPLIER_ID"

        Dim productquery As String = "SELECT ITEM_ID,PRODUCT_NAME,DESCRIPTION,CATEGORY_ID,MEDTYPE_ID,BARCODE,COSTPRICE_BYPIECE FROM tblitemm"
        Dim productadapter As New SqlDataAdapter(productquery, connection)
        Dim producttable As New DataTable
        productadapter.Fill(producttable)
        cmbproduct.DataSource = producttable
        cmbproduct.DisplayMember = "PRODUCT_NAME"
        cmbproduct.ValueMember = "ITEM_ID"
    End Sub

    Private Sub Guna2TextBox2_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox2.TextChanged
        Dim maxLength As Integer = 13
        Dim textBox As Guna.UI2.WinForms.Guna2TextBox = DirectCast(sender, Guna.UI2.WinForms.Guna2TextBox)

        If textBox.TextLength > maxLength Then
            textBox.Text = textBox.Text.Substring(0, maxLength) ' Trim the text to 13 characters
        ElseIf textBox.TextLength < maxLength Then
            textBox.Text = textBox.Text.PadRight(maxLength) ' Pad the text with spaces to make it 13 characters long
        End If
    End Sub

End Class
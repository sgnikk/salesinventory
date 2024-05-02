Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class managesupplies

    Dim connection As New SqlConnection(Module1.connectionStrings)


    Private Sub managesupplies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        populatevalues()
        cmbproduct.SelectedValue = 0
        cmbsuppliername.SelectedValue = 0
        txtornumber.Text = GenerateORNumber()
        txtornumber.Enabled = False
    End Sub
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub
    Private Sub expdate_ValueChanged(sender As Object, e As EventArgs) Handles expdate.ValueChanged
        expdate.MinDate = DateTime.Now
    End Sub
    Private Sub ValidatesInput(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[^0-9\s-]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub
    Private Sub txtbypack_TextChanged(sender As Object, e As EventArgs) Handles txtqty.TextChanged
        ValidatesInput(txtqty)
    End Sub
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
    Dim newSupplyIDs As New List(Of Integer)
    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        Try
            Dim amount As Decimal = Convert.ToDecimal(txtqty.Text) * Convert.ToDecimal(txtprice.Text)

            connection.Open()

            Dim query As String = "INSERT INTO tblsupply (SUPPLIER_ID, ITEM_ID, PRICE, EXPIRATION_DATE, AMOUNT, QUANTITY, ornumber) " &
                              "VALUES (@sID, @iID, @price, @expdate, @amount, @qty, @ornumber); SELECT SCOPE_IDENTITY();"

            Using cmdinsert As New SqlCommand(query, connection)
                cmdinsert.Parameters.AddWithValue("@sID", Convert.ToInt32(cmbsuppliername.SelectedValue))
                cmdinsert.Parameters.AddWithValue("@iID", Convert.ToInt32(cmbproduct.SelectedValue))
                cmdinsert.Parameters.AddWithValue("@expdate", expdate.Value)
                cmdinsert.Parameters.AddWithValue("@qty", Convert.ToInt32(txtqty.Text))
                cmdinsert.Parameters.AddWithValue("@price", Convert.ToDecimal(txtprice.Text))
                cmdinsert.Parameters.AddWithValue("@amount", amount)
                cmdinsert.Parameters.AddWithValue("@ornumber", txtornumber.Text)

                Dim newSupplyID As Integer = Convert.ToInt32(cmdinsert.ExecuteScalar())
                If newSupplyID > 0 Then
                    newSupplyIDs.Add(newSupplyID)
                    updateandinsertinventory(cmbproduct.SelectedValue)
                    AddTotalAmountRowToDataGridView(gridviewsupplied)
                Else
                    MessageBox.Show("Error: Could not retrieve the newly inserted supply ID.")
                End If
            End Using

            connection.Close()
            displaysupplied()
            AddTotalAmountRowToDataGridView(gridviewsupplied)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Public Sub updateandinsertinventory(itemID As Integer)
        Dim stockout As Integer = 0
        Using cmdselect As New SqlCommand("SELECT COUNT (*) FROM tblinventory where ITEM_ID = @iID", connection)
            cmdselect.Parameters.AddWithValue("@iID", itemID)
            If cmdselect.ExecuteScalar() > 0 Then
                Using cmdupdateinventory As New SqlCommand("UPDATE tblinventory set STOCK_IN = STOCK_IN + @sIN,
                STOCK_AVAILABLE = STOCK_AVAILABLE + @sAVAIL WHERE ITEM_ID = @iID", connection)
                    cmdupdateinventory.Parameters.AddWithValue("@sIN", txtqty.Text)
                    cmdupdateinventory.Parameters.AddWithValue("@sAVAIL", txtqty.Text)
                    cmdupdateinventory.Parameters.AddWithValue("@iID", itemID)
                    cmdupdateinventory.ExecuteNonQuery()
                End Using
            Else
                Using cmdinventoryinsert As New SqlCommand("INSERT INTO tblinventory(ITEM_ID,STOCK_IN,STOCK_OUT,STOCK_AVAILABLE) 
                    VALUES (@itemID,@sIN,@sOUT,@sAVAIL)", connection)
                    cmdinventoryinsert.Parameters.AddWithValue("@itemID", cmbproduct.SelectedValue)
                    cmdinventoryinsert.Parameters.AddWithValue("@sOUT", stockout)
                    cmdinventoryinsert.Parameters.AddWithValue("@sIN", txtqty.Text)
                    cmdinventoryinsert.Parameters.AddWithValue("@sAVAIL", txtqty.Text)
                    cmdinventoryinsert.ExecuteNonQuery()
                    MessageBox.Show("Insert Successfully")
                End Using
            End If
        End Using
    End Sub

    Public Sub displaysupplied()
        Dim suppliedtable As New DataTable
        Dim querysupply As String = "
        SELECT 
            ts.SUPPLY_ID, 
            ISNULL(s.SUPPLIER_NAME, 'NONE') AS 'Supplier Name',
            ISNULL(i.PRODUCT_NAME, 'NONE') AS 'Product Name',
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
            tblitemm i ON ts.ITEM_ID = i.ITEM_ID;"
        Dim filterExpression As String = ""
        If newSupplyIDs.Count > 0 Then
            filterExpression = String.Format("SUPPLY_ID IN ({0})", String.Join(",", newSupplyIDs))
        Else
            filterExpression = "1=0"
        End If

        Dim adsupplied As New SqlDataAdapter(querysupply, connection)
        adsupplied.Fill(suppliedtable)

        Dim dv As New DataView(suppliedtable)
        dv.RowFilter = filterExpression

        gridviewsupplied.DataSource = dv.ToTable()
        gridviewsupplied.Columns("SUPPLY_ID").Visible = False
        gridviewsupplied.Columns("OR Number").Visible = False
        gridviewsupplied.Columns("Expiration Date").DisplayIndex = 3
    End Sub

    Public Sub AddTotalAmountRowToDataGridView(dataGridView As DataGridView)
        ' Get the DataTable bound to the DataGridView
        Dim table As DataTable = TryCast(dataGridView.DataSource, DataTable)
        If table IsNot Nothing Then
            ' Show the "Supplier Name" column
            dataGridView.Columns("Supplier Name").Visible = True

            ' Calculate total amount
            Dim totalAmount As Decimal = 0
            For Each row As DataRow In table.Rows
                totalAmount += Convert.ToDecimal(row("Amount"))
            Next

            ' Add extra row for the total amount
            Dim totalRow As DataRow = table.NewRow()
            totalRow("Supplier Name") = "Total Amount"
            totalRow("Amount") = totalAmount
            table.Rows.Add(totalRow)

            ' Bind the updated DataTable to the DataGridView
            dataGridView.DataSource = table
        End If
    End Sub

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        For Each row As DataGridViewRow In gridviewsupplied.Rows
            If row.Cells("SUPPLY_ID").Value IsNot DBNull.Value Then
                Dim supplyId As Integer = Convert.ToInt32(row.Cells("SUPPLY_ID").Value)
                Dim supplierName As String = row.Cells("Supplier Name").Value.ToString()

                Dim supplierID As Integer = GetSupplierID(connectionStrings, supplierName)
                If supplierID <> -1 Then
                    InsertData(connectionStrings, supplyId, supplierID)
                Else
                    MessageBox.Show("Error: Supplier ID not found.")
                End If
            End If
        Next
        MessageBox.Show("Data saved successfully.")
        gridviewsupplied.DataSource = Nothing
        newSupplyIDs.Clear() ' Clear the list of newly added supply IDs
        Form1.Displaysupplies()
        Form1.populateinventory()

        displaysupplied()
    End Sub


    Private Function GetSupplierID(connectionString As String, supplierName As String) As Integer
        Dim supplierID As Integer = -1 ' Default value if not found
        Dim query As String = "SELECT SUPPLIER_ID FROM tblsupplier WHERE SUPPLIER_NAME = @SupplierName"

        Using connection As New SqlConnection(connectionString)
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

    Private Sub InsertData(connectionString As String, supplyId As Integer, supplierName As String)
        Dim query As String = "INSERT INTO tblsupplied (SUPPLY_ID, DATE_SUPPLIED,SUPPLIER_ID) VALUES (@SupplyId, @DateSupplied, @ssID)"
        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@SupplyId", supplyId)
                command.Parameters.AddWithValue("@DateSupplied", DateTime.Now)
                command.Parameters.AddWithValue("@ssID", supplierName)
                connection.Open()
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub cmbproduct_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedRow As DataRowView = DirectCast(cmbproduct.SelectedItem, DataRowView)
        If selectedRow IsNot Nothing Then
            Dim selectedProductId As Integer = Convert.ToInt32(selectedRow("ITEM_ID"))
            Dim selectedProductTable As DataTable = DirectCast(cmbproduct.DataSource, DataTable)
            Dim selectedProductDataRow As DataRow = selectedProductTable.Select($"ITEM_ID = {selectedProductId}").FirstOrDefault()
            If selectedProductDataRow IsNot Nothing Then
                Dim costPriceByPiece As Decimal = Convert.ToDecimal(selectedProductDataRow("COSTPRICE_BYPIECE"))
                txtprice.Text = costPriceByPiece.ToString
            End If
        End If
    End Sub

    Public Shared Function GenerateORNumber() As String
        Dim currentDate As Date = Date.Now
        Dim formattedDate As String = currentDate.ToString("yyyyMMddHHmmss")
        Dim randomNumber As Integer = New Random().Next(100, 999) ' Generate a random 3-digit number

        ' Concatenate the date and random number to form the OR number
        Dim orNumber As String = "OR" & formattedDate & randomNumber.ToString()

        Return orNumber
    End Function

End Class
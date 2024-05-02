Imports System.Data.SqlClient

Public Class salesform
    Dim connection As New SqlConnection(Module1.connectionStrings)
    Dim connectionstring As String = Module1.connectionStrings
    Dim newsaleIDs As New List(Of Integer)
    Dim transactionID As Integer = Nothing

    Private ReadOnly productsDiscountStatus As New Dictionary(Of String, Boolean)()

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub

    Private Sub salesform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateProduct()
        populatediscount()
        cmbdiscount.SelectedValue = 0
        cmbproductname.SelectedValue = 0
        cmbcustomername.Text = GetUniqueCustomerName()
        RadioButton1.Checked = True
    End Sub

    Public Sub populatediscount()
        Dim discountquery As String = "SELECT discount_id, CONCAT(discount_name, '(', discount_amount, '%)') AS display_name FROM tbldiscount"
        Using connection As New SqlConnection(connectionStrings)
            Dim discountadapter As New SqlDataAdapter(discountquery, connection)
            Dim discounttable As New DataTable
            discountadapter.Fill(discounttable)
            cmbdiscount.DataSource = discounttable
            cmbdiscount.DisplayMember = "display_name"
            cmbdiscount.ValueMember = "discount_id"
        End Using
    End Sub

    Private Sub cmbproductName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbproductname.SelectedIndexChanged
        cmbdiscount.Enabled = productsDiscountStatus.ContainsKey(cmbproductname.Text) AndAlso productsDiscountStatus(cmbproductname.Text)
        If Not cmbdiscount.Enabled Then
            cmbdiscount.SelectedValue = 0
        End If
    End Sub


    Private Sub SearchByBarcode(barcode As String)
        If String.IsNullOrEmpty(barcode) Then
            Return
        End If

        Dim productName As String = ""

        Dim query As String = "
    SELECT 
        i.PRODUCT_NAME
    FROM 
        tblinventory AS iv
    INNER JOIN 
        tblitemm AS i 
    ON 
        iv.ITEM_ID = i.ITEM_ID
    WHERE 
        i.BARCODE = @barcode"

        Using connection As New SqlConnection(connectionstring)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@barcode", barcode)

                Try
                    connection.Open()
                    Dim result As Object = command.ExecuteScalar()

                    If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                        productName = Convert.ToString(result)
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error searching for product by barcode: " & ex.Message)
                End Try
            End Using
        End Using

        If Not String.IsNullOrEmpty(productName) Then
            Dim index As Integer = cmbproductname.FindStringExact(productName)

            If index <> -1 Then
                cmbproductname.SelectedIndex = index
            End If
        End If
    End Sub


    Private Sub txtsrchbar_TextChanged(sender As Object, e As EventArgs) Handles txtsrchbar.TextChanged
        If txtsrchbar.Text.Length > 13 Then
            txtsrchbar.Text = txtsrchbar.Text.Substring(0, 13)
        End If

        If txtsrchbar.Text.Length = 13 Then
            SearchByBarcode(txtsrchbar.Text)
        Else
            PopulateProduct()
            cmbproductname.SelectedValue = 0
        End If
    End Sub
    Private Function GetDiscountAmount(discountId As Integer) As Decimal
        Dim discountAmount As Decimal = 0
        Dim query As String = "SELECT discount_amount FROM tbldiscount WHERE discount_id = @discountId"
        Using connection As New SqlConnection(connectionstring)

            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@discountId", discountId)
                connection.Open()

                Dim result As Object = command.ExecuteScalar()

                If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                    discountAmount = Convert.ToDecimal(result)
                End If
            End Using
        End Using
        Return discountAmount
    End Function


    Private Function IsItemIdValid(itemId As Integer) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM tblitemm WHERE ITEM_ID = @itemId"
        Using command As New SqlCommand(query, connection)
            command.Parameters.AddWithValue("@itemId", itemId)
            connection.Open()
            Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
            connection.Close()
            Return count > 0
        End Using
    End Function

    Private Function GetAvailableStock(itemId As Integer) As Tuple(Of Integer, String)
        Dim availableStock As Integer = 0
        Dim productName As String = ""
        Using connection As New SqlConnection(connectionstring)
            Using cmd As New SqlCommand("SELECT STOCK_AVAILABLE, PRODUCT_NAME FROM tblinventory iv INNER JOIN tblitemm i ON iv.ITEM_ID = i.ITEM_ID WHERE iv.ITEM_ID = @itemId", connection)
                cmd.Parameters.AddWithValue("@itemId", itemId)
                connection.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        availableStock = Convert.ToInt32(reader("STOCK_AVAILABLE"))
                        productName = reader("PRODUCT_NAME").ToString()
                    End If
                End Using
            End Using
        End Using
        Return New Tuple(Of Integer, String)(availableStock, productName)
    End Function

    Private Sub LogSaleChanges(customerName As String, soldProducts As List(Of String), totalAmount As Decimal)
        Dim userID As Integer = loginformm.UserID
        Dim action As String = $"Sold Products For '{customerName}' Product: {String.Join(", ", soldProducts)} The Total Amount is: {totalAmount}"
        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
        Dim timee As String = DateTime.Now.ToString("HH:mm")

        Using conn As New SqlConnection(Module1.connectionStrings)
            Using cmdinsert As New SqlCommand("INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)", conn)
                cmdinsert.Parameters.AddWithValue("@uID", userID)
                cmdinsert.Parameters.AddWithValue("@acts", action)
                cmdinsert.Parameters.AddWithValue("@time", timee)
                cmdinsert.Parameters.AddWithValue("@date", datee)
                Try
                    conn.Open()
                    cmdinsert.ExecuteNonQuery()
                    Form1.AuditTrail()
                Catch ex As Exception
                    MessageBox.Show("Error executing audit trail query: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Public Function GetVAT() As Decimal
        Dim vatRate As Decimal = 0
        Dim query As String = "SELECT vat_amount FROM tblvat WHERE active = 'True'"
        Using connection As New SqlConnection(connectionstring)
            Using command As New SqlCommand(query, connection)
                connection.Open()
                Dim result As Object = command.ExecuteScalar()
                If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                    vatRate = Convert.ToDecimal(result)
                End If
            End Using
        End Using
        Return vatRate
    End Function

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click

        cmbproductname.SelectedValue = 0
        txtqty.Text = ""
        RadioButton1.Checked = False
        txtsrchbar.Text = ""

        If gridviewsale.Rows.Count > 0 Then
            gridviewsale.Rows.RemoveAt(gridviewsale.Rows.Count - 1)
        End If
    End Sub
    Private Sub ValidateInputLetters(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[0-9]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub
    Private Sub ValidatesInputNumber(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Dim regex As New System.Text.RegularExpressions.Regex("[^0-9\s-]")
        textBox.Text = regex.Replace(textBox.Text, "")
        textBox.SelectionStart = textBox.Text.Length
    End Sub
    Private Sub txtqty_TextChanged(sender As Object, e As EventArgs) Handles txtqty.TextChanged
        ValidatesInputNumber(txtqty)
    End Sub


    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            cmbcustomername.Enabled = False
            cmbcustomername.Text = GetUniqueCustomerName()
        Else
            cmbcustomername.Enabled = True ' Disable the ComboBox when RadioButton1 is checked
        End If
    End Sub

    Private Function GetUniqueCustomerName() As String
        Dim connectionString As String = Module1.connectionStrings
        Dim uniqueName As String = "Guests"

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim query As String = "SELECT customer_name FROM tblsold WHERE customer_name LIKE 'Guests%'"

            Using command As New SqlCommand(query, connection)
                Using reader As SqlDataReader = command.ExecuteReader()
                    Dim existingNames As New List(Of String)()

                    While reader.Read()
                        existingNames.Add(reader("customer_name").ToString())
                    End While
                    If existingNames.Count > 0 Then
                        Dim maxNumber As Integer = 0

                        For Each name As String In existingNames
                            Dim numberString As String = name.Substring("Guests".Length)

                            Dim number As Integer
                            If Integer.TryParse(numberString, number) Then
                                If number > maxNumber Then
                                    maxNumber = number
                                End If
                            End If
                        Next

                        uniqueName = "Guests" & (maxNumber + 1).ToString()
                    End If
                End Using
            End Using
        End Using
        Return uniqueName
    End Function
    Private Sub cmbcustomername_Leave(sender As Object, e As EventArgs)
        If cmbcustomername.Text.Trim().Equals("Guests", StringComparison.OrdinalIgnoreCase) Then
            cmbcustomername.Text = GetUniqueCustomerName()
        End If
    End Sub

    Private Shared ReadOnly rng As New Random()

    Dim transactionNumber As Integer = GetTransactionNumber()

    Public Function GetTransactionNumber() As Integer
        Dim transactionNumber As Integer = rng.Next(10000000, 99999999)

        Return transactionNumber
    End Function


    Public Sub PopulateProduct()
        Dim productAdapter As New SqlDataAdapter("SELECT i.PRODUCT_NAME,i.PRODUCT_NAME, i.DISCOUNT, i.sellingprice , iv.ITEM_ID, iv.STOCK_IN, iv.STOCK_OUT, iv.STOCK_AVAILABLE " &
                                                 "FROM tblinventory AS iv " & "INNER JOIN tblitemm AS i ON iv.ITEM_ID = i.ITEM_ID", connection)
        Dim productTable As New DataTable
        productAdapter.Fill(productTable)

        cmbproductname.DataSource = productTable
        cmbproductname.DisplayMember = "PRODUCT_NAME"
        cmbproductname.ValueMember = "ITEM_ID"
        For Each row As DataRow In productTable.Rows
            productsDiscountStatus(row("PRODUCT_NAME").ToString()) = If(row("DISCOUNT") IsNot DBNull.Value AndAlso row("DISCOUNT").ToString().Trim().ToUpper() = "DISCOUNT", True, False)
        Next

        cmbproductName_SelectedIndexChanged(Nothing, EventArgs.Empty)
    End Sub

    Private Function TransactionExists(transactionId As Integer) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM tblsold WHERE sold_id = @transactionId"
        Using connection As New SqlConnection(connectionstring)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@transactionId", transactionId)
                connection.Open()
                Dim count As Integer = Convert.ToInt32(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Function GetSellingPrice(itemId As Integer) As Decimal
        Dim sellingPrice As Decimal = 0
        Dim query As String = "SELECT sellingprice FROM tblitemm WHERE ITEM_ID = @itemId"
        Using cmd As New SqlCommand(query, connection)
            cmd.Parameters.AddWithValue("@itemId", itemId)
            connection.Open()
            Dim result As Object = cmd.ExecuteScalar()
            If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                sellingPrice = Convert.ToDecimal(result)
            End If
            connection.Close()
        End Using
        Return sellingPrice
    End Function

    Private Function GetNewestSoldId() As Integer
        connection.Open()
        Dim soldId As Integer
        Using cmd As New SqlCommand("SELECT TOP 1 sold_id FROM tblsold ORDER BY sold_id DESC", connection)
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            If reader.Read() Then
                soldId = Convert.ToInt32(reader("sold_id"))
            End If
            reader.Close()
        End Using
        connection.Close()
        Return soldId
    End Function

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        Try
            connection.Open()

            Dim stockTuple As Tuple(Of Integer, String) = GetAvailableStock(cmbproductname.SelectedValue)

            ' Extract the available stock value from the tuple
            Dim availableStock As Integer = stockTuple.Item1



            ' Check if there's enough stock available
            If txtqty.Text > availableStock Then
                MsgBox("Insufficient stock available.")
                Return
            End If

            'Dim remainingStock As Integer = availableStock - txtqty.Text

            '' Update STOCK_AVAILABLE and STOCK_OUT in tblinventory
            'Using cmdUpdate As New SqlCommand("UPDATE tblinventory SET STOCK_AVAILABLE = @RemainingStock, STOCK_OUT = STOCK_OUT + @QuantityToSell WHERE ITEM_ID = @ItemId", connection)
            '    cmdUpdate.Parameters.AddWithValue("@RemainingStock", remainingStock)
            '    cmdUpdate.Parameters.AddWithValue("@QuantityToSell", txtqty.Text)
            '    cmdUpdate.Parameters.AddWithValue("@ItemId", cmbproductname.SelectedValue)
            '    cmdUpdate.ExecuteNonQuery()
            'End Using

            Dim sellingPrice As Decimal
            Dim discountAmount As Decimal = 0

            Using cmd As New SqlCommand("SELECT sellingprice FROM tblitemm WHERE ITEM_ID = @itemId", connection)
                cmd.Parameters.AddWithValue("@itemId", cmbproductname.SelectedValue)
                sellingPrice = Convert.ToDecimal(cmd.ExecuteScalar())
            End Using

            Dim discountId As Integer = Convert.ToInt32(cmbdiscount.SelectedValue)
            If discountId <> 0 Then
                discountAmount = GetDiscountAmount(discountId)
            End If

            ' Calculate the discount amount
            Dim discountValue As Decimal = sellingPrice * (discountAmount / 100)

            Dim discountedPrice As Decimal = sellingPrice - discountValue

            Dim totalAmount As Decimal = discountedPrice * txtqty.Text

            gridviewsale.Rows.Add(cmbproductname.SelectedValue, cmbproductname.Text, discountedPrice, txtqty.Text, totalAmount)

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub





    Dim currentDate As DateTime = DateTime.Now
    Dim dateString As String = currentDate.ToString("yyyy-MM-dd")


    Public Sub InsertSold()

        connection.Open()

        Using cmdinsertsold As New SqlCommand("INSERT INTO tblsold (customer_name) 
                                              OUTPUT INSERTED.transactionID 
                                              VALUES (@cname)", connection)
            cmdinsertsold.Parameters.AddWithValue("@cname", cmbproductname.Text)
            transactionID = Convert.ToInt32(cmdinsertsold.ExecuteScalar)
        End Using
        connection.Close()
    End Sub

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        Try
            Dim totalamount As Integer
            For Each row As DataGridViewRow In gridviewsale.Rows
                Dim amount As Integer = row.Cells(4).Value
                totalamount += amount
            Next

            connection.Open()

            Using cmd As New SqlCommand("Insert into tblsold(transactionDate,customer_name,totalamount) VALUES (@transactiondate,@customer_name,@totalamount)", connection)
                cmd.Parameters.AddWithValue("@transactionDate", dateString)
                cmd.Parameters.AddWithValue("@customer_name", If(String.IsNullOrWhiteSpace(cmbcustomername.Text), "Guest", cmbcustomername.Text))
                cmd.Parameters.AddWithValue("@totalamount", totalamount)
                cmd.ExecuteNonQuery()
                cmd.Parameters.Clear()
            End Using

            Dim lastinsertedid As Integer
            Using cmd As New SqlCommand("SELECT TOP(1) transactionID FROM tblsold ORDER BY transactionID DESC", connection)
                lastinsertedid = Convert.ToInt32(cmd.ExecuteScalar())
            End Using

            For Each row As DataGridViewRow In gridviewsale.Rows
                Dim itemId As Integer = Convert.ToInt32(row.Cells(0).Value)
                Dim quantity As Integer = Convert.ToInt32(row.Cells(2).Value)
                Dim availableStock As Integer
                Using cmd As New SqlCommand("SELECT STOCK_AVAILABLE FROM tblinventory WHERE ITEM_ID = @ITEM_ID", connection)
                    cmd.Parameters.AddWithValue("@ITEM_ID", itemId)
                    availableStock = Convert.ToInt32(cmd.ExecuteScalar())
                End Using
                If quantity > availableStock Then
                    MsgBox($"Insufficient stock for {row.Cells(1).Value}. Please check and try again.")
                    Return
                End If
            Next



            For Each row As DataGridViewRow In gridviewsale.Rows
                Using cmd As New SqlCommand("INSERT INTO tbsale(transactionID,ITEM_ID,Quantity,amount,price) VALUES (@transactionID,@ITEM_ID,@Quantity,@amount,@price)", connection)
                    cmd.Parameters.AddWithValue("@transactionID", lastinsertedid)
                    cmd.Parameters.AddWithValue("@ITEM_ID", row.Cells(0).Value)
                    cmd.Parameters.AddWithValue("@Quantity", row.Cells(3).Value)
                    cmd.Parameters.AddWithValue("@amount", row.Cells(4).Value)
                    cmd.Parameters.AddWithValue("@price", row.Cells(2).Value)
                    cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                End Using
            Next

            For Each row As DataGridViewRow In gridviewsale.Rows
                Using cmd As New SqlCommand("UPDATE tblinventory SET STOCK_AVAILABLE = STOCK_AVAILABLE - @Quantity, STOCK_OUT = STOCK_OUT + @sout  WHERE ITEM_ID = @ITEM_ID", connection)
                    cmd.Parameters.AddWithValue("@Quantity", row.Cells(3).Value)
                    cmd.Parameters.AddWithValue("@sout", txtqty.Text) ' Use the quantity from the DataGridView
                    cmd.Parameters.AddWithValue("@ITEM_ID", row.Cells(0).Value)
                    cmd.ExecuteNonQuery()
                End Using
            Next

            MsgBox("Saved")

            ' Displaying total amount in payment form
            paymentform.txtotalamount.Text = totalamount
            paymentform.Show()

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        cmbdiscount.SelectedValue = 0
        cmbdiscount.Enabled = False
    End Sub
End Class


Imports System.Data.SqlClient

Public Class paymentform
    Private oldValues As New List(Of String)() ' List to store old values


    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        Try
            ' Clear existing rows
            gridviewpayment.Rows.Clear()

            ' Define columns
            gridviewpayment.Columns.Clear()
            gridviewpayment.Columns.Add("PRODUCT_NAME", "Product Name")
            gridviewpayment.Columns.Add("Quantity", "Quantity")
            gridviewpayment.Columns.Add("Price", "Price")
            gridviewpayment.Columns.Add("TotalAmount", "Total Amount")

            Dim query As String = "SELECT i.PRODUCT_NAME, s.Quantity, i.sellingprice AS Price, ts.totalamount AS TotalAmount " &
                                  "FROM tbsale s " &
                                  "INNER JOIN tblitemm i ON s.ITEM_ID = i.ITEM_ID " &
                                  "INNER JOIN (SELECT transactionID, totalamount FROM tblsold) ts ON s.transactionID = ts.transactionID"

            Dim totalAmount As Decimal = 0
            Dim totalChange As Decimal = 0 ' Total change for the entire purchase

            Using connection As New SqlConnection(connectionStrings)
                Using command As New SqlCommand(query, connection)
                    connection.Open()
                    Using reader As SqlDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim productName As String = reader("PRODUCT_NAME").ToString()
                            Dim quantity As Integer = Convert.ToInt32(reader("Quantity"))
                            Dim price As Decimal = Convert.ToDecimal(reader("Price"))
                            Dim itemTotal As Decimal = Convert.ToDecimal(reader("TotalAmount"))

                            ' Check if the value already exists in oldValues
                            If Not oldValues.Contains(itemTotal.ToString()) Then
                                ' Add rows to the DataGridView
                                gridviewpayment.Rows.Add(productName, quantity, price, itemTotal)

                                ' Update total amount
                                totalAmount += itemTotal

                                Dim itemPayment As Decimal = Convert.ToDecimal(txtpayment.Text)
                                Dim change As Decimal = itemPayment - itemTotal
                                totalChange += change

                                oldValues.Add(itemTotal.ToString())
                            End If
                        End While
                    End Using
                End Using
            End Using



            ' Calculate VAT and other amounts
            Dim vatRate As Decimal = GetVAT()
            Dim vatResult = CalculateVATableAndVAT(totalAmount, vatRate)
            Dim vatAbleAmount As Decimal = Math.Round(vatResult.Item1, 2)
            Dim vatAmount As Decimal = Math.Round(vatResult.Item2, 2)

            gridviewpayment.Rows.Add("Total Amount", "", "", totalAmount)
            gridviewpayment.Rows.Add("Vat", "", "", vatAbleAmount)
            gridviewpayment.Rows.Add("Vatable Amount", "", "", vatAmount)
            Dim payment As Decimal = Convert.ToDecimal(txtpayment.Text)
            gridviewpayment.Rows.Add("Total Payment", "", "", payment)
            gridviewpayment.Rows.Add("Total Change", "", "", totalChange)
        Catch ex As Exception
            MessageBox.Show("Error generating receipt: " & ex.Message)
        End Try
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


    Public Function GetVAT() As Decimal
        Dim vatRate As Decimal = 0
        Dim query As String = "SELECT vat_amount FROM tblvat WHERE active = 'True'"
        Try
            Using connection As New SqlConnection(connectionStrings)
                Using command As New SqlCommand(query, connection)
                    connection.Open()
                    Dim result As Object = command.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                        vatRate = Convert.ToDecimal(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error retrieving VAT rate: " & ex.Message)
        End Try
        Return vatRate
    End Function

    Private Function CalculateVATableAndVAT(ByVal totalAmount As Decimal, ByVal vatRate As Decimal) As (Decimal, Decimal)
        Dim vatAmount As Decimal = totalAmount / (1 + (vatRate / 100))

        Dim vatAbleAmount As Decimal = totalAmount - vatAmount
        Return (vatAbleAmount, vatAmount)
    End Function

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        Dim vatAbleAmount As Decimal = 0
        Dim vatAmount As Decimal = 0
        Dim discountedAmount As Decimal = 0
        Dim transactionID As Integer = 0

        Try
            Dim selectedDiscountID As Integer = Convert.ToInt32(cmbdiscount.SelectedValue)
            Dim discountAmount As Decimal = 0
            Dim totalAmount As Decimal = 0

            Using connection As New SqlConnection(connectionStrings)
                connection.Open()

                ' Fetch discount amount
                Dim discountQuery As String = "SELECT discount_amount FROM tbldiscount WHERE discount_id = @discountID"
                Using cmdDiscount As New SqlCommand(discountQuery, connection)
                    cmdDiscount.Parameters.AddWithValue("@discountID", selectedDiscountID)
                    discountAmount = Convert.ToDecimal(cmdDiscount.ExecuteScalar())
                End Using

                Dim vatRate As Decimal = GetVAT()

                Dim transactionQuery As String = "SELECT TOP (1) transactionID, totalamount FROM tblsold ORDER BY transactionID DESC"
                Using cmdTransaction As New SqlCommand(transactionQuery, connection)
                    Using reader As SqlDataReader = cmdTransaction.ExecuteReader()
                        If reader.Read() Then
                            totalAmount = Convert.ToDecimal(reader("totalamount"))
                            transactionID = Convert.ToInt32(reader("transactionID"))
                        Else
                            MessageBox.Show("No transactions found.")
                            Return
                        End If
                    End Using
                End Using

                discountedAmount = totalAmount - (totalAmount * (discountAmount / 100))

                Dim vatResult = CalculateVATableAndVAT(discountedAmount, vatRate)
                vatAbleAmount = vatResult.Item1
                vatAmount = vatResult.Item2

                Dim payment As Decimal = Convert.ToDecimal(txtpayment.Text)
                Dim change As Decimal = payment - discountedAmount

                Dim updateQuery As String = "UPDATE tblsold SET payment = @payment, changes = @change WHERE transactionID = @transactionID"
                Using cmdUpdate As New SqlCommand(updateQuery, connection)
                    cmdUpdate.Parameters.AddWithValue("@payment", payment)
                    cmdUpdate.Parameters.AddWithValue("@change", change)
                    cmdUpdate.Parameters.AddWithValue("@transactionID", transactionID)
                    cmdUpdate.ExecuteNonQuery()
                End Using
            End Using
            MessageBox.Show($"Payment details updated successfully. Discounted Amount: {discountedAmount.ToString("0.00")}, VATable Amount: {vatAbleAmount.ToString("0.00")}, VAT Amount: {vatAmount.ToString("0.00")}")
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub paymentform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        populatediscount()
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        If cmbdiscount.DataSource IsNot Nothing Then
            cmbdiscount.DataSource = Nothing
        End If
        Dim index As Integer = cmbdiscount.FindStringExact("No Discount")
        If index = -1 Then
            cmbdiscount.Items.Add("No Discount")
            cmbdiscount.SelectedIndex = cmbdiscount.Items.IndexOf("No Discount")
        Else
            cmbdiscount.SelectedIndex = index
        End If
        cmbdiscount.Enabled = False
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub
End Class

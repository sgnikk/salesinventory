Imports System.Data.SqlClient
Public Class viewallsales
    Dim connection As New SqlConnection(Module1.connectionStrings)
    Dim connectionstring As String = Module1.connectionStrings
    Public Sub viewsales(dateBought As Date, customername As String)
        Dim dataTable As New DataTable
        Dim queryString As String =
        "
        SELECT
        ts.sale_id,
        i.PRODUCT_NAME AS 'Product Name',
        ts.Quantity,
        ts.Price,
        ts.Amount,
        ts.customer_name 
        FROM tbsale AS ts
        INNER JOIN tblitemm AS i ON ts.ITEM_ID = i.ITEM_ID
        INNER JOIN tblsold AS sd ON ts.sale_id = sd.sale_id
        WHERE sd.date_bought = @dateBought AND ts.customer_name = @cname
        "

        Using connection As New SqlConnection(connectionstring)
            Using command As New SqlCommand(queryString, connection)
                command.Parameters.AddWithValue("@dateBought", dateBought)
                command.Parameters.AddWithValue("@cname", customername)
                Dim dataAdapter As New SqlDataAdapter(command)
                dataAdapter.Fill(dataTable)
            End Using
        End Using
        AddTotalAmountRowToDataGridView(dataTable)
        gridviewsales.DataSource = dataTable
        gridviewsales.Columns("sale_id").Visible = False
        gridviewsales.Columns("customer_name").Visible = False
        Label2.Text = customername
        Label4.Text = "Date Bought :"
        Label5.Text = dateBought
    End Sub

    Private Sub AddTotalAmountRowToDataGridView(dataTable As DataTable)
        ' Calculate total amount
        Dim totalAmount As Decimal = 0
        For Each row As DataRow In dataTable.Rows
            totalAmount += Convert.ToDecimal(row("Amount"))
        Next

        ' Add total amount row
        Dim totalRow As DataRow = dataTable.NewRow()
        totalRow("Product Name") = "Total Amount"
        totalRow("Amount") = totalAmount
        dataTable.Rows.Add(totalRow)
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Me.Close()
    End Sub
End Class
Imports System.Data.SqlClient
Public Class viewallsales
    Dim connection As New SqlConnection(Module1.connectionStrings)
    Dim connectionstring As String = Module1.connectionStrings
    Public Sub viewsales(dateBought As Date, customername As String)
        Dim dataTable As New DataTable
        Dim queryString As String =
        "
SELECT 
    tsold.customer_name AS 'CustomerName',
    titemm.PRODUCT_NAME AS Product,
    tsale.Quantity,
    SUM(tsold.totalamount) AS TotalPrice
FROM 
    [salesinventory].[dbo].[tbsale] AS tsale
INNER JOIN 
    [salesinventory].[dbo].[tblitemm] AS titemm ON tsale.ITEM_ID = titemm.ITEM_ID
INNER JOIN 
    [salesinventory].[dbo].[tblsold] AS tsold ON tsale.transactionID = tsold.transactionID
WHERE 
    tsold.transactionDate = @date
    AND tsold.customer_name = @name
GROUP BY 
    tsold.customer_name,
    titemm.PRODUCT_NAME,
    tsale.Quantity;

        "

        Using connection As New SqlConnection(connectionstring)
            Using command As New SqlCommand(queryString, connection)
                command.Parameters.AddWithValue("@date", dateBought)
                command.Parameters.AddWithValue("@name", customername)
                Dim dataAdapter As New SqlDataAdapter(command)
                dataAdapter.Fill(dataTable)
            End Using
        End Using
        gridviewsales.DataSource = dataTable
        Label2.Text = customername
        Label4.Text = "Date Bought :"
        Label5.Text = dateBought
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Me.Close()
    End Sub
End Class
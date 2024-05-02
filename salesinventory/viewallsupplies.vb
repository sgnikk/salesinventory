Imports System.Data.SqlClient

Public Class viewallsupplies

    Dim connectionstring As String = "Data Source=DESKTOP-1A0SD84\SQLEXPRESS;Initial Catalog=salesinventory;Integrated Security=True"

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Me.Close()
    End Sub
    Public Sub viewsupplied(dateSupplied As Date, supplierName As String, orNumber As String)
        Dim viewtable As New DataTable
        Dim viewquery As String = "SELECT S.SUPPLY_ID, S.SUPPLIER_ID, I.PRODUCT_NAME AS 'Product Name', S.EXPIRATION_DATE AS 'Expiration Date', " &
                  "S.PRICE AS 'Price', S.ornumber AS 'OR Number', S.QUANTITY AS 'Quantity', " &
                  "S.AMOUNT AS 'Amount'" &
                  "FROM tblsupply AS S " &
                  "INNER JOIN tblsupplied AS SP ON S.SUPPLY_ID = SP.SUPPLY_ID " &
                  "INNER JOIN tblsupplier AS SN ON SP.SUPPLIER_ID = SN.SUPPLIER_ID " &
                  "INNER JOIN tblitemm AS I ON S.ITEM_ID = I.ITEM_ID " &
                  "WHERE SP.DATE_SUPPLIED = @dateSupplied AND SN.SUPPLIER_NAME = @supplierName AND S.ornumber = @ornumber"

        Using Connection As New SqlConnection(connectionstring)
            Using command As New SqlCommand(viewquery, Connection)
                command.Parameters.AddWithValue("@dateSupplied", dateSupplied)
                command.Parameters.AddWithValue("@supplierName", supplierName)
                command.Parameters.AddWithValue("@ornumber", orNumber)

                Dim suppliedadapter As New SqlDataAdapter(command)
                suppliedadapter.Fill(viewtable)
            End Using
        End Using

        ' Calculate and add total amount row
        AddTotalAmountRowToDataGridView(viewtable)

        gridviewsupplied.DataSource = viewtable
        gridviewsupplied.Columns("SUPPLY_ID").Visible = False
        gridviewsupplied.Columns("SUPPLIER_ID").Visible = False
        gridviewsupplied.Columns("OR Number").Visible = False
        Label2.Text = supplierName
        Label5.Text = dateSupplied
        Label4.Text = "Date Supplied :"
        Label1.Text = orNumber
        Label3.Text = "OR Number :"
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

End Class

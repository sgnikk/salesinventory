Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DisplayItem()
        DisplayCategory()
        displaymedtype()
        DisplaySupplier()
        Displaysupplies()
        populateinventory()
        discount()
        DisplaySale()
        'Displaysold()
        AuditTrail()
        populateusers()
        populatevat()
        Dim medtypebutton As New DataGridViewButtonColumn
        medtypebutton.UseColumnTextForButtonValue = True
        medtypebutton.HeaderText = "Action"
        medtypebutton.Width = 100
        medtypebutton.Text = "Update"
        medtypebutton.Name = "Update"
        medtypegridview.Columns.Insert(2, medtypebutton)
        Dim meddeletetypebutton As New DataGridViewButtonColumn
        meddeletetypebutton.UseColumnTextForButtonValue = True
        meddeletetypebutton.HeaderText = "Action"
        meddeletetypebutton.Width = 100
        meddeletetypebutton.Text = "Delete"
        meddeletetypebutton.Name = "Delete"
        medtypegridview.Columns.Insert(3, meddeletetypebutton)

        'ITEMS GRIDVIEW
        Dim discountupdatebutton As New DataGridViewButtonColumn
        discountupdatebutton.UseColumnTextForButtonValue = True
        discountupdatebutton.HeaderText = "Action"
        discountupdatebutton.Width = 100
        discountupdatebutton.Text = "Update"
        discountupdatebutton.Name = "Update"
        discountgridview.Columns.Insert(3, discountupdatebutton)
        Dim discountdeletebutton As New DataGridViewButtonColumn
        discountdeletebutton.UseColumnTextForButtonValue = True
        discountdeletebutton.HeaderText = "Action"
        discountdeletebutton.Width = 100
        discountdeletebutton.Text = "Delete"
        discountdeletebutton.Name = "Delete"
        discountgridview.Columns.Insert(4, discountdeletebutton)
        'USERS Gridview
        Dim usersupdatebutton As New DataGridViewButtonColumn
        usersupdatebutton.UseColumnTextForButtonValue = True
        usersupdatebutton.HeaderText = "Action"
        usersupdatebutton.Width = 100
        usersupdatebutton.Text = "Update"
        usersupdatebutton.Name = "Update"
        usergridview.Columns.Insert(5, usersupdatebutton)
        Dim usersdeletebutton As New DataGridViewButtonColumn
        usersdeletebutton.UseColumnTextForButtonValue = True
        usersdeletebutton.HeaderText = "Action"
        usersdeletebutton.Width = 100
        usersdeletebutton.Text = "Delete"
        usersdeletebutton.Name = "Delete"
        usergridview.Columns.Insert(6, usersdeletebutton)

        Dim supplierupdatebutton As New DataGridViewButtonColumn
        supplierupdatebutton.UseColumnTextForButtonValue = True
        supplierupdatebutton.HeaderText = "Action"
        supplierupdatebutton.Width = 100
        supplierupdatebutton.Text = "Update"
        supplierupdatebutton.Name = "Update"
        suppliergridview.Columns.Insert(4, supplierupdatebutton)
        Dim supplierdeletebutton As New DataGridViewButtonColumn
        supplierdeletebutton.UseColumnTextForButtonValue = True
        supplierdeletebutton.HeaderText = "Action"
        supplierdeletebutton.Width = 100
        supplierdeletebutton.Text = "Delete"
        supplierdeletebutton.Name = "Delete"
        suppliergridview.Columns.Insert(5, supplierdeletebutton)

        Dim itemupdatebutton As New DataGridViewButtonColumn
        itemupdatebutton.UseColumnTextForButtonValue = True
        itemupdatebutton.HeaderText = "Action"
        itemupdatebutton.Width = 100
        itemupdatebutton.Text = "Update"
        itemupdatebutton.Name = "Update"
        itemsgridview.Columns.Insert(10, itemupdatebutton)
        Dim itemdeletebutton As New DataGridViewButtonColumn
        itemdeletebutton.UseColumnTextForButtonValue = True
        itemdeletebutton.HeaderText = "Action"
        itemdeletebutton.Width = 100
        itemdeletebutton.Text = "Delete"
        itemdeletebutton.Name = "Delete"
        itemsgridview.Columns.Insert(11, itemdeletebutton)


        Dim categoryupdatebutton As New DataGridViewButtonColumn
        categoryupdatebutton.UseColumnTextForButtonValue = True
        categoryupdatebutton.HeaderText = "Action"
        categoryupdatebutton.Width = 100
        categoryupdatebutton.Text = "Update"
        categoryupdatebutton.Name = "Update"
        categorygridview.Columns.Insert(3, categoryupdatebutton)
        Dim categorydeletebutton As New DataGridViewButtonColumn
        categorydeletebutton.UseColumnTextForButtonValue = True
        categorydeletebutton.HeaderText = "Action"
        categorydeletebutton.Width = 100
        categorydeletebutton.Text = "Delete"
        categorydeletebutton.Name = "Delete"
        categorygridview.Columns.Insert(4, categorydeletebutton)


        Dim vatupdatebutton As New DataGridViewButtonColumn
        vatupdatebutton.UseColumnTextForButtonValue = True
        vatupdatebutton.HeaderText = "Action"
        vatupdatebutton.Width = 100
        vatupdatebutton.Text = "Update"
        vatupdatebutton.Name = "Update"
        vatGridView.Columns.Insert(3, vatupdatebutton)
        Dim vatDeletebutton As New DataGridViewButtonColumn
        vatDeletebutton.UseColumnTextForButtonValue = True
        vatDeletebutton.HeaderText = "Action"
        vatDeletebutton.Width = 100
        vatDeletebutton.Text = "Delete"
        vatDeletebutton.Name = "Delete"
        vatGridView.Columns.Insert(4, vatDeletebutton)

        Dim saleshowbutton As New DataGridViewButtonColumn
        saleshowbutton.UseColumnTextForButtonValue = True
        saleshowbutton.HeaderText = "Action"
        saleshowbutton.Width = 100
        saleshowbutton.Text = "Show"
        saleshowbutton.Name = "Show"
        salesgridview.Columns.Insert(2, saleshowbutton)
    End Sub
    Private Sub Guna2Button7_Click(sender As Object, e As EventArgs) Handles Guna2Button7.Click
        managesupplies.Show()
    End Sub
    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        itemform.Show()
    End Sub
    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click
        supplierform.Show()
    End Sub
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Me.Close()
    End Sub
    Private Sub btnaddmedtype_Click(sender As Object, e As EventArgs) Handles btnaddmedtype.Click
        addtype.Show()
    End Sub
    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        categoryform.Show()
    End Sub

    Public Sub DisplayCategory()
        Dim categorytable As New DataTable()
        Dim query As String = "SELECT CATEGORY_ID, CATEGORY_DESCRIPTION AS 'Category Description', CATEGORY_NAME AS 'Category Name' FROM tblcategory"
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim adapter As New SqlDataAdapter(query, connection)
            adapter.Fill(categorytable)
        End Using

        categorygridview.DataSource = categorytable
        categorygridview.Columns("CATEGORY_ID").Visible = False
    End Sub

    Public Sub DisplaySale()
        Dim salestable As New DataTable()
        Dim query As String = "SELECT 
            tsold.customer_name AS 'Customer Name',
            tsold.transactionDate
        FROM 
            [salesinventory].[dbo].[tbsale] AS tsale
        INNER JOIN 
            [salesinventory].[dbo].[tblsold] AS tsold ON tsale.transactionID = tsold.transactionID"
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim adapter As New SqlDataAdapter(query, connection)
            adapter.Fill(salestable)
        End Using

        salesgridview.DataSource = salestable
    End Sub

    Private Sub salesgridview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles salesgridview.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim dateBought As Date = CType(salesgridview.Rows(e.RowIndex).Cells("TransactionDate").Value, Date)
            Dim customername As String = salesgridview.Rows(e.RowIndex).Cells("Customer Name").Value.ToString()
            Dim viewSalesForm As New viewallsales()
            viewSalesForm.viewsales(dateBought, customername)
            viewSalesForm.Show()
        End If
    End Sub


    Public Sub discount()
        Dim discounttable As New DataTable
        Dim discountquery As String = "
        SELECT
        discount_id,
        discount_name AS 'Discount Name', 
        CONCAT(discount_amount, '%') AS 'Discount Amount'  
        FROM 
        tbldiscount
        "
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim adapterdicount As New SqlDataAdapter(discountquery, connection)
            adapterdicount.Fill(discounttable)
        End Using
        discountgridview.DataSource = discounttable
        discountgridview.Columns("discount_id").Visible = False
    End Sub
    Private Sub DisplayCategoryDAta(categoryID As Integer)
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim query As String = "SELECT CATEGORY_ID, CATEGORY_DESCRIPTION, CATEGORY_NAME " &
                      "FROM tblcategory " &
                      "WHERE CATEGORY_ID = @catid"
            Using Command As New SqlCommand(query, connection)
                Command.Parameters.AddWithValue("@catid", categoryID)
                Using reader As SqlDataReader = Command.ExecuteReader
                    If reader.Read Then
                        categoryform2.txtid.Text = reader("CATEGORY_ID").ToString
                        categoryform2.txtdescription.Text = reader("CATEGORY_DESCRIPTION").ToString
                        categoryform2.txtname.Text = reader("CATEGORY_NAME").ToString
                    End If
                End Using
            End Using
        End Using
    End Sub
    Private Sub categorygridview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles categorygridview.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim columnName As String = categorygridview.Columns(e.ColumnIndex).Name
            Dim cellValue As Object = categorygridview.Rows(e.RowIndex).Cells("CATEGORY_ID").Value
            If cellValue IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(cellValue.ToString()) Then
                Dim selectedCategoryId As Integer = Convert.ToInt32(cellValue)
                DisplayCategoryDAta(selectedCategoryId)
                If columnName = "Delete" Then
                    DeleteRowFromCategory(selectedCategoryId)
                    DisplayCategory()
                ElseIf columnName = "Update" Then
                    categoryform2.Show()
                Else
                    MessageBox.Show("Invalid operation.")
                End If
            Else
                MessageBox.Show("No value to perform operation.")
            End If
        End If
    End Sub
    Private Sub DeleteRowFromCategory(CategoryID As Integer)
        Try
            Dim deletedCategoryName As String = ""

            ' Retrieve the name of the category being deleted
            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Dim getCategoryNameQuery As String = "SELECT CATEGORY_NAME FROM tblcategory WHERE CATEGORY_ID = @categoryID"
                Using getCategoryNameCommand As New SqlCommand(getCategoryNameQuery, connection)
                    getCategoryNameCommand.Parameters.AddWithValue("@categoryID", CategoryID)
                    deletedCategoryName = Convert.ToString(getCategoryNameCommand.ExecuteScalar())
                End Using
            End Using

            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Using transaction As SqlTransaction = connection.BeginTransaction()
                    Try
                        Dim updateItemsQuery As String = "UPDATE tblitemm SET CATEGORY_ID = NULL WHERE CATEGORY_ID = @categoryID"
                        Using updateItemsCommand As New SqlCommand(updateItemsQuery, connection, transaction)
                            updateItemsCommand.Parameters.AddWithValue("@categoryID", CategoryID)
                            updateItemsCommand.ExecuteNonQuery()
                        End Using

                        Dim deleteCategoryQuery As String = "DELETE FROM tblcategory WHERE CATEGORY_ID = @categoryID"
                        Using deleteCategoryCommand As New SqlCommand(deleteCategoryQuery, connection, transaction)
                            deleteCategoryCommand.Parameters.AddWithValue("@categoryID", CategoryID)
                            deleteCategoryCommand.ExecuteNonQuery()
                        End Using

                        ' Log the deletion action into tblaudit
                        Dim loggedInUsername As String = loginformm.LoggedInUsername
                        Dim userID As Integer = loginformm.UserID
                        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
                        Dim timee As String = DateTime.Now.ToString("HH:mm")
                        Dim action As String = $"{loggedInUsername} has deleted category '{deletedCategoryName} from category'"
                        Dim auditInsertQuery As String = "INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)"
                        Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection, transaction)
                            auditInsertCommand.Parameters.AddWithValue("@uID", userID)
                            auditInsertCommand.Parameters.AddWithValue("@acts", action)
                            auditInsertCommand.Parameters.AddWithValue("@time", timee)
                            auditInsertCommand.Parameters.AddWithValue("@date", datee)
                            auditInsertCommand.ExecuteNonQuery()
                        End Using

                        transaction.Commit()
                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
            DisplayItem()
            AuditTrail()
            MessageBox.Show("Category has been deleted successfully.")
        Catch ex As Exception
            MessageBox.Show("Error Deleting category: " & ex.Message)
        End Try
    End Sub

    'ITEMS FUNCTIONS
    Public Sub DisplayItem()
        Dim itemtable As New DataTable()
        Dim query As String = "
        SELECT 
            i.ITEM_ID,
            i.PRODUCT_NAME AS 'Product Name',
            i.DESCRIPTION AS 'Description',
            i.COSTPRICE_BYPIECE AS 'Cost Price',
            i.sellingprice AS 'Selling Price',
            ISNULL(c.CATEGORY_NAME, 'NONE') AS 'Category',
            ISNULL(m.MEDICINE_TYPE, 'NONE') AS 'Medicine Type',
            I.BARCODE AS 'Bar Code',
            I.discount AS 'Discount',
            I.expirable AS 'Expirable'
        FROM 
            tblitemm i 
        LEFT JOIN 
            tblcategory c ON i.CATEGORY_ID = c.CATEGORY_ID
        LEFT JOIN
            tblmedtype m ON i.MEDTYPE_ID = m.MEDTYPE_ID;"
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim adapter As New SqlDataAdapter(query, connection)
            adapter.Fill(itemtable)
        End Using

        itemsgridview.DataSource = itemtable
        itemsgridview.Columns("ITEM_ID").Visible = False
    End Sub
    Private Sub itemsgridview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles itemsgridview.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim columnName As String = itemsgridview.Columns(e.ColumnIndex).Name
            Dim cellValue As Object = itemsgridview.Rows(e.RowIndex).Cells("ITEM_ID").Value
            If cellValue IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(cellValue.ToString()) Then
                Dim selectedItemId As Integer = Convert.ToInt32(cellValue)
                DisplayItemData(selectedItemId)
                If columnName = "Delete" Then
                    DeleteRowFromDatabase(selectedItemId)
                    DisplayItem()
                ElseIf columnName = "Update" Then
                    itemsupdate.Show()
                Else
                    MessageBox.Show("Invalid operation.")
                End If
            Else
                MessageBox.Show("No value to perform operation.")
            End If
        End If
    End Sub
    Private Sub DisplayItemData(itemID As Integer)
        Dim categoryDataTable As New DataTable()
        Dim medtypetable As New DataTable
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()

            Dim categoryQuery As String = "SELECT CATEGORY_ID, CATEGORY_NAME FROM tblcategory"

            Using categoryCommand As New SqlCommand(categoryQuery, connection)
                Using categoryAdapter As New SqlDataAdapter(categoryCommand)
                    categoryAdapter.Fill(categoryDataTable)
                End Using
            End Using

            Dim medtypequery As String = "SELECT MEDTYPE_ID, MEDICINE_TYPE FROM tblmedtype"

            Using medtypeCommand As New SqlCommand(medtypequery, connection)
                Using medtypeAdapter As New SqlDataAdapter(medtypeCommand)
                    medtypeAdapter.Fill(medtypetable)
                End Using
            End Using
        End Using

        With itemsupdate.cmbcategory
            .DataSource = categoryDataTable
            .DisplayMember = "CATEGORY_NAME"
            .ValueMember = "CATEGORY_ID"
        End With

        With itemsupdate.cmdmedtype
            .DataSource = medtypetable
            .DisplayMember = "MEDICINE_TYPE"
            .ValueMember = "MEDTYPE_ID"
        End With

        Using connection As New SqlConnection(connectionStrings)
            connection.Open()

            Dim query As String = "SELECT  ITEM_ID,PRODUCT_NAME,DESCRIPTION,CATEGORY_ID,MEDTYPE_ID,BARCODE,COSTPRICE_BYPIECE,discount,expirable
            FROM tblitemm WHERE ITEM_ID = @ItemID"

            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@ItemID", itemID)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        itemsupdate.txtid.Text = reader("ITEM_ID").ToString()
                        itemsupdate.txtproductname.Text = reader("PRODUCT_NAME").ToString()
                        itemsupdate.txtdescription.Text = reader("DESCRIPTION").ToString
                        itemsupdate.cmbcategory.SelectedValue = reader("CATEGORY_ID")
                        itemsupdate.cmdmedtype.SelectedValue = reader("MEDTYPE_ID")
                        itemsupdate.txtcpricepiece.Text = reader("COSTPRICE_BYPIECE").ToString
                        Dim discountValue As String = reader("discount").ToString()
                        If discountValue.ToLower() = "discount" Then
                            itemsupdate.Guna2CheckBox1.Checked = True
                        Else
                            itemsupdate.Guna2CheckBox1.Checked = False
                        End If
                        Dim isexpirable As String = reader("expirable").ToString
                        If isexpirable.ToLower() = "yes" Then
                            itemsupdate.Guna2CheckBox2.Checked = True
                        Else
                            itemsupdate.Guna2CheckBox2.Checked = False
                        End If

                    End If
                End Using
            End Using
        End Using
    End Sub
    Private Sub DeleteRowFromDatabase(itemID As Integer)
        Try
            Dim deletedItemName As String = ""

            ' Retrieve the name of the item being deleted
            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Dim getItemNameQuery As String = "SELECT PRODUCT_NAME FROM tblitemm WHERE ITEM_ID = @itemID"
                Using getItemNameCommand As New SqlCommand(getItemNameQuery, connection)
                    getItemNameCommand.Parameters.AddWithValue("@itemID", itemID)
                    deletedItemName = Convert.ToString(getItemNameCommand.ExecuteScalar())
                End Using
            End Using

            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Using transaction As SqlTransaction = connection.BeginTransaction()
                    Try
                        ' Update associated tables
                        Dim updateinventory As String = "UPDATE tblinventory SET ITEM_ID = NULL WHERE ITEM_ID = @itemID"
                        Using updateinventorycommand As New SqlCommand(updateinventory, connection, transaction)
                            updateinventorycommand.Parameters.AddWithValue("@itemID", itemID)
                            updateinventorycommand.ExecuteNonQuery()
                        End Using

                        Dim deleteinventory As String = "DELETE FROM tblinventory WHERE ITEM_ID = @itemID"
                        Using deleteinventorycommand As New SqlCommand(deleteinventory, connection, transaction)
                            deleteinventorycommand.Parameters.AddWithValue("@itemID", itemID)
                            deleteinventorycommand.ExecuteNonQuery()
                        End Using

                        Dim updatesupply As String = "UPDATE tblsupply SET ITEM_ID = NULL WHERE ITEM_ID = @itemID"
                        Using updatesupplycommand As New SqlCommand(updatesupply, connection, transaction)
                            updatesupplycommand.Parameters.AddWithValue("@itemID", itemID)
                            updatesupplycommand.ExecuteNonQuery()
                        End Using

                        Dim deletesupply As String = "DELETE FROM tblsupply WHERE ITEM_ID = @itemID"
                        Using deletesupplycommand As New SqlCommand(deletesupply, connection, transaction)
                            deletesupplycommand.Parameters.AddWithValue("@itemID", itemID)
                            deletesupplycommand.ExecuteNonQuery()
                        End Using

                        Dim updatesale As String = "UPDATE tbsale SET ITEM_ID = NULL WHERE ITEM_ID = @itemID"
                        Using updatesalecommand As New SqlCommand(updatesale, connection, transaction)
                            updatesalecommand.Parameters.AddWithValue("@itemID", itemID)
                            updatesalecommand.ExecuteNonQuery()
                        End Using

                        Dim deletesale As String = "DELETE FROM tbsale WHERE ITEM_ID = @itemID"
                        Using deletesalecommand As New SqlCommand(deletesale, connection, transaction)
                            deletesalecommand.Parameters.AddWithValue("@itemID", itemID)
                            deletesalecommand.ExecuteNonQuery()
                        End Using

                        ' Delete the item from tblitemm
                        Dim deleteitemsQuery As String = "DELETE FROM tblitemm WHERE ITEM_ID = @itemID"
                        Using deleteitemsCommand As New SqlCommand(deleteitemsQuery, connection, transaction)
                            deleteitemsCommand.Parameters.AddWithValue("@itemID", itemID)
                            deleteitemsCommand.ExecuteNonQuery()
                        End Using

                        ' Log the deletion action into tblaudit
                        Dim loggedInUsername As String = loginformm.LoggedInUsername
                        Dim userID As Integer = loginformm.UserID
                        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
                        Dim timee As String = DateTime.Now.ToString("HH:mm")
                        Dim action As String = $"{loggedInUsername} has deleted the '{deletedItemName} from products'"
                        Dim auditInsertQuery As String = "INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)"
                        Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection, transaction)
                            auditInsertCommand.Parameters.AddWithValue("@uID", userID)
                            auditInsertCommand.Parameters.AddWithValue("@acts", action)
                            auditInsertCommand.Parameters.AddWithValue("@time", timee)
                            auditInsertCommand.Parameters.AddWithValue("@date", datee)
                            auditInsertCommand.ExecuteNonQuery()
                        End Using

                        transaction.Commit()
                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
            AuditTrail()
            populateinventory()
            DisplayItem()
            MessageBox.Show("Product has been deleted successfully.")
        Catch ex As Exception
            MessageBox.Show("Error Deleting item: " & ex.Message)
        End Try
    End Sub



    'MEDICINE TYPE FUNCTION S
    Public Sub displaymedtype()
        Dim medtypetable As New DataTable
        Dim query As String = "SELECT MEDTYPE_ID AS 'Medicine ID', MEDICINE_TYPE AS 'Medicine Type' FROM tblmedtype"
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim adapter As New SqlDataAdapter(query, connection)
            adapter.Fill(medtypetable)
        End Using
        medtypegridview.DataSource = medtypetable
    End Sub
    Private Sub medtypegridview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles medtypegridview.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim columnName As String = medtypegridview.Columns(e.ColumnIndex).Name
            Dim cellValue As Object = medtypegridview.Rows(e.RowIndex).Cells("Medicine ID").Value
            If cellValue IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(cellValue.ToString()) Then
                Dim selectedMedtypeId As Integer = Convert.ToInt32(cellValue)
                DisplayMedTypeDAta(selectedMedtypeId)
                If columnName = "Delete" Then
                    DeleteRowFromMedType(selectedMedtypeId)
                    displaymedtype()
                ElseIf columnName = "Update" Then
                    typeupdate.Show()
                Else
                    MessageBox.Show("Invalid operation.")
                End If
            Else
                MessageBox.Show("No value to perform operation.")
            End If
        End If
    End Sub
    Private Sub DisplayMedTypeDAta(medtypeID As Integer)
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim query As String = "SELECT MEDTYPE_ID, MEDICINE_TYPE " &
                      "FROM tblmedtype " &
                      "WHERE MEDTYPE_ID = @mid"
            Using Command As New SqlCommand(query, connection)
                Command.Parameters.AddWithValue("@mid", medtypeID)
                Using reader As SqlDataReader = Command.ExecuteReader
                    If reader.Read Then
                        typeupdate.txtid.Text = reader("MEDTYPE_ID").ToString
                        typeupdate.txttype.Text = reader("MEDICINE_TYPE").ToString
                    End If
                End Using
            End Using
        End Using
    End Sub
    Private Sub DeleteRowFromMedType(medtypeID As Integer)
        Try
            Dim deletedMedTypeName As String = ""

            ' Retrieve the name of the medicine type being deleted
            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Dim getMedTypeNameQuery As String = "SELECT MEDICINE_TYPE FROM tblmedtype WHERE MEDTYPE_ID = @medtypeID"
                Using getMedTypeNameCommand As New SqlCommand(getMedTypeNameQuery, connection)
                    getMedTypeNameCommand.Parameters.AddWithValue("@medtypeID", medtypeID)
                    deletedMedTypeName = Convert.ToString(getMedTypeNameCommand.ExecuteScalar())
                End Using
            End Using

            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Using transaction As SqlTransaction = connection.BeginTransaction()
                    Try
                        ' Update associated items
                        Dim updatemedTypeQuery As String = "UPDATE tblitemm SET MEDTYPE_ID = NULL WHERE MEDTYPE_ID = @medtypeID"
                        Using updateItemsCommand As New SqlCommand(updatemedTypeQuery, connection, transaction)
                            updateItemsCommand.Parameters.AddWithValue("@medtypeID", medtypeID)
                            updateItemsCommand.ExecuteNonQuery()
                        End Using

                        ' Delete the medicine type from tblmedtype
                        Dim deletemedTypeQuery As String = "DELETE FROM tblmedtype WHERE MEDTYPE_ID = @medtypeID"
                        Using deleteCategoryCommand As New SqlCommand(deletemedTypeQuery, connection, transaction)
                            deleteCategoryCommand.Parameters.AddWithValue("@medtypeID", medtypeID)
                            deleteCategoryCommand.ExecuteNonQuery()
                        End Using
                        transaction.Commit()

                        ' Log the deletion action into tblaudit
                        Dim loggedInUsername As String = loginformm.LoggedInUsername
                        Dim userID As Integer = loginformm.UserID
                        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
                        Dim timee As String = DateTime.Now.ToString("HH:mm")
                        Dim action As String = $"{loggedInUsername} has deleted the medicine type '{deletedMedTypeName}'"
                        Dim auditInsertQuery As String = "INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)"
                        Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection, transaction)
                            auditInsertCommand.Parameters.AddWithValue("@uID", userID)
                            auditInsertCommand.Parameters.AddWithValue("@acts", action)
                            auditInsertCommand.Parameters.AddWithValue("@time", timee)
                            auditInsertCommand.Parameters.AddWithValue("@date", datee)
                            auditInsertCommand.ExecuteNonQuery()
                        End Using
                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
            AuditTrail()
            DisplayItem()
            MessageBox.Show("Medicine Type has been deleted successfully.")
        Catch ex As Exception
            MessageBox.Show("Error occurred while deleting medicine type: " & ex.Message)
        End Try
    End Sub


    'SUPPLIERS FUNCTIONS
    Public Sub DisplaySupplier()
        Dim suppliertable As New DataTable
        Dim query As String = "SELECT SUPPLIER_ID AS 'Supplier ID', SUPPLIER_NAME AS 'Supplier Name',ADDRESS AS 'Address', CONTACT AS 'Contact' FROM tblsupplier"
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim adapter As New SqlDataAdapter(query, connection)
            adapter.Fill(suppliertable)
        End Using
        suppliergridview.DataSource = suppliertable
        suppliergridview.Columns("Supplier ID").Visible = False
    End Sub
    Private Sub suppliergridview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles suppliergridview.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim columnName As String = suppliergridview.Columns(e.ColumnIndex).Name
            Dim cellValue As Object = suppliergridview.Rows(e.RowIndex).Cells("Supplier ID").Value
            If cellValue IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(cellValue.ToString()) Then
                Dim supplierID As Integer = Convert.ToInt32(cellValue)
                DisplaySupplierData(supplierID)
                If columnName = "Delete" Then
                    DeleteRowFromSupplier(supplierID)
                    DisplaySupplier()
                ElseIf columnName = "Update" Then
                    supplierform2.Show()
                Else
                    MessageBox.Show("Invalid operation.")
                End If
            Else
                MessageBox.Show("No value to perform operation.")
            End If
        End If
    End Sub
    Private Sub DeleteRowFromSupplier(supplierID As Integer)
        Try
            Dim deletedSupplierName As String = ""

            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Dim getSupplierNameQuery As String = "SELECT SUPPLIER_NAME FROM tblsupplier WHERE SUPPLIER_ID = @supplierID"
                Using getSupplierNameCommand As New SqlCommand(getSupplierNameQuery, connection)
                    getSupplierNameCommand.Parameters.AddWithValue("@supplierID", supplierID)
                    deletedSupplierName = Convert.ToString(getSupplierNameCommand.ExecuteScalar())
                End Using
            End Using

            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Using transaction As SqlTransaction = connection.BeginTransaction()
                    Try
                        ' Update associated tables
                        Dim updatesupplyQuery As String = "UPDATE tblsupply SET SUPPLIER_ID = NULL WHERE SUPPLIER_ID = @supplierID"
                        Using updatesupplyCommand As New SqlCommand(updatesupplyQuery, connection, transaction)
                            updatesupplyCommand.Parameters.AddWithValue("@supplierID", supplierID)
                            updatesupplyCommand.ExecuteNonQuery()
                        End Using

                        Dim updatesuppliedQuery As String = "UPDATE tblsupplied SET SUPPLIER_ID = NULL WHERE SUPPLIER_ID = @supplierID"
                        Using updatesuppliercommand As New SqlCommand(updatesuppliedQuery, connection, transaction)
                            updatesuppliercommand.Parameters.AddWithValue("@supplierID", supplierID)
                            updatesuppliercommand.ExecuteNonQuery()
                        End Using

                        ' Delete the supplier from tblsupplier
                        Dim deletesupplierQuery As String = "DELETE FROM tblsupplier WHERE SUPPLIER_ID = @supplierID"
                        Using deleteitemsCommand As New SqlCommand(deletesupplierQuery, connection, transaction)
                            deleteitemsCommand.Parameters.AddWithValue("@supplierID", supplierID)
                            deleteitemsCommand.ExecuteNonQuery()
                        End Using

                        ' Log the deletion action into tblaudit
                        Dim loggedInUsername As String = loginformm.LoggedInUsername
                        Dim userID As Integer = loginformm.UserID
                        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
                        Dim timee As String = DateTime.Now.ToString("HH:mm")
                        Dim action As String = $"{loggedInUsername} has deleted supplier '{deletedSupplierName}'"
                        Dim auditInsertQuery As String = "INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)"
                        Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection, transaction)
                            auditInsertCommand.Parameters.AddWithValue("@uID", userID)
                            auditInsertCommand.Parameters.AddWithValue("@acts", action)
                            auditInsertCommand.Parameters.AddWithValue("@time", timee)
                            auditInsertCommand.Parameters.AddWithValue("@date", datee)
                            auditInsertCommand.ExecuteNonQuery()
                        End Using

                        transaction.Commit()
                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
            AuditTrail()
            DisplayItem()
            DisplaySupplier()
            Displaysupplies()
            MessageBox.Show("Supplier has been deleted successfully.")
        Catch ex As Exception
            MessageBox.Show("Error Deleting supplier: " & ex.Message)
        End Try
    End Sub

    Private Sub DisplaySupplierData(SupplierID As Integer)
        Dim connectionString As String = "Data Source=DESKTOP-1A0SD84\SQLEXPRESS;Initial Catalog=salesinventory;Integrated Security=True"
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Dim query As String = "SELECT SUPPLIER_ID AS 'Supplier ID', SUPPLIER_NAME AS 'Supplier Name',ADDRESS AS 'Address', CONTACT AS 'Contact'" &
                      "FROM tblsupplier " &
                      "WHERE SUPPLIER_ID = @sid"
            Using Command As New SqlCommand(query, connection)
                Command.Parameters.AddWithValue("@sid", SupplierID)
                Using reader As SqlDataReader = Command.ExecuteReader
                    If reader.Read Then
                        supplierform2.txtid.Text = reader("Supplier ID").ToString
                        supplierform2.txtsuppliername.Text = reader("Supplier Name").ToString
                        supplierform2.txtaddress.Text = reader("Address").ToString
                        supplierform2.txtcontact.Text = reader("Contact").ToString
                    End If
                End Using
            End Using
        End Using
    End Sub

    'SUPPLIES LIST PAGE FUNCTIONS
    Public Sub populateinventory()
        Dim inventorytable As New DataTable
        Dim criticalProducts As New List(Of String)() ' List to store critical product names
        Dim inventoryquery As String = "
        SELECT
            MAX(ti.INVENTORY_ID) AS 'Inventory ID',
            ISNULL(i.PRODUCT_NAME, 'NONE') AS 'Product Name',
            ISNULL(i.BARCODE, 'NONE') AS 'Barcode',  
            ISNULL(SUM(ti.STOCK_IN), 0) AS 'Stock In',
            ISNULL(SUM(ti.STOCK_OUT), 0) AS 'Stock Out',
            ISNULL(SUM(ti.STOCK_AVAILABLE), 0) AS 'Stock Available'
        FROM 
            tblinventory ti
        LEFT JOIN 
            tblitemm i ON ti.ITEM_ID = i.ITEM_ID
        GROUP BY 
            i.PRODUCT_NAME, i.BARCODE"

        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim inventoryadapter As New SqlDataAdapter(inventoryquery, connection)
            inventoryadapter.Fill(inventorytable)
        End Using

        For Each row As DataRow In inventorytable.Rows
            Dim productName As String = row("Product Name").ToString()
            Dim stockAvailable As Integer = Convert.ToInt32(row("Stock Available"))

            If stockAvailable < 11 Then
                criticalProducts.Add(productName)
            End If
        Next

        Dim criticalProductsMessage As String = If(criticalProducts.Count > 0,
        "Products at critical level:" & Environment.NewLine & String.Join(Environment.NewLine, criticalProducts.Select(Function(product) "- " & product)),
        "")

        If Not String.IsNullOrEmpty(criticalProductsMessage) Then
            MessageBox.Show(criticalProductsMessage, "Critical Products", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        inventorygridview.DataSource = inventorytable

        If inventorygridview.Columns.Contains("Inventory ID") Then
            inventorygridview.Columns("Inventory ID").Visible = False
        End If

        If inventorygridview.Columns.Contains("Barcode") Then
            inventorygridview.Columns("Barcode").Visible = False
        End If
    End Sub



    Private Sub txtsrchbar_TextChanged(sender As Object, e As EventArgs) Handles txtsrchbar.TextChanged
        Dim searchText As String = txtsrchbar.Text.Trim()

        If searchText <> "" Then
            Dim filteredTable As New DataTable
            Dim inventoryquery As String = "
        SELECT
            MAX(ti.INVENTORY_ID) AS 'Inventory ID',
            ISNULL(i.PRODUCT_NAME, 'NONE') AS 'Product Name',
            ISNULL(i.BARCODE, 'NONE') AS 'Barcode',  
            SUM(STOCK_IN) AS 'Total Stock In',
            SUM(STOCK_BY_PIECES) AS 'Total Stock By Pieces',
            SUM(STOCK_BY_PACK) AS 'Total Stock By Pack',
            SUM(STOCK_OUT) AS 'Total Stock Out',
            SUM(STOCK_AVAILABLE) AS 'Total Stock Available'
        FROM 
            tblinventory ti
        LEFT JOIN 
            tblitemm i ON ti.ITEM_ID = i.ITEM_ID
        WHERE 
            i.BARCODE LIKE @Barcode
        GROUP BY 
            i.PRODUCT_NAME, i.BARCODE"

            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Dim inventoryadapter As New SqlDataAdapter()
                Dim command As New SqlCommand(inventoryquery, connection)
                command.Parameters.AddWithValue("@Barcode", "%" & searchText & "%")
                inventoryadapter.SelectCommand = command
                inventoryadapter.Fill(filteredTable)
            End Using
            inventorygridview.DataSource = filteredTable

            If inventorygridview.Columns.Contains("Inventory ID") Then
                inventorygridview.Columns("Inventory ID").Visible = False
            End If

            If inventorygridview.Columns.Contains("Barcode") Then
                inventorygridview.Columns("Barcode").Visible = False
            End If
        Else
            populateinventory()
        End If
    End Sub

    Private Sub DisplaySuppliesData(SupplierID As Integer)
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim query As String = "SELECT SUPPLIER_ID AS 'Supplier ID', SUPPLIER_NAME AS 'Supplier Name',ADDRESS AS 'Address', CONTACT AS 'Contact'" &
                      "FROM tblsupplier " &
                      "WHERE SUPPLIER_ID = @sid"
            Using Command As New SqlCommand(query, connection)
                Command.Parameters.AddWithValue("@sid", SupplierID)
                Using reader As SqlDataReader = Command.ExecuteReader
                    If reader.Read Then
                        supplierform2.txtid.Text = reader("Supplier ID").ToString
                        supplierform2.txtsuppliername.Text = reader("Supplier Name").ToString
                        supplierform2.txtaddress.Text = reader("Address").ToString
                        supplierform2.txtcontact.Text = reader("Contact").ToString
                    End If
                End Using
            End Using
        End Using
    End Sub

    Public Sub Displaysupplies()
        Dim suppiestable As New DataTable()
        Dim suppliesquery As String = "
        SELECT 
            MAX(SUPPLIED_ID) AS SUPPLIED_ID, 
            MAX(ISNULL(s.ornumber, 'NONE')) AS 'OR Number',
            DATE_SUPPLIED AS 'Date Supplied',
            MAX(s.SUPPLY_ID) AS SUPPLY_ID,
            MAX(ISNULL(tsr.SUPPLIER_NAME, 'NONE')) AS 'Supplier Name'
        FROM 
            tblsupplied tsl
        LEFT JOIN 
            tblsupply s ON tsl.SUPPLY_ID = s.SUPPLY_ID
        LEFT JOIN
            tblsupplier tsr ON tsl.SUPPLIER_ID = tsr.SUPPLIER_ID
        GROUP BY 
            DATE_SUPPLIED, tsr.SUPPLIER_NAME, s.ornumber;"
        'MAX(tsr.SUPPLIER_NAME) AS 'Supplier Name'
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim supplyadapter As New SqlDataAdapter(suppliesquery, connection)
            supplyadapter.Fill(suppiestable)
        End Using

        suppliesdatagridview.DataSource = suppiestable
        suppliesdatagridview.Columns("SUPPLY_ID").Visible = False
        suppliesdatagridview.Columns("SUPPLIED_ID").Visible = False
    End Sub

    Private Sub suppliesdatagridview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles suppliesdatagridview.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim dateSupplied As Date = CType(suppliesdatagridview.Rows(e.RowIndex).Cells("Date Supplied").Value, Date)
            Dim supplierName As String = suppliesdatagridview.Rows(e.RowIndex).Cells("Supplier Name").Value.ToString()
            Dim orNumber As String = suppliesdatagridview.Rows(e.RowIndex).Cells("OR Number").Value.ToString()
            ' Removed the lines related to OR number

            ' Call the viewallsupplies method without passing the OR number
            viewallsupplies.viewsupplied(dateSupplied, supplierName, orNumber)
            viewallsupplies.Show()
        End If
    End Sub



    Private Sub Guna2Button6_Click(sender As Object, e As EventArgs) Handles Guna2Button6.Click
        salesform.Show()
    End Sub

    Private Sub Guna2Button8_Click(sender As Object, e As EventArgs) Handles Guna2Button8.Click
        discountform.Show()
    End Sub

    Private Sub discountgridview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles discountgridview.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim columnName As String = discountgridview.Columns(e.ColumnIndex).Name
            Dim cellValue As Object = discountgridview.Rows(e.RowIndex).Cells("discount_id").Value
            If cellValue IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(cellValue.ToString()) Then
                Dim discountID As Integer = Convert.ToInt32(cellValue)
                DisplaydiscountData(discountID)
                If columnName = "Delete" Then
                    DeleteRowFromdiscount(discountID)
                    discount()
                ElseIf columnName = "Update" Then
                    discountupdate.Show()
                Else
                    MessageBox.Show("Invalid operation.")
                End If
            Else
                MessageBox.Show("No value to perform operation.")
            End If
        End If
    End Sub
    Private Sub DeleteRowFromdiscount(discountID As Integer)
        Try
            Dim deletedDiscountName As String = ""

            ' Retrieve the name of the discount being deleted
            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Dim getDiscountNameQuery As String = "SELECT discount_name FROM tbldiscount WHERE discount_id = @discountID"
                Using getDiscountNameCommand As New SqlCommand(getDiscountNameQuery, connection)
                    getDiscountNameCommand.Parameters.AddWithValue("@discountID", discountID)
                    deletedDiscountName = Convert.ToString(getDiscountNameCommand.ExecuteScalar())
                End Using
            End Using

            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Using transaction As SqlTransaction = connection.BeginTransaction()
                    Try
                        ' Delete the discount from tbldiscount
                        Dim deletediscount As String = "DELETE FROM tbldiscount WHERE discount_id = @discountID"
                        Using deletediscountcommand As New SqlCommand(deletediscount, connection, transaction)
                            deletediscountcommand.Parameters.AddWithValue("@discountID", discountID)
                            deletediscountcommand.ExecuteNonQuery()
                        End Using

                        ' Log the deletion action into tblaudit
                        Dim loggedInUsername As String = loginformm.LoggedInUsername
                        Dim userID As Integer = loginformm.UserID
                        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
                        Dim timee As String = DateTime.Now.ToString("HH:mm")
                        Dim action As String = $"{loggedInUsername} has deleted discount '{deletedDiscountName}'"
                        Dim auditInsertQuery As String = "INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)"
                        Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection, transaction)
                            auditInsertCommand.Parameters.AddWithValue("@uID", userID)
                            auditInsertCommand.Parameters.AddWithValue("@acts", action)
                            auditInsertCommand.Parameters.AddWithValue("@time", timee)
                            auditInsertCommand.Parameters.AddWithValue("@date", datee)
                            auditInsertCommand.ExecuteNonQuery()
                        End Using

                        transaction.Commit()
                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
            MessageBox.Show("Discount has been deleted successfully.")
            AuditTrail()
        Catch ex As Exception
            MessageBox.Show("Error Deleting discount: " & ex.Message)
        End Try
    End Sub

    Private Sub DisplaydiscountData(discountID As Integer)
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim query As String = "SELECT discount_id, discount_name AS 'Discount Name', discount_amount AS 'Discount Amount'" &
                      "FROM tbldiscount " &
                      "WHERE discount_id = @did"
            Using Command As New SqlCommand(query, connection)
                Command.Parameters.AddWithValue("@did", discountID)
                Using reader As SqlDataReader = Command.ExecuteReader
                    If reader.Read Then
                        discountupdate.txtids.Text = reader("discount_id").ToString
                        discountupdate.txtdiscountname.Text = reader("Discount Name").ToString
                        discountupdate.txtdiscountamount.Text = reader("Discount Amount").ToString
                    End If
                End Using
            End Using
        End Using
    End Sub

    Public Sub AuditTrail()
        Dim auditTable As New DataTable
        Dim AuditQuery As String =
        "SELECT 
            auditID,
            COALESCE(u.name, 'Unknown') AS 'Name',
            actions AS 'Actions',
            time AS 'Time',
            date AS 'Date'
        FROM tblaudit ta
        LEFT JOIN tblusers u ON ta.users_id = u.users_id"
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim auditadapter As New SqlDataAdapter(AuditQuery, connection)
            auditadapter.Fill(auditTable)
        End Using
        auditgridview.DataSource = auditTable
        auditgridview.Columns("auditID").Visible = False
        auditgridview.Columns("Actions").Width = 600
    End Sub

    Public Sub populateusers()
        Dim userstable As New DataTable
        Dim usersquery As String =
        "select
        users_id,
        name AS 'Name',
        lastname AS 'Last Name',
        userlevel AS 'User Level',
        username AS 'User Name',
        password AS 'Password'
        FROM tblusers"
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim usersadapter As New SqlDataAdapter(usersquery, connection)
            usersadapter.Fill(userstable)
        End Using
        usergridview.DataSource = userstable
        usergridview.Columns("users_id").Visible = False
    End Sub

    Private Sub usergridview_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles usergridview.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim columnName As String = usergridview.Columns(e.ColumnIndex).Name
            Dim cellValue As Object = usergridview.Rows(e.RowIndex).Cells("users_id").Value
            If cellValue IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(cellValue.ToString()) Then
                Dim usersID As Integer = Convert.ToInt32(cellValue)
                DisplayUsersData(usersID)
                If columnName = "Delete" Then
                    DeleteRowFromUsers(usersID)
                    populateusers()
                    AuditTrail()

                ElseIf columnName = "Update" Then
                    formregister.Show()
                    formregister.btnupdate.Visible = True
                    formregister.btnsave.Visible = False
                    formregister.populateblanks()
                Else
                    MessageBox.Show("Invalid operation.")
                End If
            Else
                MessageBox.Show("No value to perform operation.")
            End If
        End If
    End Sub
    Private Sub DeleteRowFromUsers(usersID As Integer)
        Try
            Dim deletedUsername As String = ""

            ' Retrieve the username of the user being deleted
            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Dim getUsernameQuery As String = "SELECT username FROM tblusers WHERE users_id = @usersID"
                Using getUsernameCommand As New SqlCommand(getUsernameQuery, connection)
                    getUsernameCommand.Parameters.AddWithValue("@usersID", usersID)
                    deletedUsername = Convert.ToString(getUsernameCommand.ExecuteScalar())
                End Using
            End Using

            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Using transaction As SqlTransaction = connection.BeginTransaction()
                    Try
                        ' Update associated audit records
                        Dim updateauditusers As String = "UPDATE tblaudit SET users_id = NULL WHERE users_id = @usersID"
                        Using updateaudituserscommand As New SqlCommand(updateauditusers, connection, transaction)
                            updateaudituserscommand.Parameters.AddWithValue("@usersID", usersID)
                            updateaudituserscommand.ExecuteNonQuery()
                        End Using

                        ' Delete the user from tblusers
                        Dim deleteusers As String = "DELETE FROM tblusers WHERE users_id = @usersID"
                        Using deleteuserscommand As New SqlCommand(deleteusers, connection, transaction)
                            deleteuserscommand.Parameters.AddWithValue("@usersID", usersID)
                            deleteuserscommand.ExecuteNonQuery()
                        End Using

                        ' Log the deletion action into tblaudit
                        Dim loggedInUsername As String = loginformm.LoggedInUsername
                        Dim userID As Integer = loginformm.UserID
                        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
                        Dim timee As String = DateTime.Now.ToString("HH:mm")
                        Dim action As String = $"{loggedInUsername} has deleted user '{deletedUsername}'"
                        Dim auditInsertQuery As String = "INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)"
                        Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection, transaction)
                            auditInsertCommand.Parameters.AddWithValue("@uID", userID)
                            auditInsertCommand.Parameters.AddWithValue("@acts", action)
                            auditInsertCommand.Parameters.AddWithValue("@time", timee)
                            auditInsertCommand.Parameters.AddWithValue("@date", datee)
                            auditInsertCommand.ExecuteNonQuery()
                        End Using

                        transaction.Commit()
                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
            MessageBox.Show("User has been deleted successfully.")
            AuditTrail()
        Catch ex As Exception
            MessageBox.Show("Error Deleting user: " & ex.Message)
        End Try
    End Sub


    Private Sub DisplayUsersData(usersID As Integer)
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim query As String = "SELECT users_id, name, lastname, userlevel, username, password FROM tblusers WHERE users_id = @uID"
            Using Command As New SqlCommand(query, connection)
                Command.Parameters.AddWithValue("@uID", usersID)
                Using reader As SqlDataReader = Command.ExecuteReader
                    If reader.Read Then
                        formregister.txtID.Text = reader("users_id")
                        formregister.txtlname.Text = reader("lastname").ToString
                        formregister.txtusername.Text = reader("username").ToString
                        formregister.txtpassword.Text = reader("password").ToString
                        formregister.txtfname.Text = reader("name").ToString
                    End If
                End Using
            End Using
        End Using
    End Sub


    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        formregister.Show()
    End Sub

    Public Sub populatevat()
        Dim VATtable As New DataTable
        Dim VatQuery As String =
        "Select
        vatID,
        vat_amount AS 'Vat Amount',
        active AS 'Active'
        from tblvat"
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim VatAdapter As New SqlDataAdapter(VatQuery, connection)
            VatAdapter.Fill(VATtable)
        End Using
        vatGridView.DataSource = VATtable
        vatGridView.Columns("vatID").Visible = False
    End Sub

    Private Sub vatGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles vatGridView.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim columnName As String = vatGridView.Columns(e.ColumnIndex).Name
            Dim cellValue As Object = vatGridView.Rows(e.RowIndex).Cells("VatID").Value
            If cellValue IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(cellValue.ToString()) Then
                Dim VatIDs As Integer = Convert.ToInt32(cellValue)
                DisplayVatData(VatIDs)
                If columnName = "Delete" Then
                    DeleteRowFromVat(VatIDs)
                    populatevat()
                    AuditTrail()
                ElseIf columnName = "Update" Then
                    vatforms.btnadd.Visible = False
                    vatforms.Show()
                Else
                    MessageBox.Show("Invalid operation.")
                End If
            Else
                MessageBox.Show("No value to perform operation.")
            End If
        End If
    End Sub

    Private Sub DeleteRowFromVat(VatID As Integer)
        Try
            Dim deletedVatName As String = ""

            ' Retrieve the name of the VAT being deleted
            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Dim getVatNameQuery As String = "SELECT vat_name FROM tblvat WHERE vatID = @vatID"
                Using getVatNameCommand As New SqlCommand(getVatNameQuery, connection)
                    getVatNameCommand.Parameters.AddWithValue("@vatID", VatID)
                    deletedVatName = Convert.ToString(getVatNameCommand.ExecuteScalar())
                End Using
            End Using

            Using connection As New SqlConnection(connectionStrings)
                connection.Open()
                Using transaction As SqlTransaction = connection.BeginTransaction()
                    Try
                        ' Delete the VAT from tblvat
                        Dim deleteVAT As String = "DELETE FROM tblvat WHERE vatID = @vatID"
                        Using deletevatcommand As New SqlCommand(deleteVAT, connection, transaction)
                            deletevatcommand.Parameters.AddWithValue("@vatID", VatID)
                            deletevatcommand.ExecuteNonQuery()
                        End Using

                        ' Log the deletion action into tblaudit
                        Dim loggedInUsername As String = loginformm.LoggedInUsername
                        Dim userID As Integer = loginformm.UserID
                        Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
                        Dim timee As String = DateTime.Now.ToString("HH:mm")
                        Dim action As String = $"{loggedInUsername} has deleted VAT '{deletedVatName}'"
                        Dim auditInsertQuery As String = "INSERT INTO tblaudit (users_id, actions, time, date) VALUES (@uID, @acts, @time, @date)"
                        Using auditInsertCommand As New SqlCommand(auditInsertQuery, connection, transaction)
                            auditInsertCommand.Parameters.AddWithValue("@uID", userID)
                            auditInsertCommand.Parameters.AddWithValue("@acts", action)
                            auditInsertCommand.Parameters.AddWithValue("@time", timee)
                            auditInsertCommand.Parameters.AddWithValue("@date", datee)
                            auditInsertCommand.ExecuteNonQuery()
                        End Using

                        transaction.Commit()
                    Catch ex As Exception
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using
            MessageBox.Show("VAT has been deleted successfully.")
            AuditTrail()
        Catch ex As Exception
            MessageBox.Show("Error Deleting VAT: " & ex.Message)
        End Try
    End Sub



    Private Sub DisplayVatData(VatIDs As Integer)
        Using connection As New SqlConnection(connectionStrings)
            connection.Open()
            Dim query As String = "SELECT vatID , vat_amount, active FROM tblvat WHERE vatID = @vID"
            Using Command As New SqlCommand(query, connection)
                Command.Parameters.AddWithValue("@vID", VatIDs)
                Using reader As SqlDataReader = Command.ExecuteReader
                    If reader.Read Then
                        vatforms.txtvatID.Text = reader("vatID")
                        vatforms.txtvat.Text = reader("vat_amount").ToString
                        Dim vatActive As String = reader("active").ToString()
                        If vatActive.ToLower() = "true" Then
                            vatforms.Guna2CheckBox1.Checked = True
                        Else
                            vatforms.Guna2CheckBox1.Checked = False
                        End If
                    End If
                End Using
            End Using
        End Using
    End Sub

    Private Sub btnvat_Click(sender As Object, e As EventArgs) Handles btnvat.Click
        vatforms.btnupdate.Visible = False
        vatforms.Show()
    End Sub

    Private Sub Guna2TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Guna2TabControl1.SelectedIndexChanged
        If Guna2TabControl1.SelectedIndex = 6 Then
            Application.Exit()
            Dim loggedInUsername As String = loginformm.LoggedInUsername
            Dim userID As Integer = loginformm.UserID
            Dim datee As String = DateTime.Now.ToString("yyyy-MM-dd")
            Dim timee As String = DateTime.Now.ToString("HH:mm")
            Dim action As String = $"{loggedInUsername} has logged out" ' Use the logged-in username

            connection.Open()

            Using cmdinsert As New SqlCommand("INSERT INTO tblaudit (users_id,actions, time, date) VALUES (@uID,@acts, @time, @date)", connection)
                cmdinsert.Parameters.AddWithValue("@uID", userID)
                cmdinsert.Parameters.AddWithValue("@acts", action)
                cmdinsert.Parameters.AddWithValue("@time", timee)
                cmdinsert.Parameters.AddWithValue("@date", datee)
                cmdinsert.ExecuteNonQuery()
                AuditTrail()
            End Using

            connection.Close()
        End If
    End Sub

    Private Sub Guna2Button9_Click(sender As Object, e As EventArgs)

    End Sub

    Dim dt As New DataTable
    Dim adp As New SqlDataAdapter

    Private Sub Guna2Button9_Click_1(sender As Object, e As EventArgs) Handles Guna2Button9.Click
        ' Create a new DataTable
        dt = New DataTable("tbsale")

        ' Create a SqlConnection (replace connection string and initialize it properly)
        Using connection As New SqlConnection(Module1.connectionStrings)
            ' Open the connection
            connection.Open()

            ' Create a SqlCommand with the query
            Dim query As String = "SELECT ts.customer_name, i.PRODUCT_NAME, s.Quantity, i.sellingprice AS Price, ts.totalamount AS TotalAmount, ts.transactionDate FROM tbsale s INNER JOIN tblitemm i ON s.ITEM_ID = i.ITEM_ID INNER JOIN (SELECT transactionID, transactionDate, customer_name, totalamount FROM tblsold) ts ON s.transactionID = ts.transactionID"

            ' Create a SqlDataAdapter with the command and connection
            adp = New SqlDataAdapter(query, connection)

            ' Fill the DataTable with the data from the query
            adp.Fill(dt)
        End Using

        ' Create a new instance of the Crystal Report
        Dim crystal As New CrystalReport4

        ' Set the DataSource of the Crystal Report to the filled DataTable
        crystal.SetDataSource(dt)

        ' Set the Crystal Report Viewer's ReportSource to the Crystal Report
        CrystalReportViewer1.ReportSource = crystal
    End Sub


End Class
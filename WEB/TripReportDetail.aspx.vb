Imports BLL
Imports BLL.DTO

Public Class WebForm1
    Inherits System.Web.UI.Page

    Dim _tripBLL As New TripBLL

    Protected Sub LoadDataExpense()
        Dim _tripID = Request.QueryString("TripID")
        Dim _expenses = _tripBLL.GetTripWithExpenseByTripId(CInt(_tripID))
        lvExpenseItems.DataSource = _expenses
        lvExpenseItems.DataBind()
    End Sub

    Protected Sub LoadDataTrip()
        Dim _tripsID = Request.QueryString("TripID")
        Dim _trips = _tripBLL.GetByIdTrip(CInt(_tripsID))
        lblTripID.Text = lblTripID.Text & _trips.TripID
        lblStatus.Text = _trips.Status.StatusName
        lblLocation.Text = _trips.Location
        lblStartDate.Text = _trips.StartDate
        lblEndDate.Text = _trips.EndDate
        lblTotalExpense.Text = _trips.TotalCost

        'Check the Status of the trip
        If String.Equals(_trips.Status.StatusName, "Drafted/Planned", StringComparison.OrdinalIgnoreCase) Then
            btnAddExpenseItem.Visible = True
            btnAddExpenseItem.Enabled = True
            btnReimbursement.Visible = True
            btnReimbursement.Enabled = True
        Else
            btnAddExpenseItem.Visible = False
            btnAddExpenseItem.Enabled = False
            btnReimbursement.Visible = False
            btnReimbursement.Enabled = False
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadDataTrip()
            LoadDataExpense()

        End If
    End Sub

    Sub GetEditExpenseItem(id As Integer)
        Dim tripBLL As New TripBLL()
        Dim expenseItem As ExpenseItemsDTO = tripBLL.GetExpenseByExpenseID(id)
        txtEditExpenseType.Text = expenseItem.ExpenseType
        txtEditItemCost.Text = expenseItem.ItemCost
        txtEditDescription.Text = expenseItem.Description
        imgEditReceiptImage.ImageUrl = "~/ReceiptImages/" & expenseItem.ReceiptImage
    End Sub

    Function CheckFileType(ByVal fileName As String) As Boolean
        Dim ext As String = IO.Path.GetExtension(fileName)
        Select Case ext.ToLower()
            Case ".gif"
                Return True
            Case ".png"
                Return True
            Case ".jpg"
                Return True
            Case ".jpeg"
                Return True
            Case ".bmp"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Sub InitializeFormAddArticle()
        txtAddDescription.Text = String.Empty
        txtAddExpenseType.Text = String.Empty
        txtAddItemCost.Text = String.Empty
    End Sub

    Protected Sub btnAddExpense_Click(sender As Object, e As EventArgs)
        Try
            Dim fileName As String = Guid.NewGuid().ToString() + fuAddReceiptImage.FileName
            If fuAddReceiptImage.HasFile Then
                If CheckFileType(fileName) Then
                    Dim _path = Server.MapPath("~/ReceiptImages/")
                    fuAddReceiptImage.SaveAs(_path + fileName)
                End If
            Else
                ltMessage.Text = "<span class='alert alert-danger'>Error: Only images are allowed</span><br/><br/>"
                Return
            End If


            Dim tripBLL As New TripBLL()
            Dim expenseDTO As New ExpenseItemsDTO

            expenseDTO.TripID = CInt(Request.QueryString("TripID"))
            expenseDTO.ExpenseType = txtAddExpenseType.Text
            expenseDTO.ItemCost = CDec(txtAddItemCost.Text)
            expenseDTO.Description = txtAddDescription.Text
            expenseDTO.ReceiptImage = fileName

            tripBLL.CreateExpense(expenseDTO)
            InitializeFormAddArticle()
            LoadDataExpense()
            LoadDataTrip()
            ltMessage.Text = "<span class='alert alert-success'>Artice added successfully</span><br/><br/>"
        Catch ex As Exception
            ltMessage.Text = "<span class='alert alert-danger'>Error BackEnd: " & ex.Message & "</span><br/><br/>"
        End Try
    End Sub

    Protected Sub btnReimbursement_Click(sender As Object, e As EventArgs)
        Dim userSession = Session("LoggedInUser")

        If userSession IsNot Nothing Then
            Dim roleUser As String = userSession.Role.ToString()
            If roleUser = "Admin" Then
                btnReimbursement.Enabled = False
                btnReimbursement.Visible = False
            End If
        End If

        Try
            Dim tripBLL As New TripBLL()
            Dim trip As New CreateTripReportDTO

            trip.TripID = CInt(Request.QueryString("TripID"))
            trip.StatusID = 2
            tripBLL.ClaimReimbursmnt(trip)
            LoadDataTrip()
            LoadDataExpense()
            ltMessage.Text = "<span class='alert alert-success'>Reimbursement claimed successfully</span><br/><br/>"
        Catch ex As Exception
            ltMessage.Text = "<span class='alert alert-danger'>Error Claim: " & ex.Message & "</span><br/><br/>"
        End Try
    End Sub

    Protected Sub lvExpenseItems_SelectedIndexChanging(sender As Object, e As ListViewSelectEventArgs)

    End Sub

    Protected Sub lvExpenseItems_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ViewState("Command") = "Edit" Then
            ltMessage.Text = "<span class='alert alert-success'>Edit</span><br/><br/>"
            GetEditExpenseItem(CInt(lvExpenseItems.SelectedDataKey.Value))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenModalScript", "$(window).on('load',function(){$('#editExpenseModal').modal('show');})", True)
        End If
    End Sub

    Protected Sub lvExpenseItems_ItemDeleting(sender As Object, e As ListViewDeleteEventArgs)
        Try
            Dim tripBLL As New TripBLL
            Dim expenseItems As New ExpenseItemsDTO

            expenseItems.ExpenseID = Convert.ToInt32(lvExpenseItems.DataKeys(e.ItemIndex).Value)
            expenseItems.TripID = Convert.ToInt32(Request.QueryString("TripID"))
            tripBLL.DeleteExpense(expenseItems)
            LoadDataExpense()
            LoadDataTrip()

            ltMessage.Text = "<span class='alert alert-success'>Expense deleted successfully</span><br/><br/>"
        Catch ex As Exception
            ltMessage.Text = "<span class='alert alert-danger'>Error: " & ex.Message & "</span><br/><br/>"
        End Try
    End Sub

    Protected Sub lvExpenseItems_ItemCommand(sender As Object, e As ListViewCommandEventArgs)
        ViewState("Command") = e.CommandArgument
    End Sub

    Protected Sub btnUpdateExpenseItem_Click(sender As Object, e As EventArgs)
        Try
            'rename uploaded file
            Dim fileName As String = Guid.NewGuid().ToString() + fuEditReceiptImage.FileName
            If fuEditReceiptImage.HasFile Then
                If CheckFileType(fileName) Then
                    Dim _path = Server.MapPath("~/ReceiptImages/")
                    fuEditReceiptImage.SaveAs(_path + fileName)
                End If
            Else
                ltMessage.Text = "<span class='alert alert-danger'>Error: Only images are allowed</span><br/><br/>"
                Return
            End If

            Dim tripBLL As New TripBLL()
            Dim expenseItems As New ExpenseItemsDTO
            expenseItems.ExpenseID = Convert.ToInt32(lvExpenseItems.SelectedDataKey.Value)
            expenseItems.TripID = Convert.ToInt32(Request.QueryString("TripID"))
            expenseItems.ExpenseType = txtEditExpenseType.Text
            expenseItems.ItemCost = CDec(txtEditItemCost.Text)
            expenseItems.Description = txtEditDescription.Text
            expenseItems.ReceiptImage = fileName
            tripBLL.UpdateExpense(expenseItems)

            lvExpenseItems.DataBind()
            LoadDataTrip()
            LoadDataExpense()
            ltMessage.Text = "<span class='alert alert-success'>Expense updated successfully</span><br/><br/>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CloseModalScript", "$('#editExpenseModal').modal('hide');", True)
        Catch ex As Exception
            ltMessage.Text = "<span class='alert alert-danger'>Error: " & ex.Message & "</span><br/><br/>"
        End Try
    End Sub
End Class
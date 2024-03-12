Imports BLL
Imports BLL.DTO

Public Class ApprovalPageDetail
    Inherits System.Web.UI.Page

    Dim _tripBLL As New TripBLL
    Dim _staffBLL As New StaffBLL
    Dim _approvalBLL As New ApprovalBLL

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

        'button Submit Disable if Status is Approved
        If _trips.Status.StatusID = 3 Then
            btnSubmitApproval.Enabled = False
            btnSubmitApproval.Visible = False

            For Each item As ListViewItem In lvExpenseItems.Items
                Dim lnkApprove As LinkButton = CType(item.FindControl("lnkApprove"), LinkButton)
                Dim lnkReject As LinkButton = CType(item.FindControl("lnkReject"), LinkButton)
                If lnkApprove IsNot Nothing AndAlso lnkReject IsNot Nothing Then
                    lnkApprove.Visible = False
                    lnkReject.Visible = False
                End If
            Next

        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Login Check
            If Session("LoggedInUser") Is Nothing Then
                Response.Redirect("~/LoginPage.aspx")
            End If

            'Role Check from Session
            Dim loggedInUser As StaffDTO = DirectCast(Session("LoggedInUser"), StaffDTO)
            Dim role As String = loggedInUser.Role

            If role = "Admin" Or role = "Approver" Then
                LoadDataExpense()
                LoadDataTrip()
            Else
                Response.Redirect("~/TripReport.aspx")
            End If
        End If
    End Sub

    Protected Sub lvExpenseItems_ItemCommand(sender As Object, e As ListViewCommandEventArgs)
        If e.CommandName = "Approve" Then
            Try
                Dim expenseID As Integer = Convert.ToInt32(e.CommandArgument)
                Dim approverUsername As String = DirectCast(Session("LoggedInUser"), StaffDTO).Username

                Dim users As List(Of StaffDTO) = _staffBLL.GetByName(approverUsername)
                If users.Count = 0 Then
                    Throw New Exception("Approver not found")
                End If
                Dim approverID As Integer = users(0).StaffID

                Dim approval As New ApprovalDTO
                approval.ExpenseID = expenseID
                approval.ApproverID = approverID
                _approvalBLL.SetApproval(approval)
                LoadDataExpense()
            Catch ex As Exception
                ltMessage.Text = ex.Message
            End Try
        ElseIf e.CommandName = "Reject" Then
            Try
                Dim expenseID As Integer = Convert.ToInt32(e.CommandArgument)
                Dim approverUsername As String = DirectCast(Session("LoggedInUser"), StaffDTO).Username

                Dim users As List(Of StaffDTO) = _staffBLL.GetByName(approverUsername)
                If users.Count = 0 Then
                    Throw New Exception("Approver not found")
                End If
                Dim approverID As Integer = users(0).StaffID

                Dim approval As New ApprovalDTO
                approval.ExpenseID = expenseID
                approval.ApproverID = approverID
                _approvalBLL.SetRejection(approval)
                LoadDataExpense()
            Catch ex As Exception
                ltMessage.Text = ex.Message
            End Try

        End If
    End Sub

    Protected Sub lvExpenseItems_ItemDataBound(sender As Object, e As ListViewItemEventArgs)

    End Sub

    Protected Sub lnkBack_Click(sender As Object, e As EventArgs)
        ' Redirect to ApprovalPage.aspx
        Response.Redirect("~/ApprovalPage.aspx")
    End Sub

    Protected Sub btnSubmitApproval_Click(sender As Object, e As EventArgs)
        ' Redirect to ApprovalPage.aspx
        _tripBLL.SubmitApproval(CInt(Request.QueryString("TripID")), 3)
        Response.Redirect("~/ApprovalPage.aspx")
        ltMessage.Text = "Trip has been submitted for approval"
    End Sub
End Class
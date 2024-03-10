Imports BLL
Imports BLL.DTO
Imports BO
Imports Microsoft.AspNet.FriendlyUrls

Public Class AddTripReport
    Inherits System.Web.UI.Page

    Dim _tripBLL As New TripBLL
    Dim _staffBLL As New StaffBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Bind ListView with empty data
            Dim expenses As New List(Of ExpenseItemsDTO)
            expenses.Add(New ExpenseItemsDTO())
            lvExpenseItems.DataSource = expenses
            lvExpenseItems.DataBind()

            'Bind StaffID from Session
            Dim loggedInUser As StaffDTO = TryCast(Session("LoggedInUser"), StaffDTO)
            If loggedInUser IsNot Nothing Then
                'txtSubmittedBy.Text = loggedInUser.StaffID.ToString()
            End If
            BindStaffDropDown()
        End If
    End Sub

    Private Sub BindStaffDropDown()
        Dim staff As List(Of StaffDTO) = _staffBLL.GetAll()
        'For staff who submitted
        ddlSubmittedBy.DataSource = staff
        ddlSubmittedBy.DataTextField = "Name"
        ddlSubmittedBy.DataValueField = "StaffID"
        ddlSubmittedBy.DataBind()
        'For Attendees
        'ddlAttendees.DataSource = staff
        'ddlAttendees.DataTextField = "Name"
        'ddlAttendees.DataValueField = "StaffID"
        'ddlAttendees.DataBind()
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

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        Try
            'Bind data from TextBoxes to TripReportDTO
            Dim tripReport As New CreateTripReportDTO

            'Check Session
            'Dim loggedInUser As StaffDTO = TryCast(Session("LoggedInUser"), StaffDTO)
            'If loggedInUser IsNot Nothing Then
            '    TripReport.SubmittedBy = loggedInUser.StaffID
            'End If

            'tripReport.SubmittedBy = ddlAttendees.SelectedValue
            tripReport.TripID = Convert.ToInt32(Request.QueryString("TripID"))
            tripReport.StartDate = Convert.ToDateTime(txtStartDate.Text)
            tripReport.EndDate = Convert.ToDateTime(txtEndDate.Text)
            tripReport.Location = txtLocation.Text
            tripReport.StatusID = 2 'InProgress

            'Dim attendees As New TripAttendeesDTO
            'attendees.TripID = Convert.ToInt32(Request.QueryString("TripID"))
            'attendees.StaffID = Convert.ToInt32(ddlAttendees.SelectedValue)

            'Bind data from Table Expense
            Dim expenses As New List(Of ExpenseItemsDTO)


            For Each item As ListViewItem In lvExpenseItems.Items
                Dim txtExpenseType As TextBox = DirectCast(item.FindControl("txtExpenseType"), TextBox)
                Dim txtItemCost As TextBox = DirectCast(item.FindControl("txtItemCost"), TextBox)
                Dim txtDescription As TextBox = DirectCast(item.FindControl("txtDescription"), TextBox)
                Dim receiptImage As FileUpload = DirectCast(item.FindControl("fuReceiptImage"), FileUpload)

                'Check if FileUpload has file
                Dim fileName As String = Guid.NewGuid.ToString() & receiptImage.FileName
                If receiptImage.HasFile Then
                    If CheckFileType(fileName) Then
                        receiptImage.SaveAs(Server.MapPath("~/ReceiptImages/") & fileName)
                    Else
                        ltMessage.Text = "<span class='alert alert-danger'>Error: Only images are allowed</span><br/><br/>"
                    End If
                End If

                Dim expense As New ExpenseItemsDTO With {
                    .ExpenseType = txtExpenseType.Text,
                    .ItemCost = Convert.ToDecimal(txtItemCost.Text),
                    .Description = txtDescription.Text,
                    .ReceiptImage = fileName
                }
                expenses.Add(expense)
            Next

            'Add TripReport and Expense
            _tripBLL.CreateTrip(tripReport, expenses)

            ltMessage.Text = "<span class='alert alert-success'>Trip Report has been submitted</span><br/><br/>"
            'Redirect to TripReport.aspx
            Response.Redirect("TripReport.aspx")
        Catch ex As Exception
            ltMessage.Text = "<span class='alert alert-danger'>Error: " & ex.Message & "</span><br/><br/>"
        End Try
    End Sub

    Protected Sub btnDraft_Click(sender As Object, e As EventArgs)

    End Sub
End Class

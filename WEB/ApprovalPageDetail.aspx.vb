Imports BLL
Imports BLL.DTO

Public Class ApprovalPageDetail
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
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Login Check
            If Session("LoggedInUser") Is Nothing Then
                Response.Redirect("~/LoginPage.aspx")
            End If

            'Role Check from Session
            Dim loggedInUser As StaffDTO = DirectCast(Session("LoggedInUser"), StaffDTO)

            If loggedInUser.Role = "Admin" And loggedInUser.Role = "Approver" Then
                LoadDataExpense()
                LoadDataTrip()
            Else
                Response.Redirect("~/TripReport.aspx")
            End If
        End If
    End Sub

End Class
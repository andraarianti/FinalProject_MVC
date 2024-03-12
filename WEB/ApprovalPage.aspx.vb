Imports BLL
Imports BLL.DTO

Public Class ApprovalPage
    Inherits System.Web.UI.Page

    Dim tripBLL As New TripBLL()

    Public Sub LoadData()
        Dim results = tripBLL.GetAllWithStatus()
        lvTrip.DataSource = results
        lvTrip.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Login Check
            If Session("LoggedInUser") Is Nothing Then
                Response.Redirect("~/LoginPage.aspx")
            Else
                'Role Check from Session
                Dim loggedInUser As StaffDTO = DirectCast(Session("LoggedInUser"), StaffDTO)
                Dim role As String = loggedInUser.Role

                If role = "Admin" Or role = "Approver" Then
                    LoadData()
                Else
                    Response.Redirect("~/TripReport.aspx")
                End If
            End If
        End If
    End Sub

    Protected Sub lvTrip_ItemCommand(sender As Object, e As ListViewCommandEventArgs)
        If e.CommandName = "Detail" Then
            Dim tripID As Integer = Convert.ToInt32(e.CommandArgument)
            Response.Redirect("ApprovalPageDetail.aspx?TripID=" & tripID)
        End If
    End Sub
End Class
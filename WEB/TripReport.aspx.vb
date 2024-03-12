Imports BLL
Imports BLL.DTO
Imports BLL.Interfaces

Public Class TripReport
    Inherits System.Web.UI.Page

    Dim tripBLL As New TripBLL()

    Public Sub LoadData()
        Dim staffUsername As String = DirectCast(Session("LoggedInUser"), StaffDTO).Username
        Dim _staffBLL As New StaffBLL()
        'get the staff ID
        Dim staff As List(Of StaffDTO) = _staffBLL.GetByName(staffUsername)
        If staff.Count = 0 Then
            Throw New Exception("Approver not found")
        End If
        Dim submittedBy As Integer = staff(0).StaffID

        Dim results = tripBLL.GetTripByUserId(submittedBy)
        lvTrip.DataSource = results
        lvTrip.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadData()
        End If
    End Sub

    Protected Sub btnAddTrip_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub lvTrip_ItemCommand(sender As Object, e As ListViewCommandEventArgs)
        ViewState("Command") = e.CommandArgument
    End Sub

    Protected Sub lvTrip_ItemDeleting(sender As Object, e As ListViewDeleteEventArgs)
        Try
            Dim tripID As Integer = Convert.ToInt32(lvTrip.DataKeys(e.ItemIndex).Value)
            tripBLL.DeleteTrip(tripID)
            LoadData()
            ltMessage.Text = "<span class='alert alert-success'>Trip Successfully Deleted</span><br/><br/>"
        Catch ex As Exception
            ltMessage.Text = "<span class='alert alert-danger'>Error: " & ex.Message & "</span><br/><br/>"
        End Try

    End Sub
End Class
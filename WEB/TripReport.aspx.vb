Imports BLL
Imports BLL.DTO

Public Class TripReport
    Inherits System.Web.UI.Page

    Dim tripBLL As New TripBLL()

    Public Sub LoadData()
        Dim results = tripBLL.GetAllWithStatus()
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
            Dim tripBll As New TripBLL()
            Dim trip As New ReadTripDTO

            Dim tripID As Integer = Convert.ToInt32(lvTrip.DataKeys(e.ItemIndex).Value)
            tripBll.DeleteTrip(tripID)
            LoadData()
        Catch ex As Exception
            ltMessage.Text = "<span class='alert alert-danger'>Error: " & ex.Message & "</span><br/><br/>"
        End Try

    End Sub
End Class
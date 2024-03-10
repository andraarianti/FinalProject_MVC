Imports BLL
Imports BLL.DTO

Public Class Position
    Inherits System.Web.UI.Page
    Dim positionBLL As New PositionBLL()

    Sub LoadData()
        Dim results = positionBLL.GetAll()
        lvPosition.DataSource = results
        lvPosition.DataBind()
    End Sub

    Sub LoadPositionData(positionID As String)
        Dim positionBLL As New PositionBLL()
        Dim position = positionBLL.GetById(CInt(positionID))
        txtPositionName.Text = position.PositionName
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            LoadData()
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        Dim positionBLL As New PositionBLL()
        Dim position As New PositionDTO

        position.PositionName = txtPositionName.Text
        positionBLL.Insert(position)
        LoadData()
    End Sub

    Protected Sub lvPosition_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub lvPosition_SelectedIndexChanging(sender As Object, e As ListViewSelectEventArgs)

    End Sub

    Protected Sub lvPosition_ItemDeleting(sender As Object, e As ListViewDeleteEventArgs)
        Dim positionID As Integer = Convert.ToInt32(lvPosition.DataKeys(e.ItemIndex).Value)
        Try
            positionBLL.Delete(positionID)
            LoadData()
            ltMessage.Text = "<span class='alert alert-success'>Position deleted successfully</span>"
        Catch ex As Exception
            ltMessage.Text = "<span class='alert alert-danger'>Error: " & ex.Message & "</span><br/><br/>"
        End Try
    End Sub
End Class
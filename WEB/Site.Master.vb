Imports BLL.DTO

Public Class SiteMaster
    Inherits MasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("LoggedInUser") IsNot Nothing Then
                Dim _staffDto As StaffDTO = CType(Session("LoggedInUser"), StaffDTO)
                pnlAnonymous.Visible = False
                pnlLoggedIn.Visible = True
            Else
                pnlAnonymous.Visible = True
                pnlLoggedIn.Visible = False
            End If
        End If
    End Sub
End Class
Imports BLL.DTO

Public Class SiteMaster
    Inherits MasterPage

    Function RenderMenu(roleName As String) As String
        Dim menuBuilder As New StringBuilder()

        Dim _renderPosition As String = "<li class='nav-item'>
                <a class='nav-link' href='Position.aspx'>
                    <i class='fas fa-fw fa fa-sitemap'></i>
                    <span>Position</span></a>
            </li>"

        Dim _renderStaff As String = "<li class='nav-item'>
                <a class='nav-link' href='Staff.aspx'>
                    <i class='fas fa-fw fa fa-users'></i>
                    <span>Staff</span></a>
            </li>"

        Dim _renderTrip As String = "<li class='nav-item'>
                <a class='nav-link' href='TripReport.aspx'>
                    <i class='fas fa-fw fa fa-plane'></i>
                    <span>Trip Report</span></a>
            </li>"

        Dim _renderApproval As String = "<li class='nav-item'>
                <a class='nav-link' href='ApprovalPage.aspx'>
                    <i class='fas fa-fw fa-check-square'></i>
                    <span>Approval Expense</span></a>
            </li>"

        If roleName = "Admin" Then
            menuBuilder.Append(_renderPosition)
            menuBuilder.Append(_renderStaff)
            menuBuilder.Append(_renderTrip)
            menuBuilder.Append(_renderApproval)
        End If

        If roleName = "Approver" Then
            menuBuilder.Append(_renderApproval)
            menuBuilder.Append(_renderTrip)
        End If

        If roleName = "Requestor" Then
            menuBuilder.Append(_renderTrip)
        End If

        Return menuBuilder.ToString()
    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim sb As New StringBuilder()

        If Not Page.IsPostBack Then
            If Session("LoggedInUser") IsNot Nothing Then
                Dim _staffDto As StaffDTO = CType(Session("LoggedInUser"), StaffDTO)
                pnlAnonymous.Visible = False
                pnlLoggedIn.Visible = True
                Dim role As String = _staffDto.Role

                sb.Append(RenderMenu(role))
                ltDynamicMenu.Text = sb.ToString()
            Else
                pnlAnonymous.Visible = True
                pnlLoggedIn.Visible = False
                Response.Redirect("LoginPage.aspx")
            End If
        End If
    End Sub
End Class
Imports BLL.DTO

Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim loggedInUser As StaffDTO = DirectCast(Session("LoggedInUser"), StaffDTO)
        Dim Name As String = loggedInUser.Name
        Dim Role As String = loggedInUser.Role

        ltlWelcome.Text = "<h3>Welcome " & Name & " (" & Role & ")</h3>"
        ltlWelcome.Mode = LiteralMode.PassThrough
    End Sub
End Class
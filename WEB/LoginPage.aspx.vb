Imports System.Web.Configuration
Imports BLL
Imports BLL.DTO

Public Class LoginPage
    Inherits System.Web.UI.Page

    Dim staffBLL As New StaffBLL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Try

            Dim username As String = txtUsername.Text
            Dim password As String = txtPassword.Text
            Dim _staffDto As StaffDTO = staffBLL.Login(username, password)

            If _staffDto IsNot Nothing Then
                'Save the user in session
                Session("LoggedInUser") = _staffDto
                Response.Redirect("Staff.aspx")
            Else
                ltMessage.Text = "<span class='alert alert-danger'>Invalid username or password</span>"
            End If
        Catch ex As Exception
            ltMessage.Text = "<span class='alert alert-danger'>Error: " & ex.Message & "</span>"
        End Try
    End Sub
End Class
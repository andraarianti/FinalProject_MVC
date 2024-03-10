Imports BLL
Imports BLL.DTO

Public Class AddStaff
    Inherits System.Web.UI.Page

    Dim staffBLL As New StaffBLL()
    Dim positionBLL As New PositionBLL()

    Sub LoadData()
        gvStaff.DataSource = staffBLL.GetAll()
        gvStaff.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("LoggedInUser") Is Nothing Then
                Response.Redirect("~/LoginPage.aspx")
            End If

            'btnSave.Enabled = False
            LoadData()

            'Show Position in dropdown
            ddPosition.DataSource = positionBLL.GetAll()
            ddPosition.DataTextField = "PositionName"
            ddPosition.DataValueField = "PositionID"
            ddPosition.DataBind()

            ddEditPosition.DataSource = positionBLL.GetAll()
            ddEditPosition.DataTextField = "PositionName"
            ddEditPosition.DataValueField = "PositionID"
            ddEditPosition.DataBind()
        End If
    End Sub

    Protected Sub gvStaff_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "Select" Then
            Dim staffID As Integer = Convert.ToInt32(e.CommandArgument)
            Dim objStaff = staffBLL.GetById(staffID)
            txtEditName.Text = objStaff.Name
            ddEditPosition.SelectedValue = objStaff.PositionID
            ddEditRole.SelectedValue = objStaff.Role
            txtEditUsername.Text = objStaff.Username
            txtEditEmail.Text = objStaff.Email
            btnUpdate.Enabled = True
            txtEditID.Text = staffID
        ElseIf e.CommandName = "Delete" Then
            Dim staffID As Integer = Convert.ToInt32(e.CommandArgument)
            Try
                staffBLL.Delete(staffID)
                LoadData()
                ltMessage.Text = "<span class='alert alert-success'>Staff deleted successfully</span>"
            Catch ex As Exception
                ltMessage.Text = "<span class='alert alert-danger'>Error: " & ex.Message & "</span>"
            End Try
        End If

    End Sub

    Protected Sub gvStaff_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        Try
            Dim _staff As New StaffDTO
            _staff.Name = txtName.Text
            _staff.PositionID = ddPosition.SelectedValue
            _staff.Role = ddRole.SelectedValue
            _staff.Username = txtUsername.Text
            _staff.Email = txtEmail.Text
            _staff.Password = txtPassword.Text
            staffBLL.Insert(_staff)

            LoadData()

            'clear form
            txtName.Text = ""
            txtUsername.Text = ""
            txtEmail.Text = ""
            txtPassword.Text = ""
            ddPosition.SelectedIndex = 0
            ddRole.SelectedIndex = 0

        Catch ex As Exception
            ltModal.Text = "<span class='alert alert-danger'>Error Insert Data: " & ex.Message & "</span>"
        End Try
    End Sub

    Protected Sub gvStaff_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

    End Sub

    Protected Sub gvStaff_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim staffID As Integer = Convert.ToInt32(gvStaff.SelectedValue())
        ltMessage.Text = "<span class='alert alert-success'>StaffID: " & staffID.ToString & "</span>"
        'Dim objCategory = staffBLL.GetById(staffID)
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If String.IsNullOrEmpty(txtEditID.Text) OrElse String.IsNullOrEmpty(txtEditName.Text) Then
            ltMessage.Text = "<span class='alert alert-danger'>Category ID and Name are required</span>"
            Return
        End If

        Try
            Dim editStaff As New StaffDTO
            editStaff.StaffID = Convert.ToInt32(txtEditID.Text)
            editStaff.Name = txtEditName.Text
            editStaff.PositionID = ddEditPosition.SelectedValue
            editStaff.Role = ddEditRole.SelectedValue
            editStaff.Username = txtEditUsername.Text
            editStaff.Email = txtEditEmail.Text
            staffBLL.Update(editStaff)
            LoadData()
            ltMessage.Text = "<span class='alert alert-success'>Staff updated successfully</span>"

        Catch ex As Exception
            ltMessage.Text = "<span class='alert alert-danger'>Error Update Data: " & ex.Message & "</span>"
        End Try
    End Sub


    Protected Sub gvStaff_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub

    Protected Sub gvStaff_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        gvStaff.PageIndex = e.NewPageIndex
        LoadData()
    End Sub
End Class
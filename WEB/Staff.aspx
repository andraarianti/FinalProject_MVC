<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Staff.aspx.vb" Inherits="WEB.AddStaff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h1 class="h3 mb-4 text-gray-800">Staff Page</h1>
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Staff List</h6>
            </div>
            <div class="card-body">
                <asp:Literal ID="ltMessage" runat="server" /><br />
                <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#myModal">
                    Add Staff
                </button>
                <br />
                <br />
                <asp:GridView ID="gvStaff" CssClass="table table-hover table-bordered" AutoGenerateColumns="False"
                    runat="server" DataKeyNames="StaffID" OnRowCommand="gvStaff_RowCommand" OnRowDeleting="gvStaff_RowDeleting"
                    OnRowDataBound="gvStaff_RowDataBound" OnSelectedIndexChanged="gvStaff_SelectedIndexChanged" OnRowEditing="gvStaff_RowEditing" AllowPaging="True"
                    PageSize="5" OnPageIndexChanging="gvStaff_PageIndexChanging">
                    <HeaderStyle CssClass="table-header" />
                    <Columns>
                        <asp:BoundField DataField="StaffID" HeaderText="StaffID"/>
                        <asp:BoundField DataField="Name" HeaderText="Staff Name" SortExpression="Name" />
                        <asp:BoundField DataField="Email" HeaderText="Staff Email" SortExpression="Email" />
                        <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                        <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Role" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnSelect" runat="server" CssClass="btn btn-outline-info btn-sm" CausesValidation="False" CommandName="Select" CommandArgument='<%# Eval("StaffID") %>' Text="Select"/>
                                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-danger btn-sm" CauseValidation="False" CommandName="Delete" CommandArgument='<%# Eval("StaffID") %>' Text="Delete" />
                                <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-outline-warning btn-sm" CauseValidation="False" CommandName="Select" CommandArgument='<%# Eval("StaffID") %>' Text="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            </div>
        </div>
    </div>
    
    <hr />

      <div class="card">
        <div class="card-header">
            <h6>Edit Data</h6>
        </div>
        <div class="card-body">
            <div class="form-group">
                <asp:Label ID="ltErrorMessage" runat="server" />
            </div>

            <!-- Row 1 -->
            <div class="form-row">
                 <div class="form-group col-md-3">
                     <label for="txtEditID">ID:</label>
                     <asp:TextBox ID="txtEditID" runat="server" EnableViewState="false" CssClass="form-control"/>
                 </div>
                <div class="form-group col-md-3">
                    <label for="txtEditName">Name:</label>
                    <asp:TextBox ID="txtEditName" runat="server" EnableViewState="false" CssClass="form-control" placeholder="Enter Name" />
                </div>
                <div class="form-group col-md-3">
                    <label for="ddEditPosition">Position:</label>
                    <asp:DropDownList ID="ddEditPosition" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group col-md-3">
                    <label for="ddEditRole">Role:</label>
                    <asp:DropDownList ID="ddEditRole" CssClass="form-control" runat="server">
                        <asp:ListItem Value="Admin">Admin</asp:ListItem>
                        <asp:ListItem Value="Approver">Approver</asp:ListItem>
                        <asp:ListItem Value="Requestor">Requestor</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <!-- Row 2 -->
            <div class="form-row">                
                <div class="form-group col-md-3">
                    <label for="txtEditEmail">Email:</label>
                    <asp:TextBox ID="txtEditEmail" runat="server" EnableViewState="false" CssClass="form-control" placeholder="Enter Email" />
                </div>
                 <div class="form-group col-md-3">
                     <label for="txtEditUsername">Username:</label>
                     <asp:TextBox ID="txtEditUsername" runat="server" EnableViewState="false" CssClass="form-control" placeholder="Enter Username" />
                 </div>
                 <div class="form-group col-md-3">
                     <br />
                     <asp:Button Text="Update" ID="btnUpdate" CssClass="btn btn-primary btn-md" OnClick="btnUpdate_Click" runat="server" />
                 </div>
            </div>
        </div>
    </div>

    <!-- The Modal Add -->
    <div class="modal" id="myModal">
        <div class="modal-dialog">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header bg-primary text-white">
                    <h4 class="modal-title">Add Staff</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>

                <!-- Modal Body -->
                <div class="modal-body">
                    <div="form-group">
                        <asp:Label ID="ltModal" runat="server" />
                    </div="form-group">
                    <div class="form-group">
                        <label for="txtName">Name:</label>
                        <asp:TextBox ID="txtName" runat="server" EnableViewState="false" CssClass="form-control" placeholder="Enter Name" />
                    </div>
                    <div class="form-group">
                        <label for="ddPosition">Position:</label>
                        <asp:DropDownList ID="ddPosition" runat="server" CssClass="form-control" AppendDataBoundItems="true"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="ddRole">Role:</label>
                        <asp:DropDownList ID="ddRole" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                            <asp:ListItem Selected="True">-- Select Role --</asp:ListItem>
                            <asp:ListItem Value="Admin">Admin</asp:ListItem>
                            <asp:ListItem Value="Approver">Approver</asp:ListItem>
                            <asp:ListItem Value="Requestor">Requestor</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="txtEmail">Email:</label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" EnableViewState="false" CssClass="form-control" placeholder="Enter Email" />
                    </div>
                    <div class="form-group">
                        <label for="txtUsername">Username:</label>
                        <asp:TextBox ID="txtUsername" runat="server" EnableViewState="false" CssClass="form-control" placeholder="Enter Username" />
                    </div>
                    <div class="form-group">
                        <label for="txtPassword">Password:</label>
                        <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" EnableViewState="false" CssClass="form-control" placeholder="Enter Password" />
                    </div>
                </div>

                <!-- Modal Footer -->
                <div class="modal-footer">
                    <asp:Button Text="Submit" ID="btnSubmit" CssClass="btn btn-primary btn-sm"
                        OnClick="btnSubmit_Click" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

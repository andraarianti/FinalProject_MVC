<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Position.aspx.vb" Inherits="WEB.Position" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="card">
            <div class="card-header">
                <h2>Position</h2>

            </div>
            <div class="card-body">
                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#addPositionModal">
                    Add
                </button>
               <asp:Literal ID="ltMessage" runat="server" />
                <br />
                <br />
                <asp:ListView ID="lvPosition" runat="server" DataKeyNames="PositionID" OnSelectedIndexChanged="lvPosition_SelectedIndexChanged"
                    OnSelectedIndexChanging="lvPosition_SelectedIndexChanging" OnItemDeleting="lvPosition_ItemDeleting">
                    <LayoutTemplate>
                        <table class="table table-hover table-bordered">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Position</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr id="itemPlaceholder" runat="server"></tr>
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("PositionID") %></td>
                            <td><%# Eval("PositionName") %></td>
                            <td>
                                <asp:LinkButton ID="lnkEdit" Text="Edit" CssClass="btn btn-outline-warning btn-sm"
                                    CommandName="Select" runat="server" />
                                &nbsp;
                                <asp:LinkButton ID="lnkDelete" Text="Delete" CssClass="btn btn-outline-danger btn-sm"
                                    CommandName="Delete" runat="server" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </div>

    <!-- Modal for Add Position -->
    <div class="modal fade" id="addPositionModal" tabindex="-1" role="dialog" aria-labelledby="addPositionModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="addPositionModalLabel">Add Position</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="txtIDPosition">Position:</label>
                        <asp:TextBox ID="txtIDPosition" ReadOnly="true" runat="server" EnableViewState="false" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label for="txtPositionName">Position Name :</label>
                        <asp:TextBox ID="txtPositionName" runat="server" EnableViewState="false" CssClass="form-control" placeholder="Enter Position Name" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnSave" Text="Save" runat="server" class="btn btn-primary" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

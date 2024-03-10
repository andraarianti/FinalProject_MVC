<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ApprovalPage.aspx.vb" Inherits="WEB.ApprovalPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
    <h1 class="h3 mb-4 text-gray-800">Approval Page</h1>
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Trip Report</h6>
        </div>
        <div class="card-body">
            <asp:Literal ID="ltMessage" runat="server" />
            <br />
            <br />
            <div class="table-responsive">
                <asp:ListView ID="lvTrip" runat="server" DataKeyNames="TripID" OnItemCommand="lvTrip_ItemCommand">
                    <LayoutTemplate>
                        <table class="table table-bordered table-responsive" id="dataTable" width="100%" cellspacing="0">
                            <thead>
                                <tr>
                                    <th>Trip ID</th>
                                    <th>Submitted By</th>
                                    <th>Destination</th>
                                    <th>Start Date</th>
                                    <th>End Date</th>
                                    <th>Total Cost</th>
                                    <th>Status</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("TripID") %></td>
                            <td><%# Eval("Staff.Name") %></td>
                            <td><%# Eval("Location") %></td>
                            <td><%# Eval("StartDate", "{0:yyyy-MM-dd}") %></td>
                            <td><%# Eval("EndDate", "{0:yyyy-MM-dd}") %></td>
                            <td><%# Eval("TotalCost", "{0:C}") %></td>
                            <td><%# Eval("Status.StatusName") %></td>
                            <td>
                                <asp:LinkButton ID="lnkDetail" runat="server" CommandName="Detail" CommandArgument='<%# Eval("TripID") %>'
                                    PostBackUrl='<%# $"~/ApprovalPageDetail.aspx?TripID={Eval("TripID")}" %>'
                                    Text="Detail" CssClass="btn btn-info" />                               
                        </tr>
                    </ItemTemplate>
                </asp:ListView>

            </div>
        </div>
    </div>
</div>
</asp:Content>

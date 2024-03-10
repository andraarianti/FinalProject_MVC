<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TripReport.aspx.vb" Inherits="WEB.TripReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h1 class="h3 mb-4 text-gray-800">Trip Report Page</h1>
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Trip Report</h6>
            </div>
            <div class="card-body">
                <asp:Literal ID="ltMessage" runat="server" />
                <br />
                <asp:LinkButton ID="btnAddTrip" runat="server" PostBackUrl="~/AddTripReport.aspx" Text="Add Trip Report" CssClass="btn btn-primary" />
                <br />
                <br />
                <div class="table-responsive">

                    <asp:ListView ID="lvTrip" runat="server" DataKeyNames="TripID" OnItemCommand="lvTrip_ItemCommand" OnItemDeleting="lvTrip_ItemDeleting" EnableDeleting="True">
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
                                <td><%# Eval("TotalCost") %></td>
                                <td><%# Eval("Status.StatusName") %></td>
                                <td>
                                    <asp:LinkButton ID="lnkDetail" runat="server" CommandName="Detail" CommandArgument='<%# Eval("TripID") %>'
                                        PostBackUrl='<%# $"~/TripReportDetail.aspx?TripID={Eval("TripID")}" %>'
                                        Text="Detail" CssClass="btn btn-info" />
                                    <%--<asp:HyperLink NavigateUrl='<%# Eval("TripID", "~/TripReportDetail?TripID={0}") %>' runat="server" CssClass="btn btn-success btn-sm" Text="Detail"/>--%>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# $"{Eval("TripID")}" %>' Text="Delete" CssClass="btn btn-danger"
                                        OnClientClick="return confirm('Are you sure you want to delete this trip?')" />

                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>

                </div>
            </div>
        </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ApprovalPageDetail.aspx.vb" Inherits="WEB.ApprovalPageDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h3 class="h3 mb-4 text-gray-800">Trip Report Detail</h3>

        <asp:Literal ID="ltMessage" runat="server" />

        <!-- Main content -->
        <div class="row">
            <div class="col-lg-12">

                <!-- Trip Report Detail Items -->
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="mb-3 d-flex justify-content-between">
                            <div>
                                <asp:Label Text="ID TRIP : #TRP-2024-" runat="server" Style="font-weight: 800" />
                                <asp:Label ID="lblTripID" runat="server" Style="font-weight: 800" />
                                <asp:Label ID="lblStatus" CssClass="badge rounded-pill bg-info text-white" Text="STATUS" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group mb-2">
                                    <strong>
                                        <asp:Label Text="Description    : " runat="server" /></strong>
                                    <asp:Label ID="lblDescription" runat="server" />
                                </div>
                                <div class="form-group mb-2">
                                    <strong>
                                        <asp:Label Text="Location   : " runat="server" /></strong>
                                    <asp:Label ID="lblLocation" runat="server" />
                                </div>
                                <div class="form-group mb-2">
                                    <strong>
                                        <asp:Label Text="Total Expense:" runat="server" /></strong>&nbsp;
                                    <asp:Label ID="lblTotalExpense" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group mb-2">
                                    <strong>
                                        <asp:Label Text="Start Date : " runat="server" /></strong>
                                    <asp:Label ID="lblStartDate" runat="server" />
                                </div>
                                <div class="form-group mb-2">
                                    <strong>
                                        <asp:Label Text="End Date   : " runat="server" /></strong>
                                    <asp:Label ID="lblEndDate" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Expenses Item Detail -->
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="mb-3 d-flex justify-content-between">
                            <div>
                                <span class="me-3">
                                    <asp:Label ID="lblTripIDDetail" Text="Expense Item Detail" Style="font-weight: 800" runat="server" />
                                </span>
                            </div>
                        </div>
                        <table class="table table-borderless table-responsive" width="100%" cellspacing="0">
                            <!-- ... (Table List View) -->
                            <asp:ListView ID="lvExpenseItems" runat="server" DataKeyNames="ExpenseID">
                                <LayoutTemplate>
                                    <table class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>Type</th>
                                                <th>Cost</th>
                                                <th>Description</th>
                                                <th>Receipt</th>
                                                <th>Approval</th>
                                            </tr>
                                        </thead>
                                        <tbody id="itemPlaceholder" runat="server"></tbody>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ExpenseType") %></td>
                                        <td><%# Eval("ItemCost", "{0:C}") %></td>
                                        <td><%# Eval("Description") %></td>
                                        <td>
                                            <asp:Image ImageUrl='<%# ResolveUrl("~/ReceiptImages/") + Eval("ReceiptImage") %>' runat="server" Width="100" CssClass="img-thumbnail" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkApprove" Text="Approve" CssClass="btn btn-outline-success btn-sm"
                                                CommandName="Approve" CommandArgument='<%# Eval("ExpenseID") %>'
                                                OnClientClick="return confirm('Apakah anda yakin untuk approve data ?')" runat="server" />
                                            <asp:LinkButton ID="lnkReject" Text="Reject" CssClass="btn btn-outline-danger btn-sm"
                                                CommandName="Reject" CommandArgument='<%# Eval("ExpenseID") %>'
                                                OnClientClick="return confirm('Apakah anda yakin untuk reject data ?')" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </table>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>

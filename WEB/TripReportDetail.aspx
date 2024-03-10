<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TripReportDetail.aspx.vb" Inherits="WEB.WebForm1" %>

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
                                <%-- button to adding new items expanse --%>
                                <button type="button" class="btn btn-success btn-sm" data-toggle="modal" data-target="#addExpenseModal">
                                    Add Expense Item
                                </button>
                            </div>
                            <div class="d-flex">
                                <div class="dropdown">
                                    <!-- Dropdown content -->
                                </div>
                            </div>
                        </div>
                        <table class="table table-borderless table-responsive" width="100%" cellspacing="0">
                            <!-- ... (Table List View) -->
                            <asp:ListView ID="lvExpenseItems" runat="server" DataKeyNames="ExpenseID"
                                OnSelectedIndexChanging="lvExpenseItems_SelectedIndexChanging"
                                OnSelectedIndexChanged="lvExpenseItems_SelectedIndexChanged"
                                OnItemCommand="lvExpenseItems_ItemCommand"
                                OnItemDeleting="lvExpenseItems_ItemDeleting">
                                <LayoutTemplate>
                                    <table class="table table-bordered table-striped">
                                        <thead>
                                            <tr>
                                                <th>ID</th>
                                                <th>Type</th>
                                                <th>Cost</th>
                                                <th>Description</th>
                                                <th>Receipt</th>
                                                <th>Actions</th>
                                            </tr>
                                        </thead>
                                        <tbody id="itemPlaceholder" runat="server"></tbody>

                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ExpenseID") %></td>
                                        <td><%# Eval("ExpenseType") %></td>
                                        <td><%# Eval("ItemCost", "{0:C}") %></td>
                                        <td><%# Eval("Description") %></td>
                                        <td>
                                            <asp:Image ImageUrl='<%# ResolveUrl("~/ReceiptImages/") + Eval("ReceiptImage") %>' runat="server" Width="100" CssClass="img-thumbnail" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkEdit" Text="Edit" CssClass="btn btn-outline-warning btn-sm"
                                                CommandName="Select" CommandArgument="Edit" runat="server" />
                                            <asp:LinkButton ID="lnkDelete" Text="Delete" CssClass="btn btn-outline-danger btn-sm"
                                                CommandName="Delete" CommandArgument='<%# Eval("ExpenseID") %>'
                                                OnClientClick="return confirm('Apakah anda yakin untuk delete data ?')" runat="server" />

                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </table>
                    </div>

                    <div class="card-footer">
                        <div class="total-expense">

                            <div id="divReimbursementButton" runat="server">
                                <asp:Button runat="server" ID="btnReimbursement" Text="Submit Reimbursement" CssClass="btn btn-primary" OnClick="btnReimbursement_Click" />
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <!-- Add Modal -->
    <div class="modal fade" id="addExpenseModal" tabindex="-1" role="dialog" aria-labelledby="addExpenseModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="addExpenseModalLabel">Add Expense Item</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="txtAddExpenseType">Expense Type:</label>
                        <asp:TextBox runat="server" ID="txtAddExpenseType" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label for="txtAddItemCost">Item Cost:</label>
                        <asp:TextBox runat="server" ID="txtAddItemCost" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label for="txtAddDescription">Description:</label>
                        <asp:TextBox runat="server" ID="txtAddDescription" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <label for="fuAddReceiptImage">Receipt Image:</label>
                        <asp:FileUpload runat="server" ID="fuAddReceiptImage" CssClass="form-control" />
                    </div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary btn-sm" data-dismiss="modal">Close</button>
                    <asp:Button runat="server" ID="btnAddExpenseItem" Text="Add Expense" CssClass="btn btn-primary btn-sm" OnClick="btnAddExpense_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Edit Modal -->
    <div class="modal fade" id="editExpenseModal" tabindex="-1" role="dialog" aria-labelledby="editExpenseModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="editExpenseModalLabel">Edit Expense Item</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <!-- Your Edit Form Goes Here -->
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="txtEditExpenseType">Expense Type:</label>
                            <asp:TextBox runat="server" ID="txtEditExpenseType" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <label for="txtEditItemCost">Item Cost:</label>
                            <asp:TextBox runat="server" ID="txtEditItemCost" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <label for="txtEditDescription">Description:</label>
                            <asp:TextBox runat="server" ID="txtEditDescription" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <label for="fuEditReceiptImage">Receipt Image:</label>
                            <%--Show the image here--%>
                            <asp:Image runat="server" ID="imgEditReceiptImage" CssClass="img-thumbnail" />
                            <asp:FileUpload runat="server" ID="fuEditReceiptImage" CssClass="form-control" />
                        </div>
                    </div>

                    <!-- Add other fields as needed -->
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button runat="server" ID="btnUpdateExpenseItem" Text="Save Changes" CssClass="btn btn-primary" OnClick="btnUpdateExpenseItem_Click" />
                </div>
            </div>
        </div>
    </div>


</asp:Content>

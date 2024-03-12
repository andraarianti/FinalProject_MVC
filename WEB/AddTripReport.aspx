<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AddTripReport.aspx.vb" Inherits="WEB.AddTripReport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <h2 class="mb-4">Add Trip Report</h2>
        <h5 class="mb-4">
            <asp:Literal ID="ltMessage" runat="server"></asp:Literal>
        </h5>

        <!-- Trip Form -->
        <asp:Panel runat="server" ID="pnlGeneralInfo">
            <!-- General Trip Information -->
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label for="txtSubmittedBy">Submitted By:</label>
                        <%-- Dropdown for submitted staff --%>
                        <asp:DropDownList runat="server" ID="ddlSubmittedBy" CssClass="form-control" DataValueField="StaffID" AppendDataBoundItems="True">
                            <asp:ListItem Text="-- Select Staff --" Value="" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="txtStartDate">Start Date:</label>
                        <asp:TextBox runat="server" ID="txtStartDate" CssClass="form-control" TextMode="Date" Required="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="txtEndDate">End Date:</label>
                        <asp:TextBox runat="server" ID="txtEndDate" CssClass="form-control" TextMode="Date" Required="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label for="txtLocation">Location:</label>
                <asp:TextBox runat="server" ID="txtLocation" CssClass="form-control" Required="true"></asp:TextBox>
            </div>
        </asp:Panel>
        <hr />
        <!-- Expense Items Section -->
        <h4>Expense Items</h4>
        <asp:Panel>
            <table class="table table-hover">
                <asp:ListView ID="lvExpenseItems" runat="server" ItemPlaceholderID="itemPlaceholder" ClientIDMode="Static">
                    <LayoutTemplate>
                        <thead>
                            <tr>
                                <th>Expense Type</th>
                                <th>Item Cost</th>
                                <th>Description</th>
                                <th>Receipt Image</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody id="expenseItemsTableBody" runat="server">
                            <tr id="itemPlaceholder" runat="server"></tr>
                        </tbody>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtExpenseType" CssClass="form-control" Required="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtItemCost" CssClass="form-control" Required="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" Required="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:FileUpload runat="server" ID="fuReceiptImage" CssClass="form-control" Required="true" />
                            </td>
                            <td>
                                <button type="button" class="btn btn-danger btn-sm" onclick="removeExpenseRow(this)">Remove</button>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </table>
        </asp:Panel>
        <!-- Add Button to Dynamically Add More Rows -->
        <button type="button" class="btn btn-primary btn-sm" id="addExpenseRow">Add Expense Item</button>
        <br />
        <br />
        <!-- Submit Button -->
        <asp:Button runat="server" ID="btnSubmit" Text="Submit Trip Report" CssClass="btn btn-success" OnClientClick="return validateForm();" OnClick="btnSubmit_Click" />
        <asp:Button runat="server" ID="btnDraft" Text="Save to Draft" CssClass="btn btn-success" OnClientClick="return validateForm();" OnClick="btnDraft_Click" />
        <br />
    </div>

    <script>
        document.getElementById('addExpenseRow').addEventListener('click', function () {
            // Get reference to the Expense Items table body
            var expenseItemsTableBody = document.getElementById('expenseItemsTableBody');

            if (!expenseItemsTableBody) {
                console.error("Elemen dengan ID 'expenseItemsTableBody' tidak ditemukan.");
                return;
            }

            // Create a new table row
            var newRow = expenseItemsTableBody.insertRow();

            // Add cells to the row
            var expenseTypeCell = newRow.insertCell(0);
            var itemCostCell = newRow.insertCell(1);
            var descriptionCell = newRow.insertCell(2);
            var receiptImageCell = newRow.insertCell(3);
            var actionsCell = newRow.insertCell(4);

            // Add input elements to cells
            var expenseTypeInput = document.createElement('input');
            expenseTypeInput.type = 'text';
            expenseTypeInput.className = 'form-control';
            expenseTypeInput.name = 'expenseType[]';
            expenseTypeInput.required = true;
            expenseTypeCell.appendChild(expenseTypeInput);

            var itemCostInput = document.createElement('input');
            itemCostInput.type = 'number';
            itemCostInput.className = 'form-control';
            itemCostInput.name = 'itemCost[]';
            itemCostInput.required = true;
            itemCostCell.appendChild(itemCostInput);

            var descriptionInput = document.createElement('input');
            descriptionInput.type = 'text';
            descriptionInput.className = 'form-control';
            descriptionInput.name = 'description[]';
            descriptionInput.required = true;
            descriptionCell.appendChild(descriptionInput);

            var receiptImageInput = document.createElement('input');
            receiptImageInput.type = 'file';
            receiptImageInput.className = 'form-control';
            receiptImageInput.name = 'receiptImage[]';
            receiptImageInput.accept = 'image/*';
            receiptImageInput.required = true;
            receiptImageCell.appendChild(receiptImageInput);

            var removeButton = document.createElement('button');
            removeButton.type = 'button';
            removeButton.className = 'btn btn-danger btn-sm';
            removeButton.textContent = 'Remove'; // Menggunakan textContent untuk teks
            removeButton.addEventListener('click', function () {
                removeExpenseRow(this);
            });
            actionsCell.appendChild(removeButton);
        });

        function removeExpenseRow(button) {
            // Get reference to the Expense Items table body
            var expenseItemsTableBody = document.getElementById('expenseItemsTableBody');

            if (!expenseItemsTableBody) {
                console.error("Elemen dengan ID 'expenseItemsTableBody' tidak ditemukan.");
                return;
            }

            // Get the row index of the button's parent row
            var rowIndex = button.parentNode.parentNode.rowIndex;

            // Remove the row from the table
            expenseItemsTableBody.deleteRow(rowIndex);
        }
    </script>


</asp:Content>

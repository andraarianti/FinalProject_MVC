<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="WEB._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section>
            <asp:Literal ID="ltlWelcome" runat="server"></asp:Literal>
        </section>
        <hr />
        <section class="row" aria-labelledby="appTitle">
            <h1 id="appTitle">Business Trip Expense</h1>
            <p class="lead">Manage your business trip expenses efficiently with our application. Submit, track, and approve expenses seamlessly.</p>
        </section>

        <a href="#" class="btn btn-primary btn-md">Get Started &raquo;</a>
        <hr />
        <div class="row">
            <section class="col-md-4" aria-labelledby="submitExpenseTitle">
                <h2 id="submitExpenseTitle">Submit Expenses</h2>
                <p>
                    Easily submit your business trip expenses with our user-friendly interface. Capture receipts and provide necessary details for quick approval.
                </p>
                <p>
                    <a class="btn btn-default" href="#">Learn more &raquo;</a>
                </p>
            </section>
            <section class="col-md-4" aria-labelledby="trackExpenseTitle">
                <h2 id="trackExpenseTitle">Track Expenses</h2>
                <p>
                    Track the status of your submitted expenses in real-time. Stay informed about the approval process and get updates on your reimbursement.
                </p>
                <p>
                    <a class="btn btn-default" href="#">Learn more &raquo;</a>
                </p>
            </section>
            <section class="col-md-4" aria-labelledby="approveExpenseTitle">
                <h2 id="approveExpenseTitle">Approve Expenses</h2>
                <p>
                    Approvers can efficiently review and approve business trip expenses. Streamline the approval workflow and ensure timely processing.
                </p>
                <p>
                    <a class="btn btn-default" href="#">Learn more &raquo;</a>
                </p>
            </section>
        </div>
    </main>

</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transactions.aspx.cs" Inherits="Merchant.Transactions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            <h1>All Transaction</h1>
            </br>
            <asp:GridView ID="grd_transaction" runat="server">
            </asp:GridView>
        </div>
        </br>
        <div>
            <h1>Previous Transaction</h1>
            <asp:GridView ID="grd_previous" runat="server">
            </asp:GridView>
        </div>
    </form>
</body>
</html>

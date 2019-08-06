<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="Merchant.Payment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css" rel="stylesheet" id="bootstrapcss">
    <script src="//netdna.bootstrapcdn.com/bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="container">
                <div class='row'>
                    <div class='col-md-4'>
                    </div>
                    <div class='col-md-4'>
                        <div class='form-row'>
                            <div class='col-xs-12 form-group required'>
                                <label class='control-label'>Checkout .Net Challenge. Submitted by Mr. Kaveersing Rajcoomar</label>
                            </div>
                        </div>
                        <div style="padding: 50px">
                        </div>
                        <div class='form-row'>
                            <div class='col-xs-12 form-group required'>
                                <label class='control-label'>Amount</label>
                                <asp:TextBox ID="txt_amount" CssClass="form-control" size='4' type='text' runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class='form-row'>
                            <div class='col-xs-12 form-group card required'>
                                <label class='control-label'>Card Number</label>
                                <asp:TextBox ID="txt_card_number" CssClass="form-control card-number" size='20' type='text' runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class='form-row'>
                            <div class='col-xs-4 form-group cvc required'>
                                <label class='control-label'>CVC</label>
                                <asp:TextBox ID="txt_cvc" CssClass="form-control card-cvc"  placeholder="ex. 311" size='4' type='text' runat="server"></asp:TextBox>
                            </div>
                            <div class='col-xs-4 form-group expiration required'>
                                <label class='control-label'>Exp Month</label>
                                <asp:TextBox ID="txt_expiry_month" CssClass="form-control card-expiry-year"  placeholder="MM" size='2' type='text' runat="server"></asp:TextBox>

                            </div>
                            <div class='col-xs-4 form-group expiration required'>
                                <label class='control-label'>Exp Year</label>
                                <asp:TextBox ID="txt_expiry_year" CssClass="form-control card-expiry-year"  placeholder="YYYY" size='4' type='text' runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class='form-row'>
                            <div class='col-md-12 form-group'>
                                <asp:Button ID="btn_submit" CssClass="form-control btn btn-primary submit-button" runat="server" Text="Pay" />
                            </div>
                        </div>

                        <div class='form-row'>
                            <asp:Panel ID="pnl_sucess" runat="server">
                                <div class='col-md-12 success form-group'>
                                    <div class='alert-success alert'>
                                        Message:
                                        <asp:Label ID="lbl_success_message" runat="server" Text="Label"></asp:Label>
                                        </br>
                                        Details:
                                        <asp:Label ID="lbl_success_detail" runat="server" Text="Label"></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnl_fail" runat="server">
                                <div class='col-md-12 error form-group'>
                                    <div class='alert-danger alert'>
                                        Message:
                                        <asp:Label ID="lbl_fail_message" runat="server" Text="Label"></asp:Label>
                                        </br>
                                        Details:
                                        <asp:Label ID="lbl_fail_details" runat="server" Text="Label"></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class='col-md-4'>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

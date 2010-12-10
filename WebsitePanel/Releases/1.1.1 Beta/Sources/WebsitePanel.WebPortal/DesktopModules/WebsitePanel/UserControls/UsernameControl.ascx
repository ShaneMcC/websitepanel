<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsernameControl.ascx.cs" Inherits="WebsitePanel.Portal.UsernameControl" %>
<asp:Literal ID="litPrefix" runat="server"></asp:Literal>
<asp:textbox id="txtName" runat="server" CssClass="NormalTextBox" Width="200px" MaxLength="100" style="vertical-align:middle;"></asp:textbox>
<asp:Label ID="lblName" runat="server" style="line-height: 22px;" CssClass="Huge"></asp:Label>
<asp:Literal ID="litSuffix" runat="server"></asp:Literal>
<asp:RequiredFieldValidator id="valRequireUsername" runat="server" ErrorMessage="*"
    ControlToValidate="txtName" Display="Dynamic" CssClass="NormalBold"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator id="valCorrectUsername" runat="server" Display="Dynamic"
    ControlToValidate="txtName" ErrorMessage="Only letters, numbers and '_', '-', '.' symbols are allowed"
	ValidationExpression="^[a-zA-Z0-9_\.\-]{1,20}$" CssClass="NormalBold"></asp:RegularExpressionValidator>
<asp:RegularExpressionValidator id="valCorrectMinLength" runat="server" Display="Dynamic"
    ControlToValidate="txtName" ErrorMessage="Min length"
	ValidationExpression="^.{3,}$" CssClass="NormalBold"></asp:RegularExpressionValidator>
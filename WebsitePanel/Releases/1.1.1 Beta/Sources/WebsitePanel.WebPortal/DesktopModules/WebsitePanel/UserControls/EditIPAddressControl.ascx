<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditIPAddressControl.ascx.cs" Inherits="WebsitePanel.Portal.UserControls.EditIPAddressControl" %>
<asp:TextBox ID="txtAddress" runat="server" Width="110px" MaxLength="15" CssClass="NormalTextBox"></asp:TextBox>
<asp:RequiredFieldValidator ID="requireAddressValidator" runat="server" meta:resourcekey="requireAddressValidator"
    ControlToValidate="txtAddress" SetFocusOnError="true" Text="*" Enabled="false" Display="Dynamic">
</asp:RequiredFieldValidator><asp:RegularExpressionValidator id="addressValidator" runat="server"
    ValidationExpression="^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$"
    Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAddress" Text="*" meta:resourcekey="addressValidator">
</asp:RegularExpressionValidator>
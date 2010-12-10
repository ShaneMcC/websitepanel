<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleInterval.ascx.cs" Inherits="WebsitePanel.Portal.ScheduleInterval" %>
<asp:TextBox ID="txtInterval" runat="server" CssClass="NormalTextBox" Width="40px"></asp:TextBox>
<asp:DropDownList ID="ddlUnits" runat="server" resourcekey="ddlUnits"  CssClass="NormalDropDown">
    <asp:ListItem Value="Days">Days</asp:ListItem>
    <asp:ListItem Value="Hours">Hours</asp:ListItem>
    <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
    <asp:ListItem Value="Seconds">Seconds</asp:ListItem>
</asp:DropDownList>
<asp:RequiredFieldValidator id="valRequireInterval" runat="server" ErrorMessage="*" ControlToValidate="txtInterval" Display="Dynamic"></asp:RequiredFieldValidator>

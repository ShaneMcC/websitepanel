<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DaysBox.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.UserControls.DaysBox" %>
<asp:TextBox ID="txtValue" runat="server" CssClass="TextBox50" MaxLength="3"></asp:TextBox>
<asp:Localize ID="locDays" runat="server" meta:resourcekey="locDays" Text="days"></asp:Localize>
<asp:RegularExpressionValidator ID="valRequireCorrectNumber" runat="server" meta:resourcekey="valRequireCorrectNumber"
	ErrorMessage="Enter correct number" ControlToValidate="txtValue" Display="Dynamic" ValidationExpression="[0-9]{1,3}" SetFocusOnError="True"></asp:RegularExpressionValidator>
<asp:RequiredFieldValidator ID="valRequireNumber" runat="server" ControlToValidate="txtValue" meta:resourcekey="valRequireNumber"
	Display="Dynamic" ErrorMessage="The number of days could not be left blank" SetFocusOnError="True"></asp:RequiredFieldValidator>

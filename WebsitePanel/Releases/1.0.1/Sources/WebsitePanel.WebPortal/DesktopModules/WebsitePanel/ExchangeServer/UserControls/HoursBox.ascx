<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HoursBox.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.UserControls.HoursBox" %>
<asp:TextBox ID="txtValue" runat="server" CssClass="TextBox100" MaxLength="15"></asp:TextBox>
<asp:Localize ID="locHours" runat="server" meta:resourcekey="locHours" Text="Hours"></asp:Localize>
<asp:RangeValidator  ErrorMessage="*"  ID="valRangeHours" runat="server"  Display="Dynamic"
Type="Integer" MinimumValue="0" MaximumValue="596523" meta:resourcekey="valRangeHours" ControlToValidate="txtValue" SetFocusOnError="true" />

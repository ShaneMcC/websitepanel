<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchBox.ascx.cs" Inherits="WebsitePanel.Portal.SearchBox" %>
<asp:Panel ID="tblSearch" runat="server" DefaultButton="cmdSearch" CssClass="NormalBold">
<asp:Label ID="lblSearch" runat="server" meta:resourcekey="lblSearch" Visible="false"></asp:Label>
    <asp:DropDownList ID="ddlFilterColumn" runat="server" CssClass="NormalTextBox" style="vertical-align: middle;">
    </asp:DropDownList><asp:TextBox ID="txtFilterValue" runat="server" CssClass="NormalTextBox" Width="100" style="vertical-align: middle;"></asp:TextBox><asp:ImageButton ID="cmdSearch" Runat="server" meta:resourcekey="cmdSearch" SkinID="SearchButton"
		CausesValidation="false" style="vertical-align: middle;"/>
</asp:Panel>
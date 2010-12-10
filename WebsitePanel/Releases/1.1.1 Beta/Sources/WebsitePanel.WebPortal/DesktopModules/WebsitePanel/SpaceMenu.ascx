<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceMenu.ascx.cs" Inherits="WebsitePanel.Portal.SpaceMenu" %>
<div class="MenuHeader">
    <asp:Localize ID="locMenuTitle" runat="server" meta:resourcekey="locMenuTitle"></asp:Localize>
</div>
<div class="Menu">
    <asp:Menu ID="menu" runat="server"
        Orientation="Vertical"
        EnableViewState="false"
        CssSelectorClass="LeftMenu" >
    </asp:Menu>
</div>
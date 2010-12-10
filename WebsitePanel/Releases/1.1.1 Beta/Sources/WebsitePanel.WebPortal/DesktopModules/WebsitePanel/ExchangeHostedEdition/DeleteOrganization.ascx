<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeleteOrganization.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeHostedEdition.DeleteOrganization" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="FormBody">
    <wsp:SimpleMessageBox ID="messageBox" runat="server" />

    <div>
        <asp:Localize ID="locWarningMessage" runat="server" meta:resourcekey="locWarningMessage"></asp:Localize>
    </div>

    <asp:CheckBox ID="confirmDelete" runat="server" meta:resourcekey="confirmDelete" Text="I confirm deletion of this organization and all its contents" />
</div>
<div class="FormFooter">
    <asp:Button ID="delete" runat="server" meta:resourcekey="delete" Text="Delete" 
        CssClass="Button1" onclick="delete_Click" />
    <asp:Button ID="cancel" runat="server" meta:resourcekey="cancel" Text="Cancel" 
        CssClass="Button1" CausesValidation="false" onclick="cancel_Click" />
</div>
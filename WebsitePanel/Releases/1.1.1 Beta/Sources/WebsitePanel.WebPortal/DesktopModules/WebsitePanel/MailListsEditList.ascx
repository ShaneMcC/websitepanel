<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailListsEditList.ascx.cs" Inherits="WebsitePanel.Portal.MailListsEditList" %>
<%@ Register TagPrefix="dnc" TagName="EmailAddress" Src="MailEditAddress.ascx" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>


<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
   if (!confirm("Are you sure you want to delete this Mail List?")) return false; else ShowProgressDialog('Deleting Mail List...');
}
</script>

<div class="FormBody">
    <dnc:EmailAddress id="emailAddress" runat="server"></dnc:EmailAddress>
    <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
</div>
<div class="FormFooter">
    <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="Button1" Text="Save" OnClick="btnSave_Click" onClientClick = "ShowProgressDialog('Saving Mail List...');"/>
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" CausesValidation="false" 
        Text="Cancel" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1" CausesValidation="false" 
        Text="Delete" OnClientClick="return confirmation();" OnClick="btnDelete_Click" />
</div>
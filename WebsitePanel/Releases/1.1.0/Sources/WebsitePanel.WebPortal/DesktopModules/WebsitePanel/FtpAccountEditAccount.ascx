<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FtpAccountEditAccount.ascx.cs" Inherits="WebsitePanel.Portal.FtpAccountEditAccount" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc4" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc2" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>


<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
	if (!confirm("Are you sure you want to delete this FTP Account?")) return false; else ShowProgressDialog('Deleting FTP Account...');
}
</script>


<div class="FormBody">
    <table cellSpacing="0" cellPadding="4" width="100%">
	    <tr>
		    <td class="SubHead" style="width:150px;"><asp:Label ID="lblUserName" runat="server" meta:resourcekey="lblUserName" Text="User name:"></asp:Label></td>
		    <td class="Normal">
                <uc4:UsernameControl ID="usernameControl" runat="server" />
            </td>
	    </tr>
	    <tr>
		    <td class="SubHead" valign="top"><asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Password:"></asp:Label></td>
		    <td class="Normal">
                <uc3:PasswordControl ID="passwordControl" runat="server" />
            </td>
	    </tr>
	    <tr>
		    <td class="SubHead" height="30"><asp:Label ID="lblHomeFolder" runat="server" meta:resourcekey="lblHomeFolder" Text="Home folder:"></asp:Label></td>
		    <td class="Normal" height="30">
                <uc2:FileLookup ID="fileLookup" runat="server" Width="300" />
		    </td>
	    </tr>
    </table>
    <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
</div>
<div class="FormFooter">
	<asp:Button ID="btnSave" runat="server" CssClass="Button1" meta:resourcekey="btnSave" Text="Save" OnClick="btnSave_Click" OnClientClick = "ShowProgressDialog('Saving FTP Account...');"/>
    <asp:Button ID="btnCancel" runat="server" CssClass="Button1" meta:resourcekey="btnCancel" CausesValidation="false" 
        Text="Cancel" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" CssClass="Button1" meta:resourcekey="btnDelete" CausesValidation="false" 
        Text="Delete" OnClick="btnDelete_Click" OnClientClick="return confirmation();"/>
</div>
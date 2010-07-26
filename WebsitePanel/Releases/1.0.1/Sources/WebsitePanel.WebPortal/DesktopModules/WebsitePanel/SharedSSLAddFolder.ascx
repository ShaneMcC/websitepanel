<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedSSLAddFolder.ascx.cs" Inherits="WebsitePanel.Portal.SharedSSLAddFolder" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="FormBody">
    <table cellSpacing="0" cellPadding="3">
	    <tr>
		    <td class="SubHead" style="width:150px;">
		        <asp:Label ID="lblDomain" runat="server" meta:resourcekey="lblDomain" Text="Domain:"></asp:Label>
		    </td>
		    <td class="Normal">
		        <asp:DropDownList ID="ddlDomains" runat="server" CssClass="NormalTextBox">
		        </asp:DropDownList>
                <asp:RequiredFieldValidator ID="valRequireDomain" runat="server" meta:resourcekey="valRequireDomain"
                    ErrorMessage="*" ControlToValidate="ddlDomains" Display="Dynamic"></asp:RequiredFieldValidator></td>
	    </tr>
	    <tr>
    		<td>&nbsp;</td>
	    </tr>
	    <tr>
		    <td class="SubHead" valign="top">
		        <asp:Label ID="lblWebSites" runat="server" meta:resourcekey="lblWebSites" Text="Web Site:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:DropDownList ID="ddlWebSites" runat="server" CssClass="NormalTextBox">
                </asp:DropDownList>&nbsp;&nbsp;
                <asp:RequiredFieldValidator ID="valRequireWebSite" runat="server" meta:resourcekey="valRequireWebSite"
                    ErrorMessage="*" ControlToValidate="ddlWebSites" Display="Dynamic"></asp:RequiredFieldValidator><br />
                <asp:Label ID="lblWebSitesComment" runat="server" meta:resourcekey="lblWebSitesComment" Text="* Created virtual directory will use security settings from this web site"></asp:Label>

	        </td>
	    </tr>
	    <tr>
    		<td>&nbsp;</td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Label ID="lblDirectoryName" runat="server" meta:resourcekey="lblDirectoryName" Text="Directory name:"></asp:Label>
		    </td>
		    <td>
		        <uc2:UsernameControl ID="virtDirName" runat="server" />
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead" valign="top" style="padding-top: 7px;">
			    <asp:Label ID="lblFolder" runat="server" meta:resourcekey="lblFolder" Text="Folder:"></asp:Label>
	        </td>
		    <td class="Normal" valign="top">
                <uc1:FileLookup ID="fileLookup" runat="server" ValidationEnabled="true" Width="300" />
    			
		    </td>
	    </tr>
    </table>
</div>
<div class="FormFooter">
    <asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" CssClass="Button3" Text="Create Directory" OnClick="btnAdd_Click" OnclientClick="ShowProgressDialog('Creating Shared SSL Folder...')"/>
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click"  />
</div>
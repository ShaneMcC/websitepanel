<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesAddVirtualDir.ascx.cs" Inherits="WebsitePanel.Portal.WebSitesAddVirtualDir" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="FormBody">
    <table cellSpacing="0" cellPadding="3" width="600">
	    <tr>
		    <td class="SubHead" width="200" nowrap>
		        <asp:Label ID="lblDirectoryName" runat="server" meta:resourcekey="lblDirectoryName" Text="Directory name:"></asp:Label>
		    </td>
		    <td width="100%">
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
    <asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" CssClass="Button3" Text="Create Directory" OnClick="btnAdd_Click" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click"  />
</div>
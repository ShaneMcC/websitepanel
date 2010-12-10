<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MSFTP70_Settings.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.MSFTP70_Settings" %>
<%@ Register Src="Common_ActiveDirectoryIntegration.ascx" TagName="ActiveDirectoryIntegration" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>
<table cellpadding="4" cellspacing="0" width="100%">
	<tr>
		<td class="Normal" width="200" nowrap>
		    <asp:Label ID="lblSharedIP" runat="server" meta:resourcekey="lblSharedIP" Text="Web Sites Shared IP Address:"></asp:Label>
		</td>
		<td width="100%">
            <uc1:SelectIPAddress ID="ipAddress" runat="server" ServerIdParam="ServerID" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" width="200" nowrap>
		    <asp:Label ID="lblSite" runat="server" meta:resourcekey="lblSite" Text="FTP Accounts Site:"></asp:Label>
		</td>
		<td width="100%">
            <asp:TextBox ID="txtSiteId" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
        </td>
	</tr>
	<tr>
	    <td class="SubHead">
	        <asp:Label ID="lblGroupName" runat="server" meta:resourcekey="lblGroupName" Text="FTP Users Group Name:"></asp:Label>
	    </td>
	    <td class="Normal">
            <asp:TextBox ID="txtFtpGroupName" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblBuildUncFilesPath" runat="server" meta:resourcekey="lblBuildUncFilesPath" Text="Build UNC Path to Space Files:"></asp:Label>
		</td>
		<td>
			<asp:CheckBox ID="chkBuildUncFilesPath" runat="server" meta:resourcekey="chkBuildUncFilesPath" Text="Yes" />
		</td>
	</tr>
	<tr>
	    <td class="SubHead">
	        <asp:Label ID="lblADIntegration" runat="server" meta:resourcekey="lblADIntegration" Text="Active Directory Integration:"></asp:Label>    
	    </td>
	    <td class="Normal">
            <uc1:ActiveDirectoryIntegration ID="ActiveDirectoryIntegration" runat="server" />

        </td>
	</tr>
</table>
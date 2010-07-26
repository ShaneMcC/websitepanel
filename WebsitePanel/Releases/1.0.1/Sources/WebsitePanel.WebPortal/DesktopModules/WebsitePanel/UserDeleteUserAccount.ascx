<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDeleteUserAccount.ascx.cs" Inherits="WebsitePanel.Portal.UserDeleteUserAccount" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="FormBody">
<table width="400">
	<tr>
		<td class="Normal">
			<asp:Label ID="lblAccountContains" runat="server" meta:resourcekey="lblAccountContains" Text="The account contains ..."></asp:Label>
			<br/>
			<br/>
			<asp:GridView ID="gvPackages" Runat="server" Width="100%" AutoGenerateColumns="False"
			    EmptyDataText="gvPackages"
			    CssSelectorClass="NormalGridView">
				<columns>
					<asp:BoundField DataField="PackageName" HeaderText="gvPackagesPackage"></asp:BoundField>
				</columns>
			</asp:GridView>
			<br/>
		</td>
	</tr>
	<tr>
		<td class="Normal">
		    <asp:Label ID="lblWarning" runat="server" meta:resourcekey="lblWarning" Text="Deleting this user also..."></asp:Label>
			<br/>
			<br/>
		</td>
	</tr>
	<tr>
		<td class="Normal">
			<asp:CheckBox ID="chkConfirm" Runat="server" meta:resourcekey="chkConfirm" Text="Yes, I understand ..."></asp:CheckBox>
			<br/>
		</td>
	</tr>
</table>
</div>
<div class="FormFooter">
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" Text="Delete" CssClass="Button1" OnClick="btnDelete_Click" OnClientClick="ShowProgressDialog('Deleting...');"/>
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel" CssClass="Button1" OnClick="btnCancel_Click" />
</div>
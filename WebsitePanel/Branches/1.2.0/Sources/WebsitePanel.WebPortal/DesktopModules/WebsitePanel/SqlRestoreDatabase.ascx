<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlRestoreDatabase.ascx.cs" Inherits="WebsitePanel.Portal.SqlRestoreDatabase" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<div class="FormBody">
    <div class="Huge">
        <asp:Literal id="litDatabaseName" runat="server"></asp:Literal>
    </div>
    <br />
    <div class="FormRow">
        <asp:Label ID="lblRestoreFrom" runat="server" meta:resourcekey="lblRestoreFrom" Text="Restore From:"></asp:Label>
    </div>
    
	<table width=100%>
		<tr>
			<td class="Normal"><asp:radiobutton id="radioUpload" meta:resourcekey="radioUpload" Checked="True" GroupName="media" Text="Uploaded File" Runat="server"
					AutoPostBack="True" OnCheckedChanged="radioUpload_CheckedChanged"></asp:radiobutton></td>
		</tr>
		<tr>
			<td class="Normal"><asp:radiobutton id="radioFile" meta:resourcekey="radioFile" GroupName="media" Text="Hosting Space File" Runat="server"
					AutoPostBack="True" OnCheckedChanged="radioUpload_CheckedChanged"></asp:radiobutton></td>
		</tr>
		<tr>
			<td class="Normal" id="cellUploadFile" runat="server">
				<table width=100%>
					<tr>
						<td>
                            <asp:FileUpload ID="uploadFile" runat="server" Width="300px" /></td>
					</tr>
					<tr>
						<td class="Small" nowrap><asp:Label ID="lblAllowedFiles1" runat="server" meta:resourcekey="lblAllowedFiles" Text=".ZIP, .BAK and .SQL files are allowed"></asp:Label></td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td class="Normal" id="cellFile" runat="server">
				<table width=100%>
					<tr>
						<td>
                            <uc1:FileLookup ID="fileLookup" runat="server" Width="300" IncludeFiles="true" />
                        </td>
					</tr>
					<tr>
						<td class="Small" nowrap><asp:Label ID="lblAllowedFiles2" runat="server" meta:resourcekey="lblAllowedFiles" Text=".ZIP, .BAK and .SQL files are allowed"></asp:Label></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
<div class="FormFooter">
	<asp:Button ID="btnRestore" runat="server" meta:resourcekey="btnRestore" CssClass="Button1" Text="Restore" OnClick="btnRestore_Click" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" CausesValidation="false" 
        Text="Cancel" OnClick="btnCancel_Click" />
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesEditWebFolder.ascx.cs" Inherits="WebsitePanel.Portal.WebSitesEditWebFolder" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<div class="FormBody">
<table cellSpacing="0" cellPadding="0" width="100%">
	<tr>
		<td>
            <table cellSpacing="0" cellPadding="5" width="100%">
	            <tr>
		            <td class="SubHead" style="width: 150px;">
						<asp:Label ID="lblFolderTitle" runat="server" meta:resourcekey="lblFolderTitle" Text="Folder Title:"></asp:Label>
					</td>
		            <td class="NormalBold">
                        <asp:TextBox ID="txtTitle" runat="server" Width="200" CssClass="NormalTextBox"></asp:TextBox>
                    </td>
	            </tr>
	            <tr>
		            <td class="SubHead"><asp:Label ID="lblFolderName" runat="server" meta:resourcekey="lblFolderName" Text="Folder Path:"></asp:Label></td>
		            <td class="NormalBold">
                        <uc1:FileLookup id="folderPath" runat="server" Width="250">
                        </uc1:FileLookup></td>
	            </tr>
            </table>
            
            <wsp:CollapsiblePanel id="secUsers" runat="server"
                TargetControlID="UsersPanel" meta:resourcekey="secUsers" Text="Allowed Users">
            </wsp:CollapsiblePanel>
	        <asp:Panel ID="UsersPanel" runat="server" Height="0" style="overflow:hidden;">
                <table cellspacing="0" cellpadding="3" width="100%">
	                <tr>
		                <td colspan="2">
			                <asp:checkboxlist id="dlUsers" CellPadding="3" RepeatColumns="2" CssClass="NormalBold" Runat="server"
				                DataValueField="Name" DataTextField="Name"></asp:checkboxlist>
		                </td>
	                </tr>
                </table>
            </asp:Panel>
            
            <wsp:CollapsiblePanel id="secGroups" runat="server"
                TargetControlID="GroupsPanel" meta:resourcekey="secGroups" Text="Allowed Groups">
            </wsp:CollapsiblePanel>
	        <asp:Panel ID="GroupsPanel" runat="server" Height="0" style="overflow:hidden;">
                <table cellspacing="0" cellpadding="3" width="100%">
	                <tr>
		                <td colspan="2">
			                <asp:checkboxlist id="dlGroups" CellPadding="3" RepeatColumns="2" CssClass="NormalBold" Runat="server"
				                DataValueField="Name" DataTextField="Name"></asp:checkboxlist>
		                </td>
	                </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
</div>
<div class="FormFooter">
    <asp:Button ID="btnUpdate" runat="server" Text="Update" meta:resourcekey="btnUpdate" CssClass="Button1" OnClick="btnUpdate_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" meta:resourcekey="btnCancel" CssClass="Button1" CausesValidation="false" OnClick="btnCancel_Click" />
 </div>
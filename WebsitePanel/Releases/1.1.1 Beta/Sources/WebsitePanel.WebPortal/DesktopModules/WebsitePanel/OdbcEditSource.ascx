<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OdbcEditSource.ascx.cs" Inherits="WebsitePanel.Portal.OdbcEditSource" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc2" %>
<div class="FormBody">
<table cellSpacing="0" cellPadding="4" width="100%">
	<tr>
		<td class="SubHead" style="width:150px;">
		    <asp:Label ID="lblSourceName" runat="server" meta:resourcekey="lblSourceName" Text="Data Source Name:"></asp:Label>
		</td>
		<td class="NormalBold">
		    <uc3:UsernameControl ID="dsnName" runat="server" />
        </td>
	</tr>
	<tr>
		<td class="SubHead" valign="top"><asp:Label ID="lblDriver" runat="server" meta:resourcekey="lblDriver" Text="ODBC Driver:"></asp:Label></td>
		<td class="NormalBold">
            <asp:DropDownList ID="ddlDriver" runat="server" CssClass="NormalTextBox" resourcekey="ddlDriver"
                AutoPostBack="true" OnSelectedIndexChanged="ddlDriver_SelectedIndexChanged">
                <asp:ListItem Value="">Select</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="valRequireDriver" runat="server" ControlToValidate="ddlDriver"
                ErrorMessage="*"></asp:RequiredFieldValidator></td>
	</tr>
	<tr id="rowDatabaseName" runat="server">
		<td class="SubHead" valign="top"><asp:Label ID="lblDatabaseName" runat="server" meta:resourcekey="lblDatabaseName"></asp:Label></td>
		<td class="NormalBold">
            <asp:DropDownList ID="ddlDatabaseName" runat="server" CssClass="NormalTextBox"
                DataValueField="Name" DataTextField="Name">
            </asp:DropDownList>&nbsp;
            <asp:RequiredFieldValidator ID="valRequireDatabase" runat="server" ControlToValidate="ddlDatabaseName"
                ErrorMessage="*"></asp:RequiredFieldValidator></td>
	</tr>
	<tr id="rowFile" runat="server">
		<td class="SubHead" height="30"><asp:Label ID="lblFile" runat="server" meta:resourcekey="lblFile"></asp:Label></td>
		<td class="Normal" height="30">
            <uc2:FileLookup ID="fileLookup" runat="server" Width="300" IncludeFiles="true" />
		</td>
	</tr>
	<tr id="rowDatabaseUser" runat="server">
		<td class="SubHead" valign="top"><asp:Label ID="lblDatabaseUser" runat="server" meta:resourcekey="lblDatabaseUser"></asp:Label></td>
		<td class="NormalBold">
            <asp:DropDownList ID="ddlDatabaseUser" runat="server" CssClass="NormalTextBox"
                DataValueField="Name" DataTextField="Name">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="valRequireDatabaseUser" runat="server" ControlToValidate="ddlDatabaseUser"
                ErrorMessage="*"></asp:RequiredFieldValidator></td>
	</tr>
	<tr id="rowUser" runat="server">
		<td class="SubHead" height="30"><asp:Label ID="lblUser" runat="server" meta:resourcekey="lblUser"></asp:Label></td>
		<td class="Normal" height="30">
			<asp:TextBox ID="txtUser" runat="server" CssClass="NormalTextBox"></asp:TextBox>
		</td>
	</tr>
	<tr id="rowPassword" runat="server">
		<td class="SubHead" valign="top"><asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword"></asp:Label></td>
		<td class="NormalBold">
            <uc3:PasswordControl ID="passwordControl" runat="server" ValidationEnabled="false" />
        </td>
	</tr>
</table>
</div>
<div class="FormFooter">
	<asp:Button ID="btnSave" runat="server" CssClass="Button1" meta:resourcekey="btnSave" Text="Save" OnClick="btnSave_Click" />
    <asp:Button ID="btnCancel" runat="server" CssClass="Button1" meta:resourcekey="btnCancel" CausesValidation="false" 
        Text="Cancel" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" CssClass="Button1" meta:resourcekey="btnDelete" CausesValidation="false" 
        Text="Delete" OnClientClick="return confirm('Delete this ODBC DSN?');" OnClick="btnDelete_Click" />
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceEditAddon.ascx.cs" Inherits="WebsitePanel.Portal.SpaceEditAddon" %>
<%@ Register Src="UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="wsp" %>

<div class="FormBody">
<asp:Label ID="lblMessage" runat="server" CssClass="NormalBold" ForeColor="red"></asp:Label>
<table cellspacing="0" cellpadding="3" width="100%">
	<tr>
		<td class="SubHead" noWrap width="200"><asp:Label ID="lblAddon" runat="server" meta:resourcekey="lblAddon" Text="Add-on:"></asp:Label></td>
		<td class="Normal" width="100%">
			<asp:DropDownList id="ddlPlan" runat="server" CssClass="NormalTextBox" DataTextField="PlanName" DataValueField="PlanID"></asp:DropDownList>
			<asp:RequiredFieldValidator id="planValidator" CssClass="NormalBold" runat="server" ErrorMessage="*"
				Display="Dynamic" ControlToValidate="ddlPlan" ValidationGroup="EditAddon"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="SubHead"><asp:Label ID="lblQuantity" runat="server" meta:resourcekey="lblQuantity" Text="Quantity:"></asp:Label></td>
		<td class="Normal">
			<asp:TextBox ID="txtQuantity" runat="server" CssClass="NormalTextBox" Width="50px">1</asp:TextBox>
            <asp:RequiredFieldValidator ID="valQuantity" runat="server" ControlToValidate="txtQuantity"
                CssClass="NormalBold" Display="Dynamic" ErrorMessage="*" ValidationGroup="EditAddon"></asp:RequiredFieldValidator></td>
	</tr>
    <tr>
        <td class="SubHead"><asp:Label ID="lblStatus" runat="server" meta:resourcekey="lblStatus" Text="Space Status:"></asp:Label></td>
        <td class="Normal">
            <asp:DropDownList id="ddlStatus" runat="server" resourcekey="ddlStatus" CssClass="NormalTextBox">
                <asp:ListItem Value="1">Active</asp:ListItem>
                <asp:ListItem Value="2">Suspended</asp:ListItem>
                <asp:ListItem Value="3">Cancelled</asp:ListItem>
                <asp:ListItem Value="4">New</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
	<tr>
		<td class="SubHead">
            <asp:Label ID="lblCreationDate" runat="server" meta:resourcekey="lblCreationDate" Text="Creation date:"></asp:Label></td>
		<td class="Normal">
			<wsp:CalendarControl id="PurchaseDate" runat="server" ValidationEnabled="true" ValidationGroup="EditAddon" />
		</td>
	</tr>
	<tr>
		<td class="SubHead"><asp:Label ID="lblComments" runat="server" meta:resourcekey="lblComments" Text="Comments:"></asp:Label></td>
		<td class="Normal">
			<asp:TextBox id="txtComments" runat="server" CssClass="NormalTextBox" Columns="40" Width="300px"
				Rows="3" TextMode="MultiLine"></asp:TextBox>
		</td>
	</tr>
</table>
</div>
<div class="FormFooter">
    <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="Button1" Text="Save" OnClick="btnSave_Click" ValidationGroup="EditAddon" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1" Text="Delete" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete add-on?');" />
</div> 
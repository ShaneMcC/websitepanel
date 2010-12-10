<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactDetails.ascx.cs" Inherits="WebsitePanel.Portal.ContactDetails" %>
<table>
	<tr>
		<td class="SubHead" style="width:200px;">
			<asp:Label ID="lblCompanyName" runat="server" meta:resourcekey="lblCompanyName" Text="Company Name:"></asp:Label>
		</td>
		<td class="Normal">
			<asp:TextBox id="txtCompanyName" runat="server" Columns="40" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Label ID="lblAddress" runat="server" meta:resourcekey="lblAddress" Text="Address:"></asp:Label>
		</td>
		<td class="Normal">
			<asp:TextBox id="txtAddress" runat="server" Columns="40" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Label ID="lblCity" runat="server" meta:resourcekey="lblCity" Text="City:"></asp:Label>
		</td>
		<td class="Normal">
			<asp:TextBox id="txtCity" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Label ID="lblCountry" runat="server" meta:resourcekey="lblCountry" Text="Country:"></asp:Label>
		</td>
		<td class="Normal">
			<asp:dropdownlist runat="server" id="ddlCountry" cssclass="NormalTextBox" autopostback="True" width="200px" onselectedindexchanged="ddlCountry_SelectedIndexChanged" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Label ID="lblState" runat="server" meta:resourcekey="lblState" Text="State:"></asp:Label>
		</td>
		<td class="Normal">
			<asp:TextBox id="txtState" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
			<asp:DropDownList ID="ddlStates" Runat="server" DataTextField="Text" DataValueField="Value" CssClass="NormalTextBox"
				Width="200px"></asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Label ID="lblZip" runat="server" meta:resourcekey="lblZip" Text="Zip:"></asp:Label>
		</td>
		<td class="Normal">
			<asp:TextBox id="txtZip" runat="server" Columns="10" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="normal">&nbsp;</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Label ID="lblPrimaryPhone" runat="server" meta:resourcekey="lblPrimaryPhone" Text="Primary phone:"></asp:Label>
		</td>
		<td class="Normal">
			<asp:TextBox id="txtPrimaryPhone" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Label ID="lblSecPhone" runat="server" meta:resourcekey="lblSecPhone" Text="Secondary phone:"></asp:Label>
		</td>
		<td class="Normal">
			<asp:TextBox id="txtSecondaryPhone" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Label ID="lblFax" runat="server" meta:resourcekey="lblFax" Text="Fax:"></asp:Label>
		</td>
		<td class="Normal">
			<asp:TextBox id="txtFax" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Label ID="lblIM" runat="server" meta:resourcekey="lblIM" Text="Instant messenger ID:"></asp:Label>
		</td>
		<td class="Normal">
			<asp:TextBox id="txtMessengerId" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
		</td>
	</tr>
</table>

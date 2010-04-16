<%@ Control Language="C#" AutoEventWireup="true" Codebehind="CategoriesEditCategory.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.CategoriesEditCategory" %>
<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclCommonFields" /></div>
</div>
<div class="FormBody">
	<table cellspacing="0" cellpadding="3">
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclCategoryName" meta:resourcekey="lclCategoryName" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtCategoryName" Width="250px" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCategoryName" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclCategorySku" meta:resourcekey="lclCategorySku" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtCategorySku" Width="250px" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCategorySku" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclParentCategory" meta:resourcekey="lclParentCategory" /></td>
			<td>
				<asp:DropDownList runat="server" DataValueField="CategoryID" Width="250px" AppendDataBoundItems="true" 
					DataTextField="CategoryName" ID="ddlCategories">
					<asp:ListItem meta:resourcekey="ListLabel" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclShortDescription" meta:resourcekey="lclShortDescription" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtShortDescription" 
					TextMode="MultiLine" Rows="10" Columns="60" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclFullDescription" meta:resourcekey="lclFullDescription" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtFullDescription" TextMode="MultiLine" 
					Rows="10" Columns="60" /></td>
		</tr>
	</table>
</div>

<div class="FormFooter">
	<asp:Button ID="btnUpdate" runat="server" meta:resourcekey="btnUpdate" CssClass="Button1" 
		OnClick="btnUpdate_Click" />&nbsp;
	<asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1" 
		CausesValidation="false" OnClick="btnDelete_Click" />&nbsp;
	<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" 
		CausesValidation="false" OnClick="btnCancel_Click" />
</div>
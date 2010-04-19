<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesCustomErrorsControl.ascx.cs" Inherits="WebsitePanel.Portal.WebSitesCustomErrorsControl" %>
<div class="FormButtonsBar">
    <asp:Button id="btnAdd" runat="server" meta:resourcekey="btnAddError" Text="Add Custom Error" CssClass="Button3" OnClick="btnAdd_Click" CausesValidation="False"></asp:Button>
</div>
<asp:GridView id="gvErrorPages" Runat="server" AutoGenerateColumns="False"
    CssSelectorClass="NormalGridView"
    OnRowCommand="gvErrorPages_RowCommand" OnRowDataBound="gvErrorPages_RowDataBound"
    EmptyDataText="gvErrorPages">
    <columns>
	    <asp:TemplateField HeaderText="gvErrorPagesCode" ItemStyle-Width="70px">
		    <itemtemplate>
			    <asp:TextBox id="txtErrorCode" Runat="server" Width="30px" CssClass="NormalTextBox" Text='<%# Eval("ErrorCode") %>'>
			    </asp:TextBox>.<asp:TextBox id="txtErrorSubcode" Runat="server" Width="20px" CssClass="NormalTextBox" Text='<%# GetSubCode(Eval("ErrorSubcode")) %>'>
			    </asp:TextBox>
		    </itemtemplate>
	    </asp:TemplateField>
	    <asp:TemplateField HeaderText="gvErrorPagesHandlerType" ItemStyle-Width="100px">
		    <itemtemplate>
			    <asp:dropdownlist id="ddlHandlerType" Width="100px" Runat="server"
			        CssClass="NormalTextBox">
			    </asp:dropdownlist>
		    </itemtemplate>
	    </asp:TemplateField>
	    <asp:TemplateField HeaderText="gvErrorPagesErrorContent">
		    <itemtemplate>
			    <asp:TextBox id="txtErrorContent" Runat="server" Width="100%" CssClass="NormalTextBox" Text='<%# Eval("ErrorContent") %>'>
			    </asp:TextBox>
		    </itemtemplate>
	    </asp:TemplateField>
	    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
		    <itemtemplate>
			    <asp:ImageButton ID="cmdDelete" runat="server" SkinID="DeleteSmall"
			        CommandName="delete_item" CausesValidation="false" meta:resourcekey="cmdDeleteError"></asp:ImageButton>
		    </itemtemplate>
	    </asp:TemplateField>
    </columns>
</asp:GridView>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesMimeTypesControl.ascx.cs" Inherits="WebsitePanel.Portal.WebSitesMimeTypesControl" %>
<div style="width:400px">
    <div class="FormButtonsBar">
        <asp:Button id="btnAdd" runat="server" meta:resourcekey="btnAddMime" Text="Add MIME" CssClass="Button2" CausesValidation="false" OnClick="btnAdd_Click"/>
    </div>

    <asp:GridView id="gvMimeTypes" Runat="server" AutoGenerateColumns="False"
        CssSelectorClass="NormalGridView"
	    OnRowCommand="gvMimeTypes_RowCommand" OnRowDataBound="gvMimeTypes_RowDataBound"
	    EmptyDataText="gvMimeTypes">
	    <columns>
		    <asp:TemplateField HeaderText="gvMimeTypesExtension">
			    <itemtemplate>
				    <asp:TextBox id="txtExtension" Runat="server" Width="70" CssClass="NormalTextBox" Text='<%# DataBinder.Eval(Container.DataItem, "Extension") %>'>
				    </asp:TextBox>
			    </itemtemplate>
		    </asp:TemplateField>
		    <asp:TemplateField HeaderText="gvMimeTypesMIMEtype">
			    <itemtemplate>
				    <asp:TextBox id="txtMimeType" Runat="server" Width="150" CssClass="NormalTextBox" Text='<%# DataBinder.Eval(Container.DataItem, "MimeType") %>'>
				    </asp:TextBox>
			    </itemtemplate>
		    </asp:TemplateField>
	        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
		        <itemtemplate>
			        <asp:ImageButton ID="cmdDeleteMime" runat="server" SkinID="DeleteSmall"
			        CommandName="delete_item" CausesValidation="false" meta:resourcekey="cmdDeleteMime"></asp:ImageButton>
		        </itemtemplate>
	        </asp:TemplateField>
	    </columns>
    </asp:GridView>

</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailDomainsEditDomain.ascx.cs" Inherits="WebsitePanel.Portal.MailDomainsEditDomain" %>

<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>


<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
	if (!confirm("Are you sure you want to delete this Domain?")) return false; else ShowProgressDialog('Deleting Domain...');
}
</script>

<div class="FormBody">
    <div class="Huge">
        <asp:Literal ID="litDomainName" runat="server"></asp:Literal>
    </div>
    <div class="FormBody" style="width: 400px;">
        <div class="FormButtonsBar">
            <asp:Button ID="btnAddPointer" runat="server" meta:resourcekey="btnAddPointer" Text="Add Pointer" CssClass="Button2" OnClick="btnAddPointer_Click" />
        </div>
        <asp:GridView id="gvPointers" Runat="server" EnableViewState="True" AutoGenerateColumns="false"
            ShowHeader="false"
            CssSelectorClass="NormalGridView"
            EmptyDataText="gvPointers" DataKeyNames="DomainID" OnRowDeleting="gvPointers_RowDeleting">
            <Columns>
	            <asp:TemplateField HeaderText="gvPointersName">
		            <ItemStyle Wrap="false" Width="100%"></ItemStyle>
		            <ItemTemplate>
                        <%# Eval("DomainName") %>
                        <asp:ImageButton ID="cmdDeletePointer" Runat="server" SkinID="DeleteSmall" meta:resourcekey="cmdDeletePointer" AlternateText="Remove pointer"
							CommandName='delete' CommandArgument='<%# Eval("DomainId") %>' OnClientClick="return confirm('Remove pointer?');"
							Visible='<%# !(bool)Eval("IsInstantAlias") %>'></asp:ImageButton>
		            </ItemTemplate>
	            </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="FormBody">
        <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
    </div>
</div>

<div class="FormFooter">
    <asp:Button ID="btnUpdate" runat="server" CssClass="Button1" meta:resourcekey="btnUpdate" Text="Update" OnClick="btnUpdate_Click" OnClientClick = "ShowProgressDialog('Updating Domain Settings...');"/>
    <asp:Button ID="btnCancel" runat="server" CssClass="Button1" meta:resourcekey="btnCancel" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" CssClass="Button1" meta:resourcekey="btnDelete" Text="Delete" CausesValidation="false" OnClientClick="return confirmation();" OnClick="btnDelete_Click" />
</div>
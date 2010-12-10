<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceDeleteSpace.ascx.cs" Inherits="WebsitePanel.Portal.SpaceDeleteSpace" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="FormBody">
    <table>
	    <tr id="rowPackageItems" runat="server">
		    <td class="Normal">
			    <asp:Label ID="lblServiceItems" runat="server" meta:resourcekey="lblServiceItems" Text="The package contains the following service items:"></asp:Label>
			    <br/>
			    <br/>
                <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="gvItems" CssSelectorClass="NormalGridView">
                    <Columns>
                        <asp:BoundField DataField="ItemName" HeaderText="gvItemsName" ></asp:BoundField>
                        <asp:TemplateField HeaderText="gvItemsType">
                            <ItemTemplate>
                                <%# GetSharedLocalizedString("ServiceItemType." + (string)Eval("DisplayName")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
			    <br/>
		    </td>
	    </tr>
	    <tr id="rowPackagePackages" runat="server">
		    <td class="Normal">
			    <asp:Label ID="lblChildPackages" runat="server" meta:resourcekey="lblChildPackages" Text="The package contains the following child packages:"></asp:Label>
			    <br/>
			    <br/>
                <asp:GridView ID="gvPackages" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="gvPackages" CssSelectorClass="NormalGridView">
                    <Columns>
                        <asp:BoundField DataField="PackageName" HeaderText="gvPackagesPackageName" ></asp:BoundField>
                    </Columns>
                </asp:GridView>
			    <br/>
		    </td>
	    </tr>
	    <tr>
		    <td class="Normal">
			    <asp:Label ID="lblDeleteWarning" runat="server" meta:resourcekey="lblDeleteWarning" Text="Deleting this package also deletes all its child packages and all service items such as web sites, 
			    databases, user accounts, etc."></asp:Label>
			    <br/>
			    <br/>
		    </td>
	    </tr>
	    <tr>
		    <td class="Normal">
			    <asp:CheckBox ID="chkConfirm" Runat="server" meta:resourcekey="chkConfirm" Text="Yes, I understand it and want to delete this package"></asp:CheckBox>
			    <br/>
			    <br/>
		    </td>
	    </tr>
    </table>
</div>
<div class="FormFooter">
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1" Text="Delete" CausesValidation="false" OnClick="btnDelete_Click" OnClientClick="ShowProgressDialog('Deleting...');"/>
	<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
</div>
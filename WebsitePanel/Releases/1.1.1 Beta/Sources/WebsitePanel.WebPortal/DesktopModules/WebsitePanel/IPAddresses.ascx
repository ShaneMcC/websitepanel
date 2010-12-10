<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IPAddresses.ascx.cs" Inherits="WebsitePanel.Portal.IPAddresses" %>
<%@ Register Src="UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>

<script language="javascript">
    function SelectAllCheckboxes(box)
    {
		var state = box.checked;
        var elm = box.parentElement.parentElement.parentElement.parentElement.getElementsByTagName("INPUT");
        for(i = 0; i < elm.length; i++)
            if(elm[i].type == "checkbox" && elm[i].id != box.id && elm[i].checked != state && !elm[i].disabled)
		        elm[i].checked = state;
    }
</script>

    <wsp:SimpleMessageBox id="messageBox" runat="server" />


<div style="padding: 4px;">
    <asp:Button ID="btnAddItem" runat="server" meta:resourcekey="btnAddItem" Text="Add IP Address" CssClass="Button3" OnClick="btnAddItem_Click" />
</div>

<div class="FormButtonsBar">
	<div class="Left">
		<asp:Label ID="lblPool" runat="server" meta:resourcekey="lblPool" Text="Pool:" CssClass="Normal"></asp:Label>
		<asp:DropDownList ID="ddlPools" runat="server" CssClass="NormalTextBox" AutoPostBack="true">
		    <asp:ListItem Value="General" meta:resourcekey="ddlPoolsGeneral">General</asp:ListItem>
		    <asp:ListItem Value="WebSites" meta:resourcekey="ddlPoolsWebSites">WebSites</asp:ListItem>
		    <asp:ListItem Value="VpsExternalNetwork" meta:resourcekey="ddlPoolsVpsExternalNetwork">VpsExternalNetwork</asp:ListItem>
		    <asp:ListItem Value="VpsManagementNetwork" meta:resourcekey="ddlPoolsVpsManagementNetwork">VpsManagementNetwork</asp:ListItem>
		</asp:DropDownList>
	</div>
	<div class="Right">
		<wsp:SearchBox ID="searchBox" runat="server" />
	</div>
</div>
    
<asp:GridView id="gvIPAddresses" runat="server" AutoGenerateColumns="False"
	AllowSorting="True" EmptyDataText="gvIPAddresses"
	CssSelectorClass="NormalGridView" DataKeyNames="AddressID"
	AllowPaging="True" DataSourceID="odsIPAddresses">
	<Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# ((int)Eval("ItemID") == 0) %>'  />
            </ItemTemplate>
            <ItemStyle Width="10px" />
        </asp:TemplateField>
		<asp:TemplateField SortExpression="ExternalIP" HeaderText="gvIPAddressesExternalIP">
			<ItemTemplate>
				<asp:hyperlink NavigateUrl='<%# EditUrl("AddressID", DataBinder.Eval(Container.DataItem, "AddressID").ToString(), "edit_ip") %>' runat="server" ID="Hyperlink2">
					<%# Eval("ExternalIP") %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="InternalIP" SortExpression="InternalIP" HeaderText="gvIPAddressesInternalIP"></asp:BoundField>
        <asp:BoundField DataField="DefaultGateway" SortExpression="DefaultGateway" HeaderText="gvIPAddressesGateway"></asp:BoundField>
		<asp:BoundField DataField="ServerName" SortExpression="ServerName" HeaderText="gvIPAddressesServer" ItemStyle-Wrap="false"></asp:BoundField>
		<asp:TemplateField HeaderText="gvAddressesUser" meta:resourcekey="gvAddressesUser" SortExpression="Username"  >						        
	        <ItemTemplate>
		        <%# Eval("UserName") %>&nbsp;
	        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvAddressesSpace" meta:resourcekey="gvAddressesSpace" SortExpression="PackageName" >
	        <ItemTemplate>
		        <asp:hyperlink id="lnkSpace" runat="server" NavigateUrl='<%# GetSpaceHomeUrl((int)Eval("PackageID")) %>'>
			        <%# Eval("PackageName") %>
		        </asp:hyperlink>&nbsp;
	        </ItemTemplate>
        </asp:TemplateField>
		<asp:BoundField HeaderText="gvAddressesItemName" meta:resourcekey="gvAddressesItemName" DataField="ItemName" SortExpression="ItemName" />
        <asp:BoundField DataField="Comments" HeaderText="gvIPAddressesComments"></asp:BoundField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsIPAddresses" runat="server" EnablePaging="True"
	    SelectCountMethod="GetIPAddressesPagedCount"
	    SelectMethod="GetIPAddressesPaged"
	    SortParameterName="sortColumn"
	    TypeName="WebsitePanel.Portal.IPAddressesHelper"
	    OnSelected="odsIPAddresses_Selected">
    <SelectParameters>
	    <asp:ControlParameter Name="pool" ControlID="ddlPools" PropertyName="SelectedValue" />
	    <asp:Parameter Name="serverId" DefaultValue="0" />
        <asp:ControlParameter Name="filterColumn" ControlID="searchBox"  PropertyName="FilterColumn" />
        <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
    </SelectParameters>
</asp:ObjectDataSource>

<div class="GridFooter">
    <div class="Left">
        <asp:Button id="btnEditSelected" runat="server" Text="Edit Selected..."
            meta:resourcekey="btnEditSelected" CssClass="SmallButton" 
            CausesValidation="false" onclick="btnEditSelected_Click"></asp:Button>
        <asp:Button id="btnDeleteSelected" runat="server" Text="Delete Selected"
            meta:resourcekey="btnDeleteSelected" CssClass="SmallButton" 
            CausesValidation="false" onclick="btnDeleteSelected_Click"></asp:Button>
    </div>
    <div class="Right">
        <asp:Label ID="lblItemsPerPage" runat="server" meta:resourcekey="lblItemsPerPage" Text="Page size:" CssClass="Normal"></asp:Label>
        <asp:DropDownList ID="ddlItemsPerPage" runat="server" CssClass="NormalTextBox" 
            AutoPostBack="True" onselectedindexchanged="ddlItemsPerPage_SelectedIndexChanged">
            <asp:ListItem Value="10">10</asp:ListItem>
            <asp:ListItem Value="20">20</asp:ListItem>
            <asp:ListItem Value="50">50</asp:ListItem>
            <asp:ListItem Value="100">100</asp:ListItem>
        </asp:DropDownList>
    </div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PackageIPAddresses.ascx.cs" Inherits="WebsitePanel.Portal.UserControls.PackageIPAddresses" %>
<%@ Register Src="SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="SearchBox.ascx" TagName="SearchBox" TagPrefix="wsp" %>


<script language="javascript">
    function SelectAllCheckboxes(box) {
        var state = box.checked;
        var elm = box.parentElement.parentElement.parentElement.parentElement.getElementsByTagName("INPUT");
        for (i = 0; i < elm.length; i++)
            if (elm[i].type == "checkbox" && elm[i].id != box.id && elm[i].checked != state && !elm[i].disabled)
            elm[i].checked = state;
    }
</script>

<wsp:SimpleMessageBox id="messageBox" runat="server" />

<div class="FormButtonsBarClean">
    <div class="FormButtonsBarCleanLeft">
        <asp:Button ID="btnAllocateAddress" runat="server" meta:resourcekey="btnAllocateAddress"
        Text="Allocate IP Addresses" CssClass="Button1" CausesValidation="False" 
            onclick="btnAllocateAddress_Click" />
    </div>
    <div class="FormButtonsBarCleanRight">
        <wsp:SearchBox ID="searchBox" runat="server" />
    </div>
</div>

<asp:GridView ID="gvAddresses" runat="server" AutoGenerateColumns="False"
    Width="100%" EmptyDataText="gvAddresses" CssSelectorClass="NormalGridView"
    AllowPaging="True" AllowSorting="True" DataSourceID="odsExternalAddressesPaged" 
    onrowdatabound="gvAddresses_RowDataBound" DataKeyNames="PackageAddressID" >
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" />&nbsp;
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:BoundField HeaderText="gvAddressesIPAddress" meta:resourcekey="gvAddressesIPAddress"
            DataField="ExternalIP" SortExpression="ExternalIP" />
            
        <asp:BoundField HeaderText="gvAddressesNATAddress" meta:resourcekey="gvAddressesNATAddress"
            DataField="InternalIP" SortExpression="InternalIP" />

        <asp:BoundField HeaderText="gvAddressesDefaultGateway" meta:resourcekey="gvAddressesDefaultGateway"
            DataField="DefaultGateway" SortExpression="DefaultGateway" />

        <asp:TemplateField HeaderText="gvAddressesItemName" meta:resourcekey="gvAddressesItemName" SortExpression="ItemName">						        						        
	        <ItemTemplate>
		         <asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# GetItemEditUrl(Eval("ItemID").ToString()) %>'>
			        <%# Eval("ItemName") %>
		        </asp:hyperlink>&nbsp;
	        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvAddressesPrimary" meta:resourcekey="gvAddressesPrimary" SortExpression="IsPrimary">						        						        
	        <ItemTemplate>						        
		        <asp:Image ID="imgPrimary" runat="server" SkinID="Checkbox16" Visible='<%# Eval("IsPrimary") %>' />&nbsp;
	        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvAddressesSpace" meta:resourcekey="gvAddressesSpace" SortExpression="PackageName" >
	        <ItemTemplate>
		        <asp:hyperlink id="lnkSpace" runat="server" NavigateUrl='<%# GetSpaceHomeUrl(Eval("PackageID").ToString()) %>'>
			        <%# Eval("PackageName") %>
		        </asp:hyperlink>
	        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvAddressesUser" meta:resourcekey="gvAddressesUser" SortExpression="Username"  >						        
	        <ItemTemplate>
		        <%# Eval("UserName") %>
	        </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsExternalAddressesPaged" runat="server" EnablePaging="True"
	    SelectCountMethod="GetPackageIPAddressesCount"
	    SelectMethod="GetPackageIPAddresses"
	    SortParameterName="sortColumn"
	    TypeName="WebsitePanel.Portal.VirtualMachinesHelper"
	    OnSelected="odsExternalAddressesPaged_Selected" 
    onselecting="odsExternalAddressesPaged_Selecting">
    <SelectParameters>
	    <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="0" />						    
	    <asp:Parameter Name="pool" DefaultValue="0" />
        <asp:ControlParameter Name="filterColumn" ControlID="searchBox"  PropertyName="FilterColumn" />
        <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
    </SelectParameters>
</asp:ObjectDataSource>

<div style="margin-top:4px;">
    <asp:Button ID="btnDeallocateAddresses" runat="server" meta:resourcekey="btnDeallocateAddresses"
            Text="Deallocate selected" CssClass="SmallButton" CausesValidation="False" 
        onclick="btnDeallocateAddresses_Click" />
</div>
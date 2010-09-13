<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IPAddressesAddIPAddress.ascx.cs" Inherits="WebsitePanel.Portal.IPAddressesAddIPAddress" %>
<%@ Register Src="UserControls/EditIPAddressControl.ascx" TagName="EditIPAddressControl" TagPrefix="wsp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<div class="FormBody">

    <wsp:SimpleMessageBox id="messageBox" runat="server" />
    
    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
            ValidationGroup="EditAddress" ShowMessageBox="True" ShowSummary="False" />

    <table cellspacing="0" cellpadding="3">
	    <tr>
		    <td style="width:150px;">
		        <asp:Localize ID="locPool" runat="server" meta:resourcekey="locPool" Text="Pool:"></asp:Localize>
		    </td>
		    <td>
		        <asp:DropDownList ID="ddlPools" runat="server" CssClass="NormalTextBox" 
                    AutoPostBack="true" onselectedindexchanged="ddlPools_SelectedIndexChanged">
		            <asp:ListItem Value="General" meta:resourcekey="ddlPoolsGeneral">General</asp:ListItem>
		            <asp:ListItem Value="WebSites" meta:resourcekey="ddlPoolsWebSites">WebSites</asp:ListItem>
		            <asp:ListItem Value="VpsExternalNetwork" meta:resourcekey="ddlPoolsVpsExternalNetwork">VpsExternalNetwork</asp:ListItem>
		            <asp:ListItem Value="VpsManagementNetwork" meta:resourcekey="ddlPoolsVpsManagementNetwork">VpsManagementNetwork</asp:ListItem>
		        </asp:DropDownList>
            </td>
	    </tr>
	    <tr>
		    <td><asp:Localize ID="locServer" runat="server" meta:resourcekey="locServer" Text="Server:"></asp:Localize></td>
		    <td>
		        <asp:dropdownlist id="ddlServer" CssClass="NormalTextBox" runat="server" DataTextField="ServerName" DataValueField="ServerID"></asp:dropdownlist>
		    </td>
	    </tr>
	    <tr>
		    <td><asp:Localize ID="lblExternalIP" runat="server" meta:resourcekey="lblExternalIP" Text="IP Address:"></asp:Localize></td>
		    <td>
		    
		        <wsp:EditIPAddressControl id="startIP" runat="server" ValidationGroup="EditAddress" Required="true" />
					            
			    &nbsp;<asp:Localize ID="locTo" runat="server" meta:resourcekey="locTo" Text="to"></asp:Localize>&nbsp;

		        <wsp:EditIPAddressControl id="endIP" runat="server" ValidationGroup="EditAddress" />
		        
		    </td>
	    </tr>
	    <tr id="InternalAddressRow" runat="server">
		    <td>
		        <asp:Localize ID="lblInternalIP" runat="server" meta:resourcekey="lblInternalIP" Text="NAT Address:"></asp:Localize>
		    </td>
		    <td>
		        <wsp:EditIPAddressControl id="internalIP" runat="server" ValidationGroup="EditAddress"  />
            </td>
	    </tr>
        <tr id="SubnetRow" runat="server">
	        <td><asp:Localize ID="locSubnetMask" runat="server" meta:resourcekey="locSubnetMask" Text="Subnet Mask:"></asp:Localize></td>
	        <td class="NormalBold">
	            <wsp:EditIPAddressControl id="subnetMask" runat="server" ValidationGroup="EditAddress" Required="true"  />
            </td>
        </tr>
        <tr id="GatewayRow" runat="server">
	        <td><asp:Localize ID="locDefaultGateway" runat="server" meta:resourcekey="locDefaultGateway" Text="Default Gateway:"></asp:Localize></td>
	        <td class="NormalBold">
	            <wsp:EditIPAddressControl id="defaultGateway" runat="server" ValidationGroup="EditAddress" Required="true"  />
            </td>
        </tr>
	    <tr>
		    <td><asp:Localize ID="lblComments" runat="server" meta:resourcekey="lblComments" Text="Comments:"></asp:Localize></td>
		    <td class="NormalBold"><asp:textbox id="txtComments" Width="250px" CssClass="NormalTextBox" runat="server" Rows="3" TextMode="MultiLine"></asp:textbox></td>
	    </tr>
    </table>
    
</div>
<div class="FormFooter">
    <asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" CssClass="Button1" OnClick="btnAdd_Click" Text="Add" ValidationGroup="EditAddress" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" CausesValidation="False" OnClick="btnCancel_Click" />
</div>
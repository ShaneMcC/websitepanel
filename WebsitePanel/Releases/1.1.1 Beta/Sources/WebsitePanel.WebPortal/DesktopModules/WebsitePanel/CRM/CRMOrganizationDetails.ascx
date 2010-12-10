<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMOrganizationDetails.ascx.cs" Inherits="WebsitePanel.Portal.CRM.CRMOrganizationDetails" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector"
    TagPrefix="wsp" %>
<%@ Register Src="../ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="../ExchangeServer/UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/Gauge.ascx" TagName="Gauge" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
		</div>
		<div class="Left">
			<wsp:Menu id="menu" runat="server" SelectedItem="storage_usage" />
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="CRMLogo" runat="server" />
					<asp:Localize ID="locTitle" runat="server"  Text="CRM Organization"></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
				    
				    <div >
				       <table>
				          <tr height="23px">
				            <td class="FormLabel150"><asp:Label runat="server" ID="lblName" meta:resourcekey="lblName"/></td>
				            <td><asp:Label runat="server" ID="lblCrmOrgName" />&nbsp;&nbsp; <span class="Huge"><asp:HyperLink runat="server" id="hlOrganizationPage" Visible="false"  Target="_blank" /></span></td>
				          </tr>
				          
				          <tr height="23px">
				            <td class="FormLabel150"><asp:Label runat="server" ID="lblOrgID" meta:resourcekey="lblOrgID"/></td>
				            <td><asp:Label runat="server" ID="lblCrmOrgId" /></td>
				          </tr>
				          
				          <tr height="23px">
				            <td class="FormLabel150"><asp:Label runat="server" ID="lblAdministrator" meta:resourcekey="lblAdministrator"/></td>
				            <td>
                                <wsp:UserSelector  id="administrator" runat="server" IncludeMailboxes="true">
                                </wsp:UserSelector>
                                <asp:Label runat="server" ID="lblAdminValid" Text="*" ForeColor="red" Visible="false" />
                                <asp:Label runat="server" ID="lblAdmin" />
                                </td>
				          </tr>
				          
				          <tr height="23px">
				            <td class="FormLabel150"><asp:Label runat="server" ID="lblCurrency" meta:resourcekey="lblCurrency"/></td>
				            <td><asp:DropDownList runat="server" ID="ddlCurrency" /></td>
				          </tr>
				          				          				        
                          <tr height="23px">
				            <td class="FormLabel150"><asp:Label runat="server" ID="lblCollation" meta:resourcekey="lblCollation"/></td>
				            <td><asp:DropDownList runat="server" ID="ddlCollation" /></td>
				          </tr>                         

				       </table>			            
			            <div class="FormFooterClean">
					    <asp:Button runat="server" meta:resourcekey="btnCreate" ID="btnCreate" CssClass="Button2" OnClick="btnCreate_Click"  />		
					    <asp:Button runat="server" meta:resourcekey="btnDelete" ID="btnDelete" CssClass="Button2" Visible="false" OnClick="btnDelete_Click" />			    					    
				    </div>
			            
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
                </div>
		</div>
	</div>
</div>
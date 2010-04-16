<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeStorageUsage.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeStorageUsage" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/Gauge.ascx" TagName="Gauge" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
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
					<asp:Image ID="Image1" SkinID="ExchangeStorage48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Storage Usage"></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
				    
				    <div style="margin-left: 30px;margin-top: 30px;">
				       
			            <table cellpadding="2">					        
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="locUsedSize" runat="server" meta:resourcekey="locUsedSize" Text="Used Disk Space:"></asp:Localize></td>
					            <td>
						            <asp:LinkButton runat="server" CssClass="NormalBold" Text="100"  meta:resourcekey="btnUsedSize"  ID="btnUsedSize" onclick="btnUsedSize_Click"  />						            
					            </td>
					        </tr>
				        </table>
				        <br />
				        <br />
				        <asp:Button ID="btnRecalculate" runat="server" meta:resourcekey="btnRecalculate"
							CssClass="Button1" Text="Recalculate Disk Space" CausesValidation="false" OnClick="btnRecalculate_Click" />							
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>
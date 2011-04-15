<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostedSharePointStorageSettings.ascx.cs" Inherits="WebsitePanel.Portal.HostedSharePointStorageSettings" %>
<%@ Register Src="ExchangeServer/UserControls/Breadcrumb.ascx" TagName="Breadcrumb"
    TagPrefix="wsp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="wsp" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel"
    TagPrefix="wsp" %>
<%@ Register Src="ExchangeServer/UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="wsp" %>

<%@ Register Src="ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register src="UserControls/QuotaEditor.ascx" tagname="QuotaEditor" tagprefix="uc1" %>



<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
		</div>
		<div class="Left">
			<wsp:Menu id="menu" runat="server" SelectedItem="storage_limits" />
            </div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="ExchangeStorageConfig48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" ></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
				    
					<wsp:CollapsiblePanel id="secStorageLimits" runat="server"
                        TargetControlID="StorageLimits" meta:resourcekey="secStorageLimits" >
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="StorageLimits" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    
						    <tr>
							    <td class="FormLabel200" align="right"><asp:Localize ID="locMaxStorage" runat="server" meta:resourcekey="locMaxStorage" ></asp:Localize></td>
							    <td>                                    
									<uc1:QuotaEditor QuotaTypeId="2" ID="maxStorageSettingsValue" runat="server" />                                    																	    
								</td>
						    </tr>
						    <tr>
							    <td class="FormLabel200" align="right"><asp:Localize ID="locWarningStorage" runat="server" meta:resourcekey="locWarningStorage" ></asp:Localize></td>
							    <td>
									<uc1:QuotaEditor  QuotaTypeId="2" ID="warningValue" runat="server" />
									
								</td>
						    </tr>
					    </table>
					    <br />
					</asp:Panel>
                   									                    
				    <div class="FormFooterClean">
					    <asp:Button id="btnSave" runat="server" Text="Save Changes" CssClass="Button1" meta:resourcekey="btnSave"
							 ValidationGroup="EditStorageSettings" OnClick="btnSave_Click" ></asp:Button>
						<asp:Button id="btnSaveApply" runat="server" Text="Save and Apply to All SiteCollections" CssClass="Button1" meta:resourcekey="btnSaveApply"
							 ValidationGroup="EditStorageSettings" OnClick="btnSaveApply_Click" ></asp:Button>
						
				    </div>
				    
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>
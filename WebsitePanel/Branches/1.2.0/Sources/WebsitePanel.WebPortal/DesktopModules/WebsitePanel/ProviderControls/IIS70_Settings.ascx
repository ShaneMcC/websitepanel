<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IIS70_Settings.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.IIS70_Settings" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>
<%@ Register Src="Common_ActiveDirectoryIntegration.ascx" TagName="ActiveDirectoryIntegration" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/EditDomainsList.ascx" TagName="EditDomainsList" TagPrefix="uc5" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagPrefix="wsp" TagName="CollapsiblePanel" %>
<%@ Register Src="../UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="wsp" %>
<%@ Import Namespace="WebsitePanel.Portal.ProviderControls" %>

<fieldset>
    <legend>
        <asp:Label ID="secServiceSettings" runat="server" meta:resourcekey="secServiceSettings" Text="Service Settings" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>

    <table width="100%" cellpadding="4">
		<tr>
			<td class="Normal" width="200" nowrap>
			    <asp:Label ID="lblSharedIP" runat="server" meta:resourcekey="lblSharedIP" Text="Web Sites Shared IP Address:"></asp:Label>
			</td>
			<td width="100%">
                <uc1:SelectIPAddress ID="ipAddress" runat="server" ServerIdParam="ServerID" />
			</td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Label ID="lblGroupName" runat="server" meta:resourcekey="lblGroupName" Text="Web Users Group Name:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtWebGroupName" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
            </td>
		</tr>
	    <tr>
	        <td colspan="2">
	            <asp:CheckBox ID="chkAssignIPAutomatically" runat="server" meta:resourcekey="chkAssignIPAutomatically" Text="Assign IP addresses to the space on creation" />
	        </td>
	    </tr>
    </table>
</fieldset>
<br />


<fieldset>
    <legend>
        <asp:Label ID="secAspNet" runat="server" meta:resourcekey="secAspNet" Text="ASP.NET" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>

    <table width="100%" cellpadding="4">

		<tr>
		    <td width="200" class="Normal">
		        <asp:Label ID="lblAspNet11Path" runat="server" meta:resourcekey="lblAspNet11Path" Text="ASP.NET 1.1:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet11Path" runat="server" CssClass="NormalTextBox" Width="450px"></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Label ID="lblAspNet20Path" runat="server" meta:resourcekey="lblAspNet20Path" Text="ASP.NET 2.0:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet20Path" runat="server" CssClass="NormalTextBox" Width="450px"></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Label ID="lblAspNet20x64Path" runat="server" meta:resourcekey="lblAspNet20x64Path" Text="ASP.NET 2.0 64-bit:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet20x64Path" runat="server" CssClass="NormalTextBox" Width="450px"></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Label runat="server" meta:resourcekey="AspNet40Path" Text="ASP.NET 4.0:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet40Path" runat="server" CssClass="NormalTextBox" Width="450px"></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Label runat="server" meta:resourcekey="AspNet40x64Path" Text="ASP.NET 4.0 64-bit:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet40x64Path" runat="server" CssClass="NormalTextBox" Width="450px"></asp:TextBox>
            </td>
		</tr>
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Label ID="secPools" runat="server" meta:resourcekey="secPools" Text="Pools" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>

    <table width="100%" cellpadding="4">
		<tr>
		    <td width="200" class="Normal" valign="top">
		        <asp:Label ID="lblAsp11Pool" runat="server" meta:resourcekey="lblAsp11Pool" Text="ASP.NET 1.1:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet11Pool" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Label ID="lblAsp20Pool" runat="server" meta:resourcekey="lblAsp20Pool" Text="ASP.NET 2.0 Classic:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet20Pool" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Label ID="lblAsp20IntegratedPool" runat="server" meta:resourcekey="lblAsp20IntegratedPool" Text="ASP.NET 2.0 Integrated:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspNet20IntegratedPool" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
				<asp:Localize runat="server" meta:resourcekey="ClassicAspNet40PoolLocalize" />
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="ClassicAspNet40Pool" runat="server" CssClass="NormalTextBox" Width="200px" />
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Localize runat="server" meta:resourcekey="IntegratedAspNet40PoolLocalize" />
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="IntegratedAspNet40Pool" runat="server" CssClass="NormalTextBox" Width="200px"/>
            </td>
		</tr>
		<tr>
			<td class="Normal" valign="top">
				<asp:Localize runat="server" meta:resourcekey="AspNetBitnessModeLocalize" Text="ASP.NET Mode (2.0/4.0):" />
			</td>
			<td class="Normal" valign="top">
				<asp:DropDownList ID="AspNetBitnessMode" runat="server">
					<asp:ListItem Value="32">32-bit</asp:ListItem>
					<asp:ListItem Value="64">64-bit</asp:ListItem>
				</asp:DropDownList>
			</td>
		</tr>
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Label ID="lblWebAppGallery" runat="server" meta:resourcekey="lblWebAppGallery" Text="Pools" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
	<br />
    <table width="100%" cellpadding="4">
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblGalleryFeed" runat="server" meta:resourcekey="lblGalleryFeed" Text="Gallery feed URL:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtGalleryFeedUrl" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
                <p style="text-align: justify;"><i><asp:Localize runat="server" meta:resourcekey="lclGalleryFeedNote" /></i></p>
			</td>
		</tr>
		<tr>
			<td class="Normal" valign="top" width="192">
		        <asp:Label runat="server" meta:resourcekey="GalleryFeedFilter" Text="Gallery feed filter:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:LinkButton runat="server" ID="FilterDialogButton" meta:resourcekey="FilterDialogButton" Text="Click to apply a filter..." />
			</td>
		</tr>
    </table>
</fieldset>
<br />

<asp:Panel ID="FilterDialogPanel" runat="server" CssClass="PopupContainer" style="display:none" 
	DefaultButton="OkButton">
	<wsp:PopupHeader runat="server" meta:resourcekey="popWebAppGalleryFilter" Text="Web Application Gallery Filter" />
	<div class="Content">
		<div style="padding: 5px 10px 5px 10px;"><asp:Localize runat="server" meta:resourcekey="lclGalleryFilterNote" /></div>
		<div class="BorderFillBox" style="padding: 5px 5px 5px 5px; overflow-y: scroll; width: auto; height: 300px;">
			<asp:CheckBoxList runat="server" ID="WebAppGalleryList" DataSourceID="WebAppGalleryListDS" 
				DataValueField="Id" DataTextField="Title" OnDataBound="WebAppGalleryList_DataBound">
			</asp:CheckBoxList>
		</div>
		<div class="FormFooter">
			<asp:Button ID="OkButton" runat="server" CssClass="Button1" Text="Apply" CausesValidation="false" />
            <asp:Button ID="ResetButton" runat="server" CssClass="Button1" Text="Reset" OnClick="ResetButton_Click" CausesValidation="false" />
            <asp:Button ID="CancelButton" runat="server" CssClass="Button1" Text="Cancel" CausesValidation="false" />
		</div>
	</div>
	
	<asp:ObjectDataSource runat="server" ID="WebAppGalleryListDS" TypeName="WebsitePanel.Portal.WebAppGalleryHelpers" 
		SelectMethod="GetGalleryApplicationsByServiceId">
		<SelectParameters>
			<asp:QueryStringParameter Name="serviceId" QueryStringField="ServiceID" Type="Int32" />
		</SelectParameters>
	</asp:ObjectDataSource>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="FilterDialogModal" runat="server"
    TargetControlID="FilterDialogButton" PopupControlID="FilterDialogPanel"
    BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="CancelButton" 
    OkControlID="OkButton" />

<fieldset>
    <legend>
        <asp:Label ID="secWebExtensions" runat="server" meta:resourcekey="secWebExtensions" Text="Web Extensions" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>

    <table width="100%" cellpadding="4">
		<tr>
		    <td width="200" class="Normal" valign="top">
		        <asp:Label ID="lblAspPath" runat="server" meta:resourcekey="lblAspPath" Text="ASP Library Path:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtAspPath" runat="server" CssClass="NormalTextBox" Width="450px"></asp:TextBox>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Label runat="server" meta:resourcekey="php4PathLabel" Text="PHP 4.x Executable Path:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="php4Path" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
                <asp:DropDownList ID="php4Mode" runat="server">
                    <asp:ListItem Value="ISAPI">ISAPI</asp:ListItem>
                </asp:DropDownList>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Label ID="lblPhpPath" runat="server" meta:resourcekey="lblPhpPath" Text="PHP 5.x Executable Path:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtPhpPath" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
                <asp:DropDownList ID="ddlPhpMode" runat="server">
                    <asp:ListItem Value="FastCGI">FastCGI</asp:ListItem>
                    <asp:ListItem Value="ISAPI">ISAPI</asp:ListItem>
                </asp:DropDownList>
            </td>
		</tr>
		<tr>
		    <td class="Normal" valign="top">
		        <asp:Label runat="server" meta:resourcekey="perlPathLabel" Text="Perl Executable Path:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="perlPath" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
                <asp:DropDownList ID="perlMode" runat="server">
                    <asp:ListItem Value="ISAPI">ISAPI</asp:ListItem>
                </asp:DropDownList>
            </td>
		</tr>
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Label runat="server" meta:resourcekey="secWmSvcManagement" Text="Web Management Service" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>

    <table width="100%" cellpadding="4">
		<tr>
		    <td width="200" class="Normal">
		        <asp:Label runat="server" meta:resourcekey="lblWmSvcServiceUrl" Text="Service URL:"></asp:Label>
		    </td>
		    <td class="Normal">
                <asp:TextBox runat="server" ID="txtWmSvcServiceUrl" CssClass="NormalTextBox" Width="350px" />
            </td>
		</tr>
		<tr>
		    <td>
		        <asp:Label runat="server" meta:resourcekey="lblWmSvcServicePort" Text="Service Port:"></asp:Label>
		    </td>
		    <td class="Normal">
		        <asp:TextBox runat="server" ID="txtWmSvcServicePort" CssClass="NormalTextBox" Width="70px" />
            </td>
		</tr>
		<tr>
		    <td width="200" class="Normal">
		        <asp:Label runat="server" meta:resourcekey="lblWmSvcCredentialsMode" Text="Credentials Mode"></asp:Label>
		    </td>
		    <td class="Normal">
                <asp:DropDownList runat="server" ID="ddlWmSvcCredentialsMode" CssClass="NormalTextBox">
					<asp:ListItem Value="WINDOWS" Text="Windows Credentials" />
					<asp:ListItem Value="IISMNGR" Text="IIS Manager Credentials" />
                </asp:DropDownList>
            </td>
		</tr>
	</table>
</fieldset>

<fieldset>
    <legend>
        <asp:Label ID="secColdFusion" runat="server" meta:resourcekey="secColdFusion" Text="ColdFusion" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<br />
    <table width="100%" cellpadding="4">

		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblColdFusionPath" runat="server" meta:resourcekey="lblColdFusionPath" Text="ColdFusion Path:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtColdFusionPath" runat="server" CssClass="NormalTextBox" Width="350px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblScriptsDirectory" runat="server" meta:resourcekey="lblScriptsDirectory" Text="Scripts Directory:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtScriptsDirectory" runat="server" CssClass="NormalTextBox" Width="350px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblFlashRemotingDir" runat="server" meta:resourcekey="lblFlashRemotingDir" Text="Flash Remoting Directory:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtFlashRemotingDir" runat="server" CssClass="NormalTextBox" Width="350px"></asp:TextBox></td>
		</tr>
    </table>
</fieldset>
<br />

<fieldset>
    <legend>
        <asp:Label ID="secureFoldersLabel" runat="server" meta:resourcekey="secureFoldersLabel" Text="Secure Folders" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
<br />
    <table width="100%" cellpadding="4">
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblSecureFoldersModulesAsm" runat="server" meta:resourcekey="lblSecureFoldersModulesAsm" Text="Module Assembly:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtSecureFoldersModuleAsm" runat="server" CssClass="NormalTextBox" Width="350px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblProtectedUsersFile" runat="server" meta:resourcekey="lblProtectedUsersFile" Text="IISPassword Users File:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtProtectedUsersFile" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>
		</tr>
		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblProtectedGroupsFile" runat="server" meta:resourcekey="lblProtectedGroupsFile" Text="IISPassword Groups File:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:TextBox ID="txtProtectedGroupsFile" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>
		</tr>
    </table>
</fieldset>
<br />

<!-- Helicon Ape -->

<fieldset >
    <legend>
        <asp:Label ID="Label1" runat="server" meta:resourcekey="HtaccessLabel" Text=".htaccess" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
    
    <asp:Panel ID="downloadApePanel" runat="server">
    <table width="100%" cellpadding="4">
    		<tr>
		    <td class="Normal" valign="top" width="192">
		    </td>
            <td class="Normal" valign="top">
                <asp:Localize ID="Localize1" runat="server" meta:resourcekey="lclHeliconApeInstallNote" />
            </td>
		</tr>
    </table>

    </asp:Panel>

    <asp:Panel ID="configureApePanel" runat="server">
    <table width="100%" cellpadding="4">

		<tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="lblHeliconApeVersion" runat="server" meta:resourcekey="lblHtaccesVersion" Text="Helicon Ape Version:"></asp:Label>
		    </td>
            <td class="Normal" valign="top">
                <asp:Label ID="txtHeliconApeVersion" runat="server" meta:resourcekey="txtHtaccesVersion" Text="Unknown"></asp:Label>
            </td>
		</tr>
        <tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="Label3" runat="server" Text="Registration Info:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <asp:Label ID="lblHeliconRegistrationText" runat="server" Text=""></asp:Label>
            </td>
		</tr>

        
        <tr>
		    <td class="Normal" valign="top" width="192">
		        <asp:Label ID="Label4" runat="server" Text="httpd.conf:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                    
                    <asp:Button ID="EditHeliconApeConfButton" runat="server" class="Button2" 
                        Text="Edit httpd.conf (server config)" onclick="EditHeliconApeConfButton_Click"  
                    />
                  
            </td>
		</tr>
    
       
		
    </table>
    </asp:Panel>
</fieldset>
<br />

<!-- Helicon Ape END-->

<fieldset>
    <legend>
        <asp:Label ID="secOther" runat="server" meta:resourcekey="secOther" Text="Other Settings" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>

    <table width="100%" cellpadding="4">
	    <tr>
	        <td width="200" class="Normal" valign="top">
	            <asp:Label ID="lblSharedSslSites" runat="server" meta:resourcekey="lblSharedSslSites" Text="Shared SSL Sites:"></asp:Label>
	        </td>
	        <td class="Normal" valign="top">
                <uc5:EditDomainsList id="sharedSslSites" runat="server" DisplayNames="false">
                </uc5:EditDomainsList>
            </td>
	    </tr>
	    <tr>
	        <td class="Normal" valign="top">
	            <asp:Label ID="lblADIntegration" runat="server" meta:resourcekey="lblADIntegration" Text="Active Directory Integration:"></asp:Label>
	        </td>
	        <td class="Normal" valign="top">
                <uc1:ActiveDirectoryIntegration ID="ActiveDirectoryIntegration" runat="server" />
            </td>
	    </tr>
    </table>
</fieldset>
<br />
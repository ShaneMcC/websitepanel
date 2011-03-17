<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcFastCreateServer.ascx.cs" Inherits="WebsitePanel.Portal.VPSForPC.VdcFastCreateServer" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


<div id="VpsContainer">
    <div class="Module">

	    <div class="Header">
		    <wsp:Breadcrumb id="breadcrumb" runat="server" />
	    </div>
    	
	    <div class="Left">
		    <wsp:Menu id="menu" runat="server" SelectedItem="" />
	    </div>
    	
	    <div class="Content">
		    <div class="Center">
			    <div class="Title">
				    <asp:Image ID="imgIcon" SkinID="AddServer48" runat="server" />
                    <asp:Localize ID="Localize1" runat="server" meta:resourcekey="locTitle" Text="Create New VM"></asp:Localize>
			    </div>
			    <div class="FormBody">
<%--			        <wsp:ServerTabs id="tabs" runat="server" />	
--%>                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="Tools" ShowMessageBox="True" ShowSummary="False" />
                    
		            <p class="SubTitle">
		                <asp:Localize ID="locSubTitle" runat="server"
		                    meta:resourcekey="locSubTitle" Text="Fast Create VM" />
		            </p>
                    <p>
                        <asp:Localize ID="locDescription" runat="server"
		                    meta:resourcekey="locDescription" Text="This wizard will Create new VM." />
                    </p>
                    <p class="warningText">
                        <asp:Localize ID="Localize2" runat="server"
		                    meta:resourcekey="locWarningCloning" Text="VM will be stopped before cloning, are you Ok with this?" />
                    </p>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                        ValidationGroup="VpsWizard" ShowMessageBox="True" ShowSummary="False" />

                    <table cellspacing="5">
				        <tr>
                            <td class="FormLabel150"><asp:Localize ID="locOperatingSystem" runat="server"
                                meta:resourcekey="locOperatingSystem" Text="Virtual Machine:"></asp:Localize></td>
                            <td>
                                <asp:DropDownList ID="listOperatingSystems" runat="server"
                                    DataValueField="Path" DataTextField="Name">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="OperatingSystemValidator" runat="server" Text="*" Display="Dynamic"
                                    ControlToValidate="listOperatingSystems" meta:resourcekey="OperatingSystemValidator" SetFocusOnError="true"
                                    ValidationGroup="VpsWizard">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr>
                            <td class="FormLabel150" valign="top"><asp:Localize ID="Localize3" runat="server"
                                meta:resourcekey="VMName" Text="VM Name:"></asp:Localize></td>
                            <td>
                                <asp:TextBox id="txtVmName" runat="server" ValidationGroup="VpsWizard">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="vmNameValidator" runat="server" Text="*" Display="Dynamic"
                                    ControlToValidate="txtVmName" meta:resourcekey="vmNameValidator" SetFocusOnError="true"
                                    ValidationGroup="VpsWizard">*</asp:RequiredFieldValidator>

                            </td>
				        </tr>
				    </table>
                    <p>
                        <asp:Button ID="btnCreate" runat="server" meta:resourcekey="btnCreate"
                            ValidationGroup="Tools" Text="Create" CssClass="Button1" 
                            onclick="btnCreate_Click" />
                        <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel"
                            CausesValidation="false" Text="Cancel" CssClass="Button1" 
                            onclick="btnCancel_Click" />
                    </p>
			    </div>
		    </div>
		    <div class="Right">
			    <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
		    </div>
	    </div>
    	
    </div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditServer.ascx.cs" Inherits="WebsitePanel.Portal.ServersEditServer" %>
<%@ Register Src="ServerServicesControl.ascx" TagName="ServerServicesControl" TagPrefix="uc4" %>
<%@ Register Src="ServerIPAddressesControl.ascx" TagName="ServerIPAddressesControl" TagPrefix="uc2" %>
<%@ Register Src="ServerDnsRecordsControl.ascx" TagName="ServerDnsRecordsControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>
<%@ Register TagPrefix="wsp" TagName="ProductVersion" Src="SkinControls/ProductVersion.ascx" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<asp:ValidationSummary ID="summary" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="Server" />
<div class="FormBody">
<table width="100%" cellpadding="0" cellspacing="0">
    <tr>
        <td class="Normal" style="width: 450px;">
            <table width="100%">
                <tr>
                    <td class="Normal" width="100%">
                        <asp:TextBox ID="txtName" runat="server" CssClass="HugeTextBox" Width="430px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="VirtualServerNameValidator" runat="server" ControlToValidate="txtName"
                     ValidationGroup="Server" meta:resourcekey="ServerNameValidator"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="Normal">
                        <asp:TextBox ID="txtComments" runat="server" CssClass="NormalTextBox"
                        Width="430px" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
            </table>
            <wsp:CollapsiblePanel id="ConnectionHeader" runat="server" IsCollapsed="true"
                TargetControlID="ConnectionPanel" resourcekey="ConnectionHeader" Text="Connection Settings">
            </wsp:CollapsiblePanel>
            <asp:Panel ID="ConnectionPanel" runat="server" Height="0" style="overflow:hidden;">
                <table>
                    <tr>
                        <td class="SubHead" style="width:300px">
                            <asp:Label ID="lblServerUrl" runat="server" meta:resourcekey="lblServerUrl"></asp:Label></td>
                        <td class="Normal">
                            <asp:TextBox ID="txtUrl" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="SubHead" valign="top">
                            <asp:Label ID="lblNewPassword" runat="server" meta:resourcekey="lblNewPassword"></asp:Label></td>
                        <td class="Normal">
                            <uc1:PasswordControl id="serverPassword" runat="server" ValidationGroup="ServerPassword">
                            </uc1:PasswordControl>
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal"></td>
                        <td class="Normal">
                            <asp:Button ID="btnChangeServerPassword" meta:resourcekey="btnChangeServerPassword" runat="server" CssClass="Button3" CausesValidation="true" OnClick="btnChangeServerPassword_Click" ValidationGroup="ServerPassword" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <wsp:CollapsiblePanel id="ADHeader" runat="server" IsCollapsed="true"
                TargetControlID="ADPanel" resourcekey="ADHeader" Text="Active Directory Settings">
            </wsp:CollapsiblePanel>
            <asp:Panel ID="ADPanel" runat="server" Height="0" style="overflow:hidden;">
                <table>
                    <tr>
                        <td class="SubHead" valign="top" width="150px">
                            <asp:Label ID="lblSecurityMode" runat="server" meta:resourcekey="lblSecurityMode"></asp:Label>
                        </td>
                        <td class="Normal" valign="top">
                            <asp:RadioButtonList ID="rbUsersCreationMode" runat="server" resourcekey="rbUsersCreationMode" CssClass="Normal">
                                <asp:ListItem Value="0">LocalAccounts</asp:ListItem>
                                <asp:ListItem Value="1">ADAccounts</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead">
                            <asp:Label ID="lblAuthType" runat="server" meta:resourcekey="lblAuthType"></asp:Label></td>
                        <td class="Normal">
                            <asp:DropDownList ID="ddlAdAuthType" runat="server" meta:resourcekey="ddlAdAuthType" CssClass="NormalTextBox">
                                <asp:ListItem Value="Secure">Secure</asp:ListItem>
                                <asp:ListItem Value="Delegation">Delegation</asp:ListItem>
                                <asp:ListItem Value="Anonymous">Anonymous</asp:ListItem>
                                <asp:ListItem Value="None">None</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead">
                            <asp:Label ID="lblAdDomain" runat="server" meta:resourcekey="lblAdDomain"></asp:Label></td>
                        <td class="Normal">
                            <asp:TextBox ID="txtDomainName" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="SubHead">
                            <asp:Label ID="lblAdUsername" runat="server" meta:resourcekey="lblAdUsername"></asp:Label></td>
                        <td class="Normal">
                            <asp:TextBox ID="txtAdUsername" runat="server" CssClass="NormalTextBox"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="SubHead" valign="top">
                            <asp:Label ID="lblAdPassword" runat="server" meta:resourcekey="lblAdPassword"></asp:Label></td>
                        <td class="Normal">
                            <uc1:PasswordControl id="adPassword" runat="server"
                                ValidationEnabled="false" ValidationGroup="ADPassword">
                            </uc1:PasswordControl>
                        </td>
                    </tr>
                    <tr>
                        <td class="Normal"></td>
                        <td class="Normal">
                            <asp:Button ID="btnChangeADPassword" meta:resourcekey="btnChangeADPassword" runat="server" CssClass="Button3" CausesValidation="true" OnClick="btnChangeADPassword_Click" ValidationGroup="ADPassword" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <wsp:CollapsiblePanel id="InstantAliasHeader" runat="server" IsCollapsed="true"
                TargetControlID="InstantAliasPanel" resourcekey="InstantAliasHeader" Text="Instant Alias">
            </wsp:CollapsiblePanel>
            <asp:Panel ID="InstantAliasPanel" runat="server" Height="0" style="overflow:hidden;">
                <table width="100%">
                    <tr>
                        <td class="Normal">
                            customerdomain.com.&nbsp;<asp:TextBox ID="txtInstantAlias" runat="server" CssClass="NormalTextBox" Width="200px" CausesValidation="true"></asp:TextBox>
                            <asp:RegularExpressionValidator id="DomainFormatValidator" ValidationGroup="Server" runat="server" meta:resourcekey="DomainFormatValidator"
		    ControlToValidate="txtInstantAlias" Display="Dynamic" SetFocusOnError="true"
		    ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,6}$"></asp:RegularExpressionValidator></td>
                    </tr>
                </table>
            </asp:Panel>
            <wsp:CollapsiblePanel id="IPAddressesHeader" runat="server" IsCollapsed="true"
                TargetControlID="IPAddressesPanel" resourcekey="IPAddressesHeader" Text="IP Addresses">
            </wsp:CollapsiblePanel>
            <asp:Panel ID="IPAddressesPanel" runat="server" Height="0" style="overflow:hidden;">
                <table width="100%">
                    <tr>
                        <td>
                            <uc2:ServerIPAddressesControl id="ServerIPAddressesControl1" runat="server">
                            </uc2:ServerIPAddressesControl></td>
                    </tr>
                </table>
            </asp:Panel>
            <wsp:CollapsiblePanel id="ServicesHeader" runat="server"
                TargetControlID="ServicesPanel" resourcekey="ServicesHeader" Text="Services">
            </wsp:CollapsiblePanel>
            <asp:Panel ID="ServicesPanel" runat="server" Height="0" style="overflow:hidden;">
                <table width="100%">
                    <tr>
                        <td>
                            <uc4:ServerServicesControl id="ServerServicesControl1" runat="server">
                            </uc4:ServerServicesControl>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <wsp:CollapsiblePanel id="DnsRecordsHeader" runat="server" IsCollapsed="true"
                TargetControlID="DnsRecordsPanel" resourcekey="DnsRecordsHeader" Text="DNS Records Template">
            </wsp:CollapsiblePanel>
            <asp:Panel ID="DnsRecordsPanel" runat="server" Height="0" style="overflow:hidden;">
                <table width="100%">
                    <tr>
                        <td>
                            <uc3:ServerDnsRecordsControl id="ServerDnsRecordsControl1" runat="server">
                            </uc3:ServerDnsRecordsControl>
                            
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
        <td class="Normal" width="40" nowrap>&nbsp;</td>
        <td class="Normal" width="200" nowrap valign="top">
            <table class="Toolbox" cellpadding="7" width="100%">
                <tr>
                    <td class="SmallBold">
                        <asp:Label ID="lblServerTools" runat="server" meta:resourcekey="lblServerTools" Text="Server Tools"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="Normal">
                        &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="lnkTerminalSessions" runat="server"
                            meta:resourcekey="lnkTerminalConnections" Text="Remote Desktop Sessions"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td class="Normal">
                        &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="lnkWindowsServices" runat="server"
                            meta:resourcekey="lnkWindowsServices" Text="Windows Services"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td class="Normal">
                        &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="lnkWindowsProcesses" runat="server"
                            meta:resourcekey="lnkWindowsProcesses" Text="System Processes"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td class="Normal">
                        &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="lnkEventViewer" runat="server"
                            meta:resourcekey="lnkEventViewer" Text="Event Viewer"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td class="Normal">
                        &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="lnkServerReboot" runat="server"
                            meta:resourcekey="lnkServerReboot" Text="Server Reboot"></asp:HyperLink>
                    </td>
                </tr>
            </table>
            <br />
            <table class="Toolbox" cellpadding="7" width="100%">
                <tr>
                    <td class="SmallBold">
                        <asp:Label ID="lblRecoveryTools" runat="server" meta:resourcekey="lblRecoveryTools" Text="Server Recovery"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="Normal">
                        &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="lnkBackup" runat="server"
                            meta:resourcekey="lnkBackup" Text="Backup"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td class="Normal">
                        &nbsp;&nbsp;&nbsp;<asp:HyperLink ID="lnkRestore" runat="server"
                            meta:resourcekey="lnkRestore" Text="Restore"></asp:HyperLink>
                    </td>
                </tr>
            </table>
            <br />
        <table class="Toolbox" cellpadding="7" width="100%">
                <tr>
                    <td class="SmallBold">
                        <asp:Label ID="lblServerVersion" runat="server" meta:resourcekey="lblServerVersion" Text="WebsitePanel Server Version"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="Normal">
                        &nbsp;&nbsp;<asp:Localize ID="locVersion" runat="server" meta:resourcekey="locVersion" /> <asp:Label id="wspVersion" runat="server"/>
                    </td>
                </tr>
               </table>
         </td>
       </tr>
</table>
</div>
<div class="FormFooter">
    <asp:Button ID="btnUpdate" runat="server" meta:resourcekey="btnUpdate" ValidationGroup="Server" Text="Update" CssClass="Button1" OnClick="btnUpdate_Click" OnClientClick="ShowProgressDialog('Updating Server...');"/>
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel" CausesValidation="false" CssClass="Button1" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" Text="Delete" CausesValidation="false" CssClass="Button1" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete server?');" /></td>
</div>
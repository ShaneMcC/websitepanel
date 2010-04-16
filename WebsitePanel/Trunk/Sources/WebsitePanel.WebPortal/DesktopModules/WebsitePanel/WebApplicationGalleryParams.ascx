<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebApplicationGalleryParams.ascx.cs"
    Inherits="WebsitePanel.Portal.WebApplicationGalleryParams" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="installerapplicationheader.ascx" TagName="ApplicationHeader" TagPrefix="dnc" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="wsp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="uc1" %>
<%@ Register src="WebApplicationGalleryHeader.ascx" tagname="WebApplicationGalleryHeader" tagprefix="uc4" %>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<div class="FormBody">
    <uc1:SimpleMessageBox ID="messageBox" runat="server" />
    
    <uc4:WebApplicationGalleryHeader ID="WebApplicationGalleryHeader1" 
        runat="server" />
    
    <table width="100%" runat="server" id="applicationUrl" visible="false">
        <tr>
            <td align="center">
                <asp:HyperLink runat="server" ID="hlApplication" meta:resourcekey="hlApplication"
                    CssClass="MediumBold" Target="_blank" />
            </td>
        </tr>
    </table>
    <!-- location -->
    <wsp:CollapsiblePanel id="secLocation" runat="server" TargetControlID="LocationPanel"
        meta:resourcekey="secLocation">
    </wsp:CollapsiblePanel>
    <asp:Panel ID="LocationPanel" runat="server" Height="0" Style="overflow: hidden;">
        <table cellpadding="2" width="100%">
            <tr>
                <td class="SubHead" nowrap width="200">
                    <asp:Label ID="lblInstallOnWebSite" runat="server" meta:resourcekey="lblInstallOnWebSite"></asp:Label>
                </td>
                <td width="100%">
                    <asp:DropDownList ID="ddlWebSite" runat="server" DataValueField="Id" DataTextField="Name"
                        CssClass="NormalTextBox">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="valRequireWebSite" runat="server" CssClass="NormalBold"
                        ControlToValidate="ddlWebSite" Display="Dynamic" ErrorMessage="Select web site to install on">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <!-- WARNING: Do not remove the placeholder if decide to change HTML layout for this block -->
            <asp:PlaceHolder runat="server" ID="pnlVirtualDir">
            <tr>
                <td class="SubHead" nowrap width="200">
                    <asp:Label ID="lblInstallOnDirectory" runat="server" meta:resourcekey="lblInstallOnDirectory"></asp:Label>
                </td>
                <td width="100%">
                    <uc2:UsernameControl id="directoryName" runat="server" RequiredField="false"></uc2:UsernameControl>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td class="Normal">
                    <asp:Label ID="lblLeaveThisFieldBlank" runat="server" meta:resourcekey="lblLeaveThisFieldBlank"></asp:Label>
                    <br />
                    <br />
                </td>
            </tr>
            </asp:PlaceHolder>
            <!-- End of placeholder -->
        </table>
    </asp:Panel>
    <!-- database -->
    <div id="divDatabase" runat="server">
        <wsp:CollapsiblePanel id="secDatabase" runat="server" TargetControlID="DatabasePanel"
            meta:resourcekey="secDatabase" Text="Configure Database">
        </wsp:CollapsiblePanel>
        <asp:Panel ID="DatabasePanel" runat="server" Height="0" Style="overflow: hidden;">
            <table cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td class="NormalBold" width="200" height="50" valign="middle" nowrap>
                        <asp:DropDownList ID="ddlDatabaseGroup" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDatabaseGroup_SelectedIndexChanged"
                            CssClass="NormalTextBox">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="Normal">
                        <fieldset>
                            <legend class="NormalBold">
                                <asp:RadioButtonList ID="rblDatabase" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" resourcekey="rblDatabase" AutoPostBack="True" OnSelectedIndexChanged="rblDatabase_SelectedIndexChanged">
                                    <asp:ListItem Value="New" Selected="True">NewDatabase</asp:ListItem>
                                    <asp:ListItem Value="Existing">ExistingDatabase</asp:ListItem>
                                </asp:RadioButtonList>
                                &nbsp; </legend>
                            <table id="tblNewDatabase" runat="server" cellpadding="3">
                                <tr>
                                    <td class="NormalBold" width="190" nowrap>
                                        <asp:Label ID="lblDatabaseName" runat="server" meta:resourcekey="lblDatabaseName"></asp:Label>
                                    </td>
                                    <td width="100%" class="Normal">
                                        <uc2:UsernameControl id="databaseName" runat="server"></uc2:UsernameControl>
                                    </td>
                                </tr>
                            </table>
                            <table id="tblExistingDatabase" runat="server" cellpadding="3">
                                <tr>
                                    <td class="NormalBold" width="190" nowrap>
                                        <asp:Label ID="lblExistingDatabaseName" runat="server" meta:resourcekey="lblDatabaseName"></asp:Label>
                                    </td>
                                    <td width="100%" class="Normal">
                                        <asp:DropDownList ID="ddlDatabase" runat="server" DataValueField="Id" DataTextField="Name"
                                            CssClass="NormalTextBox">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="valRequireDatabase" runat="server" ControlToValidate="ddlDatabase"
                                            CssClass="NormalBold" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td class="Normal">
                        <fieldset>
                            <legend class="NormalBold">
                                <asp:RadioButtonList ID="rblUser" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                    resourcekey="rblUser" AutoPostBack="True" OnSelectedIndexChanged="rblDatabase_SelectedIndexChanged">
                                    <asp:ListItem Value="New" Selected="True">NewUser</asp:ListItem>
                                    <asp:ListItem Value="Existing">ExistingUser</asp:ListItem>
                                </asp:RadioButtonList>
                                &nbsp; </legend>
                            <table cellpadding="3">
                                <tr id="rowNewUser" runat="server">
                                    <td class="NormalBold" width="190" nowrap>
                                        <asp:Label ID="lblUsername" runat="server" meta:resourcekey="lblUsername"></asp:Label>
                                    </td>
                                    <td width="100%" class="Normal">
                                        <uc2:UsernameControl id="databaseUser" runat="server"></uc2:UsernameControl>
                                    </td>
                                </tr>
                                <tr id="rowExistingUser" runat="server">
                                    <td class="NormalBold" width="190" nowrap>
                                        <asp:Label ID="lblUsername2" runat="server" meta:resourcekey="lblUsername"></asp:Label>
                                    </td>
                                    <td width="100%" class="Normal">
                                        <asp:DropDownList ID="ddlUser" runat="server" DataValueField="Id" DataTextField="Name"
                                            CssClass="NormalTextBox">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="valRequireUser" runat="server" ControlToValidate="ddlUser"
                                            CssClass="NormalBold" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="NormalBold" valign="top">
                                        <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword"></asp:Label>
                                    </td>
                                    <td class="Normal">
                                        <uc3:PasswordControl id="databasePassword" runat="server"></uc3:PasswordControl>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <br />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <div id="divSettings" runat="server">
        <!-- app settings -->
        <wsp:CollapsiblePanel id="secAppSettings" runat="server" TargetControlID="SettingsPanel"
            meta:resourcekey="secAppSettings" Text="Application settings">
        </wsp:CollapsiblePanel>
        <asp:Panel ID="SettingsPanel" runat="server" Height="0" Style="overflow: hidden;">
            <table id="Table2" width="100%" runat="server">
                <tr>
                    <td class="Normal">
                        <asp:GridView runat="server" ID="gvParams" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="190px">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblParamName" Text='<%# Eval("FriendlyName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:TextBox CssClass="NormalTextBox" runat="server" Text='<%# Eval("DefaultValue") %>'
                                            ToolTip='<%# Eval("Description") %>' TextMode='<%# GetFieldTextMode((string)Eval("Name")) %>' ID="txtParamValue" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</div>
<div class="FormFooter">
    <asp:Button ID="btnInstall" runat="server" meta:resourcekey="btnInstall" Text="Install"
        CssClass="Button1" OnClick="btnInstall_Click" OnClientClick="ShowProgressDialog('Installing application...');"/>
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel"
        CssClass="Button1" CausesValidation="false" OnClick="btnCancel_Click" />
    <asp:Button runat="server" ID="btnOK" Visible="false" meta:resourcekey="btnOK" CssClass="Button1"
        OnClick="btnOK_Click" />
</div>

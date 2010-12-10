<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebApplicationGalleryParams.ascx.cs"
    Inherits="WebsitePanel.Portal.WebApplicationGalleryParams" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="installerapplicationheader.ascx" TagName="ApplicationHeader" TagPrefix="dnc" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="uc1" %>
<%@ Register src="WebApplicationGalleryHeader.ascx" tagname="WebApplicationGalleryHeader" tagprefix="uc4" %>
<%@ Register src="WebApplicationGalleryParamControl.ascx" tagname="Parameter" tagprefix="wag" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<div class="FormBody">
    <uc1:SimpleMessageBox ID="messageBox" runat="server" />
    
    <uc4:WebApplicationGalleryHeader ID="appHeader" runat="server" />

    <div id="urlPanel" runat="server" style="padding: 20px;text-align:center;" visible="false">
        <asp:HyperLink runat="server" ID="hlApplication" meta:resourcekey="hlApplication" CssClass="MediumBold" Target="_blank" />
    </div>

    <div id="tempUrlPanel" runat="server" style="padding: 10px;" visible="false">
        <asp:Literal ID="tempUrl" runat="server"></asp:Literal>
    </div>

    <wsp:CollapsiblePanel id="secAppSettings" runat="server" TargetControlID="SettingsPanel"
        meta:resourcekey="secAppSettings" Text="Application Information">
    </wsp:CollapsiblePanel>
    <asp:Panel ID="SettingsPanel" runat="server" Height="0" Style="overflow: hidden;">

        <fieldset>
            <legend>
                <asp:Localize ID="lblInstallOnWebSite" runat="server" meta:resourcekey="lblInstallOnWebSite"></asp:Localize>
            </legend>
            <div class="FormFieldDescription">
                <asp:Localize ID="locWebSiteDescription" meta:resourcekey="locWebSiteDescription" runat="server"></asp:Localize>
            </div>
            <div class="FormField">
                <asp:DropDownList ID="ddlWebSite" runat="server" DataValueField="Id" DataTextField="Name" Width="600px"
                    CssClass="NormalTextBox">
                </asp:DropDownList>
                <div>
                    <asp:RequiredFieldValidator ID="valRequireWebSite" runat="server" ValidationGroup="wag" SetFocusOnError="true" meta:resourcekey="valRequireWebSite"
                        ControlToValidate="ddlWebSite" Display="Dynamic" Text="*">*</asp:RequiredFieldValidator>
                </div>
            </div>
        </fieldset>

        <fieldset>
            <legend>
                <asp:Localize ID="lblInstallOnDirectory" runat="server" meta:resourcekey="lblInstallOnDirectory"></asp:Localize>
            </legend>
            <div class="FormField">
                <uc2:UsernameControl id="directoryName" runat="server" RequiredField="false" width="600px"></uc2:UsernameControl>
            </div>
            <div class="FormFieldDescription">
                <asp:Localize ID="lblLeaveThisFieldBlank" runat="server" meta:resourcekey="lblLeaveThisFieldBlank"></asp:Localize>
            </div>
        </fieldset>
        
        <div id="divDatabase" runat="server">

            <%-- database engine --%>
            <fieldset id="databaseEngineBlock" runat="server">
                <legend>
                    <asp:Localize ID="locDatabaseGroup" meta:resourcekey="locDatabaseGroup" runat="server"></asp:Localize>
                </legend>
                <div class="FormFieldDescription">
                    <asp:Localize ID="locDatabaseGroupDescr" meta:resourcekey="locDatabaseGroupDescr" runat="server"></asp:Localize>
                </div>
                <div class="FormField">
                    <asp:DropDownList ID="databaseEngines" runat="server" AutoPostBack="true" Width="600px"
                        OnSelectedIndexChanged="databaseEngines_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </fieldset>

            <%-- existing or new database --%>
            <fieldset id="databaseModeBlock" runat="server">
                <legend>
                    <asp:Localize ID="locNewDatabase" meta:resourcekey="locNewDatabase" runat="server"></asp:Localize>
                </legend>
                <div class="FormFieldDescription">
                    <asp:Localize ID="locNewDatabaseDescr" meta:resourcekey="locNewDatabaseDescr" runat="server"></asp:Localize>
                </div>
                <div class="FormField">
                    <asp:DropDownList ID="databaseMode" runat="server"
                        resourcekey="rblDatabase" AutoPostBack="True" Width="600px"
                        onselectedindexchanged="databaseMode_SelectedIndexChanged">
                        <asp:ListItem Value="new" Selected="True">NewDatabase</asp:ListItem>
                        <asp:ListItem Value="existing">ExistingDatabase</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </fieldset>

        </div>

        <%-- application parameters --%>
        <asp:Repeater ID="repParams" runat="server" 
            onitemdatabound="repParams_ItemDataBound">
            <ItemTemplate>
                <wag:Parameter id="param" runat="server"></wag:Parameter>
            </ItemTemplate>
        </asp:Repeater>

    </asp:Panel>
</div>


<div class="FormFooter">
    <asp:Button ID="btnInstall" runat="server" meta:resourcekey="btnInstall" Text="Install" ValidationGroup="wag"
        CssClass="Button1" OnClick="btnInstall_Click" OnClientClick="ShowProgressDialog('Installing application...');"/>
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel"
        CssClass="Button1" CausesValidation="false" OnClick="btnCancel_Click" />
    <asp:Button runat="server" ID="btnOK" Visible="false" meta:resourcekey="btnOK" CssClass="Button1"
        OnClick="btnOK_Click" />
</div>

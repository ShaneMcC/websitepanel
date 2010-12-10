<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainsAddDomain.ascx.cs" Inherits="WebsitePanel.Portal.DomainsAddDomain" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagPrefix="wsp" TagName="CollapsiblePanel" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<asp:ValidationSummary ID="summary" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="Domain" />

<div class="FormBody">

    <p id="DomainPanel" runat="server" style="padding: 15px 0 15px 5px;" visible="false">
        <asp:Label ID="DomainPrefix" runat="server" meta:resourcekey="DomainPrefix" Text="www." CssClass="Huge"></asp:Label>
        <asp:TextBox ID="DomainName" runat="server" Width="300" CssClass="HugeTextBox"></asp:TextBox>
        <asp:RequiredFieldValidator id="DomainRequiredValidator" runat="server" meta:resourcekey="DomainRequiredValidator"
            ControlToValidate="DomainName" Display="Dynamic" ValidationGroup="Domain" SetFocusOnError="true"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator id="DomainFormatValidator" runat="server" meta:resourcekey="DomainFormatValidator"
		    ControlToValidate="DomainName" Display="Dynamic" ValidationGroup="Domain" SetFocusOnError="true"
		    ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,6}$"></asp:RegularExpressionValidator>
    </p>
    
    <p id="SubDomainPanel" runat="server" style="padding: 15px 0 15px 5px;" visible="false">
        <asp:TextBox ID="SubDomainName" runat="server" Width="150" CssClass="HugeTextBox"></asp:TextBox>
        .
        <asp:DropDownList ID="DomainsList" Runat="server" CssClass="NormalTextBox" DataTextField="DomainName" DataValueField="DomainName"></asp:DropDownList>
        <asp:RequiredFieldValidator id="SubDomainRequiredValidator" runat="server" meta:resourcekey="SubDomainRequiredValidator"
            ControlToValidate="SubDomainName" Display="Dynamic" ValidationGroup="Domain" SetFocusOnError="true"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator id="SubDomainFormatValidator" runat="server" meta:resourcekey="SubDomainFormatValidator"
		    ControlToValidate="SubDomainName" Display="Dynamic" ValidationGroup="Domain" SetFocusOnError="true"
		    ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?)(\.[a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?){0,9}$"></asp:RegularExpressionValidator>
    </p>
    
    <wsp:CollapsiblePanel id="OptionsPanelHeader" runat="server"
        TargetControlID="OptionsPanel" resourcekey="OptionsPanelHeader" Text="Provisioning options">
    </wsp:CollapsiblePanel>
    <asp:Panel ID="OptionsPanel" runat="server">
        
        <br />
        <asp:Panel id="CreateWebSitePanel" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="CreateWebSite" runat="server" meta:resourcekey="CreateWebSite" Text="Create Web Site" CssClass="Checkbox Bold" Checked="true" /><br />
            <div style="padding-left: 20px;">
                <asp:Localize ID="DescribeCreateWebSite" runat="server" meta:resourcekey="DescribeCreateWebSite">Description...</asp:Localize>
            </div>
        </asp:Panel>
        
        <asp:Panel id="PointWebSitePanel" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="PointWebSite" runat="server" meta:resourcekey="PointWebSite" Text="Point to Web Site" CssClass="Checkbox Bold"
                AutoPostBack="true" /><br />
            <div style="padding-left: 20px;">
                <asp:DropDownList ID="WebSitesList" Runat="server" CssClass="NormalTextBox" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
            </div>
        </asp:Panel>
        
        <asp:Panel id="PointMailDomainPanel" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="PointMailDomain" runat="server" meta:resourcekey="PointMailDomain" Text="Point to Mail Domain" CssClass="Checkbox Bold"
                AutoPostBack="true" /><br />
            <div style="padding-left: 20px;">
                <asp:DropDownList ID="MailDomainsList" Runat="server" CssClass="NormalTextBox" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
            </div>
        </asp:Panel>
        
        <asp:Panel id="EnableDnsPanel" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="EnableDns" runat="server" meta:resourcekey="EnableDns" Text="Enable DNS" CssClass="Checkbox Bold"
                Checked="true" /><br />
            <div style="padding-left: 20px;">
                <asp:Localize ID="DescribeEnableDns" runat="server" meta:resourcekey="DescribeEnableDns">Description...</asp:Localize>
            </div>
        </asp:Panel>
        
        <asp:Panel id="InstantAliasPanel" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="CreateInstantAlias" runat="server" meta:resourcekey="CreateInstantAlias"
                Text="Create Instant Alias" CssClass="Checkbox Bold" Checked="true" /><br />
            <div style="padding-left: 20px;">
                <asp:Localize ID="DescribeCreateInstantAlias" runat="server" meta:resourcekey="DescribeCreateInstantAlias">Description...</asp:Localize>
            </div>
        </asp:Panel>
        
        <asp:Panel id="AllowSubDomainsPanel" runat="server" style="padding-bottom: 15px;">
            <asp:CheckBox ID="AllowSubDomains" runat="server" meta:resourcekey="AllowSubDomains" Text="Allow sub-domains" CssClass="Checkbox Bold" /><br />
            <div style="padding-left: 20px;">
                <asp:Localize ID="DescribeAllowSubDomains" runat="server" meta:resourcekey="DescribeAllowSubDomains">Description...</asp:Localize>
            </div>
        </asp:Panel>
        
    </asp:Panel>

</div>

<div class="FormFooter">
    <asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" CssClass="Button2" Text="Add Domain" ValidationGroup="Domain" OnClick="btnAdd_Click" OnClientClick="ShowProgressDialog('Adding Domain...');"/>
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail50_EditDomain.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.SmarterMail50_EditDomain" %>
<%@ Register Src="SmarterMail50_EditDomain_Features.ascx" TagName="SmarterMail50_EditDomain_Features"
    TagPrefix="uc4" %>
<%@ Register Src="SmarterMail50_EditDomain_Sharing.ascx" TagName="SmarterMail50_EditDomain_Sharing"
    TagPrefix="uc3" %>
<%@ Register Src="SmarterMail50_EditDomain_Throttling.ascx" TagName="SmarterMail50_EditDomain_Throttling"
    TagPrefix="uc5" %>

<%@ Register Src="../UserControls/QuotaEditor.ascx" TagName="QuotaEditor" TagPrefix="uc1" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>


<table width="100%">
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label ID="lblCatchAll" runat="server" meta:resourcekey="lblCatchAll" Text="Catch-All Account:"></asp:Label></td>
        <td class="Normal" width="100%">
            <asp:DropDownList ID="ddlCatchAllAccount" runat="server" CssClass="NormalTextBox">
            </asp:DropDownList></td>
    </tr>
</table>
<asp:Panel runat="server" ID="AdvancedSettingsPanel">



<wsp:collapsiblepanel id="secFeatures" runat="server" targetcontrolid="FeaturesPanel"
    meta:resourcekey="secFeatures" ></wsp:collapsiblepanel>
<asp:Panel runat="server" ID="FeaturesPanel">
    <uc4:SmarterMail50_EditDomain_Features id="featuresSection" runat="server"></uc4:SmarterMail50_EditDomain_Features>

</asp:Panel>


<wsp:collapsiblepanel id="secSharing" runat="server" targetcontrolid="SharingPanel"
    meta:resourcekey="secSharing" ></wsp:collapsiblepanel>

<asp:Panel runat="server" ID="SharingPanel">
    <uc3:SmarterMail50_EditDomain_Sharing id="sharingSection" runat="server"></uc3:SmarterMail50_EditDomain_Sharing>
</asp:Panel>

<wsp:collapsiblepanel id="secThrottling" runat="server" targetcontrolid="ThrottlingPanel"
    meta:resourcekey="secThrottling" ></wsp:collapsiblepanel>

<asp:Panel runat="server" ID="ThrottlingPanel">
    <uc5:SmarterMail50_EditDomain_Throttling id="throttlingSection" runat="server"></uc5:SmarterMail50_EditDomain_Throttling>
</asp:Panel>


<wsp:collapsiblepanel id="secLimits" runat="server" targetcontrolid="LimitsPanel"
    meta:resourcekey="secLimits" text="Limits"></wsp:collapsiblepanel>

<asp:Panel runat="server" ID="LimitsPanel">
    <table width="100%">
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblDomainDiskSpace" runat="server" meta:resourcekey="lblDomainDiskSpace"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server"  ID="txtSize" Text="0" Width="80px" CssClass="NormalTextBox" />
                <asp:RangeValidator Type="Integer" ID="valDomainDiskSpace" MinimumValue="0" runat="server" ControlToValidate="txtSize"
                    Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValDiskSpace" runat="server" ControlToValidate="txtSize"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblDomainAliases" runat="server" meta:resourcekey="lblDomainAliases"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" ID="txtDomainAliases" Text="0" Width="80px" CssClass="NormalTextBox"/>
                <asp:RangeValidator  Type="Integer" ID="valDomainAliases" MinimumValue="0" runat="server" ControlToValidate="txtDomainAliases"
                    Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValDomainAliases" runat="server" ControlToValidate="txtDomainAliases"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblUserQuota" runat="server" meta:resourcekey="lblUserQuota"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" ID="txtUser" Width="80px" CssClass="NormalTextBox"/>
                <asp:RangeValidator Type="Integer" MinimumValue="0" ID="valUser" runat="server" ControlToValidate="txtUser"
                    Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValUser" runat="server" ControlToValidate="txtUser"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblUserAliasesQuota" runat="server" meta:resourcekey="lblUserAliasesQuota"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" ID="txtUserAliases" Width="80px" CssClass="NormalTextBox"/>
                <asp:RangeValidator Type="Integer" runat="server" ID="valUserAliases" ControlToValidate="txtUserAliases"
                    MinimumValue="0" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValUserAliases" runat="server" ControlToValidate="txtUserAliases"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblMailingListsQuota" runat="server" meta:resourcekey="lblMailingListsQuota"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" Width="80px" ID="txtMailingLists" CssClass="NormalTextBox"/>
                <asp:RangeValidator Type="Integer" runat="server" ID="valMailingLists" ControlToValidate="txtMailingLists"
                    MinimumValue="0" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValMailingLists" runat="server" ControlToValidate="txtMailingLists"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblPopRetreivalAccounts" runat="server" meta:resourcekey="lblPopRetreivalAccounts"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" Width="80px" ID="txtPopRetreivalAccounts" CssClass="NormalTextBox"/>
            <asp:RangeValidator Type="Integer" runat="server" ID="valPopRetreivalAccounts" ControlToValidate="txtPopRetreivalAccounts"
                    MinimumValue="0" Display="None"/>
            <asp:RequiredFieldValidator ID="reqPopRetreivalAccounts" runat="server" ControlToValidate="txtPopRetreivalAccounts"
                    Display="None"/>
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap  align="right">
                <asp:Label ID="lblMessageSizeQuota" runat="server" meta:resourcekey="lblMessageSizeQuota"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" ID="txtMessageSize" CssClass="NormalTextBox" Width="80px"/>
                <asp:RangeValidator Type="Integer" runat="server" ID="valMessageSize" ControlToValidate="txtMessageSize"
                    MinimumValue="0" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValMessageSize" runat="server" ControlToValidate="txtMessageSize"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblRecipientsPerMessageQuota" runat="server" meta:resourcekey="lblRecipientsPerMessageQuota"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" ID="txtRecipientsPerMessage" CssClass="NormalTextBox" Width="80px"/>
                <asp:RangeValidator Type="Integer" runat="server" ID="valRecipientsPerMessage" ControlToValidate="txtRecipientsPerMessage"
                    MinimumValue="0" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValRecipientsPerMessage" runat="server" ControlToValidate="txtRecipientsPerMessage"
                    Display="Dynamic" />
            </td>
        </tr>
    </table>
</asp:Panel>

</asp:Panel>
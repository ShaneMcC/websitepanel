<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingAddonsEditAddon.ascx.cs" Inherits="WebsitePanel.Portal.HostingAddonsEditAddon" %>
<%@ Register Src="HostingPlansQuotas.ascx" TagName="HostingPlansQuotas" TagPrefix="uc1" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<div class="FormBody">
<asp:UpdatePanel runat="server" ID="updatePanelUsers">
    <ContentTemplate> 

<asp:Label ID="lblMessage" runat="server" CssClass="NormalBold" ForeColor="red"></asp:Label>
<table width="100%">
    <tr>
        <td class="SubHead" style="width:200px;"><asp:Label ID="lblAddOnName" runat="server" meta:resourcekey="lblAddOnName" Text="Add-On Name:"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtPlanName" runat="server" Width="300" CssClass="HugeTextBox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valPlanName" runat="server" ControlToValidate="txtPlanName"
                ErrorMessage="*"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td class="SubHead" valign="top"><asp:Label ID="lblAddOnDescription" runat="server" meta:resourcekey="lblAddOnDescription" Text="Add-On Description:"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtPlanDescription" runat="server" Width="300" Rows="4" TextMode="MultiLine" CssClass="NormalTextBox"></asp:TextBox></td>
    </tr>
</table>

<wsp:CollapsiblePanel id="secQuotas" runat="server"
    TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
</wsp:CollapsiblePanel>
<asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
    <uc1:HostingPlansQuotas id="hostingPlansQuotas" runat="server" IsPlan="false">
    </uc1:HostingPlansQuotas>
</asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>
</div>

<div class="FormFooter">
    <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="Button1" Text="Save" OnClick="btnSave_Click" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1" Text="Delete" CausesValidation="false"
        OnClick="btnDelete_Click" />
</div>

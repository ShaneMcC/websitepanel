<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingPlansEditPlan.ascx.cs" Inherits="WebsitePanel.Portal.HostingPlansEditPlan" %>
<%@ Register Src="HostingPlansQuotas.ascx" TagName="HostingPlansQuotas" TagPrefix="uc1" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<asp:Panel ID="HostingPlansEditPanel" runat="server" DefaultButton="btnSave" >

<asp:UpdatePanel runat="server" ID="updatePanelUsers" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate> 

<div class="FormBody">

<asp:Label ID="lblMessage" runat="server" CssClass="NormalBold" ForeColor="red"></asp:Label>&nbsp;
<table>
    <tr>
        <td class="SubHead" style="width:200px;"><asp:Label ID="lblPlanName" runat="server" meta:resourcekey="lblPlanName" Text="Plan Name:"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtPlanName" runat="server" Width="300" CssClass="Huge"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valPlanName" runat="server" ErrorMessage="*" ControlToValidate="txtPlanName" SetFocusOnError="True"></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td class="SubHead" valign="top"><asp:Label ID="lblPlanDescription" runat="server" meta:resourcekey="lblPlanDescription" Text="Plan Description:"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtPlanDescription" runat="server" Width="300" Rows="4" TextMode="MultiLine" CssClass="NormalTextBox"></asp:TextBox></td>
    </tr>
</table>


<wsp:CollapsiblePanel id="secTarget" runat="server"
    TargetControlID="TargetPanel" meta:resourcekey="secTarget" Text="Plan Target">
</wsp:CollapsiblePanel>
<asp:Panel ID="TargetPanel" runat="server" Height="0" style="overflow:hidden;">
<table cellpadding="3" cellspacing="0">
    <tr id="rowTargetServer" runat="server">
        <td class="SubHead" style="width:200px;">
            <asp:Label ID="lblTargetServer" runat="server" meta:resourcekey="lblTargetServer" Text="Server:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:DropDownList ID="ddlServer" runat="server" DataValueField="ServerID" DataTextField="ServerName" AutoPostBack="True" OnSelectedIndexChanged="planTarget_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="valRequireServer" runat="server" ControlToValidate="ddlServer"
                ErrorMessage="Select target server"></asp:RequiredFieldValidator></td>
    </tr>
    <tr id="rowTargetSpace" runat="server">
        <td class="SubHead" style="width:200px;">
            <asp:Label ID="lblTargetSpace" runat="server" meta:resourcekey="lblTargetSpace" Text="Hosting Space:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:DropDownList ID="ddlSpace" runat="server" DataValueField="PackageId" DataTextField="PackageName" AutoPostBack="True" OnSelectedIndexChanged="planTarget_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="valRequireSpace" runat="server" ControlToValidate="ddlSpace"
                ErrorMessage="Select target space"></asp:RequiredFieldValidator></td>
    </tr>
</table>
</asp:Panel>

<wsp:CollapsiblePanel id="secQuotas" runat="server"
    TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
</wsp:CollapsiblePanel>
<asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
    <uc1:HostingPlansQuotas id="hostingPlansQuotas" runat="server">
    </uc1:HostingPlansQuotas>
</asp:Panel>

</div>

</ContentTemplate>
</asp:UpdatePanel>

<div class="FormFooter">
    <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="Button1" Text="Save" OnClick="btnSave_Click" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1" Text="Delete" CausesValidation="false" OnClick="btnDelete_Click"
        OnClientClick="return confirm('Are you sure you want to delete hosting plan?');" />
   </div>
</asp:Panel>
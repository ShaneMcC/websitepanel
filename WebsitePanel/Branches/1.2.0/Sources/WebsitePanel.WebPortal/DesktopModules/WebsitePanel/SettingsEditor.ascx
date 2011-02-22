<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEditor.ascx.cs" Inherits="WebsitePanel.Portal.SettingsEditor" %>
<asp:UpdatePanel runat="server" ID="updatePanelUsers" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate> 

<div class="FormBody">
    <asp:DropDownList ID="ddlOverride" runat="server" CssClass="NormalTextBox"
            resourcekey="ddlOverride" AutoPostBack="true" OnSelectedIndexChanged="ddlOverride_SelectedIndexChanged">
        <asp:ListItem>UseHost</asp:ListItem>
        <asp:ListItem>OverrideHost</asp:ListItem>
    </asp:DropDownList>
    
    <br />
    <br />
    <asp:PlaceHolder ID="settingsPlace" runat="server"></asp:PlaceHolder>
</div>

    </ContentTemplate>
</asp:UpdatePanel>

<div class="FormFooter">
    <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="Button1" Text="Save" OnClick="btnSave_Click" ValidationGroup="SettingsEditor" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
</div>
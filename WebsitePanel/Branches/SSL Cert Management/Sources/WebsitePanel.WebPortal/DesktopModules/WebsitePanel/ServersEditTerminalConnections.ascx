<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditTerminalConnections.ascx.cs" Inherits="WebsitePanel.Portal.ServersEditTerminalConnections" %>
<%@ Register Src="ServerHeaderControl.ascx" TagName="ServerHeaderControl" TagPrefix="uc1" %>
<uc1:ServerHeaderControl id="ServerHeaderControl1" runat="server">
</uc1:ServerHeaderControl>

<div class="FormButtonsBar">
    <div class="Left" style="padding: 5px;">
        <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel" CssClass="Button1"
                CausesValidation="False" OnClick="btnCancel_Click" />
    </div>
    <div class="Right">
        <asp:UpdateProgress ID="updatePanelProgress" runat="server"
            AssociatedUpdatePanelID="ItemsUpdatePanel" DynamicLayout="false">
            <ProgressTemplate>
                <asp:Image ID="imgSep" runat="server" SkinID="MediumAjaxIndicator" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
</div>

<asp:UpdatePanel ID="ItemsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
        <asp:Timer runat="server" Interval="10000" ID="itemsTimer" />
        <asp:GridView ID="gvSessions" runat="server" AutoGenerateColumns="False"
            EmptyDataText="gvSessions"
            CssSelectorClass="NormalGridView" EnableViewState="false"
            DataKeyNames="SessionID" OnRowDeleting="gvSessions_RowDeleting">
            <Columns>
                <asp:BoundField DataField="SessionID" HeaderText="gvSessionsSessionID" />
                <asp:BoundField DataField="Username" HeaderText="gvSessionsUserName" />
                <asp:BoundField DataField="Status" HeaderText="gvSessionsStatus" />
                <asp:TemplateField HeaderText="gvSessionsReset">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="cmdReset" runat="server" SkinID="DeleteSmall" meta:resourcekey="cmdReset"
                            CommandName="delete" OnClientClick="return confirm('Reset session?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
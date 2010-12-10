<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceQuotasControl.ascx.cs" Inherits="WebsitePanel.Portal.SpaceQuotasControl" %>
<%@ Register Src="UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="uc1" %>

<asp:Repeater ID="dlGroups" runat="server" EnableViewState="false">
    <ItemTemplate>
        <div class="HostingPlanGroup">
            <div class="Header">
                <asp:Panel ID="GroupPanel" runat="server" CssClass="GroupName" visible='<%# IsGroupVisible((int)Eval("GroupID")) %>'>
                    <%# GetSharedLocalizedString("ResourceGroup." + (string)Eval("GroupName")) %>
                </asp:Panel>
            </div>
            <asp:Repeater ID="dlQuotas" runat="server" DataSource='<%# GetGroupQuotas((int)Eval("GroupID")) %>' EnableViewState="false">
                <ItemTemplate>
                    <div class="Quota">
                        <div class="Left">
                            <%# GetSharedLocalizedString("Quota." + (string)Eval("QuotaName"))%>:
                        </div>
                        <div class="Viewer">
                            <uc1:QuotaViewer ID="quota" runat="server"
                                QuotaTypeId='<%# Eval("QuotaTypeId") %>'
                                QuotaUsedValue='<%# Eval("QuotaUsedValue") %>'
                                QuotaValue='<%# Eval("QuotaValue") %>' />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </ItemTemplate>
</asp:Repeater>

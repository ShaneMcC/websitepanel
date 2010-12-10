<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingPlansQuotas.ascx.cs" Inherits="WebsitePanel.Portal.HostingPlansQuotas" %>
<%@ Register Src="UserControls/QuotaEditor.ascx" TagName="QuotaEditor" TagPrefix="uc1" %>

<asp:Repeater ID="dlGroups" runat="server">
    <ItemTemplate>
        <div class="HostingPlanGroup">
            <asp:Panel ID="GroupPanel" runat="server" CssClass="Header">
                <div class="Left">
                    <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# (bool)Eval("Enabled") & (bool)Eval("ParentEnabled") %>'
                        AutoPostBack="true"
                        Enabled='<%# Eval("ParentEnabled") %>'
                        Text='<%# GetSharedLocalizedString("ResourceGroup." + (string)Eval("GroupName")) %>' />
                    <asp:Literal ID="groupId" runat=server Text='<%# Eval("GroupID") %>' Visible=false></asp:Literal>
                </div>
                <div class="Right">
                    <asp:CheckBox ID="chkCountDiskspace" runat="server" meta:resourcekey="chkCountDiskspace" Checked='<%# Eval("CalculateDiskspace") %>' Visible='<%# IsPlan %>' Text="Count Diskspace" />&nbsp;
                    <asp:CheckBox ID="chkCountBandwidth" runat="server" meta:resourcekey="chkCountBandwidth" Checked='<%# Eval("CalculateBandwidth") %>' Visible='<%# IsPlan %>' Text="Count Bandwidth" />&nbsp;
                </div>
            </asp:Panel>
            
            <asp:Panel ID="QuotaPanel" runat="server">
                <asp:DataList ID="dlQuotas" runat="server"
                    DataSource='<%# GetGroupQuotas((int)Eval("GroupID")) %>'
                    RepeatColumns="1">
                    <ItemTemplate>
                        <div class="Quota">
                            <div class="Left">
                                <%# GetSharedLocalizedString("Quota." + (string)Eval("QuotaName"))%>:
                            </div>
                            <div class="Right">
                                <uc1:QuotaEditor id="quotaEditor" runat="server"
                                    QuotaID='<%# Eval("QuotaID") %>'
                                    QuotaTypeID='<%# Eval("QuotaTypeID") %>'
                                    QuotaValue='<%# Eval("QuotaValue") %>'
                                    ParentQuotaValue='<%# Eval("ParentQuotaValue") %>'>
                                </uc1:QuotaEditor>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>   
            </asp:Panel>
        </div>
    </ItemTemplate>
</asp:Repeater>
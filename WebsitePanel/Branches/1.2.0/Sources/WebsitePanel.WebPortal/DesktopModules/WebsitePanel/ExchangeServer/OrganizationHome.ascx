<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationHome.ascx.cs"
    Inherits="WebsitePanel.Portal.ExchangeServer.OrganizationHome" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="wsp" %>
<div id="ExchangeContainer">
    <div class="Module">
        <div class="Header">
            <wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
        </div>
        <div class="Left">
            <wsp:Menu id="menu" runat="server" SelectedItem="organization_home" />
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="Image1" SkinID="Organization48" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Welcome"></asp:Localize>
                </div>
                <div class="FormBody">
                    <table>
                        <tr class="OrgStatsRow">
                            <td align="right">
                                <asp:Label runat="server" ID="lblOrganizationName" meta:resourcekey="lblOrganizationName" />
                            </td>
                            <td>
                                <asp:Label CssClass="Huge" runat="server" ID="lblOrganizationNameValue" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td >
                                <asp:Label runat="server" meta:resourcekey="lblOrganizationID" ID="lblOrganizationID" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblOrganizationIDValue" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td>
                                <asp:Label runat="server" meta:resourcekey="lblCreated" ID="lblCreated" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCreatedValue" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%">
                        <tr>
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locHeadStatistics" runat="server" meta:resourcekey="locHeadStatistics"
                                    Text="Organization Statistics"></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
                                <asp:HyperLink ID="lnkDomains" runat="server" meta:resourcekey="lnkDomains"></asp:HyperLink>
                            </td>
                            <td width="100%">
                                <wsp:QuotaViewer ID="domainStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
                                <asp:HyperLink ID="lnkUsers" runat="server" meta:resourcekey="lnkUsers"></asp:HyperLink>
                            </td>
                            <td>
                                <wsp:QuotaViewer ID="userStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
                                <asp:Literal runat="server" ID="litTotalUserDiskSpace" meta:resourcekey="litTotalUserDiskSpace" />
                            </td>
                            <td><asp:Literal runat="server" ID="litTotalDiskSpaceValue" /></td>
                        </tr>
                        
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <asp:Panel runat="server" ID="exchangeStatsPanel">
                        <tr>
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locExchangeStatistics" runat="server" meta:resourcekey="locExchangeStatistics" ></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow"> 
                            <td align="right" nowrap>
                                <asp:HyperLink ID="lnkMailboxes" runat="server" meta:resourcekey="lnkMailboxes" />
                            </td>
                            <td>
                                <wsp:QuotaViewer ID="mailboxesStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
                                <asp:HyperLink ID="lnkContacts" runat="server" meta:resourcekey="lnkContacts"></asp:HyperLink>
                            </td>
                            <td>
                                <wsp:QuotaViewer ID="contactsStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
                                <asp:HyperLink ID="lnkLists" runat="server" meta:resourcekey="lnkLists"></asp:HyperLink>
                            </td>
                            <td>
                                <wsp:QuotaViewer ID="listsStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
                                <asp:HyperLink ID="lnkFolders" runat="server" meta:resourcekey="lnkFolders"></asp:HyperLink>
                            </td>
                            <td>
                                <wsp:QuotaViewer ID="foldersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
                                <asp:Literal runat="server" ID="Literal1" meta:resourcekey="litUsedDiskSpace" />
                            </td>
                            <td><asp:HyperLink runat="server" ID="lnkUsedExchangeDiskSpace" /> </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>                        
                        </asp:Panel>
                        
                        <asp:Panel runat="server" ID="sharePointStatsPanel">
                        <tr class="OrgStatsRow">
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locSharePoint" runat="server" meta:resourcekey="locSharePoint"
                                    Text="Organization Statistics"></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap> 
                                <asp:HyperLink ID="lnkSiteCollections" runat="server" meta:resourcekey="lnkSiteCollections"></asp:HyperLink>
                            </td>
                            <td>
                                <wsp:QuotaViewer ID="siteCollectionsStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        </asp:Panel>
                        
                        <asp:Panel runat="server" ID="crmStatsPanel">
                        <tr >
                            <td class="OrgStatsGroup" width="100%" colspan="2">
                                <asp:Localize ID="locCRM" runat="server" meta:resourcekey="locCRM"
                                    Text="Organization Statistics"></asp:Localize>
                            </td>
                        </tr>
                        <tr class="OrgStatsRow">
                            <td align="right" nowrap>
                                <asp:HyperLink ID="lnkCRMUsers" runat="server" meta:resourcekey="lnkCRMUsers"></asp:HyperLink>
                            </td>
                            <td>
                                <wsp:QuotaViewer ID="crmUsersStats" QuotaTypeId="2" runat="server" DisplayGauge="true" />
                            </td>
                        </tr>
                        </asp:Panel>
                    </table>
                  
                </div>
            </div>
            <div class="Right">
                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="HSFormComments"></asp:Localize>
            </div>
        </div>
    </div>
</div>

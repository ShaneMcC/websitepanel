<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailboxTabs.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.UserControls.MailboxTabs" %>
<table width="100%" cellpadding="0" cellspacing="1">
    <tr>
        <td>
            
            <asp:DataList ID="dlTabs" runat="server" RepeatDirection="Horizontal"
                RepeatLayout="Flow" EnableViewState="false"  RepeatColumns="6" ItemStyle-Height="28px"  SeparatorStyle-CssClass="Separator" SeparatorStyle-Height="22px" >
                <ItemStyle Wrap="False" />
                <ItemTemplate >
                    <asp:HyperLink ID="lnkTab" runat="server" CssClass="Tab" NavigateUrl='<%# Eval("Url") %>'>
                        <%# Eval("Name") %>
                    </asp:HyperLink>
                </ItemTemplate>
                <SelectedItemStyle Wrap="False" />
                <SelectedItemTemplate>
                    <asp:HyperLink ID="lnkSelTab" runat="server" CssClass="ActiveTab" NavigateUrl='<%# Eval("Url") %>'>
                        <%# Eval("Name") %>
                    </asp:HyperLink>
                </SelectedItemTemplate>
                <SeparatorTemplate>
                    &nbsp;
                </SeparatorTemplate>
            </asp:DataList>
        </td>
    </tr>
</table>
<br />
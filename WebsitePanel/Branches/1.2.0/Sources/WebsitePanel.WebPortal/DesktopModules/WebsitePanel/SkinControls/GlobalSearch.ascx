<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalSearch.ascx.cs" Inherits="WebsitePanel.Portal.SkinControls.GlobalSearch" %>
<asp:UpdatePanel runat="server" ID="updatePanelUsers" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
<table cellpadding="0" cellspacing="0" align="right">
    <tr>
        <td align="left">
            <asp:DataList ID="dlTabs" runat="server" RepeatDirection="Horizontal"
                OnSelectedIndexChanged="dlTabs_SelectedIndexChanged" RepeatLayout="Table">
                <ItemStyle Wrap="false" VerticalAlign="Top" />
                <ItemTemplate>
                    <asp:LinkButton ID="lnkTab" runat="server" CommandName="select"
                        CausesValidation="false" CssClass="SearchMethod">
                        <%# Eval("Name") %>
                    </asp:LinkButton>
                </ItemTemplate>
                <SelectedItemStyle Wrap="false" />
                <SelectedItemTemplate>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="SearchMethodSide" valign="top"></td>
                            <td class="SearchMethodSelected" valign="top"><%# Eval("Name")%></td>
                            <td class="SearchMethodSide" valign="top"></td>
                        </tr>
                    </table>
                </SelectedItemTemplate>
            </asp:DataList>
        </td>
    </tr>
    <tr>
        <td align="left" class="SearchQuery">
            <asp:MultiView ID="tabs" runat="server" ActiveViewIndex="0">
                <asp:View ID="tabSearchUsers" runat="server">
                    <asp:Panel ID="pnlSearchUsers" runat="server" DefaultButton="btnSearchUsers">
                        <asp:DropDownList ID="ddlUserFields" runat="server" resourcekey="ddlUserFields" CssClass="NormalTextBox" Width="150px" style="vertical-align: middle;">
                            <asp:ListItem Value="Username">Username</asp:ListItem>
                            <asp:ListItem Value="Email">Email</asp:ListItem>
                            <asp:ListItem Value="FullName">FullName</asp:ListItem>
                            <asp:ListItem Value="CompanyName">CompanyName</asp:ListItem>
                        </asp:DropDownList><asp:TextBox ID="txtUsersQuery" runat="server" CssClass="NormalTextBox" Width="120px" style="vertical-align: middle;"></asp:TextBox><asp:ImageButton ID="btnSearchUsers" runat="server" SkinID="SearchButton" OnClick="btnSearchUsers_Click" CausesValidation="false" style="vertical-align: middle;" />
                    </asp:Panel>
                </asp:View>
                <asp:View ID="tabSearchSpaces" runat="server">
                    <asp:Panel ID="pnlSearchSpaces" runat="server" DefaultButton="btnSearchSpaces">
                        <asp:DropDownList ID="ddlItemType" runat="server" Width="150px" CssClass="NormalTextBox" style="vertical-align: middle;">
                        </asp:DropDownList><asp:TextBox ID="txtSpacesQuery" runat="server" CssClass="NormalTextBox" Width="120px" style="vertical-align: middle;"></asp:TextBox><asp:ImageButton ID="btnSearchSpaces" runat="server" SkinID="SearchButton" OnClick="btnSearchSpaces_Click" CausesValidation="false" style="vertical-align: middle;" />
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>

    </ContentTemplate>
</asp:UpdatePanel>

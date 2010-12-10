<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalDnsRecordsControl.ascx.cs" Inherits="WebsitePanel.Portal.GlobalDnsRecordsControl" %>
<%@ Register Src="UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>

<asp:Panel ID="pnlRecords" runat="server">
    <div class="FormButtonsBar">
        <asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" Text="Add record" CssClass="Button2" OnClick="btnAdd_Click" CausesValidation="False" />
    </div>
</asp:Panel>
<asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="False"
    DataKeyNames="RecordID" EmptyDataText="gvRecords" CssSelectorClass="NormalGridView"
    OnRowEditing="gvRecords_RowEditing" OnRowDeleting="gvRecords_RowDeleting">
    <Columns>
        <asp:TemplateField HeaderText="gvRecordsName" ItemStyle-CssClass="NormalBold" ItemStyle-Wrap="false">
            <ItemTemplate>
                <asp:ImageButton ID="cmdEdit" runat="server" SkinID="EditSmall" CommandName="edit" AlternateText="Edit record">
                </asp:ImageButton>
                <%# Eval("RecordName") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="RecordType" HeaderText="gvRecordsType" />
        <asp:BoundField DataField="FullRecordData" HeaderText="gvRecordsData" ItemStyle-Width="100%" />
        <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton ID="cmdDelete" runat="server" SkinID="DeleteSmall" CommandName="delete" meta:resourcekey="cmdDelete"
                    AlternateText="Delete" OnClientClick="return confirm('Delete?');">
                </asp:ImageButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnSave">
    <table width="450">
        <tr>
            <td class="SubHead" width="150" nowrap><asp:Label ID="lblRecordType" runat="server" meta:resourcekey="lblRecordType" Text="Record Type:"></asp:Label></td>
            <td class="Normal" width="100%">
                <asp:DropDownList ID="ddlRecordType" runat="server" SelectedValue='<%# Bind("RecordType") %>' CssClass="NormalTextBox" AutoPostBack="True" OnSelectedIndexChanged="ddlRecordType_SelectedIndexChanged">
                    <asp:ListItem>A</asp:ListItem>
                    <asp:ListItem>MX</asp:ListItem>
                    <asp:ListItem>NS</asp:ListItem>
                    <asp:ListItem>TXT</asp:ListItem>
                    <asp:ListItem>CNAME</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="SubHead"><asp:Label ID="lblRecordName" runat="server" meta:resourcekey="lblRecordName" Text="Record Name:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtRecordName" runat="server" Width="100px" CssClass="NormalTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr id="rowData" runat="server">
            <td class="SubHead"><asp:Label ID="lblRecordData" runat="server" meta:resourcekey="lblRecordData" Text="Record Data:"></asp:Label></td>
            <td class="Normal" nowrap>
                <asp:TextBox ID="txtRecordData" runat="server" Width="100px" CssClass="NormalTextBox"></asp:TextBox><uc1:SelectIPAddress ID="ipAddress" runat="server" />
            </td>
        </tr>
        <tr id="rowMXPriority" runat="server">
            <td class="SubHead"><asp:Label ID="lblMXPriority" runat="server" meta:resourcekey="lblMXPriority" Text="MX Priority:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtMXPriority" runat="server" Width="30" CssClass="NormalTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" Text="Save" CssClass="Button1" OnClick="btnSave_Click" ValidationGroup="DnsRecord" />
                <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel" CssClass="Button1" OnClick="btnCancel_Click" CausesValidation="False" /></td>
        </tr>
    </table>
</asp:Panel>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Exchange_Settings.ascx.cs"
    Inherits="WebsitePanel.Portal.ProviderControls.Exchange2010_Settings" %>
<table cellpadding="3" cellspacing="0" width="100%">
    <tr runat="server" id="storageGroup">
			<td class="SubHead" width="200" nowrap>
			    <asp:Localize ID="locStorageGroup" runat="server" meta:resourcekey="locStorageGroup" Text="Storage Group Name:"></asp:Localize>
			</td>
			<td>
				<asp:TextBox ID="txtStorageGroup" runat="server" Width="200px"></asp:TextBox>	
            </td>
		</tr>
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Localize ID="locMailboxDatabase" runat="server" meta:resourcekey="locMailboxDatabase"
                Text="Mailbox Database Name:"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtMailboxDatabase" runat="server" Width="200px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <asp:Localize ID="locKeepDeletedItems" runat="server" meta:resourcekey="locKeepDeletedItems"
                Text="Keep Deleted Items (days):"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtKeepDeletedItems" runat="server" Width="50px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <asp:Localize ID="locKeepDeletedMailboxes" runat="server" meta:resourcekey="locKeepDeletedMailboxes"
                Text="Keep Deleted Mailboxes (days):"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtKeepDeletedMailboxes" runat="server" Width="50px"></asp:TextBox>
        </td>
    </tr>
</table>
<br />
<table cellpadding="4" cellspacing="0" width="100%">
    <tr runat="server" id="clusteredMailboxServer">
        <td class="SubHead">
            <asp:Localize ID="locMailboxClusterName" runat="server" meta:resourcekey="locMailboxClusterName"
                Text="Clustered Mailbox Server:"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtMailboxClusterName" runat="server" Width="200px"></asp:TextBox>
        </td>
    </tr>
    
    <tr>
        <td class="SubHead">
            <asp:Localize ID="Localize1" runat="server" meta:resourcekey="locPublicFolderServer"    ></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtPublicFolderServer" runat="server" Width="200px"></asp:TextBox>
        </td>
    </tr>
    
    <tr>
        <td class="SubHead">
            <asp:Localize ID="Localize2" runat="server" meta:resourcekey="locOABServer"    ></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtOABServer" runat="server" Width="200px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap valign="top">
            <asp:Localize ID="locHubTransport" runat="server" meta:resourcekey="locHubTransport"
                Text="Hub Transport Service:"></asp:Localize>
        </td>
        <td>
            <asp:DropDownList ID="ddlHubTransport" runat="server" CssClass="NormalTextBox">
            </asp:DropDownList>
            <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" meta:resourcekey="btnAdd"
                CssClass="Button1" /><br />
            <asp:GridView ID="gvHubTransport" runat="server" AutoGenerateColumns="False" EmptyDataText="gvRecords"
                CssSelectorClass="NormalGridView" OnRowCommand="gvHubTransport_RowCommand" meta:resourcekey="gvHubTransport">
                <Columns>
                    <asp:TemplateField meta:resourcekey="locServerNameColumn" ItemStyle-Width="100%" >
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblServiceName" Text='<%#Eval("ServiceName") + "(" + Eval("ServerName") +")"%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                                        
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="cmdDelete" runat="server" SkinID="DeleteSmall" CommandName="RemoveServer"
                                CommandArgument='<%#Eval("ServiceId") %>' meta:resourcekey="cmdDelete" AlternateText="Delete"
                                OnClientClick="return confirm('Delete?');"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="200" nowrap valign="top">
            <asp:Localize ID="locClientAccess" runat="server" meta:resourcekey="locClientAccess"
                Text="Client Access Service:"></asp:Localize>
        </td>
        <td>
            <asp:DropDownList ID="ddlClientAccess" runat="server" CssClass="NormalTextBox">
            </asp:DropDownList>
            <asp:Button runat="server" ID="Button1" OnClick="btnAddClientAccess_Click" meta:resourcekey="btnAdd"
                CssClass="Button1" /><br />
            <asp:GridView ID="gvClients" runat="server" AutoGenerateColumns="False" EmptyDataText="gvRecords"
                CssSelectorClass="NormalGridView" OnRowCommand="gvClientAccess_RowCommand" meta:resourcekey="gvClientAccess">
                <Columns>
                    <asp:TemplateField meta:resourcekey="locServerNameColumn" ItemStyle-Width="100%" >
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblServiceName" Text='<%#Eval("ServiceName") + "(" + Eval("ServerName") +")"%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="cmdDelete" runat="server" SkinID="DeleteSmall" CommandName="RemoveServer"
                                CommandArgument='<%#Eval("ServiceId") %>' meta:resourcekey="cmdDelete" AlternateText="Delete"
                                OnClientClick="return confirm('Delete?');"></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
<br />
<fieldset>
    <legend>
        <asp:Label ID="lblSetupVariables" runat="server" meta:resourcekey="lblSetupVariables"
            Text="Setup Instruction Variables" CssClass="NormalBold"></asp:Label>&nbsp;
    </legend>
    <table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
        <tr>
            <td class="SubHead" valign="top" style="width: 200px;">
                <asp:Localize ID="locSmtpServers" runat="server" meta:resourcekey="locSmtpServers"
                    Text="SMTP Servers:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtSmtpServers" runat="server" Width="300px" CssClass="NormalTextBox"
                    TextMode="MultiLine" Rows="3"></asp:TextBox>
                <br />
                <asp:Localize ID="locSmtpComments" runat="server" meta:resourcekey="locSmtpComments"
                    Text=" * one SMTP server record per line"></asp:Localize>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Localize ID="locAutodiscoverIP" runat="server" meta:resourcekey="locAutodiscoverIP"
                    Text="Autodiscover Server IP:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtAutodiscoverIP" runat="server" Width="100px" CssClass="NormalTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Localize ID="locAutodiscoverDomain" runat="server" meta:resourcekey="locAutodiscoverDomain"
                    Text="Autodiscover Server Domain:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtAutodiscoverDomain" runat="server" Width="300px" CssClass="NormalTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Localize ID="locOwaUrl" runat="server" meta:resourcekey="locOwaUrl" Text="OWA URL:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtOwaUrl" runat="server" Width="300px" CssClass="NormalTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Localize ID="locActiveSyncServer" runat="server" meta:resourcekey="locActiveSyncServer"
                    Text="ActiveSync Server:"></asp:Localize>
            </td>
            <td>
                <asp:TextBox ID="txtActiveSyncServer" runat="server" Width="300px" CssClass="NormalTextBox"></asp:TextBox>
            </td>
        </tr>
    </table>
</fieldset>

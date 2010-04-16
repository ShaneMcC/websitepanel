<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OCS_Settings.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.OCS_Settings" %>
<table>
    <tr>
        <td class="Normal" width="200" >
            <asp:Localize runat="server" ID="locServerName" meta:resourcekey="locServerName"/>
        </td>
        <td >
            <asp:TextBox runat="server" ID="txtServerName"  CssClass="NormalTextBox" Width="200px"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Localize runat="server" ID="locEdgeServices" meta:resourcekey="locEdgeServices"/>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlEdgeServers" />
            <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" meta:resourcekey="btnAdd" CssClass="Button1" /><br />
           
            <asp:GridView ID="gvEdgeServices" runat="server" AutoGenerateColumns="False"  
                EmptyDataText="gvRecords" CssSelectorClass="NormalGridView" 
                onrowcommand="gvEdgeServices_RowCommand"  meta:resourcekey="gvEdgeServices">
                <Columns>                                       
                    <asp:BoundField DataField="ServiceName" meta:resourcekey="locServerNameColumn" HeaderText="gvRecordsData" ItemStyle-Width="100%" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="cmdDelete" runat="server" SkinID="DeleteSmall" CommandName="RemoveServer" CommandArgument='<%#Eval("ServiceId") %>'
                                meta:resourcekey="cmdDelete" AlternateText="Delete" OnClientClick="return confirm('Delete?');">
                            </asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>

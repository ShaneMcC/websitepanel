<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DnsZoneRecords.ascx.cs" Inherits="WebsitePanel.Portal.DnsZoneRecords" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
	if (!confirm('Are you sure you want to delete this DNS Zone Record?')) return false; else ShowProgressDialog('Deleting DNS Zone Record...');
}
</script>
<asp:Panel ID="pnlRecords" runat="server">
	<div class="FormBody">
		<div class="Huge" style="padding: 10px;border: solid 1px #e5e5e5;background-color: #f5f5f5;">
			<asp:Literal ID="litDomainName" runat="server"></asp:Literal>
		</div>
	</div>
    <div class="FormButtonsBar">
        <asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" Text="Add record" CssClass="Button2" OnClick="btnAdd_Click" CausesValidation="False" />
    </div>
    <asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="False" EmptyDataText="gvRecords"
        CssSelectorClass="NormalGridView"
        OnRowEditing="gvRecords_RowEditing" OnRowDeleting="gvRecords_RowDeleting"
        AllowSorting="True" DataSourceID="odsDnsRecords">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="cmdEdit" runat="server" SkinID="EditSmall" CommandName="edit" AlternateText="Edit record">
                    </asp:ImageButton>
                    <asp:Literal ID="litMxPriority" runat="server" Text='<%# Eval("MxPriority") %>' Visible="false"></asp:Literal>
                    <asp:Literal ID="litRecordName" runat="server" Text='<%# Eval("RecordName") %>' Visible="false"></asp:Literal>
                    <asp:Literal ID="litRecordType" runat="server" Text='<%# Eval("RecordType") %>' Visible="false"></asp:Literal>
                    <asp:Literal ID="litRecordData" runat="server" Text='<%# Eval("RecordData") %>' Visible="false"></asp:Literal>
                </ItemTemplate>
                <ItemStyle CssClass="NormalBold" Wrap="False" />
            </asp:TemplateField>
            <asp:BoundField DataField="RecordName" SortExpression="RecordName" HeaderText="gvRecordsName" />
            <asp:BoundField DataField="RecordType" SortExpression="RecordType" HeaderText="gvRecordsType" />
            <asp:TemplateField SortExpression="RecordData" HeaderText="gvRecordsData" >
                <ItemStyle Width="100%" />
                <ItemTemplate>
                    <%# GetRecordFullData((string)Eval("RecordType"), (string)Eval("RecordData"), (int)Eval("MxPriority"))  %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="cmdDelete" runat="server" SkinID="DeleteSmall" CommandName="delete"
                        AlternateText="Delete record" OnClientClick="return confirmation();">
                    </asp:ImageButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="FormFooter">
        <asp:Button ID="btnBack" runat="server" meta:resourcekey="btnBack" CssClass="Button1" CausesValidation="false" 
            Text="Back" OnClick="btnBack_Click"/>
    </div>
</asp:Panel>
<asp:ObjectDataSource ID="odsDnsRecords" runat="server"
    SelectMethod="GetRawDnsZoneRecords" TypeName="WebsitePanel.Portal.ServersHelper"
		OnSelected="odsDnsRecords_Selected">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="0" Name="domainId" QueryStringField="DomainID" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnSave">
    <div class="FormBody">
        <table width="450">
            <tr>
                <td class="SubHead" style="width:150px;"><asp:Label ID="lblRecordType" runat="server" meta:resourcekey="lblRecordType" Text="Record Type:"></asp:Label></td>
                <td class="NormalBold">
                    <asp:DropDownList ID="ddlRecordType" runat="server" SelectedValue='<%# Bind("RecordType") %>' CssClass="NormalTextBox" AutoPostBack="True" OnSelectedIndexChanged="ddlRecordType_SelectedIndexChanged">
                        <asp:ListItem>A</asp:ListItem>
                        <asp:ListItem>MX</asp:ListItem>
                        <asp:ListItem>NS</asp:ListItem>
                        <asp:ListItem>TXT</asp:ListItem>
                        <asp:ListItem>CNAME</asp:ListItem>
                    </asp:DropDownList><asp:Literal ID="litRecordType" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="SubHead"><asp:Label ID="lblRecordName" runat="server" meta:resourcekey="lblRecordName" Text="Record Name:"></asp:Label></td>
                <td class="NormalBold">
                    <asp:TextBox ID="txtRecordName" runat="server" Width="100px" CssClass="NormalTextBox"></asp:TextBox>
                </td>
            </tr>
            <tr id="rowData" runat="server">
                <td class="SubHead"><asp:Label ID="lblRecordData" runat="server" meta:resourcekey="lblRecordData" Text="Record Data:"></asp:Label></td>
                <td class="NormalBold" nowrap>
                    <asp:TextBox ID="txtRecordData" runat="server" Width="200px" CssClass="NormalTextBox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valRequireData" runat="server" ControlToValidate="txtRecordData"
                        ErrorMessage="*" ValidationGroup="DnsZoneRecord" Display="Dynamic"></asp:RequiredFieldValidator>
                   <asp:regularexpressionvalidator id="IPValidator" runat="server" ValidationExpression="^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$"
							    Display="Dynamic" ErrorMessage="Please enter a valid IP" ValidationGroup="DnsZoneRecord" ControlToValidate="txtRecordData" CssClass="NormalBold"></asp:regularexpressionvalidator>
                 </td>
                        
            </tr>
            <tr id="rowMXPriority" runat="server">
                <td class="SubHead"><asp:Label ID="lblMXPriority" runat="server" meta:resourcekey="lblMXPriority" Text="MX Priority:"></asp:Label></td>
                <td class="NormalBold">
                    <asp:TextBox ID="txtMXPriority" runat="server" Width="30" CssClass="NormalTextBox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valRequireMxPriority" runat="server" ControlToValidate="txtMXPriority"
                        ErrorMessage="*" ValidationGroup="DnsZoneRecord" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="valRequireCorrectPriority" runat="server" ControlToValidate="txtMXPriority"
                        ErrorMessage="*" ValidationExpression="\d{1,3}"></asp:RegularExpressionValidator></td>
            </tr>
        </table>
    </div>
    <div class="FormFooter">
        <asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" Text="Save" CssClass="Button1" OnClick="btnSave_Click" OnClientClick = "ShowProgressDialog('Saving DNS Zone Record ...');" ValidationGroup="DnsZoneRecord" />
        <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel" CssClass="Button1" OnClick="btnCancel_Click" CausesValidation="False" /></td>
    </div>
</asp:Panel>
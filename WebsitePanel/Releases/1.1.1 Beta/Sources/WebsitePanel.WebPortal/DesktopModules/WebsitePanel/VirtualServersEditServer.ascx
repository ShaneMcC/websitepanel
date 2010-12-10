<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VirtualServersEditServer.ascx.cs" Inherits="WebsitePanel.Portal.VirtualServersEditServer" %>
<%@ Register Src="GlobalDnsRecordsControl.ascx" TagName="GlobalDnsRecordsControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<div class="FormBody">
<asp:ValidationSummary ID="summary" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="VirtualServer" />
    <table width="100%">
        <tr>
            <td class="SubHead" width="200" nowrap><asp:Label ID="lblServerName" runat="server" meta:resourcekey="lblServerName"></asp:Label></td>
            <td class="Normal" width="100%">
                <asp:TextBox ID="txtName" runat="server" CssClass="Huge" Width="300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="VirtualServerNameValidator" runat="server" ControlToValidate="txtName"
                     ValidationGroup="VirtualServer" meta:resourcekey="VirtualServerNameValidator"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="SubHead"><asp:Label ID="lblServerComments" runat="server" meta:resourcekey="lblServerComments"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtComments" runat="server" CssClass="NormalTextBox" Width="300px" Rows="2" TextMode="MultiLine"></asp:TextBox></td>
        </tr>
    </table>
    <br />
    <wsp:CollapsiblePanel id="secServices" runat="server"
        TargetControlID="ServicesPanel" resourcekey="secServices" Text="Services">
    </wsp:CollapsiblePanel>
    <asp:Panel ID="ServicesPanel" runat="server">
        <table width="100%">
            <tr id="rowPrimaryGroup" runat="server">
                <td>
                    <table width="100%">
                        <tr>
                            <td class="SubHead" width="200" nowrap>
                                <asp:Label ID="lblPDR" runat="server" meta:resourcekey="lblPDR" Text="Primary distribution group:"></asp:Label>
                            </td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlPrimaryGroup" runat="server" CssClass="NormalTextBox" AutoPostBack=true></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnAddServices" runat="server" meta:resourcekey="btnAddServices" CssClass="Button2" Text="Add services" OnClick="btnAddServices_Click" />
                    <asp:Button ID="btnRemoveSelected" runat="server" meta:resourcekey="btnRemoveSelected" CssClass="Button3" Text="Remove selected" OnClick="btnRemoveSelected_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:datalist id="dlServiceGroups" CellPadding="0" CellSpacing="5" Width="100%"
                        RepeatDirection="Vertical" Runat="server" DataKeyField="GroupID" OnItemDataBound="dlServiceGroups_ItemDataBound">
	                    <ItemTemplate>
		                    <table width="100%" class="BorderFillBox">
			                    <tr>
				                    <td valign="top" width="150" nowrap>
				                        <div class="MediumBold">
				                            <%# GetSharedLocalizedString("ResourceGroup." + (string)Eval("GroupName")) %>
				                        </div>
					                    <table id="tblGroupDistribution" runat="server">
					                        <tr>
					                            <td class="SubHead">Distribution</td>
					                        </tr>
					                        <tr id="rowBound" runat="server">
					                            <td class="Normal" nowrap>
					                                <asp:CheckBox ID="chkBind" runat="server" Text="Bind to primary"
					                                    AutoPostBack="true" Checked='<%# Eval("BindDistributionToPrimary") %>' />
					                            </td>
					                        </tr>
					                        <tr id="rowDistType" runat="server">
					                            <td>
					                                <asp:DropDownList ID="ddlDistType" runat="server" CssClass="NormalTextBox" SelectedValue='<%# Eval("DistributionType") %>'>
					                                    <asp:ListItem Value="1">Balanced</asp:ListItem>
					                                    <asp:ListItem Value="2">Randomized</asp:ListItem>
					                                </asp:DropDownList>
					                            </td>
					                        </tr>
					                    </table>
				                    </td>
				                    <td align="center" width="100%" valign="top">
					                    <asp:DataList ID="dlServices" Runat="server" RepeatDirection="Horizontal" CellSpacing="2"
					                        DataSource='<%# GetGroupServices((int)Eval("GroupID")) %>'
					                        DataKeyField="ServiceID">
						                    <ItemStyle CssClass="Brick" VerticalAlign="Top" HorizontalAlign="Left"></ItemStyle>
						                    <ItemTemplate>
						                        <table width="100%">
						                            <tr>
						                                <td rowspan="2" valign="top">
						                                    <asp:CheckBox ID="chkSelected" runat="server" />
						                                </td>
						                                <td class="NormalBold">
						                                     <%# Eval("ServiceName") %>
						                                </td>
						                            </tr>
						                            <tr>
						                                <td class="Normal"><%# Eval("ServerName") %></td>
						                            </tr>
						                        </table>     
						                    </ItemTemplate>
					                    </asp:DataList>
				                    </td>
			                    </tr>
		                    </table>
	                    </ItemTemplate>
                    </asp:datalist>
                </td>
            </tr>
            <tr>
                <td class="Normal">&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    
    <wsp:CollapsiblePanel id="secDnsRecords" runat="server" IsCollapsed="true"
        TargetControlID="DnsRecordsPanel" resourcekey="secDnsRecords" Text="DNS Records Template">
    </wsp:CollapsiblePanel>
    <asp:Panel ID="DnsRecordsPanel" runat="server">
        <table width="450px">
            <tr>
                <td>
                    <uc1:GlobalDnsRecordsControl ID="GlobalDnsRecordsControl" runat="server" ServerIdParam="ServerID" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    
    <wsp:CollapsiblePanel id="secInstantAlias" runat="server" IsCollapsed="true"
        TargetControlID="InstantAliasPanel" resourcekey="secInstantAlias" Text="Instant Alias">
    </wsp:CollapsiblePanel>
    <asp:Panel ID="InstantAliasPanel" runat="server">
        <table width="100%">
            <tr>
                <td class="Normal" width="100%">
                    customerdomain.com.&nbsp;<asp:TextBox ID="txtInstantAlias" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                                               <asp:RegularExpressionValidator id="DomainFormatValidator" ValidationGroup="VirtualServer" runat="server" meta:resourcekey="DomainFormatValidator"
		    ControlToValidate="txtInstantAlias" Display="Dynamic" SetFocusOnError="true"
		    ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,6}$"></asp:RegularExpressionValidator>
                    </td>
            </tr>
        </table>
    </asp:Panel>
</div>

<div class="FormFooter">
    <asp:Button ID="btnUpdate" runat="server" meta:resourcekey="btnUpdate" CausesValidation="true" ValidationGroup="VirtualServer" CssClass="Button1" Text="Update" OnClick="btnUpdate_Click" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CausesValidation="false" CssClass="Button1" Text="Cancel" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CausesValidation="false" CssClass="Button1" Text="Delete" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete server?');" />
</div>
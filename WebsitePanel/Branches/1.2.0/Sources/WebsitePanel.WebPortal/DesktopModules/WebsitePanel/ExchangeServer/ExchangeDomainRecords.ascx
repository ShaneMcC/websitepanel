<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeDomainRecords.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeDomainRecords" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
		</div>
		<div class="Left">
			<wsp:Menu id="menu" runat="server" SelectedItem="domains" />
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="ExchangeDomainName48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Domain Names"></asp:Localize>
					-
					<asp:Literal ID="litDomainName" runat="server"></asp:Literal>
				</div>
				<div class="FormBody">
				
<asp:UpdatePanel ID="RecordsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
    
				<wsp:SimpleMessageBox id="messageBox" runat="server" />
		    

				<div class="FormButtonsBarClean">
					<div class="FormButtonsBarCleanLeft" style="padding-bottom: 4px;">
						<asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" Text="Add record" CssClass="Button2" CausesValidation="False" />
					</div>
					<div class="FormButtonsBarCleanRight">
						<asp:UpdateProgress ID="recordsProgress" runat="server"
							AssociatedUpdatePanelID="RecordsUpdatePanel" DynamicLayout="false">
							<ProgressTemplate>
								<asp:Image ID="imgSep" runat="server" SkinID="AjaxIndicator" />
							</ProgressTemplate>
						</asp:UpdateProgress>
					</div>
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
								<asp:ImageButton ID="cmdDelete" runat="server" SkinID="ExchangeDelete" CommandName="delete"
									AlternateText="Delete record" OnClientClick="return confirm('Delete record?');">
								</asp:ImageButton>
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
				</asp:GridView>

				<br />
				<div style="text-align: center">
					<asp:Button ID="btnBack" runat="server" meta:resourcekey="btnBack" CssClass="Button1" CausesValidation="false" 
						Text="Back" OnClick="btnBack_Click"/>
				</div>



				<asp:ObjectDataSource ID="odsDnsRecords" runat="server"
					SelectMethod="GetRawDnsZoneRecords" TypeName="WebsitePanel.Portal.ServersHelper"
						OnSelected="odsDnsRecords_Selected">
					<SelectParameters>
						<asp:QueryStringParameter DefaultValue="0" Name="domainId" QueryStringField="DomainID" />
					</SelectParameters>
				</asp:ObjectDataSource>
					
					
		<asp:Panel ID="EditRecordPanel" runat="server" CssClass="Popup" style="display:none">
			<table class="Popup-Header" cellpadding="0" cellspacing="0">
				<tr>
					<td class="Popup-HeaderLeft"></td>
					<td class="Popup-HeaderTitle">
						<asp:Localize ID="headerEditRecord" runat="server" meta:resourcekey="headerEditRecord"></asp:Localize>
					</td>
					<td class="Popup-HeaderRight"></td>
				</tr>
			</table>
			<div class="Popup-Content">
				<div class="Popup-Body">
					<br />
					
			<asp:UpdatePanel ID="EditRecordUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
				<ContentTemplate>
					<table width="450">
						<tr>
							<td class="SubHead" width="150" nowrap><asp:Label ID="lblRecordType" runat="server" meta:resourcekey="lblRecordType" Text="Record Type:"></asp:Label></td>
							<td class="NormalBold" width="100%">
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
							</td>
						</tr>
						<tr>
						    <asp:regularexpressionvalidator id="IPValidator1" runat="server" ValidationExpression="^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$"
							    Display="Dynamic" ErrorMessage="Please enter a valid IP" ValidationGroup="DnsZoneRecord" ControlToValidate="txtRecordData" CssClass="NormalBold"></asp:regularexpressionvalidator>
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
					
					</ContentTemplate>
				</asp:UpdatePanel>
					
					<br />
				</div>
				<div class="FormFooter">
					<asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" Text="Save" CssClass="Button1" OnClick="btnSave_Click" ValidationGroup="DnsZoneRecord" />
					<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel" CssClass="Button1" OnClick="btnCancel_Click" CausesValidation="False" /></td>
				</div>
			</div>
		</asp:Panel>
				    
	<ajaxToolkit:ModalPopupExtender ID="EditRecordModal" runat="server"
		TargetControlID="btnAdd" PopupControlID="EditRecordPanel"
		BackgroundCssClass="modalBackground" DropShadow="false" />

		</ContentTemplate>
	</asp:UpdatePanel>
				    
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>
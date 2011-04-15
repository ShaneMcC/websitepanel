<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationCreateOrganization.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.OrganizationCreateOrganization" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
        </div>
        <div class="Left">
            &nbsp;
        </div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="OrganizationAdd48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Welcome"></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
					<table>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locOrgName" runat="server" meta:resourcekey="locOrgName" Text="Organization Name: *"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtOrganizationName" runat="server" CssClass="HugeTextBox200"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequireOrgName" runat="server" meta:resourcekey="valRequireOrgName" ControlToValidate="txtOrganizationName"
									ErrorMessage="Enter Organization Name" ValidationGroup="CreateOrganization" Display="Dynamic" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locOrganizationID" runat="server" meta:resourcekey="locOrganizationID" Text="Organization ID: *"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtOrganizationID" runat="server" CssClass="TextBox100" MaxLength="9"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequiretxtOrganizationID" runat="server" meta:resourcekey="valRequiretxtOrganizationID" ControlToValidate="txtOrganizationID"
									ErrorMessage="Enter Organization ID" ValidationGroup="CreateOrganization" Display="Dynamic" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
								<asp:RegularExpressionValidator ID="valRequireCorrectOrgID" runat="server"
									ErrorMessage="Please enter valid organization ID" ControlToValidate="txtOrganizationID"
										Display="Dynamic" ValidationExpression="[a-zA-Z][a-zA-Z0-9]{1,8}" meta:resourcekey="valRequireCorrectOrgID"
										ValidationGroup="CreateOrganization">*</asp:RegularExpressionValidator>
							</td>
						</tr>
					</table>
				    <div class="FormFooterClean">
					    <asp:Button id="btnCreate" runat="server" Text="Create Organization" CssClass="Button1" OnClick="btnCreate_Click"
							meta:resourcekey="btnCreate" ValidationGroup="CreateOrganization" OnClientClick="ShowProgressDialog('Creating Organization...');"></asp:Button>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateOrganization" />
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>
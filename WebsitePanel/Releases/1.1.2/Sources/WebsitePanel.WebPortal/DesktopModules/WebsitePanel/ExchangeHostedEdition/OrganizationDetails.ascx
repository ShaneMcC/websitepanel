<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationDetails.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeHostedEdition.OrganizationDetails" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="FormBody">
    <wsp:SimpleMessageBox ID="messageBox" runat="server" />
    <fieldset>
        <legend><asp:Localize ID="locOrganizationDetails" runat="server" meta:resourcekey="locOrganizationDetails">Organization Details</asp:Localize></legend>
        <table>
            <tr>
                <td class="Label" style="width:150px;"><asp:Localize ID="locOrganizationName" runat="server" meta:resourcekey="locOrganizationName">Organization name:</asp:Localize></td>
                <td><asp:Literal ID="organizationName" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td class="Label"><asp:Localize ID="locAdministratorName" runat="server" meta:resourcekey="locAdministratorName">Administrator name:</asp:Localize></td>
                <td><asp:Literal ID="administratorName" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td class="Label"><asp:Localize ID="locAdministratorEmail" runat="server" meta:resourcekey="locAdministratorEmail">Administrator e-mail:</asp:Localize></td>
                <td><asp:Literal ID="administratorEmail" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td class="Label"><asp:Localize ID="locEcpURL" runat="server" meta:resourcekey="locEcpURL">Exchange Control Panel:</asp:Localize></td>
                <td><asp:HyperLink ID="ecpURL" runat="server" Target="_blank"></asp:HyperLink></td>
            </tr>
        </table>
        <div style="text-align:right;">
            <asp:Button ID="deleteOrganization" runat="server" 
                meta:resourcekey="deleteOrganization" Text="Delete Organization" 
                CssClass="Button1" onclick="deleteOrganization_Click" />
        </div>
    </fieldset>

    <fieldset id="servicePlanBlock" runat="server">
        <legend><asp:Localize ID="locServicePlan" runat="server" meta:resourcekey="locServicePlan">Service Plan</asp:Localize></legend>
        
        <table>
            <tr>
                <td class="Label"><asp:Localize ID="locService" runat="server" meta:resourcekey="locService">Service:</asp:Localize></td>
                <td><asp:Literal ID="serviceName" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td class="Label"><asp:Localize ID="locProgramID" runat="server" meta:resourcekey="locProgramID">Program ID:</asp:Localize></td>
                <td><asp:Literal ID="programID" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td class="Label"><asp:Localize ID="locOfferID" runat="server" meta:resourcekey="locOfferID">Offer ID:</asp:Localize></td>
                <td><asp:Literal ID="offerID" runat="server"></asp:Literal></td>
            </tr>
        </table>

        <div style="text-align:right;">
            <asp:Button ID="changeServicePlan" runat="server" 
                meta:resourcekey="changeServicePlan" Text="Change Service Plan" 
                CssClass="Button1" onclick="changeServicePlan_Click" />
        </div>
        <div>
            <asp:Label ID="lblAdminsWarning" runat="server" ForeColor="Red" meta:resourcekey="lblAdminsWarning">Visible to admins only</asp:Label>
        </div>
    </fieldset>

    <fieldset>
        <legend><asp:Localize ID="locQuotas" runat="server" meta:resourcekey="locQuotas">Quotas</asp:Localize></legend>
        <table>
            <tr>
                <td class="Label"><asp:Localize ID="locMailboxes" runat="server" meta:resourcekey="locMailboxes">Mailboxes:</asp:Localize></td>
                <td><asp:Literal ID="mailboxes" runat="server">0 of 0 (max 0)</asp:Literal></td>
            </tr>
            <tr>
                <td class="Label"><asp:Localize ID="locContacts" runat="server" meta:resourcekey="locContacts">Contacts:</asp:Localize></td>
                <td><asp:Literal ID="contacts" runat="server">0 of 0 (max 0)</asp:Literal></td>
            </tr>
            <tr>
                <td class="Label"><asp:Localize ID="locDistributionLists" runat="server" meta:resourcekey="locDistributionLists">Distribution Lists:</asp:Localize></td>
                <td><asp:Literal ID="distributionLists" runat="server">0 of 0 (max 0)</asp:Literal></td>
            </tr>
        </table>
        <div style="text-align:right;">
            <asp:Button ID="updateQuotas" runat="server" meta:resourcekey="updateQuotas" 
                Text="Update Quotas" CssClass="Button1" onclick="updateQuotas_Click" />
        </div>
    </fieldset>

    <fieldset>
        <legend><asp:Localize ID="locDomains" runat="server" meta:resourcekey="locDomains">Domains</asp:Localize></legend>
        <div class="FormButtonsBarClean">
            <asp:Button ID="btnAddDomain" runat="server" meta:resourcekey="btnAddDomain"
                Text="Add Domain" CssClass="Button1" onclick="btnAddDomain_Click" />
        </div>

		<asp:GridView ID="gvDomains" runat="server" AutoGenerateColumns="False" EnableViewState="true"
			EmptyDataText="gvDomains" CssSelectorClass="NormalGridView" DataKeyNames="Name" 
            onrowdeleting="gvDomains_RowDeleting">
			<Columns>
				<asp:TemplateField HeaderText="gvDomainsName">
					<ItemStyle Width="70%"></ItemStyle>
					<ItemTemplate>
					    <%# Eval("Name") %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="gvDomainsIsDefault">
					<ItemTemplate>
						<div style="text-align:center">
							&nbsp;
							<asp:Image runat="server" SkinID="Checkbox16" Visible='<%# Eval("IsDefault") %>' />
						</div>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="gvDomainsIsTemp">
					<ItemTemplate>
						<div style="text-align:center">
							&nbsp;
							<asp:Image runat="server" SkinID="Checkbox16" Visible='<%# Eval("IsTemp") %>' />
						</div>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemTemplate>
						&nbsp;<asp:ImageButton ID="imgDelDomain" runat="server" Text="Delete" SkinID="ExchangeDelete"
							CommandName="delete" CommandArgument='<%# Eval("Name") %>' Visible='<%# !((bool)Eval("IsTemp") || (bool)Eval("IsDefault")) %>'
							meta:resourcekey="cmdDelete" OnClientClick="return confirm('Are you sure you want to remove selected domain?')"></asp:ImageButton>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
		<br />
		<asp:Localize ID="locDomainsQuota" runat="server" meta:resourcekey="locDomainsQuota" Text="Total domains used:"></asp:Localize>
		&nbsp;&nbsp;&nbsp;
		<wsp:QuotaViewer ID="domainsQuota" runat="server" QuotaTypeId="2" />
    </fieldset>

    <%-- 
    <fieldset>
        <legend><asp:Localize ID="locCatchAll" runat="server" meta:resourcekey="locCatchAll">Catch-all</asp:Localize></legend>
        <table>
            <tr>
                <td class="Label"><asp:Localize ID="locCatchAllAddress" runat="server" meta:resourcekey="locCatchAllAddress">Catch-all address:</asp:Localize></td>
                <td><asp:Literal ID="catchAllAddress" runat="server">(not set)</asp:Literal></td>
            </tr>
        </table>
        <div style="text-align:right;">
            <asp:Button ID="setCatchAll" runat="server" meta:resourcekey="setCatchAll" 
                Text="Set Catch-all" CssClass="Button1" onclick="setCatchAll_Click" />
        </div>
    </fieldset>
    --%>

    <fieldset id="organizationSummary" runat="server">
        <legend><asp:Localize ID="locSetupInstructions" runat="server" meta:resourcekey="locSetupInstructions">Setup Instructions</asp:Localize></legend>
        <div style="padding:10px;">
            <asp:Literal ID="setupInstructions" runat="server">(not set)</asp:Literal>
        </div>
        <div>
            <asp:Localize ID="locSendTo" runat="server" meta:resourcekey="locSendTo" Text="Send to e-mail:"></asp:Localize>
            <asp:TextBox ID="sendTo" runat="server" Width="150"></asp:TextBox>
            <asp:Button ID="sendSetupInstructions" runat="server" ValidationGroup="SendInstructions"
                meta:resourcekey="sendSetupInstructions" Text="Send" 
                CssClass="Button1" onclick="sendSetupInstructions_Click" />
            <asp:RequiredFieldValidator ID="requireSendTo" runat="server" meta:resourcekey="requireSendTo" ControlToValidate="sendTo" ValidationGroup="SendInstructions"
                    Text="*" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
        </div>
    </fieldset>
</div>
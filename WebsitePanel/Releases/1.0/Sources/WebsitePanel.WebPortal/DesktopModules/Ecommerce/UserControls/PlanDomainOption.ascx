<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanDomainOption.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.PlanDomainOption" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>

<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:CheckBox runat="server" ID="chkSelected" 
		AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chkSelected_OnCheckedChanged" /><asp:Localize runat="server" meta:resourcekey="lclPlanDomainOption" /></div>
</div>

<table cellpadding="0" cellspacing="0" class="QuickOutlineTbl">
<asp:PlaceHolder runat="server" ID="pnlTopLevelDomain">
	<tr>
		<td><wsp:RadioGroupValidator runat="server" ID="reqDomainOption" CssClass="QuickLabel"
			RadioButtonsGroup="DomainOption" Display="Dynamic" meta:resourcekey="reqDomainOption" /></td>
	</tr>
	<tr>
		<td class="QuickLabel">
			<div><wsp:GroupRadioButton runat="server" ID="rbRegisterNew" GroupName="DomainOption" 
				meta:resourcekey="rbRegisterNew" AutoPostBack="true" OnCheckedChanged="rbRegisterNew_OnCheckedChanged" /></div>
			<asp:PlaceHolder runat="server" ID="pnlDomainReg" Visible="false">
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td colspan="4"><wsp:ManualContextValidator runat="server" ID="ctxDomainValidator" EnableClientScript="false" 
							OnEvaluatingContext="ctxDomainValidator_EvaluatingContext" Display="Dynamic" EnableViewState="false" />
							<asp:RequiredFieldValidator runat="server" ID="reqDomainVal" meta:resourcekey="reqDomainVal" ControlToValidate="txtDomainReg" 
								EnableClientScript="false" Display="Dynamic" EnableViewState="false" />
							<asp:Label runat="server" ID="lblDomainAvailable" Visible="false" EnableViewState="false" 
								CssClass="NormalGreen" meta:resourcekey="lblDomainAvailable" /></td>
					</tr>
					<tr>
						<td><asp:Localize runat="server" meta:resourcekey="lclDomain" /></td>
						<td><asp:TextBox runat="server" ID="txtDomainReg" CssClass="NormalTextBox" />&nbsp;.&nbsp;<asp:DropDownList runat="server" 
							ID="ddlTopLevelDoms" CssClass="NormalTextBox" DataValueField="ProductId" DataTextField="ProductName" AutoPostBack="true" 
							OnSelectedIndexChanged="ddlTopLevelDoms_OnSelectedIndexChanged" /></td>
						<td><asp:DropDownList runat="server" ID="ddlDomCycles" /></td>
						<td><asp:Button runat="server" ID="btnCheckDomain" CssClass="Button1" OnClick="btnCheckDomain_Click" 
							meta:resourcekey="btnCheckDomain" CausesValidation="false" /></td>
					</tr>
				</table>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="usTldFields" Visible="false">
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td><asp:Localize runat="server" meta:resourcekey="lclNexusCategory" /></td>
						<td>
							<asp:DropDownList runat="server" ID="ddlNexusCategory" CssClass="NormalTextBox">
								<asp:ListItem Value="C11" meta:resourcekey="lclNexusCategory11" />
								<asp:ListItem Value="C12" meta:resourcekey="lclNexusCategory12" />
								<asp:ListItem Value="C21" meta:resourcekey="lclNexusCategory21" />
								<asp:ListItem Value="C31" meta:resourcekey="lclNexusCategory31" />
								<asp:ListItem Value="C32" meta:resourcekey="lclNexusCategory32" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td><asp:Localize runat="server" meta:resourcekey="lclAppPurpose" /></td>
						<td>
							<asp:DropDownList runat="server" ID="ddlAppPurpose" CssClass="NormalTextBox">
								<asp:ListItem Value="P1" meta:resourcekey="lclAppPurpose1" />
								<asp:ListItem Value="P2" meta:resourcekey="lclAppPurpose2" />
								<asp:ListItem Value="P3" meta:resourcekey="lclAppPurpose3" />
								<asp:ListItem Value="P4" meta:resourcekey="lclAppPurpose4" />
								<asp:ListItem Value="P5" meta:resourcekey="lclAppPurpose5" />
							</asp:DropDownList>
						</td>
					</tr>
				</table>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="ukTldFields" Visible="false">
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td align="right"><asp:Localize runat="server" meta:resourcekey="lclRegisteredFor" /></td>
						<td>
							<asp:TextBox runat="server" ID="txtRegisteredFor" CssClass="NormalTextBox" Width="150px" />
							<asp:RequiredFieldValidator runat="server" ControlToValidate="txtRegisteredFor" Display="Dynamic" ErrorMessage="*" /></td>
					</tr>
					<tr>
						<td align="right"><asp:Localize runat="server" meta:resourcekey="lclUkLegalType" /></td>
						<td>
							<asp:DropDownList runat="server" ID="ddlUkLegalType" CssClass="NormalTextBox">
								<asp:ListItem Value="IND" meta:resourcekey="lclLegalTypeIND" />
								<asp:ListItem Value="Non-UK" meta:resourcekey="lclLegalTypeNonUK" />
								<asp:ListItem Value="LTD" meta:resourcekey="lclLegalTypeLTD" />
								<asp:ListItem Value="PLC" meta:resourcekey="lclLegalTypePLC" />
								<asp:ListItem Value="PTNR" meta:resourcekey="lclLegalTypePTNR" />
								<asp:ListItem Value="LLP" meta:resourcekey="lclLegalTypeLLP" />
								<asp:ListItem Value="STRA" meta:resourcekey="lclLegalTypeSTRA" />
								<asp:ListItem Value="RCHAR" meta:resourcekey="lclLegalTypeRCHAR" />
								<asp:ListItem Value="ip" meta:resourcekey="lclLegalTypeIP" />
								<asp:ListItem Value="SCH" meta:resourcekey="lclLegalTypeSCH" />
								<asp:ListItem Value="GOV" meta:resourcekey="lclLegalTypeGOV" />
								<asp:ListItem Value="CRC" meta:resourcekey="lclLegalTypeCRC" />
								<asp:ListItem Value="STAT" meta:resourcekey="lclLegalTypeSTAT" />
								<asp:ListItem Value="OTHER" meta:resourcekey="lclLegalTypeOTHER" />
								<asp:ListItem Value="FCORP" meta:resourcekey="lclLegalTypeFCORP" />
								<asp:ListItem Value="FOTHER" meta:resourcekey="lclLegalTypeFOTHER" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right"><asp:Localize runat="server" meta:resourcekey="lclCompanyIdNumber" /></td>
						<td>
							<asp:TextBox runat="server" ID="txtCompanyIdNum" CssClass="NormalTextBox" Width="150px" />
							<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCompanyIdNum" Display="Dynamic" ErrorMessage="*" /></td>
					</tr>
					<tr>
						<td align="right"><asp:Localize runat="server" meta:resourcekey="lclHideWhoisInfo" /></td>
						<td><asp:CheckBox runat="server" ID="chkHideWhoisInfo" /></td>
					</tr>
				</table>
			</asp:PlaceHolder>
			<asp:PlaceHolder runat="server" ID="euTldFields" Visible="false">
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td><asp:Localize runat="server" meta:resourcekey="lclDTPolicyAgreement" /></td>
						<td><asp:CheckBox runat="server" ID="chkDtPolicyAgree" /></td>
					</tr>
					<tr>
						<td><asp:Localize runat="server" meta:resourcekey="lclDelPolicyAgreement" /></td>
						<td><asp:CheckBox runat="server" ID="chkDelPolicyAgree" /></td>
					</tr>
					<tr>
						<td><asp:Localize runat="server" meta:resourcekey="lclEuAdrLang" /></td>
						<td>
							<asp:DropDownList runat="server" ID="ddlEuAdrLang" CssClass="NormalTextBox">
								<asp:ListItem Value="cs" Text="Czech" />
								<asp:ListItem Value="da" Text="Danish" />
								<asp:ListItem Value="de" Text="German" />
								<asp:ListItem Value="el" Text="Greek" />
								<asp:ListItem Value="en" Text="English" />
								<asp:ListItem Value="es" Text="Spanish" />
								<asp:ListItem Value="et" Text="Estonian" />
								<asp:ListItem Value="fi" Text="Finnish" />
								<asp:ListItem Value="fr" Text="French" />
								<asp:ListItem Value="hu" Text="Hungarian" />
								<asp:ListItem Value="it" Text="Italian" />
								<asp:ListItem Value="LT" Text="Lithuanian" />
								<asp:ListItem Value="lv" Text="Latvian" />
								<asp:ListItem Value="mt" Text="Maltese" />
								<asp:ListItem Value="nl" Text="Dutch" />
								<asp:ListItem Value="pl" Text="Polish" />
								<asp:ListItem Value="pt" Text="Portugese" />
								<asp:ListItem Value="sk" Text="Slovak" />
								<asp:ListItem Value="sl" Text="Slovenian" />
								<asp:ListItem Value="sv" Text="Swedish" />
							</asp:DropDownList>
						</td>
					</tr>
				</table>
			</asp:PlaceHolder>
		</td>
	</tr>
	<asp:PlaceHolder runat="server" ID="pnlDomainTrans" Visible="false">
	<tr>
		<td class="QuickLabel">
			<div><wsp:GroupRadioButton runat="server" ID="rbTransferDomain" GroupName="DomainOption" 
				meta:resourcekey="rbTransferDomain" AutoPostBack="true" OnCheckedChanged="rbTransferDomain_OnCheckedChanged" /></div>
				<table>
					<tr>
						<td><asp:Localize runat="server" meta:resourcekey="lclDomain" /></td>
						<td><asp:TextBox runat="server" ID="txtDomainTrans" /></td>
						<td>.</td>
						<td><asp:DropDownList runat="server" ID="ddlTopLevelDomsTrans" DataValueField="ProductId" 
							DataTextField="ProductName" AutoPostBack="true" OnSelectedIndexChanged="ddlTransDomCycles_OnSelectedIndexChanged" /></td>
						<td><asp:DropDownList runat="server" ID="ddlTransDomCycles" /></td>
					</tr>
				</table>
		</td>
	</tr>
	</asp:PlaceHolder>
	<tr>
		<td class="QuickLabel">
			<div><wsp:GroupRadioButton runat="server" ID="rbUpdateNs" GroupName="DomainOption" 
				meta:resourcekey="rbUpdateNs" AutoPostBack="true" OnCheckedChanged="rbUpdateNs_OnCheckedChanged" /></div>
			<asp:PlaceHolder runat="server" ID="pnlUpdateNs" Visible="false">
				<table>
					<tr>
						<td><asp:Localize runat="server" meta:resourcekey="lclDomain" /></td>
						<td><asp:TextBox runat="server" ID="txtDomainUpdate" /></td>
					</tr>
				</table>
			</asp:PlaceHolder>
		</td>
	</tr>
</asp:PlaceHolder>
	<tr>
		<td class="QuickLabel"><asp:Localize runat="server" ID="lclNoTLDsInStock" meta:resourcekey="lclNoTLDsInStock" Visible="false" /></td>
	</tr>
</table>

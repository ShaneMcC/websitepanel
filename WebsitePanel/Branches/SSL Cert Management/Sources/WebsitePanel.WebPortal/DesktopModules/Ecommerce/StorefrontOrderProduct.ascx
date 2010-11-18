<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StorefrontOrderProduct.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.StorefrontOrderProduct" %>
<%@ Register TagPrefix="wsp" TagName="PlanHighlights" Src="ProductControls/HostingPlan_Highlights.ascx" %>
<%@ Register TagPrefix="wsp" TagName="PlanCycles" Src="UserControls/QuickHostingPlanCycles.ascx" %>
<%@ Register TagPrefix="wsp" TagName="DomainOption" Src="UserControls/PlanDomainOption.ascx" %>
<%@ Register TagPrefix="wsp" TagName="HostingAddons" Src="UserControls/PlanHostingAddons.ascx" %>
<%@ Register TagPrefix="wsp" TagName="SelectPaymentMethod" Src="UserControls/ChoosePaymentMethod.ascx" %>
<%@ Register TagPrefix="wsp" TagName="LoginUser" Src="UserControls/LoginUserAccount.ascx" %>
<%@ Register TagPrefix="wsp" TagName="CreateUser" Src="UserControls/CreateUserAccount.ascx" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>

<%@ Import Namespace="WebsitePanel.Ecommerce.Portal" %>

<asp:Wizard runat="server" ID="wzrdOrderHosting" OnFinishButtonClick="wzrdOrderHosting_FinishButtonClick" 
	OnNextButtonClick="wzrdOrderHosting_NextButtonClick" OnActiveStepChanged="wzrdOrderHosting_ActiveStepChanged" 
	DisplaySideBar="false" StartNextButtonStyle-CssClass="Button1" 
	FinishCompleteButtonStyle-CssClass="Button1" NavigationStyle-HorizontalAlign="Left" 
	NavigationButtonStyle-CssClass="Button1" meta:resourcekey="wzrdOrderHosting" Width="100%">
	<WizardSteps>
		<asp:WizardStep runat="server" ID="Step1" AllowReturn="true" StepType="Auto">
			<div class="ProductInfo">
				<asp:Literal runat="server" ID="ltrProductName" />
			</div>
			<p class="Normal" style="text-align: justify;"><asp:Literal runat="server" ID="ltrProductDesc" /></p>

			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclProductHighlights" /></div>
			</div>
			<div>
				<wsp:PlanHighlights runat="server" ID="ctlPlanHighlights" ShowMoreDetails="true" />
			</div>

			<wsp:PlanCycles runat="server" ID="ctlPlanCycles" />

			<wsp:DomainOption runat="server" ID="ctlPlanDomain" />

			<asp:PlaceHolder runat="server" ID="pnlPlanAddons">
				<div class="StrongHeaderLabel"><asp:Localize runat="server" meta:resourcekey="lclChooseAddonMsg" /></div>
				<br />
				<wsp:HostingAddons runat="server" ID="ctlPlanAddons" />
			</asp:PlaceHolder>

			<wsp:SelectPaymentMethod runat="server" ID="ctlPaymentMethod" />
			
			<asp:PlaceHolder runat="server" ID="pnlUAccountOptions">
				<div class="FormButtonsBar">
					<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclUAccountOption" /></div>
				</div>
				<wsp:RadioGroupValidator runat="server" ID="reqUserOptionValidator" Display="Dynamic" RadioButtonsGroup="UAccountGroup" 
						CssClass="FormBody QuickLabel" meta:resourcekey="reqUserOptionValidator" />
				<div class="FormBody QuickLabel">
					<wsp:GroupRadioButton runat="server" GroupName="UAccountGroup" ID="rbExistingUser" meta:resourcekey="rbExistingUser" />
					<br />
					<wsp:GroupRadioButton runat="server" GroupName="UAccountGroup" ID="rbNewUser" meta:resourcekey="rbNewUser" />
				</div>
			</asp:PlaceHolder>
		</asp:WizardStep>
		<asp:WizardStep runat="server" ID="Step2" StepType="Step">
			<div class="ProductInfo">
				<asp:Localize runat="server" meta:resourcekey="lclExistingUserLogin" />
			</div>
			<wsp:LoginUser runat="server" ID="ctlCustomerLogin" />
		</asp:WizardStep>
		<asp:WizardStep runat="server" ID="Step3" StepType="Step">
			<div class="ProductInfo">
				<asp:Localize runat="server" meta:resourcekey="lclRegisterUser" />
			</div>
			<wsp:CreateUser runat="server" ID="ctlCustomerCreate" />
		</asp:WizardStep>
		<asp:WizardStep runat="server" ID="Step4" StepType="Finish">
		    <div class="ProductInfo">
				<asp:Localize runat="server" meta:resourcekey="lclTermsAndConds" />
			</div>
			<div class="FormBody Scrollable500" runat="server" id="pnlTermsAndConds"></div>
		</asp:WizardStep>
	</WizardSteps>
	<FinishNavigationTemplate>
	    <center class="FormFooter">
	        <asp:Button id="Button1" runat="server" CssClass="Button1" meta:resourcekey="btnAccept" CommandName="MoveComplete" />&nbsp;&nbsp;&nbsp;
	        <asp:Button runat="server" CssClass="Button1" meta:resourcekey="btnDecline" CommandName="MoveCancel" />
	    </center>
	</FinishNavigationTemplate>
</asp:Wizard>
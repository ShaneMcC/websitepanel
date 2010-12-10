<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickSignup.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.QuickSignup" %>
<%@ Register TagPrefix="wsp" TagName="QuickPlans" Src="UserControls/QuickHostingPlans.ascx" %>
<%@ Register TagPrefix="wsp" TagName="PlanCycles" Src="UserControls/QuickHostingPlanCycles.ascx" %>
<%@ Register TagPrefix="wsp" TagName="DomainOption" Src="UserControls/PlanDomainOption.ascx" %>
<%@ Register TagPrefix="wsp" TagName="SelectPaymentMethod" Src="UserControls/ChoosePaymentMethod.ascx" %>
<%@ Register TagPrefix="wsp" TagName="CreateUser" Src="UserControls/CreateUserAccount.ascx" %>

<asp:Wizard runat="server" ID="wzrdOrderHosting" OnFinishButtonClick="wzrdOrderHosting_FinishButtonClick" 
	OnNextButtonClick="wzrdOrderHosting_NextButtonClick" OnActiveStepChanged="wzrdOrderHosting_ActiveStepChanged" 
	DisplaySideBar="false" StartNextButtonStyle-CssClass="Button1" 
	FinishCompleteButtonStyle-CssClass="Button1" NavigationStyle-HorizontalAlign="Left" 
	NavigationButtonStyle-CssClass="Button1" meta:resourcekey="wzrdOrderHosting" Width="100%">
	<WizardSteps>
		<asp:WizardStep runat="server" ID="Step1" AllowReturn="true" StepType="Auto">
			<wsp:QuickPlans runat="server" ID="ctlQuickPlans" OnPlanSelected="ctlQuickPlans_OnPlanSelected" />

			<wsp:PlanCycles runat="server" ID="ctlPlanCycles" Visible="false" />

			<wsp:DomainOption runat="server" ID="ctlPlanDomain" Visible="false" />

			<wsp:SelectPaymentMethod runat="server" ID="ctlPaymentMethod" />

			<asp:PlaceHolder runat="server" ID="pnlUserAccount">
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclUserAccount" /></div>
			</div>
			<div class="FormBody">
				<wsp:CreateUser runat="server" ID="ctlUserAccount" />
			</div>
			</asp:PlaceHolder>
		</asp:WizardStep>
		<asp:WizardStep runat="server" ID="Step2" StepType="Finish">
		    <div class="ProductInfo">
				<asp:Localize runat="server" meta:resourcekey="lclTermsAndConds" />
			</div>
			<div class="FormBody Scrollable500" runat="server" id="pnlTermsAndConds"></div>
		</asp:WizardStep>
	</WizardSteps>
	<FinishNavigationTemplate>
		<center class="FormFooter">
			<asp:Button id="Button1" runat="server" CssClass="Button1" meta:resourcekey="btnAccept" CommandName="MoveComplete" />&nbsp;&nbsp;&nbsp;
			<asp:Button id="Button2" runat="server" CssClass="Button1" meta:resourcekey="btnDecline" CommandName="MoveCancel" />
		</center>
	</FinishNavigationTemplate>
</asp:Wizard>
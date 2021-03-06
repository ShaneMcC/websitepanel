﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcCreateServer.ascx.cs"
    Inherits="WebsitePanel.Portal.VPSForPC.VdcCreate" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl"
    TagPrefix="wsp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel"
    TagPrefix="wsp" %>
<%@ Register Src="../UserControls/CheckBoxOption.ascx" TagName="CheckBoxOption" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="wsp" %>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<div id="VpsContainer">
    <div class="Module">
        <div class="Header">
            <wsp:Breadcrumb id="breadcrumb" runat="server" />
        </div>
        <div class="Left">
            <wsp:Menu id="menu" runat="server" SelectedItem="" />
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="imgIcon" SkinID="AddServer48" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Create New VM"></asp:Localize>
                </div>
                <div class="FormBody">
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    <asp:ValidationSummary ID="validatorsSummary" runat="server" ValidationGroup="VpsWizard"
                        ShowMessageBox="True" ShowSummary="False" />
                    <asp:Wizard ID="wizard" runat="server" meta:resourcekey="wizard" CellSpacing="5"
                        OnFinishButtonClick="wizard_FinishButtonClick" OnSideBarButtonClick="wizard_SideBarButtonClick"
                        OnActiveStepChanged="wizard_ActiveStepChanged" OnNextButtonClick="wizard_NextButtonClick">
                        <SideBarStyle CssClass="SideBar" VerticalAlign="Top" />
                        <StepStyle VerticalAlign="Top" />
                        <StartNavigationTemplate>
                            <asp:Button ID="btnNext" runat="server" CommandName="MoveNext" ValidationGroup="VpsWizard"
                                CssClass="Button1" Text="Next" meta:resourcekey="btnNext" />
                        </StartNavigationTemplate>
                        <StepNavigationTemplate>
                            <asp:Button ID="btnPrevious" runat="server" CommandName="MovePrevious" ValidationGroup="VpsWizard"
                                CssClass="Button1" Text="Previous" meta:resourcekey="btnPrevious" />
                            <asp:Button ID="btnNext" runat="server" CommandName="MoveNext" ValidationGroup="VpsWizard"
                                CssClass="Button1" Text="Next" meta:resourcekey="btnNext" />
                        </StepNavigationTemplate>
                        <FinishNavigationTemplate>
                            <asp:Button ID="btnPrevious" runat="server" CommandName="MovePrevious" ValidationGroup="VpsWizard"
                                CssClass="Button1" Text="Previous" meta:resourcekey="btnPrevious" />
                            <asp:Button ID="btnFinish" runat="server" CommandName="MoveComplete" ValidationGroup="VpsWizard"
                                CssClass="Button1" Text="Finish" meta:resourcekey="btnFinish" />
                        </FinishNavigationTemplate>
                        <WizardSteps>
                            <asp:WizardStep ID="stepName" runat="server" meta:resourcekey="stepName" Title="Name and OS">
                                <p class="SubTitle">
                                    <asp:Localize ID="locNameStepTitle" runat="server" meta:resourcekey="locNameStepTitle"
                                        Text="Name and Operating System" /></p>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel150">
                                            <asp:Localize ID="locOperatingSystem" runat="server" meta:resourcekey="locOperatingSystem"
                                                Text="Operating system:"></asp:Localize>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="listOperatingSystems" runat="server" DataValueField="Path"
                                                DataTextField="Name">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="OperatingSystemValidator" runat="server" Text="*"
                                                Display="Dynamic" ControlToValidate="listOperatingSystems" meta:resourcekey="OperatingSystemValidator"
                                                SetFocusOnError="true" ValidationGroup="VpsWizard">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel150" valign="top">
                                            <asp:Localize ID="Localize3" runat="server" meta:resourcekey="VMName" Text="VM Name:"></asp:Localize>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVmName" runat="server" ValidationGroup="VpsWizard">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vmNameValidator" runat="server" Text="*" Display="Dynamic"
                                                ControlToValidate="txtVmName" meta:resourcekey="vmNameValidator" SetFocusOnError="true"
                                                ValidationGroup="VpsWizard">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="FormLabel150">
                                            <asp:CheckBox ID="chkSendSummary" runat="server" AutoPostBack="true" Checked="true"
                                                meta:resourcekey="chkSendSummary" Text="Send summary letter to:" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSummaryEmail" runat="server" CssClass="NormalTextBox" AutoPostBack="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="SummaryEmailValidator" runat="server" Text="*" Display="Dynamic"
                                                ControlToValidate="txtSummaryEmail" meta:resourcekey="SummaryEmailValidator"
                                                SetFocusOnError="true" ValidationGroup="VpsWizard">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:WizardStep>
                            <asp:WizardStep ID="stepConfig" runat="server" meta:resourcekey="stepConfig" Title="Configuration">
                                <p class="SubTitle">
                                    <asp:Localize ID="locConfigStepTitle" runat="server" meta:resourcekey="locConfigStepTitle"
                                        Text="Configuration" /></p>
                                <br />
                                <wsp:CollapsiblePanel id="secResources" runat="server" TargetControlID="ResourcesPanel"
                                    meta:resourcekey="secResources" Text="Resources">
                                </wsp:CollapsiblePanel>
                                <asp:Panel ID="ResourcesPanel" runat="server" Height="0" Style="overflow: hidden;
                                    padding: 10px; width: 400px;">
                                    <table cellpadding="3">
                                        <tr>
                                            <td style="width: 60px;">
                                                <asp:Label ID="lblCpu" runat="server" AssociatedControlID="ddlCpu" meta:resourcekey="lblCpu"
                                                    Text="CPU:" CssClass="MediumBold" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCpu" runat="server" CssClass="HugeTextBox" Width="70">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Localize ID="locCores" runat="server" meta:resourcekey="locCores" Text="cores" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="3">
                                        <tr>
                                            <td style="width: 60px;">
                                                <asp:Label ID="lblRam" runat="server" AssociatedControlID="txtRam" meta:resourcekey="lblRam"
                                                    Text="RAM:" CssClass="MediumBold" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRam" runat="server" CssClass="HugeTextBox" Width="70" Text="0"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequireRamValidator" runat="server" Text="*" Display="Dynamic"
                                                    ControlToValidate="txtRam" meta:resourcekey="RequireRamValidator" SetFocusOnError="true"
                                                    ValidationGroup="VpsWizard">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Localize ID="locMB" runat="server" meta:resourcekey="locMB" Text="MB" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="3">
                                        <tr>
                                            <td style="width: 60px;">
                                                <asp:Label ID="lblHdd" runat="server" AssociatedControlID="txtHdd" meta:resourcekey="lblHdd"
                                                    Text="HDD:" CssClass="MediumBold" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHdd" runat="server" CssClass="HugeTextBox" Width="70" Text="0"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequireHddValidator" runat="server" Text="*" Display="Dynamic"
                                                    ControlToValidate="txtHdd" meta:resourcekey="RequireHddValidator" SetFocusOnError="true"
                                                    ValidationGroup="VpsWizard">*</asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:Localize ID="locGB" runat="server" meta:resourcekey="locGB" Text="GB" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <wsp:CollapsiblePanel id="secSnapshots" runat="server" TargetControlID="SnapshotsPanel"
                                    meta:resourcekey="secSnapshots" Text="Snapshots">
                                </wsp:CollapsiblePanel>
                                <asp:Panel ID="SnapshotsPanel" runat="server" Height="0" Style="overflow: hidden;
                                    padding: 5px;">
                                    <table>
                                        <tr>
                                            <td class="FormLabel150">
                                                <asp:Localize ID="locSnapshots" runat="server" meta:resourcekey="locSnapshots" Text="Number of snapshots:"></asp:Localize>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSnapshots" runat="server" CssClass="NormalTextBox" Width="50"
                                                    Text="0"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="SnapshotsValidator" runat="server" Text="*" Display="Dynamic"
                                                    ControlToValidate="txtSnapshots" meta:resourcekey="SnapshotsValidator" SetFocusOnError="true"
                                                    ValidationGroup="VpsWizard">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <wsp:CollapsiblePanel id="secDvd" runat="server" TargetControlID="DvdPanel" meta:resourcekey="secDvd"
                                    Text="DVD">
                                </wsp:CollapsiblePanel>
                                <asp:Panel ID="DvdPanel" runat="server" Height="0" Style="overflow: hidden; padding: 5px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkDvdInstalled" runat="server" Text="DVD drive installed" meta:resourcekey="chkDvdInstalled" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <wsp:CollapsiblePanel id="secBios" runat="server" TargetControlID="BiosPanel" meta:resourcekey="secBios"
                                    Text="BIOS">
                                </wsp:CollapsiblePanel>
                                <asp:Panel ID="BiosPanel" runat="server" Height="0" Style="overflow: hidden; padding: 5px;">
                                    <table>
                                        <tr>
                                            <td style="width: 200px;">
                                                <asp:CheckBox ID="chkBootFromCd" runat="server" Text="Boot from CD" meta:resourcekey="chkBootFromCd" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkNumLock" runat="server" Text="Num Lock enabled" meta:resourcekey="chkNumLock" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <wsp:CollapsiblePanel id="secActions" runat="server" TargetControlID="ActionsPanel"
                                    meta:resourcekey="secActions" Text="Allowed actions">
                                </wsp:CollapsiblePanel>
                                <asp:Panel ID="ActionsPanel" runat="server" Height="0" Style="overflow: hidden; padding: 5px;">
                                    <table style="width: 400px;">
                                        <tr>
                                            <td style="width: 200px;">
                                                <asp:CheckBox ID="chkStartShutdown" runat="server" Text="Start, Turn off and Shutdown"
                                                    meta:resourcekey="chkStartShutdown" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkReset" runat="server" Text="Reset" meta:resourcekey="chkReset" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkPauseResume" runat="server" Text="Pause, Resume" meta:resourcekey="chkPauseResume" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkReinstall" runat="server" Text="Re-install" meta:resourcekey="chkReinstall" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkReboot" runat="server" Text="Reboot" meta:resourcekey="chkReboot" />
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                            </asp:WizardStep>
                             <asp:WizardStep ID="stepPrivateNetwork" runat="server" meta:resourcekey="stepPrivateNetwork"
                                Title="Private network">
                                <p class="SubTitle">
                                    <asp:Localize ID="locPrivateNetwork" runat="server" meta:resourcekey="locPrivateNetwork"
                                        Text="Private Network" /></p>
                                <br />
                                <p>
                                    <asp:CheckBox ID="chkPrivateNetworkEnabled" runat="server" AutoPostBack="true" Checked="true"
                                        meta:resourcekey="chkPrivateNetworkEnabled" Text="Private network enabled" />
                                </p>
                                <p runat="server" id="pVLanListIsEmptyMessage">User account does not have available VLans</p>
                                <table id="tablePrivateNetwork" runat="server" cellspacing="5" style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Localize ID="lvPrivateVLanID" Text="VLanID" runat="server" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPrivateVLanID" runat="server" DataTextField="VLanID" DataValueField="VLanID" EnableViewState="true" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:WizardStep>
                           <asp:WizardStep ID="stepExternalNetwork" runat="server" meta:resourcekey="stepExternalNetwork"
                                Title="External network" >
                                <p class="SubTitle">
                                    <asp:Localize ID="locExternalNetwork" runat="server" meta:resourcekey="locExternalNetwork"
                                        Text="External Network" /></p>
                                <br />
                                <p>
                                    <asp:CheckBox ID="chkExternalNetworkEnabled" runat="server" AutoPostBack="true" Checked="true"
                                        meta:resourcekey="chkExternalNetworkEnabled" Text="External network enabled" />
                                </p>
                                <div runat="server" id="EmptyExternalAddressesMessage" style="padding: 5px;" visible="false">
                                    <asp:Localize ID="locNotEnoughExternalAddresses" runat="server" Text="Not enough..."
                                        meta:resourcekey="locNotEnoughExternalAddresses"></asp:Localize>
                                </div>
                                <br />
                            </asp:WizardStep>
                            <asp:WizardStep ID="stepSummary" runat="server" meta:resourcekey="stepSummary" Title="Summary">
                                <p class="SubTitle">
                                    <asp:Localize ID="locSummary" runat="server" meta:resourcekey="locSummary" Text="Summary" /></p>
                                <br />
                                <table cellspacing="6">
                                    <tr>
                                        <td colspan="2" class="NormalBold">
                                            <asp:Localize ID="locNameStepTitle2" runat="server" meta:resourcekey="locNameStepTitle"
                                                Text="Name and Operating System" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="Localize1" runat="server" meta:resourcekey="locHostname" Text="Host name"></asp:Localize>
                                        </td>
                                        <td>
                                            <asp:Literal ID="litHostname" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="Localize2" runat="server" meta:resourcekey="locOperatingSystem"
                                                Text="Operating system"></asp:Localize>
                                        </td>
                                        <td>
                                            <asp:Literal ID="litOperatingSystem" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr id="SummSummaryEmailRow" runat="server">
                                        <td>
                                            <asp:Localize ID="locSendSummary" runat="server" meta:resourcekey="chkSendSummary"
                                                Text="Send summary letter to"></asp:Localize>
                                        </td>
                                        <td>
                                            <asp:Literal ID="litSummaryEmail" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="NormalBold">
                                            <asp:Localize ID="locConfig2" runat="server" meta:resourcekey="locConfigStepTitle"
                                                Text="Configuration" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locCpu" runat="server" meta:resourcekey="locCpu" Text="CPU cores:" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="litCpu" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locRam" runat="server" meta:resourcekey="locRam" Text="RAM, MB:" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="litRam" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locHdd" runat="server" meta:resourcekey="locHdd" Text="Hard disk size, GB:" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="litHdd" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locSnapshots2" runat="server" meta:resourcekey="locSnapshots" Text="Number of snapshots:" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="litSnapshots" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locDvdInstalled" runat="server" meta:resourcekey="locDvdInstalled"
                                                Text="DVD Drive installed:" />
                                        </td>
                                        <td>
                                            <wsp:CheckBoxOption id="optionDvdInstalled" runat="server" Value="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locBootFromCd" runat="server" meta:resourcekey="locBootFromCd"
                                                Text="Boot from CD:" />
                                        </td>
                                        <td>
                                            <wsp:CheckBoxOption id="optionBootFromCd" runat="server" Value="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locNumLock" runat="server" meta:resourcekey="locNumLock" Text="Num Lock enabled:" />
                                        </td>
                                        <td>
                                            <wsp:CheckBoxOption id="optionNumLock" runat="server" Value="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locStartShutdownAllowed" runat="server" meta:resourcekey="locStartShutdownAllowed"
                                                Text="Start, turn off and shutdown allowed:" />
                                        </td>
                                        <td>
                                            <wsp:CheckBoxOption id="optionStartShutdown" runat="server" Value="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locPauseResumeAllowed" runat="server" meta:resourcekey="locPauseResumeAllowed"
                                                Text="Pause, resume allowed:" />
                                        </td>
                                        <td>
                                            <wsp:CheckBoxOption id="optionPauseResume" runat="server" Value="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locRebootAllowed" runat="server" meta:resourcekey="locRebootAllowed"
                                                Text="Reboot allowed:" />
                                        </td>
                                        <td>
                                            <wsp:CheckBoxOption id="optionReboot" runat="server" Value="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locResetAllowed" runat="server" meta:resourcekey="locResetAllowed"
                                                Text="Reset allowed:" />
                                        </td>
                                        <td>
                                            <wsp:CheckBoxOption id="optionReset" runat="server" Value="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locReinstallAllowed" runat="server" meta:resourcekey="locReinstallAllowed"
                                                Text="Re-install allowed:" />
                                        </td>
                                        <td>
                                            <wsp:CheckBoxOption id="optionReinstall" runat="server" Value="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="NormalBold">
                                            <asp:Localize ID="locExternalNetwork2" runat="server" meta:resourcekey="locExternalNetwork"
                                                Text="External Network" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locExternalNetworkEnabled" runat="server" meta:resourcekey="locExternalNetworkEnabled"
                                                Text="External network enabled:" />
                                        </td>
                                        <td>
                                            <wsp:CheckBoxOption id="optionExternalNetwork" runat="server" Value="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="NormalBold">
                                            <asp:Localize ID="locPrivateNetwork2" runat="server" meta:resourcekey="locPrivateNetwork"
                                                Text="Private Network" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locPrivateNetworkEnabled" runat="server" meta:resourcekey="locPrivateNetworkEnabled"
                                                Text="Private network enabled:" />
                                        </td>
                                        <td>
                                            <wsp:CheckBoxOption id="optionPrivateNetwork" runat="server" Value="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locPrivateNetworkVLanID" runat="server" meta:resourcekey="locPrivateNetworkVLanID"
                                                Text="Private VLan:" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="litPrivateNetworkVLanID" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:WizardStep>
                        </WizardSteps>
                        <StepPreviousButtonStyle CssClass="Button1" />
                        <CancelButtonStyle CssClass="Button1" />
                    </asp:Wizard>
                </div>
            </div>
            <div class="Right">
                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
            </div>
        </div>
    </div>
</div>

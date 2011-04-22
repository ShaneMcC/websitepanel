<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HyperVForPrivateCloud_Settings.ascx.cs"
	Inherits="WebsitePanel.Portal.ProviderControls.HyperVForPrivateCloud_Settings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<asp:ValidationSummary ID="ValidationSummary" runat="server" ShowMessageBox="true"
	ShowSummary="false" />
<fieldset>
	<legend>
		<asp:Localize ID="locHyperVServer" runat="server" meta:resourcekey="locHyperVServer"
			Text="Host name"></asp:Localize>
	</legend>
    <wsp:SimpleMessageBox id="messageBoxError" runat="server" />
	<table cellpadding="2" cellspacing="0" style="margin: 10px;">
		<tr>
			<td class="SubHead" style="width: 200px;">
				<asp:Localize ID="locConnectTypeName" runat="server" meta:resourcekey="locConnectTypeName"
					Text="Select Server type:"></asp:Localize>
			</td>
			<td>
				<asp:RadioButtonList ID="radioServer" runat="server" AutoPostBack="true" OnSelectedIndexChanged="radioServer_SelectedIndexChanged">
					<asp:ListItem Value="cluster" meta:resourcekey="radioServerCloud" Selected="True">Cluster</asp:ListItem>
					<asp:ListItem Value="host" meta:resourcekey="radioServerHost">Host</asp:ListItem>
				</asp:RadioButtonList>
			</td>
		</tr>
		<tr id="ServerNameRow" runat="server">
			<td class="FormLabel150">
				<asp:Localize ID="locHost" runat="server" meta:resourcekey="locHost" Text="Hosts:"></asp:Localize>
			</td>
			<td>
				<asp:DropDownList ID="listHosts" runat="server" DataValueField="Path" DataTextField="Name"
					CssClass="NormalTextBox" Width="500" AutoPostBack="true" OnSelectedIndexChanged="listHosts_OnSelectedIndexChanged">
				</asp:DropDownList>
				<asp:RequiredFieldValidator ID="HostsValidator" runat="server" Text="*" Display="Dynamic"
					ControlToValidate="listHosts" meta:resourcekey="HostsValidator" SetFocusOnError="true"
					ValidationGroup="ValidationSummary">*</asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr id="LibraryPath" runat="server">
			<td class="FormLabel150">
				<asp:Localize ID="locLibraryPath" runat="server" meta:resourcekey="locLibraryPath"
					Text="Library path:"></asp:Localize>
			</td>
			<td>
				<asp:TextBox Width="300px" CssClass="NormalTextBox" runat="server" ID="txtLibraryPath"></asp:TextBox>
				<asp:RequiredFieldValidator ID="LibraryPathValidator" runat="server" ControlToValidate="txtLibraryPath"
					Text="*" meta:resourcekey="LibraryPathValidator" Display="Dynamic" SetFocusOnError="true" />
			</td>
		</tr>
		<tr id="monitoringServerName" runat="server">
			<td class="FormLabel150">
				<asp:Localize ID="locMonitoringServerName" runat="server" meta:resourcekey="locMonitoringServerName"
					Text="Monitoring server:"></asp:Localize>
			</td>
			<td>
				<asp:TextBox Width="300px" CssClass="NormalTextBox" runat="server" ID="txtMonitoringServerName"></asp:TextBox>
				<asp:RequiredFieldValidator ID="MonitoringServerNameValidator" runat="server" ControlToValidate="txtMonitoringServerName"
					Text="*" meta:resourcekey="MonitoringServerNameValidator" Display="Dynamic" SetFocusOnError="true" />
			</td>
		</tr>
	</table>
</fieldset>
<br />
<asp:Panel runat="server" ID="upHyperVCloud">
	<fieldset>
		<legend>
			<asp:Localize ID="locHyperVCloud" runat="server" meta:resourcekey="locHyperVCloud"
				Text="Hyper-V Cloud"></asp:Localize>
		</legend>
		<fieldset>
			<legend>
				<asp:Localize ID="locSCVMMServer" runat="server" meta:resourcekey="locSCVMMServer"
					Text="SCVMM Server Connection options"></asp:Localize>
			</legend>
			<table cellpadding="2" cellspacing="0" style="margin: 10px;">
				<tr>
					<td class="FormLabel150">
						<asp:Label ID="lblSCVMMServer" runat="server" meta:resourcekey="lblSCVMMServer" Text="Server URL:"
							CssClass="SubHead"></asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtSCVMMServer" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
						<asp:Button ID="btnSCVMMServer" runat="server" CssClass="disabled" meta:resourcekey="btnSCVMMServer"
							CausesValidation="false" OnClick="btnSCVMMServer_Click" />
					</td>
				</tr>
				<tr>
					<td class="FormLabel150">
						<asp:Label ID="lblSCVMMPrincipalName" runat="server" meta:resourcekey="lblSCVMMPrincipalName"
							Text="Principal Name:" CssClass="SubHead"></asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtSCVMMPrincipalName" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
					</td>
				</tr>
			</table>
		</fieldset>
		<fieldset>
			<legend>
				<asp:Localize ID="locSCOMServer" runat="server" meta:resourcekey="locSCOMServer"
					Text="SCOM Server Connection options"></asp:Localize>
			</legend>
			<table cellpadding="2" cellspacing="0" style="margin: 10px;">
				<tr>
					<td class="FormLabel150">
						<asp:Label ID="lblSCOMServer" runat="server" meta:resourcekey="lblSCOMServer" Text="Server URL:"
							CssClass="SubHead"></asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtSCOMServer" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
						<asp:Button ID="btnSCOMServer" runat="server" CssClass="disabled" meta:resourcekey="btnSCOMServer"
							CausesValidation="false" OnClick="btnSCOMServer_Click" />
					</td>
				</tr>
				<tr>
					<td class="FormLabel150">
						<asp:Label ID="lblSCOMPrincipalName" runat="server" meta:resourcekey="lblSCOMPrincipalName"
							Text="Principal Name:" CssClass="SubHead"></asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtSCOMPrincipalName" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
					</td>
				</tr>
			</table>
		</fieldset>
	</fieldset>
</asp:Panel>
<br />
<fieldset>
	<legend>
		<asp:Localize ID="locProcessorSettings" runat="server" meta:resourcekey="locProcessorSettings"
			Text="Processor Resource Settings"></asp:Localize>
	</legend>
	<table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
		<tr>
			<td class="SubHead" style="width: 200px;">
				<asp:Localize ID="locCpuReserve" runat="server" meta:resourcekey="locCpuReserve"
					Text="Virtual machine reserve:"></asp:Localize>
			</td>
			<td>
				<asp:TextBox Width="50px" CssClass="NormalTextBox" runat="server" ID="txtCpuReserve"></asp:TextBox>
				%
				<asp:RequiredFieldValidator ID="CpuReserveValidator" runat="server" ControlToValidate="txtCpuReserve"
					Text="*" meta:resourcekey="CpuReserveValidator" Display="Dynamic" SetFocusOnError="true" />
			</td>
		</tr>
		<tr>
			<td class="SubHead" style="width: 200px;">
				<asp:Localize ID="locCpuLimit" runat="server" meta:resourcekey="locCpuLimit" Text="Virtual machine limit:"></asp:Localize>
			</td>
			<td>
				<asp:TextBox Width="50px" CssClass="NormalTextBox" runat="server" ID="txtCpuLimit"></asp:TextBox>
				%
				<asp:RequiredFieldValidator ID="CpuLimitValidator" runat="server" ControlToValidate="txtCpuLimit"
					Text="*" meta:resourcekey="CpuLimitValidator" Display="Dynamic" SetFocusOnError="true" />
			</td>
		</tr>
		<tr>
			<td class="SubHead" style="width: 200px;">
				<asp:Localize ID="locCpuWeight" runat="server" meta:resourcekey="locCpuWeight" Text="Relative weight:"></asp:Localize>
			</td>
			<td>
				<asp:TextBox Width="50px" CssClass="NormalTextBox" runat="server" ID="txtCpuWeight"></asp:TextBox>
				<asp:RequiredFieldValidator ID="CpuWeightValidator" runat="server" ControlToValidate="txtCpuWeight"
					Text="*" meta:resourcekey="CpuWeightValidator" Display="Dynamic" SetFocusOnError="true" />
			</td>
		</tr>
	</table>
</fieldset>
<br />
<fieldset>
	<legend>
		<asp:Localize ID="locVhd" runat="server" meta:resourcekey="locVhd" Text="Virtual Hard Drive"></asp:Localize>
	</legend>
	<table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
		<tr>
			<td class="SubHead" style="width: 200px;" valign="top">
				<asp:Localize ID="locDiskType" runat="server" meta:resourcekey="locDiskType" Text="Disk Type:"></asp:Localize>
			</td>
			<td>
				<asp:RadioButtonList ID="radioVirtualDiskType" runat="server">
					<asp:ListItem Value="dynamic" meta:resourcekey="radioVirtualDiskTypeDynamic" Selected="True">Dynamic</asp:ListItem>
					<asp:ListItem Value="fixed" meta:resourcekey="radioVirtualDiskTypeFixed">Fixed</asp:ListItem>
				</asp:RadioButtonList>
			</td>
		</tr>
	</table>
</fieldset>
<br />
<fieldset>
	<legend>
		<asp:Localize ID="locExternalNetwork" runat="server" meta:resourcekey="locExternalNetwork"
			Text="External Network"></asp:Localize>
	</legend>
	<table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
		<tr>
			<td class="SubHead" style="width: 200px;">
				<asp:Localize ID="locExternalNetworkName" runat="server" meta:resourcekey="locExternalNetworkName"
					Text="Connect to Network:"></asp:Localize>
			</td>
			<td>
				<asp:DropDownList ID="ddlExternalNetworks" runat="server" CssClass="NormalTextBox"
					Width="450" DataValueField="Name" DataTextField="Name" />
				<asp:RequiredFieldValidator ID="ExternalNetworkValidator" runat="server" Text="*"
					Display="Dynamic" ControlToValidate="ddlExternalNetworks" meta:resourcekey="ExternalNetworkValidator"
					SetFocusOnError="true" ValidationGroup="ValidationSummary">*</asp:RequiredFieldValidator>
			</td>
		</tr>
	</table>
</fieldset>
<br />
<fieldset>
	<legend>
		<asp:Localize ID="locPrivateNetwork" runat="server" meta:resourcekey="locPrivateNetwork"
			Text="Private Network"></asp:Localize>
	</legend>
	<table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
		<tr>
			<td class="SubHead" style="width: 200px;">
				<asp:Localize ID="locPrivateNetworkName" runat="server" meta:resourcekey="locPrivateNetworkName"
					Text="Connect to Network:"></asp:Localize>
			</td>
			<td>
				<asp:DropDownList ID="ddlPrivateNetworks" runat="server" CssClass="NormalTextBox"
					Width="450" DataValueField="Name" DataTextField="Name" />
				<asp:RequiredFieldValidator ID="PrivateNetworkValidator" runat="server" Text="*"
					Display="Dynamic" ControlToValidate="ddlPrivateNetworks" meta:resourcekey="PrivateNetworkValidator"
					SetFocusOnError="true" ValidationGroup="ValidationSummary">*</asp:RequiredFieldValidator>
			</td>
		</tr>
	</table>
</fieldset>
<br />
<fieldset>
	<legend>
		<asp:Localize ID="locHostname" runat="server" meta:resourcekey="locHostname" Text="Host name"></asp:Localize>
	</legend>
	<table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
		<tr>
			<td class="SubHead" style="width: 200px;">
				<asp:Localize ID="locHostnamePattern" runat="server" meta:resourcekey="locHostnamePattern"
					Text="VPS host name pattern:"></asp:Localize>
			</td>
			<td>
				<asp:TextBox Width="200px" CssClass="NormalTextBox" runat="server" ID="txtHostnamePattern"></asp:TextBox>
				<asp:RequiredFieldValidator ID="HostnamePatternValidator" runat="server" ControlToValidate="txtHostnamePattern"
					Text="*" meta:resourcekey="HostnamePatternValidator" Display="Dynamic" SetFocusOnError="true" />
			</td>
		</tr>
	</table>
	<p style="margin: 10px;">
		<asp:Localize ID="locPatternText" runat="server" meta:resourcekey="locPatternText"
			Text="Help text goes here..."></asp:Localize>
	</p>
</fieldset>
<br />
<fieldset>
	<legend>
		<asp:Localize ID="locStartAction" runat="server" meta:resourcekey="locStartAction"
			Text="Automatic Start Action"></asp:Localize>
	</legend>
	<table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
		<tr>
			<td>
				<asp:Localize ID="locStartOptionsText" runat="server" meta:resourcekey="locStartOptionsText"
					Text="What do you want VPS to do when the physical computer starts?"></asp:Localize>
			</td>
		</tr>
		<tr>
			<td>
				<asp:RadioButtonList ID="radioStartAction" runat="server">
					<asp:ListItem Value="0" meta:resourcekey="radioStartActionNothing">Nothing</asp:ListItem>
					<asp:ListItem Value="1" meta:resourcekey="radioStartActionStart" Selected="True">Start</asp:ListItem>
					<asp:ListItem Value="2" meta:resourcekey="radioStartActionAlwaysStart">AlwaysStart</asp:ListItem>
				</asp:RadioButtonList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize ID="locStartupDelayText" runat="server" meta:resourcekey="locStartupDelayText"
					Text="Specify a startup delay to reduce resource contention between virtual machines."></asp:Localize>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize ID="locStartupDelay" runat="server" meta:resourcekey="locStartupDelay"
					Text="Startup delay:"></asp:Localize>
				<asp:TextBox ID="txtStartupDelay" runat="server" Width="30px"></asp:TextBox>
				<asp:Localize ID="locSeconds" runat="server" meta:resourcekey="locSeconds" Text="seconds"></asp:Localize>
				<asp:RequiredFieldValidator ID="StartupDelayValidator" runat="server" ControlToValidate="txtStartupDelay"
					Text="*" meta:resourcekey="StartupDelayValidator" Display="Dynamic" SetFocusOnError="true" />
			</td>
		</tr>
	</table>
</fieldset>
<br />
<fieldset>
	<legend>
		<asp:Localize ID="locStopAction" runat="server" meta:resourcekey="locStopAction"
			Text="Automatic Stop Action"></asp:Localize>
	</legend>
	<table cellpadding="2" cellspacing="0" width="100%" style="margin: 10px;">
		<tr>
			<td>
				<asp:Localize ID="locStopActionText" runat="server" meta:resourcekey="locStopActionText"
					Text="What do you want VPS to do when the physical shuts down?"></asp:Localize>
			</td>
		</tr>
		<tr>
			<td>
				<asp:RadioButtonList ID="radioStopAction" runat="server">
					<asp:ListItem Value="1" meta:resourcekey="radioStopActionSave">Save</asp:ListItem>
					<asp:ListItem Value="0" meta:resourcekey="radioStopActionTurnOff">TurnOff</asp:ListItem>
					<asp:ListItem Value="2" meta:resourcekey="radioStopActionShutDown" Selected="True">ShutDown</asp:ListItem>
				</asp:RadioButtonList>
			</td>
		</tr>
	</table>
</fieldset>
<br />

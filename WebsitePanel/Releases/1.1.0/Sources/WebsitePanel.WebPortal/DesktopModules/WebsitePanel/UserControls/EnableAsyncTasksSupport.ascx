<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnableAsyncTasksSupport.ascx.cs" Inherits="WebsitePanel.Portal.EnableAsyncTasksSupport" %>
<%@ Import Namespace="WebsitePanel.Portal" %>

<input type="hidden" id="taskID" runat="server" />

<script type="text/javascript" language="javascript" type="text/javascript">
	var _ctrlTaskID = "<%= taskID.ClientID %>";
	var _completeMessage = "<%= GetLocalizedString("Text.CompleteMessage") %>";
</script>
<script src="<%= GetAjaxUtilsUrl() %>" language="javascript" type="text/javascript"></script>

<asp:Panel id="pnlModal" runat="server" CssClass="PopupContainer" style="display:none">
	<table class="Header" cellpadding="0" cellspacing="0">
		<tr>
			<td class="HeaderLeft"></td>
			<td class="HeaderTitle">
				<asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitle" Text="Running"></asp:Label>
			</td>
			<td class="HeaderRight"></td>
		</tr>
	</table>
	<div class="Content">
		<div class="FormBody">
			<div class="ProgressPanelArea">
				<div class="MediumBold">
					<img id="imgAjaxIndicator" src='<%= PortalUtils.GetThemedImage("indicator_medium.gif") %>' align="absmiddle" />&nbsp;
					<span id="objProgressDialogTitle"></span>
				</div>
			</div>
			<div id="ProgressPanelArea" class="ProgressPanelArea">
				<fieldset>
					<table width="100%" cellpadding="3">
						<tr>
							<td class="SubHead" colspan="2">
								<span id="objProgressDialogStep">Step</span>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<div class="ProgressBarContainer">
									<div id="objProgressDialogProgressBar" class="ProgressBarIndicator" style="width:0px;"></div>
								</div>
							</td>
						</tr>
						<tr>
							<td class="SubHead" width="100" nowrap>
								<asp:Label ID="lblStarted" runat="server" meta:resourcekey="lblStarted" Text="Started"></asp:Label>
							</td>
							<td class="Normal" width="100%">
								<span id="progressStartTime"></span>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblDuration" runat="server" meta:resourcekey="lblDuration" Text="Duration"></asp:Label>   
							</td>
							<td class="Normal">
								<span id="progressDuration"></span>
							</td>
						</tr>
					</table>
				</fieldset>
			</div>
		</div>	
		<div id="PopupFormFooter" class="FormFooter" style="text-align: center;">
			<div id="objProgressDialogCommandButtons">
				<asp:Button id="btnCancelProgressDialog" Text="  Close  " CssClass="Button1" meta:resourcekey="btnCancelProgressDialog" runat="server" />
			</div>
			<div id="objProgressDialogCloseButton" style="display: none;">
				<asp:Button id="btnCloseProgressDialog" Text="  OK  " CssClass="Button1" meta:resourcekey="btnCloseProgressDialog" runat="server" />
			</div>
		</div>
	</div>
</asp:Panel>


<ajaxToolkit:ModalPopupExtender ID="ModalPopupProperties" runat="server"
	BehaviorID="ModalPopupProperties"
	TargetControlID="btnShowProgressDialog" PopupControlID="pnlModal"
    BackgroundCssClass="modalBackground" DropShadow="false"
    OkControlID="btnCloseProgressDialog" OnCancelScript="OnCancelProgressDialog()"
    CancelControlID="btnCancelProgressDialog" />

<asp:Button id="btnShowProgressDialog" runat="server" Text="Progress" style="display:none;" />
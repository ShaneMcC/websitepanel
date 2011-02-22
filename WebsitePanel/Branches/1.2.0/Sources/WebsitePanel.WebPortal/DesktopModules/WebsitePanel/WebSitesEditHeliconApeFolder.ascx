<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesEditHeliconApeFolder.ascx.cs"
	Inherits="WebsitePanel.Portal.WebSitesEditHeliconApeFolder" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>
<script type="text/javascript">
	function pageLoad() {
		$('input:hidden').each(function (index, el) {
			if ($(this).attr('id').indexOf('ApeDebuggerUrl') >= 0) {
				if (this.value) {
					$('#toolbar-start-debug').show();
				}
			}
		});

	};

	function openDebugWindow() {
		$('input:hidden').each(function (index, el) {
			if ($(this).attr('id').indexOf('ApeDebuggerUrl') >= 0) {
				if (this.value) {
					window.open(this.value);
				}
			}
		});

		return false;
	}
</script>
<div class="FormBody">
	<table cellspacing="0" cellpadding="0" width="100%">
		<tr>
			<td>
				<table cellspacing="0" cellpadding="5" width="100%">
					<tr>
						<td class="SubHead">
							<asp:Label ID="lblFolderName" runat="server" meta:resourcekey="lblFolderName" Text="Folder Path:"></asp:Label>
						</td>
						<td class="NormalBold">
							<uc1:FileLookup id="folderPath" runat="server" Width="400">
							</uc1:FileLookup>
							<asp:HiddenField ID="contentPath" runat="server" />
							<asp:HiddenField ID="ApeDebuggerUrl" runat="server" />
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<table id="toolbar">
								<tr>
									<td>
										<asp:Button ID="btnApeDebug" runat="server" Text="Start Debug" meta:resourcekey="btnApeDebugger"
											CssClass="toolbar-button" OnClick="btnApeDebug_Click" />
									</td>
									<td>
										<button id="toolbar-start-debug" cssclass="toolbar-button" style="display: none;"
											onclick="return openDebugWindow();">
											Open&nbsp;Debug&nbsp;window</button>
									</td>
									<td class="toolbar-space">&nbsp;</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<asp:TextBox ID="htaccessContent" runat="server" TextMode="MultiLine" class="htaccess CodeEditor"></asp:TextBox>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
<div class="FormFooter">
	<asp:Button ID="btnUpdate" runat="server" Text="Update" meta:resourcekey="btnUpdate"
		CssClass="Button1" OnClick="btnUpdate_Click" />
	<asp:Button ID="btnSave" runat="server" Text="Save and continue editing" meta:resourcekey="btnSave"
		CssClass="Button1" OnClick="btnSave_Click" />
	<asp:Button ID="btnCancel" runat="server" Text="Cancel" meta:resourcekey="btnCancel"
		CssClass="Button1" CausesValidation="false" OnClick="btnCancel_Click" />
</div>

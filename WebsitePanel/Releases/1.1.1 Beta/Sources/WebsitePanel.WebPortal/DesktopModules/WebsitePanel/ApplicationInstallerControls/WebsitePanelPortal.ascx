<%@ Control Language="c#" AutoEventWireup="true" %>
<%@ Implements interface="WebsitePanel.Portal.IWebInstallerSettings" %>
<%@ Import namespace="WebsitePanel.EnterpriseServer" %>
<%@ Import Namespace="System.Text" %>

<script language="C#" runat="server">
		void Page_Load()
		{
		}
		
		public void GetSettings(InstallationInfo inst)
		{
			inst["wsp.portalname"] = txtPortalName.Text;
			inst["wsp.enterpriseserver"] = txtEsURL.Text;
		}
</script>
<table cellPadding="2" width="100%">
	<tr>
		<td align="left" width=200 nowrap class=SubHead>Portal Name:</TD>
		<td align="left" width=100% class=Normal>
			<asp:textbox id="txtPortalName" runat="server" CssClass="NormalTextBox" Width="200px">WebsitePanel</asp:textbox>
		</td>
	</tr>
	<tr>
		<td align="left" width=200 nowrap class=SubHead>Enterprise Server URL:</TD>
		<td align="left" width=100% class=Normal>
			<asp:textbox id="txtEsURL" runat="server" CssClass="NormalTextBox" Width="200px">http://127.0.0.1:9002</asp:textbox>
		</td>
	</tr>
</table>
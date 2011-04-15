<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailDomainsAddPointer.ascx.cs" Inherits="WebsitePanel.Portal.MailDomainsAddPointer" %>
<%@ Register Src="DomainsSelectDomainControl.ascx" TagName="DomainsSelectDomainControl" TagPrefix="uc1" %>
<div class="FormBody">
    <table cellspacing="0" cellpadding="4" width="100%">
	    <tr>
		    <td class="SubHead" width="200" nowrap><asp:Label ID="lblDomainName" runat="server" meta:resourcekey="lblDomainName" Text="Domain name:"></asp:Label></td>
		    <td width="100%">
                <uc1:DomainsSelectDomainControl ID="domainsSelectDomainControl" runat="server"
                    HideMailDomains="true" HideDomainsSubDomains="false" />
		    </td>
	    </tr>
    </table>
</div>
<div class="FormFooter">
    <asp:Button ID="btnAdd" runat="server" CssClass="Button1" meta:resourcekey="btnAdd" Text="Add" OnClick="btnAdd_Click" />
    <asp:Button ID="btnCancel" runat="server" CssClass="Button1" meta:resourcekey="btnCancel" CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click" />
</div>
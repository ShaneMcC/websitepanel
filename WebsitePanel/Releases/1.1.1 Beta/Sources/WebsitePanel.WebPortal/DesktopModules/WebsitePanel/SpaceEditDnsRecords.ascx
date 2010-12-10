<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceEditDnsRecords.ascx.cs" Inherits="WebsitePanel.Portal.SpaceEditDnsRecords" %>
<%@ Register Src="GlobalDnsRecordsControl.ascx" TagName="GlobalDnsRecordsControl" TagPrefix="uc1" %>

<div class="FormBody">
    <uc1:GlobalDnsRecordsControl ID="GlobalDnsRecordsControl1" runat="server" PackageIdParam="SpaceID" />
</div>
<div class="FormFooter">
    <asp:Button ID="btnBack" runat="server" meta:resourcekey="btnBack" CssClass="Button1" Text="Back" CausesValidation="false" OnClick="btnBack_Click" />
</div>
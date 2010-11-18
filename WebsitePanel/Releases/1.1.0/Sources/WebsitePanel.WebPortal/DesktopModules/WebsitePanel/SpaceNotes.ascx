<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceNotes.ascx.cs" Inherits="WebsitePanel.Portal.SpaceNotes" %>
<%@ Register Src="UserControls/EditItemComments.ascx" TagName="EditItemComments" TagPrefix="wsp" %>


<wsp:EditItemComments ID="packageComments" runat="server"
    ItemTypeId="PACKAGE" RequestItemId="SpaceID" />

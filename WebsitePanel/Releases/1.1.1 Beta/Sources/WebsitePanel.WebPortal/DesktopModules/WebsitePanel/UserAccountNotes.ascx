<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountNotes.ascx.cs" Inherits="WebsitePanel.Portal.UserAccountNotes" %>
<%@ Register Src="UserControls/EditItemComments.ascx" TagName="EditItemComments" TagPrefix="wsp" %>
   
<wsp:EditItemComments ID="userComments" runat="server"
    ItemTypeId="USER" />
            

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebsitePanel.WebPortal.DefaultPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link runat="server" rel="stylesheet" href="~/Styles/Import.css" type="text/css" id="AdaptersInvariantImportCSS" />
<!--[if lt IE 7]>
    <link runat="server" rel="stylesheet" href="~/Styles/BrowserSpecific/IEMenu6.css" type="text/css" id="IEMenu6CSS" />
<![endif]--> 
<!--[if gt IE 6]>
    <link runat="server" rel="stylesheet" href="~/Styles/BrowserSpecific/IEMenu7.css" type="text/css" id="IEMenu7CSS" />
<![endif]--> 
</head>
<body>
    <form id="form1" runat="server">
        <asp:PlaceHolder ID="skinPlaceHolder" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>

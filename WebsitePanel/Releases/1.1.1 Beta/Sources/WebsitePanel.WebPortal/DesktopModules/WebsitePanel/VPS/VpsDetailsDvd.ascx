<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsDvd.ascx.cs" Inherits="WebsitePanel.Portal.VPS.VpsDetailsDvd" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

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
				    <asp:Image ID="imgIcon" SkinID="DvdDrive48" runat="server" />
				    <wsp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="DVD" />
			    </div>
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_dvd" />
	
                        <wsp:SimpleMessageBox id="messageBox" runat="server" />
                        
			            <table style="margin: 50px 0px 50px 50px">
			                <tr>
			                    <td><asp:Localize ID="locDvdDrive" runat="server" meta:resourcekey="locDvdDrive" Text="DVD Drive:"></asp:Localize></td>
			                </tr>
			                <tr>
			                    <td>
			                        <asp:TextBox ID="txtInsertedDisk" runat="server" Width="400px"
			                            CssClass="NormalTextBox" ReadOnly="true"></asp:TextBox>
			                    </td>
			                </tr>
			                <tr>
			                    <td>
			                        <br />
			                        <br />
			                        <asp:Button ID="btnInsertDisk" runat="server" CausesValidation="false"
			                            Text="Insert Disk..." meta:resourcekey="btnInsertDisk" CssClass="Button1" 
                                        onclick="btnInsertDisk_Click" />
                                    <asp:Button ID="btnEjectDisk" runat="server" CausesValidation="false"
			                            Text="Eject" meta:resourcekey="btnEjectDisk" CssClass="Button1" 
                                        onclick="btnEjectDisk_Click" />
			                    </td>
			                </tr>
			            </table>
     
			    </div>
		    </div>
		    <div class="Right">
			    <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
		    </div>
	    </div>
    	
    </div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcAccountVLanAdd.ascx.cs"
    Inherits="WebsitePanel.Portal.VdcAccountVLanAdd" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<div id="VpsContainer">
    <div class="Module">
        <div class="Header">
            <wsp:breadcrumb id="breadcrumb" runat="server" />
        </div>
        <div class="Left">
            <wsp:menu id="menu" runat="server" selecteditem="vdc_account_vlan_network" />
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="imgIcon" SkinID="VLanNetwork" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Add VLan to user"></asp:Localize>
                </div>
                <div class="FormBody">
                    <table cellspacing="0" cellpadding="2" width="100%">
                        <tr>
                            <td class="SubHead" style="width: 150px;">
                                <asp:Label ID="lblUsername2" runat="server" meta:resourcekey="lblUsername" Text="Username:"></asp:Label>
                            </td>
                            <td class="Huge">
                                <asp:Literal ID="lblUsername" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top">
                                <asp:Label ID="lblVLanID" runat="server" meta:resourcekey="lblVLanID" Text="VLan:" />
                            </td>
                            <td>
                                <asp:TextBox ID="tbVLanID" runat="server" CssClass="NormalTextBox" />
                                <asp:RequiredFieldValidator ID="VLanIDValidator" runat="server" ErrorMessage="*"
                                    Display="Dynamic" ControlToValidate="tbVLanID" />
								<asp:RangeValidator runat="server" ID="VLanIDValidatorRange" Display="Dynamic" 
									ControlToValidate="tbVLanID" meta:resourcekey="VLanIDValidatorRange" />
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead" valign="top">
                                <asp:Label ID="lblComment" runat="server" meta:resourcekey="lblComment" Text="Comment:" />
                            </td>
                            <td class="NormalBold">
                                <asp:TextBox ID="tbComment" runat="server" TextMode="MultiLine" />
                            </td>
                        </tr>
                    </table>
                    <div class="FormFooter">
                        <asp:Button ID="btAddVLan" runat="server" meta:resourcekey="btAddVLan" CssClass="Button3"
                            Text="Add" OnClick="btAddVLan_Click" />
                        <asp:Button ID="btnCancel" CssClass="Button1" runat="server" meta:resourcekey="btnCancel"
                            CausesValidation="False" Text="Cancel" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
            <div class="Right">
                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
            </div>
        </div>
    </div>
</div>

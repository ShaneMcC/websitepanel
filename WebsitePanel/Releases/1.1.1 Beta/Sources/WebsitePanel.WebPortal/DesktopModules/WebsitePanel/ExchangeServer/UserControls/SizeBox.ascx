<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SizeBox.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.UserControls.SizeBox" %>
<asp:TextBox ID="txtValue" runat="server" CssClass="TextBox100" MaxLength="15"></asp:TextBox>
<asp:Localize ID="locKB" runat="server" meta:resourcekey="locKB" Text="KB"></asp:Localize>

<ajaxtoolkit:ValidatorCalloutExtender ID="_ValidatorCalloutExtender" runat="server"
                            TargetControlID="valRequireNumber"
                             HighlightCssClass="validatorCalloutHighlight" Width="125px">
                       </ajaxtoolkit:ValidatorCalloutExtender>

<ajaxtoolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server"
                            TargetControlID="valRequireCorrectNumber"
                             HighlightCssClass="validatorCalloutHighlight" Width="200px">
                       </ajaxtoolkit:ValidatorCalloutExtender>


<asp:RequiredFieldValidator ID="valRequireNumber" runat="server" meta:resourcekey="valRequireNumber" Enabled="false"
    ErrorMessage="Please enter value" ControlToValidate="txtValue" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="valRequireCorrectNumber" runat="server" meta:resourcekey="valRequireCorrectNumber"
	ErrorMessage="Enter correct number" ControlToValidate="txtValue" Display="None" ValidationExpression="[0-9]{0,15}" SetFocusOnError="True"></asp:RegularExpressionValidator>

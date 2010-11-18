<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebApplicationGallery.ascx.cs" Inherits="WebsitePanel.Portal.WebApplicationGallery" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="uc1" %>
<%@ Import Namespace="WebsitePanel.Portal" %>

<div class="FormButtonsBar">
    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="NormalTextBox"
        DataValueField="Id" DataTextField="Name" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
    </asp:DropDownList>
</div>

<uc1:SimpleMessageBox ID="messageBox" runat="server" />

<asp:GridView id="gvApplications" runat="server" AutoGenerateColumns="False" AllowPaging="true" 
	ShowHeader="false" CssSelectorClass="LightGridView" EmptyDataText="gvApplications" OnRowCommand="gvApplications_RowCommand" 
	OnPageIndexChanging="gvApplications_PageIndexChanging">
	<Columns>
		<asp:TemplateField HeaderText="gvApplicationsApplication">
		    <ItemStyle HorizontalAlign="Center" />
			<ItemTemplate>
				<div style="text-align:center;">
					<asp:hyperlink NavigateUrl='<%# EditUrl("ApplicationID", Eval("Id").ToString(), "edit", "SpaceID=" + PanelSecurity.PackageId.ToString()) %>'
							runat="server" ID="Hyperlink3" ToolTip='<%# Eval("Title") %>'>
						<asp:Image runat=server ID="Image1" Width="120" Height="120"
                            ImageUrl='<%# "~/DesktopModules/WebsitePanel/ResizeImage.ashx?width=120&height=120&url=" + Server.UrlEncode((string)Eval("IconUrl"))  %>'
							AlternateText='<%# Eval("Title") %>'>
						</asp:Image>
					</asp:hyperlink>
				</div>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<div class="MediumBold" style="padding:4px;">
					<asp:hyperlink CssClass=MediumBold NavigateUrl='<%# EditUrl("ApplicationID", Eval("Id").ToString(), "edit", "SpaceID=" + PanelSecurity.PackageId.ToString()) %>'
					    runat="server" ID="lnkAppDetails" ToolTip='<%# Eval("Title") %>'>
						<%# Eval("Title")%>
					</asp:hyperlink>
				</div>
				<div class="Normal" style="padding:4px;">
					<%# Eval("Summary") %>
				</div>				
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="gvApplicationsApplication">
		    <ItemStyle HorizontalAlign="Center" />
			<ItemTemplate>
			    <asp:Button ID="btnInstall" runat="server"
			        Text='<%# GetLocalizedString("btnInstall.Text") %>' CssClass="Button1"
			        CommandArgument='<%# Eval("Id") %>'
			        CommandName="Install" />
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
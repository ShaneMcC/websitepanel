<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditEventViewer.ascx.cs" Inherits="WebsitePanel.Portal.ServersEditEventViewer" %>
<%@ Register Src="UserControls/Comments.ascx" TagName="Comments" TagPrefix="uc4" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<%@ Register Src="ServerHeaderControl.ascx" TagName="ServerHeaderControl" TagPrefix="uc1" %>
<uc1:ServerHeaderControl id="ServerHeaderControl1" runat="server">
</uc1:ServerHeaderControl>

<asp:UpdatePanel runat="server" ID="updatePanelUsers">
    <ContentTemplate>
        <div class="FormButtonsBar">
            <div class="Left">
                <asp:DropDownList ID="ddlLogNames" runat="server"
                    CssClass="NormalTextBox" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="Right">
                <asp:Button ID="btnClearLog" runat="server" Text="Clear Log" meta:resourcekey="btnClearLog" CssClass="Button1" OnClick="btnClearLog_Click" />
            </div>
        </div>
        <asp:GridView ID="gvEntries" runat="server" AutoGenerateColumns="False"
            EmptyDataText="gvEntries" AllowPaging="true" DataSourceID="odsLogEntries" PageSize="20"
            CssSelectorClass="NormalGridView" EnableViewState="false">
            <Columns>
                <asp:TemplateField HeaderText="gvEntriesType">
                    <ItemTemplate>
                        <asp:Image ID="imgType" runat="server"
                            ImageUrl='<%# PortalUtils.GetThemedImage(Eval("EntryType").ToString() + "_icon_small.gif") %>'
                            ImageAlign="AbsMiddle" />
                        <%# GetLocalizedString("EventType." + Eval("EntryType").ToString()) %>
                    </ItemTemplate>
                    <ItemStyle Wrap="False" />
                    <HeaderStyle Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="Created" HeaderText="gvEntriesCreated" >
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="Source" HeaderText="gvEntriesSource" >
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="Category" HeaderText="gvEntriesCategory" >
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="EventID" HeaderText="gvEntriesEvent" >
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="UserName" HeaderText="gvEntriesUserName" >
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="MachineName" HeaderText="gvEntriesMachineName" >
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
	                     <uc4:Comments id="Comments1" runat="server" Comments='<%# Eval("Message") %>'>
                         </uc4:Comments>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
        <asp:ObjectDataSource ID="odsLogEntries" runat="server"
                SelectMethod="GetEventLogEntriesPaged"
                SelectCountMethod="GetEventLogEntriesPagedCount"
                TypeName="WebsitePanel.Portal.ServersHelper"
                OnSelected="odsLogEntries_Selected" EnablePaging="true">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlLogNames" Name="logName" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        
    </ContentTemplate>
</asp:UpdatePanel>


<div class="FormFooter">
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel" CssClass="Button1"
        CausesValidation="False" OnClick="btnCancel_Click" />
</div>
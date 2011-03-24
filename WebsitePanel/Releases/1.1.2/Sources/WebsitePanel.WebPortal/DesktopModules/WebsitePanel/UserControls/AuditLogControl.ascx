<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuditLogControl.ascx.cs" Inherits="WebsitePanel.Portal.UserControls.AuditLogControl" %>
<%@ Register Src="PopupHeader.ascx" TagName="PopupHeader" TagPrefix="wsp" %>

<table cellpadding="5" width="100%">
    <tr>
        <td valign="top">
        <asp:Calendar ID="calPeriod" runat="server"
            SelectionMode="DayWeekMonth"
            DayNameFormat="Shortest"
            Height="180px" Width="200px" OnSelectionChanged="calPeriod_SelectionChanged">
        </asp:Calendar></td>
        <td width="100%" valign="top" align="left">
            <table width="300">
                <tr>
                    <td class="Big" colspan="2">
                        <asp:Literal ID="litPeriod" runat="server"></asp:Literal>
                        <asp:Literal ID="litStartDate" runat="server" Visible="false"></asp:Literal>
                        <asp:Literal ID="litEndDate" runat="server" Visible="false"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" nowrap>
                        <asp:Label id="lblSeverity" runat="server" meta:resourcekey="lblSeverity" Text="Severity"></asp:Label>
                    </td>
                    <td class="Normal">
                        <asp:DropDownList ID="ddlSeverity" runat="server" CssClass="NormalTextBox" resourcekey="ddlSeverity" AutoPostBack="true">
                            <asp:ListItem Value="-1">All</asp:ListItem>
                            <asp:ListItem Value="0">Information</asp:ListItem>
                            <asp:ListItem Value="1">Warning</asp:ListItem>
                            <asp:ListItem Value="2">Error</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="SourceRow" runat="server">
                    <td class="SubHead" nowrap>
                        <asp:Label id="lblSource" runat="server" meta:resourcekey="lblSource" Text="Source"></asp:Label>
                    </td>
                    <td class="Normal">
                        <asp:DropDownList ID="ddlSource" runat="server" CssClass="NormalTextBox"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlSource_SelectedIndexChanged">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="SubHead" nowrap style="height: 24px">
                        <asp:Label id="lblTask" runat="server" meta:resourcekey="lblTask" Text="Task"></asp:Label>
                    </td>
                    <td class="Normal" style="height: 24px">
                        <asp:DropDownList ID="ddlTask" runat="server" CssClass="NormalTextBox" AutoPostBack="true">
                        </asp:DropDownList></td>
                </tr>
                <tr id="ItemNameRow" runat="server">
                    <td class="SubHead" nowrap style="height: 24px">
                        <asp:Label id="lblItemName" runat="server" meta:resourcekey="lblItemName" Text="Item Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemName" runat="server" CssClass="NormalTextBox"></asp:TextBox>
                    </td>
                </tr>
                <tr id="FilterButtonsRow" runat="server">
                    <td colspan="2">
                        <asp:Button ID="btnDisplay" runat="server" Text="Display Records" meta:resourcekey="btnDisplay"
                            CssClass="Button2" OnClick="btnDisplay_Click" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>



<div class="FormButtonsBar">
	<div class="FormButtonsBarCleanLeft">
	    <asp:Button ID="btnExportLog" runat="server" Text="Export Log" meta:resourcekey="btnExportLog"
		    CssClass="Button2" OnClick="btnExportLog_Click" />
		<asp:Button ID="btnClearLog" runat="server" Text="Clear Log" meta:resourcekey="btnClearLog"
			CssClass="Button2" OnClick="btnClearLog_Click" OnClientClick="return confirm('Clear Log?');" />
	</div>
	<div class="FormButtonsBarCleanRight">
		<asp:UpdateProgress ID="recordsProgress" runat="server"
			AssociatedUpdatePanelID="updatePanelLog" DynamicLayout="false">
			<ProgressTemplate>
				<asp:Image ID="imgSep" runat="server" SkinID="AjaxIndicator" vspace="4" />
			</ProgressTemplate>
		</asp:UpdateProgress>
	</div>
</div>


<asp:UpdatePanel runat="server" ID="updatePanelLog" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
<div style="clear: both;">

<asp:GridView ID="gvLog" runat="server" AutoGenerateColumns="False"
    EmptyDataText="gvLog" CssSelectorClass="NormalGridView" EnableViewState="false"
    AllowSorting="True" DataSourceID="odsLog" AllowPaging="True"
    DataKeyNames="RecordID" OnRowCommand="gvLog_RowCommand">
    <Columns>
        <asp:TemplateField SortExpression="SeverityID" HeaderText="gvLogSeverity">
            <ItemStyle Wrap="False" />
            <ItemTemplate>
                <asp:Image ID="imgIcon" runat="server" hspace="2" ImageUrl='<%# GetIconUrl((int)Eval("SeverityID")) %>' ImageAlign="AbsMiddle" />
	            <%# GetAuditLogRecordSeverityName((int)Eval("SeverityID")) %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="StartDate" HeaderText="gvLogStartDate">
            <ItemStyle Wrap="False" />
            <ItemTemplate>
	            <%# ((DateTime)Eval("StartDate")).ToShortDateString() %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvLogStartTime">
            <ItemStyle Wrap="False" />
            <ItemTemplate>
	            <%# ((DateTime)Eval("StartDate")).ToShortTimeString() %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvLogFinishTime">
            <ItemStyle Wrap="False" />
            <ItemTemplate>
	            <%# ((DateTime)Eval("FinishDate")).ToShortTimeString() %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="SourceName" HeaderText="gvLogSource">
            <ItemStyle Wrap="False" />
            <ItemTemplate>
		         <%# GetAuditLogSourceName((string)Eval("SourceName")) %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="TaskName" HeaderText="gvLogTask">
            <ItemStyle Width="100%" />
            <ItemTemplate>
                <asp:LinkButton ID="cmdShowLog" runat="server"
					CommandName="ViewDetails" CommandArgument='<%# Eval("RecordID") %>'>
		            <%# GetAuditLogTaskName((string)Eval("SourceName"), (string)Eval("TaskName"))%>
		        </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="ItemName" HeaderText="gvLogItemName">
            <ItemStyle Wrap="false" />
            <ItemTemplate>
		         <%# Eval("ItemName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Username" HeaderText="gvLogUser">
            <ItemStyle Wrap="False" />
            <ItemTemplate>
		         <asp:HyperLink ID="lnkUser" runat="server" NavigateUrl='<%# NavigateURL("UserID", Eval("EffectiveUserID").ToString())%>'>
		            <%# Eval("Username")%>
		         </asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsLog" runat="server" EnablePaging="True" SelectCountMethod="GetAuditLogRecordsPagedCount"
    SelectMethod="GetAuditLogRecordsPaged" SortParameterName="sortColumn" TypeName="WebsitePanel.Portal.AuditLogHelper" OnSelected="odsLog_Selected">
    <SelectParameters>
        <asp:ControlParameter Name="sStartDate" ControlID="litStartDate" PropertyName="Text" />
        <asp:ControlParameter Name="sEndDate" ControlID="litEndDate" PropertyName="Text" />
        <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" Type="int32" DefaultValue="0" />
        <asp:QueryStringParameter Name="itemId" QueryStringField="ItemId" Type="int32" DefaultValue="0" />
        <asp:ControlParameter Name="itemName" ControlID="txtItemName" PropertyName="Text" />
        <asp:ControlParameter Name="severityId" ControlID="ddlSeverity" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter Name="sourceName" ControlID="ddlSource" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="taskName" ControlID="ddlTask" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:ObjectDataSource>
</div>

<asp:Panel ID="pnlTaskDetails" runat="server" CssClass="PopupContainer" style="display:none;">

	<table class="Popup-Header" cellpadding="0" cellspacing="0">
		<tr>
			<td class="Popup-HeaderLeft"></td>
			<td class="Popup-HeaderTitle">
				<asp:Localize ID="TaskDetailsHeader" runat="server" Text="Task Details"
				    meta:resourcekey="TaskDetailsHeader"></asp:Localize>
			</td>
			<td class="Popup-HeaderRight"></td>
		</tr>
	</table>
	
	<div class="Popup-Content">

        <table cellpadding="5">
            <tr>
                <td valign="top">
                    <table cellpadding="3">
                        <tr>
                            <td class="SubHead">
                                <asp:Label ID="lblSourceName" runat="server" meta:resourcekey="lblSourceName" Text="Source:"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:Literal ID="litSourceName" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead">
                                <asp:Label ID="lblTaskName" runat="server" meta:resourcekey="lblTaskName" Text="Task Name:"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:Literal ID="litTaskName" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead">
                                <asp:Label ID="lblItemName1" runat="server" meta:resourcekey="lblItemName1" Text="Item Name:"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:Literal ID="litItemName" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead">
                                <asp:Label ID="lblRecordUser" runat="server" meta:resourcekey="lblRecordUser" Text="User:"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:Literal ID="litUsername" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td valign="top">
                    <table cellpadding="3">
                        <tr>
                            <td class="SubHead">
                                <asp:Label ID="lblStarted" runat="server" meta:resourcekey="lblStarted" Text="Started:"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:Literal ID="litStarted" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead">
                                <asp:Label ID="lblFinished" runat="server" meta:resourcekey="lblFinished" Text="Finished:"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:Literal ID="litFinished" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead">
                                <asp:Label ID="lblDuration" runat="server" meta:resourcekey="lblDuration" Text="Duration:"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:Literal ID="litDuration" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="SubHead">
                                <asp:Label ID="lblResultSeverity" runat="server" meta:resourcekey="lblResultSeverity" Text="Severity:"></asp:Label>
                            </td>
                            <td class="Normal">
                                <asp:Literal ID="litSeverity" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
               <td colspan="3" class="SubHead">
                <asp:Label ID="lblExecutionLog" runat="server" meta:resourcekey="lblExecutionLog" Text="Execution Log:"></asp:Label>
               </td> 
            </tr>
            <tr>
                <td colspan="3" class="Normal">
                    <asp:Panel ID="pnlExecutionLog" runat="server" style="border: solid 1px #e0e0e0; width:430px; height: 175px; overflow: auto; white-space: nowrap; background-color: #ffffff;padding:3px;">
                        <asp:Literal ID="litLog" runat="server"></asp:Literal>
                    </asp:Panel>
                </td>
            </tr>
        </table>
            
	    <div class="FormFooter">
	        <asp:Button ID="btnCloseTaskDetails" runat="server" Text="Close" CssClass="Button1" meta:resourcekey="btnCloseTaskDetails" />
	    </div>
	    
    </div>
</asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="modalTaskDetailsProperties" runat="server"
    BackgroundCssClass="modalBackground"
    TargetControlID="btnShowTaskDetails"
    PopupControlID="pnlTaskDetails"
    OkControlID="btnCloseTaskDetails" />
<asp:Button ID="btnShowTaskDetails" runat="server" Text="11" style="display:none;" />


</ContentTemplate>
</asp:UpdatePanel>
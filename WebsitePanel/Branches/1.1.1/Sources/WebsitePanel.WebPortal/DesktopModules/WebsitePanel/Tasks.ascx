<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tasks.ascx.cs" Inherits="WebsitePanel.Portal.Tasks" %>
<asp:Timer runat="server" Interval="5000" ID="tasksTimer" />
<asp:UpdatePanel runat="server" ID="tasksUpdatePanel" UpdateMode="Conditional">
  <Triggers>
    <asp:AsyncPostBackTrigger ControlID="tasksTimer" EventName="Tick" />
  </Triggers>
  <ContentTemplate>

<asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False"
    EmptyDataText="gvTasks" CssSelectorClass="NormalGridView" EnableViewState="false"
    DataSourceID="odsTasks" OnRowDataBound="gvTasks_RowDataBound" OnRowCommand="gvTasks_RowCommand">
    <Columns>
        <asp:TemplateField HeaderText="gvTasksName">
            <ItemStyle Width="40%"></ItemStyle>
            <ItemTemplate>
	            <asp:hyperlink id="lnkTaskName" runat="server">
	            </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ItemName" HeaderText="gvTasksItemName"></asp:BoundField>
        <asp:BoundField DataField="StartDate" HeaderText="gvTasksStarted"></asp:BoundField>
		<asp:TemplateField HeaderText="gvTasksDuration">
			<ItemTemplate>
			    <asp:Literal ID="litTaskDuration" runat="server"></asp:Literal>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="gvTasksProgress">
			<ItemTemplate>
                <div class="ProgressBarContainer">
                    <asp:Panel id="pnlProgressIndicator" runat="server" CssClass="ProgressBarIndicator"></asp:Panel>
                </div>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="gvTasksActions">
			<ItemTemplate>
			    <asp:LinkButton ID="cmdStop" runat="server" CommandName="stop"
			        CausesValidation="false" Text="Stop" OnClientClick="return confirm('Do you really want to terminate this task?');"></asp:LinkButton>
			</ItemTemplate>
		</asp:TemplateField>
    </Columns>
</asp:GridView>

<asp:ObjectDataSource ID="odsTasks" runat="server"
    SelectMethod="GetRunningTasks"
    TypeName="WebsitePanel.Portal.TasksHelper"
    OnSelected="odsTasks_Selected">
    <SelectParameters>
    </SelectParameters>
</asp:ObjectDataSource>

  </ContentTemplate>
</asp:UpdatePanel>

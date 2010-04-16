<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditItemComments.ascx.cs" Inherits="WebsitePanel.Portal.EditItemComments" %>
<%@ Register TagPrefix="uc2" TagName="UserDetails" Src="UserDetails.ascx"  %>

<asp:UpdatePanel runat="server" ID="commentsUpdatePanel" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
    
    <asp:GridView ID="gvComments" runat="server" AutoGenerateColumns="False"
    ShowHeader="false" DataKeyNames="CommentID"
    CssSelectorClass="NormalGridView"
    EmptyDataText="gvComments" OnRowDeleting="gvComments_RowDeleting">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div style="float:right">
                        <asp:ImageButton ID="btnDelete" SkinID="DeleteSmall" CommandName="delete" runat="server" Text="Delete" meta:resourcekey="btnDelete" />
                    </div>
                    <div class="Small">                                
                         <uc2:UserDetails ID="userDetails" runat="server"
                            UserID='<%# Eval("UserID") %>'
                            Username='<%# Eval("Username") %>' />
                            
                            <asp:Label ID="lblCommented" runat="server" meta:resourcekey="lblCommented"></asp:Label>
                            
                            <%# Eval("CreatedDate") %>
                    </div>
                    <div class="FormRow">
                        <%# WrapComment((string)Eval("CommentText")) %>
                    </div>
                    
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Panel id="AddCommentPanel" runat="server" style="padding: 5px 5px 0px 5px;">
        <asp:TextBox ID="txtComments" runat="server" CssClass="LogArea" Rows="3"
            TextMode="MultiLine" Width="520px"></asp:TextBox>
            <div class="FormRow">
                <asp:Button ID="btnAdd" runat="server" Text="Add" meta:resourcekey="btnAdd" CssClass="Button3" OnClick="btnAdd_Click" ValidationGroup="AddItemComment" />
                 <asp:RequiredFieldValidator ID="valRequireComment" runat="server" ControlToValidate="txtComments"
                    ErrorMessage="*" ValidationGroup="AddItemComment" meta:resourcekey="valRequireComment"></asp:RequiredFieldValidator>
            </div>
    </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>

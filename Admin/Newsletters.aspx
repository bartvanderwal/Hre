<%@ Page Title="" Language="C#" MasterPageFile="~/HRE.Master" AutoEventWireup="True" Inherits="HRE.Admin.Newsletters" Codebehind="Newsletters.aspx.cs" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>Admin - Beheer nieuwsbrieven</title>
</asp:Content>


<asp:Content ContentPlaceHolderID="main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Always" EnableViewState="true" >
        <ContentTemplate>
            <h1>Nieuwsbrieven</h1>
            <a href="/Admin/Dashboard.aspx" class="back" runat="server" tooltip="Terug naar dashboard" alt="back">Terug</a>
            <asp:Label ID="MessageText" runat="server"></asp:Label>
            <br />
            <asp:Button ID="btnCreateNew" runat="server" Text="Nieuwe nieuwsbrief" ToolTip="Maak een nieuwe e-mail nieuwsbrief" OnClick="btnCreateNew_Click" />
  	        <asp:Button ID="btnReload" runat="server" Text="Update" Tooltip="Ververs de lijst" OnClick="reload_Click" />
            <br clear="all" /><br />
            <div class="horizontalOverflow smalltext">
        
                <asp:GridView ID="GVNewsletter" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <EmptyDataTemplate>
                        <div class="empty">Er zijn nog geen nieuwsbrieven. Maak een nieuwe aan!
                            <br /><br /><br /><br />
                        </div>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="" InsertVisible="False" ReadOnly="True" Visible="true"/>
                        <asp:TemplateField HeaderText="Status" >
                            <ItemTemplate>
                                <div class='newsletter-is-sent-<%# Eval("IsSent") %>' title='<%# (bool) Eval("IsSent") ? "Verstuurd" : "Draft" %>'>&nbsp;<br />&nbsp;</div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Titel">
                            <ItemTemplate>
                                <div title='<%# Eval("Item1") %>'>
                                    <a href="newsletter.aspx?Id=<%# Eval("ID") %>"> <%# string.IsNullOrEmpty((String) Eval("Title")) ? "[Geen titel]" : Eval("Title") %></a>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Aangemaakt">
                            <ItemTemplate><%# ((DateTime) Eval("DateCreated")).ToShortDateString() %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Verzonden">
                            <ItemTemplate><%# Eval("DateSent")!=null ? ((DateTime) Eval("DateSent")).ToShortDateString() : "-" %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SequenceNumber" HeaderText="" />
                        <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDeleteNewsletter" runat="server" OnCommand="CommandBtn_Click" CommandName="Delete" CommandArgument='<%# Eval("ID") %>'>
                                    <div class='delete-newsletter-<%# Eval("NotSent") %>' title='Verwijder nieuwsbrief <%# Eval("ID") %>'>
                                        &nbsp;<br />&nbsp;
                                    </div>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


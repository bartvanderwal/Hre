<%@ Page Title="" Language="C#" MasterPageFile="~/HRE.Master" AutoEventWireup="True" Inherits="HRE.NewsletterSubscription" Codebehind="NewsletterSubscription.aspx.cs" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="server" >
</asp:Content>

<asp:Content ID="main" ContentPlaceHolderID="main" Runat="server">
    <h1><asp:Label ID="lblSubscriptionHeader" runat="server" Text="Afmelden / aanmelden nieuwsbrief" /></h1>
    
    <div class="subscription-panel">
        <asp:Panel ID="pnlEditButtons" runat="server" Visible="false" CssClass="admin-section">
            <asp:Button ID="btnEditEmailAddress" runat="server" Text="Edit" OnClick="btnEditEmailAddress_Click" CausesValidation="false" />
            <asp:Button ID="btnCheckStatus" runat="server" Text="Check" OnClick="btnCheckStatus_Click"/>
            <div class="admin-section-comment">
                <a href="/Site/Admin/Dashboard.aspx" class="back" runat="server" tooltip="Terug naar dashboard" alt="back">Terug</a>
                <asp:literal runat="server" Text="(alleen beschikbaar voor beheerders)"/>
            </div>
        </asp:Panel>
        <br />

        <asp:Panel ID="pnlSubscriptionInput" runat="server">
            <asp:Label ID="lblUnsubscribe" runat="server" Text="E-mail adres"/>
            <asp:TextBox ID="tbEmail" runat="server" CssClass="inputclass" Enabled="false" Width="300px" />
            <br clear="all" />
            <asp:RequiredFieldValidator ID="valUnsubscribeEmailReq" runat="server" ControlToValidate="tbEmail" ErrorMessage="E-mail adres is verplicht" />
            <asp:Label ID="lblUnsubscribeFailed" runat="server" Text="Het e-mail adres is niet gevonden."  ForeColor="Red" Visible="false"/>
            <asp:Label ID="lblSubscribeFailed" runat="server" Text="Het e-mail adres is niet gevonden."  ForeColor="Red" Visible="false"/>
            <br clear="all"/><br />
            <asp:Button ID="btnUnsubscribe" runat="server" OnClick="btnUnsubscribe_Click" Text="Afmelden" />
            <asp:Button ID="btnSubscribe" runat="server" OnClick="btnSubscribe_Click" Text="Aanmelden" Visible="false"/>
        </asp:Panel>

        <asp:Panel ID="pnlUnsubscribeSucceeded" runat="server" Visible="false" CssClass="user-message">
            <asp:Label ID="lblUnsubscribeSucceeded" runat="server" Text="Het e-mail adres is afgemeld voor de periodieke nieuwsbrief!" />
        </asp:Panel>

        <asp:Panel ID="pnlSubscribeSucceeded" runat="server" Visible="false" CssClass="user-message">
            <asp:Label ID="lblSubscribeSucceeded" runat="server" Text="Het e-mail adres is aangemeld voor de periodieke nieuwsbrief!" />
        </asp:Panel>
    </div>

    <br /><br clear="all"/>

    <asp:Panel runat="server">
        <p><asp:Label runat="server" Text="Aanmelden of afmelden voor de karakterstructuren e-mail nieuwsbrief." /></p>
    </asp:Panel>

    <br /><br />

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/HRE.Master" AutoEventWireup="True" CodeBehind="Dashboard.aspx.cs" Inherits="HRE.Admin.Dashboard" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="/Site/Css/dashboard.css" />
</asp:Content>

<asp:Content ID="main" ContentPlaceHolderID="main" runat="server">
    <div id="Dashboard">
        <p id="DashboardFirstLine">Welkom op de dashboard pagina.</p>
        <asp:image AlternateText="Admin Dashboard" runat="server" ID="header" ImageUrl="/Site/Images/admin-header.png" /><br/>
        <div id="LeftDashboard">
            <ul>
                <li><a href="Newsletters.aspx">Nieuwsbrieven</a></li>
                <li><a href="/ScrapeNtb">Inschrijvingen</a></li>
                <li><a href="EmailAudits.aspx">E-mail Audit</a></li>
                <li><a href="/NewsletterSubscription.aspx">Nieuwsbrief subscribe/unsubscribe</a></li>
            </ul>
        </div>

        <div id="RightDashboard">
            <h1>Develop/Debug</h1>
            <asp:HyperLink ID="TestIdeal" runat="server" Text="Test IDEAL"/><br />
            <br/>Test mail functionaliteit
            <asp:Button ID="SendEmailButton" runat="server" Text="Stuur mail" OnClick="SendEmail_Click"/>
            <br/>Test bevestigingsmail bestelling
        </div>
    <//div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/HRE.Master" AutoEventWireup="True" CodeBehind="NotAuthorized.aspx.cs" Inherits="HRE.Site.NotAuthorized" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h1>Onvoldoende gebruikersrechten</h1>
    <p>De actieve gebruiker heeft onvoldoende rechten om de opgevraagde pagina te bekijken. Log eerst in als admin.</p>
    <p>Referred page: <%= Request.QueryString["RedirectedFrom"] ?? "None" %></p>
    <br/ ><b/>

    <%-- Existing User --%>
    <div id="LogOnExistingUser" runat="server">
        <table class="login">
            <tr><th colspan="2">Inloggen admin</th></tr>
            <tr>
                <td class="Label">E-mail adres</td>
                <td><asp:TextBox ID="UserNameExisting" runat="server" CssClass="Email" />
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                        ControlToValidate="UserNameExisting" ErrorMessage="Email adres is verplicht." 
                        ToolTip="Email adres is verplicht." Text="*" >*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator id="UserNameValidmail" runat="server"
                        ControlToValidate="UserNameExisting" ErrorMessage="Email adres is ongeldig" ToolTip="Email adres is ongeldig." 
                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" >*
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr><td class="Label">Wachtwoord</td>
                <td>
                    <asp:TextBox ID="PasswordExisting" CssClass="Password" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                        ControlToValidate="PasswordExisting" ErrorMessage="Wachtwoord is verplicht." 
                        ToolTip="Wachtwoord is verplicht" >*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="PasswordExisting" 
                        ErrorMessage="Incorrecte gebruikersnaam/wachtwoord combinatie." OnServerValidate="validateExistingPasswordCorrect">*</asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td class="error" colspan="2">
                    <asp:ValidationSummary ID="valSum" EnableClientScript="true" DisplayMode="List" runat="server"/>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Aanmelden" onclick="LogonUser_Click"/>
                </td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td class="Label"></td>
                <td><a href="/Site/ChangePassword.aspx?RedirectedFrom=NotAuthorized.aspx">Wachtwoord vergeten?</a></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
        </table>
    </div>

    <div id="ExistingUserLoggedInMessage" runat="server" visible="false">
        <div class="NotificationMessage">
            <h1>Ingelogd</h1>
                <p>Je bent nu ingelogd.</p>
        </div>
        Ga <a href="/Site/Admin/Dashboard.aspx">naar admin</a>
    </div>

</asp:Content>
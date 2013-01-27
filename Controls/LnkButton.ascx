<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LnkButton.ascx.cs" Inherits="HRE.Controls.LnkButton" %>

<asp:Panel ID="pnlLinkButton" runat="server" >
    <asp:Button ID="btnButton" runat="server" CausesValidation="true" CssClass="lnk-button">
    </asp:Button><span></span> 

    <%-- Span is necessary for the closing picture load bij css --%>
</asp:Panel>
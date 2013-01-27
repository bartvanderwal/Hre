<%@ Page Title="" Language="C#" MasterPageFile="~/HRE.Master" AutoEventWireup="true" CodeBehind="Newsletter.aspx.cs" Inherits="HRE.Admin.Newsletter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="toolkit" %>

<%@ Register Src="~/Controls/LnkButton.ascx" TagName="LnkButton" TagPrefix="ks" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <h1>Nieuwsbrief Wizard</h1>
	<div id="newsletter" class="newsletter">
        <asp:UpdatePanel ID="updPanel" runat="server">
            <ContentTemplate>
                <asp:Wizard ID="NewsletterWizard" runat="server" DisplaySideBar="false" DisplayCancelButton="false" EnableViewState="true">
                    <StartNavigationTemplate ></StartNavigationTemplate>
                    <StepNavigationTemplate></StepNavigationTemplate>
                    <FinishNavigationTemplate></FinishNavigationTemplate>
                    <WizardSteps>

                        <asp:WizardStep ID="stepEdit" Title="Opstellen" StepType="Start" runat="server">
                            <div class="divButtons">
                                <ks:LnkButton ID="btnBackToOverview" runat="server" OnClick="btnBackToOverview_Click" 
                                    Text="< Overzicht" EnableConfirmation="true" ConfirmText="Weet je zeker dat je terug wilt naar het overzichtscherm? Eventuele wijzigingen na de laatste keer opslaan gaan verloren.\n\n Druk [OK] om toch terug te keren\nDruk op [Annuleren] om hier te blijven en nog op te kunnen slaan."/>
                                <ks:LnkButton ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Opslaan" />
                                <ks:LnkButton ID="btnGoToPreview" runat="server" OnClick="btnGoToPreview_Click" Text="Toon preview >" />
                                <br clear="all"/>
                                <asp:Label ID="lbSavedMessage" runat="server" Visible="false" ForeColor="red" Font-Size="Small" />
                            </div>
                            <br clear="all"/>
                            <h2>Opstellen nieuwe nieuwsbrief</h2>
                            <p>Vul de titel van de nieuwsbrief en de drie nieuwsbrief items in. In stap 2 of 3 krijg je een preview van de nieuwsbrief en de geadresseerden en kun je deze versturen.</p>
                            <h2>Algemeen</h2>
                            <div class="label">Titel</div>
                            <asp:TextBox ID="tbTitle" runat="server"/>
                            <br />
                            <div class="label">Volgnummer</div>
                            <asp:Literal ID="litSequenceNumber" runat="server" />
                            <br /><br />
                            <h2>Item 1</h2>
                            <div class="label">Titel</div>
                            <asp:TextBox ID="tbItem1Title" runat="server"/>
                            <br />
                            <div class="label">Tekst</div>
                            <div class="html-entry">
                                <asp:TextBox ID="tbItem1Text" TextMode="MultiLine" runat="server" MaxLength="10000"/>
                                <toolkit:HtmlEditorExtender ID="HtmlEditorExtender1" TargetControlID="tbItem1Text" runat="server" >
                                    <Toolbar>
                                        <ajaxToolkit:Undo /><ajaxToolkit:Redo /><ajaxToolkit:Bold /><ajaxToolkit:Italic /><ajaxToolkit:StrikeThrough /><ajaxToolkit:Subscript /><ajaxToolkit:Superscript />
                                        <ajaxToolkit:InsertOrderedList /><ajaxToolkit:InsertUnorderedList /><ajaxToolkit:CreateLink /><ajaxToolkit:UnLink />
                                        <ajaxToolkit:RemoveFormat /><ajaxToolkit:Indent /><ajaxToolkit:Outdent /><ajaxToolkit:HorizontalSeparator />
                                    </Toolbar>
                                </toolkit:HtmlEditorExtender>
                            </div>

                            <div class="label">Afbeelding</div>
                            <asp:TextBox ID="tbItem1Picture" runat="server" />

                            <br />
                            <h2>Item 2</h2>
                            <div class="label">Titel</div>
                            <asp:TextBox ID="tbItem2Title" runat="server"/>
                            <br />
                            <span class="label">Tekst</span>
                            <div class="html-entry">
                                <asp:TextBox ID="tbItem2Text" TextMode="MultiLine" runat="server" MaxLength="10000"/>
                                <toolkit:HtmlEditorExtender ID="HtmlEditorExtender2" TargetControlID="tbItem2Text" runat="server">
                                    <Toolbar>
                                        <ajaxToolkit:Undo /><ajaxToolkit:Redo /><ajaxToolkit:Bold /><ajaxToolkit:Italic /><ajaxToolkit:StrikeThrough /><ajaxToolkit:Subscript /><ajaxToolkit:Superscript />
                                        <ajaxToolkit:InsertOrderedList /><ajaxToolkit:InsertUnorderedList /><ajaxToolkit:CreateLink /><ajaxToolkit:UnLink />
                                        <ajaxToolkit:RemoveFormat /><ajaxToolkit:Indent /><ajaxToolkit:Outdent /><ajaxToolkit:HorizontalSeparator />
                                    </Toolbar>
                                </toolkit:HtmlEditorExtender>
                            </div>

                            <div class="label">Afbeelding</div>
                            <asp:TextBox ID="tbItem2Picture" runat="server"/>

                            <h2>Item 3</h2>
                            <div class="label">Titel</div>
                            <asp:TextBox ID="tbItem3Title" runat="server"/>
                            <br />
                            <div class="label">Tekst</div>
                            <div class="html-entry">
                                <asp:TextBox ID="tbItem3Text" TextMode="MultiLine" runat="server" MaxLength="10000"/>
                                <toolkit:HtmlEditorExtender ID="HtmlEditorExtender3" TargetControlID="tbItem3Text" runat="server">
                                    <Toolbar>
                                        <ajaxToolkit:Undo /><ajaxToolkit:Redo /><ajaxToolkit:Bold /><ajaxToolkit:Italic /><ajaxToolkit:StrikeThrough /><ajaxToolkit:Subscript /><ajaxToolkit:Superscript />
                                        <ajaxToolkit:InsertOrderedList /><ajaxToolkit:InsertUnorderedList /><ajaxToolkit:CreateLink /><ajaxToolkit:UnLink />
                                        <ajaxToolkit:RemoveFormat /><ajaxToolkit:Indent /><ajaxToolkit:Outdent /><ajaxToolkit:HorizontalSeparator />
                                    </Toolbar>
                                </toolkit:HtmlEditorExtender>
                            </div>

                            <br />
                            <div class="label">Afbeelding</div>
                            <asp:TextBox ID="tbItem3Picture" runat="server"/>
                        </asp:WizardStep>

                        <asp:WizardStep ID="stepPreview" Title="Preview" StepType="Start" runat="server">
                            <div class="divButtons" id="divButtonsInPreviewStep" runat="server">
                                <ks:LnkButton ID="btnBackToEdit" runat="server" OnClick="btnBackToEdit_Click" Text="< Wijzig" />
                                <ks:LnkButton ID="btnBackToOverviewFromPreviewStep" runat="server" OnClick="btnBackToOverviewFromPreviewStep_Click" Text="< Overzicht" />
                                <ks:LnkButton ID="btnSendTestMail" runat="server" OnClick="btnSendTestMail_Click" Text="Test mail" />
                                <ks:LnkButton ID="btnSendNewsletterDirect" runat="server" OnClick="btnSendNewsletterDirect_Click" 
                                    Text="Verstuur" EnableConfirmation="true" ConfirmText="Weet je zeker dat je de nieuwsbrief wilt versturen naar alle adressen? Na versturen kun je deze niet meer aannpasen! Aangeraden wordt voor het versturen goed de preview te controleren, een test mail te sturen en te controleren en de lijst van geadresseerden te bekijken.\n\nDruk op [OK] om toch direct te versturen.\nDruk anders op [Annuleren] om hier te blijven."/>
                                <ks:LnkButton ID="btnGoToAdressees" runat="server" OnClick="btnGoToAdressees_Click" Text="Adressen >" />
                                <br clear="all"/>
                                <asp:Label ID="lbSendMessage" runat="server" Visible="false" ForeColor="red" Font-Size="Small" />
                            </div>
                            <br clear="all"/>
                            <div class="label">E-mail adres(sen) voor test</div>
                            <asp:TextBox ID="tbTestEmailAddress" runat="server"/>
                            <br />
                            <br />
                            <div class="label">Aangemaakt op</div>
                            <asp:Label ID="lbDateCreated" Enabled="false" runat="server" />
                            <br />
                            <div class="label">Verzonden op</div>
                            <asp:Label ID="lbDateSent" Enabled="false" runat="server" />
                            <br />
                            <h2>Voorbeeld</h2>
                            <p>Onderstaande voorbeeld nieuwsbrief is exclusief de 'unsubscribe' link, die in de verzonden e-mail onderaan staat.</p>
                            <div class="newsletter-preview">
                                <asp:Literal runat="server" ID="litNewsletterPreview" Mode="PassThrough" />
                            <br clear="all"/>
                            </div>
                        </asp:WizardStep>
                
                        <asp:WizardStep ID="stepAddressees" Title="Ontvangers" StepType="Step" runat="server">
                            <div class="divButtons">
                                <ks:LnkButton ID="btnBackToPreview" runat="server" OnClick="btnBackToPreview_Click" Text="< Terug" />
                                <ks:LnkButton ID="btnSendNewsletter" runat="server" OnClick="btnSendNewsletter_Click" Text="Verstuur >" 
                                        EnableConfirmation="True" ConfirmText="Weet je zeker dat je de nieuwsbrief wil versturen naar alle adressen? Na versturen kun je deze niet meer aannpasen!\n\nDruk op [OK] om toch direct te versturen.\nDruk op [Annuleren] om de nieuwsbrief nog verder aan te kunnen passen." />
                            </div>
                            <br clear="all"/>
                            <h2>Ontvangers</h2>
                            <p>Er zijn totaal <%= NrOfAddressees %> ontvangers.</p>
                            <asp:BulletedList runat="server" ID="blAddressees" BulletStyle="Numbered"/>
                        </asp:WizardStep>

                        <asp:WizardStep ID="stepConfirmation" Title="Bevestiging" StepType="Finish" runat="server">
                            <h2>Afgerond</h2>
                            <p>Je bent klaar! De nieuwsbrief is via e-mail verzonden naar alle geadresseerden.</p>
                            <p>Op het <a href="EmailAudits.aspx">mail audit scherm</a> kun je desgewenst controleren of het verzenden van de e-mail nieuwsbrieven goed is verlopen.</p>
                            <p>Controleer na het verzenden ook enkele keren het afzender account ('noreply@...') op foutmeldingen van ongeldige e-mail adressen of eventuele e-mails van providers die willen controleren dat de nieuwsbrief geen spam is (CAPTCHA mails).</p>
                        </asp:WizardStep>
                    </WizardSteps>
                </asp:Wizard>
            </ContentTemplate>
        </asp:UpdatePanel>
	</div>
</asp:Content>

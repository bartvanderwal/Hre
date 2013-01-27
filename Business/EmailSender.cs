using System;
using System.Text;
using System.Net.Mail;
using System.IO;
using HRE.Dal;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using HRE.Common;
using System.Collections.Specialized;


namespace HRE.Business {
    public class EmailSender {

        /// <summary>
        /// Sends an e-mail (and creates an e-mail audit record).
        /// </summary>
        public static void SendEmail(MailMessage message, EmailCategory? category, int? relatedEntityID) {
            EmailAuditDal emailAuditDal = new EmailAuditDal(message, category, relatedEntityID);

            SmtpClient smtpClient = new SmtpClient();
        
            // Try to send the mail.
            try {
                smtpClient.Send(message);
            
                // When the send succeeds set the status to send and set the time and date.
                emailAuditDal.EmailStatus = EmailStatus.Sent;
                emailAuditDal.DateSent = System.DateTime.Now;
            } catch (SmtpException e) {
                // When sending the mail fails set the status to sendError and set the status message.
                emailAuditDal.EmailStatus = EmailStatus.SendError;                          
                emailAuditDal.StatusMessage = e.Message;
            }
        
            emailAuditDal.Save();
        }


        /// <summary>
        /// Sends an e-mail (and creates an e-mail audit record).
        /// </summary>
        public static void SendEmail(string to, string from, string cc, string bcc, string subject, string body, bool isBodyHtml, 
                                        EmailCategory? category, int? relatedEntityID, Attachment attachment) {
            string fromAddress = from ?? "noreply@karakterstructuren.com";
            MailMessage message = new MailMessage(fromAddress, to, subject, body);
            message.To.Add(new MailAddress(fromAddress, "karakterstructuren.com"));
            if (cc!=null) {
                message.CC.Add(cc);
            }
            if (bcc!=null) {
                message.Bcc.Add(bcc);
            }
            message.IsBodyHtml = isBodyHtml;
            message.To.Add(to);
            if (attachment != null) {
                message.Attachments.Add(attachment);
            }

            SendEmail(message, category, relatedEntityID);
        }


        /// <summary>
        /// Sends a previously audited e-mail.
        /// Returns true if sending succeeds, false otherwise.
        /// </summary>
        public static bool ResendEmail(EmailAuditDal emailAuditDal) { 
            MailMessage message = emailAuditDal.CreateMailMessage();

            SmtpClient smtpClient = new SmtpClient();

            bool isMailSent = false;

            // Try to send the mail.
            try {
                emailAuditDal.DateSent = System.DateTime.Now;
                smtpClient.Send(message);
            
                // When the send succeeds set the status to send and set the time and date.
                emailAuditDal.EmailStatus = EmailStatus.Sent;
                emailAuditDal.StatusMessage = "Opnieuw verzonden na eerdere fout bij verzenden.";
                isMailSent = true;
            } catch (SmtpException e) {
                // When sending the mail fails set the status to sendError and set the status message.
                emailAuditDal.EmailStatus = EmailStatus.SendError;                          
                emailAuditDal.StatusMessage = e.Message;
            } finally {
                emailAuditDal.Save();
            }
            
            return isMailSent;
        }


        /// <summary>
        /// Create a mailmessage from a file with an e-mail template.
        /// </summary>
        /// <param name="emailPathName">The email path, excluding 1) the application path prefix and 2) the culture code + '.html' postfix.</param>
        /// <param name="replacements">A key-value collection of all the tokens to be replace in the e=mail template.</param>
        /// <param name="toAddress">The e-mail addres the mailmessage is addressed to.</param>
        /// <returns></returns>
        private static MailMessage DetermineLocalizedMailMessage(string emailPathName, ListDictionary replacements, string toAddress) {
            MailDefinition mailDefinition = new MailDefinition();
            string filePath = string.Format("{0}\\{1}.html", HttpContext.Current.Request.PhysicalApplicationPath,
                                                                emailPathName);
            mailDefinition.BodyFileName = Common.Util.RetrievePhysicalFilename(filePath);
            LiteralControl dummy = new LiteralControl();
            MailMessage message = mailDefinition.CreateMailMessage(toAddress, replacements, dummy);
            message.From = new MailAddress(HreSettings.ReplyToAddress, HreSettings.ReplyToAddress);
            return message;
        }

    }
}
using System.Net.Mail;

namespace HRE.Common {

    /// <summary>
    /// Utility for constructing, sending (and auditing) e-mails.
    /// </summary>
    public static class EmailSender {

        /*
        /// <summary>
        /// Sends an e-mail (and creates an e-mail audit record).
        /// </summary>
        /// <param name="message">The e-mail message. NB Set the to, from, subject, isHtml etc, before calling this function.</param>
        /// <param name="category">The EmailCategory the e-mail should be audited under, for instance 'NewsLetter'.</param>
        /// <param name="relatedEntityID">The primary key of the database entity the e-mail is related to. Optional. Can be used to add filter-functionality on the E-mail Audit screen.</param>
        public static void SendMail(MailMessage message, Enums.MailCategory? category, int? relatedEntityID) {

            EmailAudit mailAuditDal = new EmailAudit(message, category, relatedEntityID);

            SmtpClient clientcontact = new SmtpClient();

            // Try to send the mail.
            try {
                clientcontact.Send(message);

                // When the send succeeds set the status to send and set the time and date.
                mailAuditDal.MailStatus = Enums.MailStatus.Sent;
                mailAuditDal.DateSent = System.DateTime.Now;
            }
            catch (SmtpException e) {
                // When sending the mail fails set the status to sendError and set the status message.
                mailAuditDal.MailStatus = Enums.MailStatus.SendError;
                mailAuditDal.StatusMessage = e.Message;
            }

            mailAuditDal.Save();
        }
        */
    }
}

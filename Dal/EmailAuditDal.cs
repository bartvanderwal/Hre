using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using HRE.Business;
using HRE.Data;

namespace HRE.Dal {
    public class EmailAuditDal : ObjectDal {

        #region :: Members
        
        /// <summary>
        ///  Connection to the Database class.
        /// </summary>
        private emailaudit _emailAudit;
        
        #endregion :: Members

        #region :: Properties

        /// <summary>
        /// The unique identifier / primary key of the audited e-mail.
        /// </summary>
        public int ID {
            get { return _emailAudit.Id; }
        }
		
        /// <summary>
        /// The from addresses of the audited e-mail.
        /// </summary>
        public string FromAddress { 
            get { return _emailAudit.FromAddress; }
            set { _emailAudit.FromAddress = value; }
        }
		
        /// <summary>
        /// The to addresses of the audited e-mail.
        /// </summary>
		public string ToAddresses {
            get { return _emailAudit.ToAddresses; }
            set { _emailAudit.ToAddresses = value; }
        }
		
        /// <summary>
        /// The carbon copy addresses of the audited e-mail.
        /// </summary>
		public string CCAddresses {
            get { return _emailAudit.CCAddresses; }
            set { _emailAudit.CCAddresses = value; }
        }
		
        /// <summary>
        /// The blind carbon copy addresses of the audited e-mail.
        /// </summary>
		public string BccAddresses {
            get { return _emailAudit.BccAddresses; }
            set { _emailAudit.BccAddresses = value; }
        }
		

        /// <summary>
        /// The subject of the audited e-mail.
        /// </summary>
		public string Subject {
            get { return _emailAudit.Subject; }
            set { _emailAudit.Subject = value; }
        }
		
        /// <summary>
        /// The body of the audited e-mail.
        /// </summary>
		public string Body {
            get { return _emailAudit.Body; }
            set { _emailAudit.Body = value; }
        }
		
        /// <summary>
        /// The shortened body of the audited e-mail.
        /// </summary>
		public string BodyShort {
            get { return Body.Shorten(300); }
        }

        /// <summary>
        /// The status of the audited e-mail: unsent, sent or send error.
        /// </summary>
		public EmailStatus EmailStatus {
            get { return (EmailStatus) _emailAudit.EmailStatusId; }
            set { _emailAudit.EmailStatusId = (int) value; }
        }
		
        /// <summary>
        /// Optional text message when status is SendError.
        /// </summary>
		public string StatusMessage {
            get { return _emailAudit.StatusMessage; }
            set { _emailAudit.StatusMessage = value; }
        }

        /// <summary>
        /// The date(time) this e-mail audit was sent (last time e-mail succceeded send attempt).
        /// </summary>
        public DateTime? DateSent {
            get { return _emailAudit.DateSent; }
            set { _emailAudit.DateSent = value; }
        }

        /// <summary>
        /// The date(time) this e-mail audit was first created (first time e-mail send attempt).
        /// </summary>
        public DateTime DateCreated {
            get { return _emailAudit.DateCreated; }
            set { _emailAudit.DateCreated = value; }
        }

        /// <summary>
        ///  Is this an HTML (or plaintext) mail (body is HTML).
        /// </summary>
        public bool? IsHtml {
            get { return _emailAudit.isHtml; }
            set { _emailAudit.isHtml = value; }
        }


        /// <summary>
        /// The category of the mail audit.
        /// For instance 'Newsletter' for an audited newsletter e-mail.
        /// </summary>
        // TODO Hernoemen naar EmailCategory.
        public EmailCategory? EmailCategory {
            get { return (EmailCategory) _emailAudit.EmailCategoryId; }
            set { _emailAudit.EmailCategoryId = (int) value; }
        }

        /// <summary>
        /// The ID of the related entity.
        /// For instance the Newsletter ID for a mailaudit of mailCategory=Newsletter.
        /// </summary>
        public int AttachmentEntityId {
            get { return _emailAudit.AttachmentEntityId.Value; }
            set { _emailAudit.AttachmentEntityId = value; }
        }
        

        #endregion :: Properties
        
        #region :: Methods

        /// <summary>
        /// Constructor. Construct a mailaudit DAL object based on a mailAudit DB object.
        /// </summary>
        public EmailAuditDal(emailaudit emailAudit) {
            _emailAudit = emailAudit;
        }

        /// <summary>
        /// Constructor. Construct a mailaudit object based on a mailmessage object.
        /// </summary>
        /// <param name="mailMessage"></param>
        public EmailAuditDal(MailMessage mailMessage, EmailCategory? category, int? attachmentEntityID) {
            _emailAudit = new emailaudit();

            // Copy data from the MailMessage object to the MailAudit object.
            FromAddress = mailMessage.From.Address;
            ToAddresses = MailAddressesToString(mailMessage.To);
            CCAddresses = MailAddressesToString(mailMessage.CC);
            BccAddresses = MailAddressesToString(mailMessage.Bcc);
            Subject = mailMessage.Subject;
            Body = mailMessage.Body;
            IsHtml = mailMessage.IsBodyHtml;

            // Initialize the mailstatus to Unsent and set the creation date.
            EmailStatus = (int) EmailStatus.Unsent;
            DateCreated = System.DateTime.Now;
            
            // Set the mailcateogry and related entity (both can be null)
            if (category.HasValue) {
                EmailCategory = category.Value;
            }
            if (attachmentEntityID.HasValue) {
                AttachmentEntityId = attachmentEntityID.Value;
            }

        }


        /// <summary>
        /// Store the MailAudit object in the database.
        /// </summary>
        public void Save() {
            // Add entity if it has no primary key yet.
            if (ID == 0) {
                DB.AddToemailaudit(_emailAudit);
            }
  		    DB.SaveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        private static string MailAddressesToString(MailAddressCollection mailAddresses) {
            return String.Join(", ",mailAddresses.ToArray().Select(e => e.Address).ToArray());
        }


        /// <summary>
        /// Create a mail message object from this mail audit object.
        /// </summary>
        public MailMessage CreateMailMessage() {        
            MailMessage mailMessage = new MailMessage();
        
            // Copy data from the MailMessage object to the MailAudit object.
            mailMessage.From = new MailAddress(FromAddress);
            if (!string.IsNullOrEmpty(ToAddresses)) {
                mailMessage.To.Add(ToAddresses);
            }
            if (!string.IsNullOrEmpty(CCAddresses)) {
                mailMessage.CC.Add(CCAddresses);
            }
            if (!string.IsNullOrEmpty(BccAddresses)) {
                mailMessage.Bcc.Add(BccAddresses);
            }
            mailMessage.Subject = Subject;
            mailMessage.Body = Body;

            mailMessage.IsBodyHtml = IsHtml.HasValue ? false : IsHtml.Value;

            return mailMessage;
        }


        /// <summary>
        /// Return all the mail audits.
        /// </summary>
        public static List<EmailAuditDal> GetAll() {
             List<EmailAuditDal> mailAudits = new List<EmailAuditDal>();
             
             foreach (emailaudit emailAudit in DB.emailaudit.OrderByDescending(m => m.DateCreated)) {
                mailAudits.Add(new EmailAuditDal(emailAudit));
             }

             return mailAudits;
        }


        /// <summary>
        /// Return all the mail audits of mails that don't have status 'Sent' yet.
        /// </summary>
        public static List<EmailAuditDal> GetNotSent() {
             List<EmailAuditDal> emailAudits = new List<EmailAuditDal>();
             
             foreach (emailaudit emailAudit in DB.emailaudit.Where(m => m.EmailStatusId != (int) EmailStatus.Sent).OrderByDescending(m => m.DateCreated)) {
                emailAudits.Add(new EmailAuditDal(emailAudit));
             }

             return emailAudits;
        }


        /// <summary>
        /// Try to send all mail audits that were not yet sent succesfully before.
        /// Returns the number of succesfully sent e-mails.
        /// </summary>
        public static int ResentUnsent() {
             List<EmailAuditDal> emailAudits = new List<EmailAuditDal>();
             
             int nrOfSentMails = 0;

             var emailsNotSend = GetNotSent();
             
             foreach (EmailAuditDal emailAuditDal in emailsNotSend) {
                // Resend the mail and add one to nr of sent mails if it succeeds.
                if (EmailSender.ResendEmail(emailAuditDal)) {
                    nrOfSentMails++;
                }
             }

             return nrOfSentMails;
        }

        #endregion :: Methods
    }
}

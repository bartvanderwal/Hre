using HRE.Models;
using System;
using System.Linq;
using System.Net.Mail;

namespace MeaMedicaMVC.Common {

    public class EmailAudit : BaseModel {

        private MailAudit _mailAudit;

        #region :: Properties

        /// <summary>
        /// The unique identifier / primary key of the audited e-mail.
        /// </summary>
        // OLD: this property had an override keyword, but not existent in base class, so far
        public int ID {
            get { return _mailAudit.PK_MailAuditID; }
        }

        /// <summary>
        /// The from addresses of the audited e-mail.
        /// </summary>
        public string FromAddress {
            get { return _mailAudit.FromAddress; }
            set { _mailAudit.FromAddress = value; }
        }

        /// <summary>
        /// The to addresses of the audited e-mail.
        /// </summary>
        public string ToAddresses {
            get { return _mailAudit.ToAddresses; }
            set { _mailAudit.ToAddresses = value; }
        }

        /// <summary>
        /// The carbon copy addresses of the audited e-mail.
        /// </summary>
        public string CCAddresses {
            get { return _mailAudit.CCAddresses; }
            set { _mailAudit.CCAddresses = value; }
        }

        /// <summary>
        /// The blind carbon copy addresses of the audited e-mail.
        /// </summary>
        public string BccAddresses {
            get { return _mailAudit.BccAddresses; }
            set { _mailAudit.BccAddresses = value; }
        }

        /// <summary>
        /// The subject of the audited e-mail.
        /// </summary>
        public string Subject {
            get { return _mailAudit.Subject; }
            set { _mailAudit.Subject = value; }
        }

        /// <summary>
        /// The body of the audited e-mail.
        /// </summary>
        public string Body {
            get { return _mailAudit.Body; }
            set { _mailAudit.Body = value; }
        }

        /// <summary>
        /// The shortened body of the audited e-mail.
        /// </summary>
        public string BodyShort {
            get { return Body.Shorten(); }
        }

        /// <summary>
        /// The status of the audited e-mail: unsent, sent or send error.
        /// </summary>
        public Enums.MailStatus MailStatus {
            get { return (Enums.MailStatus)_mailAudit.MailStatus; }
            set { _mailAudit.MailStatus = (int)value; }
        }

        /// <summary>
        /// Optional text message when status is SendError.
        /// </summary>
        public string StatusMessage {
            get { return _mailAudit.StatusMessage; }
            set { _mailAudit.StatusMessage = value; }
        }

        /// <summary>
        /// The date(time) this e-mail audit was sent (last time e-mail succceeded send attempt).
        /// </summary>
        public DateTime? DateSent {
            get { return _mailAudit.DateSent; }
            set { _mailAudit.DateSent = value; }
        }

        /// <summary>
        /// The date(time) this e-mail audit was first created (first time e-mail send attempt).
        /// </summary>
        public DateTime DateCreated {
            get { return _mailAudit.DateCreated; }
            set { _mailAudit.DateCreated = value; }
        }

        /// <summary>
        ///  Is this an HTML (or plaintext) mail (body is HTML).
        /// </summary>
        public bool IsHtmlMail {
            get { return _mailAudit.isHtmlMail; }
            set { _mailAudit.isHtmlMail = value; }
        }

        /// <summary>
        /// The category of the mail audit.
        /// For instance 'Newsletter' for an audited newsletter e-mail.
        /// </summary>
        public Enums.MailCategory? MailCategory {
            get { return (Enums.MailCategory)_mailAudit.mailCategoryID; }
            set { _mailAudit.mailCategoryID = (int)value; }
        }

        /// <summary>
        /// The ID of the related entity.
        /// For instance the Newsletter ID for a mailaudit of mailCategory=Newsletter.
        /// </summary>
        public int RelatedEntityID {
            get { return _mailAudit.relatedEntityID.Value; }
            set { _mailAudit.relatedEntityID = value; }
        }

        #endregion :: Properties

        /// <summary>
        /// Constructor. Construct a mailaudit object based on a mailmessage object.
        /// </summary>
        /// <param name="mailMessage"></param>
        public EmailAudit(MailMessage mailMessage, Enums.MailCategory? category, int? relatedEntityID) {
            _mailAudit = new MailAudit();

            // Copy data from the MailMessage object to the MailAudit object.
            _mailAudit.FromAddress = mailMessage.From.Address;
            _mailAudit.ToAddresses = MailAddressesToString(mailMessage.To);
            _mailAudit.CCAddresses = MailAddressesToString(mailMessage.CC);
            _mailAudit.BccAddresses = MailAddressesToString(mailMessage.Bcc);
            _mailAudit.Subject = mailMessage.Subject;
            _mailAudit.Body = mailMessage.Body;
            _mailAudit.isHtmlMail = mailMessage.IsBodyHtml;
            
            // Initialize the mailstatus to Unsent and set the creation date.
            _mailAudit.MailStatus = (int)Enums.MailStatus.Unsent;
            _mailAudit.DateCreated = System.DateTime.Now;
            
            // Set the mailcateogry and related entity (both can be null)
            if (category.HasValue) {
                _mailAudit.mailCategoryID = (int)category.Value;
            }
            if (relatedEntityID.HasValue) {
                _mailAudit.relatedEntityID = relatedEntityID.Value;
            }
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

            mailMessage.IsBodyHtml = IsHtmlMail;

            return mailMessage;
        }

        /// <summary>
        /// Store the MailAudit object in the database.
        /// </summary>
        public void Save() {
            // Add entity if it has no primary key yet.
            // OLD: review this validation (ObjectDal class)
            //if (!IsStoredInDB) {
            db.MailAudit.AddObject(_mailAudit);
            //OLD:db.AddToMailAudit(_mailAudit);
            //}
            db.SaveChanges();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private static string MailAddressesToString(MailAddressCollection mailAddresses) {
            return String.Join(", ", mailAddresses.ToArray().Select(e => e.Address).ToArray());
        }

    }
}
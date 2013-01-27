using System.Configuration;
using System.Net.Mail;
 
namespace HRE.Business {
    public class Email {
        public void Send(Contact contact) {
            MailMessage mail = new MailMessage(
                contact.From,
                Settings.ContactToEmail,
                contact.Subject,
                contact.Message);
 
            new SmtpClient().Send(mail);
        }
    }
}

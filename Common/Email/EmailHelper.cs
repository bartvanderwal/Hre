using HRE.Models;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mvc;

namespace HRE.Common {

    public class EmailHelper {

        /*
        public static void RegistrationConfirmationEmail(string userName, string ValidationKey) {
            CustomerModel customer = CustomerModelRepository.GetCustomerByUsername(userName);
            string name = customer.Addressment;
            string ActivateUrl = "?username=" + HttpUtility.UrlEncode(userName) + "&key=" + HttpUtility.UrlEncode(ValidationKey);

            ListDictionary replacements = new ListDictionary();
            replacements.Add("<%Name%>", name);
            replacements.Add("<%Username%>", userName); 
            replacements.Add("<%DomainBase%>", Util.DetermineDomainBaseWww(null));
            replacements.Add("<%ActivateUrl%>", ActivateUrl);

            string emailPathName = "Views\\Account\\Includes\\confirm-account.email";
            MailMessage mailMessage = DetermineLocalizedMailMessage(emailPathName, replacements, customer.Email);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = string.Format(@GlobalResources.Resource.RegistrationConfirmationMailSubject, userName);
            EmailSender.SendMail(mailMessage, Enums.MailCategory.SubscriptionConfirmation, customer.CustomerID);
        }


        /// <summary>
        /// Send an end user a confirmation mail that he/she has reset his/her password and also add the new password.
        /// </summary>
        /// <param name="newPassword"></param>
        public static void SendResetPasswordConfirmationMail(string newPassword, string toAddress, int customerId) {
            ListDictionary replacements = new ListDictionary();
            replacements.Add("<%Password%>", newPassword);
            string emailPathName = "Views\\Account\\Includes\\reset-password.email";
            MailMessage mailMessage = DetermineLocalizedMailMessage(emailPathName, replacements, toAddress);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = @GlobalResources.Resource.ResetPasswordEmailSubject;
            EmailSender.SendMail(mailMessage, Enums.MailCategory.ResetPasswordConfirmation, customerId);
        }

        */
    }
}
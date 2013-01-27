using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRE.Common {
    // TODO Remove this! Use the one from Business instead!
    public class Enums_Bad {

        /// <summary>
        /// Overview of the categories
        /// Filled in in audited mails for purposes of statistics.
        /// </summary>
        public enum MailCategory {
            SubscriptionConfirmation = 0,
            Newsletter = 1,
            NewsletterTest = 2,
            NewsletterActivation = 3
        }

        /// <summary>
        /// The status of an audited mail.
        /// </summary>
        public enum MailStatus {
            Unsent,
            Sent,
            SendError,
            Cancelled,
        }

        public enum PictureTypes {
            ParticipantPicture = 0
        }

    }
}
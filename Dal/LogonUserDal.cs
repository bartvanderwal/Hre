using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using HRE.Business;
using HRE.Data;
using System.Web.Security;

namespace HRE.Dal {
    public class LogonUserDal : ObjectDal {


        #region :: Members
        
        /// <summary>
        ///  Connection to the Database class.
        /// </summary>
        private logonuser _user;
        
        private MembershipUser _membershipUser;

        // private List<SportsEventParticipationDal> _eventParticipations;

        private SportsEventParticipationDal _HRE2012Participation;

        private address _primaryAddress;

        #endregion :: Members

        #region :: Properties

        /// <summary>
        /// The unique identifier / primary key of the user.
        /// </summary>
        public int ID {
            get { return _user.Id; }
        }
		
        // TODO BW 2012-12-27: Change this to use AddressDal.
        public address PrimaryAddress {
            get {
                if (_primaryAddress==null) {
                    _primaryAddress = (from primaryAddress in DB.address where primaryAddress.Id==_user.PrimaryAddressId select primaryAddress).FirstOrDefault();
                    if (_primaryAddress==null) {
                        _primaryAddress = new address();
                    }
                }
                return _primaryAddress;
            }
            set {
                _primaryAddress = value;
            }
        }


        private MembershipUser MembershipUser {
            get {
                if (_membershipUser==null) {
                    _membershipUser = Membership.GetUser(UserName);
                }
                return _membershipUser;
            }
        }

        /// <summary>
        /// The NTB license number of the user.
        /// </summary>
		public string NtbLicenseNumber {
            get { return _user.NtbLicenseNumber; }
            set { _user.NtbLicenseNumber = value; }
        }
        

        /// <summary>
        /// The external Id of the user.
        /// </summary>
		public string ExternalId {
            get { return _user.ExternalId; }
            set { _user.ExternalId = value; }
        }


        public SportsEventParticipationDal HRE2012Participation {
            get {
                if (_HRE2012Participation==null) {
                    _HRE2012Participation = new SportsEventParticipationDal(DB.sportseventparticipation.Where(ep => ep.SportsEventId == SportsEventDal.HRE2012Id && ep.UserId == _user.Id).FirstOrDefault());
                }
                return _HRE2012Participation;
            }
        }


        /// <summary>
        /// The date (time) the user's account was created.
        /// </summary>
        public DateTime DateCreated { 
            get { return _user.DateCreated; }
            set { _user.DateCreated = value; }
        }
		
        /// <summary>
        /// The date of birth of the user.
        /// </summary>
		public DateTime? DateOfBirth {
            get { return _user.DateOfBirth; }
            set { _user.DateOfBirth = value; }
        }
		
        /// <summary>
        /// The e-mail address of the user.
        /// </summary>
		public string EmailAddress {
            get { return _user.EmailAddress; }
            set { _user.EmailAddress = value; }
        }

		/// <summary>
        /// The telepone number of the user.
        /// </summary>
		public string TelephoneNumber {
            get { return _user.TelephoneNumber; }
            set { _user.TelephoneNumber = value; }
        }

        /// <summary>
        /// The subject of the audited e-mail.
        /// </summary>
		public bool? Gender {
            get { return _user.Gender; }
            set { _user.Gender = value; }
        }
		
        /// <summary>
        /// Is the user active?
        /// </summary>
		public bool? IsActive {
            get { return _user.IsActive; }
            set { _user.IsActive = value; }
        }
		

        /// <summary>
        /// Is the user mailing list member.
        /// </summary>
		public bool? IsMailingListMember {
            get { return _user.IsMailingListMember; }
            set { _user.IsMailingListMember = value; }
        }
		
        /// <summary>
        /// The language of the user.
        /// </summary>
		public string Language {
            get { return _user.Language; }
            set { _user.Language = value; }
        }

        /// <summary>
        /// The name / alias of the user.
        /// </summary>
        public string DateSent {
            get { return _user.Name; }
            set { _user.Name = value; }
        }

        /// <summary>
        /// The username of the user.
        /// </summary>
        public string UserName {
            get { return _user.UserName; }
            set { _user.UserName = value; }
        }



        #endregion :: Properties
        
        #region :: Methods

        
        /*
        /// <summary>
        /// Call this function to set a user as a HRE edition 2012 participant.
        /// </summary>3
        public void SetAsHRE2012Participant() {
            if (HRE2012Participation==null) {
                SportsEventParticipationDal SportsEventParticipation = new SportsEventParticipationDal();
                SportsEventParticipation.UserId = _user.Id;
                SportsEventParticipation.SportsEventId = SportsEventDal.HRE2012Id;
                DC.AddTosportseventparticipation(SportsEventParticipation);
            }
            HRE2012Participation.DateRegistered = DateTime.Now;
        }

        */

        /// <summary>
        /// Call this function to remove a user as a HRE edition 2012 participant (only for administrative purposes or errors, or as show of goodwill).
        /// Formally, use the deactivateparticipation method, so in case payment is already done, users have to pay a deregistration fee. 
        /// Or register someone else on their registration (which is then for free, they ave to collect the money thhemselves).
        /// </summary>
        public void RemoveAsHRE2012Participant() {
            
        }


        /// <summary>
        /// Call this function to deactivate a user's HRE edition 2012 participant
        /// They participation itself is kept as the user still has to pay a deregistration fee. 
        /// </summary>
        public void DeactivateHRE2012Participation() {
            
        }


        /// <summary>
        /// Constructor. Construct a user DAL object based on a mailAudit DB object.
        /// </summary>
        public LogonUserDal() {
            _user = new logonuser();
        }


        /// <summary>
        /// Constructor. Construct a user DAL object based on a mailAudit DB object.
        /// </summary>
        public LogonUserDal(logonuser user) {
            if (user.Id==0) {
                throw new ArgumentException("This constructor may only be used with existing 'logonuser' argument, which was retrieved from the database.");
            }
            _user = user;
        }

        
        /// <summary>
        /// Store the user object in the database.
        /// </summary>
        public void Save() {
            // Add entity if it has no primary key yet.
            if (ID==0) {
                DB.AddTologonuser(_user);
            }
            // Add address entity if it has no primary key yet.
            if (PrimaryAddress.Id==0) {
                // Add address entity if it has no primary key yet.
                DB.AddToaddress(PrimaryAddress);
                DB.SaveChanges();
            }
            _user.PrimaryAddressId=PrimaryAddress.Id;

            // And save.
            DB.SaveChanges();
        }


        /// <summary>
        /// Subscribe the user to the newsletter.
        /// </summary>
        public void SubscribeNewsletter() {
            IsMailingListMember=true;
            Save();
        }


        // Creates a new logonuser, that is not active and with a random password stored in the comments.
        public static LogonUserDal CreateOrRetrieveUser(string emailAddressAndUsername, string password="") {
            
            // Create the user for ASP.NET membership.
            MembershipUser membershipUser;

            string userName = Membership.GetUserNameByEmail(emailAddressAndUsername);
            if (!string.IsNullOrEmpty(userName)) {
                if (!string.IsNullOrEmpty(password)) {
                    throw new ArgumentException(string.Format("Supplying a password ({0}) for an already existing user ({1}) is NOT allowed!", password,emailAddressAndUsername));
                }
                membershipUser = Membership.GetUser(userName);
            } else {
                // If no password was supplied then use a temporary keyword.
                bool useTemporaryPassword = string.IsNullOrEmpty(password);
                if (useTemporaryPassword) {
                    password = Guid.NewGuid().ToString().Substring(0,7);
                }

                // Create a new user with a the password.
                MembershipCreateStatus createStatus;
                membershipUser = Membership.CreateUser(emailAddressAndUsername, password, emailAddressAndUsername, null, null, false, null, out createStatus);

                // Create the profile if the user was succesfully made.
                if (createStatus != MembershipCreateStatus.Success) {
                    throw new Exception(string.Format("Error while creating new user for {0}, createStatus: {1}", emailAddressAndUsername, createStatus));
                }

                membershipUser.IsApproved = !useTemporaryPassword;

                if (useTemporaryPassword) {
                    membershipUser.Comment = password;
                }
                Membership.UpdateUser(membershipUser);
            }

            // Create the logonuser.
            logonuser user = (from logonUser in DB.logonuser where logonUser.EmailAddress==membershipUser.Email select logonUser).FirstOrDefault();
            if (user==null) {
                user = new logonuser();
                user.UserName = emailAddressAndUsername;
                user.EmailAddress = emailAddressAndUsername;
                user.DateCreated = DateTime.Now;
                user.DateCreated = user.DateCreated;
                DB.AddTologonuser(user);
            } else {
                user.DateUpdated = DateTime.Now;
            }
            user.ExternalId = membershipUser.ProviderUserKey.ToString();
            DB.SaveChanges();

            return new LogonUserDal(user);
            // return GetByEmailAddress(emailAddressAndUsername);
        }


        /// <summary>
        /// Maak een nieuwe - niet actieve - gebruiker aan, met een lidmaatschap van de e-mail nieuwsbrief.
        /// </summary>
        /// <param name="email"></param>
        public static void AddNotActiveUserWithNewsletterSubscription(string email) {
            LogonUserDal user = CreateOrRetrieveUser(email);
            user.EmailAddress = email;
            user.UserName = email;
            user.IsMailingListMember = true;
            user.MembershipUser.IsApproved = false;
            user.Save();
        }


        /*
        /// <summary>
        /// Return all users.
        /// </summary>
        public static List<LogonUserDal> GetAll(Enums.Roles? role) {
            List<LogonUserDal> users = new List<LogonUserDal>();

            List<logonuser> logonUsers = DC.logonuser.OrderByDescending(u => u.AccountCreated).ToList();
            if (role.HasValue) {
                List<string> userNames = Roles.GetUsersInRole(role.Value.ToString()).ToList();
                logonUsers = logonUsers.Where(u => userNames.Contains(u.UserName));
            }
            
            foreach (logonuser user in ) {
                users.Add(new LogonUserDal(user));
            }

            return users;
        }
        */

        /// <summary>
        /// Return all the users according to the desired audience:
        /// - OnlyToMembers: All that are undefined or that are subscribed to the mailing list / newsletter.
        ///    This is the default; basically only use this!
        /// - SpamAll: All logonusers.
        ///    Use with caution, SPAM alert!
        /// - OnlyToNonMembers: All that are defined NOT to be subscribed to the mailing list / newsletter.
        ///    More SPAM alert. Only to notify non members with a modest message. Basically only to expand OnlyToMembers to if SpamAll was forgotten to be set.
        /// </summary>
        public static List<LogonUserDal> GetNewsletterReceivers(NewsletterAudience audience) {
            int audienceAsInt = (int) audience;
            IQueryable<LogonUserDal> users = from logonuser user in DB.logonuser where 
                    audience == (int) NewsletterAudience.OnlyToMembers && (!user.IsMailingListMember.HasValue || user.IsMailingListMember.Value)
                    || (audienceAsInt == (int) NewsletterAudience.SpamAll)
                    || ((audienceAsInt == (int) NewsletterAudience.OnlyToNonMembers) && user.IsMailingListMember.HasValue && !user.IsMailingListMember.Value) 
                        select new LogonUserDal() {
                            _user = user
                        };

             return users.ToList();
        }


        /// <summary>
        /// Return user by email address.
        /// </summary>
        public static LogonUserDal GetByEmailAddress(string emailaddress) {
            logonuser user = DB.logonuser.Where(u => u.EmailAddress == emailaddress).FirstOrDefault();
            return user!=null ? new LogonUserDal(user) : null;
        }


        /// <summary>
        /// Return user by id.
        /// </summary>
        public static LogonUserDal GetByID(int id) {
            logonuser user = DB.logonuser.Where(u => u.Id == id).FirstOrDefault();
            return user!=null ? new LogonUserDal(user) : null;
        }


        /// <summary>
        /// Return user by user name.
        /// </summary>
        public static LogonUserDal GetByUserName(string userName) {
            logonuser user = DB.logonuser.Where(u => u.UserName==userName).FirstOrDefault();
            return user!=null ? new LogonUserDal(user) : null;
        }


        /// <summary>
        /// Create an entry in the membership table and the user (unless the second item already exists).
        /// </summary>
        public static logonuser CreateUser(string userName, string password, string name, bool? gender, DateTime? dateOfBirth, bool? isMailingListMember, bool isLogOnUser) {
            if (isLogOnUser) {
                // Create ASP.NET user.
                Membership.CreateUser(userName, password, userName);
            }

            // Create custom user to match.
            logonuser user;
            user = GetUserByUsername(userName);
            bool isNewUser = user==null;
            if (isNewUser) {
                user = new logonuser();
                user.DateCreated = DateTime.Now;
                user.UserName = userName;
                user.EmailAddress = user.UserName;
            }
            if (isLogOnUser) {
                user.ExternalId = Membership.GetUser(user.UserName).ProviderUserKey.ToString();
            }
            user.Name = name;
            
            // Read date of birth, and parse with dutch locale (assuming format 'dd-MM-yyyy'enforced in UI).
            user.DateOfBirth = dateOfBirth; 
            user.Gender = gender;
            user.IsMailingListMember = isMailingListMember;
            user.IsActive = isLogOnUser;
            if (isNewUser) {
                DB.AddTologonuser(user);
            }
            DB.SaveChanges();

            if (isLogOnUser) {
                //Make sure the user stays logged in and has a session by setting the (persisted) authorization cookie\
                // (NB Session is stored via URL param if user has cookies disabled if web.config configured correctly).
                FormsAuthentication.SetAuthCookie(userName, true);
            }

            return user;
        }


        /// <summary>
        /// Determines if a user already exists.
        /// </summary>
        public static bool UserExists(string userName) {
            return (DB.logonuser.Where(u => u.UserName == userName).FirstOrDefault())!=null;
        }

        /// <summary>
        /// Gets a user by username.
        /// </summary>
        public static logonuser GetUserByUsername(string userName) {
            return (DB.logonuser.Where(u => u.UserName == userName).FirstOrDefault());
        }

        /// <summary>
        /// Gets a user by Id.
        /// </summary>
        public static logonuser GetUserById(int userId) {
            return (DB.logonuser.Where(u => u.Id == userId).FirstOrDefault());
        }


        public static string getFullName(string userName) {
            logonuser user = DB.logonuser.Where(u => u.UserName == userName).FirstOrDefault();
            address address = DB.address.Where(a => a.Id == user.PrimaryAddressId).FirstOrDefault();
            return address.Firstname + (string.IsNullOrEmpty(address.Insertion) ? " " + address.Insertion : "") + " " + address.Lastname;
        }


        /// <summary>
        /// Return the address.
        /// </summary>
        public static string GetAddress(logonuser user) {
            if (user.PrimaryAddressId!=null && user.PrimaryAddressId!=0) {
                address primaryAddress = DB.address.Where(a => a.Id == user.PrimaryAddressId).FirstOrDefault();
                string address = "";
                if (!string.IsNullOrEmpty(primaryAddress.CompanyName)) { 
                    address += primaryAddress.CompanyName + "\n" + "T.a.v. ";
                }
                address += (user.Gender.HasValue ? (user.Gender.Value ? "Dhr." : "Mw.") : "")
                    + " " + primaryAddress.Firstname + " " + primaryAddress.Insertion + " " + primaryAddress.Lastname + "\n"
                    + primaryAddress.Street + " " + primaryAddress.Housenumber + " " + primaryAddress.HouseNumberAddition + "\n"
                    + primaryAddress.PostalCode + " " + primaryAddress.City;
                return address;
            } else {
                return "Adres onbekend";
            }
        }

        /// <summary>
        /// Change the addres of a user.
        /// </summary>
        public static void SetPrimaryAddress(int userId, int addressId) {
            logonuser user = GetUserById(userId);
            user.PrimaryAddressId = addressId;
            DB.SaveChanges();
        }
        #endregion :: Methods


        /// <summary>
        /// Get the LogonUserDal object for the currently logged in (Membership) user.
        /// </summary>
        /// <returns></returns>
        public static LogonUserDal GetCurrentUser() {
            return GetByEmailAddress(Membership.GetUser().Email);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using HRE.Business;
using HRE.Data;
using System.Web.Security;
using System.Web.Mvc;
using HRE.Models;

namespace HRE.Dal {
    public class LogonUserDal : ObjectDal {


        #region :: Members
        
        /// <summary>
        ///  Connection to the Database class.
        /// </summary>
        private logonuser _user;
        
        private MembershipUser _membershipUser;

        private address _primaryAddress;

        #endregion :: Members

        #region :: Properties

        /// <summary>
        /// The unique identifier / primary key of the user.
        /// </summary>
        public int Id {
            get { return _user.Id; }
        }
		
        // TODO BW 2012-12-27: Change this to use AddressDal.
        public address PrimaryAddress {
            get {
                if (_primaryAddress==null) {
                    _primaryAddress = (from primaryAddress in DB.address where primaryAddress.Id==_user.PrimaryAddressId select primaryAddress).FirstOrDefault();
                    if (_primaryAddress==null) {
                        _primaryAddress = new address();
                        _primaryAddress.DateCreated = DateTime.Now;
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
            set { 
                if (Membership.GetUser()!=null && Membership.GetUser().ProviderUserKey.ToString() == _user.ExternalId) {
                    throw new ArgumentException("Het e-mail adres van de gebruiker kan NIET gewijzigd worden als deze momenteel ingelogd is/de huidige gebruiker is.");
                }
                _user.EmailAddress = value;
                MembershipUser.Email = value;
                if (_user.UserName!=EmailAddress) {
                    ChangeUserName(int.Parse(ExternalId), value);
                }
            }
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

        
        public LogonUserStatus Status {
            get {
                return (LogonUserStatus) (_user.StatusId.HasValue ? _user.StatusId.Value : 0);
            }
            set {
                _user.StatusId=(int) value;
            }
        }


        /// <summary>
        /// This method should be called on existing logon users (stored in database) on receiving confirmation of the users email address.
        /// This sets the user status from undetermined or new to confirmed email address.
        /// </summary>
        /// <returns>Returns true when the e-mail address was not confirmed before (but is now), otherwise returns false.</returns>
        public bool ConfirmEmailAddress() {
            if (Id!=0 && IsEmailAddressUnconfirmed) {
                Status = LogonUserStatus.EmailAddressConfirmed;
                Save();
                return true;
            }
            return false;
        }

        public bool IsEmailAddressUnconfirmed {
            get {
                return Status==LogonUserStatus.Undetermined || Status==LogonUserStatus.New;
            }
        }

        /// <summary>
        /// Store the user object in the database.
        /// </summary>
        public void Save() {
            // Add entity if it has no primary key yet.
            if (Id==0) {
                DB.AddTologonuser(_user);
            }

            // Set the user status to new, if it does not have a value yet.
            if (!_user.StatusId.HasValue) {
                Status=LogonUserStatus.New;
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
        public static LogonUserDal CreateOrRetrieveUser(string emailAddressAndUsername, string password="", string externalSubscriptionIdentifier = null) {
            
            // If an non 'HRE' external identifier (meaning a real ext. id) was given, then try to retrieve the user via this.
            if (!string.IsNullOrEmpty(externalSubscriptionIdentifier) && externalSubscriptionIdentifier!="HRE0" /* && !externalSubscriptionIdentifier.StartsWith("HRE") */) {
                int? userId = (from p in DB.sportseventparticipation where p.ExternalIdentifier == externalSubscriptionIdentifier select p.UserId).FirstOrDefault();
                if (userId.HasValue) {
                    logonuser u = (from logonUser in DB.logonuser where logonUser.Id==userId.Value select logonUser).FirstOrDefault();
                    if (u!=null) {
                        return new LogonUserDal(u);
                    }
                }
            }

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
                user.DateUpdated = user.DateCreated;
                DB.AddTologonuser(user);
            } else {
                user.DateUpdated = DateTime.Now;
            }
            user.ExternalId = membershipUser.ProviderUserKey.ToString();
            DB.SaveChanges();

            return new LogonUserDal(user);
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
        public static List<LogonUserDal> GetNewsletterReceivers(
            NewsletterSubscriptionStatus subscriptionStatus = NewsletterSubscriptionStatus.OnlyToMembers, 
            EntryFeePaidStatus feePaidStatus = EntryFeePaidStatus.All,
            EarlyBirdStatus earlyBirdStatus = EarlyBirdStatus.All,
            HREEventParticipantStatus hre2012ParticipantStatus = HREEventParticipantStatus.All) {
            
            List<LogonUserDal> users = LogonUserDal.MakeList(DB.logonuser.ToList());

            switch (subscriptionStatus) {
                case NewsletterSubscriptionStatus.OnlyToMembers:
                    users = users.Where(u => u.IsMailingListMember.HasValue && u.IsMailingListMember.Value).ToList();
                    break;
                case NewsletterSubscriptionStatus.OnlyToNonMembers:
                    users = users.Where(u => u.IsMailingListMember.HasValue && !u.IsMailingListMember.Value).ToList();
                    break;
            }


            // Check if one of the provided filter parameters requires further trimming.
            if (feePaidStatus!=EntryFeePaidStatus.All 
                || earlyBirdStatus!=EarlyBirdStatus.All
                || hre2012ParticipantStatus!=HREEventParticipantStatus.All) {

                /*
                switch (feePaidStatus) {
                    case EntryFeePaidStatus.OnlyPaid:
                        users = users.Where(u => entries.Contains(u.Id)).ToList();
                        break;
                    case EntryFeePaidStatus.OnlyNonPaid:
                        users = users.Where(u => u.IsMailingListMember.HasValue && !u.IsMailingListMember.Value).ToList();
                        break;
                }
                */
                
                List<int> entries2012UserIds = InschrijvingenRepository.GetEntries(InschrijvingenRepository.HRE_EVENTNR, true).Select(e => e.UserId).ToList();


                switch (hre2012ParticipantStatus) {
                    case HREEventParticipantStatus.OnlyParticipants:
                        users = users.Where(u => entries2012UserIds.Contains(u.Id)).ToList();
                        break;
                    case HREEventParticipantStatus.OnlyNonParticipants:
                        users = users.Where(u => !entries2012UserIds.Contains(u.Id)).ToList();
                        break;
                }
            }
            return users;
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
        public static LogonUserDal CreateUser(string userName, string password, string name, bool? gender, DateTime? dateOfBirth, bool? isMailingListMember, bool isLogOnUser) {
            if (isLogOnUser) {
                // Create ASP.NET user.
                Membership.CreateUser(userName, password, userName);
            }

            // Create custom user to match.
            LogonUserDal user;
            user = GetUserByUsername(userName);
            bool isNewUser = user==null;
            if (isNewUser) {
                user = new LogonUserDal();
                user.DateCreated = DateTime.Now;
                user.UserName = userName;
                user.EmailAddress = user.UserName;
            }
            if (isLogOnUser) {
                user.ExternalId = Membership.GetUser(user.UserName).ProviderUserKey.ToString();
            }
            user.UserName = name;
            
            // Read date of birth, and parse with dutch locale (assuming format 'dd-MM-yyyy'enforced in UI).
            user.DateOfBirth = dateOfBirth; 
            user.Gender = gender;
            user.IsMailingListMember = isMailingListMember;
            user.IsActive = isLogOnUser;
            if (isNewUser) {
                user.Save();
            }
            // DB.SaveChanges();

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
        public static LogonUserDal GetUserByUsername(string userName) {
            logonuser user = DB.logonuser.Where(u => u.UserName == userName).FirstOrDefault();
            
            if (user!=null) {
                return new LogonUserDal((DB.logonuser.Where(u => u.UserName == userName).FirstOrDefault()));
            } else {
                return null;
            }
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
            var currentUser = Membership.GetUser();
            return currentUser!=null ? GetByEmailAddress(currentUser.Email) : null;
        }


        /// <summary>
        /// Determines the number of participants or Early Birds in the 2013 HRE event.
        /// </summary>
        /// <returns></returns>
        public static int AantalIngeschrevenEarlyBirds() {
            return (from p in DB.sportseventparticipation 
                    where p.SportsEventId==SportsEventDal.Hre2013Id && (p.EarlyBird.HasValue && p.EarlyBird.Value)
                    select p
                    ).Count();
        }


        /// <summary>
        /// Determines the number of participants or Early Birds in the 2013 HRE event.
        /// </summary>
        /// <returns></returns>
        public static int AantalIngeschrevenDeelnemers(string eventNr, bool? food=null, bool? camp = null, bool? bike = null, bool? emailConfirmed = null, bool? earlyBird = null) {
            var result = from p in DB.sportseventparticipation 
                    join e in DB.sportsevent on p.SportsEventId equals e.Id
                    where e.ExternalEventIdentifier==eventNr
                    select p
                    ;

            if (food.HasValue) {
                result = result.Where(r => r.Food==food.Value);
            }

            if (camp.HasValue) {
                result = result.Where(r => r.Camp==camp.Value);
            }

            if (bike.HasValue) {
                result = result.Where(r => r.Bike==bike.Value);
            }

            if (emailConfirmed.HasValue) {
                result = result.Where(r => r.ParticipationStatus.HasValue && r.ParticipationStatus.Value==2);
            }

            if (earlyBird.HasValue) {
                result = result.Where(r => r.EarlyBird==earlyBird);
            }

            return result.Count();
        }

        
        /// <summary>
        /// For testing purposes this gets that part of the registered admin users who were also subscribed 
        /// for HRE 2012 (inserted for testing in NTB inschrijvingen).
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<LogonUserDal> GetTestParticipants() {
            List<LogonUserDal> adminAndSpeakerUsers = new List<LogonUserDal>();

            List<string> adminUserNames = Roles.GetUsersInRole(InschrijvingenRepository.ADMIN_ROLE_NAME).ToList();
            List<string> speakerUserNames = Roles.GetUsersInRole(InschrijvingenRepository.ADMIN_ROLE_NAME).ToList();

            List<string> adminAndSpeakerUserNames = adminUserNames;
            adminAndSpeakerUserNames.AddRange(speakerUserNames);

            foreach (string adminUserName in adminAndSpeakerUserNames) {
                LogonUserDal user = LogonUserDal.GetByUserName(adminUserName);
                if(user!=null) {
                    SportsEventParticipationDal participation = SportsEventParticipationDal.GetByUserIdEventId(user.Id, InschrijvingenRepository.GetHreEvent().Id);
                    if (participation!=null) {
                        adminAndSpeakerUsers.Add(user);
                    }
                }
            }

            return adminAndSpeakerUsers;
        }


        public static List<SelectListItem> GetTestParticipantsAsSelectList() {
            List<SelectListItem> result = new List<SelectListItem>();
            
            foreach(LogonUserDal user in GetTestParticipants()) {
                string text = user.UserName.Substring(0, user.UserName.IndexOf('@'));
                result.Add(new SelectListItem() { Text = text, Value = user.Id.ToString()});
            }

            return result;
        }


        public static List<int> GetTestParticipantIds() {
            List<int> result = new List<int>();
            
            foreach(LogonUserDal user in GetTestParticipants()) {
                result.Add(user.Id);
            }

            return result;
        }


        /// <summary>
        /// Constructor for list. Construct a list of objects based on a list of DB objects.
        /// </summary>
        public static List<LogonUserDal> MakeList(List<logonuser> items) {
            List<LogonUserDal> dals = new List<LogonUserDal>(items.Count());
            foreach (logonuser item in items) {
                dals.Add(new LogonUserDal(item));
            }
            return dals;
        }


        /// <summary>
        /// Return all entities.
        /// </summary>
        public static List<LogonUserDal> GetAll() {             
            return MakeList(DB.logonuser.ToList());
        }


        /// <summary>
        /// Return all entities.
        /// </summary>
        public static List<SelectListItem> GetAllAsSelectList() {
            List<SelectListItem> result = new List<SelectListItem>();
            
            List<int> testParticipants = GetTestParticipantIds();

            foreach(LogonUserDal user in GetAll()) {
                if (!testParticipants.Contains(user.Id)) {
                    result.Add(new SelectListItem() { Text = user.EmailAddress, Value = user.Id.ToString()});
                }
            }

            return result;
        }
            


        /// <summary>
        /// Source: http://stackoverflow.com/questions/2141952/manually-changing-username-in-asp-net-membership
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newUserName"></param>
        /// <returns></returns>
        public static bool ChangeUserName(int externalUserId, string newUserName) {
            bool success = false;
            newUserName = newUserName.Trim();

            // Make sure there is no user with the new username.
            if (Membership.GetUser(newUserName) == null) {
                MembershipUser u = Membership.GetUser(externalUserId);
                string oldUsername = u.UserName;
                // get current application

                my_aspnet_users userToChange = (from user in DB.my_aspnet_users
                                    where user.id == externalUserId
                                    select user).FirstOrDefault();

                if (userToChange != null) {
                    userToChange.name = newUserName;

                    DB.SaveChanges();

                    /*
                    // ASP.NET Issues a cookie with the user name. 
                    // When a request is made with the specified cookie, 
                    // ASP.NET creates a row in aspnet_users table.
                    // To prevent this sign out the user and then sign it in
                    string cookieName = FormsAuthentication.FormsCookieName;
                    HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];

                    FormsAuthenticationTicket authTicket = null;

                    try {
                        authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                        FormsIdentity formsIdentity = new FormsIdentity(
                                new FormsAuthenticationTicket(
                                    authTicket.Version, 
                                    newUserName, 
                                    authTicket.IssueDate, 
                                    authTicket.Expiration, 
                                    authTicket.IsPersistent, 
                                    authTicket.UserData));

                        string y = HttpContext.Current.User.Identity.Name;
                        string[] roles = authTicket.UserData.Split(new char[] { '|' });
                        System.Security.Principal.GenericPrincipal genericPrincipal = 
                            new System.Security.Principal.GenericPrincipal(
                                                                formsIdentity, 
                                                                roles);

                        HttpContext.Current.User = genericPrincipal;
                    }
                    catch (ArgumentException ex) {
                        // Handle exceptions
                    }
                    catch( NullReferenceException ex) {
                        // Handle exceptions
                    }

                    FormsAuthentication.SignOut();
                    HttpContext.Current.Session.Abandon();
                    FormsAuthentication.SetAuthCookie(newUserName, false);
                    */
                    success = true;
                }
            }

            return success;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using HRE.Business;
using HRE.Data;

namespace HRE.Dal {
    public class SportsEventParticipationDal : UserCreatableObjectDal {

        #region :: Members
        
        /// <summary>
        ///  Connection to the Database class.
        /// </summary>
        private sportseventparticipation _sportseventparticipation;
        

        #endregion :: Members

        #region :: Properties

        /// <summary>
        /// The unique identifier / primary key of the user.
        /// </summary>
        public int ID {
            get { return _sportseventparticipation.Id; }
        }

		
        /// <summary>
        /// The date (time) the enitity was created.
        /// </summary>
        public override DateTime DateCreated { 
            get { return _sportseventparticipation.DateCreated; }
        }


        /// <summary>
        /// The date (time) the enitity was last updated.
        /// </summary>
        public override DateTime DateUpdated { 
            get { return _sportseventparticipation.DateUpdated; }
        }


		/// <summary>
        /// The identifier of this eventparticipation in the external system (NTBinschrijvingen.nl).
        /// </summary>
		public string ExternalIdentifier {
            get { return _sportseventparticipation.ExternalIdentifier; }
            set { _sportseventparticipation.ExternalIdentifier = value; }
        }
		

        /// <summary>
        /// The identifier of this eventparticipation in the external system (NTBinschrijvingen.nl).
        /// </summary>
		public DateTime DateRegistered {
            get { return _sportseventparticipation.DateRegistered; }
            set { _sportseventparticipation.DateRegistered = value; }
        }


        /// <summary>
        /// The date the (last) payment was received.
        /// </summary>
		public DateTime? DatePaymentReceived {
            get { return _sportseventparticipation.DatePaymentReceived; }
            set { _sportseventparticipation.DatePaymentReceived = value; }
        }


        /// <summary>
        /// Het te betalen bedragen (afhankelijk van NTB lidmaatschap ja/nee, eigen chip ja/nee en moment van inschrijven ivm tussentijdse prijsstijging).
        /// </summary>
		public int? ParticipationAmountInEuroCents {
            get { return _sportseventparticipation.ParticipationAmountInEuroCents; }
            set { _sportseventparticipation.ParticipationAmountInEuroCents = value; }
        }


        /// <summary>
        /// Het betaalde bedrag (bv. op afschrift). Zou gelijk moeten zijn aan het te betaal
        /// </summary>
        public int? ParticipationAmountPaidInEuroCents {
            get { return _sportseventparticipation.ParticipationAmountPaidInEuroCents; }
            set { _sportseventparticipation.ParticipationAmountInEuroCents = value; }
        }


        public SportsEventDal SportsEvent {
            get {
                return SportsEventDal.GetById(_sportseventparticipation.SportsEventId.Value);
            }
        }

        #endregion :: Properties
        
        #region :: Methods


        /// <summary>
        /// Constructor. Construct a DAL object based on a DB object.
        /// </summary>
        public SportsEventParticipationDal(sportseventparticipation sportseventparticipation) {
            _sportseventparticipation = sportseventparticipation;
        }


        /// <summary>
        /// Constructor. Construct an empty DAL object.
        /// </summary>
        public SportsEventParticipationDal() {
            _sportseventparticipation = new sportseventparticipation();
        }


        /// <summary>
        /// Store the user object in the database.
        /// </summary>
        public void Save() {
            // Add entity if it has no primary key yet.
            if (ID == 0) {
                _sportseventparticipation.DateCreated = DateTime.Now;
                DB.AddTosportseventparticipation(_sportseventparticipation);
            }
            _sportseventparticipation.DateUpdated = DateTime.Now;
            DB.SaveChanges();
        }


        /// <summary>
        /// Delete the participation.
        /// </summary>
        public void Delete() {
            // Add entity if it has no primary key yet.
  		    DB.DeleteObject(_sportseventparticipation);
  		    DB.SaveChanges();
        }



        /// <summary>
        /// Return all users.
        /// </summary>
        public static List<SportsEventParticipationDal> GetAll() {
             List<SportsEventParticipationDal> participations = new List<SportsEventParticipationDal>();
             
             foreach (sportseventparticipation participation in DB.sportseventparticipation.OrderByDescending(p => p.DateRegistered)) {
                participations.Add(new SportsEventParticipationDal(participation));
             }

             return participations;
        }


        /// <summary>
        /// Return by id.
        /// </summary>
        public static SportsEventParticipationDal GetByID(int id) {
            sportseventparticipation participation = DB.sportseventparticipation.Where(p => p.Id == id).FirstOrDefault();
            return participation!=null ? new SportsEventParticipationDal(participation) : null;
        }


        /// <summary>
        /// Return by userId and event Id combination.
        /// </summary>
        public static SportsEventParticipationDal GetByUserIdEventId(int userId, int eventId) {
            sportseventparticipation participation = DB.sportseventparticipation.Where(p => p.UserId == userId && p.SportsEventId == eventId).FirstOrDefault();
            return participation!=null ? new SportsEventParticipationDal(participation) : null;
        }


        #endregion :: Methods
    }
}

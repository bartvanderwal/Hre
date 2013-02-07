using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using HRE.Business;
using HRE.Data;

namespace HRE.Dal {
    public class SportsEventDal : ObjectDal {

        #region :: Members
        
        /// <summary>
        ///  Connection to the Database class.
        /// </summary>
        private sportsevent _sportsEvent;
        
        public const int HRE2012Id = 1;



        #endregion :: Members

        #region :: Properties

        /// <summary>
        /// The unique identifier / primary key of the audited e-mail.
        /// </summary>
        public int ID {
            get { return _sportsEvent.Id; }
        }
		

        /// <summary>
        /// The date(time) this e-mail audit was sent (last time e-mail succceeded send attempt).
        /// </summary>
        public DateTime? EventDate{
            get { return _sportsEvent.EventDate; }
            set { _sportsEvent.EventDate = value; }
        }


        #endregion :: Properties
        
        #region :: Methods

        public SportsEventDal GetCurrentEvent() {
            // TODO BW 2012-07-04: Make user configurable through UI.
            return GetById(HRE2012Id);
        }


        /// <summary>
        /// Constructor. Construct a mailaudit DAL object based on a mailAudit DB object.
        /// </summary>
        public SportsEventDal(sportsevent sportsEvent) {
            _sportsEvent = sportsEvent;
        }


        /// <summary>
        /// Store the object in the database.
        /// </summary>
        public void Save() {
            // Add entity if it has no primary key yet.
            if (ID == 0) {
                DB.AddTosportsevent(_sportsEvent);
            }
  		    DB.SaveChanges();
        }


        /// <summary>
        /// Return all the mail audits.
        /// </summary>
        public static List<SportsEventDal> GetAll() {
             List<SportsEventDal> sportsEvents = new List<SportsEventDal>();
             
             foreach (sportsevent sportsEvent in DB.sportsevent.OrderByDescending(m => m.DateCreated)) {
                sportsEvents.Add(new SportsEventDal(sportsEvent));
             }

             return sportsEvents;
        }


        /// <summary>
        /// Return the item with the given ID.
        /// </summary>
        public static SportsEventDal GetById(int id) {
            sportsevent sportsevent = DB.sportsevent.Where(n => n.Id == id).FirstOrDefault();
            if (sportsevent != null) { 
                return new SportsEventDal(sportsevent);
            } else {
                return null;
            }
        }


        /// <summary>
        /// Return the item with the given ExternalId.
        /// </summary>
        public static SportsEventDal GetByExternalId(string id) {
            sportsevent sportsevent = DB.sportsevent.Where(n => n.ExternalEventIdentifier == id).FirstOrDefault();
            if (sportsevent != null) { 
                return new SportsEventDal(sportsevent);
            } else {
                return null;
            }
        }
        
        #endregion :: Methods
    }
}

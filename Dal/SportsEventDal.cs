using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using HRE.Business;
using HRE.Data;
using HRE.Models;

namespace HRE.Dal {
    public class SportsEventDal : ObjectDal {

        public static int Hre2012Id {
            get {
                return SportsEventRepository.GetEvent(SportsEventRepository.HRE_EVENTNR).Id;
            }
        }


        public static int Hre2013Id {
            get {
                return SportsEventRepository.GetEvent(SportsEventRepository.H2RE_EVENTNR).Id;
            }
        }

        
        #region :: Members
        
        /// <summary>
        ///  Connection to the Database class.
        /// </summary>
        private sportsevent _sportsEvent;


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
        public DateTime? EventDate {
            get { return _sportsEvent.EventDate; }
            set { _sportsEvent.EventDate = value; }
        }


        /// <summary>
        /// The name of the event.
        /// </summary>
        public string Name {
            get { return _sportsEvent.Name; }
            set { _sportsEvent.Name = value; }
        }


        /// <summary>
        /// The external event identifier from NTB inschrijvingen.
        /// </summary>
        public string ExternalEventIdentifier {
            get { return _sportsEvent.ExternalEventIdentifier; }
            set { _sportsEvent.ExternalEventIdentifier = value; }
        }


        /// <summary>
        /// The external event serie identifier from NTB inschrijvingen.
        /// </summary>
        public string ExternalEventSerieIdentifier {
            get { return _sportsEvent.ExternalEventSerieIdentifier; }
            set { _sportsEvent.ExternalEventSerieIdentifier = value; }
        }


        /// <summary>
        /// The place the event takes ehm... place.
        /// </summary>
        public string EventPlace {
            get { return _sportsEvent.EventPlace; }
            set { _sportsEvent.EventPlace = value; }
        }


        #endregion :: Properties
        
        #region :: Methods


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

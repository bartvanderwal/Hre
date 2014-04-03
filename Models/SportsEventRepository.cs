using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using HRE.Models;
using HRE.Data;
using HRE.Common;
using HRE.Dal;
using HRE.Business;

namespace HRE.Models {

    /// <summary>
    /// Repostory to retrieve information about sportsevent from the database.
    /// </summary>
    public sealed class SportsEventRepository : BaseRepository {

        #region :: Constants

        public const string HRE_NAME = "HRE";

        public const string HRE_EVENTNR = "2005881";

        public const string HRE_SERIENR = "4549";

        public static DateTime HRE_DATE = new DateTime(2012, 8, 4, 16, 0, 0);

        public const string H2RE_NAME = "H2RE";

        public const string H2RE_EVENTNR = "2005971";

        public const string H2RE_SERIENR = "5089";

        public static DateTime H2RE_DATE = new DateTime(2013, 8, 3, 16, 0, 0);

        public const string H3RE_NAME = "H3RE";

        public const string H3RE_EVENTNR = "2006240";

        public const string H3RE_SERIENR = "6941";

        public static DateTime H3RE_DATE = new DateTime(2014, 8, 2, 15, 0, 0);


        #endregion :: Constants


        public static sportsevent GetHreEvent() {
            return GetOrCreateEvent(HRE_EVENTNR, HRE_SERIENR, HRE_NAME, HRE_DATE);
        }

        
        public static sportsevent GetH2reEvent() {
            return GetOrCreateEvent(H2RE_EVENTNR, H2RE_SERIENR, H2RE_NAME, H2RE_DATE);
        }


        private static SportsEventRepository _instance;

        private static object syncRoot = new Object();

        /// <summary>
        /// The current event.
        /// This is a singleton; source: http://msdn.microsoft.com/en-us/library/ff650316.aspx
        /// </summary>
        public static SportsEventRepository Instance {
            get  {
                if (_instance == null)  {
                    lock (syncRoot) {
                        if (_instance == null) {
                            _instance = new SportsEventRepository();
                        }
                    }
                }
                return _instance;
            }
        }

   
        /// <summary>
        /// The current event
        /// </summary>
        public sportsevent CurrentEvent { get; set; }


        /// <summary>
        /// The current event
        /// </summary>
        public DateTime LastUpdated { get; set; }


        /// <summary>
        /// The current event from the instance
        /// </summary>
        public static sportsevent CurrentEventInstance { 
            get {
                DateTime now = DateTime.Now;
                DateTime fourHoursAgo = now.AddMinutes(-1);
                if (Instance.CurrentEvent == null || Instance.LastUpdated<=fourHoursAgo ) {
                    Instance.CurrentEvent = GetCurrentEvent();
                    Instance.LastUpdated = now;
                }
                return Instance.CurrentEvent;
            }
        }


        /// <summary>
        /// Get the current HRE event (e.g. of 'this' year).
        /// </summary>
        /// <returns></returns>
        public static sportsevent GetCurrentEvent() {
            hreEntities DB = DBConnection.GetHreContext();
            int currentYear = DateTime.Now.Year;

            sportsevent result = (
                from e in DB.sportsevent 
                where e.EventDate.HasValue && e.EventDate.Value.Year==currentYear
                select e).FirstOrDefault();

            return result;
        }


        /// <summary>
        /// Get the next HRE event (e.g. of 'next' year).
        /// </summary>
        /// <returns></returns>
        public static sportsevent GetNextEvent() {
            hreEntities DB = DBConnection.GetHreContext();
            int nextYear = DateTime.Now.Year+1;

            sportsevent result = (
                from e in DB.sportsevent 
                where e.EventDate.HasValue && e.EventDate.Value.Year==nextYear
                select e).FirstOrDefault();

            return result;
        }


        /// <summary>
        /// Gets an event by external=eventNumber, or creates it in the database if it doesn't exist yet.
        /// </summary>
        /// <param name="eventNumber"></param>
        /// <param name="eventName"></param>
        /// <param name="eventDate"></param>
        /// <returns></returns>
        private static sportsevent GetOrCreateEvent(string eventNumber, string serieNumber, string eventName, DateTime eventDate) {
            sportsevent hreEvent = GetEvent(eventNumber);
            if (hreEvent==null) {
                hreEvent = new sportsevent();
                hreEvent.Name=HRE_NAME;
                hreEvent.ExternalEventIdentifier=eventNumber;
                hreEvent.ExternalEventSerieIdentifier=serieNumber;
                hreEvent.DateCreated = DateTime.Now;
                hreEvent.DateUpdated = DateTime.Now;
                hreEvent.EventDate = eventDate;
                hreEvent.EventPlace = "Vinkeveen - Zandeiland 1";
                DB.AddTosportsevent(hreEvent);
                DB.SaveChanges();
            }
            return hreEvent;
        }


        /// <summary>
        /// Gets an event by external=eventNumber, or creates it in the database if it doesn't exist yet.
        /// </summary>
        /// <param name="eventNumber"></param>
        public static sportsevent GetEvent(string eventNumber) {
            return (from e in DB.sportsevent where e.ExternalEventIdentifier==eventNumber select e).FirstOrDefault();
        }


        /// <summary>
        /// Event identifier van het huidige HRE event.
        /// </summary>
        public static string CurrentExternalEventIdentifier {
            get {
                return CurrentEventInstance.ExternalEventIdentifier;
            }
        }
        

        /// <summary>
        /// Event serie identifier van het huidige HRE event.
        /// </summary>
        public static string CurrentExternalEventSerieIdentifier {
            get {
                return CurrentEventInstance.ExternalEventSerieIdentifier;
            }
        }


        /// <summary>
        /// Het totaal aantal startplekken van de wedstrijd.
        /// Iets van: 700.
        /// </summary>
        public static int AantalStartPlekken {
            get {
                return CurrentEventInstance.AantalStartPlekken;
            }
        }


        /// <summary>
        /// Het aantal plekken op de reservelijst (inclusief early birds).
        /// Iets van: 100.
        /// </summary>
        public static int AantalPlekkenReserveLijst {
            get {
                return CurrentEventInstance.AantalPlekkenReserveLijst;
            }
        }

        
        /// <summary>
        /// Het aantal Early Bird startplekken.
        /// Iets van: 200.
        /// </summary>
        public static int AantalEarlyBirdStartPlekken {
            get {
                return CurrentEventInstance.AantalEarlyBirdStartPlekken;
            }
        }


        /// <summary>
        /// De einddatum waarna er geen Early Bird korting meer wordt gegeven.
        /// Iets van: dag dat de algemene inschrijving opent.
        /// </summary>
        public static DateTime EindDatumEarlyBirdKorting {
            get {
                return CurrentEventInstance.EindDatumEarlyBirdKorting;
            }
        }


        /// <summary>
        /// Openingsdatum algemene inschrijving.
        /// </summary>
        public static DateTime OpeningsdatumAlgemeneInschrijving {
            get {
                return CurrentEventInstance.OpeningsdatumAlgemeneInschrijving;
            }
        }


        /// <summary>
        /// Sluitingsdatum algemene inschrijving.
        /// </summary>
        public static DateTime SluitingsDatumAlgemeneInschrijving {
            get {
                return CurrentEventInstance.SluitingsDatumAlgemeneInschrijving;
            }
        }


        /// <summary>
        /// Het huidige - kale - deelnamebedrag (exclusief korting zoals early bird, en extra kosten zoals chip/licentie).
        /// Iets van: 2500.
        /// </summary>
        public static int HuidigeDeelnameBedrag {
            get {
                return CurrentEventInstance.HuidigeDeelnameBedrag;
            }
        }


        /// <summary>
        /// Kosten van een daglicentie.
        /// Iets van: 220 (EUR 2,20).
        /// </summary>
        public static int KostenNtbDagLicentie {
            get {
                return CurrentEventInstance.KostenNtbDagLicentie;
            }
        }


        /// <summary>
        /// De kosten voor huur van een gele MyLaps chip.
        /// Iets van: 200 (2,- EUR).
        /// </summary>
        public static int KostenHuurMyLapsChipGeel {
            get {
                return CurrentEventInstance.KostenHuurMyLapsChipGeel;
            }
        }


        /// <summary>
        /// Kosten voor gebruik van een groene MyLaps chip.
        /// Iets van: 150 (1,50 EUR).
        /// </summary>
        public static int KostenGebruikMyLapsChipGroen {
            get {
                return CurrentEventInstance.KostenHuurMyLapsChipGeel;
            }
        }


        /// <summary>
        /// De kosten voor eten.
        /// Iets van: 1000 (10,- EUR).
        /// </summary>
        public static int KostenEten {
            get {
                return CurrentEventInstance.KostenEten;
            }
        }

        
        /// <summary>
        /// De korting voor Early Birds.
        /// Iets van: 500 (5,- EUR).
        /// </summary>
        public static int HoogteEarlyBirdKorting {
            get {
                return CurrentEventInstance.HoogteEarlyBirdKorting;
            }
        }
    }
}

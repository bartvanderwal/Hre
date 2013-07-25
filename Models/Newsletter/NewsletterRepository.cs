using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using HRE.Models;
using HRE.Data;
using HRE.Dal;
using System.Web.Mvc;
using HRE.Business;

namespace HRE.Models.Newsletters {
    public class NewsletterRepository : BaseRepository {

        /// <summary>
        /// Get a select list of possible subscription stati of receivers.
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> SubscriptionStatusSelectList(string selectedValue) {
            List<SelectListItem> result = new List<SelectListItem> { 
                new SelectListItem { Text = "Alleen leden", Value = NewsletterSubscriptionStatus.OnlyToMembers.ToString()},
                new SelectListItem { Text = "Iedereen (spam alert!)", Value = NewsletterSubscriptionStatus.SpamAll.ToString()},
		        new SelectListItem { Text = "Alleen niet leden (spam alert!)", Value = NewsletterSubscriptionStatus.OnlyToNonMembers.ToString()},
            };
            SelectListItem selectedItem = result.Where(sli => sli.Value==selectedValue).FirstOrDefault();
            if (selectedItem!=null) {
                selectedItem.Selected = true;
            }
            return result;
        }


        /// <summary>
        /// Get a select list of possible status for participation in a certain year (e.g. 2012) or not of end users.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static List<SelectListItem> HreParticipantStatusSelectList(string selectedValue) {
            List<SelectListItem> result = new List<SelectListItem> { 
                new SelectListItem { Text = "Maakt niet uit", Value = HREEventParticipantStatus.All.ToString()},
                new SelectListItem { Text = "Deelnemer", Value = HREEventParticipantStatus.OnlyParticipants.ToString()},
		        new SelectListItem { Text = "Geen deelnemer", Value = HREEventParticipantStatus.OnlyNonParticipants.ToString()},
            };
            SelectListItem selectedItem = result.Where(sli => sli.Value==selectedValue).FirstOrDefault();
            if (selectedItem!=null) {
                selectedItem.Selected = true;
            }
            return result;
        }
        

        /*
        /// <summary>
        /// Get a select list of Early Bird status for participation in a certain year (e.g. 2013) or not of end users.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static List<SelectListItem> EarlyBirdStatus(string selectedValue) {
            List<SelectListItem> result = new List<SelectListItem> { 
                new SelectListItem { Text = "Maakt niet uit", Value = EarlyBirdStatus.All.ToString()},
                new SelectListItem { Text = "Early Birds", Value = EarlyBirdStatus.OnlyParticipants.ToString()},
		        new SelectListItem { Text = "Non Early Birds", Value = EarlyBirdStatus.OnlyNonParticipants.ToString()},
            };
            SelectListItem selectedItem = result.Where(sli => sli.Value==selectedValue).FirstOrDefault();
            if (selectedItem!=null) {
                selectedItem.Selected = true;
            }
            return result;
        }
        */


        public static List<NewsletterViewModel> GetNewsletterList(int CultureID = 0) {

             List<NewsletterViewModel> Newsletters = new List<NewsletterViewModel>();

            if (CultureID != 0) {
                Newsletters = (from n in DB.newsletter
                                  orderby n.DateCreated
                                  select new NewsletterViewModel {
                                      ID = n.Id,
                                      DateCreated = n.DateCreated,
                                      DateSent = (DateTime)n.DateSent,
                                      DateUpdated = (DateTime)n.DateUpdated,
                                      IntroText = n.IntroText,
                                      SubscriptionStatus = n.Audience.HasValue ? (NewsletterSubscriptionStatus) n.Audience : NewsletterSubscriptionStatus.OnlyToMembers,
                                      Title = n.Title
                                  }).ToList();
            } else {
                Newsletters = (from n in DB.newsletter
                                  orderby n.DateCreated
                                  select new NewsletterViewModel {
                                      ID = n.Id,
                                      DateCreated = n.DateCreated,
                                      DateSent = (DateTime)n.DateSent,
                                      DateUpdated = (DateTime)n.DateUpdated,
                                      IntroText = n.IntroText,
                                      Title = n.Title
                                  }).ToList();
            }

            return Newsletters;
        }

        public static NewsletterViewModel GetByID(int id) {
            newsletter nl;
            try {
                nl = DB.newsletter.Where(n => n.Id == id).Single();
            } catch (Exception) {
                return null;
            }
            List<newsletteritem> nlis = DB.newsletteritem.Where(nli => nli.NewsletterId == nl.Id).ToList();

            NewsletterViewModel nivm = new NewsletterViewModel {
                ID = nl.Id,
                DateCreated = nl.DateCreated,
                DateUpdated = nl.DateUpdated,
                DateSent = nl.DateSent,
                Title = nl.Title,
                IntroText = nl.IntroText,
                IncludeLoginLink = nl.AddPersonalLoginLink.HasValue && nl.AddPersonalLoginLink.Value,
                SubscriptionStatus = nl.Audience.HasValue ? (NewsletterSubscriptionStatus) nl.Audience.Value : NewsletterSubscriptionStatus.OnlyToMembers
            };

            nivm.Items = new List<NewsletterItemViewModel>();

            foreach (newsletteritem nli in nlis) {
                nivm.Items.Add(
                    new NewsletterItemViewModel {
                        ID = nli.NewsletterId,
                        SequenceNumber = nli.SequenceNumber,
                        SubTitle = nli.ItemSubTitle,
                        Title = nli.ItemTitle,
                        Text = nli.ItemText,
                        ImagePath = nli.PictureURL,
                        IconImagePath = nli.IconPictureURL,
                        HeadingHtmlColour = nli.HeadingHtmlColour
                    }
                );
            }

            return nivm;
        }

        public static void AddNewsletter(NewsletterViewModel nvm) {
            newsletter nl = new newsletter {
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                DateSent = null,
                AddPersonalLoginLink = nvm.IncludeLoginLink,
                IntroText = nvm.IntroText,
                Title = nvm.Title,
                Audience = (int) nvm.SubscriptionStatus
            };

            if (nvm.Items != null) {
                foreach(NewsletterItemViewModel nivm in nvm.Items) {
                    DB.AddTonewsletteritem(new newsletteritem() {
                        ItemTitle = nivm.Title,
                        ItemSubTitle = nivm.SubTitle,
                        SequenceNumber = nivm.SequenceNumber,
                        ItemText = nivm.Text,
                        PictureURL = nivm.ImagePath,
                        newsletter = nl
                    });
                }
            }

            DB.AddTonewsletter(nl);
            DB.SaveChanges();

            nvm.ID = nl.Id;
        }


        public static void UpdateNewsletter(NewsletterViewModel nvm) {
            newsletter nl = DB.newsletter.Where(n => n.Id == nvm.ID).Single();

            nl.DateUpdated = DateTime.Now;
            nl.DateSent = nvm.DateSent;
            nl.IntroText = nvm.IntroText;
            nl.Title = nvm.Title;
            nl.AddPersonalLoginLink = nvm.IncludeLoginLink;
            nl.Audience = (int) nvm.SubscriptionStatus;
            List<newsletteritem> nlis = DB.newsletteritem.Where(nli => nli.NewsletterId == nl.Id).ToList();

            foreach (newsletteritem nli in nlis) {
                DB.newsletteritem.DeleteObject(nli);
            }

            if (nvm.Items != null) {
                foreach (NewsletterItemViewModel nivm in nvm.Items) {
                    DB.AddTonewsletteritem(new newsletteritem() {
                        ItemTitle = nivm.Title,
                        ItemSubTitle = nivm.SubTitle,
                        SequenceNumber = nivm.SequenceNumber,
                        ItemText = nivm.Text,
                        PictureURL = nivm.ImagePath,
                        IconPictureURL = nivm.IconImagePath,
                        HeadingHtmlColour = nivm.HeadingHtmlColour,
                        newsletter = nl
                    });
                }
            }

            DB.SaveChanges();
        }

    }
}
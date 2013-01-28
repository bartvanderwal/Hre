using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using HRE.Models;
using HRE.Data;
using HRE.Dal;

namespace HRE.Models.Newsletters {
    public class NewsletterRepository : BaseRepository {

        public static List<NewsletterViewModel> GetNewsletterList(int CultureID = 0) {

             List<NewsletterViewModel> Newsletters = new List<NewsletterViewModel>();

            if (CultureID != 0) {
                Newsletters = (from n in DB.newsletter
                                  orderby n.DateCreated
                                  select new NewsletterViewModel {
                                      ID = n.Id,
                                      Created = n.DateCreated,
                                      Sent = (DateTime)n.DateSent,
                                      Updated = (DateTime)n.DateUpdated,
                                      SequenceNumber = n.SequenceNumber,
                                      Title = n.Title
                                  }).ToList();
            } else {
                Newsletters = (from n in DB.newsletter
                                  orderby n.DateCreated
                                  select new NewsletterViewModel {
                                      ID = n.Id,
                                      Created = n.DateCreated,
                                      Sent = (DateTime)n.DateSent,
                                      Updated = (DateTime)n.DateUpdated,
                                      SequenceNumber = n.SequenceNumber,
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
                Created = nl.DateCreated,
                Updated = nl.DateUpdated,
                Sent = nl.DateSent,
                Title = nl.Title
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
                SequenceNumber = nvm.SequenceNumber,
                Title = nvm.Title,
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
            nl.DateSent = nvm.Sent;
            nl.SequenceNumber = nvm.SequenceNumber;
            nl.Title = nvm.Title;
            nl.AddPersonalLoginLink = nvm.IncludeLoginLink;
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


        public List<LogonUserDal> DetermineAddressees() {
            // List<string> users = DB.logonuser.Where(c => c.IsMailingListMember.HasValue && c.IsMailingListMember.HasValue).Select(c => c.EmailAddress).ToList();
            // List<LogonUserDal> users = LogonUserDal.GetMailingListMembers();
            // List<string> addresses = new List<string>();
            return LogonUserDal.GetMailingListMembers();
            
            /*
            foreach (string user in users) {
                if (mu != null && !mu.IsLockedOut) {
                    // Profile profile = (Profile)Profile.GetUserProfile(mu.UserName);
                    addresses.Add(mu.Email);
                }
            }
            */
            // return addresses;
        }
    }
}
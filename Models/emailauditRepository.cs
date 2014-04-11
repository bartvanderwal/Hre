using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using HRE.Data;

namespace HRE.Models { 
    public class emailauditRepository : IemailauditRepository {
        
        HREContext context = new HREContext();
        
        public IQueryable<emailaudit> All
        {
            get { return context.emailaudits; }
        }

        public IQueryable<emailaudit> AllIncluding(params Expression<Func<emailaudit, object>>[] includeProperties)
        {
            IQueryable<emailaudit> query = context.emailaudits;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public emailaudit Find(int id)
        {
            return context.emailaudits.Find(id);
        }

        public void InsertOrUpdate(emailaudit emailaudit)
        {
            if (emailaudit.Id == default(int)) {
                // New entity
                context.emailaudits.Add(emailaudit);
            } else {
                // Existing entity
                context.Entry(emailaudit).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var emailaudit = context.emailaudits.Find(id);
            context.emailaudits.Remove(emailaudit);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface IemailauditRepository : IDisposable
    {
        IQueryable<emailaudit> All { get; }
        IQueryable<emailaudit> AllIncluding(params Expression<Func<emailaudit, object>>[] includeProperties);
        emailaudit Find(int id);
        void InsertOrUpdate(emailaudit emailaudit);
        void Delete(int id);
        void Save();
    }
}
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataAccess
{
    public class ContactDataAccess
    {
        OnlineShopDbContext db = null;
        public ContactDataAccess()
        {
            db = new OnlineShopDbContext();
        }

        public int InsertFeedBack(FeedBack feedback)
        {
            db.FeedBacks.Add(feedback);
            db.SaveChanges();
            return feedback.ID;
        }
    }
}

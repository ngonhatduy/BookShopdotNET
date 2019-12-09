using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataAccess
{
    public class MenuDataAccess
    {
        OnlineShopDbContext db = null;

        public MenuDataAccess()
        {
            db = new OnlineShopDbContext();
        }

        public List<Menu> ListByGroupId(int groupId)
        {
            return db.Menus.Where(x => x.TypeID == groupId).OrderBy(x => x.DisplayOrder).ToList();
        }
    }
}

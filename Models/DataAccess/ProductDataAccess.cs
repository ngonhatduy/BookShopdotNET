using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataAccess
{
    public class ProductDataAccess
    {
        OnlineShopDbContext db = null;
        public ProductDataAccess()
        {
            db = new OnlineShopDbContext();
        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public List<Product> ListAllNewProduct()
        {
            return db.Products.OrderByDescending(x => x.CreateDate).ToList();
        }
        public List<Product> ListAllFeatureProduct()
        {
            return db.Products.Where(x => x.TopHot != null).OrderByDescending(x => x.CreateDate).ToList();
        }
        
        public Product ViewProductDetail(long id)
        {
            return db.Products.Find(id);
        }

        public List<Product> ViewDetail(long id)
        {
            string code = db.Products.Find(id).Code;
            switch (code)
            {
                case "A01":
                    return db.Products.Where(x => x.Code == "A01").OrderByDescending(x => x.CreateDate).ToList();
                case "A02":
                    return db.Products.Where(x => x.Code == "A02").OrderByDescending(x => x.CreateDate).ToList();
                case "A03":
                    return db.Products.Where(x => x.Code == "A03").OrderByDescending(x => x.CreateDate).ToList();
                case "A04":
                    return db.Products.Where(x => x.Code == "A04").OrderByDescending(x => x.CreateDate).ToList();
                case "A05":
                    return db.Products.Where(x => x.Code == "A05").OrderByDescending(x => x.CreateDate).ToList();
                case "A06":
                    return db.Products.Where(x => x.Code == "A06").OrderByDescending(x => x.CreateDate).ToList();
                case "A07":
                    return db.Products.Where(x => x.Code == "A07").OrderByDescending(x => x.CreateDate).ToList();
                case "A08":
                    return db.Products.Where(x => x.Code == "A08").OrderByDescending(x => x.CreateDate).ToList();
                case "A09":
                    return db.Products.Where(x => x.Code == "A09").OrderByDescending(x => x.CreateDate).ToList();
                default:
                    return db.Products.ToList();
            }

        }

    }
}

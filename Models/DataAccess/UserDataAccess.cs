using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EntityFramework;
using System.Data.SqlClient;
using PagedList;
namespace Models.DataAccess
{
    public class UserDataAccess
    {
        OnlineShopDbContext db = null;

        public UserDataAccess()
        {
            db = new OnlineShopDbContext();
        }

        public bool ChangeStatus (long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }
        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                if(!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.UserName = entity.UserName;
                user.Name = entity.Name;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = user.UserName;
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                //logging
                return false;
            }
        }


        public IEnumerable<User> ListAllPaging(int page, int pageSize)
        {
            return db.Users.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }

        public User GetById(string UserName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == UserName);
        }

        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }
        public long Delete(User entity)
        {
            db.Users.Attach(entity);
            db.Users.Remove(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public int Login(string UserName, string PassWord)
        {
            //var query = from x in db.Users where x.UserName = userName && x.Password == password LinQ
            //var result = db.Users.Count(x => x.UserName == UserName && x.Password == PassWord); //Case này thì không xác định được trường hợp sai tên đăng nhập hay mật khẩu, nên phát triển tiếp case bên dưới.
            var result = db.Users.SingleOrDefault(x => x.UserName == UserName); // trả về null hoặc not null
            if (result == null)
            {
                return 0;
                //Tài khoản không tồn tại
            }
            else
            {
                if(result.Status == false)
                {
                    return -1;
                    //Tài khoản đang bị khóa
                }
                else
                {
                    if(result.Password == PassWord)
                    {
                        return 1;
                        //Nhập đúng mật khẩu
                    }
                    else
                    {
                        return -2;
                        //Nhập sai mật khẩu
                    }
                }
            }
        }
    }
}

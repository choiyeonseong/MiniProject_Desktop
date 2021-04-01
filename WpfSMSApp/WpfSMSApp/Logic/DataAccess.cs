using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSMSApp.Model;

namespace WpfSMSApp.Logic
{
    public class DataAccess
    {
        public static List<User> GetUsers()
        {
            List<User> users;

            using (var ctx = new SMSEntities())
            {
                users = ctx.User.ToList();  // SELECT * FROM User
            }
            return users;
        }

        /// <summary>
        /// 입력, 수정 동시에 수행
        /// </summary>
        /// <param name="user"></param>
        /// <returns> 0 또는 1 이상 </returns>
        public static int SetUsers(User user)
        {
            using (var ctx = new SMSEntities())
            {
                ctx.User.AddOrUpdate(user);
                return ctx.SaveChanges();   // commit
            }
        }

        public static List<Stock> GetStocks()
        {
            List<Stock> stocks;

            using (var ctx = new SMSEntities())
            {
                stocks = ctx.Stock.ToList();    // SELECT * FROM Store
            }
            return stocks;
        }

        public static List<Store> GetStores()
        {
            List<Store> stores;

            using (var ctx = new SMSEntities())
            {
                stores = ctx.Store.ToList();    // SELECT * FROM Store
            }
            return stores;
        }

        public static int SetStores(Store store)
        {
            using (var ctx = new SMSEntities())
            {
                ctx.Store.AddOrUpdate(store);
                return ctx.SaveChanges();   // commit
            }
        }
    }
}

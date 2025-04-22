using MVCAPIWithBearerToken.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MVCAPIWithBearerToken.Repositories
{
    //[Authorize]
    public class UserRepo:IDisposable
    {
        private readonly AppDbContext db=new AppDbContext();

        public void Dispose()
        {
            db.Dispose();
        }

        public User ValidateUser(string userName, string password)
        {
            return db.Users.FirstOrDefault(u => u.UserName.Equals(userName,
                StringComparison.OrdinalIgnoreCase) && u.Password == password);
        }
    }
}
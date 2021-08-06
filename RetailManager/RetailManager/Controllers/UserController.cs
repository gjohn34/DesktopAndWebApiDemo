using Microsoft.AspNet.Identity;
using RetailManager.Library.DataAccess;
using RetailManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RetailManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {

        // GET: api/User/
        public List<UserModel> GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();

            UserData sql = new UserData();
            List<UserModel> user = sql.GetUserById(userId);
            return user;
        }
    }
}

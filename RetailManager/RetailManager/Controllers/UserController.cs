using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RetailManager.Library.DataAccess;
using RetailManager.Library.Models;
using RetailManager.Models;
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
        public UserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();

            UserData sql = new UserData();
            UserModel user = sql.GetUserById(userId);
            return user;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/Users")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                List<ApplicationUser> users = userManager.Users.ToList();
                List<IdentityRole> roles = context.Roles.ToList();

                foreach (ApplicationUser user in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email
                    };
                    foreach (IdentityUserRole role in user.Roles)
                    {
                        u.Roles.Add(
                            role.RoleId, 
                            roles.Where(x => x.Id == role.RoleId).First().Name);
                    }
                    output.Add(u);
                }
            }
            return output;
        }
    }
}

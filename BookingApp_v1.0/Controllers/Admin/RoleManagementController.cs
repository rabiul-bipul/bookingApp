using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;
using BookingApp_v1._0.Models;

namespace BookingApp_v1._0.Controllers
{
    [Authorize(Roles = "Admin")] // Ensure only admins can access
    public class RoleManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: List of users with roles
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var userProfile = db.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }


            var users = db.Users.ToList();
            var roles = db.Roles.ToList();

            var model = users.Select(user => new ManageUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                CurrentRole = user.Roles.FirstOrDefault() != null ?
                              roles.FirstOrDefault(r => r.Id == user.Roles.FirstOrDefault().RoleId)?.Name : "No Role"
            }).ToList();

            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name"); // Populate roles dropdown

            return View(model);
        }

        // POST: Change the user's role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUserRole(string userId, string roleName)
        {
            /*string userId = User.Identity.GetUserId();*/
            var userProfile =db.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }


            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("", "Invalid user or role.");
                return RedirectToAction("Index");
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            // Remove user from existing roles
            var currentRoles = userManager.GetRoles(userId);
            if (currentRoles.Any())
            {
                userManager.RemoveFromRoles(userId, currentRoles.ToArray());
            }

            // Add user to the selected role
            userManager.AddToRole(userId, roleName);

            return RedirectToAction("Index");
        }
    }
}

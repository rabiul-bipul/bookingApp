using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BookingApp_v1._0.Models;
using Microsoft.AspNet.Identity;

namespace BookingApp_v1._0.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profile/Index
        public ActionResult Index()
        {
           
            var userId = User.Identity.GetUserId(); // Get the logged-in user's ID
            var profile = db.Profiles.FirstOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (profile != null)
            {
                ViewBag.UserName = profile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }


            if (profile == null)
            {
                // Handle case where profile does not exist (e.g., show a message)
                return RedirectToAction("Create", "Profile"); // Redirect to create a new profile
            }

            return View(profile); // Pass the profile to the view
        }

      


        // GET: Profile/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var userId = User.Identity.GetUserId(); // Get the logged-in user's ID
            var profilei = db.Profiles.FirstOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (profilei != null)
            {
                ViewBag.UserName = profilei.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var profile = await db.Profiles.FindAsync(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // GET: Profile/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var userId = User.Identity.GetUserId(); // Get the logged-in user's ID
            var profilei = db.Profiles.FirstOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (profilei != null)
            {
                ViewBag.UserName = profilei.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            if (id == null)
            {
                return HttpNotFound();
            }

            var profile = await db.Profiles.FindAsync(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<ActionResult> Edit(int id, Profile profile, HttpPostedFileBase ProfilePicture)
        {


            if (id != profile.ProfileId)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle the file upload
                    if (ProfilePicture != null && ProfilePicture.ContentLength > 0)
                    {
                        // Generate a unique file name
                        var fileName = Path.GetFileName(ProfilePicture.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/ProfilePictures"), fileName);

                        // Save the file
                        ProfilePicture.SaveAs(path);

                        // Set the profile picture path in the model
                        profile.ProfilePicture = $"/Content/ProfilePictures/{fileName}";
                    }

                    profile.ApplicationUserId = User.Identity.GetUserId();
                    db.Entry(profile).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.ProfileId))
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        // GET: Profile/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var userId = User.Identity.GetUserId(); // Get the logged-in user's ID
            var profilei = db.Profiles.FirstOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (profilei != null)
            {
                ViewBag.UserName = profilei.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            if (id == null)
            {
                return HttpNotFound();
            }

            var profile = await db.Profiles.FindAsync(id);
            if (profile == null)
            {
                return HttpNotFound();
            }

            return View(profile);
        }

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var userId = User.Identity.GetUserId(); // Get the logged-in user's ID
            var profilei = db.Profiles.FirstOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (profilei != null)
            {
                ViewBag.UserName = profilei.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var profile = await db.Profiles.FindAsync(id);
            if (profile != null)
            {
                db.Profiles.Remove(profile);
                await db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(int id)
        {
            return db.Profiles.Any(e => e.ProfileId == id);
        }
    }
}

using BookingApp_v1._0.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Mvc;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace BookingApp_v1._0.Controllers
{
    [Authorize(Roles = "Manager")]
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LocationsController() : this(new ApplicationDbContext()) { }
        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<ActionResult> Index()
        {
            // Get the current logged-in user's ID from AspNetUsers
            string userId = User.Identity.GetUserId();

            // Find the profile that belongs to the logged-in user by matching the ApplicationUserId
            var userProfile = _context.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var locations = await _context.Locations.ToListAsync();
            return View(locations);
        }

        // GET: Locations/Create
        [HttpGet]
        public ActionResult Create()
        {
            // Get the current logged-in user's ID from AspNetUsers
            string userId = User.Identity.GetUserId();

            // Find the profile that belongs to the logged-in user by matching the ApplicationUserId
            var userProfile = _context.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            return View();
        }

        // POST: Locations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Location location)
        {
            // Get the current logged-in user's ID from AspNetUsers
            string userId = User.Identity.GetUserId();

            // Find the profile that belongs to the logged-in user by matching the ApplicationUserId
            var userProfile = _context.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            if (ModelState.IsValid)
            {
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();
                TempData["LocationCreate"] = "New Location added successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Locations/Details/5
        public async Task<ActionResult> Details(int id)
        {
            // Get the current logged-in user's ID from AspNetUsers
            string userId = User.Identity.GetUserId();

            // Find the profile that belongs to the logged-in user by matching the ApplicationUserId
            var userProfile = _context.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            // Get the current logged-in user's ID from AspNetUsers
            string userId = User.Identity.GetUserId();

            // Find the profile that belongs to the logged-in user by matching the ApplicationUserId
            var userProfile = _context.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Location location)
        {
            // Get the current logged-in user's ID from AspNetUsers
            string userId = User.Identity.GetUserId();

            // Find the profile that belongs to the logged-in user by matching the ApplicationUserId
            var userProfile = _context.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            if (ModelState.IsValid)
            {
                _context.Entry(location).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["LocationEdit"] = "Location info updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Locations/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            // Get the current logged-in user's ID from AspNetUsers
            string userId = User.Identity.GetUserId();

            // Find the profile that belongs to the logged-in user by matching the ApplicationUserId
            var userProfile = _context.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }*/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            // Get the current logged-in user's ID from AspNetUsers
            string userId = User.Identity.GetUserId();

            // Find the profile that belongs to the logged-in user by matching the ApplicationUserId
            var userProfile = _context.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return HttpNotFound();
            }
            // Check if any hotels are associated with this location
            var hasHotels = _context.Hotels.Any(h => h.LocationId == id);
            if (hasHotels)
            {
                ModelState.AddModelError("", "Cannot delete this location because it has associated hotels.");
                return View(location); // Return the view with an error message
            }

            // Check if any packages are associated with this location
            var hasPackages = _context.Packages.Any(p => p.LocationId == id);
            if (hasPackages)
            {
                ModelState.AddModelError("", "Cannot delete this location because it has associated packages.");
                return View(location); // Return the view with an error message
            }

            // If no packages are associated, delete the location
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            TempData["LocationDelete"] = "Location info Deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

    }
}

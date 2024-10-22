using BookingApp_v1._0.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookingApp_v1._0.Controllers
{
    [Authorize(Roles = "Manager")] 
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HotelController()
        {
            _dbContext = new ApplicationDbContext(); // Ensure ApplicationDbContext is initialized
        }

        // GET: Hotels
        public async Task<ActionResult> Index()
        {
            
            string userId = User.Identity.GetUserId();
            var userProfile = _dbContext.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var hotels = await _dbContext.Hotels.Include(h => h.Location).ToListAsync();
            return View(hotels); // Return the list of hotels to the view
        }


        // GET: Hotels/Create
        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();
            var userProfile = _dbContext.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            ViewBag.LocationId = new SelectList(_dbContext.Locations, "LocationId", "LocationName");
            return View(); // Show the create hotel view
        }

        // POST: Hotels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Hotel hotel)
        {
            string userId = User.Identity.GetUserId();
            var userProfile = _dbContext.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);
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
                _dbContext.Hotels.Add(hotel); 
                await _dbContext.SaveChangesAsync(); 
                TempData["HotelCreate"] = "Hotel info Created successfully.";
                return RedirectToAction(nameof(Index)); 
            }
            ViewBag.LocationId = new SelectList(_dbContext.Locations, "LocationId", "LocationName", hotel.LocationId);
            return View(hotel); 
        }

        // GET: Hotels/Details/5
        public async Task<ActionResult> Details(int id)
        {
            string userId = User.Identity.GetUserId();
            var userProfile = _dbContext.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var hotel = await _dbContext.Hotels.Include(h => h.Location).FirstOrDefaultAsync(h => h.HotelId == id);
            if (hotel == null)
            {
                return HttpNotFound(); 
            }
            return View(hotel); 
        }

        // GET: Hotels/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            string userId = User.Identity.GetUserId();
            var userProfile = _dbContext.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var hotel = await _dbContext.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return HttpNotFound(); // Return a 404 if the hotel is not found
            }
            ViewBag.LocationId = new SelectList(_dbContext.Locations, "LocationId", "LocationName", hotel.LocationId);
            return View(hotel); // Return the edit view with the hotel data
        }

        // POST: Hotels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Hotel hotel)
        {
            string userId = User.Identity.GetUserId();
            var userProfile = _dbContext.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);
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
                _dbContext.Entry(hotel).State = EntityState.Modified; // Mark the hotel as modified
                await _dbContext.SaveChangesAsync(); // Save changes to the database
                TempData["HotelEdit"] = "Hotel info Updated successfully.";
                return RedirectToAction(nameof(Index)); // Redirect to the Index action
            }
            ViewBag.LocationId = new SelectList(_dbContext.Locations, "LocationId", "LocationName", hotel.LocationId);
            return View(hotel); // Return the view with the model if it's not valid
        }

        // GET: Hotels/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            string userId = User.Identity.GetUserId();
            var userProfile = _dbContext.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var hotel = await _dbContext.Hotels.Include(h => h.Location).FirstOrDefaultAsync(h => h.HotelId == id);
            if (hotel == null)
            {
                return HttpNotFound(); // Return a 404 if the hotel is not found
            }
            return View(hotel); // Return the delete confirmation view
        }


        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            string userId = User.Identity.GetUserId();
            var userProfile = _dbContext.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }


            var hotel = await _dbContext.Hotels.FindAsync(id);
            if (hotel != null)
            {
                var hasPackages = _dbContext.Packages.Any(p => p.HotelId == id);
                if (hasPackages)
                {
                    ModelState.AddModelError("", "Cannot delete this Hotel because it has associated packages.");
                    return View(hotel); // Return the view with an error message
                }
                _dbContext.Hotels.Remove(hotel); 
                await _dbContext.SaveChangesAsync(); 
                TempData["HotelDelete"] = "Hotel info Deleted !!";
            }
            return RedirectToAction(nameof(Index)); 
        }
    }
}

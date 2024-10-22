using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using BookingApp_v1._0.Models;
using Microsoft.AspNet.Identity;

namespace BookingApp_v1._0.Controllers
{

    public class PackageController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        private HttpClient client;

        public PackageController()
        {
            this.client = new HttpClient();
        }

        [Authorize(Roles = "Manager")]
        // GET: Package
        public ActionResult Index()
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

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"https://localhost:44390//api/packages");
                var response = client.GetAsync("packages");
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var data = response.Result.Content.ReadAsAsync<IEnumerable<Package>>().Result;
                    return View(data);
                }
                else
                    return HttpNotFound();
            }
        }



        [Authorize]
        // GET: Package/Details/5
        public ActionResult Details(int? id)
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

            client.BaseAddress = new Uri(@"https://localhost:44390/api/packages");
            var response = client.GetAsync("packages/" + id.ToString());
            response.Wait();

            if (response.Result.IsSuccessStatusCode)
            {
                var data = response.Result.Content.ReadAsAsync<Package>().Result;
                return View(data);
            }
            return HttpNotFound();
        }


        [Authorize(Roles = "Manager")]

        // GET: Package/Create
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

            ViewBag.HotelId = new SelectList(_dbContext.Hotels, "HotelId", "HotelName");
            ViewBag.LocationId = new SelectList(_dbContext.Locations, "LocationId", "LocationName");
            return View();
        }


        // POST: Package/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Package package)
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

            client.BaseAddress = new Uri(@"https://localhost:44390/api/addpackages");

            var response = client.PostAsJsonAsync("addpackages", package);

            response.Wait();

            if (response.Result.IsSuccessStatusCode)
            {
                TempData["PackageCreate"] = "Package info Created successfully.";
                return RedirectToAction("Index");
            }

            else
            {
                ViewBag.HotelId = new SelectList(_dbContext.Hotels, "HotelId", "HotelName", package.HotelId);
                ViewBag.LocationId = new SelectList(_dbContext.Locations, "LocationId", "LocationName", package.LocationId);
                return View(package);
            }
        }


        // GET: Package/Edit/5
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int? id)
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Fetch package from the database
            var package = _dbContext.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }

            // Set the ViewBag for dropdown lists
            ViewBag.HotelId = new SelectList(_dbContext.Hotels, "HotelId", "HotelName", package.HotelId);
            ViewBag.LocationId = new SelectList(_dbContext.Locations, "LocationId", "LocationName", package.LocationId);

            return View(package);
        }


        // POST: Package/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Package package)
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

            client.BaseAddress = new Uri(@"https://localhost:44390/api/updatepackages");
            var response = client.PutAsJsonAsync("updatepackages/" + package.PackageId.ToString(), package);
            response.Wait();

            if (response.Result.IsSuccessStatusCode)
            {
                TempData["PackageEdit"] = "Package info Updated successfully.";
                return RedirectToAction("Index");

            }

            else
            {
                ViewBag.HotelId = new SelectList(_dbContext.Hotels, "HotelId", "HotelName", package.HotelId);
                ViewBag.LocationId = new SelectList(_dbContext.Locations, "LocationId", "LocationName", package.LocationId);
                return View(package);
            }
        }


        // GET: Package/Delete/5
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int? id)
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            client.BaseAddress = new Uri("https://localhost:44390/api/packages");

            
            var response = client.GetAsync("packages/" + id.ToString());
            response.Wait();

           
            if (response.Result.IsSuccessStatusCode)
            {
               
                var package = response.Result.Content.ReadAsAsync<Package>().Result;

                if (package == null)
                {
                    return HttpNotFound();
                }

                
                return View(package);
            }

            // Return a not found result if the package does not exist or the API call fails
            return HttpNotFound();
        }


        // POST: Package/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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

            client.BaseAddress = new Uri(@"https://localhost:44390/api/deletepackages");
            var response = client.DeleteAsync("deletepackages/" + id.ToString());
            response.Wait();

            // Check if any hotels are associated with this location
            var invoice = _dbContext.Invoices.Any(h => h.PackageId == id);
            if (invoice)
            {
                ModelState.AddModelError("", "Cannot delete this Package because it has associated Invoice.");
                return View("Delete", _dbContext.Packages.Find(id));
            }

            if (response.Result.IsSuccessStatusCode)
            {
                TempData["PackageDelete"] = "Package info Deleted successfully.";
                return RedirectToAction("Index");
            }

            else
                return HttpNotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

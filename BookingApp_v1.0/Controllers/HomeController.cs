using BookingApp_v1._0.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace BookingApp_v1._0.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _dbContext;

        public HomeController()
        {
            _dbContext = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _dbContext.Dispose();
        }

        
        public ActionResult Index()
        {
            
            string userId = User.Identity.GetUserId();
            var userProfile = _dbContext.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  
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

                    // Sort the packages by ExpireDate in descending order
                    var sortedData = data.OrderByDescending(p => p.ExpireDate).ToList();
                    return View(sortedData);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }






        public ActionResult About()
        {
            // Get the current logged-in user's ID from AspNetUsers
            string userId = User.Identity.GetUserId();

            // Find the profile that belongs to the logged-in user by matching the ApplicationUserId
            var userProfile = _dbContext.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            // Get the current logged-in user's ID from AspNetUsers
            string userId = User.Identity.GetUserId();

            // Find the profile that belongs to the logged-in user by matching the ApplicationUserId
            var userProfile = _dbContext.Profiles.SingleOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Error()
        {
            return View(); // Ensure there is a corresponding Error.cshtml view
        }


        [Authorize]
        public ActionResult ViewAllPackages()
        {
            //to get profile name on top of view
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

            /*var packages = _dbContext.Packages
                                    .Include("Hotel")
                                    .Include("Location")
                                    .OrderByDescending(p => p.ExpireDate) // Sort by ExpireDate descending
                                    .ToList();
            return View(packages);*/
        }


        // GET: Home/ViewAllLocations
        [Authorize]
        public ActionResult ViewAllLocations()
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

            var locations = _dbContext.Locations.ToList(); // Fetch all locations from the database
            return View(locations); // Pass locations to the view
        }

        // GET: Home/ViewAllHotels
        [Authorize]
        public ActionResult ViewAllHotels()
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

            var hotels = _dbContext.Hotels.Include("Location").ToList(); // Fetch all hotels and include their locations
            return View(hotels); // Pass hotels to the view
        }
        // GET: Home/ViewAllProfiles
        [Authorize(Roles ="Admin")]
        public ActionResult ViewAllProfiles()
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

            var profiles = _dbContext.Profiles.ToList(); // Fetch all profiles from the database
            return View(profiles); // Pass profiles to the view
        }


    }
}
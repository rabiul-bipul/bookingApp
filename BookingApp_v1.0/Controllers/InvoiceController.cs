using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BookingApp_v1._0.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace BookingApp_v1._0.Controllers
{
    public class InvoiceController : Controller
    {
        private ApplicationDbContext _dbContext;

        public InvoiceController()
        {
            _dbContext = new ApplicationDbContext();
        }

        // GET: Invoice/Create
        public ActionResult Create(int packageId)
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

            var package = _dbContext.Packages.Find(packageId);

            if (userProfile == null)
            {
                return HttpNotFound("User profile not found."); // Handle missing profile case
            }

            if (package == null)
            {
                return HttpNotFound("Package not found."); // Handle missing package case
            }

            var invoice = new Invoice
            {
                PackageId = packageId,
                ProfileId = userProfile.ProfileId, // Assuming ProfileId is an int
                BookingDate = DateTime.Now,
                ArrivalDate = DateTime.Now.AddDays(1),
                TotalAmount = package.Price
            };

            return View(invoice);
        }

        // POST: Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Invoice invoice)
        {


            // Fetch the package by PackageId
            var package = _dbContext.Packages.Find(invoice.PackageId);
            if (package == null)
            {
                ModelState.AddModelError("", "Invalid package selected.");
                return View(invoice);
            }

            // Set BookingDate to the current time
            invoice.BookingDate = DateTime.Now;

            // Ensure that the package has not expired
            if (DateTime.Now > package.ExpireDate)
            {
                ModelState.AddModelError("", "The package has expired. Booking is not possible.");
                return View(invoice);
            }

            // Validate ArrivalDate
            if (invoice.ArrivalDate <= invoice.BookingDate)
            {
                ModelState.AddModelError("", "Arrival date must be later than the booking date.");
                return View(invoice);
            }

            // Check if the quantity exceeds the available stock
            if (invoice.Quantity > package.Stock)
            {
                ModelState.AddModelError("", $"The quantity cannot exceed the available stock of {package.Stock}.");
                return View(invoice);
            }

            // Calculate TotalAmount based on the package price and quantity
            invoice.TotalAmount = package.Price * invoice.Quantity;

            // Check if ModelState is valid
            if (ModelState.IsValid)
            {
                // Update package stock and sold quantities
                package.Stock -= invoice.Quantity;
                package.Sold += invoice.Quantity;

                // Add the invoice to the database
                _dbContext.Invoices.Add(invoice);

                // Save changes to both invoice and package
                _dbContext.SaveChanges();

                // Return the invoice ID as JSON
                /*return Json(new { InvoiceId = invoice.InvoiceId });*/
                // Redirect to the index or any other appropriate page after success
                
                return RedirectToAction("Details", "Invoice", new { id = invoice.InvoiceId });

            }

            // Log the errors if ModelState is invalid
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
            }

            // Return the form with validation errors
            return View(invoice);
        }

        
        // GET: Invoice/GeneratePDF/{id}
        public ActionResult GeneratePDF(int id)
        {
            var invoice = _dbContext.Invoices
                                    .Include("Package")
                                    .Include("Profile")
                                    .FirstOrDefault(i => i.InvoiceId == id);

            if (invoice == null)
            {
                return HttpNotFound("Invoice not found.");
            }

            // Generate PDF
            MemoryStream ms = new MemoryStream();
            Document document = new Document(PageSize.A4, 50, 50, 25, 25);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            document.Open();

            // Add title
            document.Add(new Paragraph("Invoice Details", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18)));
            document.Add(new Paragraph(" "));

            // Add invoice details
            document.Add(new Paragraph($"Invoice ID: {invoice.InvoiceId}"));
            document.Add(new Paragraph($"Booking Date: {invoice.BookingDate.ToString("MM/dd/yyyy")}"));
            document.Add(new Paragraph($"Arrival Date: {invoice.ArrivalDate.ToString("MM/dd/yyyy")}"));
            document.Add(new Paragraph($"Total Quantity: {invoice.Quantity.ToString()}"));
            document.Add(new Paragraph($"Total Amount: {invoice.TotalAmount.ToString("C")}"));
            document.Add(new Paragraph($"Payment Method: {invoice.PaymentMethod}"));
            document.Add(new Paragraph(" "));

            // Add profile details
            document.Add(new Paragraph("Profile Details:", FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));
            if (invoice.Profile != null)
            {
                document.Add(new Paragraph($"Profile Name: {invoice.Profile.Name}"));
                document.Add(new Paragraph($"Email: {invoice.Profile.Address}"));
                document.Add(new Paragraph($"Contact Number: {invoice.Profile.Contact}"));
            }
            else
            {
                document.Add(new Paragraph("No profile details available."));
            }
            document.Add(new Paragraph(" "));

            // Add package details
            document.Add(new Paragraph("Package Details:", FontFactory.GetFont(FontFactory.HELVETICA_BOLD)));
            if (invoice.Package != null)
            {
                document.Add(new Paragraph($"Package Name: {invoice.Package.PackageName}"));
                document.Add(new Paragraph($"Price: {invoice.Package.Price.ToString("C")}"));
                document.Add(new Paragraph($"Stock Remaining: {invoice.Package.Stock}"));
            }
            else
            {
                document.Add(new Paragraph("No package details available."));
            }

            document.Close();

            byte[] fileBytes = ms.ToArray();

            return File(fileBytes, "application/pdf", $"Invoice_{invoice.InvoiceId}.pdf");
        }



        // GET: Invoice/Details/{id}
        public ActionResult Details(int id)
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
            // Fetch the invoice along with related package and profile details
            var invoice = _dbContext.Invoices
                                    .Include("Package")
                                    .Include("Profile")
                                    .FirstOrDefault(i => i.InvoiceId == id);

            if (invoice == null)
            {
                return HttpNotFound("Invoice not found.");
            }

            return View(invoice); // Return the invoice details to the view
        }

        // GET: Invoice/Index
        /*    public ActionResult Index()
            {
                var invoices = _dbContext.Invoices
                                         .Include("Package")
                                         .Include("Profile")
                                         .ToList();
                return View(invoices);
            }
    */
        public ActionResult Index()
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

            var invoices = _dbContext.Invoices
                                     .Include("Package")
                                     .Include("Profile")
                                     .ToList();

            // Calculate daily total
            var dailyTotal = _dbContext.Invoices
                .Where(i => DbFunctions.TruncateTime(i.BookingDate) == DbFunctions.TruncateTime(DateTime.Now))
                .Sum(i => (double?)i.TotalAmount) ?? 0;

            // Calculate monthly total
            var currentMonth = DateTime.Now.Month;
            var monthlyTotal = _dbContext.Invoices
                .Where(i => i.BookingDate.Month == currentMonth && i.BookingDate.Year == DateTime.Now.Year)
                .Sum(i => (double?)i.TotalAmount) ?? 0;

            // Calculate yearly total
            var currentYear = DateTime.Now.Year;
            var yearlyTotal = _dbContext.Invoices
                .Where(i => i.BookingDate.Year == currentYear)
                .Sum(i => (double?)i.TotalAmount) ?? 0;

            // Calculate lifetime total (sum of all invoices)
            var lifetimeTotal = _dbContext.Invoices.Sum(i => (double?)i.TotalAmount) ?? 0;

            // Passing data to view
            ViewBag.DailyTotal = dailyTotal;
            ViewBag.MonthlyTotal = monthlyTotal;
            ViewBag.YearlyTotal = yearlyTotal;
            ViewBag.LifetimeTotal = lifetimeTotal;

            return View(invoices);
        }



        // GET: Invoice/IndexByProfile/{profileId}
        public ActionResult IndexByProfile()
        {
            var userId = User.Identity.GetUserId();
            var userProfile = _dbContext.Profiles.FirstOrDefault(p => p.ApplicationUserId == userId);

            // Pass the user's name to the view via ViewBag
            if (userProfile != null)
            {
                ViewBag.UserName = userProfile.Name;  // Using 'Name' from Profile
            }
            else
            {
                ViewBag.UserName = "Unknown User";
            }

            var invoices = _dbContext.Invoices
                                     .Include("Package")
                                     .Include("Profile")
                                     .Where(i => i.ProfileId == userProfile.ProfileId)
                                     .ToList();

            // Check if the profile has any invoices
          /*  if (!invoices.Any())
            {
                return HttpNotFound("No invoices found for this profile.");
            }*/

            return View(invoices);
        }

  /*      // GET: Invoice/Statistics
        public ActionResult Statistics()
        {
            // Calculate daily total
            var dailyTotal = _dbContext.Invoices
                .Where(i => DbFunctions.TruncateTime(i.BookingDate) == DbFunctions.TruncateTime(DateTime.Now))
                .Sum(i => (double?)i.TotalAmount) ?? 0;

            // Calculate monthly total
            var currentMonth = DateTime.Now.Month;
            var monthlyTotal = _dbContext.Invoices
                .Where(i => i.BookingDate.Month == currentMonth && i.BookingDate.Year == DateTime.Now.Year)
                .Sum(i => (double?)i.TotalAmount) ?? 0;

            // Calculate yearly total
            var currentYear = DateTime.Now.Year;
            var yearlyTotal = _dbContext.Invoices
                .Where(i => i.BookingDate.Year == currentYear)
                .Sum(i => (double?)i.TotalAmount) ?? 0;

            // Calculate lifetime total (sum of all invoices)
            var lifetimeTotal = _dbContext.Invoices.Sum(i => (double?)i.TotalAmount) ?? 0;

            // Passing data to view
            ViewBag.DailyTotal = dailyTotal;
            ViewBag.MonthlyTotal = monthlyTotal;
            ViewBag.YearlyTotal = yearlyTotal;
            ViewBag.LifetimeTotal = lifetimeTotal;

            return View();
        }
*/




        // GET: Invoice/DailyTotal
        public ActionResult DailyTotal()
        {
            // Get the current date (without time)
            var today = DateTime.Today;

            // Calculate the total amount for invoices created today
            var dailyTotal = _dbContext.Invoices
                                       .Where(i => DbFunctions.TruncateTime(i.BookingDate) == today)
                                       .Sum(i => (double?)i.TotalAmount) ?? 0;

            // Pass the daily total to the view using ViewBag or ViewModel
            ViewBag.DailyTotal = dailyTotal;

            return View();
        }

        // GET: Invoice/Last7DaysTotal
        public ActionResult Last7DaysTotal()
        {
            // Get the date 7 days ago
            var sevenDaysAgo = DateTime.Today.AddDays(-6); // Include today as day 1

            // Query to calculate total amount for each day in the last 7 days
            var last7DaysTotals = _dbContext.Invoices
                                            .Where(i => DbFunctions.TruncateTime(i.BookingDate) >= sevenDaysAgo)
                                            .GroupBy(i => DbFunctions.TruncateTime(i.BookingDate))
                                            .Select(g => new
                                            {
                                                Day = g.Key.Value,
                                                TotalAmount = g.Sum(i => (double?)i.TotalAmount) ?? 0
                                            })
                                            .ToList();

            // Pass the data to the view
            ViewBag.Last7DaysTotals = last7DaysTotals;

            return View();
        }


    }
}

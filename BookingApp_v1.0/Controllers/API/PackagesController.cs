using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BookingApp_v1._0.Models;

namespace BookingApp_v1._0.Controllers.Api
{
    public class PackageController : ApiController
    {
        private ApplicationDbContext _dbContext;

        public PackageController()
        {
            this._dbContext = new ApplicationDbContext();
        }

        // GET: api/packages
        [Route("api/packages")]
        [HttpGet]
        public IEnumerable<Package> GetPackages()
        {
            return _dbContext.Packages
                             .Include("Hotel")
                             .Include("Location")
                             .ToList();
        }

        [RoutePrefix("api/top-packages")]
        public class TopPackagesController : ApiController
        {
            private readonly ApplicationDbContext _dbContext;

            public TopPackagesController()
            {
                _dbContext = new ApplicationDbContext();
            }

            // GET: api/top-packages/6
            [Route("{count:int}")]
            [HttpGet]
            public IHttpActionResult GetTopPackages(int count)
            {
                // Get the top 'count' packages, including Hotel and Location details
                var packages = _dbContext.Packages
                                         .Include("Hotel")
                                         .Include("Location")
                                         .OrderByDescending(p => p.PackageId) // You can customize the sorting logic
                                         .Take(count)
                                         .ToList();

                if (packages == null || !packages.Any())
                {
                    return NotFound();
                }

                return Ok(packages);
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
    

    // GET: api/packages/{id}
    [Route("api/packages/{id}")]
        [HttpGet]
        public IHttpActionResult GetPackage(int id)
        {
            var package = _dbContext.Packages
                                    .Include("Hotel")
                                    .Include("Location")
                                    .FirstOrDefault(p => p.PackageId == id);

            if (package == null)
                return NotFound();

            return Ok(package);
        }

        // POST: api/addpackages
        [Route("api/addpackages")]
        [HttpPost]
        public IHttpActionResult AddPackage(Package package)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // Get the Hotel from the database using the HotelId from the package
            var hotel = _dbContext.Hotels.Include("Location").FirstOrDefault(h => h.HotelId == package.HotelId);

            if (hotel == null)
                return NotFound(); // If the HotelId doesn't exist, return 404

            // Set the LocationId based on the selected hotel
            package.LocationId = hotel.LocationId;

            // Initialize any default values
            package.Sold = 0;

            _dbContext.Packages.Add(package);
            _dbContext.SaveChanges();
            

            return Ok(package);
        }


        // PUT: api/updatepackages/{id}
        [Route("api/updatepackages/{id}")]
        [HttpPut]
        public IHttpActionResult UpdatePackage(int id, Package package)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var data = _dbContext.Packages.FirstOrDefault(p => p.PackageId == id);

            if (data == null)
                return NotFound();

            // Get the Hotel from the database using the HotelId from the package
            var hotel = _dbContext.Hotels.Include("Location").FirstOrDefault(h => h.HotelId == package.HotelId);

            if (hotel == null)
                return NotFound(); // If the HotelId doesn't exist, return 404

            // Update package properties
            data.PackageName = package.PackageName;
            data.Details = package.Details;
            data.ExpireDate = package.ExpireDate;
            data.Price = package.Price;
            data.Sold = data.Sold;  // Keep sold unchanged
            data.Stock = package.Stock;
            data.HotelId = package.HotelId;
            data.LocationId = hotel.LocationId;  // Set LocationId based on the selected Hotel

            _dbContext.SaveChanges();

            return Ok();
        }


        // DELETE: api/deletepackages/{id}
        [Route("api/deletepackages/{id}")]
        [HttpDelete]
        public IHttpActionResult DeletePackage(int id)
        {
            var data = _dbContext.Packages.FirstOrDefault(p => p.PackageId == id);

            if (data == null)
                return NotFound();

            _dbContext.Packages.Remove(data);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}

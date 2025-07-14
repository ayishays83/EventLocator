using EventLocator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{


public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            var latStr = HttpContext.Session.GetString("UserLat");
            var lngStr = HttpContext.Session.GetString("UserLng");

            bool hasLocation = !string.IsNullOrEmpty(latStr) && !string.IsNullOrEmpty(lngStr);
            ViewBag.LocationSet = hasLocation; // ✅ Pass this to Razor

            List<EventPlanner> planners = new();

            if (hasLocation)
            {
                double userLat = double.Parse(latStr);
                double userLng = double.Parse(lngStr);

                planners = _context.EventPlanners.ToList()
                    .Where(p => GetDistance(userLat, userLng, p.Latitude, p.Longitude) <= 50)
                    .ToList();
            }

            return View(planners);
        }

        [HttpPost]
        public IActionResult SetUserLocation([FromBody] LocationModel model)
        {
            HttpContext.Session.SetString("UserLat", model.Latitude.ToString());
            HttpContext.Session.SetString("UserLng", model.Longitude.ToString());
            return Ok();
        }

        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // KM
            var dLat = ToRad(lat2 - lat1);
            var dLon = ToRad(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double ToRad(double deg) => deg * (Math.PI / 180);
    }

}

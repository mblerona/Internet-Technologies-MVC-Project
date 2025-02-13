using JanuaryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity; 


namespace JanuaryProject.Controllers
{
    public class GalleryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Main Gallery - visible to all
        public ActionResult Index()
        {
            var paintings = db.Paintings
                .Include(p => p.Artist)
                .ToList();
            return View(paintings);
        }

        // MyGallery - only for logged in Artist
 
        public ActionResult MyGallery()
        {
            string userId = User.Identity.GetUserId();
            System.Diagnostics.Debug.WriteLine("Logged in User ID: " + userId);

            if (string.IsNullOrEmpty(userId))
            {
                return HttpNotFound("User ID is null or empty.");
            }

            var artist = db.Artists.FirstOrDefault(a => a.UserId == userId);

            // Log the artist found
            System.Diagnostics.Debug.WriteLine("Artist found: " + (artist != null ? artist.Name + " " + artist.LastName : "Not Found"));

            if (artist == null)
            {
                return HttpNotFound("Artist not found.");
            }

            var paintings = db.Paintings
                .Include(p => p.Artist)
                .Where(p => p.ArtistId == artist.Id)
                .ToList();

            // Log the number of paintings to debug
            System.Diagnostics.Debug.WriteLine("Number of paintings: " + paintings.Count);

            ViewBag.ArtistName = artist.Name + " " + artist.LastName;
            return View(paintings);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
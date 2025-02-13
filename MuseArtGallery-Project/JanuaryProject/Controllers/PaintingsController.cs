using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JanuaryProject.Models;
using Microsoft.AspNet.Identity;

namespace JanuaryProject.Controllers
{



    public class PaintingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Paintings
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var paintings = db.Paintings.Include(p => p.Artist);
            return View(paintings.ToList());
        }

        // GET: Paintings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Painting painting = db.Paintings.Find(id);
            if (painting == null)
            {
                return HttpNotFound();
            }
            return View(painting);
        }

        // GET: Paintings/Create
        [Authorize(Roles = "Artist")]
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name");
            return View();
        }

        // POST: Paintings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PaintingId,Name,Description,Dimensions,Price,ImageUrl,ArtistId")] Painting painting)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Paintings.Add(painting);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", painting.ArtistId);
        //    return View(painting);
        //}

        [HttpPost]
        [Authorize(Roles = "Artist")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Dimensions,Price,ImageUrl")] Painting painting)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                var artist = db.Artists.FirstOrDefault(a => a.UserId == userId);

                if (artist == null)
                    return HttpNotFound();

                painting.ArtistId = artist.Id;
                db.Paintings.Add(painting);
                db.SaveChanges();
                return RedirectToAction("MyGallery", "Gallery");
            }

            return View(painting);
        }


        // GET: Paintings/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Painting painting = db.Paintings.Find(id);
        //    if (painting == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", painting.ArtistId);
        //    return View(painting);
        //}

        //// POST: Paintings/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "PaintingId,Name,Description,Dimensions,Price,ImageUrl,ArtistId")] Painting painting)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(painting).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ArtistId = new SelectList(db.Artists, "Id", "Name", painting.ArtistId);
        //    return View(painting);
        //}

        // Edit action - only for the Artist who owns the painting
        [Authorize(Roles = "Artist")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string userId = User.Identity.GetUserId();
            var artist = db.Artists.FirstOrDefault(a => a.UserId == userId);

            if (artist == null)
                return HttpNotFound();

            Painting painting = db.Paintings.Find(id);

            if (painting == null || painting.ArtistId != artist.Id)
                return HttpNotFound();

            return View(painting);
        }

        [HttpPost]
        [Authorize(Roles = "Artist")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaintingId,Name,Description,Dimensions,Price,ImageUrl,ArtistId")] Painting painting)
        {
            string userId = User.Identity.GetUserId();
            var artist = db.Artists.FirstOrDefault(a => a.UserId == userId);

            if (artist == null || painting.ArtistId != artist.Id)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                db.Entry(painting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyGallery", "Gallery");
            }
            return View(painting);
        }

        // GET: Paintings/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Painting painting = db.Paintings.Find(id);
        //    if (painting == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(painting);
        //}

        //// POST: Paintings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Painting painting = db.Paintings.Find(id);
        //    db.Paintings.Remove(painting);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        // Delete action - only for the Artist who owns the painting


        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string userId = User.Identity.GetUserId();
            var artist = db.Artists.FirstOrDefault(a => a.UserId == userId);

            if (artist == null)
                return HttpNotFound();

            Painting painting = db.Paintings.Find(id);

            if (painting == null || painting.ArtistId != artist.Id)
                return HttpNotFound();

            return View(painting);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Artist")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string userId = User.Identity.GetUserId();
            var artist = db.Artists.FirstOrDefault(a => a.UserId == userId);

            if (artist == null)
                return HttpNotFound();

            Painting painting = db.Paintings.Find(id);

            if (painting == null || painting.ArtistId != artist.Id)
                return HttpNotFound();

            db.Paintings.Remove(painting);
            db.SaveChanges();
            return RedirectToAction("MyGallery", "Gallery");
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

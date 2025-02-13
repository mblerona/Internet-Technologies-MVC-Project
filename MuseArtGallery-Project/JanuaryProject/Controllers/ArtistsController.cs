using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JanuaryProject.Models;
using static System.Net.Mime.MediaTypeNames;

namespace JanuaryProject.Controllers
{



    public class ArtistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Artists
        //public ActionResult Index()
        //{
        //    return View(db.Artists.ToList());
        //}
        [Authorize(Roles = "Admin")]


        public ActionResult Index()
        {
            var artistRoleId = db.Roles
               .Where(r => r.Name == "Artist")
               .Select(r => r.Id)
               .FirstOrDefault();

            if (artistRoleId == null)
            {
                return HttpNotFound("Artist role not found.");
            }

            // Retrieve all users that belong to the Artist role
            var users = db.Users
                .Where(u => u.Roles.Any(r => r.RoleId == artistRoleId))
                .ToList(); // Execute the query to fetch users

            // Now map the users to your custom Artist model
            var artists = users.Select(u => new Artist
            {
                UserId = u.Id,
                Name = u.UserName // Adjust this according to the actual properties of the Artist class
            }).ToList();

            return View(artists);
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Artist artist = db.Artists.Find(id);
            Artist artist = db.Artists.Include(a => a.Paintings).FirstOrDefault(a => a.Id == id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }
        [Authorize(Roles = "Admin")]

        // GET: Artists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Create([Bind(Include = "Id,Name,LastName,UserId")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult Edit([Bind(Include = "Id,Name,LastName,UserId")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artists.Find(id);
            db.Artists.Remove(artist);
            db.SaveChanges();
            return RedirectToAction("Index");
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

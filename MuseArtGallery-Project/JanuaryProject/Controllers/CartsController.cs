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
    [Authorize(Roles = "Customer")]
    public class CartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Carts
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var customer = db.Customers.FirstOrDefault(c => c.UserId == userId);

            if (customer == null)
                return HttpNotFound();

            var cartItems = db.Carts
                .Include(c => c.Painting)
                .Include(c => c.Painting.Artist)
                .Where(c => c.CustomerId == customer.CustomerId)
                .ToList();

            return View(cartItems);
        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name");
            ViewBag.PaintingId = new SelectList(db.Paintings, "PaintingId", "Name");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartId,CustomerId,PaintingId,DateAdded")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", cart.CustomerId);
            ViewBag.PaintingId = new SelectList(db.Paintings, "PaintingId", "Name", cart.PaintingId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", cart.CustomerId);
            ViewBag.PaintingId = new SelectList(db.Paintings, "PaintingId", "Name", cart.PaintingId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartId,CustomerId,PaintingId,DateAdded")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Name", cart.CustomerId);
            ViewBag.PaintingId = new SelectList(db.Paintings, "PaintingId", "Name", cart.PaintingId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Cart cart = db.Carts.Find(id);
        //    if (cart == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cart);
        //}














        //[Authorize(Roles = "Customer")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id)
        //{
        //    string userId = User.Identity.GetUserId();
        //    var customer = db.Customers.FirstOrDefault(c => c.UserId == userId);

        //    if (customer == null)
        //        return HttpNotFound();

        //    var cartItem = db.Carts.Find(id);
        //    if (cartItem == null || cartItem.CustomerId != customer.CustomerId)
        //        return HttpNotFound();

        //    db.Carts.Remove(cartItem);
        //    db.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        //// POST: Carts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Cart cart = db.Carts.Find(id);
        //    db.Carts.Remove(cart);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}







        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //[Authorize(Roles = "Customer")]
        //public ActionResult MyCart()
        //{
        //    string userId = User.Identity.GetUserId();
        //    var customer = db.Customers.FirstOrDefault(c => c.UserId == userId);

        //    if (customer == null)
        //        return HttpNotFound();

        //    var cartItems = db.Carts
        //        .Include(c => c.Painting)
        //        .Include(c => c.Painting.Artist)
        //        .Where(c => c.CustomerId == customer.CustomerId)
        //        .ToList();

        //    return View(cartItems);
        //}

        // POST: Carts/AddToCart/5 (5 being the painting ID)
        // AddToCart - only for logged in Customer
        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int paintingId)
        {
            string userId = User.Identity.GetUserId();
            var customer = db.Customers.FirstOrDefault(c => c.UserId == userId);

            if (customer == null)
                return HttpNotFound();

            // Check if painting already in cart
            var existingItem = db.Carts.FirstOrDefault(c =>
                c.CustomerId == customer.CustomerId &&
                c.PaintingId == paintingId);

            if (existingItem != null)
            {
                TempData["Message"] = "This painting is already in your cart.";
                return RedirectToAction("Index");
            }

            var cart = new Cart
            {
                CustomerId = customer.CustomerId,
                PaintingId = paintingId,
                DateAdded = DateTime.Now
            };

            db.Carts.Add(cart);
            db.SaveChanges();

            return RedirectToAction("Index");
        }




        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            string userId = User.Identity.GetUserId();
            var customer = db.Customers.FirstOrDefault(c => c.UserId == userId);

            if (customer == null)
                return HttpNotFound();

            var cartItem = db.Carts.Find(id);
            if (cartItem == null || cartItem.CustomerId != customer.CustomerId)
                return HttpNotFound();

            db.Carts.Remove(cartItem);
            db.SaveChanges();

            TempData["Message"] = "Item removed from cart successfully.";
            return RedirectToAction("Index");
        }

    }
}

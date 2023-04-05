using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;

namespace QLKS.Areas.Admin.Controllers.Admin
{
    public class TangController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();

     
        public ActionResult Index()
        {
            return View(db.tblTangs.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTang tblTang = db.tblTangs.Find(id);
            if (tblTang == null)
            {
                return HttpNotFound();
            }
            return View(tblTang);
        }

      
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ma_tang,ten_tang")] tblTang tblTang)
        {
            if (ModelState.IsValid)
            {
                db.tblTangs.Add(tblTang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblTang);
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTang tblTang = db.tblTangs.Find(id);
            if (tblTang == null)
            {
                return HttpNotFound();
            }
            return View(tblTang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma_tang,ten_tang")] tblTang tblTang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblTang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblTang);
        }

     
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTang tblTang = db.tblTangs.Find(id);
            if (tblTang == null)
            {
                return HttpNotFound();
            }
            return View(tblTang);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                tblTang tblTang = db.tblTangs.Find(id);
                db.tblTangs.Remove(tblTang);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

       
    }
}

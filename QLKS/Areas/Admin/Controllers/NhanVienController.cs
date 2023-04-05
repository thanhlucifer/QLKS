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
    public class NhanVienController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();

       
        public ActionResult Index()
        {
            var tblNhanViens = db.tblNhanViens.Include(t => t.tblChucVu);
            return View(tblNhanViens.ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblNhanVien tblNhanVien = db.tblNhanViens.Find(id);
            if (tblNhanVien == null)
            {
                return HttpNotFound();
            }
            return View(tblNhanVien);
        }

       
        public ActionResult Create()
        {
            ViewBag.ma_chuc_vu = new SelectList(db.tblChucVus, "ma_chuc_vu", "chuc_vu");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ma_nv,ho_ten,ngay_sinh,dia_chi,sdt,tai_khoan,mat_khau,ma_chuc_vu")] tblNhanVien tblNhanVien)
        {
            if (ModelState.IsValid)
            {
                db.tblNhanViens.Add(tblNhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ma_chuc_vu = new SelectList(db.tblChucVus, "ma_chuc_vu", "chuc_vu", tblNhanVien.ma_chuc_vu);
            return View(tblNhanVien);
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblNhanVien tblNhanVien = db.tblNhanViens.Find(id);
            if (tblNhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.ma_chuc_vu = new SelectList(db.tblChucVus, "ma_chuc_vu", "chuc_vu", tblNhanVien.ma_chuc_vu);
            return View(tblNhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma_nv,ho_ten,ngay_sinh,dia_chi,sdt,tai_khoan,mat_khau,ma_chuc_vu")] tblNhanVien tblNhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblNhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ma_chuc_vu = new SelectList(db.tblChucVus, "ma_chuc_vu", "chuc_vu", tblNhanVien.ma_chuc_vu);
            return View(tblNhanVien);
        }

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblNhanVien tblNhanVien = db.tblNhanViens.Find(id);
            if (tblNhanVien == null)
            {
                return HttpNotFound();
            }
            return View(tblNhanVien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                tblNhanVien tblNhanVien = db.tblNhanViens.Find(id);
                db.tblNhanViens.Remove(tblNhanVien);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

        
    }
}

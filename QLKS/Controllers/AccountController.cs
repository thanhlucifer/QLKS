using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;


namespace QLKS.Controllers.Home
{
    public class AccountController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();
       
        public ActionResult Index()
        {
            return View(db.tblKhachHangs.ToList());
        }

       

     
        public ActionResult Register()
        {
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "ma_kh,mat_khau,ho_ten,cmt,sdt,mail")] tblKhachHang tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                if (db.tblKhachHangs.Find(tblKhachHang.ma_kh) == null)
                {
                    db.tblKhachHangs.Add(tblKhachHang);
                    db.SaveChanges();
                    Session["KH"] = tblKhachHang;
                    return RedirectToAction("BookRoom", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên tài khoản đã được sử dụng !");
                }
            }

            return View(tblKhachHang);
        }

        


        public ActionResult CaNhan()
        {
            tblKhachHang kh = new tblKhachHang();
            if (Session["KH"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                kh = (tblKhachHang)Session["KH"];
            }
            return View(kh);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaNhan([Bind(Include = "ma_kh,mat_khau,ho_ten,cmt,sdt,mail,diem")] tblKhachHang tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKhachHang).State = EntityState.Modified;
                db.SaveChanges();
                Session["KH"] = tblKhachHang;
                return RedirectToAction("Index", "Home");
            }
            return View(tblKhachHang);
        }

        

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tblKhachHang objUser)
        {
            if (ModelState.IsValid)
            {
                var obj = db.tblKhachHangs.Where(a => a.ma_kh.Equals(objUser.ma_kh) && a.mat_khau.Equals(objUser.mat_khau)).FirstOrDefault();
                var obj1 = db.tblNhanViens.Where(a => a.tai_khoan.Equals(objUser.ma_kh) && a.mat_khau.Equals(objUser.mat_khau)).FirstOrDefault();
                if (obj1 != null)
                {
                    Session["NV"] = obj1;
                    return RedirectToAction("Index", "Admin");

                  
                }
                   
                else
                {
                    if (obj != null)
                    {
                        Session["KH"] = obj;
                        return RedirectToAction("BookRoom", "Home");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Login data is incorrect!");

                    }
                    
                }

                

            }
            return View(objUser);
        }
        [HttpGet]
        public ActionResult Login()
        {
            Session["KH"] = null;
            tblKhachHang kh = (tblKhachHang)Session["KH"];
            if (kh != null)
                return RedirectToAction("BookRoom", "Home");
            return View();
        }



        
        public ActionResult XoaPhieuDatPhong(int? id)
        {
            tblKhachHang kh = new tblKhachHang();
            if (Session["KH"] != null)
                kh = (tblKhachHang)Session["KH"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblPhieuDatPhong tblPhieuDatPhong = db.tblPhieuDatPhongs.Find(id);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            if (tblPhieuDatPhong.ma_kh != kh.ma_kh)
                return RedirectToAction("Index", "Home");
            return View(tblPhieuDatPhong);
        }

        
        [HttpPost, ActionName("XoaPhieuDatPhong")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmXoaPhieuDatPhong(int id)
        {
            tblPhieuDatPhong tblPhieuDatPhong = db.tblPhieuDatPhongs.Find(id);
            tblPhieuDatPhong.ma_tinh_trang = 3;
            db.Entry(tblPhieuDatPhong).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("BookRoom", "Home");
        }

        public ActionResult Logout()
        {
            Session["KH"] = null;
            return RedirectToAction("Login", "Account");
        }





        public ActionResult HoaDon()
        {
            tblKhachHang kh = new tblKhachHang();
            if (Session["KH"] != null)
                kh = (tblKhachHang)Session["KH"];
            else
                return RedirectToAction("Index", "Home");

            var dsHoaDon = db.tblHoaDons.Where(t => t.tblPhieuDatPhong.ma_kh == kh.ma_kh).ToList();
            return View(dsHoaDon);
        }
        public ActionResult PhieuDatPhong()
        {
           
            tblKhachHang kh = new tblKhachHang();
            if (Session["KH"] != null)
                kh = (tblKhachHang)Session["KH"];
            else
                return RedirectToAction("Index", "Home");

            var dsPDP = db.tblPhieuDatPhongs.Where(t => t.ma_kh == kh.ma_kh).ToList();
            return View(dsPDP);
        }
        
        public ActionResult ChiTietHoaDon(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblHoaDon tblHoaDon = db.tblHoaDons.Find(id);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }

            var tien_phong = (tblHoaDon.tblPhieuDatPhong.ngay_ra - tblHoaDon.tblPhieuDatPhong.ngay_vao).Value.TotalDays * tblHoaDon.tblPhieuDatPhong.tblPhong.tblLoaiPhong.gia;
            ViewBag.tien_phong = tien_phong;

            ViewBag.time_now = DateTime.Now.ToString();

            List<tblDichVuDaDat> dsdv = db.tblDichVuDaDats.Where(u => u.ma_hd == id).ToList();
            ViewBag.list_dv = dsdv;
            double tongtiendv = 0;
            List<double> tt = new List<double>();
            foreach (var item in dsdv)
            {
                double t = (double)(item.so_luong * item.tblDichVu.gia);
                tongtiendv += t;
                tt.Add(t);
            }
            ViewBag.list_tt = tt;
            ViewBag.tien_dich_vu = tongtiendv;
            ViewBag.tong_tien = tien_phong + tongtiendv;
            return View(tblHoaDon);
        }
    }
}

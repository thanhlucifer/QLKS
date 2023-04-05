using QLKS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QLKS.Areas.Admin  .Controllers.Admin
{
    public class ThongKeController : Controller
    {
        dataQLKSEntities db = new dataQLKSEntities();

        public ActionResult Index()
        {
            Tab();
            return View();
        }
        
        private void Tab()
        {
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var tong = db.tblHoaDons.Where(t => t.ma_tinh_trang == 2 && t.ngay_tra_phong >= firstDayOfMonth && t.ngay_tra_phong <= lastDayOfMonth).Sum(t => t.tong_tien);
            if (tong != null)
                ViewBag.tien_ht = String.Format("{0:0,0.00}", tong);
            else
                ViewBag.tien_ht = "0";

            ViewBag.so_hoa_don = db.tblHoaDons.Count();
            ViewBag.so_phieu_dat_phong = db.tblPhieuDatPhongs.Where(u => u.ma_tinh_trang == 1).Count();
            ViewBag.so_phong_dang_dat = db.tblPhongs.Where(u => u.ma_tinh_trang == 2).Count();
            ViewBag.so_dich_vu = db.tblDichVus.Count();
        }
       
    }
}
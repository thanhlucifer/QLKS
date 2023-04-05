using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKS.Controllers
{
    public class BangGiaController : Controller
    {
        
        QLKS.Models.dataQLKSEntities db = new Models.dataQLKSEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GiaPhong()
        {
            return View(db.tblLoaiPhongs.ToList());
        }
        public ActionResult GiaDichVu()
        {
            return View(db.tblDichVus.ToList());
        }
    }
}
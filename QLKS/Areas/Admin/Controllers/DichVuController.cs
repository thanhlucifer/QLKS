using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;

namespace QLKS.Areas.Admin.Controllers
{
    public class DichVuController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();

        
        public ActionResult Index()
        {
            return View(db.tblDichVus.ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDichVu tblDichVu = db.tblDichVus.Find(id);
            if (tblDichVu == null)
            {
                return HttpNotFound();
            }
            return View(tblDichVu);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, [Bind(Include = "ma_dv,ten_dv,gia,don_vi,ton_kho")] tblDichVu tblDichVu)
        {
            if (ModelState.IsValid)
            {
                String anh = "/Content/Images/DichVu/default.png";
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);//Trả về tên tệp và phần mở rộng của chuỗi đường dẫn đã chỉ định.
                    String path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images/DichVu"), pic);//Kết hợp hai chuỗi thành một đường dẫn.
                    // file is uploaded
                    file.SaveAs(path);
                    anh = "/Content/Images/DichVu/" + pic;
                    // lưu đường dẫn hình ảnh vào cơ sở dữ liệu 
                    // trực tiếp vào cơ sở dữ liệu trong trường hợp nếu bạn muốn lưu trữ byte[]
                    
                    using (MemoryStream ms = new MemoryStream())//Tạo một luồng có kho lưu trữ dự phòng là bộ nhớ.
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }

                tblDichVu.anh = anh;
                db.tblDichVus.Add(tblDichVu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblDichVu);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDichVu tblDichVu = db.tblDichVus.Find(id);
            if (tblDichVu == null)
            {
                return HttpNotFound();
            }
            return View(tblDichVu);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]//ngăn chặn các giả mạo yêu cầu trang web chéo
        public ActionResult Edit(HttpPostedFileBase file, [Bind(Include = "ma_dv,ten_dv,gia,don_vi,ton_kho")] tblDichVu tblDichVu)
        {
            tblDichVu dv = db.tblDichVus.Find(tblDichVu.ma_dv);
            if (ModelState.IsValid)//kiểm tra xem mô hình có hợp lệ cho cơ sở dữ liệu hay không 
            {
                String anh = dv.anh;
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);//Trả về tên tệp và phần mở rộng của chuỗi đường dẫn đã chỉ định.
                    String path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images/DichVu"), pic);//Kết hợp hai chuỗi thành một đường dẫn.
                    
                    file.SaveAs(path);
                    anh = "/Content/Images/DichVu/" + pic;
                    
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }

                dv.anh = anh;
                dv.don_vi = tblDichVu.don_vi;
                dv.gia = tblDichVu.gia;
                dv.ten_dv = tblDichVu.ten_dv;
                db.Entry(dv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblDichVu);
        }

 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDichVu tblDichVu = db.tblDichVus.Find(id);
            if (tblDichVu == null)
            {
                return HttpNotFound();
            }
            return View(tblDichVu);
        }
               
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] //ngăn chặn các giả mạo yêu cầu trang web chéo
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                tblDichVu tblDichVu = db.tblDichVus.Find(id);
                db.tblDichVus.Remove(tblDichVu);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

        
    }
}

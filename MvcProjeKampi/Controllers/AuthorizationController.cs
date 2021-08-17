using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    [Authorize(Roles = "B")]
    public class AuthorizationController : Controller
    {
        AdminManager adm = new AdminManager(new EfAdminDal());
        // GET: Authorization
        public ActionResult Index()
        {
            var values = adm.GetList();
            return View(values);
        }
        [HttpGet]
        public ActionResult AddAdmin()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult AddAdmin(Admin p)
        {
            adm.AdminAdd(p);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditAdmin(int id)
        {
            List<SelectListItem> valueStatus = (from x in adm.GetList()
                                                select new SelectListItem
                                                {
                                                    Text = x.AdminRole,
                                                    Value = x.AdminId.ToString()
                                                }
                                                     ).ToList();
            ViewBag.vlc = valueStatus;
            var value = adm.GetById(id);
            return View(value);
        }
        [HttpPost]
        public ActionResult EditAdmin(Admin p)
        {
            adm.AdminUpdate(p);
            return RedirectToAction("Index");
        }
        public ActionResult PasifYap(int id)
        {
            var adminValue = adm.GetById(id);
            if (adminValue.AdminStatus == true)
            {
                adminValue.AdminStatus = false;
            }
            else
            {
                adminValue.AdminStatus = true;
            }
           
            adm.AdminUpdate(adminValue);
            return RedirectToAction("Index");
        }
    }
}
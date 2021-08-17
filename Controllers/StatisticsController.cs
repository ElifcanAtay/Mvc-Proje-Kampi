using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    [Authorize(Roles = "B")]
    public class StatisticsController : Controller
    {
        // GET: Statistics

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Statistics()
        {
            Context c = new Context();
            var deger1 = c.Categories.Count();
            ViewBag.d1 = deger1;


            var categoryId = c.Categories.Where(x => x.CategoryName == "Yazılım").Select(y => y.CategoryId).FirstOrDefault();
            var deger2 = c.Headings.Where(x => x.CategoryId == categoryId).Count(); 
            ViewBag.d2 = deger2;

            var deger3 = c.Writers.Where(x => x.WriterName.Contains("a")).Count();
            ViewBag.d3 = deger3;


           var maxCategory= c.Headings.OrderByDescending(x => x.CategoryId).Select(y => y.CategoryId).FirstOrDefault();
            var deger4 = c.Categories.Where(x => x.CategoryId == maxCategory).Select(y => y.CategoryName).FirstOrDefault();
            ViewBag.d4 = deger4;

            var statusTure = c.Categories.Where(x => x.CategoryStatus == true).Count();
            var statusFalse = c.Categories.Where(x => x.CategoryStatus == false).Count();
            int deger5 = (statusTure - statusFalse);
            ViewBag.d5 = deger5;

          
            return View();
        }
    }
}
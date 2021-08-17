using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    public class HomeController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterDal());
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Test()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult HomePage()
        {

            Context c = new Context();

            
            var deger1 = c.Headings.Count();
            ViewBag.d1 = deger1;

            var deger2 = c.Contents.Count();
            ViewBag.d2 = deger2;

            var deger3 = c.Writers.Count();
            ViewBag.d3 = deger3;

            var deger4 = c.Messages.Count();
            ViewBag.d4 = deger4;
           
            return View();
        }
    }
}
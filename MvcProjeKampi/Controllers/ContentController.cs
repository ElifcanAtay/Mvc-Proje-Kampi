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
    [Authorize(Roles = "B")]
    public class ContentController : Controller
    {
        ContentManager cm = new ContentManager(new EfContentDal());

        public ActionResult Index()
        {
            return View();
        }
      

        
        public ActionResult GetAllContent(string p)
        {

            var values = cm.GetList(p);
            if (p==null)
            {
                var list = cm.GetAllList();
                
                return View(list);
            }
           

           // var values = c.Contents.ToList();
            return View(values);
        }
        
        public ActionResult ContentByHeading(int id)
        {
            var contenValue = cm.GetListbyHeading(id);
            return View(contenValue);
        }
    }
}
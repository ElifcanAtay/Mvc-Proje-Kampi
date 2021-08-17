using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{

    public class WriterPanelController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        WriterValidator writerValidator = new WriterValidator();

        Context c = new Context();
        int id;


        public ActionResult WriterProfile(string p)
        {
            p = (string)Session["WriterMail"];
            var writerIdInfo = c.Writers.Where(x => x.WriterMail == p).Select(y => y.WriterId).FirstOrDefault();
            var writer = wm.GetListByWriter(writerIdInfo);
            return View(writer);
        }
        [HttpGet]
        public ActionResult WriterProfileEdit(int id)
        {
            var writerValue = wm.GetById(id);
            return View(writerValue);
            
        }
        [HttpPost]
        public ActionResult WriterProfileEdit(Writer p)
        {

            SHA1 sha1 = new SHA1CryptoServiceProvider();
            string password = p.WriterPassword;
            string hash = Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(password)));
            
            ValidationResult result = writerValidator.Validate(p);
            if (result.IsValid)
            {
                var writer = new Writer
                {
                    WriterPassword = hash,
                    WriterAbout=p.WriterAbout,
                    WriterId=p.WriterId,
                    WriterImage=p.WriterImage,
                    WriterMail=p.WriterMail,
                    WriterName=p.WriterName,
                    WriterStatus=p.WriterStatus,
                    WriterSurName=p.WriterSurName,
                    WriterTitle=p.WriterTitle,
                    
            };
               
                wm.WriterUpdate(writer);

                return RedirectToAction("WriterProfile");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
        public ActionResult MyHeading(string p)
        {

            p = (string)Session["WriterMail"];
            var writerIdInfo = c.Writers.Where(x => x.WriterMail == p).Select(y => y.WriterId).FirstOrDefault();
            var values = hm.GetListByWriter(writerIdInfo);
            return View(values);
        }
        [HttpGet]
        public ActionResult NewHeading()
        {


            List<SelectListItem> valuecategory = (from x in cm.GetList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryId.ToString()
                                                  }
                                                ).ToList();
            ViewBag.vlc = valuecategory;
            return View();
        }
        [HttpPost]
        public ActionResult NewHeading(Heading p)
        {
            string mail = (string)Session["WriterMail"];
            var writerIdInfo = c.Writers.Where(x => x.WriterMail == mail).Select(y => y.WriterId).FirstOrDefault();
            p.HeadingDate = DateTime.Parse(DateTime.Now.ToShortTimeString());
            p.WriterId = writerIdInfo;
            p.HeadingStatus = true;
            hm.HeadingAdd(p);
            return RedirectToAction("MyHeading");
        }

        [HttpGet]
        public ActionResult UpdateHeading(int id)
        {
            List<SelectListItem> valuecategory = (from x in cm.GetList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryId.ToString()
                                                  }
                                                  ).ToList();

            ViewBag.vlc = valuecategory;
            var headingValue = hm.GetById(id);
            return View(headingValue);
        }
        [HttpPost]
        public ActionResult UpdateHeading(Heading p)
        {
            hm.HeadingUpdate(p);
            return RedirectToAction("MyHeading");
        }
        public ActionResult DeleteHeading(int id)
        {
            var headingValue = hm.GetById(id);
            headingValue.HeadingStatus = false;
            hm.HeadingyDelete(headingValue);
            return RedirectToAction("MyHeading");
        }
        public ActionResult AllHeading(int p = 1)
        {

            var headings = hm.GetList().ToPagedList(p, 4);
            return View(headings);
        }

    }
}
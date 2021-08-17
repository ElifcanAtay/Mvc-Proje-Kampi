using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    [Authorize(Roles = "B")]
    public class ContactController : Controller
    {
        ContactManager cm = new ContactManager(new EfContactDal());
        MessageManager mm = new MessageManager(new EfMessageDal());
        ContactValidator cv = new ContactValidator();
        Context c = new Context();
        public ActionResult Index()
        {
            var contactValues = cm.GetList();
            return View(contactValues);
        }
        public ActionResult GetContactDetails(int id)
        {
            var contactValues = cm.GetById(id);
            return View(contactValues);
        }
       

        public PartialViewResult ContactPartial(string p)
        {
            Context c = new Context();
            p = (string)Session["AdminUserName"];
            var adminUserInfo = c.Admins.Where(x => x.AdminUserName == p).Select(y => y.AdminUserName).FirstOrDefault();

            var deger1 = c.Contacts.Count();
            ViewBag.d1 = deger1;

            var deger2 = c.Messages.Where(x => x.ReceiverMail == adminUserInfo).Count();
            ViewBag.d2 = deger2;

            var deger3 = c.Messages.Where(x => x.SenderMail == adminUserInfo).Count( y=> y.isDraft == false);
            ViewBag.d3 = deger3;

            var deger4 = c.Messages.Where(x => x.SenderMail == adminUserInfo).Count(y => y.isDraft == true);
            ViewBag.d4 = deger4;

            var deger5 = mm.GetList(adminUserInfo).Count();
            ViewBag.d5 = deger5;

            var deger6 = mm.GetListUnRead(adminUserInfo).Count();
            ViewBag.d6 = deger6;


            return PartialView();
        }
        public PartialViewResult MessageListMenu()
        {
            return PartialView();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult NewMessage(Contact p)
        {
            ValidationResult result = cv.Validate(p);
            if (result.IsValid)
            {
                p.ContactDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                cm.ContactAdd(p);
                return RedirectToAction("HomePage", "Home");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return RedirectToAction("HomePage", "Home");
        }
    }
}
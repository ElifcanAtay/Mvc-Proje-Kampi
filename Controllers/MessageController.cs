using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    [Authorize(Roles = "B")]
    public class MessageController : Controller
    {
        MessageValidator messageValidator = new MessageValidator();
        MessageManager mm = new MessageManager(new EfMessageDal());
        Context c = new Context();
        public ActionResult Inbox(string p)
        {
            p = (string)Session["AdminUserName"];
            var adminUserInfo = c.Admins.Where(x => x.AdminUserName == p).Select(y => y.AdminUserName).FirstOrDefault();
            var messageList = mm.GetListInbox(adminUserInfo);
            var sendbox = messageList.FindAll(x => x.isDraft == false).ToList();
            return View(sendbox);
        }
        public ActionResult Sendbox(string p)
        {
            p = (string)Session["AdminUserName"];
            var adminUserInfo = c.Admins.Where(x => x.AdminUserName == p).Select(y => y.AdminUserName).FirstOrDefault();
            var messageList = mm.GetListSendbox(adminUserInfo);
          
            return View(messageList);
        }
        public ActionResult GetInboxMessageDetails(int id)
        {
            var Values = mm.GetById(id);
            return View(Values);
        }
        public ActionResult GetSendboxMessageDetails(int id)
        {
            var Values = mm.GetById(id);
            return View(Values);
        }

        [HttpGet]
        public ActionResult AddNewMessage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewMessage(Message p,string m, string button)
        {
            ValidationResult result = messageValidator.Validate(p);

            m = (string)Session["AdminUserName"];
            var adminUserInfo = c.Admins.Where(x => x.AdminUserName == m).Select(y => y.AdminUserName).FirstOrDefault();

            if (button == "draft")
            {

                result = messageValidator.Validate(p);
                if (result.IsValid)
                {
                    p.MessageDate = DateTime.Now;
                    p.SenderMail = adminUserInfo;

                    p.isDraft = true;
                    mm.MessageAdd(p);
                    return RedirectToAction("Draft");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            else if (button == "send")
            {
                result = messageValidator.Validate(p);
                if (result.IsValid)
                {
                    p.MessageDate = DateTime.Now;
                    p.SenderMail = adminUserInfo;
                    p.isDraft = false;
                    mm.MessageAdd(p);
                    return RedirectToAction("Sendbox");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            return View();
            //if (result.IsValid)
            //{
            //    p.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            //    mm.MessageAdd(p);
            //    return RedirectToAction("Sendbox");
            //}
            //else
            //{
            //    foreach (var item in result.Errors)
            //    {
            //        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
            //    }
            //}
            //return View();
        }
        public ActionResult Draft(string p)
        {
            p = (string)Session["AdminUserName"];
            var adminUserInfo = c.Admins.Where(x => x.AdminUserName == p).Select(y => y.AdminUserName).FirstOrDefault();
            var sendList = mm.GetListDraft(p);
            var draftList = sendList.FindAll(x => x.isDraft == true).ToList();
            return View(draftList);
        }

        public ActionResult GetDraftMessageDetails(int id)
        {
            var values = mm.GetById(id);
            return View(values);
        }

        public ActionResult IsRead(int id)
        {
            var result = mm.GetById(id);
            if (result.IsRead == false)
            {
                result.IsRead = true;
            }
            else
            {
                result.IsRead = false;
            }
           
            mm.MessageUpdate(result);
            return RedirectToAction("Inbox");
        }

    }
}

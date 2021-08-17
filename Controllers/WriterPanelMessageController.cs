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
    public class WriterPanelMessageController : Controller
    {
        MessageValidator messageValidator = new MessageValidator();
        MessageManager mm = new MessageManager(new EfMessageDal());
        Context c = new Context();
        public ActionResult Inbox(string p)
        {
            p = (string)Session["WriterMail"];
            var messageList = mm.GetListInbox(p).ToList();
            var sendbox = messageList.FindAll(x => x.isDraft == false).ToList();

            return View(sendbox);
        }
        public PartialViewResult MessageListMenu(string p)
        {
           
            p = (string)Session["WriterMail"];
            var writerUserInfo = c.Writers.Where(x => x.WriterMail == p).Select(y => y.WriterMail).FirstOrDefault();

            var deger1 = c.Contacts.Count();
            ViewBag.d1 = deger1;

            var deger2 = c.Messages.Where(x => x.ReceiverMail == writerUserInfo).Count();
            ViewBag.d2 = deger2;

            var deger3 = c.Messages.Where(x => x.SenderMail == writerUserInfo).Count(y => y.isDraft == false);
            ViewBag.d3 = deger3;

            var deger4 = c.Messages.Where(x => x.SenderMail == writerUserInfo).Count(y => y.isDraft == true);
            ViewBag.d4 = deger4;

            var deger5 = mm.GetListUnRead(writerUserInfo).Where(x=>x.isDraft==false).Count();
            ViewBag.d5 = deger5;

            return PartialView();
        }
        public ActionResult Sendbox(string p)
        {
            p = (string)Session["WriterMail"];
            var messageList = mm.GetListSendbox(p);
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
        public ActionResult AddNewMessage(Message p, string m, string button)
        {
           
            ValidationResult result = messageValidator.Validate(p);

            m = (string)Session["WriterMail"];
            var writerUserInfo = c.Writers.Where(x => x.WriterMail == m).Select(y => y.WriterMail).FirstOrDefault();

            if (button == "draft")
            {

                result = messageValidator.Validate(p);
                if (result.IsValid)
                {
                    p.MessageDate = DateTime.Now;
                    p.SenderMail = writerUserInfo;
                    p.isDraft = true;
                    p.IsRead = false;
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
                    p.SenderMail = writerUserInfo;
                    p.isDraft = false;
                    p.IsRead = false;
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
            //    p.SenderMail = (string)Session["WriterMail"];
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
            p = (string)Session["WriterMail"];
            var writerUserInfo = c.Writers.Where(x => x.WriterMail == p).Select(y => y.WriterMail).FirstOrDefault();
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
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        WriterValidator writerValidator = new WriterValidator();
        WriterManager wm = new WriterManager(new EfWriterDal());
        [AllowAnonymous]
        
        [HttpPost]
        public ActionResult WriterRegister(Writer p)
        {
            ValidationResult result = writerValidator.Validate(p);
            if (result.IsValid)
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                string password = p.WriterPassword;
                string hash = Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(password)));
                var writer = new Writer
                {
                    WriterAbout = "???",
                    WriterImage = "https://www.textileaadhar.com/_next/static/images/bfd65d5448e2981f6259bf402a492009.png",
                    WriterTitle = "Ünvan:???",
                    WriterMail = p.WriterMail,
                    WriterPassword = hash,
                    WriterName = p.WriterName,
                    WriterSurName = p.WriterSurName,
                    WriterStatus = true,
                    WriterId=p.WriterId
                };
                wm.WriterAdd(writer);
                return RedirectToAction("WriterLogin", "Login");
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
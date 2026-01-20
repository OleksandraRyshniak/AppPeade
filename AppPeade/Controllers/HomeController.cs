using AppPeade.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AppPeade.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
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

        [HttpGet]
        public ActionResult Ankeet()
        {
            var pyhad = db.Pyhad.ToList();
            //ViewBag.Pyhad = pyhad;
            ViewBag.Pyhad = new SelectList(pyhad, "Id", "Nimetus");
            return View();
        }

        public ActionResult Ankeet(Kylaline kylaline)
        {
            if (ModelState.IsValid)
            {
                db.Kylaline.Add(kylaline);
                db.SaveChanges();
                return RedirectToAction("Tanan", kylaline);
            }
            var pyhad = db.Pyhad.ToList();
            ViewBag.Pyhad = new SelectList(pyhad, "Id", "Nimetus", kylaline.PyhaId);
            return View(kylaline);
        }

        public ActionResult Tanan (int id)
        {
            var kylaline = db.Kylaline.Find(id);
            if (kylaline == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pyhanimetus = db.Pyhad.Find(kylaline.PyhaId)?.Nimetus;
            if (kylaline.OnKutse) {
                ViewBag.Pilt = "happy.jpg";
            }else
               { ViewBag.Pilt = "sad.jpg"; }
            SaadaEmail(kylaline, ViewBag.Pilt, ViewBag.Pyhanimetus, kylaline.OnKutse);

            return View("Tanan", kylaline);
        }
        //https://myaccount.google.com/apppasswords

        private void SaadaEmail(Kylaline kylaline, string pilt, string pyha, bool onkutse)
        {
            string failiTee = Path.Combine(Server.MapPath("~/Images/"), pilt);
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "oleksandraryshniak@gmail.com";
                WebMail.Password = "ezaw dpcw jhhw mhhr";
                WebMail.From = "oleksandraryshniak@gmail.com";
                string sisu = "";
                if (onkutse)
                {
                    sisu = $"Tere, {kylaline.Nimi}!<br/><br/>" +
                        $"Sinu registreerumine sündmusele <b> {pyha}</b> on salvestatud. <br/>" +
                        "Lisamine kirjale, ka sündmuse kutse. Ootame sind väga!<br/><br/>" +
                        "Kohtumiseni!";
                }
                else
                {
                    sisu = $"Tere, {kylaline.Nimi}!<br/><br/>" +
                    $"Sinu registreerumine sündmusele <b> {pyha}</b> on salvestatud. <br/>" +
                    "Lisamine kirjale, ka sündmuse kutse. Kahju, et sa ei tule peole!<br/><br/>" +
                    "Kõige head!";
                }
                WebMail.Send(
                    to: kylaline.Email,
                    subject: "Vastus: " + pyha,
                    body: sisu,
                    isBodyHtml: true,
                    filesToAttach: new string[] { failiTee }
                    );
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("E-maili viga: " + ex );
            }

        }
    }
}
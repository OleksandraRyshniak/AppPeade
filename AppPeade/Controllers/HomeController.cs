using AppPeade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
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
            ViewBag.Pilt = "vaikimisi_pilt.jpg";
            return View("Tanan", kylaline);
        }
    }
}
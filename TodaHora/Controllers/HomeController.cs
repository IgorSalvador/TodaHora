using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TodaHora.Models;

namespace TodaHora.Controllers
{
    public class HomeController : Controller
    {
        private dbTodaHoraEntities db = new dbTodaHoraEntities();

        public ActionResult Index()
        {
            ViewBag.TotalUsuarios = db.Usuario.Where(m => m.blnAtivo == true).Count();

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
    }
}
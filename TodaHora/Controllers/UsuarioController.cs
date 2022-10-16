using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TodaHora.Models;

namespace TodaHora.Controllers
{
    public class UsuarioController : Controller
    {
        dbTodaHoraEntities dbTodaHora = new dbTodaHoraEntities();

        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserProfile(int user_Id)
        {
            return View(dbTodaHora.Usuario.Where(m => m.Usuario_Id == user_Id).ToList());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TodaHora.Models;
using TodaHora.Models.Utils;
using TodaHora.Models.ViewModel;
using System.Configuration;

namespace TodaHora.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {

            dbTodaHoraEntities dbContext = new dbTodaHoraEntities();

            try
            {
                var UsuarioPost = new UserLoginView();

                UsuarioPost.UsernameOrMail = username;
                UsuarioPost.Password = Cryptography.Base64Encode(password);

                var UsuarioSystem = new Usuario();
                bool userNull = true;

                if (dbContext.Usuario.Where(m => m.Email.ToUpper() == UsuarioPost.UsernameOrMail.ToUpper() && m.Senha == UsuarioPost.Password && m.blnAtivo == true).Any())
                {
                    UsuarioSystem = dbContext.Usuario.Where(m => m.Email.ToUpper() == UsuarioPost.UsernameOrMail.ToUpper() && m.Senha == UsuarioPost.Password).FirstOrDefault();
                    userNull = false;
                }
                else if (dbContext.Usuario.Where(m => m.Username.ToUpper() == UsuarioPost.UsernameOrMail.ToUpper() && m.Senha == UsuarioPost.Password && m.blnAtivo == true).Any())
                {
                    UsuarioSystem = dbContext.Usuario.Where(m => m.Username.ToUpper() == UsuarioPost.UsernameOrMail.ToUpper() && m.Senha == UsuarioPost.Password).FirstOrDefault();
                    userNull = false;
                }
                else
                {
                    throw new Exception("Usuário ou senha inválidos!");
                }

                if (!userNull)
                {
                    // Criando instancia dos cookies
                    var cookie = new HttpCookie(ConfigurationManager.AppSettings["LoginCookieName"]);

                    //Preenchendo valores dos cookies
                    cookie.Values.Add("username", UsuarioSystem.Username);
                    cookie.Values.Add("email", UsuarioSystem.Email);
                    cookie.Values.Add("nome", $"{UsuarioSystem.Pessoa.Nome} {UsuarioSystem.Pessoa.Sobrenome}");
                    if (UsuarioSystem.blnAdmin == true)
                        cookie.Values.Add("IsAdmin", "S");
                    else
                        cookie.Values.Add("isAdmin", "N");
                    cookie.Values.Add("isLoggedIn", "S");
                    cookie.Expires = DateTime.Now.AddHours(1);

                    //Definindo segurança do cookie
                    cookie.HttpOnly = true;

                    MvcApplication objGlobal = new MvcApplication();
                    objGlobal.setCookieToResponse(cookie);
                }

                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                ViewBag.InvalidUser = ex.Message;
                return View();
            }
        }
    }
}
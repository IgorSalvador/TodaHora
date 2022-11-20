using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
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
        private dbTodaHoraEntities dbContext = new dbTodaHoraEntities();

        // GET: Login
        public ActionResult Index()
        {
            Cookies cookie = new Cookies();

            //Valida existência do cookie preenchido, caso vázio solicita login, se não redireciona para o Index/Home
            if (cookie.username == String.Empty)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
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
                    cookie.Values.Add("user_Id", UsuarioSystem.Usuario_Id.ToString());
                    cookie.Values.Add("username", UsuarioSystem.Username);
                    cookie.Values.Add("email", UsuarioSystem.Email);
                    cookie.Values.Add("nome", UsuarioSystem.Pessoa.Nome);
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

        public ActionResult LogOut()
        {
            Cookies cookies = new Cookies();
            cookies.removerCookieLogin();
            return RedirectToAction("Index", "Login");
        }
    }
}
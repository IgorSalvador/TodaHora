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
    public class LoginController : BaseController
    {
        private dbTodaHoraEntities dbContext = new dbTodaHoraEntities();

        // GET: Login
        public ActionResult Index()
        {
            Cookies cookie = new Cookies();

            //Valida existência do cookie preenchido, caso vázio solicita login, se não redireciona para o Index / Home
            if (cookie.username == String.Empty)
            {
                return View();
            }
            else
            {

                #region ::Caso o acesso tenha sido por algum link, após o login será redirecionado::

                try
                {
                    HttpCookie cookieRedirect = Request.Cookies["Usuario"];
                    if (cookieRedirect != null)
                    {
                        // Separa os valores das propriedade
                        string[] valores = cookieRedirect.Value.ToString().Split('&');

                        string controller = valores[0] as string;
                        string action = valores[1] as string;
                        string id = "";
                        try
                        {// o ID pode ser vazio
                            id = valores[2] as string;
                            id = id.Split('=')[1];
                        }
                        catch { id = ""; }

                        controller = controller.Split('=')[1];
                        action = action.Split('=')[1];

                        //Apago o Cookie
                        Response.Cookies["Usuario"].Expires = DateTime.Now.AddDays(-1);

                        return string.IsNullOrEmpty(id) ? RedirectToRoute(new { controller, action }) :
                            RedirectToRoute(new { controller, action, id });
                    }
                }
                catch { }

                #endregion

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
                    cookie.Expires = DateTime.Now.AddHours(5);

                    //Definindo segurança do cookie
                    cookie.HttpOnly = true;

                    MvcApplication objGlobal = new MvcApplication();
                    objGlobal.setCookieToResponse(cookie);
                }

                return RedirectToAction("Index", "Login");

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
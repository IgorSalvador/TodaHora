using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using TodaHora.Models;

namespace TodaHora.Controllers
{
    public class BaseController : Controller
    {
        private dbTodaHoraEntities db = new dbTodaHoraEntities();

        /// <summary>
        /// Método para ser utilizado caso se queira registrar qualquer atividade
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoginCookiesAtual.getCookies();
            if (string.IsNullOrEmpty(LoginCookiesAtual.username) && !LoginCookiesAtual.isLoggedIn)
            {
                //Sempre verifico se é necessário refazer o login
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                string id = "";
                
                if(HttpContext.Request.HttpMethod != "POST")
                {
                    try { id = this.ControllerContext.RouteData.Values["id"].ToString(); } catch { id = ""; }
                }

                if (controllerName != "Login" && controllerName != "Erro")
                {
                    // Criando a Instância do cookie
                    var cookie = new HttpCookie("Usuario");

                    cookie.Values.Add("ult_url_controller", controllerName);
                    cookie.Values.Add("ult_url_action", actionName);
                    cookie.Values.Add("ult_url_id", id);

                    cookie.Expires = DateTime.Now.AddMinutes(60);

                    // Definindo a segurança do nosso cookie
                    cookie.HttpOnly = true;
                    // Registrando cookie
                    this.Response.AppendCookie(cookie);

                    filterContext.Result = RedirectToAction("Index", "Login");
                    return;
                }
            }
        }

        /// <summary>
        /// Liberar recurso imediatamente ao GC
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

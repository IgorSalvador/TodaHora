using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TodaHora.Models
{
    public class Cookies : HttpApplication
    {

        private string cookieName { get; set; }
        public int user_Id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string nome { get; set; }
        /// <summary>
        /// Indica se o usuário é administrador ou usuário
        /// </summary>
        public bool isAdmin { get; set; }
        /// <summary>
        /// Indica se o usuário está logado.
        /// </summary>
        public bool isLoggedIn { get; set; }
        /// <summary>
        /// Indica se o login expirou.
        /// </summary>
        public bool blnLoginExpired { get; set; }

        /// <summary>
        /// Método responsável por obter as propriedades do login armazenados no cookie
        /// </summary>
        public Cookies()
        {
            this.cookieName = ConfigurationManager.AppSettings["LoginCookieName"];

            //Obtém requisicaão com os dados dos cookies
            HttpCookie cookie = obterRequisicaoCookie();

            if(cookie != null)
            {
                string[] propriedadesCookies = cookie.Value.ToString().Split('&');

                this.user_Id = (int)Convert.ToInt64(propriedadesCookies[0].Split('=')[1]);
                this.username = propriedadesCookies[1].Split('=')[1];
                this.email = propriedadesCookies[2].Split('=')[1];
                this.nome = propriedadesCookies[3].Split('=')[1];
                this.isAdmin = propriedadesCookies[4].Split('=')[1].Equals("S");
                this.isLoggedIn = propriedadesCookies[5].Split('=')[1].Equals("S");
                this.blnLoginExpired = cookie.Expires < DateTime.Now;
            }
            else
            {
                this.user_Id = 0;
                this.username = String.Empty;
                this.email = String.Empty;
                this.nome = String.Empty;
                this.isAdmin = false;
                this.isLoggedIn = false;
                this.blnLoginExpired = true;
            }
        }

        public HttpCookie obterRequisicaoCookie()
        {
            try
            {
                // Retorna cookie caso o mesmo exista
                return HttpContext.Current.Request.Cookies[this.cookieName];
            }
            catch
            {
                // Caso o cookie não exista retorna nulo
                return null;
            }
        }

        /// <summary>
        /// Atribui cookie como expirado.
        /// </summary>
        public void removerCookieLogin()
        {
            //Removendo cookie
            HttpContext.Current.Response.Cookies[this.cookieName].Expires = DateTime.Now.AddDays(-1);
        }
    }

    public static class LoginCookiesAtual
    {
        public static int user_Id { get; set; }
        public static string username { get; set;  }
        public static string email { get; set;  }
        public static string nome { get; set; }
        public static bool isAdmin { get; set; }
        public static bool isLoggedIn { get; set; }

        public static void getCookies()
        {
            Cookies loginCookies = new Cookies();

            user_Id = loginCookies.user_Id;
            username = loginCookies.username;
            email = loginCookies.email;
            nome = loginCookies.nome;
            isAdmin = loginCookies.isAdmin;
            isLoggedIn = loginCookies.isLoggedIn;
        }
    }
}
using ClassLibrary_Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using TodaHora.Models;
using TodaHora.Models.Utils;
using TodaHora.Models.ViewModel;

namespace TodaHora.Controllers
{
    public class UsuarioController : BaseController
    {
        dbTodaHoraEntities dbTodaHora = new dbTodaHoraEntities();

        // GET: Usuario
        public ActionResult Index()
        {
            LoginCookiesAtual.getCookies();

            if(LoginCookiesAtual.username == string.Empty)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View(dbTodaHora.Usuario.ToList().Take(100));
            }
        }

        [HttpPost]
        public ActionResult Index(string txtNome, string txtEmail, string txtTelefone, string txtCPF)
        {
            bool blnNome = !string.IsNullOrWhiteSpace(txtNome);
            bool blnEmail = !string.IsNullOrWhiteSpace(txtEmail);
            bool blnTelefone = !string.IsNullOrWhiteSpace(txtTelefone);
            bool blnCPF = !string.IsNullOrWhiteSpace(txtCPF); ;

            #region :: Removing Formatations ::

            string nome = txtNome;
            string email = txtEmail.Trim();
            string telefone = FormatTelefone.SemFormatacao(txtTelefone);
            string cpf = FormatCPFCNPJ.SemFormatacao(txtCPF);

            #endregion

            if (!blnNome && !blnEmail && !blnTelefone && !blnCPF)
                return RedirectToAction("Index");

            List<Usuario> listUsers = dbTodaHora.Usuario.ToList();

            if (blnNome)
            {
                ViewBag.txtNome = nome;
                listUsers = listUsers.Where(item => item.Pessoa.Nome.ToUpper().Contains(nome.ToUpper())).ToList();
            }

            if (blnCPF)
            {
                ViewBag.txtDataNascimento = txtCPF;
                listUsers = listUsers.Where(item => item.Pessoa.Cpf.ToUpper().Contains(cpf.ToUpper())).ToList();
            }

            if (blnEmail)
            {
                ViewBag.txtEmail = email;
                listUsers = listUsers.Where(item => item.Email.ToUpper().Contains(email.ToUpper())).ToList();
            }

            if (blnTelefone)
            {
                ViewBag.txtTelefone = telefone;
                listUsers = listUsers.Where(item => item.Pessoa.Telefone.ToUpper().Contains(telefone.ToUpper())).ToList();
            }

            return View(listUsers.OrderByDescending(item => item.Usuario_Id));
        }

        public ActionResult Create()
        {
            //Lista generos disponiveis para exibição na tela em caso de edição.
            ViewBag.SexoList = dbTodaHora.Sexo.Where(m => m.blnAtivo == true).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Create(Usuario usuario, int blnAdmin)
        {
            LoginCookiesAtual.getCookies();

            if (usuario != null)
            {
                usuario.blnAdmin = blnAdmin == 1 ? true : false;
                usuario.blnAtivo = false;
                usuario.Created_On = DateTime.Now;
                usuario.Created_By_UserName = LoginCookiesAtual.username;
                usuario.Created_By_Id = LoginCookiesAtual.user_Id;


                dbTodaHora.Usuario.Add(usuario);
                dbTodaHora.SaveChanges();

                Mail objM = new Mail();
                objM.Assunto = "Criação de usuário - TodaHora Supermercado";
                objM.email = usuario.Email;
                objM.username = usuario.Pessoa.Nome;
                objM.ObjectId = usuario.Usuario_Id.ToString();
                objM.UserMailPasswordDefiner(objM);

                return RedirectToAction("Index", "Usuario");
            }
            else
            {
                return RedirectToAction("Index", "Usuario"); // Redirecionar para erro
            }
            
        }

        public ActionResult Edit(int? id)
        {
            LoginCookiesAtual.getCookies();

            if (LoginCookiesAtual.username == string.Empty)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Usuario usuario = dbTodaHora.Usuario.Find(id);

                if (usuario == null)
                {
                    return HttpNotFound();
                }

                ViewBag.SexoList = dbTodaHora.Sexo.ToList();

                return View(usuario);
            }
        }

        public ActionResult Details(int? id)
        {
            LoginCookiesAtual.getCookies();

            if (LoginCookiesAtual.username == string.Empty)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Usuario usuario = dbTodaHora.Usuario.Find(id);

                if (usuario == null)
                {
                    return HttpNotFound();
                }

                return View(usuario);
            }
        }

        public ActionResult UserProfile(int user_Id)
        {

            LoginCookiesAtual.getCookies();

            if (LoginCookiesAtual.username == string.Empty)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                //Lista generos disponiveis para exibição na tela em caso de edição.
                ViewBag.GeneroList = dbTodaHora.Sexo.Where(m => m.blnAtivo == true).ToList();

                return View(dbTodaHora.Usuario.Where(m => m.Usuario_Id == user_Id).ToList());
            }
        }

        #region :: Ajax functions ::

        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult EditUserInfo(int Usuario_Id, string Nome, string Sobrenome, string Telefone,string Username, string Email, int blnAdmin)
        {
            UserViewModel Usuario = new UserViewModel();

            try
            {
                Usuario.Usuario_Id = Usuario_Id;
                Usuario.Nome = Nome;
                Usuario.Sobrenome = Sobrenome;
                Usuario.Email = Email;
                Usuario.Telefone = Telefone.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "").Trim();
                Usuario.Username = Username;
                Usuario.blnAdmin = blnAdmin == 1 ? true : false;

                var ListagemUsuario = Usuario.EditUserInfo(Usuario);

                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult DisableUser(int usuario_Id)
        {
            UserViewModel Usuario = new UserViewModel();

            try
            {
                Usuario.Usuario_Id = usuario_Id;

                var ListagemUsuario = Usuario.DisableUser(Usuario);

                return Json(true, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult EnableUser(int usuario_Id)
        {
            UserViewModel Usuario = new UserViewModel();

            try
            {
                Usuario.Usuario_Id = usuario_Id;

                var ListagemUsuario = Usuario.EnableUser(Usuario);

                return Json(true, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult EditProfile(int usuario_Id, string nome, string sobrenome, DateTime dataNascimento, string cpf, string telefone, int sexo)
        {
            UserViewModel Usuario = new UserViewModel();
            try
            {
                Usuario.Usuario_Id = usuario_Id;
                Usuario.Nome = nome;
                Usuario.Sobrenome = sobrenome;
                Usuario.DataNascimento = dataNascimento;
                Usuario.Cpf = FormatCPFCNPJ.SemFormatacao(cpf);
                Usuario.Telefone = FormatTelefone.SemFormatacao(telefone);
                Usuario.Sexo_Id = sexo;

                var ListagemUsuario = Usuario.EditProfile(Usuario);

                if (ListagemUsuario.Count > 0)
                {
                    Cookies cookíe = new Cookies();

                    if (cookíe.reloadCookies(usuario_Id))
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        throw new Exception("Erro ao atualizar cookies, após atualização do perfil");
                    }
                }
                else
                {
                    throw new Exception("Erro ao realizar a atualização do perfil");
                }

            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult EditAccount(int usuario_Id, string username, string email)
        {
            UserViewModel Usuario = new UserViewModel();

            try
            {
                Usuario.Usuario_Id = usuario_Id;
                Usuario.Username = username;
                Usuario.Email = email;

                var ListagemUsuario = Usuario.EditAccount(Usuario);

                if (ListagemUsuario.Count > 0)
                {
                    Cookies cookíe = new Cookies();

                    if (cookíe.reloadCookies(usuario_Id))
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        throw new Exception("Erro ao atualizar cookies, após atualização do perfil");
                    }
                }
                else
                {
                    throw new Exception("Erro ao realizar a atualização do perfil");
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult EditAccoutPassword(int usuario_Id, string password)
        {
            UserViewModel Usuario = new UserViewModel();

            try
            {
                Usuario.Usuario_Id = usuario_Id;
                Usuario.Password = Cryptography.Base64Encode(password);

                var ListagemUsuario = Usuario.EditAccountPassword(Usuario);

                if (ListagemUsuario.Count > 0)
                {
                    Cookies cookíe = new Cookies();

                    if (cookíe.reloadCookies(usuario_Id))
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        throw new Exception("Erro ao atualizar cookies, após atualização do perfil");
                    }
                }
                else
                {
                    throw new Exception("Erro ao realizar a atualização do perfil");
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
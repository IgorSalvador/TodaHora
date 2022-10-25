using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TodaHora.Models;
using TodaHora.Models.Utils;
using TodaHora.Models.ViewModel;

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
            //Lista generos disponiveis para exibição na tela em caso de edição.
            ViewBag.GeneroList = dbTodaHora.Sexo.Where(m => m.blnAtivo == true).ToList();

            return View(dbTodaHora.Usuario.Where(m => m.Usuario_Id == user_Id).ToList());
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

                if(ListagemUsuario.Count > 0)
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
            catch(Exception ex)
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
            catch(Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
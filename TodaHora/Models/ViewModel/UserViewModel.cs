﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.UI.WebControls.WebParts;

namespace TodaHora.Models.ViewModel
{
    public class UserViewModel
    {
        public int Usuario_Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set;  }
        public string Username { get; set;  }
        public string Password { get; set;  }
        public string Telefone { get; set;  }
        public string Cpf { get; set;  }
        public DateTime DataNascimento { get; set;  }
        public int Sexo_Id { get; set; }
        public int Pessoa_Id { get; set; }
        public bool blnAtivo { get; set; }
        public bool blnAdmin { get; set; }


        //Instancia de banco
        private dbTodaHoraEntities db = new dbTodaHoraEntities();
        private List<Usuario> ListUser = new List<Usuario>();

        public List<Usuario> EditProfile(UserViewModel UserUpdate)
        {
            try
            {
                //Recupera os cookies para salvar informações no banco referente a alteração
                LoginCookiesAtual.getCookies();

                var usuario = db.Usuario.Find(UserUpdate.Usuario_Id);

                usuario.Pessoa.Nome = UserUpdate.Nome;
                usuario.Pessoa.Sobrenome = UserUpdate.Sobrenome;
                usuario.Pessoa.DataNascimento = UserUpdate.DataNascimento;
                usuario.Pessoa.Cpf = UserUpdate.Cpf;
                usuario.Pessoa.Telefone = UserUpdate.Telefone;
                usuario.Pessoa.Sexo_Id = UserUpdate.Sexo_Id;
                usuario.Data_alteracao = DateTime.Now;
                usuario.UsuarioAlteracao = LoginCookiesAtual.nome;

                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();

                ListUser.Add(usuario);
                return ListUser.ToList();
            }
            catch(Exception ex)
            {
                throw new Exception("Erro " + ex);
;           }
        }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TodaHora.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuario
    {
        public int Usuario_Id { get; set; }
        public string Username { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public bool blnAtivo { get; set; }
        public bool blnAdmin { get; set; }
        public Nullable<System.DateTime> DataAlteracao { get; set; }
        public Nullable<int> Pessoa_Id { get; set; }
        public Nullable<System.DateTime> Created_On { get; set; }
        public string Created_By_UserName { get; set; }
        public Nullable<int> Created_By_Id { get; set; }
    
        public virtual Pessoa Pessoa { get; set; }
    }
}

﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbTodaHoraEntities : DbContext
    {
        public dbTodaHoraEntities()
            : base("name=dbTodaHoraEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Pessoa> Pessoa { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Sexo> Sexo { get; set; }
    }
}

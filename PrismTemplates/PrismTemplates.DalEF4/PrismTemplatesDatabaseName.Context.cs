﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrismTemplates.DalEF4
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PrismTemplatesDatabaseNameEntities : DbContext
    {
        public PrismTemplatesDatabaseNameEntities()
            : base("name=PrismTemplatesDatabaseNameEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CodeGenRule> CodeGenRules { get; set; }
        public DbSet<CodeGenString> CodeGenStrings { get; set; }
        public DbSet<Entity> Entities { get; set; }
    }
}

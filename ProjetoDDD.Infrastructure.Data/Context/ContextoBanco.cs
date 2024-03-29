﻿using ProjetoDDD.Domain.Entities;
using ProjetoDDD.Infrastructure.Data.Confinguration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ProjetoDDD.Infrastructure.Data.Context
{
    public class ContextoBanco : DbContext
    {
        public ContextoBanco() : base("projetoDDDContext")
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PerfilUsuario> PerfilUsuario { get; set; }
        public DbSet<ModulosAcesso> ModulosAcesso { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Removendo as convenções automáticas do Entity Framework
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //Setando as configurações base para criação de tabelas no Banco de Dados
            modelBuilder.Properties().Where(p => p.Name == p.ReflectedType.Name + "Id").Configure(p => p.IsKey());
            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(100));
            modelBuilder.Properties<string>().Where(p => p.Name.Contains("Descricao")).Configure(p => p.HasMaxLength(400));
            modelBuilder.Properties<string>().Where(p => p.Name.Contains("UF")).Configure(p => p.HasMaxLength(2));

            //Carregado as configurações especializadas de cada entidade
            modelBuilder.Configurations.Add(new ModulosAcessoMap());
            modelBuilder.Configurations.Add(new PerfilUsuarioMap());
            modelBuilder.Configurations.Add(new UsuarioMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}

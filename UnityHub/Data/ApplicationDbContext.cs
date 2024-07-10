using UnityHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnityHub.Data
{

    /// <summary>
    /// Esta classe representa a BD do nosso projeto
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<Utilizadores>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options){
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* Esta instrução importa tudo o que está pre-definido
             * na super classe
             */
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
             new IdentityRole { Id = "admin", Name = "admin", NormalizedName = "ADMIN" },
             new IdentityRole { Id = "registado", Name = "registado", NormalizedName = "REGISTADO" }
            );

            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<Utilizadores>().HasData(
            new Utilizadores
            {
                Id = "admin",
                UserName = "admin@UnityHub.pt",     // <-----------
                NormalizedUserName = "ADMIN@UNITYHUB.PT",
                Email = "admin@UnityHub.pt",
                NormalizedEmail = "ADMIN@UnityHub.PT",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Pa$$w0rd"),     // <-----------
                Nome = "Administrador",
                Telemovel = "912345678",
                DataNascimento = new DateOnly(1980, 1, 1),
                Cidade = "CidadeAdmin",
                Pais = "PaisAdmin"

            });

            //adicionar o role aqui do admin
            //associacao do 'admin' a role 'Admin'
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "admin", UserId = "admin" });

            modelBuilder.Entity<Candidaturas>()
                .HasOne(c => c.Utilizador)
                .WithMany(u => u.Candidaturas)
                .HasForeignKey(c => c.UtilizadorFK);

            modelBuilder.Entity<VagaCategoria>()
                .HasKey(vc => new { vc.VagaId, vc.CategoriaId });

            modelBuilder.Entity<VagaCategoria>()
                .HasOne(vc => vc.Vaga)
                .WithMany(v => v.VagasCategorias)
                .HasForeignKey(vc => vc.VagaId);

            modelBuilder.Entity<VagaCategoria>()
                .HasOne(vc => vc.Categoria)
                .WithMany(c => c.VagasCategorias)
                .HasForeignKey(vc => vc.CategoriaId);

        }

        /* ********************************************
         * definir as 'tabelas' da base de dados
         * ******************************************** */

        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Vagas> Vagas { get; set; }
        public DbSet<Candidaturas> Candidaturas { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<VagaCategoria> VagaCategoria { get; set; }
    }
}

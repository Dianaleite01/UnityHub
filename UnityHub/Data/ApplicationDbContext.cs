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
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string NomeUtilizador { get; set; }
    }

    /// <summary>
    /// Esta classe representa a BD do nosso projeto
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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

            modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "admin",
                UserName = "admin@UnityHub.pt",     // <-----------
                NormalizedUserName = "ADMIN@UNITYHUB.PT",
                Email = "admin@UnityHub.pt",
                NormalizedEmail = "ADMIN@UnityHub.PT",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Pa$$w0rd"),     // <-----------
                NomeUtilizador = "Administrador"
            });

            //adicionar o role aqui do admin
            //associacao do 'admin' a role 'Admin'
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "admin", UserId = "admin" });
        }

        /* ********************************************
         * definir as 'tabelas' da base de dados
         * ******************************************** */

        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Voluntariados> Voluntariados { get; set; }
        public DbSet<Candidaturas> Candidaturas { get; set; }
    }
}

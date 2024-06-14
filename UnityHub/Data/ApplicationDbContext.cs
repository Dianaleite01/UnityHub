using UnityHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UnityHub.Data
{
    /// <summary>
    /// Esta classe representa a BD do nosso projeto
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            /* Esta instrução importa tudo o que está pre-definido
             * na super classe
             */
            base.OnModelCreating(builder);
        }

        /* ********************************************
         * definir as 'tabelas' da base de dados
         * ******************************************** */

        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Voluntariados> Voluntariados { get; set; }
        public DbSet<Candidaturas> Candidaturas { get; set; }
    }
}

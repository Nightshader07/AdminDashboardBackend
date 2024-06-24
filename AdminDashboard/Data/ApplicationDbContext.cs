using AdminDashboard.models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Data;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<AdminGenerale> AdminGenerales { get; set; }
        public DbSet<RepresentantEntreprise> RepresentantEntreprises { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configuration if needed
        }
    }

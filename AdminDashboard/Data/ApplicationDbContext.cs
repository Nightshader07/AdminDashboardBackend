using AdminDashboard.models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Data;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<AdminGenerale> AdminGenerales { get; set; }
        public DbSet<RepresentantEntreprise> RepresentantEntreprises { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            // Map the base class to a table
            modelBuilder.Entity<Utilisateur>()
                .ToTable("Utilisateurs");

            // Map derived classes to separate tables
            modelBuilder.Entity<RepresentantEntreprise>()
                .ToTable("RepresentantEntreprises")
                .HasBaseType<Utilisateur>();

            modelBuilder.Entity<Employe>()
                .ToTable("Employes")
                .HasBaseType<Utilisateur>();

            modelBuilder.Entity<AdminGenerale>()
                .ToTable("AdminGenerales")
                .HasBaseType<Utilisateur>();
        }
    }

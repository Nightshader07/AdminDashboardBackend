using AdminDashboard.models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Data;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<AdminGenerale> AdminGenerales { get; set; }
        public DbSet<RepresentantEntreprise> RepresentantEntreprises { get; set; }
        public DbSet<Tache> Taches { get; set; }
        public DbSet<Column> Columns { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Utilisateur>()
                .HasMany(p => p.Taches)
                .WithOne(c => c.Utilisateur)
                .HasForeignKey(c => c.utilisateurId);
            modelBuilder.Entity<Column>()
                .HasMany(p => p.Taches)
                .WithOne(c => c.Column)
                .HasForeignKey(c => c.ColumnId);
            
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

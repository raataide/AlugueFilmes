using AlugaFilme.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AlugaFilme.Web.Models
{
    public class DbContexto : DbContext
    {
        public DbContexto()
            : base("AlugueFilmes") { }

        public DbSet<LoginModel> Login { get; set; }
        public DbSet<GeneroModel> Genero { get; set; }
        public DbSet<ClientesModel> Clientes { get; set; }
        public DbSet<FilmesModel> Filmes { get; set; }
        public DbSet<LocacaoModel> Locacao { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmesModel>()
                .HasMany(x => x.Genero)
                .WithMany(x => x.Filmes)
                .Map(x =>
                {
                    x.ToTable("FilmesGeneros");
                    x.MapLeftKey("Filmes_Id");
                    x.MapRightKey("Generos_Id");
                });
            modelBuilder.Entity<LocacaoModel>()
                .HasMany(x => x.Filmes)
                .WithMany(x => x.Locacao)
                .Map(x =>
                {
                    x.ToTable("locacao_filmes");
                    x.MapLeftKey("Locacao_Id");
                    x.MapRightKey("Filmes_Id");
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
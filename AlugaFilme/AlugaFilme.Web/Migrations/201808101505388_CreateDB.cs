namespace AlugaFilme.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        NumDocumento = c.String(nullable: false, maxLength: 15),
                        Numero = c.String(nullable: false, maxLength: 10),
                        Telefone = c.String(maxLength: 20),
                        Logradouro = c.String(maxLength: 100),
                        Complemento = c.String(maxLength: 50),
                        Cep = c.String(maxLength: 15),
                        Pais = c.String(nullable: false, maxLength: 30),
                        Estado = c.String(nullable: false, maxLength: 30),
                        Cidade = c.String(nullable: false, maxLength: 50),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Filmes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        DtCriacao = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Genero",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 50),
                        DtCriacao = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locacao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DtLocacao = c.DateTime(nullable: false),
                        Cpf_Cliente = c.String(nullable: false),
                        Id_Cliente = c.Int(nullable: false),
                        Devolvido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false, maxLength: 50),
                        Senha = c.String(nullable: false, maxLength: 100),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FilmesGeneros",
                c => new
                    {
                        Filmes_Id = c.Int(nullable: false),
                        Generos_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Filmes_Id, t.Generos_Id })
                .ForeignKey("dbo.Filmes", t => t.Filmes_Id, cascadeDelete: true)
                .ForeignKey("dbo.Genero", t => t.Generos_Id, cascadeDelete: true)
                .Index(t => t.Filmes_Id)
                .Index(t => t.Generos_Id);
            
            CreateTable(
                "dbo.locacao_filmes",
                c => new
                    {
                        Locacao_Id = c.Int(nullable: false),
                        Filmes_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Locacao_Id, t.Filmes_Id })
                .ForeignKey("dbo.Locacao", t => t.Locacao_Id, cascadeDelete: true)
                .ForeignKey("dbo.Filmes", t => t.Filmes_Id, cascadeDelete: true)
                .Index(t => t.Locacao_Id)
                .Index(t => t.Filmes_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.locacao_filmes", "Filmes_Id", "dbo.Filmes");
            DropForeignKey("dbo.locacao_filmes", "Locacao_Id", "dbo.Locacao");
            DropForeignKey("dbo.FilmesGeneros", "Generos_Id", "dbo.Genero");
            DropForeignKey("dbo.FilmesGeneros", "Filmes_Id", "dbo.Filmes");
            DropIndex("dbo.locacao_filmes", new[] { "Filmes_Id" });
            DropIndex("dbo.locacao_filmes", new[] { "Locacao_Id" });
            DropIndex("dbo.FilmesGeneros", new[] { "Generos_Id" });
            DropIndex("dbo.FilmesGeneros", new[] { "Filmes_Id" });
            DropTable("dbo.locacao_filmes");
            DropTable("dbo.FilmesGeneros");
            DropTable("dbo.Usuario");
            DropTable("dbo.Locacao");
            DropTable("dbo.Genero");
            DropTable("dbo.Filmes");
            DropTable("dbo.Clientes");
        }
    }
}

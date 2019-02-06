namespace Antiguera.Infra.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Acesso",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Novo = c.Boolean(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Emulador",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        DataLancamento = c.DateTime(nullable: false),
                        Descricao = c.String(nullable: false, unicode: false, storeType: "text"),
                        Console = c.String(nullable: false, maxLength: 50),
                        UrlArquivo = c.String(maxLength: 200),
                        UrlBoxArt = c.String(maxLength: 200),
                        Novo = c.Boolean(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rom",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmuladorId = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 100),
                        DataLancamento = c.DateTime(nullable: false),
                        Descricao = c.String(nullable: false, unicode: false, storeType: "text"),
                        Genero = c.String(nullable: false, maxLength: 50),
                        Novo = c.Boolean(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Emulador", t => t.EmuladorId)
                .Index(t => t.EmuladorId);
            
            CreateTable(
                "dbo.Jogo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Descricao = c.String(nullable: false, unicode: false, storeType: "text"),
                        Developer = c.String(nullable: false, maxLength: 100),
                        Publisher = c.String(nullable: false, maxLength: 100),
                        Lancamento = c.DateTime(nullable: false),
                        Plataforma = c.String(nullable: false, maxLength: 100),
                        Genero = c.String(nullable: false, maxLength: 50),
                        UrlBoxArt = c.String(maxLength: 200),
                        UrlArquivo = c.String(maxLength: 200),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        Novo = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Programa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 220),
                        Descricao = c.String(nullable: false, unicode: false, storeType: "text"),
                        Developer = c.String(nullable: false, maxLength: 100),
                        Publisher = c.String(nullable: false, maxLength: 100),
                        Lancamento = c.DateTime(nullable: false),
                        UrlBoxArt = c.String(maxLength: 200),
                        UrlArquivo = c.String(maxLength: 200),
                        TipoPrograma = c.String(nullable: false, maxLength: 100),
                        Novo = c.Boolean(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AcessoId = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 220),
                        Email = c.String(nullable: false, maxLength: 220),
                        Sexo = c.String(nullable: false, maxLength: 10),
                        Login = c.String(nullable: false, maxLength: 30),
                        Senha = c.String(nullable: false, maxLength: 220),
                        NumAcessos = c.Int(),
                        NumDownloadsJogos = c.Int(),
                        NumDownloadsProg = c.Int(),
                        UltimaVisita = c.DateTime(),
                        UrlFotoUpload = c.String(maxLength: 200),
                        Created = c.DateTime(),
                        Modified = c.DateTime(),
                        Novo = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Acesso", t => t.AcessoId, cascadeDelete: true)
                .Index(t => t.AcessoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuario", "AcessoId", "dbo.Acesso");
            DropForeignKey("dbo.Rom", "EmuladorId", "dbo.Emulador");
            DropIndex("dbo.Usuario", new[] { "AcessoId" });
            DropIndex("dbo.Rom", new[] { "EmuladorId" });
            DropTable("dbo.Usuario");
            DropTable("dbo.Programa");
            DropTable("dbo.Jogo");
            DropTable("dbo.Rom");
            DropTable("dbo.Emulador");
            DropTable("dbo.Acesso");
        }
    }
}

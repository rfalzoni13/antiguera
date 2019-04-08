using Antiguera.Aplicacao.Interfaces;
using Antiguera.Dominio.Entidades;
using Antiguera.WebApi.AutoMapper;
using Antiguera.WebApi.Controllers.Api;
using Antiguera.WebApi.Models;
using Antiguera.WebApi.Teste.ModelsTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Antiguera.WebApi.Teste.ControllersTests.Api
{
    [TestClass]
    public class JogoControllerTeste
    {
        #region Atributos
        private Jogos jogos = new Jogos();
        private Mock<IJogoAppServico> jogoAppServico = new Mock<IJogoAppServico>();
        private JogoController controller;
        #endregion

        #region Métodos Iniciais
        [ClassInitialize]
        public static void Iniciar_Classe(TestContext context)
        {
            if (!AutoMapperConfig.Iniciado)
            {
                AutoMapperConfig.RegisterMappings();
            }
        }

        [TestInitialize]
        public void Iniciar_Teste()
        {
            controller = new JogoController(jogoAppServico.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }
        #endregion

        #region Método ListarJogos
        [TestMethod]
        public void Listar_Jogos_Ok()
        {
            jogoAppServico.Setup(x => x.BuscarTodos()).Returns(jogos.ListaJogos);

            var result = controller.ListarTodosJogos();

            List<Jogo> jogosResult = new Jogos().ListaJogos;

            Assert.IsTrue(result.TryGetContentValue(out jogosResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Jogo>>().Result, jogosResult);
        }

        [TestMethod]
        public void Listar_Jogos_NotFound()
        {
            jogoAppServico.Setup(x => x.BuscarTodos()).Returns(new List<Jogo>());

            var result = controller.ListarTodosJogos();

            var jogosResult = new Jogos().ListaJogos;

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out jogosResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Message);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Jogos_InternalServerError()
        {
            controller = new JogoController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarTodosJogos();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            List<Jogo> jogosResult = new List<Jogo>();

            Assert.IsFalse(result.TryGetContentValue(out jogosResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método ListarJogosPorId
        [TestMethod]
        public void Listar_Jogos_Por_Id_Ok()
        {
            jogoAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns(jogos.ListaJogos.Where(x => x.Id == 1).FirstOrDefault());

            var result = controller.ListarJogosPorId(1);

            Jogo jogoResult = new Jogos().ListaJogos.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsTrue(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<Jogo>().Result, jogoResult);
        }

        [TestMethod]
        public void Listar_Jogos_Por_Id_BadRequest()
        {
            var result = controller.ListarJogosPorId(It.IsAny<int>());

            Jogo jogoResult = new Jogo();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual("Parâmetro incorreto!", status.Message);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Listar_Jogos_Por_Id_NotFound()
        {
            jogoAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns((Jogo)null);

            var result = controller.ListarJogosPorId(2);

            var jogoResult = new Jogos().ListaJogos.Where(x => x.Id == 3).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Message);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Jogos_Por_Id_InternalServerError()
        {
            controller = new JogoController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarJogosPorId(1);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Jogo jogosResult = new Jogos().ListaJogos.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out jogosResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método PesquisaJogo
        [TestMethod]
        public void Pesquisa_Jogo_PesquisaId_Ok()
        {
            int id = 1;

            jogoAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Jogo, bool>>())).Returns(jogos.ListaJogos.Where(x => x.Id == id).ToList());

            var result = controller.PesquisaJogo(id.ToString());

            List<Jogo> jogoResult = new Jogos().ListaJogos.Where(x => x.Id == id).ToList();

            Assert.IsTrue(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Jogo>>().Result, jogoResult);
        }

        [TestMethod]
        public void Pesquisa_Jogo_PesquisaNome_Ok()
        {
            var nome = "Lhx";

            jogoAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Jogo, bool>>())).Returns(jogos.ListaJogos.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList());

            var result = controller.PesquisaJogo(nome);

            List<Jogo> jogoResult = new Jogos().ListaJogos.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Jogo>>().Result, jogoResult);
        }

        [TestMethod]
        public void Pesquisa_Jogo_PesquisaDeveloper_Ok()
        {
            var developer = "Id Software";

            jogoAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Jogo, bool>>())).Returns(jogos.ListaJogos.Where(x => x.Developer.ToLower().Contains(developer.ToLower())).ToList());

            var result = controller.PesquisaJogo(developer);

            List<Jogo> jogoResult = new Jogos().ListaJogos.Where(x => x.Developer.ToLower().Contains(developer.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Jogo>>().Result, jogoResult);
        }

        [TestMethod]
        public void Pesquisa_Jogo_PesquisaPublisher_Ok()
        {
            var publisher = "Electronic Arts";

            jogoAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Jogo, bool>>())).Returns(jogos.ListaJogos.Where(x => x.Publisher.ToLower().Contains(publisher.ToLower())).ToList());

            var result = controller.PesquisaJogo(publisher);

            List<Jogo> jogoResult = new Jogos().ListaJogos.Where(x => x.Publisher.ToLower().Contains(publisher.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Jogo>>().Result, jogoResult);
        }

        [TestMethod]
        public void Pesquisa_Jogo_PesquisaGenero_Ok()
        {
            var genero = "Simulador de voô";

            jogoAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Jogo, bool>>())).Returns(jogos.ListaJogos.Where(x => x.Genero.ToLower().Contains(genero.ToLower())).ToList());

            var result = controller.PesquisaJogo(genero);

            List<Jogo> jogoResult = new Jogos().ListaJogos.Where(x => x.Genero.ToLower().Contains(genero.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Jogo>>().Result, jogoResult);
        }

        [TestMethod]
        public void Pesquisa_Jogo_PesquisaPlataforma_Ok()
        {
            var plataforma = "MS-DOS";

            jogoAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Jogo, bool>>())).Returns(jogos.ListaJogos.Where(x => x.Plataforma.ToLower().Contains(plataforma.ToLower())).ToList());

            var result = controller.PesquisaJogo(plataforma);

            List<Jogo> jogoResult = new Jogos().ListaJogos.Where(x => x.Plataforma.ToLower().Contains(plataforma.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Jogo>>().Result, jogoResult);
        }

        [TestMethod]
        public void Pesquisa_Jogo_PesquisaDescricao_Ok()
        {
            var descricao = "Doom (comercializado como DOOM) é um jogo";

            jogoAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Jogo, bool>>())).Returns(jogos.ListaJogos.Where(x => x.Descricao.ToLower().Contains(descricao.ToLower())).ToList());

            var result = controller.PesquisaJogo(descricao);

            List<Jogo> jogoResult = new Jogos().ListaJogos.Where(x => x.Descricao.ToLower().Contains(descricao.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Jogo>>().Result, jogoResult);
        }

        [TestMethod]
        public void Pesquisa_Jogo_BadRequest()
        {
            var result = controller.PesquisaJogo(string.Empty);

            Jogo jogoResult = new Jogo();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Jogo_NotFound()
        {
            var nome = "Vikings";

            jogoAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Jogo, bool>>())).Returns(new List<Jogo>());
        
            var result = controller.PesquisaJogo(nome);

            var jogoResult = new Jogos().ListaJogos.Where(x => x.Nome == nome).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out jogoResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Message);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Jogo_InternalServerError()
        {
            var nome = "Duke Nukem 3D";

            controller = new JogoController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.PesquisaJogo(nome);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Jogo jogosResult = new Jogos().ListaJogos.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out jogosResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método InserirJogo
        [TestMethod]
        public void Inserir_Jogo_Ok()
        {
            var result = controller.InserirJogo(new JogoModel
            {
                Nome = "Duke Nukem",
                Developer = "3D Realms",
                Publisher = "GT Interactive Software",
                Genero = "Tiro em primeira pessoa",
                Plataforma = "MS-DOS",
                Created = DateTime.Now,
                Novo = true,
                Descricao = "Duke Nukem 3D é um jogo eletrônico de tiro em primeira pessoa desenvolvido pela 3D Realms e publicado pela GT Interactive em Maio de 1996.",
                Lancamento = new DateTime(1996, 1, 29)
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.AreEqual("Jogo inserido com sucesso!", status);
        }

        [TestMethod]
        public void Inserir_Jogo_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.InserirJogo(new JogoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
        }

        [TestMethod]
        public void Inserir_Jogo_Internal_Server_Error()
        {
            controller = new JogoController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.InserirJogo(new JogoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método AtualizarJogo
        [TestMethod]
        public void Atualizar_Jogo_Ok()
        {
            var result = controller.AtualizarJogo(new JogoModel
            {
                Id = 2,
                Nome = "LHX Attack Chopper",
                Developer = "Electronic Arts",
                Publisher = "Electronic Arts",
                Genero = "Simulador de helicóptero",
                Plataforma = "MS-DOS",
                Created = DateTime.Now,
                Novo = true,
                Descricao = "LHX Attack Chopper is a 1990 war helicopter simulation game for the PC by Electronic Arts. The game was developed by Electronic Arts, Design and Programming led by Brent Iverson, also known for the PC DOS version of Chuck Yeager's Air Combat, and US Navy Fighters.",
                Lancamento = new DateTime(1990, 1, 1)
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Jogo atualizado com sucesso!", status);
        }

        [TestMethod]
        public void Atualizar_Jogo_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.AtualizarJogo(new JogoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
        }

        [TestMethod]
        public void Atualizar_Jogo_Internal_Server_Error()
        {
            controller = new JogoController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.AtualizarJogo(new JogoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ExcluirJogo
        [TestMethod]
        public void Excluir_Jogo_Ok()
        {
            var result = controller.ExcluirJogo(new JogoModel
            {
                Id = 1,
                Nome = "Doom",
                Developer = "Id Software",
                Publisher = "Id Software",
                Genero = "Tiro em primeira pessoa",
                Plataforma = "MS-DOS",
                Created = DateTime.Now,
                Novo = true,
                Descricao = "Doom (comercializado como DOOM) é um jogo de computador lançado em 1994 pela id Software e um dos títulos que geraram o gênero tiro em primeira pessoa.",
                Lancamento = new DateTime(1993, 12, 10)
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Jogo excluído com sucesso!", status);
        }

        [TestMethod]
        public void Excluir_Jogo_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.ExcluirJogo(new JogoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
        }

        [TestMethod]
        public void Excluir_Jogo_Internal_Server_Error()
        {
            controller = new JogoController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ExcluirJogo(new JogoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ApagarJogos
        [TestMethod]
        public void Apagar_Jogos_Ok()
        {
            var array = new int[] { 1, 2 };

            var result = controller.ApagarJogos(array);

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Jogo(s) excluído(s) com sucesso!", status);
        }

        [TestMethod]
        public void Apagar_Jogos_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.ApagarJogos(new int[] { });

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Array preenchido incorretamente!", status.Message);
        }

        [TestMethod]
        public void Apagar_Jogos_Internal_Server_Error()
        {
            var array = new int[] { 1, 2 };

            controller = new JogoController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ApagarJogos(array);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

    }
}

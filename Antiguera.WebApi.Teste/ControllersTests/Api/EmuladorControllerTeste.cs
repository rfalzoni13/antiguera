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
    public class EmuladorControllerTeste
    {
        #region Atributos
        private Emuladores emuladores = new Emuladores();
        private Mock<IEmuladorAppServico> emuladorAppServico = new Mock<IEmuladorAppServico>();
        private EmuladorController controller;
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
            controller = new EmuladorController(emuladorAppServico.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }
        #endregion

        #region Método ListarEmuladores
        [TestMethod]
        public void Listar_Emuladores_Ok()
        {
            emuladorAppServico.Setup(x => x.BuscarTodos()).Returns(emuladores.ListaEmuladores);

            var result = controller.ListarTodosEmuladores();

            List<Emulador> emuladoresResult = new Emuladores().ListaEmuladores;

            Assert.IsTrue(result.TryGetContentValue(out emuladoresResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Emulador>>().Result, emuladoresResult);
        }

        [TestMethod]
        public void Listar_Emuladores_NotFound()
        {
            emuladorAppServico.Setup(x => x.BuscarTodos()).Returns(new List<Emulador>());

            var result = controller.ListarTodosEmuladores();

            var emuladoresResult = new Emuladores().ListaEmuladores;

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out emuladoresResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Message);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Emuladores_InternalServerError()
        {
            controller = new EmuladorController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarTodosEmuladores();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            List<Emulador> emuladoresResult = new List<Emulador>();

            Assert.IsFalse(result.TryGetContentValue(out emuladoresResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método ListarEmuladoresPorId
        [TestMethod]
        public void Listar_Emuladores_Por_Id_Ok()
        {
            emuladorAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns(emuladores.ListaEmuladores.Where(x => x.Id == 1).FirstOrDefault());

            var result = controller.ListarEmuladoresPorId(1);

            Emulador emuladorResult = new Emuladores().ListaEmuladores.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsTrue(result.TryGetContentValue(out emuladorResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<Emulador>().Result, emuladorResult);
        }

        [TestMethod]
        public void Listar_Emuladores_Por_Id_BadRequest()
        {
            var result = controller.ListarEmuladoresPorId(It.IsAny<int>());

            Emulador emuladorResult = new Emulador();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out emuladorResult));
            Assert.AreEqual("Parâmetro incorreto!", status.Message);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Listar_Emuladores_Por_Id_NotFound()
        {
            emuladorAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns((Emulador)null);

            var result = controller.ListarEmuladoresPorId(2);

            var emuladorResult = new Emuladores().ListaEmuladores.Where(x => x.Id == 3).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out emuladorResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Message);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Emuladores_Por_Id_InternalServerError()
        {
            controller = new EmuladorController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarEmuladoresPorId(1);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Emulador emuladoresResult = new Emuladores().ListaEmuladores.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out emuladoresResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método PesquisaEmulador
        [TestMethod]
        public void Pesquisa_Emulador_PesquisaId_Ok()
        {
            int id = 1;

            emuladorAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Emulador, bool>>())).Returns(emuladores.ListaEmuladores.Where(x => x.Id == id).ToList());

            var result = controller.PesquisaEmulador(id.ToString());

            List<Emulador> emuladorResult = new Emuladores().ListaEmuladores.Where(x => x.Id == id).ToList();

            Assert.IsTrue(result.TryGetContentValue(out emuladorResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Emulador>>().Result, emuladorResult);
        }

        [TestMethod]
        public void Pesquisa_Emulador_PesquisaNome_Ok()
        {
            var nome = "ZSnes";

            emuladorAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Emulador, bool>>())).Returns(emuladores.ListaEmuladores.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList());

            var result = controller.PesquisaEmulador(nome);

            List<Emulador> emuladorResult = new Emuladores().ListaEmuladores.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out emuladorResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Emulador>>().Result, emuladorResult);
        }

        [TestMethod]
        public void Pesquisa_Emulador_PesquisaConsole_Ok()
        {
            var console = "Mega Drive";

            emuladorAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Emulador, bool>>())).Returns(emuladores.ListaEmuladores.Where(x => x.Console.ToLower().Contains(console.ToLower())).ToList());

            var result = controller.PesquisaEmulador(console);

            List<Emulador> emuladorResult = new Emuladores().ListaEmuladores.Where(x => x.Console.ToLower().Contains(console.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out emuladorResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Emulador>>().Result, emuladorResult);
        }

        [TestMethod]
        public void Pesquisa_Emulador_PesquisaDescricao_Ok()
        {
            var descricao = "Emulador de Super Nintendo";

            emuladorAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Emulador, bool>>())).Returns(emuladores.ListaEmuladores.Where(x => x.Descricao.ToLower().Contains(descricao.ToLower())).ToList());

            var result = controller.PesquisaEmulador(descricao);

            List<Emulador> emuladorResult = new Emuladores().ListaEmuladores.Where(x => x.Descricao.ToLower().Contains(descricao.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out emuladorResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Emulador>>().Result, emuladorResult);
        }

        [TestMethod]
        public void Pesquisa_Emulador_BadRequest()
        {
            var result = controller.PesquisaEmulador(string.Empty);

            Emulador emuladorResult = new Emulador();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out emuladorResult));
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Emulador_NotFound()
        {
            var console = "Playstation";

            emuladorAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Emulador, bool>>())).Returns(new List<Emulador>());

            var result = controller.PesquisaEmulador(console);

            var emuladorResult = new Emuladores().ListaEmuladores.Where(x => x.Console == console).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out emuladorResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Message);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Emulador_InternalServerError()
        {
            var descricao = "rfalzoni13";

            controller = new EmuladorController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.PesquisaEmulador(descricao);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Emulador emuladoresResult = new Emuladores().ListaEmuladores.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out emuladoresResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método InserirEmulador
        [TestMethod]
        public void Inserir_Emulador_Ok()
        {
            var result = controller.InserirEmulador(new EmuladorModel
            {
                Id = 1,
                Nome = "Visual Boy Advance",
                Console = "Game Boy Advance",
                Created = DateTime.Now,
                Novo = true,
                Descricao = "Emulador de Game Boy Advance",
                DataLancamento = new DateTime(2004, 5, 24)
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.AreEqual("Emulador inserido com sucesso!", status);
        }

        [TestMethod]
        public void Inserir_Emulador_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.InserirEmulador(new EmuladorModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
        }

        [TestMethod]
        public void Inserir_Emulador_Internal_Server_Error()
        {
            controller = new EmuladorController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.InserirEmulador(new EmuladorModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método AtualizarEmulador
        [TestMethod]
        public void Atualizar_Emulador_Ok()
        {
            var result = controller.AtualizarEmulador(new EmuladorModel
            {
                Id = 1,
                Nome = "ZSnes Emulador",
                Console = "Super Nintendo Entertainment System",
                Created = DateTime.Now,
                Novo = true,
                Descricao = "Emulador de Super Nintendo",
                DataLancamento = new DateTime(1997, 1, 1)
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Emulador atualizado com sucesso!", status);
        }

        [TestMethod]
        public void Atualizar_Emulador_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.AtualizarEmulador(new EmuladorModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
        }

        [TestMethod]
        public void Atualizar_Emulador_Internal_Server_Error()
        {
            controller = new EmuladorController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.AtualizarEmulador(new EmuladorModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ExcluirEmulador
        [TestMethod]
        public void Excluir_Emulador_Ok()
        {
            var result = controller.ExcluirEmulador(new EmuladorModel
            {
                Id = 2,
                Nome = "Gens",
                Console = "Mega Drive",
                Created = DateTime.Now,
                Novo = true,
                Descricao = "Emulador de Mega Drive",
                DataLancamento = new DateTime(2000, 1, 1)
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Emulador excluído com sucesso!", status);
        }

        [TestMethod]
        public void Excluir_Emulador_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.ExcluirEmulador(new EmuladorModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
        }

        [TestMethod]
        public void Excluir_Emulador_Internal_Server_Error()
        {
            controller = new EmuladorController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ExcluirEmulador(new EmuladorModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ApagarEmuladores
        [TestMethod]
        public void Apagar_Emuladores_Ok()
        {
            var array = new int[] { 1, 2 };

            var result = controller.ApagarEmuladores(array);

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Emulador(es) excluído(s) com sucesso!", status);
        }

        [TestMethod]
        public void Apagar_Emuladores_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.ApagarEmuladores(new int[] { });

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Array preenchido incorretamente!", status.Message);
        }

        [TestMethod]
        public void Apagar_Emuladores_Internal_Server_Error()
        {
            var array = new int[] { 1, 2 };

            controller = new EmuladorController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ApagarEmuladores(array);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion
    }
}

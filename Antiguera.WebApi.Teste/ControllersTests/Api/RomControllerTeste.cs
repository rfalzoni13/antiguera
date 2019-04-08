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
    public class RomControllerTeste
    {
        #region Atributos
        private Roms roms = new Roms();
        private Mock<IRomAppServico> romAppServico = new Mock<IRomAppServico>();
        private RomController controller;
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
            controller = new RomController(romAppServico.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }
        #endregion

        #region Método ListarRoms
        [TestMethod]
        public void Listar_Roms_Ok()
        {
            romAppServico.Setup(x => x.BuscarTodos()).Returns(roms.ListaRoms);

            var result = controller.ListarTodasAsRoms();

            List<Rom> romsResult = new Roms().ListaRoms;

            Assert.IsTrue(result.TryGetContentValue(out romsResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Rom>>().Result, romsResult);
        }

        [TestMethod]
        public void Listar_Roms_NotFound()
        {
            romAppServico.Setup(x => x.BuscarTodos()).Returns(new List<Rom>());

            var result = controller.ListarTodasAsRoms();

            var romsResult = new Roms().ListaRoms;

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out romsResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Message);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Roms_InternalServerError()
        {
            controller = new RomController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarTodasAsRoms();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            List<Rom> romsResult = new List<Rom>();

            Assert.IsFalse(result.TryGetContentValue(out romsResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método ListarRomsPorId
        [TestMethod]
        public void Listar_Roms_Por_Id_Ok()
        {
            romAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns(roms.ListaRoms.Where(x => x.Id == 1).FirstOrDefault());

            var result = controller.ListarRomsPorId(1);

            Rom romResult = new Roms().ListaRoms.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsTrue(result.TryGetContentValue(out romResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<Rom>().Result, romResult);
        }

        [TestMethod]
        public void Listar_Roms_Por_Id_BadRequest()
        {
            var result = controller.ListarRomsPorId(It.IsAny<int>());

            Rom romResult = new Rom();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out romResult));
            Assert.AreEqual("Parâmetro incorreto!", status.Message);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Listar_Roms_Por_Id_NotFound()
        {
            romAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns((Rom)null);

            var result = controller.ListarRomsPorId(2);

            var romResult = new Roms().ListaRoms.Where(x => x.Id == 3).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out romResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Message);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Roms_Por_Id_InternalServerError()
        {
            controller = new RomController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarRomsPorId(1);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Rom romsResult = new Roms().ListaRoms.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out romsResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método PesquisaRom
        [TestMethod]
        public void Pesquisa_Rom_PesquisaId_Ok()
        {
            int id = 1;

            romAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Rom, bool>>())).Returns(roms.ListaRoms.Where(x => x.Id == id).ToList());

            var result = controller.PesquisaRom(id.ToString());

            List<Rom> romResult = new Roms().ListaRoms.Where(x => x.Id == id).ToList();

            Assert.IsTrue(result.TryGetContentValue(out romResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Rom>>().Result, romResult);
        }

        [TestMethod]
        public void Pesquisa_Rom_PesquisaNome_Ok()
        {
            var nome = "Ultimate Mortal Kombat 3";

            romAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Rom, bool>>())).Returns(roms.ListaRoms.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList());

            var result = controller.PesquisaRom(nome);

            List<Rom> romResult = new Roms().ListaRoms.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out romResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Rom>>().Result, romResult);
        }

        [TestMethod]
        public void Pesquisa_Rom_PesquisaPorEmulador_Ok()
        {
            var emulador = "Gens";

            romAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Rom, bool>>())).Returns(roms.ListaRoms.Where(x => x.Emulador.Nome.ToLower().Contains(emulador.ToLower())).ToList());

            var result = controller.PesquisaRom(emulador);

            List<Rom> romResult = new Roms().ListaRoms.Where(x => x.Emulador.Nome.ToLower().Contains(emulador.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out romResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Rom>>().Result, romResult);
        }

        [TestMethod]
        public void Pesquisa_Rom_PesquisaGenero_Ok()
        {
            var genero = "Luta";

            romAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Rom, bool>>())).Returns(roms.ListaRoms.Where(x => x.Genero.ToLower().Contains(genero.ToLower())).ToList());

            var result = controller.PesquisaRom(genero);

            List<Rom> romResult = new Roms().ListaRoms.Where(x => x.Genero.ToLower().Contains(genero.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out romResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Rom>>().Result, romResult);
        }

        [TestMethod]
        public void Pesquisa_Rom_PesquisaDescricao_Ok()
        {
            var descricao = "Super Mario World originalmente chamado no Japão de Super Mario Bros. 4";

            romAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Rom, bool>>())).Returns(roms.ListaRoms.Where(x => x.Descricao.ToLower().Contains(descricao.ToLower())).ToList());

            var result = controller.PesquisaRom(descricao);

            List<Rom> romResult = new Roms().ListaRoms.Where(x => x.Descricao.ToLower().Contains(descricao.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out romResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Rom>>().Result, romResult);
        }

        [TestMethod]
        public void Pesquisa_Rom_BadRequest()
        {
            var result = controller.PesquisaRom(string.Empty);

            Rom romResult = new Rom();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out romResult));
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Rom_NotFound()
        {
            var nome = "Super Mario World";

            romAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Rom, bool>>())).Returns(new List<Rom>());

            var result = controller.PesquisaRom(nome);

            var romResult = new Roms().ListaRoms.Where(x => x.Nome == nome).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out romResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Message);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Rom_InternalServerError()
        {
            var nome = "Super Mario World";

            controller = new RomController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.PesquisaRom(nome);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Rom romsResult = new Roms().ListaRoms.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out romsResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método InserirRom
        [TestMethod]
        public void Inserir_Rom_Ok()
        {
            var result = controller.InserirRom(new RomModel
            {
                EmuladorId = 2,
                Nome = "Sonic the Hedgehog",
                Genero = "Plataforma",
                Created = DateTime.Now,
                Novo = true,
                Descricao = "Sonic the Hedgehog é um jogo eletrônico de plataforma produzido pela Sonic Team e publicado pela Sega para o Sega Genesis. Foi lançado originalmente na América do Norte em junho de 1991 e na região PAL no mês seguinte.",
                DataLancamento = new DateTime(1991, 6, 23)
            });

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.AreEqual("Rom inserida com sucesso!", status);
        }

        [TestMethod]
        public void Inserir_Rom_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.InserirRom(new RomModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
        }

        [TestMethod]
        public void Inserir_Rom_Internal_Server_Error()
        {
            controller = new RomController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.InserirRom(new RomModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método AtualizarRom
        [TestMethod]
        public void Atualizar_Rom_Ok()
        {
            var result = controller.AtualizarRom(new RomModel
            {
                Id = 1,
                EmuladorId = 1,
                Nome = "Super Mario World",
                Genero = "Aventura",
                Created = DateTime.Now,
                Novo = true,
                Descricao = "Super Mario World originalmente chamado no Japão de Super Mario Bros. 4, é um jogo de plataforma desenvolvido e publicado pela Nintendo como um título que acompanhava o console Super Nintendo Entertainment System.",
                DataLancamento = new DateTime(1990, 11, 21)
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Rom atualizada com sucesso!", status);
        }

        [TestMethod]
        public void Atualizar_Rom_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.AtualizarRom(new RomModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
        }

        [TestMethod]
        public void Atualizar_Rom_Internal_Server_Error()
        {
            controller = new RomController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.AtualizarRom(new RomModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ExcluirRom
        [TestMethod]
        public void Excluir_Rom_Ok()
        {
            var result = controller.ExcluirRom(new RomModel
            {
                Id = 2,
                EmuladorId = 2,
                Nome = "Ultimate Mortal Kombat 3",
                Genero = "Luta",
                Created = DateTime.Now,
                Novo = true,
                Descricao = "Ultimate Mortal Kombat 3 é uma atualização do jogo Mortal Kombat 3 seguido por Mortal Kombat Trilogy. Fora lançada em arcades, SNES, Sega Mega Drive, Sega Saturn, no Xbox Live Arcade do Xbox 360, e na versão de luxo de Mortal Kombat: Armageddon para PS2 e Xbox.",
                DataLancamento = new DateTime(1990, 11, 21)
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Rom excluída com sucesso!", status);
        }

        [TestMethod]
        public void Excluir_Rom_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.ExcluirRom(new RomModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Message);
        }

        [TestMethod]
        public void Excluir_Rom_Internal_Server_Error()
        {
            controller = new RomController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ExcluirRom(new RomModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ApagarRoms
        [TestMethod]
        public void Apagar_Roms_Ok()
        {
            var array = new int[] { 1, 2 };

            var result = controller.ApagarRoms(array);

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Rom(s) excluída(s) com sucesso!", status);
        }

        [TestMethod]
        public void Apagar_Roms_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.ApagarRoms(new int[] { });

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Array preenchido incorretamente!", status.Message);
        }

        [TestMethod]
        public void Apagar_Roms_Internal_Server_Error()
        {
            var array = new int[] { 1, 2 };

            controller = new RomController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ApagarRoms(array);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

    }
}

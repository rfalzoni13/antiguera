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
    public class ProgramaControllerTeste
    {
        #region Atributos
        private Programas programas = new Programas();
        private Mock<IProgramaAppServico> programaAppServico = new Mock<IProgramaAppServico>();
        private ProgramaController controller;
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
            controller = new ProgramaController(programaAppServico.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }
        #endregion

        #region Método ListarProgramas
        [TestMethod]
        public void Listar_Programas_Ok()
        {
            programaAppServico.Setup(x => x.BuscarTodos()).Returns(programas.ListaProgramas);

            var result = controller.ListarTodosProgramas();

            List<Programa> programasResult = new Programas().ListaProgramas;

            Assert.IsTrue(result.TryGetContentValue(out programasResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Programa>>().Result, programasResult);
        }

        [TestMethod]
        public void Listar_Programas_NotFound()
        {
            programaAppServico.Setup(x => x.BuscarTodos()).Returns(new List<Programa>());

            var result = controller.ListarTodosProgramas();

            var programasResult = new Programas().ListaProgramas;

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out programasResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Programas_InternalServerError()
        {
            controller = new ProgramaController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarTodosProgramas();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            List<Programa> programasResult = new List<Programa>();

            Assert.IsFalse(result.TryGetContentValue(out programasResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método ListarProgramasPorId
        [TestMethod]
        public void Listar_Programas_Por_Id_Ok()
        {
            programaAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns(programas.ListaProgramas.Where(x => x.Id == 1).FirstOrDefault());

            var result = controller.ListarProgramasPorId(1);

            Programa programaResult = new Programas().ListaProgramas.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsTrue(result.TryGetContentValue(out programaResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<Programa>().Result, programaResult);
        }

        [TestMethod]
        public void Listar_Programas_Por_Id_BadRequest()
        {
            var result = controller.ListarProgramasPorId(It.IsAny<int>());

            Programa programaResult = new Programa();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out programaResult));
            Assert.AreEqual("Parâmetro incorreto!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Listar_Programas_Por_Id_NotFound()
        {
            programaAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns((Programa)null);

            var result = controller.ListarProgramasPorId(2);

            var programaResult = new Programas().ListaProgramas.Where(x => x.Id == 3).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out programaResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Programas_Por_Id_InternalServerError()
        {
            controller = new ProgramaController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarProgramasPorId(1);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Programa programasResult = new Programas().ListaProgramas.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out programasResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método PesquisaPrograma
        [TestMethod]
        public void Pesquisa_Programa_PesquisaId_Ok()
        {
            int id = 1;

            programaAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Programa, bool>>())).Returns(programas.ListaProgramas.Where(x => x.Id == id).ToList());

            var result = controller.PesquisaPrograma(id.ToString());

            List<Programa> programaResult = new Programas().ListaProgramas.Where(x => x.Id == id).ToList();

            Assert.IsTrue(result.TryGetContentValue(out programaResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Programa>>().Result, programaResult);
        }

        [TestMethod]
        public void Pesquisa_Programa_PesquisaNome_Ok()
        {
            var nome = "Visual Studio";

            programaAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Programa, bool>>())).Returns(programas.ListaProgramas.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList());

            var result = controller.PesquisaPrograma(nome);

            List<Programa> programaResult = new Programas().ListaProgramas.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out programaResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Programa>>().Result, programaResult);
        }

        [TestMethod]
        public void Pesquisa_Programa_PesquisaDeveloper_Ok()
        {
            var developer = "Microsoft";

            programaAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Programa, bool>>())).Returns(programas.ListaProgramas.Where(x => x.Developer.ToLower().Contains(developer.ToLower())).ToList());

            var result = controller.PesquisaPrograma(developer);

            List<Programa> programaResult = new Programas().ListaProgramas.Where(x => x.Developer.ToLower().Contains(developer.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out programaResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Programa>>().Result, programaResult);
        }

        [TestMethod]
        public void Pesquisa_Programa_PesquisaPublisher_Ok()
        {
            var publisher = "Microsoft";

            programaAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Programa, bool>>())).Returns(programas.ListaProgramas.Where(x => x.Publisher.ToLower().Contains(publisher.ToLower())).ToList());

            var result = controller.PesquisaPrograma(publisher);

            List<Programa> programaResult = new Programas().ListaProgramas.Where(x => x.Publisher.ToLower().Contains(publisher.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out programaResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Programa>>().Result, programaResult);
        }

        [TestMethod]
        public void Pesquisa_Programa_PesquisaTipoPrograma_Ok()
        {
            var tipo = "Ide";

            programaAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Programa, bool>>())).Returns(programas.ListaProgramas.Where(x => x.TipoPrograma.ToLower().Contains(tipo.ToLower())).ToList());

            var result = controller.PesquisaPrograma(tipo);

            List<Programa> programaResult = new Programas().ListaProgramas.Where(x => x.TipoPrograma.ToLower().Contains(tipo.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out programaResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Programa>>().Result, programaResult);
        }

        [TestMethod]
        public void Pesquisa_Programa_PesquisaDescricao_Ok()
        {
            var descricao = "Microsoft Visual Studio é um ambiente de desenvolvimento integrado (IDE)";

            programaAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Programa, bool>>())).Returns(programas.ListaProgramas.Where(x => x.Descricao.ToLower().Contains(descricao.ToLower())).ToList());

            var result = controller.PesquisaPrograma(descricao);

            List<Programa> programaResult = new Programas().ListaProgramas.Where(x => x.Descricao.ToLower().Contains(descricao.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out programaResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Programa>>().Result, programaResult);
        }

        [TestMethod]
        public void Pesquisa_Programa_BadRequest()
        {
            var result = controller.PesquisaPrograma(string.Empty);

            Programa programaResult = new Programa();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out programaResult));
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Programa_NotFound()
        {
            var nome = "Windows 98";

            programaAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Programa, bool>>())).Returns(new List<Programa>());

            var result = controller.PesquisaPrograma(nome);

            var programaResult = new Programas().ListaProgramas.Where(x => x.Nome == nome).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out programaResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Programa_InternalServerError()
        {
            var nome = "Duke Nukem 3D";

            controller = new ProgramaController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.PesquisaPrograma(nome);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Programa programasResult = new Programas().ListaProgramas.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out programasResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método InserirPrograma
        [TestMethod]
        public void Inserir_Programa_Ok()
        {
            var result = controller.InserirPrograma(new ProgramaModel
            {
                Nome = "Microsoft Office 2007",
                Developer = "Microsoft",
                Publisher = "Microsoft",
                TipoPrograma = "Editores",
                Created = DateTime.Now,
                Novo = false,
                Descricao = "Microsoft Office 2007 foi uma versão do Microsoft Office, parte da família Microsoft Windows de programas de escritório. Formalmente conhecido por Office 12 nas fases iniciais do seu ciclo beta, foi lançado com volume de licença para clientes a 30 de Novembro de 2006[1] e foi disponibilizado para venda a 30 de Janeiro de 2007.",
                Lancamento = new DateTime(2006, 1, 30)
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.AreEqual("Programa inserido com sucesso!", status);
        }

        [TestMethod]
        public void Inserir_Programa_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.InserirPrograma(new ProgramaModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
        }

        [TestMethod]
        public void Inserir_Programa_Internal_Server_Error()
        {
            controller = new ProgramaController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.InserirPrograma(new ProgramaModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método AtualizarPrograma
        [TestMethod]
        public void Atualizar_Programa_Ok()
        {
            var result = controller.AtualizarPrograma(new ProgramaModel
            {
                Id = 1,
                Nome = "Windows 95",
                Developer = "Microsoft",
                Publisher = "Microsoft",
                TipoPrograma = "Sistema Operacional",
                Created = DateTime.Now,
                Novo = true,
                Descricao = "O Microsoft Windows 95 (codinome Chicago) é um sistema operacional de 16/32 bits criado pela empresa Microsoft.",
                Lancamento = new DateTime(1995, 8, 26)
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Programa atualizado com sucesso!", status);
        }

        [TestMethod]
        public void Atualizar_Programa_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.AtualizarPrograma(new ProgramaModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
        }

        [TestMethod]
        public void Atualizar_Programa_Internal_Server_Error()
        {
            controller = new ProgramaController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.AtualizarPrograma(new ProgramaModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ExcluirPrograma
        [TestMethod]
        public void Excluir_Programa_Ok()
        {
            var result = controller.ExcluirPrograma(new ProgramaModel
            {
                Id = 2,
                Nome = "Microsoft Visual Studio 2010",
                Developer = "Microsoft",
                Publisher = "Microsoft",
                TipoPrograma = "IDE Desenvolvimento",
                Created = DateTime.Now,
                Novo = false,
                Descricao = "Microsoft Visual Studio é um ambiente de desenvolvimento integrado (IDE) da Microsoft para desenvolvimento de software especialmente dedicado ao .NET Framework e às linguagens Visual Basic (VB), C, C++, C# (C Sharp) e F# (F Sharp).",
                Lancamento = new DateTime(2010, 4, 12)
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Programa excluído com sucesso!", status);
        }

        [TestMethod]
        public void Excluir_Programa_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.ExcluirPrograma(new ProgramaModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
        }

        [TestMethod]
        public void Excluir_Programa_Internal_Server_Error()
        {
            controller = new ProgramaController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ExcluirPrograma(new ProgramaModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ApagarProgramas
        [TestMethod]
        public void Apagar_Programas_Ok()
        {
            var array = new int[] { 1, 2 };

            var result = controller.ApagarProgramas(array);

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Programa(s) excluído(s) com sucesso!", status);
        }

        [TestMethod]
        public void Apagar_Programas_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.ApagarProgramas(new int[] { });

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Array preenchido incorretamente!", status.Mensagem);
        }

        [TestMethod]
        public void Apagar_Programas_Internal_Server_Error()
        {
            var array = new int[] { 1, 2 };

            controller = new ProgramaController(null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ApagarProgramas(array);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

    }
}

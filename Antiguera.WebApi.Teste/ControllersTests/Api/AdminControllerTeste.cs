using Antiguera.Aplicacao.Interfaces;
using Antiguera.Dominio.Entidades;
using Antiguera.Infra.Cross.Infrastructure;
using Antiguera.WebApi.AutoMapper;
using Antiguera.WebApi.Controllers.Api;
using Antiguera.WebApi.Models;
using Antiguera.WebApi.Teste.ModelsTests;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Antiguera.WebApi.Teste.ControllersTests.Api
{
    [TestClass]
    public class AdminControllerTeste
    {
        #region Atributos
        private static Usuarios usuarios = new Usuarios();
        private static Acessos acessos = new Acessos();
        private static Mock<IUsuarioAppServico> usuarioAppServico = new Mock<IUsuarioAppServico>();
        private static Mock<IAcessoAppServico> acessoAppServico = new Mock<IAcessoAppServico>();
        private static AdminController controller;
        #endregion

        #region Métodos Iniciais
        [ClassInitialize]
        public static void Iniciar_Classe(TestContext context)
        {
            if(!AutoMapperConfig.Iniciado)
            {
                AutoMapperConfig.RegisterMappings();
            }
        }

        [TestInitialize]
        public void Iniciar_Teste()
        {
            controller = new AdminController(usuarioAppServico.Object, acessoAppServico.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var user = new Mock<ApplicationUser>().Object;

            var store = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);

            var roles = new Mock<IList<string>>().Object;

            var manager = new Mock<ApplicationUserManager>(store.Object);

            manager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

            manager.Setup(x => x.GetRolesAsync(It.IsAny<string>())).ReturnsAsync(roles);

            manager.Setup(x => x.FindAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);

            var roleStore = new Mock<IRoleStore<IdentityRole>>(MockBehavior.Strict);
            
            var roleManager = new Mock<ApplicationRoleManager>(roleStore.Object);

            var role = new Mock<IdentityRole>().Object;

            roleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(role);

            controller.UserManager = manager.Object;

            controller.RoleManager = roleManager.Object;
        }
        #endregion

        #region Método ListarUsuarios
        [TestMethod]
        public void Listar_Usuarios_Ok()
        {
            usuarioAppServico.Setup(x => x.BuscarTodos()).Returns(usuarios.ListaUsuarios);

            var result = controller.ListarTodosUsuarios();

            List<Usuario> usuariosResult = new Usuarios().ListaUsuarios;

            Assert.IsTrue(result.TryGetContentValue(out usuariosResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Usuario>>().Result, usuariosResult);
        }

        [TestMethod]
        public void Listar_Usuarios_NotFound()
        {
            usuarioAppServico.Setup(x => x.BuscarTodos()).Returns(new List<Usuario>());

            var result = controller.ListarTodosUsuarios();

            var usuariosResult = new Usuarios().ListaUsuarios;

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out usuariosResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Usuarios_InternalServerError()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarTodosUsuarios();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            List<Usuario> usuariosResult = new List<Usuario>();

            Assert.IsFalse(result.TryGetContentValue(out usuariosResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método ListarUsuariosPorId
        [TestMethod]
        public void Listar_Usuarios_Por_Id_Ok()
        {
            usuarioAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns(usuarios.ListaUsuarios.Where(x => x.Id == 1).FirstOrDefault());

            var result = controller.ListarUsuariosPorId(1);

            Usuario usuarioResult = new Usuarios().ListaUsuarios.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsTrue(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<Usuario>().Result, usuarioResult);
        }

        [TestMethod]
        public void Listar_Usuarios_Por_Id_BadRequest()
        {
            var result = controller.ListarUsuariosPorId(It.IsAny<int>());

            Usuario usuarioResult = new Usuario();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual("Parâmetro incorreto!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Listar_Usuarios_Por_Id_NotFound()
        {
            usuarioAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns((Usuario)null);

            var result = controller.ListarUsuariosPorId(2);

            var usuarioResult = new Usuarios().ListaUsuarios.Where(x => x.Id == 3).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Usuarios_Por_Id_InternalServerError()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarUsuariosPorId(1);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Usuario usuariosResult = new Usuarios().ListaUsuarios.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out usuariosResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método ListarUsuariosPorLoginOuEmail
        [TestMethod]
        public void Listar_Usuarios_Por_Login_Ou_Email_BuscaEmail_Ok()
        {
            var login = "renato.lopes.falzoni@gmail.com";

            usuarioAppServico.Setup(x => x.BuscarUsuarioPorLoginOuEmail(It.IsAny<string>())).Returns(usuarios.ListaUsuarios.Where(x => x.Email == login).FirstOrDefault());

            var result = controller.ListarUsuariosPorLoginOuEmail(login);

            Usuario usuarioResult = new Usuarios().ListaUsuarios.Where(x => x.Email == login).FirstOrDefault();

            Assert.IsTrue(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<Usuario>().Result, usuarioResult);
        }

        [TestMethod]
        public void Listar_Usuarios_Por_Login_Ou_Email_BuscaLogin_Ok()
        {
            var login = "lilifofinha";

            usuarioAppServico.Setup(x => x.BuscarUsuarioPorLoginOuEmail(It.IsAny<string>())).Returns(usuarios.ListaUsuarios.Where(x => x.Login == login).FirstOrDefault());

            var result = controller.ListarUsuariosPorLoginOuEmail(login);

            Usuario usuarioResult = new Usuarios().ListaUsuarios.Where(x => x.Login == login).FirstOrDefault();

            Assert.IsTrue(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<Usuario>().Result, usuarioResult);
        }

        [TestMethod]
        public void Listar_Usuarios_Por_Login_Ou_Email_BadRequest()
        {
            var result = controller.ListarUsuariosPorLoginOuEmail(string.Empty);

            Usuario usuarioResult = new Usuario();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual("Parâmetro incorreto!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Listar_Usuarios_Por_Login_Ou_Email_NotFound()
        {
            var login = "brufalzoni";

            usuarioAppServico.Setup(x => x.BuscarUsuarioPorLoginOuEmail(It.IsAny<string>())).Returns((Usuario)null);

            var result = controller.ListarUsuariosPorLoginOuEmail(login);

            var usuarioResult = new Usuarios().ListaUsuarios.Where(x => x.Login == login).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Usuarios_Por_Login_Ou_Email_InternalServerError()
        {
            var login = "rfalzoni13";

            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarUsuariosPorLoginOuEmail(login);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Usuario usuariosResult = new Usuarios().ListaUsuarios.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out usuariosResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        #endregion

        #region Método PesquisaUsuario
        [TestMethod]
        public void Pesquisa_Usuario_PesquisaId_Ok()
        {
            int id = 1;

            usuarioAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Usuario, bool>>())).Returns(usuarios.ListaUsuarios.Where(x => x.Id == id).ToList());

            var result = controller.PesquisaUsuario(id.ToString());

            List<Usuario> usuarioResult = new Usuarios().ListaUsuarios.Where(x => x.Id == id).ToList();

            Assert.IsTrue(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Usuario>>().Result, usuarioResult);
        }

        [TestMethod]
        public void Pesquisa_Usuario_PesquisaNome_Ok()
        {
            var nome = "Renato";

            usuarioAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Usuario, bool>>())).Returns(usuarios.ListaUsuarios.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList());

            var result = controller.PesquisaUsuario(nome);

            List<Usuario> usuarioResult = new Usuarios().ListaUsuarios.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Usuario>>().Result, usuarioResult);
        }

        [TestMethod]
        public void Pesquisa_Usuario_PesquisaEmail_Ok()
        {
            var email = "lilifofinha@gmail.com";

            usuarioAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Usuario, bool>>())).Returns(usuarios.ListaUsuarios.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList());

            var result = controller.PesquisaUsuario(email);

            List<Usuario> usuarioResult = new Usuarios().ListaUsuarios.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Usuario>>().Result, usuarioResult);
        }

        [TestMethod]
        public void Pesquisa_Usuario_PesquisaLogin_Ok()
        {
            var login = "rfalzoni13";

            usuarioAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Usuario, bool>>())).Returns(usuarios.ListaUsuarios.Where(x => x.Login.ToLower().Contains(login.ToLower())).ToList());

            var result = controller.PesquisaUsuario(login);

            List<Usuario> usuarioResult = new Usuarios().ListaUsuarios.Where(x => x.Login.ToLower().Contains(login.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Usuario>>().Result, usuarioResult);
        }

        [TestMethod]
        public void Pesquisa_Usuario_PesquisaAcesso_Ok()
        {
            var nome = "Administrador";

            usuarioAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Usuario, bool>>())).Returns(usuarios.ListaUsuarios.Where(x => x.Acesso.Nome.ToLower().Contains(nome.ToLower())).ToList());

            var result = controller.PesquisaUsuario(nome);

            List<Usuario> usuarioResult = new Usuarios().ListaUsuarios.Where(x => x.Acesso.Nome.ToLower().Contains(nome.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Usuario>>().Result, usuarioResult);
        }

        [TestMethod]
        public void Pesquisa_Usuario_BadRequest()
        {
            var result = controller.PesquisaUsuario(string.Empty);

            Usuario usuarioResult = new Usuario();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Usuario_NotFound()
        {
            var login = "brufalzoni";

            usuarioAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Usuario, bool>>())).Returns(new List<Usuario>());

            var result = controller.PesquisaUsuario(login);

            var usuarioResult = new Usuarios().ListaUsuarios.Where(x => x.Login == login).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out usuarioResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Usuario_InternalServerError()
        {
            var login = "rfalzoni13";

            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.PesquisaUsuario(login);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Usuario usuariosResult = new Usuarios().ListaUsuarios.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out usuariosResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método ListarTodosAcessos
        [TestMethod]
        public void Listar_Todos_Acessos_Ok()
        {
            acessoAppServico.Setup(x => x.BuscarTodos()).Returns(acessos.ListaAcessos);

            var result = controller.ListarTodosAcessos();

            List<Acesso> acessosResult = new Acessos().ListaAcessos;

            Assert.IsTrue(result.TryGetContentValue(out acessosResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Acesso>>().Result, acessosResult);
        }

        [TestMethod]
        public void Listar_Todos_Acessos_Not_Found()
        {
            acessoAppServico.Setup(x => x.BuscarTodos()).Returns(new List<Acesso>());

            var result = controller.ListarTodosAcessos();

            var acessosResult = new Acessos().ListaAcessos;

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out acessosResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Todos_Acessos_InternalServerError()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarTodosAcessos();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            List<Usuario> usuariosResult = new List<Usuario>();

            Assert.IsFalse(result.TryGetContentValue(out usuariosResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método ListarAcessosPorId
        [TestMethod]
        public void Listar_Acessos_Por_Id_Ok()
        {
            acessoAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns(acessos.ListaAcessos.Where(x => x.Id == 1).FirstOrDefault());

            var result = controller.ListarAcessosPorId(1);

            Acesso acessoResult = new Acessos().ListaAcessos.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsTrue(result.TryGetContentValue(out acessoResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<Acesso>().Result, acessoResult);
        }

        [TestMethod]
        public void Listar_Acessos_Por_Id_BadRequest()
        {
            var result = controller.ListarAcessosPorId(It.IsAny<int>());

            Acesso acessoResult = new Acesso();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out acessoResult));
            Assert.AreEqual("Parâmetro incorreto!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Listar_Acessos_Por_Id_NotFound()
        {
            acessoAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns((Acesso)null);

            var result = controller.ListarAcessosPorId(2);

            var acessoResult = new Acessos().ListaAcessos.Where(x => x.Id == 2).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out acessoResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Listar_Acessos_Por_Id_InternalServerError()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ListarAcessosPorId(1);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Usuario usuariosResult = new Usuarios().ListaUsuarios.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out usuariosResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método PesquisaAcesso
        [TestMethod]
        public void Pesquisa_Acesso_PesquisaId_Ok()
        {
            int id = 1;

            acessoAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Acesso, bool>>())).Returns(acessos.ListaAcessos.Where(x => x.Id == id).ToList());

            var result = controller.PesquisaAcesso(id.ToString());

            List<Acesso> acessoResult = new Acessos().ListaAcessos.Where(x => x.Id == id).ToList();

            Assert.IsTrue(result.TryGetContentValue(out acessoResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Acesso>>().Result, acessoResult);
        }

        [TestMethod]
        public void Pesquisa_Acesso_PesquisaNome_Ok()
        {
            var nome = "Usuário";

            acessoAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Acesso, bool>>())).Returns(acessos.ListaAcessos.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList());

            var result = controller.PesquisaAcesso(nome);

            List<Acesso> acessoResult = new Acessos().ListaAcessos.Where(x => x.Nome.ToLower().Contains(nome.ToLower())).ToList();

            Assert.IsTrue(result.TryGetContentValue(out acessoResult));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(result.Content.ReadAsAsync<List<Acesso>>().Result, acessoResult);
        }

        [TestMethod]
        public void Pesquisa_Acesso_BadRequest()
        {
            var result = controller.PesquisaAcesso(string.Empty);

            Acesso acessoResult = new Acesso();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out acessoResult));
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Acesso_NotFound()
        {
            var nome = "Financeiro";

            acessoAppServico.Setup(u => u.BuscaQuery(It.IsAny<Func<Acesso, bool>>())).Returns(new List<Acesso>());

            var result = controller.PesquisaAcesso(nome);

            var acessoResult = new Acessos().ListaAcessos.Where(x => x.Nome == nome).FirstOrDefault();

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.IsFalse(result.TryGetContentValue(out acessoResult));
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
        }

        [TestMethod]
        public void Pesquisa_Acesso_InternalServerError()
        {
            var nome = "Administrador";

            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.PesquisaAcesso(nome);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Acesso usuariosResult = new Acessos().ListaAcessos.Where(x => x.Id == 1).FirstOrDefault();

            Assert.IsFalse(result.TryGetContentValue(out usuariosResult));
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método InserirUsuario
        [TestMethod]
        public async Task Inserir_Usuario_Ok()
        {
            acessoAppServico.Setup(x => x.BuscarPorId(2)).Returns(acessos.ListaAcessos.Where(x => x.Id == 2).FirstOrDefault());

            var result = await controller.InserirUsuario(new UsuarioModel
            {
                Nome = "Liliane Lopes da Silva",
                Sexo = "Feminino",
                Senha = "123456",
                Email = "lilianelopes@gmail.com",
                Login = "lililopes",
                AcessoId = 2,
            });


            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.AreEqual("Usuário incluído com sucesso!", status);
        }

        [TestMethod]
        public async Task Inserir_Usuario_Not_Found()
        {
            acessoAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns((Acesso)null);

            var result = await controller.InserirUsuario(new UsuarioModel
            {
                Nome = "Amadeu Falzoni Neto",
                Sexo = "Masculino",
                Senha = "654321",
                Email = "zarro.shop@gmail.com",
                Login = "zarro",
                AcessoId = 2,
            });


            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
            Assert.AreEqual("Id de acesso não localizado!", status.Mensagem);
        }

        [TestMethod]
        public async Task Inserir_Usuario_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = await controller.InserirUsuario(new UsuarioModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
        }

        [TestMethod]
        public async Task Inserir_Usuario_Internal_Server_Error()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = await controller.InserirUsuario(new UsuarioModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método InserirAcesso
        [TestMethod]
        public async Task Inserir_Acesso_Ok()
        {
            var result = await controller.InserirAcesso(new AcessoModel
            {
                Nome = "Financeiro"
            });

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.AreEqual("Acesso incluído com sucesso!", status);
        }

        [TestMethod]
        public async Task Inserir_Acesso_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = await controller.InserirAcesso(new AcessoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
        }

        [TestMethod]
        public async Task Inserir_Acesso_Internal_Server_Error()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = await controller.InserirAcesso(new AcessoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método LoginAdmin
        [TestMethod]
        public async Task Login_Admin_Ok()
        {
            var result = await controller.LoginAdmin(new LoginModel
            {
                UserName = "rfalzoni13",
                Password = "123456"
            });

            var status = result.Content.ReadAsAsync<ApplicationUser>().Result;


            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public async Task Login_Admin_Usuario_Ou_Senha_Incorretos()
        {
            var store = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);

            var manager = new Mock<ApplicationUserManager>(store.Object);

            manager.Setup(x => x.FindAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            controller.UserManager = manager.Object;

            var result = await controller.LoginAdmin(new LoginModel
            {
                UserName = "rfalzoni13",
                Password = "123456"
            });

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual("Login ou senha incorretos!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task Login_Admin_Usuario_Campos_Vazios()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = await controller.LoginAdmin(new LoginModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task Login_Admin_Usuario_Internal_Server_Error()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = await controller.LoginAdmin(new LoginModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        }
        #endregion

        #region Método AtualizarUsuario
        [TestMethod]
        public async Task Atualizar_Usuario_Ok()
        {
            acessoAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns(acessos.ListaAcessos.Where(x => x.Id == 1).FirstOrDefault());

            var result = await controller.AtualizarUsuario(new UsuarioModel
            {
                Id = 1,
                Nome = "Renato Lopes Falzoni",
                Sexo = "Masculino",
                Senha = "123456",
                Email = "renato.lopes.falzoni@gmail.com",
                Login = "rfalzoni",
                AcessoId = 1
            });

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Usuário atualizado com sucesso!", status);
        }

        [TestMethod]
        public async Task Atualizar_Usuario_Id_De_Acesso_Not_Found()
        {
            acessoAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns((Acesso)null);

            var result = await controller.AtualizarUsuario(new UsuarioModel
            {
                Id = 2,
                Nome = "Lilian Lopes da Silva",
                Sexo = "Feminino",
                Senha = "123456",
                Email = "lilifofinha@gmail.com.br",
                Login = "lilifofinha",
                AcessoId = 2
            });


            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
        }

        [TestMethod]
        public async Task Atualizar_Usuario_Usuario_Not_Found()
        {
            var store = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);

            var manager = new Mock<ApplicationUserManager>(store.Object);

            controller.UserManager = manager.Object;

            var result = await controller.AtualizarUsuario(new UsuarioModel
            {
                Id = 2,
                Nome = "Lilian Lopes da Silva",
                Sexo = "Feminino",
                Senha = "123456",
                Email = "lilifofinha@gmail.com.br",
                Login = "lilifofinha",
                AcessoId = 2
            });


            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
        }

        [TestMethod]
        public async Task Atualizar_Usuario_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = await controller.AtualizarUsuario(new UsuarioModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
        }

        [TestMethod]
        public async Task Atualizar_Usuario_Internal_Server_Error()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = await controller.AtualizarUsuario(new UsuarioModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método AtualizarSenhaUsuario
        [TestMethod]
        public async Task Atualizar_Senha_Usuario_Ok()
        {
            acessoAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns(acessos.ListaAcessos.Where(x => x.Id == 1).FirstOrDefault());

            var result = await controller.AtualizarSenhaUsuario(new UsuarioModel
            {
                Id = 1,
                Nome = "Renato Lopes Falzoni",
                Sexo = "Masculino",
                Senha = "654321",
                Email = "renato.lopes.falzoni@gmail.com",
                Login = "rfalzoni",
                AcessoId = 1
            });

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Senha alterada com sucesso!", status);
        }

        [TestMethod]
        public async Task Atualizar_Senha_Usuario_Not_Found()
        {
            var store = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);

            var manager = new Mock<ApplicationUserManager>(store.Object);

            controller.UserManager = manager.Object;

            var result = await controller.AtualizarSenhaUsuario(new UsuarioModel
            {
                Id = 2,
                Nome = "Lilian Lopes da Silva",
                Sexo = "Feminino",
                Senha = "654321",
                Email = "lilifofinha@gmail.com.br",
                Login = "lilifofinha",
                AcessoId = 2
            });


            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
        }

        [TestMethod]
        public async Task Atualizar_Senha_Usuario_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = await controller.AtualizarSenhaUsuario(new UsuarioModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
        }

        [TestMethod]
        public async Task Atualizar_Senha_Usuario_Internal_Server_Error()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = await controller.AtualizarSenhaUsuario(new UsuarioModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }

        #endregion

        #region Método AtualizarAcesso
        [TestMethod]
        public async Task Atualizar_Acesso_Ok()
        {
            acessoAppServico.Setup(x => x.BuscarPorId(2)).Returns(acessos.ListaAcessos.Where(x => x.Id == 2).FirstOrDefault());

            var result = await controller.AtualizarAcesso(new AcessoModel
            {
                Id = 2,
                Nome = "RH"
            });

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Acesso atualizado com sucesso!", status);
        }

        [TestMethod]
        public async Task Atualizar_Acesso_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = await controller.AtualizarAcesso(new AcessoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
        }

        [TestMethod]
        public async Task Atualizar_Acesso_Not_Found()
        {
            acessoAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns((Acesso)null);

            var roleStore = new Mock<IRoleStore<IdentityRole>>(MockBehavior.Strict);

            var roleManager = new Mock<ApplicationRoleManager>(roleStore.Object);

            controller.RoleManager = roleManager.Object;

            var result = await controller.AtualizarAcesso(new AcessoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
        }

        [TestMethod]
        public async Task Atualizar_Acesso_Internal_Server_Error()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = await controller.AtualizarAcesso(new AcessoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ExcluirUsuario
        [TestMethod]
        public async Task Excluir_Usuario_Ok()
        {
            var result = await controller.ExcluirUsuario(new UsuarioModel
            {
                Id = 1,
                Nome = "Renato Lopes Falzoni",
                Sexo = "Masculino",
                Senha = "123456",
                Email = "renato.lopes.falzoni@gmail.com",
                Login = "rfalzoni",
                AcessoId = 1
            });

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Usuário excluído com sucesso!", status);
        }

        [TestMethod]
        public async Task Excluir_Usuario_Not_Found()
        {
            var store = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);

            var manager = new Mock<ApplicationUserManager>(store.Object);

            controller.UserManager = manager.Object;

            var result = await controller.ExcluirUsuario(new UsuarioModel
            {
                Id = 2,
                Nome = "Lilian Lopes da Silva",
                Sexo = "Feminino",
                Senha = "123456",
                Email = "lilifofinha@gmail.com.br",
                Login = "lilifofinha",
                AcessoId = 2
            });


            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
        }

        [TestMethod]
        public async Task Excluir_Usuario_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = await controller.ExcluirUsuario(new UsuarioModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
        }

        [TestMethod]
        public async Task Excluir_Usuario_Internal_Server_Error()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = await controller.ExcluirUsuario(new UsuarioModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ApagarUsuarios
        [TestMethod]
        public void Apagar_Usuarios_Ok()
        {
            var array = new int[] { 1, 2 };

            var result = controller.ApagarUsuarios(array);

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Usuário(s) excluído(s) com sucesso!", status);
        }

        [TestMethod]
        public void Apagar_Usuarios_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");
            
            var result = controller.ApagarUsuarios(new int[] { });

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Array preenchido incorretamente!", status.Mensagem);
        }

        [TestMethod]
        public void Apagar_Usuarios_Internal_Server_Error()
        {
            var array = new int[] { 1, 2 };

            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ApagarUsuarios(array);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ExcluirAcesso
        [TestMethod]
        public async Task Excluir_Acesso_Ok()
        {
            var result = await controller.ExcluirAcesso(new AcessoModel
            {
                Id = 2,
                Nome = "Usuário"
            });

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Acesso excluído com sucesso!", status);
        }

        [TestMethod]
        public async Task Excluir_Acesso_Not_Found()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>(MockBehavior.Strict);

            var roleManager = new Mock<ApplicationRoleManager>(roleStore.Object);

            controller.RoleManager = roleManager.Object;

            var result = await controller.ExcluirAcesso(new AcessoModel
            {
                Id = 1,
                Nome = "Administrador"
            });


            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, status.Status);
            Assert.AreEqual("Nenhum registro encontrado!", status.Mensagem);
        }

        [TestMethod]
        public async Task Excluir_Acesso_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = await controller.ExcluirAcesso(new AcessoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Por favor, preencha os campos corretamente!", status.Mensagem);
        }

        [TestMethod]
        public async Task Excluir_Acesso_Internal_Server_Error()
        {
            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = await controller.ExcluirAcesso(new AcessoModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion

        #region Método ApagarAcessos
        [TestMethod]
        public void Apagar_Acessos_Ok()
        {
            var array = new int[] { 1, 2 };

            var result = controller.ApagarAcessos(array);

            var status = result.Content.ReadAsAsync<string>().Result;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Acesso(s) excluído(s) com sucesso!", status);
        }

        [TestMethod]
        public void Apagar_Acessos_Bad_Request()
        {
            controller.ModelState.AddModelError(string.Empty, "teste");

            var result = controller.ApagarAcessos(new int[] { });

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, status.Status);
            Assert.AreEqual("Array preenchido incorretamente!", status.Mensagem);
        }

        [TestMethod]
        public void Apagar_Acessos_Internal_Server_Error()
        {
            var array = new int[] { 1, 2 };

            controller = new AdminController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = controller.ApagarAcessos(array);

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }
        #endregion


    }
}

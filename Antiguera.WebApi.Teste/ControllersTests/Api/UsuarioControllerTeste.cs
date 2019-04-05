using Antiguera.Aplicacao.Interfaces;
using Antiguera.Dominio.Entidades;
using Antiguera.Infra.Cross.Infrastructure;
using Antiguera.WebApi.AutoMapper;
using Antiguera.WebApi.Controllers.Api;
using Antiguera.WebApi.Models;
using Antiguera.WebApi.Teste.ModelsTests;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Antiguera.WebApi.Teste.ControllersTests.Api
{
    [TestClass]
    public class UsuarioControllerTeste
    {
        #region Atributos
        private Usuarios usuarios = new Usuarios();
        private Acessos acessos = new Acessos();
        private Mock<IUsuarioAppServico> usuarioAppServico = new Mock<IUsuarioAppServico>();
        private Mock<IAcessoAppServico> acessoAppServico = new Mock<IAcessoAppServico>();
        private UsuarioController controller;
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
            controller = new UsuarioController(usuarioAppServico.Object, acessoAppServico.Object)
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

            var roleStore = new Mock<IRoleStore<IdentityRole>>(MockBehavior.Strict);

            var roleManager = new Mock<ApplicationRoleManager>(roleStore.Object);

            var role = new Mock<IdentityRole>().Object;

            roleManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(role);

            controller.UserManager = manager.Object;

            controller.RoleManager = roleManager.Object;
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
            controller = new UsuarioController(null, null);
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

            controller = new UsuarioController(null, null);
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
            controller = new UsuarioController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = await controller.InserirUsuario(new UsuarioModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
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
            acessoAppServico.Setup(x => x.BuscarPorId(1)).Returns(new Acesso());

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
            acessoAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns((Acesso)null);

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
            controller = new UsuarioController(null, null);
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
            acessoAppServico.Setup(x => x.BuscarPorId(It.IsAny<int>())).Returns((Acesso)null);

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
            controller = new UsuarioController(null, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var result = await controller.AtualizarSenhaUsuario(new UsuarioModel());

            var status = result.Content.ReadAsAsync<StatusCode>().Result;

            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual(HttpStatusCode.InternalServerError, status.Status);
        }

        #endregion
    }
}

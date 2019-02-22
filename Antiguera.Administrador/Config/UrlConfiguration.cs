﻿using System.Configuration;

namespace Antiguera.Administrador.Config
{
    public class UrlConfiguration
    {
        #region Url Principal
        public string UrlApi => ConfigurationManager.AppSettings["AmbienteApi"] == "true" ?
            ConfigurationManager.AppSettings["UrlApiProd"] :
            ConfigurationManager.AppSettings["UrlApiDev"];
        #endregion

        #region Login
        public string UrlToken => "api/antiguera/token";

        public string UrlLoginAdmin => "api/antiguera/admin/login";
        #endregion

        #region Admin
        public string UrlListarTodosUsuarios => "api/antiguera/admin/listartodososusuarios";

        public string UrlListarUsuariosPorId => "api/antiguera/admin/listarusuariosporid?id=";

        public string UrlListarUsuariosPeloLoginOuEmail => "api/antiguera/admin/listarusuariosporloginouemail?userData=";

        public string UrlInserirUsuario => "api/antiguera/admin/inserirusuario";

        public string UrlAtualizarUsuario => "api/antiguera/admin/atualizarusuario";

        public string UrlAtualizarAdmin => "api/antiguera/admin/atualizaradmin";

        public string UrlAtualizarSenhaUsuario => "api/antiguera/admin/atualizarsenhausuario";

        public string UrlAtualizarSenhaAdmin => "api/antiguera/admin/atualizarsenhaadmin";

        public string UrlAtualizarUsuarioNovo => "api/antiguera/admin/atualizarusuarionovo";

        public string UrlExcluirUsuario => "api/antiguera/admin/excluirusuario";

        public string UrlApagarUsuarios => "api/antiguera/admin/apagarusuarios";

        public string UrlListarTodosAcessos = "api/antiguera/admin/listartodososacessos";

        public string UrlListarAcessoPorId = "api/antiguera/admin/listaracessosporid?id=";

        public string UrlInserirAcesso = "api/antiguera/admin/inseriracesso";

        public string UrlAtualizarAcessoNovo => "api/antiguera/admin/atualizaracessonovo";

        public string UrlExcluirAcesso = "api/antiguera/admin/excluiracesso";

        public string UrlApagarAcessos = "api/antiguera/admin/apagaracessos";
        #endregion

        #region Emulador
        public string UrlListarTodosEmuladores => "api/antiguera/admin/emulador/listartodososemuladores";

        public string UrlListarEmuladorPorId => "api/antiguera/admin/emulador/listaremuladoresporid?id=";

        public string UrlInserirEmulador => "api/antiguera/admin/emulador/inseriremulador";

        public string UrlAtualizarEmulador => "api/antiguera/admin/emulador/atualizaremulador";

        public string UrlAtualizarEmuladorNovo => "api/antiguera/admin/emulador/atualizaremuladornovo";

        public string UrlExcluirEmulador => "api/antiguera/admin/emulador/excluiremulador";

        public string UrlApagarEmuladores => "api/antiguera/admin/emulador/apagaremuladores";
        #endregion

        #region Rom
        public string UrlListarTodasRoms => "api/antiguera/admin/rom/listartodasasroms";

        public string UrlListarRomPorId => "api/antiguera/admin/rom/listarromsporid?id=";

        public string UrlInserirRom => "api/antiguera/admin/rom/inserirrom";

        public string UrlAtualizarRom => "api/antiguera/admin/rom/atualizarrom";

        public string UrlAtualizarRomNova => "api/antiguera/admin/rom/atualizarromnova";

        public string UrlExcluirRom => "api/antiguera/admin/rom/excluirrom";

        public string UrlApagarRoms => "api/antiguera/admin/rom/apagarroms";
        #endregion

        #region Jogo
        public string UrlListarTodosJogos => "api/antiguera/admin/jogo/listartodososjogos";

        public string UrlListarJogoPorId => "api/antiguera/admin/jogo/listarjogosporid?id=";

        public string UrlInserirJogo => "api/antiguera/admin/jogo/inserirjogo";

        public string UrlAtualizarJogo => "api/antiguera/admin/jogo/atualizarjogo";

        public string UrlAtualizarJogoNovo => "api/antiguera/admin/jogo/atualizarjogonovo";

        public string UrlExcluirJogo => "api/antiguera/admin/jogo/excluirjogo";

        public string UrlApagarJogos => "api/antiguera/admin/jogo/apagarjogos";
        #endregion

        #region Programa
        public string UrlListarTodosProgramas => "api/antiguera/admin/programa/listartodososprogramas";

        public string UrlListarProgramaPorId => "api/antiguera/admin/programa/listarprogramasporid?id=";

        public string UrlInserirPrograma => "api/antiguera/admin/programa/inserirprograma";

        public string UrlAtualizarPrograma => "api/antiguera/admin/programa/atualizarprograma";

        public string UrlAtualizarProgramaNovo => "api/antiguera/admin/programa/atualizarprogramanovo";

        public string UrlExcluirPrograma => "api/antiguera/admin/programa/excluirprograma";

        public string UrlApagarProgramas => "api/antiguera/admin/programa/apagarprogramas";
        #endregion
    }
}
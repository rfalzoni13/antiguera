﻿using Antiguera.Aplicacao.Interfaces;
using Antiguera.Aplicacao.Servicos.Base;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Servicos;

namespace Antiguera.Aplicacao.Servicos
{
    public class UsuarioAppServico : AppServicoBase<Usuario>, IUsuarioAppServico
    {
        private readonly IUsuarioServico _usuarioServico;

        public UsuarioAppServico(IUsuarioServico usuarioServico)
            :base(usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        public void AlterarSenha(int id, string senha)
        {
            _usuarioServico.AlterarSenha(id, senha);
        }

        public Usuario FazerLogin(string userName, string password) => _usuarioServico.FazerLogin(userName, password);

        public void ApagarUsuarios(int[] Ids)
        {
            _usuarioServico.ApagarUsuarios(Ids);
        }

        public Usuario BuscarUsuarioPorLoginOuEmail(string data) => _usuarioServico.BuscarUsuarioPorLoginOuEmail(data);
    }
}
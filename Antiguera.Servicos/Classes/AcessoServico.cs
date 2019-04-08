using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Servicos.Base;
using System;
using System.Linq;

namespace Antiguera.Servicos.Classes
{
    public class AcessoServico : ServicoBase<Acesso>, IAcessoServico
    {
        private readonly IAcessoRepositorio _acessoRepositorio;

        public AcessoServico(IAcessoRepositorio acessoRepositorio)
            : base(acessoRepositorio)
        {
            _acessoRepositorio = acessoRepositorio;
        }

        public void ApagarAcessos(int[] Ids)
        {
            if(Ids != null && Ids.Count() > 0)
            {
                foreach (var id in Ids)
                {
                    var acesso = _acessoRepositorio.BuscarPorId(id);

                    if (acesso != null)
                    {
                        _acessoRepositorio.Apagar(acesso);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Parâmetro inválido!");
            }
            
        }
    }
}

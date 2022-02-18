using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Enum;
using Antiguera.Dominio.Helpers;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Repositorios.Base;
using System;
using System.Threading.Tasks;

namespace Antiguera.Infra.Data.Repositorios
{
    public class HistoricoRepositorio : RepositorioBase<Historico>, IHistoricoRepositorio
    {
        public async Task GravarHistorico(int usuarioId, ETipoHistorico tipoHistorico)
        {
            var historico = new Historico
            {
                UsuarioId = usuarioId,
                Data = DateTime.Now,
                TipoHistorico = StringHelper.GetEnumDescription(tipoHistorico),
                Novo = true,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            Context.Set<Historico>().Add(historico);
            await Context.SaveChangesAsync();
            //_logger.Info("Histórico atualizado na data de " + DateTime.Now.ToString());
        }

    }
}

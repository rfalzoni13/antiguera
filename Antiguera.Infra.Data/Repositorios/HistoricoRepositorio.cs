using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Enum;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Infra.Data.Contexto;
using Antiguera.Infra.Data.Repositorios.Base;
using Antiguera.Utils.Helpers;
using System;
using System.Threading.Tasks;

namespace Antiguera.Infra.Data.Repositorios
{
    public class HistoricoRepositorio : RepositorioBase<Historico>, IHistoricoRepositorio
    {
        private AntigueraContexto _context;

        public HistoricoRepositorio(AntigueraContexto context)
            : base(context)
        {
            _context = context;
        }

        public async Task GravarHistorico(Guid usuarioId, ETipoHistorico tipoHistorico)
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

            _context.Set<Historico>().Add(historico);
            await _context.SaveChangesAsync();
            //_logger.Info("Histórico atualizado na data de " + DateTime.Now.ToString());
        }

    }
}

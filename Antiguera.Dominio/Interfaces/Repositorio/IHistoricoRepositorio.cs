using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Enum;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using System.Threading.Tasks;

namespace Antiguera.Dominio.Interfaces.Repositorio
{
    public interface IHistoricoRepositorio : IRepositorioBase<Historico>
    {
        Task GravarHistorico(int usuarioId, ETipoHistorico tipoHistorico);
    }
}

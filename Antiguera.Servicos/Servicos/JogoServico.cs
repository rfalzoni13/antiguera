using Antiguera.Dominio.DTO;
using Antiguera.Dominio.Entidades;
using Antiguera.Dominio.Interfaces.Repositorio;
using Antiguera.Dominio.Interfaces.Repositorio.Base;
using Antiguera.Dominio.Interfaces.Servicos;
using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using Antiguera.Servicos.Servicos.Base;

namespace Antiguera.Servicos.Servicos
{
    public class JogoServico : ServicoBase<JogoDTO, Jogo>, IJogoServico
    {
        private readonly IJogoRepositorio _jogoRepositorio;

        public JogoServico(IJogoRepositorio jogoRepositorio, IUnitOfWork unitOfWork,
            IConvertHelper<JogoDTO, Jogo> convertToEntity, IConvertHelper<Jogo, JogoDTO> convertToDTO)
            :base(jogoRepositorio, unitOfWork, convertToEntity, convertToDTO)
        {
            _jogoRepositorio = jogoRepositorio;
        }
    }
}

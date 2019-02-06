using Antiguera.Dominio.Entidades;
using Antiguera.WebApi.Models;
using AutoMapper;

namespace Antiguera.WebApi.AutoMapper
{
    public class ModelToDomainMappingProfile : Profile
    {
        public ModelToDomainMappingProfile()
        {
            CreateMap<UsuarioModel, Usuario>();
            CreateMap<AcessoModel, Acesso>();
            CreateMap<JogoModel, Jogo>();
            CreateMap<EmuladorModel, Emulador>();
            CreateMap<RomModel, Rom>();
            CreateMap<ProgramaModel, Programa>();
        }
    }
}
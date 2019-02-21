using Antiguera.Administrador.DTOs;
using Antiguera.Administrador.Models;
using AutoMapper;

namespace Antiguera.Administrador.AutoMapper
{
    public class ModelToDTOMappingProfile : Profile
    {
        public ModelToDTOMappingProfile()
        {
            CreateMap<UsuarioModel, UsuarioDTO>();
            CreateMap<JogoModel, JogoDTO>();
            CreateMap<AcessoModel, AcessoDTO>();
            CreateMap<ProgramaModel, ProgramaDTO>();
            CreateMap<EmuladorModel, EmuladorDTO>();
            CreateMap<RomModel, RomDTO>();
        }
    }
}
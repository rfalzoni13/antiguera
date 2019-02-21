using Antiguera.Administrador.DTOs;
using Antiguera.Administrador.Models;
using AutoMapper;

namespace Antiguera.Administrador.AutoMapper
{
    public class DTOToModelMappingProfile : Profile
    {
        public DTOToModelMappingProfile()
        {
            CreateMap<UsuarioDTO, UsuarioModel>();
            CreateMap<JogoDTO, JogoModel>();
            CreateMap<AcessoDTO, AcessoModel>();
            CreateMap<ProgramaDTO, ProgramaModel>();
            CreateMap<EmuladorDTO, EmuladorModel>();
            CreateMap<RomDTO, RomModel>();
        }
    }
}
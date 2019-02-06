using Antiguera.Dominio.Entidades;
using Antiguera.WebApi.Models;
using AutoMapper;

namespace Antiguera.WebApi.AutoMapper
{
    public class DomainToModelMappingProfile : Profile
    {
        public DomainToModelMappingProfile()
        {
            CreateMap<Usuario, UsuarioModel>();
            CreateMap<Acesso, AcessoModel>();
            CreateMap<Jogo, JogoModel>();
            CreateMap<Emulador, EmuladorModel>();
            CreateMap<Rom, RomModel>();
            CreateMap<Programa, ProgramaModel>();
        }
    }
}
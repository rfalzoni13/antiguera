using AutoMapper;

namespace Antiguera.Administrador.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ModelToDTOMappingProfile>();
                x.AddProfile<DTOToModelMappingProfile>();
            });
        }
    }
}
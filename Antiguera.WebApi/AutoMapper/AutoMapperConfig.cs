using AutoMapper;

namespace Antiguera.WebApi.AutoMapper
{
    public class AutoMapperConfig
    {
        public static bool Iniciado = false;

        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToModelMappingProfile>();
                x.AddProfile<ModelToDomainMappingProfile>();
            });

            Iniciado = true;
        }
    }
}
using System.Collections.Generic;

namespace Antiguera.Dominio.Interfaces.Servicos.Helpers
{
    public interface IConvertHelper<TSource, TDestination>
        where TSource : class
        where TDestination : new()
    {
        TDestination Copy(TSource source);

        ICollection<TDestination> CopyList(IEnumerable<TSource> listSource);
    }
}

using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using System.Collections.Generic;
using System.Reflection;

namespace Antiguera.Utils.Helpers
{
    public class ConvertHelper<TSource, TDestination> : IConvertHelper<TSource, TDestination>
        where TSource : class
        where TDestination : new()
    {
        public virtual TDestination Copy(TSource source)
        {
            var dest = new TDestination();

            foreach (FieldInfo srcField in typeof(TSource).GetFields())
            {
                foreach (FieldInfo destField in typeof(TDestination).GetFields())
                {
                    if (destField.Name == srcField.Name && destField.FieldType == srcField.FieldType)
                    {
                        destField.SetValue(dest, srcField.GetValue(source));
                    }
                }
            }

            return dest;
        }

        public virtual ICollection<TDestination> CopyList(IEnumerable<TSource> listSource)
        {
            var listDest = new List<TDestination>();

            foreach (FieldInfo srcField in typeof(TSource).GetFields())
            {
                foreach (FieldInfo destField in typeof(TDestination).GetFields())
                {
                    if (destField.Name == srcField.Name && destField.FieldType == srcField.FieldType)
                    {
                        destField.SetValue(listDest, srcField.GetValue(listSource));
                    }
                }
            }

            return listDest;
        }
    }
}

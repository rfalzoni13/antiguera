using Antiguera.Dominio.Interfaces.Servicos.Helpers;
using System;
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

            foreach (PropertyInfo srcProp in typeof(TSource).GetProperties())
            {
                foreach (PropertyInfo destProp in typeof(TDestination).GetProperties())
                {
                    if (destProp.Name == srcProp.Name && destProp.PropertyType == srcProp.PropertyType)
                    {
                        destProp.SetValue(dest, srcProp.GetValue(source));
                    }
                }
            }

            return dest;
        }

        public virtual ICollection<TDestination> CopyList(IEnumerable<TSource> listSource)
        {
            var listDest = new List<TDestination>();

            foreach(var source in listSource)
            {
                var dest = new TDestination();

                foreach (PropertyInfo srcProp in typeof(TSource).GetProperties())
                {
                    foreach (PropertyInfo destProp in typeof(TDestination).GetProperties())
                    {
                        if (destProp.Name == srcProp.Name && destProp.PropertyType == srcProp.PropertyType)
                        {
                            destProp.SetValue(dest, srcProp.GetValue(source));
                        }
                    }
                }

                listDest.Add(dest);
            }

            return listDest;
        }
    }
}

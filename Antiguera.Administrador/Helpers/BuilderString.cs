using System;

namespace Antiguera.Administrador.Helpers
{
    internal class ConstantesNameSpace
    {
        public const string RotasAntiguera = "antiguera.core.configuracoes.rotas";
    }

    public static class BuilderString
    {
        public static string ObjectConstructor(this String str)
        => string.Format("{0} = {0} || {{}};", str);


        public static string ValueConstructor(this String str, string value)
        => string.Format("{0}.{1}", str, value);

        public static string RouteConstructor(this String str, string action, string route)
        => string.Format("{0}.{1} = '{2}'; ", str, action, route);
        

        public static string ToLowerFirstWord(this String str)
        => char.ToLower(str[0]) + str.Substring(1);
        

        public static string ToUowerFirstWord(this String str)
        => char.ToUpper(str[0]) + str.Substring(1);
        
        
    }
}
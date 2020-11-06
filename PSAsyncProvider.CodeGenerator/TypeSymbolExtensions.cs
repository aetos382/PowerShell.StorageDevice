using System.Linq;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal static class TypeSymbolExtensions
    {
        public static ISymbol? GetOverrideSymbol(
            this ITypeSymbol type,
            IMethodSymbol baseSymbol,
            SymbolEqualityComparer comparer)
        {
            var members = type.GetMembers(baseSymbol.Name);

            foreach (var member in members)
            {
                if (!member.IsOverride)
                {
                    continue;
                }

                if (member is IMethodSymbol method)
                {
                    if (comparer.Equals(method.OverriddenMethod, baseSymbol))
                    {
                        return method;
                    }
                }
            }

            return null;
        }

        public static IMethodSymbol? GetMethodSymbol(
            this ITypeSymbol type,
            string name,
            ITypeSymbol[] parameterTypes,
            ITypeSymbol returnType,
            SymbolEqualityComparer comparer)
        {
            var members = type.GetMembers(name);

            foreach (var member in members)
            {
                if (member is not IMethodSymbol method)
                {
                    continue;
                }

                if (!comparer.Equals(method.ReturnType, returnType))
                {
                    continue;
                }

                if (parameterTypes is null)
                {
                    if (method.Parameters.Length == 0)
                    {
                        return method;
                    }
                }
                else
                {
                    bool methodEquals = method.Parameters
                        .Select(p => p.Type)
                        .SequenceEqual(parameterTypes, comparer);

                    if (methodEquals)
                    {
                        return method;
                    }
                }
            }

            return null;
        }
    }
}

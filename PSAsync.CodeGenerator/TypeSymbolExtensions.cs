using System.Collections.Generic;
using System.Linq;

using Microsoft;
using Microsoft.CodeAnalysis;

namespace PSAsync.CodeGenerator
{
    internal static class TypeSymbolExtensions
    {
        public static bool IsType(
            this ITypeSymbol type,
            ITypeSymbol testType,
            bool allowSubType,
            IEqualityComparer<ISymbol?> comparer)
        {
            Requires.NotNull(type, nameof(type));
            Requires.NotNull(testType, nameof(testType));
            Requires.NotNull(comparer, nameof(comparer));

            if (comparer.Equals(type, testType))
            {
                return true;
            }

            if (!allowSubType)
            {
                return false;
            }

            var baseType = type.BaseType;
            while (baseType is not null)
            {
                if (comparer.Equals(baseType, testType))
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }

        public static bool HasInterface(
            this ITypeSymbol type,
            ITypeSymbol interfaceType,
            bool thisTypeOnly,
            IEqualityComparer<ISymbol?> comparer)
        {
            Requires.NotNull(type, nameof(type));
            Requires.NotNull(interfaceType, nameof(interfaceType));
            Requires.NotNull(comparer, nameof(comparer));

            var interfaces = thisTypeOnly ?
                type.Interfaces :
                type.AllInterfaces;

            return interfaces.Any(x => comparer.Equals(x, interfaceType));
        }

        public static IMethodSymbol? GetOverrideSymbol(
            this ITypeSymbol type,
            IMethodSymbol baseSymbol,
            IEqualityComparer<ISymbol?> comparer)
        {
            Requires.NotNull(type, nameof(type));
            Requires.NotNull(baseSymbol, nameof(baseSymbol));
            Requires.NotNull(comparer, nameof(comparer));

            var methods = type.GetMembers(baseSymbol.Name).OfType<IMethodSymbol>();

            foreach (var method in methods)
            {
                if (!method.IsOverride)
                {
                    continue;
                }

                if (comparer.Equals(method.OverriddenMethod, baseSymbol))
                {
                    return method;
                }
            }

            return null;
        }

        public static IMethodSymbol? GetMethodSymbol(
            this ITypeSymbol type,
            string name,
            IEnumerable<ITypeSymbol>? parameterTypes,
            ITypeSymbol returnType,
            IEqualityComparer<ISymbol?> comparer)
        {
            Requires.NotNull(type, nameof(type));
            Requires.NotNull(name, nameof(name));
            Requires.NotNull(returnType, nameof(returnType));
            Requires.NotNull(comparer, nameof(comparer));

            var methods = type.GetMembers(name).OfType<IMethodSymbol>();

            foreach (var method in methods)
            {
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

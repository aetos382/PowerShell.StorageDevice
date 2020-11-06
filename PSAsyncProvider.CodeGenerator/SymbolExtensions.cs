using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal static class SymbolExtensions
    {
        public static bool HasAttribute(
            this ISymbol symbol,
            ITypeSymbol attribute,
            bool allowSubType,
            bool noInherit,
            IEqualityComparer<ISymbol?>? comparer)
        {
            foreach (var attributeData in symbol.GetAttributes())
            {
                var attributeClass = attributeData.AttributeClass;

                if (attributeClass is null)
                {
                    continue;
                }

                if (attributeClass.IsType(attribute, allowSubType, comparer))
                {
                    return true;
                }
            }

            if (noInherit)
            {
                return false;
            }

            var attrUsage = attribute.GetAttributes()
                .SingleOrDefault(
                    x => x.AttributeClass?.ToDisplayString() == typeof(AttributeUsageAttribute).FullName);

            if (attrUsage is null)
            {
                return false;
            }

            bool inherit = true;

            var hasInherited = attrUsage.NamedArguments
                .TryGetValue(nameof(AttributeUsageAttribute.Inherited), null, out var value);

            if (hasInherited)
            {
                inherit = (bool)value.Value;
            }

            if (!inherit)
            {
                return false;
            }

            var baseSymbol = symbol.GetBaseSymbol();
            if (baseSymbol is null)
            {
                return false;
            }

            return baseSymbol.HasAttribute(attribute, allowSubType, noInherit, comparer);
        }

        public static ISymbol? GetBaseSymbol(
            this ISymbol symbol)
        {
            if (symbol is ITypeSymbol typeSymbol)
            {
                return typeSymbol.BaseType;
            }

            if (symbol is IMethodSymbol methodSymbol)
            {
                return methodSymbol.OverriddenMethod;
            }
            
            if (symbol is IPropertySymbol propertySymbol)
            {
                return propertySymbol.OverriddenProperty;
            }

            return null;
        }
    }
}

using System.Collections.Generic;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncCmdletProviderMethodGenerationHelper
    {
        public AsyncCmdletProviderMethodGenerationHelper(
            Compilation compilation,
            string providerTypeName,
            string interfaceName,
            IEqualityComparer<ISymbol?>? symbolComparer = null)
        {
            var attributeSymbol = compilation.GetTypeByMetadataName("PSAsyncProvider.GenerateMemberAttribute");
            var providerSymbol = compilation.GetTypeByMetadataName(providerTypeName);
            var interfaceSymbol = compilation.GetTypeByMetadataName(interfaceName);

            this._attributeSymbol = attributeSymbol;
            this.ProviderSymbol = providerSymbol;
            this.InterfaceSymbol = interfaceSymbol;
            this._symbolComparer = symbolComparer ?? SymbolEqualityComparer.Default;
        }

        public bool IsTargetType(
            ITypeSymbol symbol)
        {
            var comparer = this._symbolComparer;

            if (!symbol.HasAttribute(this._attributeSymbol, true, false, comparer))
            {
                return false;
            }

            if (!symbol.IsType(this.ProviderSymbol, true, comparer))
            {
                return false;
            }

            if (!symbol.HasInterface(this.InterfaceSymbol, false, comparer))
            {
                return false;
            }

            return true;
        }

        private readonly ITypeSymbol _attributeSymbol;

        public ITypeSymbol ProviderSymbol { get; }

        public ITypeSymbol InterfaceSymbol { get; }

        private readonly IEqualityComparer<ISymbol?> _symbolComparer;
    }
}

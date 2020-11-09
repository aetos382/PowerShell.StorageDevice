using System.Collections.Generic;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncCmdletProviderMethodGenerationHelper
    {
        public AsyncCmdletProviderMethodGenerationHelper(
            CodeGenerationContext context,
            string providerTypeName,
            string interfaceName)
        {
            var compilation = context.Compilation;

            var attributeSymbol = compilation.GetTypeByMetadataName("PSAsyncProvider.GenerateMemberAttribute");
            var providerSymbol = compilation.GetTypeByMetadataName(providerTypeName);
            var interfaceSymbol = compilation.GetTypeByMetadataName(interfaceName);

            this.Context = context;
            this._attributeSymbol = attributeSymbol;
            this.ProviderSymbol = providerSymbol;
            this.InterfaceSymbol = interfaceSymbol;
        }

        public bool IsTargetType(
            ITypeSymbol symbol)
        {
            var comparer = this.Context.SymbolComparer;

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

        public MethodDelegation CreateMethodDelegation(
            string methodName,
            IEnumerable<ITypeSymbol>? parameterTypes,
            ITypeSymbol returnType)
        {
            return new MethodDelegation(
                this,
                methodName,
                parameterTypes,
                returnType);
        }

        public CodeGenerationContext Context { get; }

        private readonly ITypeSymbol _attributeSymbol;

        public ITypeSymbol ProviderSymbol { get; }

        public ITypeSymbol InterfaceSymbol { get; }
    }
}

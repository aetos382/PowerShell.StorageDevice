using System;
using System.Collections.Generic;

using Microsoft;
using Microsoft.CodeAnalysis;

namespace PSAsync.CodeGenerator
{
    internal class AsyncMethodGenerationHelper
    {
        public AsyncMethodGenerationHelper(
            CodeGenerationContext context,
            string providerTypeName,
            string interfaceName)
        {
            Requires.NotNull(context, nameof(context));
            Requires.NotNull(providerTypeName, nameof(providerTypeName));
            Requires.NotNull(interfaceName, nameof(interfaceName));

            var compilation = context.Compilation;

            var attributeSymbol = compilation.GetTypeByMetadataName(GenerateMemberAttribute.FullName);
            var providerSymbol = compilation.GetTypeByMetadataName(providerTypeName);
            var interfaceSymbol = compilation.GetTypeByMetadataName(interfaceName);

            if (attributeSymbol is null ||
                providerSymbol is null ||
                interfaceSymbol is null)
            {
                throw new InvalidOperationException();
            }

            this.Context = context;
            this._attributeSymbol = attributeSymbol;
            this.ProviderSymbol = providerSymbol;
            this.InterfaceSymbol = interfaceSymbol;
        }

        public bool IsTargetType(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            var comparer = this.Context.SymbolComparer;

            if (!concreteProviderType.HasAttribute(this._attributeSymbol, true, false, comparer))
            {
                return false;
            }

            if (!concreteProviderType.IsType(this.ProviderSymbol, true, comparer))
            {
                return false;
            }

            if (!concreteProviderType.HasInterface(this.InterfaceSymbol, false, comparer))
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
            Requires.NotNull(methodName, nameof(methodName));
            Requires.NotNull(returnType, nameof(returnType));

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

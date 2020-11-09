using System.Collections.Generic;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class MethodDelegation
    {
        public MethodDelegation(
            AsyncCmdletProviderMethodGenerationHelper helper,
            string methodName,
            IEnumerable<ITypeSymbol>? parameterTypes,
            ITypeSymbol returnType)
        {
            var comparer = helper.Context.SymbolComparer;

            var typeSymbols = helper.Context.TypeSymbols;

            var overriddenMethod = helper.ProviderSymbol.GetMethodSymbol(
                methodName,
                parameterTypes,
                returnType,
                comparer);

            var interfaceMethod = helper.InterfaceSymbol.GetMethodSymbol(
                $"{methodName}Async",
                parameterTypes,
                typeSymbols.ValueTask.Construct(returnType),
                comparer);

            this.MethodName = methodName;
            this.BaseProviderMethod = overriddenMethod;
            this.AsyncInterfaceMethod = interfaceMethod;

            this._helper = helper;
            this._symbolComparer = comparer;
        }

        private readonly AsyncCmdletProviderMethodGenerationHelper _helper;

        public ITypeSymbol BaseProviderType { get; }

        public ITypeSymbol InterfaceType { get; }

        public IMethodSymbol BaseProviderMethod { get; }

        public IMethodSymbol AsyncInterfaceMethod { get; }

        public string MethodName { get; }

        private readonly IEqualityComparer<ISymbol?> _symbolComparer;

        public ISymbol GetConcreteSyncMethod(
            ITypeSymbol concreteProviderType)
        {
            var baseMethod = concreteProviderType.GetOverrideSymbol(
                this.BaseProviderMethod,
                this._symbolComparer);

            return baseMethod;
        }

        public ISymbol GetInterfaceAsyncMethod(
            ITypeSymbol concreteProviderType)
        {
            var interfaceMethod = concreteProviderType.FindImplementationForInterfaceMember(
                this.AsyncInterfaceMethod);

            return interfaceMethod;
        }


        public bool IsSyncMethodImplemented(
            ITypeSymbol concreteProviderType)
        {
            var syncMethod = this.GetConcreteSyncMethod(concreteProviderType);
            return syncMethod is not null;
        }

        public bool IsAsyncMethodImplemented(
            ITypeSymbol concreteProviderType)
        {
            var interfaceMethod = this.GetInterfaceAsyncMethod(concreteProviderType);

            return
                interfaceMethod is not null &&
                !this._symbolComparer.Equals(interfaceMethod, this.AsyncInterfaceMethod);
        }

        public bool ShouldGenerateMethod(
            ITypeSymbol concreteProviderType)
        {
            if (this.IsSyncMethodImplemented(concreteProviderType))
            {
                return false;
            }

            if (!this.IsAsyncMethodImplemented(concreteProviderType))
            {
                return false;
            }

            return true;
        }
    }
}

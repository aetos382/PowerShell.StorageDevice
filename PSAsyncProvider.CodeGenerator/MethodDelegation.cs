using System;
using System.Collections.Generic;

using Microsoft;
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
            Requires.NotNull(helper, nameof(helper));
            Requires.NotNull(methodName, nameof(methodName));
            Requires.NotNull(returnType, nameof(returnType));

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

            if (overriddenMethod is null ||
                interfaceMethod is null)
            {
                throw new InvalidOperationException();
            }

            this.MethodName = methodName;
            this.BaseProviderMethod = overriddenMethod;
            this.AsyncInterfaceMethod = interfaceMethod;

            this._symbolComparer = comparer;
        }

        public IMethodSymbol BaseProviderMethod { get; }

        public IMethodSymbol AsyncInterfaceMethod { get; }

        public string MethodName { get; }

        private readonly IEqualityComparer<ISymbol?> _symbolComparer;

        public ISymbol? GetConcreteSyncMethod(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            var baseMethod = concreteProviderType.GetOverrideSymbol(
                this.BaseProviderMethod,
                this._symbolComparer);

            return baseMethod;
        }

        public ISymbol? GetInterfaceAsyncMethod(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            var interfaceMethod = concreteProviderType.FindImplementationForInterfaceMember(
                this.AsyncInterfaceMethod);

            return interfaceMethod;
        }


        public bool IsSyncMethodImplemented(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            var syncMethod = this.GetConcreteSyncMethod(concreteProviderType);
            return syncMethod is not null;
        }

        public bool IsAsyncMethodImplemented(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            var interfaceMethod = this.GetInterfaceAsyncMethod(concreteProviderType);

            return
                interfaceMethod is not null &&
                !this._symbolComparer.Equals(interfaceMethod, this.AsyncInterfaceMethod);
        }

        public bool ShouldGenerateMethod(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

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

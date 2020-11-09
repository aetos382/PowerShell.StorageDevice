using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft;
using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncCmdletProviderMethodGenerator :
        IAsyncProviderMethodGenerator
    {
        public AsyncCmdletProviderMethodGenerator(
            CodeGenerationContext context)
        {
            Requires.NotNull(context, nameof(context));

            var helper = new AsyncCmdletProviderMethodGenerationHelper(
                context,
                "System.Management.Automation.Provider.CmdletProvider",
                "PSAsyncProvider.IAsyncCmdletProvider");

            var typeSymbols = context.TypeSymbols;

            var providerInfoSymbol = context.Compilation.GetTypeByMetadataName(
                "System.Management.Automation.ProviderInfo");

            if (providerInfoSymbol is null)
            {
                throw new InvalidOperationException();
            }
            
            this._helper = helper;

            this._start = helper.CreateMethodDelegation(
                "Start",
                new[] { providerInfoSymbol },
                providerInfoSymbol);
            
            this._startDynamicParameters = helper.CreateMethodDelegation(
                "StartDynamicParameters",
                null,
                typeSymbols.Object);
        }

        public bool IsTargetType(
            ITypeSymbol concreteProviderType)
        {
            return this._helper.IsTargetType(concreteProviderType);
        }

        public IEnumerable<string> GenerateCode(
            ITypeSymbol concreteProviderType,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            return this.GenerateCodeCore(concreteProviderType, cancellationToken);
        }

        private IEnumerable<string> GenerateCodeCore(
            ITypeSymbol concreteProviderType,
            CancellationToken cancellationToken)
        {
            string? code;

            cancellationToken.ThrowIfCancellationRequested();

            code = this._start.GenerateMethod(concreteProviderType);
            if (!string.IsNullOrWhiteSpace(code))
            {
                yield return code!;

                cancellationToken.ThrowIfCancellationRequested();

                code = this._startDynamicParameters.GenerateMethod(concreteProviderType);
                if (!string.IsNullOrWhiteSpace(code))
                {
                    yield return code!;
                }
            }

            yield break;
        }

        private readonly AsyncCmdletProviderMethodGenerationHelper _helper;

        private readonly MethodDelegation _start;

        private readonly MethodDelegation _startDynamicParameters;
    }
}

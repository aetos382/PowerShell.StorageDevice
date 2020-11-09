using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft;
using Microsoft.CodeAnalysis;

namespace PSAsync.CodeGenerator.Provider
{
    internal class AsyncCmdletProviderMethodGenerator :
        IAsyncMethodGenerator
    {
        public AsyncCmdletProviderMethodGenerator(
            CodeGenerationContext context)
        {
            Requires.NotNull(context, nameof(context));

            var helper = new AsyncMethodGenerationHelper(
                context,
                "System.Management.Automation.Provider.CmdletProvider",
                "PSAsync.Provider.IAsyncCmdletProvider");

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

        private readonly AsyncMethodGenerationHelper _helper;

        private readonly MethodDelegation _start;

        private readonly MethodDelegation _startDynamicParameters;
    }
}

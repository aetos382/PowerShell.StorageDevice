using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncCmdletProviderMethodGenerator :
        IAsyncProviderMethodGenerator
    {
        public AsyncCmdletProviderMethodGenerator(
            GeneratorExecutionContext context,
            Compilation compilation)
        {
            this._helper = new AsyncCmdletProviderMethodGenerationHelper(
                compilation,
                "System.Management.Automation.Provider.CmdletProvider",
                "PSAsyncProvider.IAsyncCmdletProvider",
                _comparer);

            var providerSymbol = this._helper.ProviderSymbol;
            var interfaceSymbol = this._helper.InterfaceSymbol;

            var providerInfoSymbol = compilation.GetTypeByMetadataName(
                "System.Management.Automation.ProviderInfo");

            var valueTaskSymbol = compilation.GetTypeByMetadataName(
                "System.Threading.Tasks.ValueTask`1");

            if (providerInfoSymbol is null)
            {
                throw new InvalidOperationException();
            }
            
            this._interfaceType = interfaceSymbol;

            this._start = providerSymbol.GetMethodSymbol(
                "Start",
                new[] { providerInfoSymbol },
                providerInfoSymbol,
                null);

            this._startAsync = interfaceSymbol.GetMethodSymbol(
                "StartAsync",
                new[] { providerInfoSymbol },
                valueTaskSymbol.Construct(providerInfoSymbol),
                null);
        }

        public bool IsTargetType(
            ITypeSymbol symbol)
        {
            return this._helper.IsTargetType(symbol);
        }

        public IEnumerable<string> GenerateCode(
            ITypeSymbol symbol,
            CancellationToken cancellationToken)
        {
            string code;

            code = this.GenerateStart(symbol, cancellationToken);
            if (!string.IsNullOrWhiteSpace(code))
            {
                yield return code;
            }

            yield break;
        }

        private string? GenerateStart(
            ITypeSymbol symbol,
            CancellationToken cancellationToken)
        {
            var start = symbol.GetOverrideSymbol(
                this._start,
                _comparer);

            if (start is not null)
            {
                return null;
            }

            var startAsync = symbol.FindImplementationForInterfaceMember(
                this._startAsync);

            if (startAsync is null ||
                _comparer.Equals(startAsync.ContainingType, this._interfaceType))
            {
                return null;
            }

            return @"
protected override System.Management.Automation.ProviderInfo Start(System.Management.Automation.ProviderInfo providerInfo)
{
    return this.StartAsync(providerInfo).Result;
}
";
        }

        private static readonly SymbolEqualityComparer _comparer = SymbolEqualityComparer.Default;

        private readonly AsyncCmdletProviderMethodGenerationHelper _helper;

        private readonly ITypeSymbol _interfaceType;

        private readonly IMethodSymbol _start;

        private readonly IMethodSymbol _startAsync;
    }
}

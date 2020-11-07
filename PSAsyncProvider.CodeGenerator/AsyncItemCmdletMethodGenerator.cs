using System.Collections.Generic;
using System.Threading;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncItemCmdletMethodGenerator :
        IAsyncProviderMethodGenerator
    {
        public AsyncItemCmdletMethodGenerator(
            GeneratorExecutionContext context,
            Compilation compilation)
        {
            this._helper = new AsyncCmdletProviderMethodGenerationHelper(
                compilation,
                "System.Management.Automation.Provider.ItemCmdletProvider",
                "PSAsyncProvider.IAsyncItemCmdletProvider",
                this._symbolComparer);

            var typeSymbols = new TypeSymbols(compilation);

            this._isValidPath = this._helper.ProviderSymbol.GetMethodSymbol(
                "IsValidPath",
                new[] { typeSymbols.String },
                typeSymbols.Boolean,
                this._symbolComparer);
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

            code = this.GenerateIsValidPath(symbol, cancellationToken);
            if (!string.IsNullOrWhiteSpace(code))
            {
                yield return code;
            }
        }

        private string GenerateIsValidPath(
            ITypeSymbol symbol,
            CancellationToken cancellation)
        {
            var isValidPath = symbol.GetOverrideSymbol(
                this._isValidPath,
                this._symbolComparer);

            if (isValidPath is not null)
            {
                return null;
            }

            return $@"protected override bool IsValidPath(string path)
{{
    return this.IsValidPathAsync(path).Result;
}}
";
        }

        private readonly AsyncCmdletProviderMethodGenerationHelper _helper;

        private readonly SymbolEqualityComparer _symbolComparer = SymbolEqualityComparer.Default;

        private readonly IMethodSymbol _isValidPath;
    }
}

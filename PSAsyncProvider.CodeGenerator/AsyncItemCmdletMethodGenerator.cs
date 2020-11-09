using System.Collections.Generic;
using System.Threading;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncItemCmdletMethodGenerator :
        IAsyncProviderMethodGenerator
    {
        public AsyncItemCmdletMethodGenerator(
            CodeGenerationContext context)
        {
            var helper = new AsyncCmdletProviderMethodGenerationHelper(
                context,
                "System.Management.Automation.Provider.ItemCmdletProvider",
                "PSAsyncProvider.IAsyncItemCmdletProvider");

            var providerSymbol = helper.ProviderSymbol;

            var typeSymbols = context.TypeSymbols;

            this._itemExists = helper.CreateMethodDelegation(
                "ItemExists",
                new[] { typeSymbols.String },
                typeSymbols.Boolean);

            this._isValidPath = providerSymbol.GetMethodSymbol(
                "IsValidPath",
                new[] { typeSymbols.String },
                typeSymbols.Boolean,
                context.SymbolComparer);

            this._helper = helper;
            this._symbolComparer = context.SymbolComparer;
        }

        public bool IsTargetType(
            ITypeSymbol symbol)
        {
            return this._helper.IsTargetType(symbol);
        }

        public IEnumerable<string> GenerateCode(
            ITypeSymbol concreteProviderType,
            CancellationToken cancellationToken)
        {
            string code;

            code = this.GenerateIsValidPath(concreteProviderType, cancellationToken);
            if (!string.IsNullOrWhiteSpace(code))
            {
                yield return code;
            }

            code = this.GenerateItemExists(concreteProviderType, cancellationToken);
            if (!string.IsNullOrWhiteSpace(code))
            {
                yield return code;
            }
        }

        private string? GenerateItemExists(
            ITypeSymbol concreteProviderType,
            CancellationToken cancellationToken)
        {
            if (!this._itemExists.ShouldGenerateMethod(concreteProviderType))
            {
                return null;
            }

            return @"
// Generated Method
protected override bool ItemExists(string path)
{
    this.WriteVerbose($""ItemExists(\""{path}\"")"");
    var result = this.ItemExistsAsync(path).Result;
    this.WriteVerbose($""returns: {result}"");

    return result;
}";
        }

        private string GenerateIsValidPath(
            ITypeSymbol concreteProviderType,
            CancellationToken cancellation)
        {
            var isValidPath = concreteProviderType.GetOverrideSymbol(
                this._isValidPath,
                this._symbolComparer);

            if (isValidPath is not null)
            {
                return null;
            }

            return @"
// Generated Method
protected override bool IsValidPath(string path)
{
    this.WriteVerbose($""IsValidPath(\""{path}\"")"");
    var result = this.IsValidPathAsync(path).Result;
    this.WriteVerbose($""returns: {result}"");

    return result;
}
";
        }

        private readonly AsyncCmdletProviderMethodGenerationHelper _helper;

        private readonly IEqualityComparer<ISymbol?> _symbolComparer;

        private readonly IMethodSymbol _isValidPath;

        private readonly MethodDelegation _itemExists;
    }
}

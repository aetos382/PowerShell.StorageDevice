using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft;
using Microsoft.CodeAnalysis;

namespace PSAsync.CodeGenerator.Provider
{
    internal class AsyncItemCmdletMethodGenerator :
        IAsyncMethodGenerator
    {
        public AsyncItemCmdletMethodGenerator(
            CodeGenerationContext context)
        {
            var helper = new AsyncMethodGenerationHelper(
                context,
                "System.Management.Automation.Provider.ItemCmdletProvider",
                "PSAsync.Provider.IAsyncItemCmdletProvider");

            var providerSymbol = helper.ProviderSymbol;

            var typeSymbols = context.TypeSymbols;

            var itemExists = helper.CreateMethodDelegation(
                "ItemExists",
                new[] { typeSymbols.String },
                typeSymbols.Boolean);

            var isValidPath = providerSymbol.GetMethodSymbol(
                "IsValidPath",
                new[] { typeSymbols.String },
                typeSymbols.Boolean,
                context.SymbolComparer);

            if (itemExists is null ||
                isValidPath is null)
            {
                throw new InvalidOperationException();
            }

            this._helper = helper;
            this._symbolComparer = context.SymbolComparer;
            this._itemExists = itemExists;
            this._isValidPath = isValidPath;
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
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            return this.GenerateCodeCore(concreteProviderType, cancellationToken);
        }

        private IEnumerable<string> GenerateCodeCore(
            ITypeSymbol concreteProviderType,
            CancellationToken cancellationToken)
        {
            string? code;

            cancellationToken.ThrowIfCancellationRequested();

            code = this.GenerateIsValidPath(concreteProviderType);
            if (!string.IsNullOrWhiteSpace(code))
            {
                yield return code!;
            }

            cancellationToken.ThrowIfCancellationRequested();

            code = this._itemExists.GenerateMethod(concreteProviderType);
            if (!string.IsNullOrWhiteSpace(code))
            {
                yield return code!;
            }
        }

        private string? GenerateIsValidPath(
            ITypeSymbol concreteProviderType)
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

        private readonly AsyncMethodGenerationHelper _helper;

        private readonly IEqualityComparer<ISymbol?> _symbolComparer;

        private readonly IMethodSymbol _isValidPath;

        private readonly MethodDelegation _itemExists;
    }
}

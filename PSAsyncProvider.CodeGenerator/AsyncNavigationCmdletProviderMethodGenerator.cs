﻿using System.Collections.Generic;
using System.Threading;

using Microsoft;
using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncNavigationCmdletProviderMethodGenerator :
        IAsyncProviderMethodGenerator
    {
        public AsyncNavigationCmdletProviderMethodGenerator(
            CodeGenerationContext context)
        {
            var helper = new AsyncCmdletProviderMethodGenerationHelper(
                context,
                "System.Management.Automation.Provider.NavigationCmdletProvider",
                "PSAsyncProvider.IAsyncNavigationCmdletProvider");

            this._helper = helper;
            
            var typeSymbols = context.TypeSymbols;

            this._isItemContainer = helper.CreateMethodDelegation(
                "IsItemContainer",
                new[] { typeSymbols.String },
                typeSymbols.Boolean);

            this._normalizeRelativePath = helper.CreateMethodDelegation(
                "NormalizeRelativePath",
                new[] { typeSymbols.String, typeSymbols.String },
                typeSymbols.String);
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

            code = this._isItemContainer.GenerateMethod(concreteProviderType);
            if (!string.IsNullOrWhiteSpace(code))
            {
                yield return code!;
            }

            cancellationToken.ThrowIfCancellationRequested();

            code = this._normalizeRelativePath.GenerateMethod(concreteProviderType);
            if (!string.IsNullOrWhiteSpace(code))
            {
                yield return code!;
            }
        }

        private readonly AsyncCmdletProviderMethodGenerationHelper _helper;

        private readonly MethodDelegation _isItemContainer;

        private readonly MethodDelegation _normalizeRelativePath;
    }
}

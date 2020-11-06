using System.Collections.Generic;
using System.Threading;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncNavigationCmdletProviderMethodGenerator :
        IAsyncProviderMethodGenerator
    {
        public AsyncNavigationCmdletProviderMethodGenerator(
            Compilation compilation)
        {
            this._helper = new AsyncCmdletProviderMethodGenerationHelper(
                compilation,
                "System.Management.Automation.Provider.NavigationCmdletProvider",
                "PSAsyncProvider.IAsyncNavigationCmdletProvider");
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
            yield break;
        }

        private readonly AsyncCmdletProviderMethodGenerationHelper _helper;
    }
}

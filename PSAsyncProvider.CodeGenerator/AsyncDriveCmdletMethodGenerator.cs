using System.Collections.Generic;
using System.Threading;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncDriveCmdletMethodGenerator :
        IAsyncProviderMethodGenerator
    {
        public AsyncDriveCmdletMethodGenerator(
            GeneratorExecutionContext context,
            Compilation compilation)
        {
            this._helper = new AsyncCmdletProviderMethodGenerationHelper(
                compilation,
                "System.Management.Automation.Provider.DriveCmdletProvider",
                "PSAsyncProvider.IAsyncDriveCmdletProvider");
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

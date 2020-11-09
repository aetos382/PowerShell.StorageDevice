using System.Collections.Generic;
using System.Threading;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncDriveCmdletMethodGenerator :
        IAsyncProviderMethodGenerator
    {
        public AsyncDriveCmdletMethodGenerator(
            CodeGenerationContext context)
        {
            this._helper = new AsyncCmdletProviderMethodGenerationHelper(
                context,
                "System.Management.Automation.Provider.DriveCmdletProvider",
                "PSAsyncProvider.IAsyncDriveCmdletProvider");
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
            yield break;
        }

        private readonly AsyncCmdletProviderMethodGenerationHelper _helper;
    }
}

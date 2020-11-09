using System.Collections.Generic;
using System.Threading;

using Microsoft;
using Microsoft.CodeAnalysis;

namespace PSAsync.CodeGenerator.Provider
{
    internal class AsyncDriveCmdletMethodGenerator :
        IAsyncMethodGenerator
    {
        public AsyncDriveCmdletMethodGenerator(
            CodeGenerationContext context)
        {
            this._helper = new AsyncMethodGenerationHelper(
                context,
                "System.Management.Automation.Provider.DriveCmdletProvider",
                "PSAsync.Provider.IAsyncDriveCmdletProvider");
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
            yield break;
        }

        private readonly AsyncMethodGenerationHelper _helper;
    }
}

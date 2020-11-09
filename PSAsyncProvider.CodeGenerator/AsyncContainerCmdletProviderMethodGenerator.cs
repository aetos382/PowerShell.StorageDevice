using System.Collections.Generic;
using System.Threading;

using Microsoft;
using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncContainerCmdletProviderMethodGenerator :
        IAsyncProviderMethodGenerator
    {
        public AsyncContainerCmdletProviderMethodGenerator(
            CodeGenerationContext context)
        {
            this._helper = new AsyncCmdletProviderMethodGenerationHelper(
                context,
                "System.Management.Automation.Provider.ContainerCmdletProvider",
                "PSAsyncProvider.IAsyncContainerCmdletProvider");
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

        private readonly AsyncCmdletProviderMethodGenerationHelper _helper;
    }
}

using System.Collections.Generic;
using System.Threading;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal interface IAsyncProviderMethodGenerator
    {
        bool IsTargetType(
            ITypeSymbol symbol);

        IEnumerable<string> GenerateCode(
            ITypeSymbol symbol,
            CancellationToken cancellationToken);
    }
}

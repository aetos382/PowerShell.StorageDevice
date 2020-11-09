using System.Collections.Generic;
using System.Threading;

using Microsoft.CodeAnalysis;

namespace PSAsync.CodeGenerator
{
    internal interface IAsyncMethodGenerator
    {
        bool IsTargetType(
            ITypeSymbol symbol);

        IEnumerable<string> GenerateCode(
            ITypeSymbol symbol,
            CancellationToken cancellationToken);
    }
}

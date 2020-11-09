using System.Collections.Generic;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class CodeGenerationContext
    {
        public CodeGenerationContext(
            Compilation compilation,
            IEqualityComparer<ISymbol?>? symbolComparer = null)
        {
            this.Compilation = compilation;
            this.TypeSymbols = new TypeSymbols(compilation);
            this.SymbolComparer = symbolComparer ?? SymbolEqualityComparer.Default;
        }

        public Compilation Compilation { get; }

        public TypeSymbols TypeSymbols { get; }

        public IEqualityComparer<ISymbol?> SymbolComparer { get; }
    }
}

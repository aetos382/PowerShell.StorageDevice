using System;
using System.Threading.Tasks;

using Microsoft;
using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class TypeSymbols
    {
        public TypeSymbols(
            Compilation compilation)
        {
            Requires.NotNull(compilation, nameof(compilation));

            var objectSymbol = compilation.GetTypeByMetadataName(typeof(object).FullName);
            var stringSymbol = compilation.GetTypeByMetadataName(typeof(string).FullName);
            var booleanSymbol = compilation.GetTypeByMetadataName(typeof(bool).FullName);
            var valueTaskSymbol = compilation.GetTypeByMetadataName(typeof(ValueTask<>).FullName);

            if (objectSymbol is null ||
                stringSymbol is null ||
                booleanSymbol is null ||
                valueTaskSymbol is null)
            {
                throw new InvalidOperationException();
            }

            this.Object = objectSymbol;
            this.String = stringSymbol;
            this.Boolean = booleanSymbol;
            this.ValueTask = valueTaskSymbol;
        }

        public ITypeSymbol Object { get; }

        public ITypeSymbol String { get; }
        
        public ITypeSymbol Boolean { get; }
        
        public INamedTypeSymbol ValueTask { get; }
    }
}

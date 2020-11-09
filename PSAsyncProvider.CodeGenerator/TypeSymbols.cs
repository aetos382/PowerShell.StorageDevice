using System.Threading.Tasks;

using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class TypeSymbols
    {
        public TypeSymbols(
            Compilation compilation)
        {
            this.Object = compilation.GetTypeByMetadataName(typeof(object).FullName);
            this.String = compilation.GetTypeByMetadataName(typeof(string).FullName);
            this.Boolean = compilation.GetTypeByMetadataName(typeof(bool).FullName);
            this.ValueTask = compilation.GetTypeByMetadataName(typeof(ValueTask<>).FullName);
        }

        public ITypeSymbol Object { get; }

        public ITypeSymbol String { get; }
        
        public ITypeSymbol Boolean { get; }
        
        public INamedTypeSymbol ValueTask { get; }
    }
}

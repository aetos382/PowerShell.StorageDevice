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

            var voidSymbol = compilation.GetTypeByMetadataName(typeof(void).FullName);
            var objectSymbol = compilation.GetTypeByMetadataName(typeof(object).FullName);
            var stringSymbol = compilation.GetTypeByMetadataName(typeof(string).FullName);
            var booleanSymbol = compilation.GetTypeByMetadataName(typeof(bool).FullName);
            var taskSymbol = compilation.GetTypeByMetadataName(typeof(Task).FullName);
            var taskWithValueSymbol = compilation.GetTypeByMetadataName(typeof(Task<>).FullName);

            if (voidSymbol is null ||
                objectSymbol is null ||
                stringSymbol is null ||
                booleanSymbol is null ||
                taskSymbol is null ||
                taskWithValueSymbol is null)
            {
                throw new InvalidOperationException();
            }

            this.Void = voidSymbol;
            this.Object = objectSymbol;
            this.String = stringSymbol;
            this.Boolean = booleanSymbol;
            this.Task = taskSymbol;
            this.TaskWithValue = taskWithValueSymbol;
        }

        public ITypeSymbol Void { get; }

        public ITypeSymbol Object { get; }

        public ITypeSymbol String { get; }
        
        public ITypeSymbol Boolean { get; }

        public ITypeSymbol Task { get; }

        public INamedTypeSymbol TaskWithValue { get; }
    }
}

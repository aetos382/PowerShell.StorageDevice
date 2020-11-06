using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PSAsyncProvider.CodeGenerator
{
    [Generator]
    public class AsyncItemCmdletProviderGenerator :
        AsyncDriveCmdletProviderCodeGenerator
    {
        protected override bool PrepareExecution(
            GeneratorExecutionContext context,
            Compilation compilation)
        {
            if (!base.PrepareExecution(context, compilation))
            {
                return false;
            }

            var comparer = SymbolEqualityComparer.Default;

            var itemProviderSymbol = compilation.GetTypeByMetadataName("System.Management.Automation.Provider.ItemCmdletProvider");
            var interfaceSymbol = compilation.GetTypeByMetadataName("PSAsyncProvider.IAsyncItemCmdletProvider");
            var stringTypeSymbol = compilation.GetTypeByMetadataName("System.String");
            var booleanTypeSymbol = compilation.GetTypeByMetadataName("System.Boolean");

            if (itemProviderSymbol is null ||
                interfaceSymbol is null ||
                stringTypeSymbol is null ||
                booleanTypeSymbol is null)
            {
                return false;
            }

            var isValidPath = itemProviderSymbol.GetMethodSymbol(
                "IsValidPath",
                new[] { stringTypeSymbol },
                booleanTypeSymbol,
                comparer);

            if (isValidPath is null)
            {
                return false;
            }

            return true;
        }

        protected override bool IsTargetType(
            ClassDeclarationSyntax syntax,
            ITypeSymbol symbol)
        {
            if (!base.IsTargetType(syntax, symbol))
            {
                return false;
            }

            return true;
        }

        private static bool IsNavigationCmdletProvider(
            ITypeSymbol typeSymbol)
        {
            var baseType = typeSymbol.BaseType;
            while (baseType is not null)
            {
                if (comparer.Equals(baseType, navigationProviderSymbol))
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }
    }
}

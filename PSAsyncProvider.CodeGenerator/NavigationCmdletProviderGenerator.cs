using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PSAsyncProvider.CodeGenerator
{
    [Generator]
    public class NavigationCmdletProviderGenerator :
        ISourceGenerator
    {
        private const string attributeText = @"
namespace PSAsyncProvider
{
    using System;
    using System.Diagnostics;

    [Conditional(""COMPILE_TIME_ONLY"")]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class GenerateMemberAttribute :
        Attribute
    {
    }
}
";

        public void Execute(
            GeneratorExecutionContext context)
        {
            var comparer = SymbolEqualityComparer.Default;

            var attributeSource = SourceText.From(attributeText, Encoding.UTF8);

            context.AddSource("GenerateMemberAttibute.cs", attributeSource);

            if (context.SyntaxReceiver is not SyntaxReceiver receiver)
            {
                return;
            }

            var compilation = context.Compilation.AddSyntaxTrees(
                CSharpSyntaxTree.ParseText(
                    attributeSource,
                    (CSharpParseOptions)context.ParseOptions,
                    cancellationToken: context.CancellationToken));

            var attributeSymbol = compilation.GetTypeByMetadataName("PSAsyncProvider.GenerateMemberAttribute");
            var navigationProviderSymbol = compilation.GetTypeByMetadataName("System.Management.Automation.Provider.NavigationCmdletProvider");
            var itemProviderSymbol = compilation.GetTypeByMetadataName("System.Management.Automation.Provider.ItemCmdletProvider");
            var interfaceSymbol = compilation.GetTypeByMetadataName("PSAsyncProvider.IAsyncNavigationCmdletProvider");
            var stringTypeSymbol = compilation.GetTypeByMetadataName("System.String");
            var booleanTypeSymbol = compilation.GetTypeByMetadataName("System.Boolean");

            if (attributeSymbol is null ||
                navigationProviderSymbol is null ||
                itemProviderSymbol is null ||
                interfaceSymbol is null ||
                stringTypeSymbol is null ||
                booleanTypeSymbol is null)
            {
                return;
            }

            var isValidPath = itemProviderSymbol.GetMethodSymbol(
                "IsValidPath",
                new[] { stringTypeSymbol },
                booleanTypeSymbol,
                comparer);

            if (isValidPath is null)
            {
                return;
            }

            foreach (var syntax in receiver.CandidateSyntaxes)
            {
                var model = compilation.GetSemanticModel(syntax.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(syntax, context.CancellationToken);

                if (symbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    continue;
                }

                if (!HasGenerateMemberAttribute(symbol))
                {
                    continue;
                }

                if (!IsNavigationCmdletProvider(symbol))
                {
                    continue;
                }

                if (!IsInterfaceImplemented(symbol))
                {
                    continue;
                }

                var buffer = new StringBuilder();

                GenerateIsValidPath(symbol, buffer);

                string fileNameBase;

                if (string.IsNullOrEmpty(syntax.SyntaxTree.FilePath))
                {
                    fileNameBase = symbol.Name;
                }
                else
                {
                    fileNameBase = Path.GetFileNameWithoutExtension(syntax.SyntaxTree.FilePath);
                }

                context.AddSource(
                    $"{fileNameBase}_Generated.cs",
                    SourceText.From(buffer.ToString(), Encoding.UTF8));
            }

            void GenerateIsValidPath(
                ITypeSymbol symbol,
                StringBuilder buffer)
            {
                var isValidPathOverride = symbol.GetOverrideSymbol(isValidPath, comparer);
                if (isValidPathOverride is not null)
                {
                    return;
                }

                const string code = @"
protected override bool IsValidPath(string path)
{
    return this.IsValidPathAsync(path).Result;
}";

                var ns = symbol.ContainingNamespace;
                bool hasNamespace = ns != null && !ns.IsGlobalNamespace;

                if (hasNamespace)
                {
                    string namespaceString = symbol.ContainingNamespace.ToDisplayString();
                    buffer.AppendLine($"namespace {namespaceString}");
                    buffer.AppendLine("{");
                }

                buffer.Append($"partial class {symbol.Name}");
                buffer.AppendLine("{");
                buffer.AppendLine(code);
                buffer.AppendLine("}");

                if (hasNamespace)
                {
                    buffer.AppendLine("}");
                }
            }

            bool HasGenerateMemberAttribute(
                ITypeSymbol typeSymbol)
            {
                foreach (var attribute in typeSymbol.GetAttributes())
                {
                    context.CancellationToken.ThrowIfCancellationRequested();

                    if (comparer.Equals(attribute.AttributeClass, attributeSymbol))
                    {
                        return true;
                    }
                }

                return false;
            }

            bool IsNavigationCmdletProvider(
                ITypeSymbol typeSymbol)
            {
                var baseType = typeSymbol.BaseType;
                while (baseType is not null)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();

                    if (comparer.Equals(baseType, navigationProviderSymbol))
                    {
                        return true;
                    }

                    baseType = baseType.BaseType;
                }

                return false;
            }

            bool IsInterfaceImplemented(
                ITypeSymbol typeSymbol)
            {
                foreach (var iface in typeSymbol.AllInterfaces)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();

                    if (comparer.Equals(iface, interfaceSymbol))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void Initialize(
            GeneratorInitializationContext context)
        {
            // System.Diagnostics.Debugger.Launch();

            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        private class SyntaxReceiver :
            ISyntaxReceiver
        {
            private readonly List<ClassDeclarationSyntax> _candidateSyntaxes =
                new List<ClassDeclarationSyntax>();

            public IReadOnlyCollection<ClassDeclarationSyntax> CandidateSyntaxes
            {
                get
                {
                    return this._candidateSyntaxes;
                }
            }

            public void OnVisitSyntaxNode(
                SyntaxNode syntaxNode)
            {
                if (syntaxNode is not ClassDeclarationSyntax classSyntax)
                {
                    return;
                }

                if (!classSyntax.Modifiers.Any(SyntaxKind.PartialKeyword))
                {
                    return;
                }

                if (!classSyntax.AttributeLists.Any())
                {
                    return;
                }

                if (classSyntax.BaseList is null)
                {
                    return;
                }

                this._candidateSyntaxes.Add(classSyntax);
            }
        }
    }
}

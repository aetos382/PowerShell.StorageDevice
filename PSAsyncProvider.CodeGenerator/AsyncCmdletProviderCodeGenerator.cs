using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PSAsyncProvider.CodeGenerator
{
    [Generator]
    public class AsyncCmdletProviderCodeGenerator :
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

        public virtual void Execute(
            GeneratorExecutionContext context)
        {
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
            var providerSymbol = compilation.GetTypeByMetadataName("System.Management.Automation.Provider.CmdletProvider");
            var interfaceSymbol = compilation.GetTypeByMetadataName("PSAsyncProvider.IAsyncCmdletProvider");
            var stringTypeSymbol = compilation.GetTypeByMetadataName("System.String");
            var booleanTypeSymbol = compilation.GetTypeByMetadataName("System.Boolean");

            if (attributeSymbol is null ||
                providerSymbol is null ||
                interfaceSymbol is null ||
                stringTypeSymbol is null ||
                booleanTypeSymbol is null)
            {
                return;
            }

            if (!this.PrepareExecution(context, compilation))
            {
                return;
            }

            var comparer = SymbolEqualityComparer.Default;

            foreach (var syntax in receiver.CandidateSyntaxes)
            {
                var model = compilation.GetSemanticModel(syntax.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(syntax, context.CancellationToken);

                if (symbol is not INamedTypeSymbol)
                {
                    continue;
                }

                // IsTargetType に移す
                if (!HasAttribute(symbol, attributeSymbol, comparer))
                {
                    continue;
                }

                if (!this.IsTargetType(syntax, symbol))
                {
                    continue;
                }
            }

            this.ExecuteCore(context, compilation);
        }

        protected static bool HasAttribute(
            ITypeSymbol type,
            ITypeSymbol attribute,
            IEqualityComparer<ISymbol> comparer)
        {
            foreach (var attributeData in type.GetAttributes())
            {
                if (comparer.Equals(attributeData.AttributeClass, attribute))
                {
                    return true;
                }
            }

            return false;
        }

        protected static bool IsType(
            ITypeSymbol type,
            ITypeSymbol testType,
            IEqualityComparer<ISymbol> comparer)
        {
            if (comparer.Equals(type, testType))
            {
                return true;
            }

            var baseType = type.BaseType;
            while (baseType is not null)
            {
                if (comparer.Equals(baseType, testType))
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }

        protected virtual bool PrepareExecution(
            GeneratorExecutionContext context,
            Compilation compilation)
        {
            return true;
        }

        protected virtual bool ExecuteCore(
            GeneratorExecutionContext context,
            Compilation compilation)
        {
            return true;
        }

        protected virtual bool IsTargetType(
            ClassDeclarationSyntax syntax,
            ITypeSymbol symbol)
        {
            return true;
        }

        public virtual void Initialize(
            GeneratorInitializationContext context)
        {
            // System.Diagnostics.Debugger.Launch();

            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        protected class SyntaxReceiver :
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

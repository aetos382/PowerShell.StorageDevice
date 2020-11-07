using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PSAsyncProvider.CodeGenerator
{
    [Generator]
    public class AsyncProviderMethodGenerator :
        ISourceGenerator
    {
        private const string attributeText = @"
namespace PSAsyncProvider
{
    using System;
    using System.Diagnostics;

    [Conditional(""COMPILE_TIME_ONLY"")]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal sealed class GenerateMemberAttribute :
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

            var compilation = context.Compilation;

            compilation = compilation.AddSyntaxTrees(
                CSharpSyntaxTree.ParseText(
                    attributeSource,
                    (CSharpParseOptions)context.ParseOptions,
                    cancellationToken: context.CancellationToken));

            var generators = new IAsyncProviderMethodGenerator[]
            {
                new AsyncCmdletProviderMethodGenerator(context, compilation),
                new AsyncDriveCmdletMethodGenerator(context, compilation),
                new AsyncItemCmdletMethodGenerator(context, compilation),
                new AsyncContainerCmdletProviderMethodGenerator(context, compilation),
                new AsyncNavigationCmdletProviderMethodGenerator(context, compilation)
            };

            foreach (var syntax in receiver.CandidateSyntaxes)
            {
                var model = compilation.GetSemanticModel(syntax.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(syntax, context.CancellationToken);

                if (symbol is not INamedTypeSymbol)
                {
                    continue;
                }

                var codes = new List<string>();

                var ns = symbol.ContainingNamespace;
                bool hasNamespace = ns is not null && !ns.IsGlobalNamespace;

                if (hasNamespace)
                {
                    codes.Add($@"namespace {ns.ToDisplayString()}
{{
");
                }

                codes.Add($@"partial class {symbol.Name}
{{
");

                bool hasCode = false;

                foreach (var generator in generators)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();

                    if (!generator.IsTargetType(symbol))
                    {
                        break;
                    }

                    var generatedCodes = generator.GenerateCode(symbol, context.CancellationToken);

                    foreach (var generatedCode in generatedCodes)
                    {
                        if (generatedCode.Length > 0)
                        {
                            codes.Add(generatedCode);
                            hasCode = true;
                        }
                    }
                }
                
                if (!hasCode)
                {
                    continue;
                }

                codes.Add(@"}
");

                if (hasNamespace)
                {
                    codes.Add(@"}
");
                }

                var length = codes.Sum(x => x.Length);
                var buffer = new StringBuilder(length);

                foreach (var code in codes)
                {
                    buffer.Append(code);
                }

                string finalCode = buffer.ToString();

                string fileName = GetFileName(syntax, symbol);

                context.AddSource(
                    fileName,
                    SourceText.From(finalCode, Encoding.UTF8));
            }
        }

        private static string GetFileName(
            ClassDeclarationSyntax syntax,
            ITypeSymbol symbol)
        {
            string fileNameBase;

            if (string.IsNullOrEmpty(syntax.SyntaxTree.FilePath))
            {
                fileNameBase = symbol.Name;
            }
            else
            {
                fileNameBase = Path.GetFileNameWithoutExtension(syntax.SyntaxTree.FilePath);
            }

            return $"{fileNameBase}_Generated.cs";
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

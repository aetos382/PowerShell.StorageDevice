using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

using System.Text;

namespace PSAsync.CodeGenerator
{
    internal class GenerateMemberAttribute
    {
        public const string FullName = "PSAsync.GenerateMemberAttribute";

        private const string rawText = @"
namespace PSAsync
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
        private static readonly SourceText sourceText =
            SourceText.From(rawText, Encoding.UTF8);

        public static void AddToProject(
            GeneratorExecutionContext context)
        {
            context.AddSource("GenerateMemberAttibute.cs", sourceText);
        }

        public static Compilation AddToCompilation(
            GeneratorExecutionContext context)
        {
            var compilation = context.Compilation.AddSyntaxTrees(
                CSharpSyntaxTree.ParseText(
                    sourceText,
                    (CSharpParseOptions)context.ParseOptions,
                    cancellationToken: context.CancellationToken));

            return compilation;
        }
    }
}

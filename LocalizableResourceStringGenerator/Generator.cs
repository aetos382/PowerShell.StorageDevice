using Microsoft.CodeAnalysis;

using System.Diagnostics;

namespace LocalizableResourceStringGenerator
{
    [Generator]
    public class Generator :
        ISourceGenerator
    {
        public void Execute(
            GeneratorExecutionContext context)
        {
            foreach (var additionalFile in context.AdditionalFiles)
            {
                Debug.WriteLine(additionalFile.Path);
            }
        }

        public void Initialize(
            GeneratorInitializationContext context)
        {
            System.Diagnostics.Debugger.Launch();
        }
    }
}

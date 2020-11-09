using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft;
using Microsoft.CodeAnalysis;

namespace PSAsyncProvider.CodeGenerator
{
    internal class AsyncCmdletProviderMethodGenerator :
        IAsyncProviderMethodGenerator
    {
        public AsyncCmdletProviderMethodGenerator(
            CodeGenerationContext context)
        {
            Requires.NotNull(context, nameof(context));

            var helper = new AsyncCmdletProviderMethodGenerationHelper(
                context,
                "System.Management.Automation.Provider.CmdletProvider",
                "PSAsyncProvider.IAsyncCmdletProvider");

            var typeSymbols = context.TypeSymbols;

            var providerInfoSymbol = context.Compilation.GetTypeByMetadataName(
                "System.Management.Automation.ProviderInfo");

            if (providerInfoSymbol is null)
            {
                throw new InvalidOperationException();
            }
            
            this._helper = helper;

            this._start = helper.CreateMethodDelegation(
                "Start",
                new[] { providerInfoSymbol },
                providerInfoSymbol);
            
            this._startDynamicParameters = helper.CreateMethodDelegation(
                "StartDynamicParameters",
                null,
                typeSymbols.Object);
        }

        public bool IsTargetType(
            ITypeSymbol concreteProviderType)
        {
            return this._helper.IsTargetType(concreteProviderType);
        }

        public IEnumerable<string> GenerateCode(
            ITypeSymbol concreteProviderType,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            return this.GenerateCodeCore(concreteProviderType, cancellationToken);
        }

        private IEnumerable<string> GenerateCodeCore(
            ITypeSymbol concreteProviderType,
            CancellationToken cancellationToken)
        {
            string? code;

            cancellationToken.ThrowIfCancellationRequested();

            code = this.GenerateStart(concreteProviderType);
            if (!string.IsNullOrWhiteSpace(code))
            {
                yield return code!;

                cancellationToken.ThrowIfCancellationRequested();

                code = this.GenerateStartDynamicParmaeters(concreteProviderType);
                if (!string.IsNullOrWhiteSpace(code))
                {
                    yield return code!;
                }
            }

            yield break;
        }

        private string? GenerateStart(
            ITypeSymbol concreteProviderType)
        {
            if (!this._start.ShouldGenerateMethod(concreteProviderType))
            {
                return null;
            }

            return @"
// Generated Method
protected override System.Management.Automation.ProviderInfo Start(System.Management.Automation.ProviderInfo providerInfo)
{
    this.WriteVerbose($""IsItemContainer(\""{path}\"");"");
    var result = this.StartAsync(providerInfo).Result;
    this.WriteVerbose($""returns: {result}"");
    return result;
}
";
        }

        private string? GenerateStartDynamicParmaeters(
            ITypeSymbol concreteProviderType)
        {
            if (!this._startDynamicParameters.ShouldGenerateMethod(concreteProviderType))
            {
                return null;
            }

            return @"
// Generated Method
protected override object StartDynamicParameters()
{
    this.WriteVerbose($""IsItemContainer(\""{path}\"");"");
    var result = this.StartDynamicParametersAsync().Result;
    this.WriteVerbose($""returns: {result}"");
    return result;
}
";
            }

        private readonly AsyncCmdletProviderMethodGenerationHelper _helper;

        private readonly MethodDelegation _start;

        private readonly MethodDelegation _startDynamicParameters;
    }
}

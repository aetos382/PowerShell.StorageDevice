using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft;
using Microsoft.CodeAnalysis;

namespace PSAsync.CodeGenerator
{
    internal class MethodDelegation
    {
        public MethodDelegation(
            AsyncCmdletProviderMethodGenerationHelper helper,
            string methodName,
            IEnumerable<ITypeSymbol>? parameterTypes,
            ITypeSymbol returnType)
        {
            Requires.NotNull(helper, nameof(helper));
            Requires.NotNull(methodName, nameof(methodName));
            Requires.NotNull(returnType, nameof(returnType));

            var comparer = helper.Context.SymbolComparer;

            var typeSymbols = helper.Context.TypeSymbols;

            var overriddenMethod = helper.ProviderSymbol.GetMethodSymbol(
                methodName,
                parameterTypes,
                returnType,
                comparer);

            var returnTypeTask =
                comparer.Equals(returnType, typeSymbols.Void) ?
                    typeSymbols.Task :
                    typeSymbols.TaskWithValue.Construct(returnType);

            var interfaceMethod = helper.InterfaceSymbol.GetMethodSymbol(
                $"{methodName}Async",
                parameterTypes,
                returnTypeTask,
                comparer);

            if (overriddenMethod is null ||
                interfaceMethod is null)
            {
                throw new InvalidOperationException();
            }

            this.MethodName = methodName;
            this.BaseProviderMethod = overriddenMethod;
            this.AsyncInterfaceMethod = interfaceMethod;

            this._typeSymbols = typeSymbols;
            this._symbolComparer = comparer;
        }

        private readonly TypeSymbols _typeSymbols;

        public IMethodSymbol BaseProviderMethod { get; }

        public IMethodSymbol AsyncInterfaceMethod { get; }

        public string MethodName { get; }

        private readonly IEqualityComparer<ISymbol?> _symbolComparer;

        public ISymbol? GetConcreteSyncMethod(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            var baseMethod = concreteProviderType.GetOverrideSymbol(
                this.BaseProviderMethod,
                this._symbolComparer);

            return baseMethod;
        }

        public ISymbol? GetInterfaceAsyncMethod(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            var interfaceMethod = concreteProviderType.FindImplementationForInterfaceMember(
                this.AsyncInterfaceMethod);

            return interfaceMethod;
        }

        public bool IsSyncMethodImplemented(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            var syncMethod = this.GetConcreteSyncMethod(concreteProviderType);
            return syncMethod is not null;
        }

        public bool IsAsyncMethodImplemented(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            var interfaceMethod = this.GetInterfaceAsyncMethod(concreteProviderType);

            return
                interfaceMethod is not null &&
                !this._symbolComparer.Equals(interfaceMethod, this.AsyncInterfaceMethod);
        }

        public bool ShouldGenerateMethod(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            if (this.IsSyncMethodImplemented(concreteProviderType))
            {
                return false;
            }

            if (!this.IsAsyncMethodImplemented(concreteProviderType))
            {
                return false;
            }

            return true;
        }

        public string? GenerateMethod(
            ITypeSymbol concreteProviderType)
        {
            Requires.NotNull(concreteProviderType, nameof(concreteProviderType));

            if (!this.ShouldGenerateMethod(concreteProviderType))
            {
                return null;
            }

            var methodName = this.MethodName;
            var returnTypeName = this.BaseProviderMethod.ReturnType.ToDisplayString();

            var parameters = this.BaseProviderMethod.Parameters
                .Select(x => $"{x.Type.ToDisplayString()} {x.Name}");

            var parameterList = string.Join(", ", parameters);

            var arguments = this.BaseProviderMethod.Parameters
                .Select(x => $"{x.Name}");

            var argumentList = string.Join(", ", arguments);

            var argumentsToPrint = this.BaseProviderMethod.Parameters
                .Select(x =>
                    this._symbolComparer.Equals(x.Type, this._typeSymbols.String) ?
                    $"\\\"{{{x.Name}}}\\\"" : $"{{{x.Name}}}");

            var argumentListToPrint = string.Join(", ", argumentsToPrint);

            if (this.BaseProviderMethod.ReturnsVoid)
            {
                return $@"
// Generated Method
protected override void {methodName}({parameterList})
{{
    this.WriteVerbose($""{methodName}({argumentListToPrint});"");
    this.{methodName}Async({argumentList});
}}
";
            }
            else
            {
                return $@"
// Generated Method
protected override {returnTypeName} {methodName}({parameterList})
{{
    this.WriteVerbose($""{methodName}({argumentListToPrint});"");
    var result = this.{methodName}Async({argumentList}).Result;
    this.WriteVerbose($""returns: {{result}}"");
    return result;
}}
";
            }

        }
    }
}

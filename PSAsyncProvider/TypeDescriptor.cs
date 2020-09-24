using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace PSAsyncProvider
{
    internal class TypeDescriptor
    {
        private readonly static ConcurrentDictionary<Type, TypeDescriptor> _descriptors =
            new ConcurrentDictionary<Type, TypeDescriptor>();

        public static TypeDescriptor GeTypeDescriptor(
            Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return _descriptors.GetOrAdd(type, x => new TypeDescriptor(x));
        }

        public Type Type { get; }

        private TypeDescriptor(
            Type type)
        {
            this.Type = type;
        }

        private readonly Dictionary<MethodInfo, bool> _isOverridden =
            new Dictionary<MethodInfo, bool>();

        public bool IsMethodOverridden(
            MethodInfo methodInfo)
        {
            if (methodInfo is null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            if (!this._isOverridden.TryGetValue(methodInfo, out var result))
            {
                result = (methodInfo.GetBaseDefinition().DeclaringType != this.Type);
                this._isOverridden[methodInfo] = result;
            }

            return result;
        }
    }
}

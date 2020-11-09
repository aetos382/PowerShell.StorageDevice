namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class MaybeNullWhenAttribute :
        Attribute
    {
        public MaybeNullWhenAttribute(
            bool returnValue)
        {
            this.ReturnValue = returnValue;
        }

        public bool ReturnValue { get; }
    }
}

using Xunit;

namespace PSAsyncProvider.UnitTests
{
    public class TypeDescriptorTest
    {
        [Fact]
        public void GetTypeDescriptorのテスト()
        {
            var descriptor = TypeDescriptor.GeTypeDescriptor(typeof(Base));

            Assert.Equal(typeof(Base), descriptor.Type);
        }

        [Fact]
        public void IsOverriddenはオーバーライドされていない場合にfalseを返す()
        {
            var descriptor = TypeDescriptor.GeTypeDescriptor(typeof(Base));
            var method = typeof(Base).GetMethod(nameof(Base.Method));

            bool isOverridden = descriptor.IsMethodOverridden(method);

            Assert.False(isOverridden);
        }

        [Fact]
        public void IsOverriddenはオーバーライドされている場合にtrueを返す()
        {
            var descriptor = TypeDescriptor.GeTypeDescriptor(typeof(Derived));
            var method = typeof(Derived).GetMethod(nameof(Derived.OverridableMethod));

            bool isOverridden = descriptor.IsMethodOverridden(method);

            Assert.True(isOverridden);
        }

        private class Base
        {
            public void Method()
            {
            }

            public virtual void OverridableMethod()
            {
            }
        }

        private class Derived : Base
        {
            public new void Method()
            {
            }

            public override void OverridableMethod()
            {
            }
        }
    }
}

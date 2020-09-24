using System;
using System.Management.Automation.Provider;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public abstract class AsyncContainerCmdletProvider :
        ContainerCmdletProvider,
        IAsyncContainerCmdletProvider
    {
        public abstract ValueTask<bool> IsValidPathAsync(
            string path);

        protected override bool IsValidPath(
            string path)
        {
            return this.IsValidPathAsync(path).Result;
        }

        protected override void ClearItem(
            string path)
        {
            var descriptor = TypeDescriptor.GeTypeDescriptor(this.GetType());

            throw new NotImplementedException();
        }
    }
}

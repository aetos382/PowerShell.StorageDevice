using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsync.Command
{
    public abstract class AsyncCmdlet :
        Cmdlet,
        IAsyncCmdlet
    {
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            if (!this.IsOverridden(nameof(this.ProcessRecordAsync)))
            {
                return;
            }

            this.ExecuteAsyncMethod((c, t) => c.ProcessRecordAsync(t));
        }

        protected override void EndProcessing()
        {
        }

        private bool IsOverridden(
            string methodName)
        {
            var method = this.GetType().GetMethod(
                nameof(this.ProcessRecordAsync),
                new[] { typeof(CancellationToken) });

            return method.DeclaringType != typeof(AsyncCmdlet);
        }

        public virtual Task ProcessRecordAsync(
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

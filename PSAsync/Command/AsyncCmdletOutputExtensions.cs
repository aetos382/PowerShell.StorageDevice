﻿using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

using PSAsync.Command;

namespace PSAsync
{
    public static class AsyncCmdletOutputExtensions
    {
        public static Task WriteObjectAsync<TCmdlet>(
            this TCmdlet cmdlet,
            object sendToPipeline,
            CancellationToken cancellationToken = default)
            where TCmdlet :
                Cmdlet,
                IAsyncCmdlet
        {
            Requires.NotNull(cmdlet, nameof(cmdlet));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(cmdlet);

            var task = context.QueueAction(
                (c, a, _) => ((Cmdlet)c).WriteObject(a),
                sendToPipeline,
                cancellationToken);

            return task;
        }

        public static Task WriteObjectAsync<TCmdlet>(
            this TCmdlet cmdlet,
            object sendToPipeline,
            bool enumerateCollection,
            CancellationToken cancellationToken = default)
            where TCmdlet :
                Cmdlet,
                IAsyncCmdlet
        {
            Requires.NotNull(cmdlet, nameof(cmdlet));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(cmdlet);

            var task = context.QueueAction(
                (c, a, _) => ((Cmdlet)c).WriteObject(a.sendToPipeline, a.enumerateCollection),
                (sendToPipeline, enumerateCollection),
                cancellationToken);

            return task;
        }

        public static Task<ShouldContinueResult> ShouldContinueAsync<TCmdlet>(
            this TCmdlet cmdlet,
            string query,
            string caption,
            bool hasSecurityImpact,
            bool saveContext,
            CancellationToken cancellationToken = default)
            where TCmdlet :
                Cmdlet,
                IAsyncCmdlet
        {
            Requires.NotNull(cmdlet, nameof(cmdlet));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(cmdlet);

            var task = context.QueueAction(
                (c, a, x) => {

                    bool yesToAll = false;
                    bool noToAll = false;

                    if (a.saveContext)
                    {
                        (yesToAll, noToAll) = x.ShouldContinueContext;
                    }

                    bool result =
                        ((Cmdlet)c).ShouldContinue(
                            a.query,
                            a.caption,
                            a.hasSecurityImpact,
                            ref yesToAll,
                            ref noToAll);

                    if (a.saveContext)
                    {
                        x.ShouldContinueContext = new ShouldContinueContext(yesToAll, noToAll);
                    }

                    return new ShouldContinueResult(result, yesToAll, noToAll);
                },
                (query, caption, hasSecurityImpact, saveContext),
                cancellationToken);

            return task;
        }
    }
}

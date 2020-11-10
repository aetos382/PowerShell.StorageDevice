using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

namespace PSAsync
{
    public static class AsyncCommonOutputExtensions
    {
        public static Task WriteErrorAsync<TObject>(
            this TObject associatedObject,
            ErrorRecord error,
            CancellationToken cancellationToken = default)
            where TObject :
                class,
                ICmdlet
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(associatedObject);

            var task = context.QueueAction(
                (o, a, _) => ((ICmdlet)o).WriteError(a),
                error,
                cancellationToken);

            return task;
        }

        public static Task WriteWarningAsync<TObject>(
            this TObject associatedObject,
            string message,
            CancellationToken cancellationToken = default)
            where TObject :
                class,
                ICmdlet
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(associatedObject);

            var task = context.QueueAction(
                (o, a, _) => ((ICmdlet)o).WriteWarning(a),
                message,
                cancellationToken);

            return task;
        }

        public static Task WriteVerboseAsync<TObject>(
            this TObject associatedObject,
            string message,
            CancellationToken cancellationToken = default)
            where TObject :
                class,
                ICmdlet
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            var context = AsyncMethodContext.GetContext(associatedObject);

            var task = context.QueueAction(
                (o, a, _) => ((ICmdlet)o).WriteVerbose(a),
                message,
                cancellationToken);

            return task;
        }

        public static Task WriteInformationAsync<TObject>(
            this TObject associatedObject,
            InformationRecord information,
            CancellationToken cancellationToken = default)
            where TObject :
                class,
                ICmdlet
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(associatedObject);

            var task = context.QueueAction(
                (o, a, _) => ((ICmdlet)o).WriteInformation(a),
                information,
                cancellationToken);

            return task;
        }

        public static Task WriteInformationAsync<TObject>(
            this TObject associatedObject,
            object messageData,
            string[] tags,
            CancellationToken cancellationToken = default)
            where TObject :
                class,
                ICmdlet
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(associatedObject);

            var task = context.QueueAction(
                (o, a, _) => ((ICmdlet)o).WriteInformation(a.messageData, a.tags),
                (messageData, tags),
                cancellationToken);

            return task;
        }

        public static Task WriteProgressAsync<TObject>(
            this TObject associatedObject,
            ProgressRecord progress,
            CancellationToken cancellationToken = default)
            where TObject :
                class,
                ICmdlet
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(associatedObject);

            var task = context.QueueAction(
                (o, a, _) => ((ICmdlet)o).WriteProgress(a),
                progress,
                cancellationToken);

            return task;
        }

        public static Task<bool> ShouldProcessAsync<TObject>(
            this TObject associatedObject,
            string target,
            CancellationToken cancellationToken = default)
            where TObject :
                class,
                ICmdlet
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(associatedObject);

            var task = context.QueueAction(
                (o, a, _) => ((ICmdlet)o).ShouldProcess(a),
                target,
                cancellationToken);

            return task;
        }

        public static Task<bool> ShouldProcessAsync<TObject>(
            this TObject associatedObject,
            string target,
            string action,
            CancellationToken cancellationToken = default)
            where TObject :
                class,
                ICmdlet
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(associatedObject);

            var task = context.QueueAction(
                (o, a, _) => ((ICmdlet)o).ShouldProcess(a.target, a.action),
                (target, action),
                cancellationToken);

            return task;
        }

        public static Task<ShouldProcessResult> ShouldProcessAsync<TObject>(
            this TObject associatedObject,
            string verboseDescription,
            string verboseWarning,
            string caption,
            CancellationToken cancellationToken = default)
            where TObject :
                class,
                ICmdlet
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(associatedObject);

            var task = context.QueueAction(
                (o, a, _) => {
                    bool result = ((ICmdlet)o).ShouldProcess(a.verboseDescription, a.verboseWarning, a.caption, out var reason);
                    return new ShouldProcessResult(result, reason);
                },
                (verboseDescription, verboseWarning, caption),
                cancellationToken);

            return task;
        }

        public static Task<ShouldContinueResult> ShouldContinueAsync<TObject>(
            this TObject associatedObject,
            string query,
            string caption,
            bool saveContext,
            CancellationToken cancellationToken = default)
            where TObject :
                class,
                ICmdlet
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            cancellationToken.ThrowIfCancellationRequested();

            var context = AsyncMethodContext.GetContext(associatedObject);

            var task = context.QueueAction(
                (o, a, x) => {

                    bool yesToAll = false;
                    bool noToAll = false;

                    if (a.saveContext)
                    {
                        (yesToAll, noToAll) = x.ShouldContinueContext;
                    }

                    bool result =
                        ((ICmdlet)o).ShouldContinue(
                            a.query,
                            a.caption,
                            ref yesToAll,
                            ref noToAll);

                    if (a.saveContext)
                    {
                        x.ShouldContinueContext = new ShouldContinueContext(yesToAll, noToAll);
                    }

                    return new ShouldContinueResult(result, yesToAll, noToAll);
                },
                (query, caption, saveContext),
                cancellationToken);

            return task;
        }
    }
}

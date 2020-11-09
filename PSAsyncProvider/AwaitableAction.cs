﻿using System;
using System.Management.Automation.Provider;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

namespace PSAsyncProvider
{
    internal class AwaitableAction<TProvider, TArgument, TResult>
        where TProvider :
            CmdletProvider,
            IAsyncCmdletProvider
    {
        internal AwaitableAction(
            TProvider cmdlet,
            Func<TProvider, TArgument, TResult> action,
            TArgument argument,
            Action<object?>? postAction = null,
            object? postActionState = null,
            CancellationToken cancellationToken = default)
        {
            Requires.NotNull(cmdlet, nameof(cmdlet));
            Requires.NotNull(action, nameof(action));

            this._cmdlet = cmdlet;
            this._action = action;
            this._argument = argument;
            this._postAction = postAction;
            this._postActionState = postActionState;
            this._cancellationToken = cancellationToken;

            cancellationToken.Register(
                () => this._tcs.TrySetCanceled(cancellationToken),
                false);
        }

        private readonly TProvider _cmdlet;

        private readonly Func<TProvider, TArgument, TResult> _action;

        private readonly TArgument _argument;

        private readonly Action<object?>? _postAction;

        private readonly object? _postActionState;

        private readonly CancellationToken _cancellationToken;

        private readonly TaskCompletionSource<TResult> _tcs = new TaskCompletionSource<TResult>();

#pragma warning disable CA1031 // 一般的な例外の種類はキャッチしません

        public void Invoke()
        {
            TResult result;

            try
            {
                if (this._cancellationToken.IsCancellationRequested)
                {
                    this._tcs.TrySetCanceled(this._cancellationToken);
                    return;
                }

                result = this._action(this._cmdlet, this._argument);
            }
            catch (OperationCanceledException ex)
            {
                this._tcs.TrySetCanceled(ex.CancellationToken);
                return;
            }
            catch (Exception ex)
            {
                this._tcs.TrySetException(ex);
                return;
            }
            finally
            {
                this._postAction?.Invoke(this._postActionState);
            }

            this._tcs.TrySetResult(result);
        }

#pragma warning restore CA1031 // 一般的な例外の種類はキャッチしません

        public Task<TResult> Task
        {
            get
            {
                return this._tcs.Task;
            }
        }
    }
}

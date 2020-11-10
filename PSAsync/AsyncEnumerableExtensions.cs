using Microsoft;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsync
{
    public static class AsyncEnumerableExtensions
    {
        public static async ValueTask<IEnumerable<T>> ToEnumerableAsync<T>(
            this IAsyncEnumerable<T> source,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(source, nameof(source));

            return await ToListAsync(source, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<IEnumerable<T>> ToArrayAsync<T>(
            this IAsyncEnumerable<T> source,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(source, nameof(source));

            return (await ToListAsync(source, cancellationToken).ConfigureAwait(false)).ToArray();
        }

        public static async ValueTask<IList<T>> ToListAsync<T>(
            this IAsyncEnumerable<T> source,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(source, nameof(source));

            await using var enumerator = source.GetAsyncEnumerator(cancellationToken);

            var result = new List<T>();

            while (await enumerator.MoveNextAsync().ConfigureAwait(false))
            {
                result.Add(enumerator.Current);
            }

            return result;
        }
    }
}

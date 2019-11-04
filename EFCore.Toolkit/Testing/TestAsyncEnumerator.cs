using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.Toolkit.Testing
{
    /// <summary>
    /// TestAsyncEnumerator&lt;T&gt; implements <seealso cref="IAsyncEnumerator&lt;T&gt;"/>
    /// which is used to mock collections behind async queryables -> ToListAsync
    /// 
    /// Source: https://stackoverflow.com/questions/40476233/how-to-mock-an-async-repository-with-entity-framework-core
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            this.inner = inner;
        }

        public T Current => this.inner.Current;

#if NETSTANDARD2_1
        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(this.inner.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            this.inner.Dispose();
            return new ValueTask();
        }
#else    
        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.inner.MoveNext());
        }

        public void Dispose()
        {
            this.inner.Dispose();
        }
#endif
    }
}
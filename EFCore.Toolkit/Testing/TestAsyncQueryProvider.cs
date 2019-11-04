using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EFCore.Toolkit.Testing
{
    /// <summary>
    /// TestAsyncQueryProvider implements <seealso cref="IAsyncQueryProvider"/>
    /// which is used to mock collections behind async queryables -> ToListAsync
    /// 
    /// Source: https://stackoverflow.com/questions/40476233/how-to-mock-an-async-repository-with-entity-framework-core
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            this.inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return this.inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return this.inner.Execute<TResult>(expression);
        }

#if NETSTANDARD2_1
        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return this.Execute<TResult>(expression);
        }
#else
        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new TestAsyncEnumerable<TResult>(expression);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Execute<TResult>(expression));
        }
#endif
    }
}
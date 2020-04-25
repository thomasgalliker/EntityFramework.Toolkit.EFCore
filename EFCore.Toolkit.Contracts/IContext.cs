﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
#if !NET40

#endif

namespace EFCore.Toolkit.Abstractions
{
    public interface IContext : IDisposable
    {
        /// <summary>
        ///     Drops and recreates the underlying database.
        ///     USE WITH CARE!
        /// </summary>
        void ResetDatabase();

        /// <summary>
        ///     Drops the underlying database.
        ///     USE WITH CARE!
        /// </summary>
        void DropDatabase();

        /// <summary>
        ///     Updates the given entity.
        /// </summary>
        TEntity Edit<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     Updates the given attached <paramref name="originalEntity" /> with the new entity <paramref name="updateEntity" />.
        /// </summary>
        TEntity Edit<TEntity>(TEntity originalEntity, TEntity updateEntity) where TEntity : class;

        TEntity Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     Reverts changes in <paramref name="entity" />.
        /// </summary>
        void UndoChanges<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        ///     Modifies the properties with <paramref name="propertyNames" /> of given entity <paramref name="entity" />.
        /// </summary>
        void ModifyProperties<TEntity>(TEntity entity, params string[] propertyNames) where TEntity : class;

        void LoadReferenced<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> navigationProperty) where TEntity : class where TProperty : class;

        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        ChangeSet SaveChanges();

#if !NET40
        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        Task<ChangeSet> SaveChangesAsync();
#endif

        ITransaction BeginTransaction();
        void UseTransaction(ITransaction transaction);
    }

    public interface ITransaction : IDisposable
    {
        /// <summary>
        ///     Commits all changes made to the database in the current transaction.
        /// </summary>
        void Commit();

        /// <summary>
        ///     Discards all changes made to the database in the current transaction.
        /// </summary>
        void Rollback();
    }
}
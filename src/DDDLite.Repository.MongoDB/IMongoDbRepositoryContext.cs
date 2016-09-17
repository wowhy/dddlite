namespace DDDLite.Repository.MongoDB
{
    using System;
    using DDDLite.Domain.Repositories;
    using Domain.Core;
    using global::MongoDB.Driver;

    public interface IMongoDBRepositoryContext : IRepositoryContext
    {
        IMongoClient Client { get; }

        IMongoDBRepositoryContextSettings Settings { get; }

        IMongoCollection<TAggregateRoot> GetCollection<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot;

        void RegisterInsert<TAggregateRoot>(TAggregateRoot entity)
            where TAggregateRoot : class, IAggregateRoot;

        void RegisterUpdate<TAggregateRoot>(TAggregateRoot entity)
            where TAggregateRoot : class, IAggregateRoot;

        void RegisterDelete<TAggregateRoot>(TAggregateRoot entity)
            where TAggregateRoot : class, IAggregateRoot;
    }
}
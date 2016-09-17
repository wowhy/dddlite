namespace DDDLite.Repositories.MongoDB
{
    using System;
    using DDDLite.Repositories;
    using DDDLite.Core;
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
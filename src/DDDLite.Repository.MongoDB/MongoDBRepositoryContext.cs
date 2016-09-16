namespace DDDLite.Repository.MongoDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Core;
    using Domain.Repositories;
    using global::MongoDB.Driver;
    using Humanizer;

    public class MongoDBRepositoryContext : RepositoryContext, IMongoDBRepositoryContext
    {
        private readonly IMongoClient client;
        private readonly IMongoDBRepositoryContextSettings settings;
        private readonly IMongoDatabase database;
        private readonly Func<string, string> collectionNameResolver;

        private readonly Queue<Action> actions = new Queue<Action>();

        private readonly Dictionary<Type, object> mapCollections = new Dictionary<Type, object>();

        public MongoDBRepositoryContext(IMongoDBRepositoryContextSettings settings)
        {
            this.settings = settings;
            this.client = new MongoClient(settings.ClientSettings);

            this.database = this.client.GetDatabase(
                this.settings.DatabaseName,
                this.settings.DatabaseSettings);
            this.collectionNameResolver = this.settings.CollectionNameResolver;
            if (this.collectionNameResolver == null)
            {
                this.collectionNameResolver = x => x.Pluralize();
            }
        }

        public IMongoClient Client => this.client;
        public IMongoDBRepositoryContextSettings Settings => this.settings;

        public override void Commit()
        {
            this.Clear();
        }

        public override Task CommitAsync()
        {
            foreach (var action in this.actions)
            {
                action();
            }

            return Task.CompletedTask;
        }

        public IMongoCollection<TAggregateRoot> GetCollection<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot
        {
            var type = typeof(TAggregateRoot);
            if (!this.mapCollections.ContainsKey(type))
            {
                var collection = this.database.GetCollection<TAggregateRoot>(
                    collectionNameResolver(typeof(TAggregateRoot).Name),
                    this.settings.CollectionSettings);
                this.mapCollections.Add(type, collection);
                return collection;
            }

            return (IMongoCollection<TAggregateRoot>)this.mapCollections[type];
        }

        internal void RegisterInsert<TAggregateRoot>(TAggregateRoot entity)
            where TAggregateRoot : class, IAggregateRoot
        {
            this.actions.Enqueue(() =>
            {
                var collection = this.GetCollection<TAggregateRoot>();
                collection.InsertOne(entity);
            });
        }

        internal void RegisterUpdate<TAggregateRoot>(TAggregateRoot entity)
            where TAggregateRoot : class, IAggregateRoot
        {
            this.actions.Enqueue(() =>
            {
                var collection = this.GetCollection<TAggregateRoot>();
                collection.ReplaceOne<TAggregateRoot>(k => k.Id == entity.Id, entity, new UpdateOptions { IsUpsert = true });
            });
        }

        internal void RegisterDelete<TAggregateRoot>(TAggregateRoot entity)
            where TAggregateRoot : class, IAggregateRoot
        {
            this.actions.Enqueue(() =>
            {
                var collection = this.GetCollection<TAggregateRoot>();
                collection.DeleteOne<TAggregateRoot>(k => k.Id == entity.Id);
            });
        }

        protected override IRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
        {
            return new MongoDBRepository<TAggregateRoot>(this);
        }

        protected override void Dispose(bool disposing)
        {
        }

        private void Clear()
        {
            this.actions.Clear();
        }
    }
}

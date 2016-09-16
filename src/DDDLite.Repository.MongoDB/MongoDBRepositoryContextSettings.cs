namespace DDDLite.Repository.MongoDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using global::MongoDB.Driver;
    using Humanizer;

    public class MongoDBRepositoryContextSettings : IMongoDBRepositoryContextSettings
    {
        public MongoDBRepositoryContextSettings(string databaseName, MongoClientSettings clientSettings)
            : this(databaseName, clientSettings, null, null, x => x.Pluralize())
        {
        }

        public MongoDBRepositoryContextSettings(string databaseName,
            MongoClientSettings clientSettings,
            MongoDatabaseSettings databaseSettings,
            MongoCollectionSettings collectionSettings,
            Func<string, string> collectionNameResolver)
        {
            this.DatabaseName = databaseName;
            this.CollectionNameResolver = collectionNameResolver;
            this.ClientSettings = clientSettings;
            this.DatabaseSettings = databaseSettings;
            this.CollectionSettings = collectionSettings;
        }

        public string DatabaseName { get; set; }

        public Func<string, string> CollectionNameResolver { get; set; }

        public MongoClientSettings ClientSettings { get; set; }

        public MongoDatabaseSettings DatabaseSettings { get; set; }

        public MongoCollectionSettings CollectionSettings { get; set; }
    }
}

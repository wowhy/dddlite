namespace DDDLite.Repositories.MongoDB
{
    using System;
    using global::MongoDB.Driver;

    public interface IMongoDBRepositoryContextSettings
    {
    string DatabaseName { get; set; }

    Func<string, string> CollectionNameResolver { get; set; }

    MongoClientSettings ClientSettings { get; set; }

    MongoDatabaseSettings DatabaseSettings { get; set; }

    MongoCollectionSettings CollectionSettings { get; set; }
  }
}
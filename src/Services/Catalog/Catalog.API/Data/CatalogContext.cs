using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        /// <summary>
        /// Creates collection in the constructor
        /// </summary>
        /// <param name="configuration"></param>
        public CatalogContext(IConfiguration configuration)
        {
            string dbConn = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            string dbName = configuration.GetValue<string>("DatabaseSettings:DatabasName");
            string collection = configuration.GetValue<string>("DatabaseSettings:CollectionName");

            var client = new MongoClient(dbConn);
            var database = client.GetDatabase(dbName);

            Products = database.GetCollection<Product>(collection);
            CatalogContextSeed.SeedData(Products);

        }
        public IMongoCollection<Product> Products  {get;}
    }
}

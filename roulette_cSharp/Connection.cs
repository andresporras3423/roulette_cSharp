using MongoDB.Driver;
using roulette_cSharp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace roulette_cSharp
{
    public class Connection
    {
        public MongoClient Client { get; set; }

        public IMongoDatabase Database { get; set; }
        public Connection()
        {
            Client = new MongoClient("mongodb://localhost:27017");
            Database = Client.GetDatabase("roulette");
        }
    }
}

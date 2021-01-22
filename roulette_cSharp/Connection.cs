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
            Client = new MongoClient("mongodb+srv://oscar:Oscarrussi1234@cluster0.6b6cp.mongodb.net/test");
            Database = Client.GetDatabase("roulette");
        }
    }
}

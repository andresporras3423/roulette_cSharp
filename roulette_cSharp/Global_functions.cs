using MongoDB.Bson;
using MongoDB.Driver;
using roulette_cSharp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace roulette_cSharp
{
    public static class Global_functions
    {
        readonly static Connection c = new Connection();
        public static void Update_player_credit(string id_player, double increase)
        {
            IMongoCollection<Player> players = c.Database.GetCollection<Player>("player");
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq("_id", ObjectId.Parse(id_player));
            UpdateDefinition<Player> update = Builders<Player>.Update.Inc("credit", increase);
            players.UpdateOne(filter, update);
        }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace roulette_cSharp.models
{
    public class Bet
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("id_game")]
        public string Id_game { get; set; }
        [BsonElement("id_player")]
        public string Id_player { get; set; }
        [BsonElement("bet_number")]
        public int Bet_number { get; set; }
        [BsonElement("value")]
        public double Value { get; set; }
    }
}

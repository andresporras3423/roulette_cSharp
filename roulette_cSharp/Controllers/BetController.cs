using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using roulette_cSharp.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace roulette_cSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        readonly Connection c = new Connection();
        [HttpPost]
        public string Post([FromBody] object response)
        {
            JObject json_params = JObject.Parse(response.ToString());
            try
            {
                var filter = Builders<Player>.Filter.Eq("_id", ObjectId.Parse((String)json_params["id_player"]));
                var nPlayer = c.Database.GetCollection<Player>("player").Find(filter).FirstOrDefault();
                var update = Builders<Player>.Update.Set("credit", nPlayer.Credit - (Double)json_params["value"]);
                var players = c.Database.GetCollection<Player>("player");
                players.UpdateOne(filter, update);
                var bets = c.Database.GetCollection<Bet>("bet");
                Bet nBet = new Bet { Id_game = (String)json_params["id_game"], Id_player = (String)json_params["id_player"], Bet_number = (Int32)json_params["bet_number"], Value = (Double)json_params["value"] };
                bets.InsertOne(nBet);
                return nBet.Id.ToString();
            }
            catch
            {
                return "-1";
            }
        }
    }
}

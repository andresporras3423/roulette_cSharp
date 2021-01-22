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
                update_player_credit((String)json_params["id_player"], -1 * (Double)json_params["value"]);
                var bets = c.Database.GetCollection<Bet>("bet");
                //bet_number: from 0 until 36 for specific number bet, 37 for odd numbers, 38 for even numbers
                Bet nBet = new Bet { Id_game = (String)json_params["id_game"], Id_player = (String)json_params["id_player"], Bet_number = (Int32)json_params["bet_number"], Value = (Double)json_params["value"] };
                bets.InsertOne(nBet);

                return nBet.Id.ToString();
            }
            catch
            {

                return "-1";
            }
        }
        [HttpPut]
        public bool Put(string idGame)
        {
            try
            {
                close_game(idGame);
                int winner_number = new Random().Next(0, 36);
                var filter = Builders<Bet>.Filter.Eq("id_game", idGame);
                var bets = c.Database.GetCollection<Bet>("bet").Find(filter).ToList();
                give_prize_to_winner(bets, winner_number);
                //c.Database.RunCommand(new JsonCommand<BsonDocument>(""));

                return true;
            }
            catch
            {

                return false;
            }
        }

        private void give_prize_to_winner(List<Bet> bets, int winner_number)
        {
            JObject message = new JObject();
            message.Add("winner_number", winner_number);
            foreach (var b in bets)
            {
                float increase_factor = 0f;
                if ((b.Bet_number == 38 && winner_number % 2 == 0) || (b.Bet_number == 37 && winner_number % 2 == 1)) increase_factor = 1.8f;
                else if (b.Bet_number == winner_number) increase_factor = 5f;
                update_player_credit(b.Id_player, b.Value * increase_factor);
            }
        }
        private void close_game(string id)
        {
            var games = c.Database.GetCollection<Game>("game");
            var filter = Builders<Game>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Game>.Update.Set("status", 2);
            games.UpdateOne(filter, update);
        }
        private void update_player_credit(string id_player, double increase)
        {
            var players = c.Database.GetCollection<Player>("player");
            var filter = Builders<Player>.Filter.Eq("_id", ObjectId.Parse(id_player));
            var update = Builders<Player>.Update.Inc("credit", increase);
            players.UpdateOne(filter, update);
        }
    }
}

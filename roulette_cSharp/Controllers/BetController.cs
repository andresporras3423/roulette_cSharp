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
        //bet_number: from 0 until 36 for specific number bet, 37 for odd numbers, 38 for even numbers
        public string Post([FromBody] object response)
        {
            JObject json_params = JObject.Parse(response.ToString());
            try
            {
                update_player_credit((String)json_params["id_player"], -1 * (Double)json_params["value"]);
                IMongoCollection<Bet> bets = c.Database.GetCollection<Bet>("bet");
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
        public string Put(string idGame)
        {
            try
            {
                close_game(idGame);
                int winner_number = new Random().Next(0, 36);
                FilterDefinition<Bet> filter = Builders<Bet>.Filter.Eq("id_game", idGame);
                List<Bet> bets = c.Database.GetCollection<Bet>("bet").Find(filter).ToList();
                JObject json_data = give_prize_to_winner(bets, winner_number);
                return json_data.ToString();
            }
            catch
            {

                return "-1";
            }
        }
        private JObject give_prize_to_winner(List<Bet> bets, int winner_number)
        {
            JObject message = new JObject();
            JObject bets_data = new JObject();
            message.Add("winner_number", winner_number);
            for(int i=0; i< bets.Count; i++)
            {
                float increase_factor = 0f;
                if ((bets[i].Bet_number == 38 && winner_number % 2 == 0) || (bets[i].Bet_number == 37 && winner_number % 2 == 1)) increase_factor = 1.8f;
                else if (bets[i].Bet_number == winner_number) increase_factor = 5f;
                update_player_credit(bets[i].Id_player, bets[i].Value * increase_factor);
                JObject current_bet = add_data_current_bet(bets[i], increase_factor);
                bets_data.Add(bets[i].Id.ToString(), current_bet);
            }
            message.Add("bets", bets_data);

            return message;
        }
        private JObject add_data_current_bet(Bet bet_data, float increase_factor)
        {
            JObject current_bet = new JObject();
            current_bet.Add("id_player", bet_data.Id_player.ToString());
            string bet_current_player = bet_data.Bet_number < 37 ? bet_data.Bet_number.ToString() : (bet_data.Bet_number == 37 ? "Black" : "Red");
            current_bet.Add("bet", bet_current_player);
            current_bet.Add("status", increase_factor == 0f ? "Lose" : "Win");
            current_bet.Add("prize", increase_factor == 0f ? 0 : bet_data.Value * increase_factor);

            return current_bet;
        }
        private void close_game(string id)
        {
            IMongoCollection<Game> games = c.Database.GetCollection<Game>("game");
            FilterDefinition<Game> filter = Builders<Game>.Filter.Eq("_id", ObjectId.Parse(id));
            UpdateDefinition<Game> update = Builders<Game>.Update.Set("status", 2);
            games.UpdateOne(filter, update);
        }
        private void update_player_credit(string id_player, double increase)
        {
            IMongoCollection<Player> players = c.Database.GetCollection<Player>("player");
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq("_id", ObjectId.Parse(id_player));
            UpdateDefinition<Player> update = Builders<Player>.Update.Inc("credit", increase);
            players.UpdateOne(filter, update);
        }
    }
}

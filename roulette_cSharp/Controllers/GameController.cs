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
    [Route("[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        readonly Connection c = new Connection();
        [HttpGet]
        public IEnumerable<Dictionary<string, string>> get_all()
        {
            try
            {
                List<Game> games = c.Database.GetCollection<Game>("game").Find(g => true).ToList();
                return generate_list_games(games);
            }
            catch
            {

                return null;
            }
        }
        [HttpPost]
        public string create()
        {
            try
            {
                IMongoCollection<Game> games = c.Database.GetCollection<Game>("game");
                Game nGame = new Game {Status=0}; //0=created, 1=open, 2=close
                games.InsertOne(nGame);

                return nGame.Id.ToString();
            }
            catch
            {

                return "-1";
            }
        }
        [HttpPut]
        public bool open(string id)
        {
            try
            {
                update_game_status(id, 1);

                return true;
            }
            catch
            {

                return false;
            }
        }

        [HttpPut]
        public string close(string idGame)
        {
            try
            {
                update_game_status(idGame, 2);
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
        private JObject give_prize_to_winner(List<Bet> bets, double winner_number)
        {
            JObject message = new JObject();
            JObject bets_data = new JObject();
            message.Add("winner_number", winner_number);
            for (int i = 0; i < bets.Count; i++)
            {
                float increase_factor = 0f;
                if ((bets[i].Bet_number == 38 && winner_number % 2 == 0) || (bets[i].Bet_number == 37 && winner_number % 2 == 1)) increase_factor = 1.8f;
                else if (bets[i].Bet_number == winner_number) increase_factor = 5f;
                Global_functions.Update_player_credit(bets[i].Id_player, bets[i].Value * increase_factor);
                JObject current_bet = add_data_current_bet(bets[i], increase_factor);
                bets_data.Add(bets[i].Id.ToString(), current_bet);
            }
            message.Add("bets", bets_data);

            return message;
        }
        private JObject add_data_current_bet(Bet bet_data, double increase_factor)
        {
            JObject current_bet = new JObject();
            current_bet.Add("id_player", bet_data.Id_player.ToString());
            string bet_current_player = bet_data.Bet_number < 37 ? bet_data.Bet_number.ToString() : (bet_data.Bet_number == 37 ? "Black" : "Red");
            current_bet.Add("bet", bet_current_player);
            current_bet.Add("status", increase_factor == 0f ? "Lose" : "Win");
            current_bet.Add("prize", increase_factor == 0f ? 0 : bet_data.Value * increase_factor);

            return current_bet;
        }
        private List<Dictionary<string, string>> generate_list_games(List<Game> games)
        {
            string[] status = { "Created", "Open", "Closed" };
            List<Dictionary<string, string>> list_games = new List<Dictionary<string, string>>();
            foreach (var game in games)
            {
                Dictionary<string, string> current_game = new Dictionary<string, string>();
                current_game["id"] = game.Id.ToString();
                current_game["status"] = status[game.Status];
                list_games.Add(current_game);
            }

            return list_games;
        }

        private void update_game_status(string id_game, int new_status)
        {
            IMongoCollection<Game> games = c.Database.GetCollection<Game>("game");
            FilterDefinition<Game> filter = Builders<Game>.Filter.Eq("_id", ObjectId.Parse(id_game));
            UpdateDefinition<Game> update = Builders<Game>.Update.Set("status", new_status);
            games.UpdateOne(filter, update);
        }
    }
}
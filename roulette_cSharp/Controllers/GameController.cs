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
    public class GameController : ControllerBase
    {
        readonly Connection c = new Connection();
        [HttpGet]
        public IEnumerable<Dictionary<string, string>> Get()
        {
            try
            {
                var games = c.Database.GetCollection<Game>("game").Find(g => true).ToList();
                return generate_list_games(games);
            }
            catch
            {

                return null;
            }
        }
        [HttpPost]
        public string Post()
        {
            try
            {
                var games = c.Database.GetCollection<Game>("game");
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
        public bool Put(string id)
        {
            try
            {
                var games = c.Database.GetCollection<Game>("game");
                var filter = Builders<Game>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Game>.Update.Set("status", 1);
                games.UpdateOne(filter, update);

                return true;
            }
            catch
            {

                return false;
            }
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
    }
}
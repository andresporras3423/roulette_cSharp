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
        [HttpPost]
        public string Post()
        {
            try
            {
                var games = c.Database.GetCollection<Game>("game");
                Game nGame = new Game {Status=0};
                games.InsertOne(nGame);

                return nGame.Id.ToString();
            }
            catch
            {

                return "error message";
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
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class playerController : ControllerBase
    {
        readonly Connection c = new Connection();
        [HttpPost]
        //body params: string name, double credit
        public bool create([FromBody]object response)
        {
            JObject json_params = JObject.Parse(response.ToString());
            try
            {
                IMongoCollection<Player> players = c.Database.GetCollection<Player>("player");
                Player nPlayer = new Player { Name = (String)json_params["name"], Credit = (Double)json_params["credit"] };
                players.InsertOne(nPlayer);

                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
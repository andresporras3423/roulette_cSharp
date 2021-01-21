using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class playerController : ControllerBase
    {
        readonly Connection c = new Connection();
        [HttpPost]
        public bool Post([FromBody]object response)
        {
            JObject p = JObject.Parse(response.ToString());
            try
            {
                var players = c.Database.GetCollection<Player>("player");
                Player nPlayer = new Player { Name = (String)p["name"], Credit = (Double)p["credit"] };
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

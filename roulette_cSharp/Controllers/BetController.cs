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
    public class BetController : ControllerBase
    {
        readonly Connection c = new Connection();
        [HttpPost]
        //Body params: string id_player, double value, int bet_number, string id_game
        //bet_number: from 0 until 36 for specific number bet, 37 for odd numbers, 38 for even numbers
        public string create([FromBody] object response)
        {
            JObject json_params = JObject.Parse(response.ToString());
            try
            {
                Global_functions.Update_player_credit((String)json_params["id_player"], -1 * (Double)json_params["value"]);
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
    }
}

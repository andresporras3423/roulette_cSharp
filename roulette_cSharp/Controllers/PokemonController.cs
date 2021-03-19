using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace roulette_cSharp.Controllers
{
    public class PokemonController : ControllerBase
    {
        [HttpGet("pokemon/get")]
        public IActionResult get()
        {
            string parent_path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName;
            string pokemons_file = Path.Combine(parent_path, "roulette_cSharp\\texts\\pokemons.txt");
            string text = System.IO.File.ReadAllText(pokemons_file);
            return Ok(text);
        }
    }
}

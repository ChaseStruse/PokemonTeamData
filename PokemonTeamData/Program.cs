using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PokemonTeamData.Repository;
namespace PokemonTeamData
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Program program = new Program();
            ApiClient apiClient = new ApiClient();

            Uri uri = new Uri("https://pokeapi.co/api/v2/pokemon/charmander");
            var pokemon = await apiClient.GetPokemon(uri);

            Console.WriteLine("Pokemon ID: " + pokemon.Id);
            Console.WriteLine("Pokemon Name: " + pokemon.Name);
            Console.WriteLine("Pokemon Types: " + pokemon.Types);
            Console.WriteLine("Pokemon Abilities: " + pokemon.Abilities);
        }

    }
}

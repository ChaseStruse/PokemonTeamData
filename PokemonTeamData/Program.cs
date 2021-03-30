using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PokemonTeamData.Repository;
using PokemonTeamData.Repository.Models;

namespace PokemonTeamData
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Program program = new Program();
            ApiClient apiClient = new ApiClient();

            Uri uri = new Uri("https://pokeapi.co/api/v2/pokemon/charizard");
            var pokemon = await apiClient.GetPokemon(uri);
            program.printPokemon(pokemon);
        }

        private void printPokemon(Pokemon pokemon)
        {
            Console.WriteLine("Pokemon ID: " + pokemon.Id);
            Console.WriteLine("Pokemon Name: " + pokemon.Name);
            Console.WriteLine("");
            Console.WriteLine("Types: ");
            foreach (var pokemonType in pokemon.Types)
            {
                Console.WriteLine(pokemonType.Name);
            }
            Console.WriteLine("");
            Console.WriteLine("Abilities:");
            foreach (var pokemonAbility in pokemon.Abilities)
            {
                Console.WriteLine(pokemonAbility.Name);
            }
            Console.WriteLine("");
            Console.WriteLine("Base Stats:");
            foreach (var pokemonBaseStats in pokemon.Stats)
            {
                Console.WriteLine(pokemonBaseStats.Name);
                Console.WriteLine(pokemonBaseStats.BaseValue);
            }
        }

    }
}

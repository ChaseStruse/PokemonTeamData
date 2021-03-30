using System;
using System.Threading.Tasks;
using PokemonTeamData.Repository;
using PokemonTeamData.Repository.Models;

namespace PokemonTeamData.Service
{
    public interface IPokemonService
    {
        Uri UriBuilder(string pokemonName);
        Task<Pokemon> GetPokemon(Uri uri);
        Team AddPokemonToTeam(Team team, Pokemon pokemonToAdd);
    }

    public class PokemonService : IPokemonService
    {
        private readonly IApiClient _client;

        public PokemonService()
        {
            _client = new ApiClient();
        }

        public Uri UriBuilder(string pokemonName)
        {
            var uri = new Uri("https://pokeapi.co/api/v2/pokemon/" + pokemonName);
            return uri;
        }

        public async Task<Pokemon> GetPokemon(Uri uri)
        {

            var pokemon = await _client.GetPokemon(uri);
            return pokemon;
        }

        public Team AddPokemonToTeam(Team team, Pokemon pokemonToAdd)
        {
            if(team.Pokemon.Count < 6)
            {
                team.Pokemon.Add(pokemonToAdd);
            }
            else
            {
                Console.WriteLine("Pokemon was not added due to team already having 6 pokemon");
            }
            
            return team;
        }
    }
}

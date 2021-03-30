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
    }
}

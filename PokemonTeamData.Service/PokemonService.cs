using System;
using PokemonTeamData.Repository.Models;

namespace PokemonTeamData.Service
{
    public interface IPokemonService
    {
        Uri UriBuilder(string pokemonName);
    }

    public class PokemonService
    {
        public PokemonService()
        {
        }

        public Uri UriBuilder(string pokemonName)
        {
            var uri = new Uri("https://pokeapi.co/api/v2/pokemon/" + pokemonName);
            return uri;
        }

        public Pokemon GetPokemon(Uri uri)
        {
            throw new NotImplementedException();
        }
    }
}

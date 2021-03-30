using System;
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
            throw new NotImplementedException();
        }
    }
}

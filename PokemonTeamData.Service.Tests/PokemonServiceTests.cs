using System;
using Xunit;
using FluentAssertions;

namespace PokemonTeamData.Service.Tests
{
    public class PokemonServiceTests
    {
        private readonly PokemonService _sut;

        public PokemonServiceTests()
        {
            _sut = new PokemonService();
        }

        [Fact]
        public void GivenValidPokemonName_UriBuilder_CreatesAndReturnsValidUri()
        {
            Uri expected = new Uri("https://pokeapi.co/api/v2/pokemon/charmander");
            var actual = _sut.UriBuilder("charmander");

            actual.Should().BeEquivalentTo(expected);
        }
    }
}

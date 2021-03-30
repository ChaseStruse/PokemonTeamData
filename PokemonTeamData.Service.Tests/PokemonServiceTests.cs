using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using PokemonTeamData.Repository.Models;
using System.Threading.Tasks;

namespace PokemonTeamData.Service.Tests
{
    public class PokemonServiceTests
    {
        private readonly IPokemonService _sut;

        public PokemonServiceTests()
        {
            _sut = new PokemonService();
        }

        [Fact]
        public async Task GivenValidPokemonName_GetPokemon_ReturnsCorrectData()
        {
            var uri = _sut.UriBuilder("charmander");
            var actual = await _sut.GetPokemon(uri);

            var expected = CreateTestPokemon_Charmander();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GivenValidPokemonName_UriBuilder_CreatesAndReturnsValidUri()
        {
            Uri expected = new Uri("https://pokeapi.co/api/v2/pokemon/charmander");
            var actual = _sut.UriBuilder("charmander");

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GivenUriThatDoesNotExist_GetPokemonReturnsNull()
        {
            Uri uri = new("https://pokeapi.co/api/v2/pokemon/NotARealPokemon");
            var actual = await _sut.GetPokemon(uri);
            actual.Should().BeNull();
        }

        [Fact]
        public void GivenListOfSixPokemon_AddPokemonToTeamReturnsCorrectValues()
        {
            var team = new Team();
            var expected = new Team();

            for(int i = 0; i < 6; i++)
            {
                team = _sut.AddPokemonToTeam(team, CreateTestPokemon_Charmander());
                expected.Pokemon.Add(CreateTestPokemon_Charmander());
            }
            team.Pokemon.Count.Should().BeLessOrEqualTo(6);
            team.Pokemon.Should().BeEquivalentTo(expected.Pokemon);
                        
        }

        private Pokemon CreateTestPokemon_Charmander()
        {
            var abilities = new List<Ability>()
            {
                new Ability
                {
                    Name = "blaze",
                    URL = "https://pokeapi.co/api/v2/ability/66/"
                },
                new Ability
                {
                    Name = "solar-power",
                    URL = "https://pokeapi.co/api/v2/ability/94/"
                }
            };

            var types = new List<Repository.Models.Type>()
            {
                new Repository.Models.Type
                {
                    Name = "fire",
                    URL = "https://pokeapi.co/api/v2/type/10/"
                }
            };

            var stats = new List<Statistic>()
            {
                new Statistic
                {
                    BaseValue = 39,
                    Name = "hp"
                },
                new Statistic
                {
                    BaseValue = 52,
                    Name = "attack"
                },
                new Statistic
                {
                    BaseValue = 43,
                    Name = "defense"
                },
                new Statistic
                {
                    BaseValue = 60,
                    Name = "special-attack"
                },
                new Statistic
                {
                    BaseValue = 50,
                    Name = "special-defense"
                },
                new Statistic
                {
                    BaseValue = 65,
                    Name = "speed"
                }
            };
            var charmander = new Pokemon()
            {
                Id = 4,
                Name = "charmander",
                Abilities = abilities,
                Types = types,
                Stats = stats
            };

            return charmander;
        }
    }
}

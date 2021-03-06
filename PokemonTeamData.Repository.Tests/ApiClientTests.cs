using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using PokemonTeamData.Repository.Models;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using PokemonTeamData.Repository.Enums;

namespace PokemonTeamData.Repository.Tests
{
    public class ApiClientTests
    {
        private IApiClient _sut;

        public ApiClientTests()
        {
            _sut = new ApiClient();
        }

        [Fact]
        public async Task GivenValidUri_GetPokemon_SetsValuesCorrectlyAsync()
        {
            Uri uri = new Uri("https://pokeapi.co/api/v2/pokemon/charmander");
            var actual = await _sut.GetPokemon(uri);

            var expectedAbilities = new List<Ability>()
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

            var expectedTypes = new List<Models.PokemonType>()
            {
                new Models.PokemonType
                {
                    Name = "fire",
                    URL = "https://pokeapi.co/api/v2/type/10/"
                }
            };

            var expectedStats = new List<Statistic>()
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
            var expectedSuperEffectiveWhenAttacking = new List<string>()
            {
                "bug",
                "steel",
                "grass",
                "ice"
            };
            var expectedSuperEffectiveWhenAttacked = new List<string>()
            {
                "ground",
                "rock",
                "water"
            };
            var expectedEffectiveWhenAttacking = new List<string>()
            {
                "rock",
                "fire",
                "water",
                "dragon"
            };
            var expectedEffectiveWhenAttacked = new List<string>()
            {
                "bug",
                "steel",
                "fire",
                "grass",
                "ice",
                "fairy"
            };
            var expectedNoEffectWhenAttacking = new List<string>();
            var expectedNoEffectWhenAttacked = new List<string>();
            var expected = new Pokemon()
            {
               Id = 4,
               Name = "charmander",
               Abilities = expectedAbilities,
               Types = expectedTypes,
               Stats = expectedStats,
               SuperEffectiveWhenAttacking = expectedSuperEffectiveWhenAttacking,
               SuperEffectiveWhenGettingAttacked = expectedSuperEffectiveWhenAttacked,
               EffectiveWhenAttacking = expectedEffectiveWhenAttacking,
               EffectiveWhenGettingAttacked = expectedEffectiveWhenAttacked,
               NoEffectWhenAttacking = expectedNoEffectWhenAttacking,
               NoEffectWhenGettingAttackedBy = expectedNoEffectWhenAttacked
            };

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GivenUriThatDoesNotExist_GetPokemonReturnsNull()
        {
            Uri uri = new("https://pokeapi.co/api/v2/pokemon/NotARealPokemon");
            var actual = await _sut.GetPokemon(uri);
            actual.Should().BeNull();
        }
    }
}

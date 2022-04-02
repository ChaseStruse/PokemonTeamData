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
            var expectedEffectiveness = new Dictionary<TypeEffectiveness, List<Models.PokemonType>>()
            {
                {
                    TypeEffectiveness.SuperEffectiveWhenGettingAttackedByThisType,
                    new List<Models.PokemonType>()
                    {
                        new Models.PokemonType { Name = "ground", URL = "https://pokeapi.co/api/v2/type/5/" },
                        new Models.PokemonType { Name = "rock", URL = "https://pokeapi.co/api/v2/type/6/" },
                        new Models.PokemonType { Name = "water", URL = "https://pokeapi.co/api/v2/type/11/" },
                    }
                },
                {
                    TypeEffectiveness.SuperEffectiveWhenAttackingThisType,
                    new List<Models.PokemonType>()
                    {
                        new Models.PokemonType { Name = "bug", URL = "https://pokeapi.co/api/v2/type/7/" },
                        new Models.PokemonType { Name = "steel", URL = "https://pokeapi.co/api/v2/type/9/" },
                        new Models.PokemonType { Name = "grass", URL = "https://pokeapi.co/api/v2/type/12/" },
                        new Models.PokemonType { Name = "ice", URL = "https://pokeapi.co/api/v2/type/15/" },
                    }
                },
                {
                    TypeEffectiveness.EffectiveWhenGettingAttackedByThisType,
                    new List<Models.PokemonType>()
                    {
                        new Models.PokemonType { Name = "bug", URL = "https://pokeapi.co/api/v2/type/7/" },
                        new Models.PokemonType { Name = "steel", URL = "https://pokeapi.co/api/v2/type/9/" },                       
                        new Models.PokemonType { Name = "fire", URL = "https://pokeapi.co/api/v2/type/10/" },
                        new Models.PokemonType { Name = "grass", URL = "https://pokeapi.co/api/v2/type/12/" },
                        new Models.PokemonType { Name = "ice", URL = "https://pokeapi.co/api/v2/type/15/" },
                        new Models.PokemonType { Name = "fairy", URL = "https://pokeapi.co/api/v2/type/18/" },
                    }
                },
                {
                    TypeEffectiveness.EffectiveWhenAttackingThisType,
                    new List<Models.PokemonType>()
                    {
                        new Models.PokemonType { Name = "rock", URL = "https://pokeapi.co/api/v2/type/6/" },
                        new Models.PokemonType { Name = "fire", URL = "https://pokeapi.co/api/v2/type/10/" },
                        new Models.PokemonType { Name = "water", URL = "https://pokeapi.co/api/v2/type/11/" },
                        new Models.PokemonType { Name = "dragon", URL = "https://pokeapi.co/api/v2/type/16/" },
                    }
                },
                {
                    TypeEffectiveness.NoEffectWhenAttackingThisType,
                    new List<Models.PokemonType>(){}
                },
                {
                    TypeEffectiveness.NoEffectWhenGettingAttackedByThisType,
                    new List<Models.PokemonType>(){}
                },
            };
            var expected = new Pokemon()
            {
               Id = 4,
               Name = "charmander",
               Abilities = expectedAbilities,
               Types = expectedTypes,
               Stats = expectedStats,
               Effective = expectedEffectiveness
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

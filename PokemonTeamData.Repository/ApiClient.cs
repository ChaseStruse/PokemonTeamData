﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using PokemonTeamData.Repository.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace PokemonTeamData.Repository
{
    public interface IApiClient
    {
        Task<Pokemon> GetPokemon(Uri uri);
        int ParseIdToInt(JToken pokemonJToken);
        string ParseNameToString(JToken pokemonJToken);
        List<Ability> ParseAbilitiesToList(JToken abilities);
        List<Statistic> ParseStatisticsToList(JToken statistics);
        List<Models.Type> ParseTypesToList(JToken types);
    }
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client = new HttpClient();


        public async Task<Pokemon> GetPokemon(Uri uri)
        {
            string response = await _client.GetStringAsync(uri);
            JObject joResponse = JObject.Parse(response);

            Pokemon pokemon = new();

            pokemon.Id = ParseIdToInt(joResponse);
            pokemon.Name = ParseNameToString(joResponse);

            var unparsedAbilities = joResponse["abilities"];
            pokemon.Abilities = ParseAbilitiesToList(unparsedAbilities);

            var unparsedStats = joResponse["stats"];
            pokemon.Stats = ParseStatisticsToList(unparsedStats);

            var unparsedTypes = joResponse["types"];
            pokemon.Types = ParseTypesToList(unparsedTypes);

            return pokemon;
        }

        public int ParseIdToInt(JToken pokemonJToken)
        {
            return (int)pokemonJToken["id"];
        }

        public List<Ability> ParseAbilitiesToList(JToken abilities)
        {
            var abilitiesList = new List<Ability>();

            foreach (var unparsedAbility in abilities)
            {
                var abilityToAdd = new Ability
                {
                    Name = unparsedAbility["ability"]["name"].ToString(),
                    URL = unparsedAbility["ability"]["url"].ToString()
                };
                abilitiesList.Add(abilityToAdd);
            }
            return (abilitiesList);
        }

        public List<Statistic> ParseStatisticsToList(JToken statistics)
        {
            var statisticsList = new List<Statistic>();

            foreach(var unparsedStatistic in statistics)
            {
                var statisticToAdd = new Statistic
                {
                    Name = unparsedStatistic["stat"]["name"].ToString(),
                    BaseValue = (int)unparsedStatistic["base_stat"]
                };
                statisticsList.Add(statisticToAdd);
            }

            return statisticsList;
        }

        public List<Models.Type> ParseTypesToList(JToken types)
        {
            var typeList = new List<Models.Type>();

            foreach (var unparsedTypes in types)
            {
                var typeToAdd = new Models.Type
                {
                    Name = unparsedTypes["type"]["name"].ToString(),
                    URL = unparsedTypes["type"]["url"].ToString()
                };
                typeList.Add(typeToAdd);
            }

            return typeList;
        }

        public string ParseNameToString(JToken pokemonJToken)
        {
            return pokemonJToken["name"].ToString();
        }
    }
}

using System;
using System.Net.Http;
using System.Threading.Tasks;
using PokemonTeamData.Repository.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.Generic;
using PokemonTeamData.Repository.Enums;

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
        Task<Dictionary<TypeEffectiveness, List<Models.Type>>> ParseEffectivenessToList(List<Models.Type> pokemonToGetsTypes);
    }
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client = new HttpClient();


        public async Task<Pokemon> GetPokemon(Uri uri)
        {
            try
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

                pokemon.Effective = await ParseEffectivenessToList(pokemon.Types);
                
                return pokemon;
            }
            catch
            {
                return null;
            }
            
        }

        public int ParseIdToInt(JToken pokemonJToken)
        {
            try
            {
                return (int)pokemonJToken["id"];
            }
            catch
            {
                throw new Exception("Could not find ID");
            }
        }

        public List<Ability> ParseAbilitiesToList(JToken abilities)
        {
            var abilitiesList = new List<Ability>();

            try
            {
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
            catch(Exception e)
            {
                throw new Exception("Unable to parse through abilities received error: " + e);
            }
        }

        public List<Statistic> ParseStatisticsToList(JToken statistics)
        {
            var statisticsList = new List<Statistic>();

            try
            {
                foreach (var unparsedStatistic in statistics)
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
            catch(Exception e)
            {
                throw new Exception("Unable to parse through statistics received error: " + e);
            }
        }

        public List<Models.Type> ParseTypesToList(JToken types)
        {
            var typeList = new List<Models.Type>();

            try
            {
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
            catch(Exception e)
            {
                throw new Exception("Unable to parse through types received error: " + e);
            }
        }

        public async Task<Dictionary<TypeEffectiveness, List<Models.Type>>> ParseEffectivenessToList(List<Models.Type> pokemonToGetsTypes)
        { 
            Dictionary<TypeEffectiveness, List<Models.Type>> effectiveness = new();
            JEnumerable<JToken> superEffective_WhenAttacked;
            JEnumerable<JToken> superEffective_WhenAttacking;
            JEnumerable<JToken> effective_WhenAttacked;
            JEnumerable<JToken> effective_WhenAttacking;
            JEnumerable<JToken> noDamage_WhenAttacked;
            JEnumerable<JToken> noDamage_WhenAttacking;

            foreach (var type in pokemonToGetsTypes)
            {
                var uri = new Uri(type.URL);
                string response = await _client.GetStringAsync(uri);
                JObject joResponse = JObject.Parse(response);

                superEffective_WhenAttacked = joResponse["damage_relations"]["double_damage_from"].Children();
                superEffective_WhenAttacking = joResponse["damage_relations"]["double_damage_to"].Children();

                effective_WhenAttacked = joResponse["damage_relations"]["half_damage_from"].Children();
                effective_WhenAttacking = joResponse["damage_relations"]["half_damage_to"].Children();

                noDamage_WhenAttacked = joResponse["damage_relations"]["no_damage_from"].Children();
                noDamage_WhenAttacking = joResponse["damage_relations"]["no_damage_to"].Children();

                var superEffectiveWhenAttackedByTypesList = ConvertJEnumerableToPokemonTypeList(superEffective_WhenAttacked);
                var superEffectiveWhenAttackingTypesList = ConvertJEnumerableToPokemonTypeList(superEffective_WhenAttacking);

                var effectiveWhenAttackedByTypesList = ConvertJEnumerableToPokemonTypeList(effective_WhenAttacked);
                var effectiveWhenAttackingTypesList = ConvertJEnumerableToPokemonTypeList(effective_WhenAttacking);

                var noDamageWhenAttackedByTypesList = ConvertJEnumerableToPokemonTypeList(noDamage_WhenAttacked);
                var noDamageWhenAttackingTypesList = ConvertJEnumerableToPokemonTypeList(noDamage_WhenAttacking);

                effectiveness.Add(TypeEffectiveness.SuperEffectiveWhenGettingAttackedByThisType, superEffectiveWhenAttackedByTypesList);
                effectiveness.Add(TypeEffectiveness.SuperEffectiveWhenAttackingThisType, superEffectiveWhenAttackingTypesList);

                effectiveness.Add(TypeEffectiveness.EffectiveWhenGettingAttackedByThisType, effectiveWhenAttackedByTypesList);
                effectiveness.Add(TypeEffectiveness.EffectiveWhenAttackingThisType, effectiveWhenAttackingTypesList);

                effectiveness.Add(TypeEffectiveness.NoEffectWhenGettingAttackedByThisType, noDamageWhenAttackedByTypesList);
                effectiveness.Add(TypeEffectiveness.NoEffectWhenAttackingThisType, noDamageWhenAttackingTypesList);
            }


            return effectiveness;
        }

        public string ParseNameToString(JToken pokemonJToken)
        {
            try
            {
                return pokemonJToken["name"].ToString();
            }
            catch
            {
                throw new Exception("Unable to parse name");
            }
        }

        private List<Models.Type> ConvertJEnumerableToPokemonTypeList(JEnumerable<JToken> jResponseTypes)
        {
            List<Models.Type> listOfTypes = new();

            foreach(var response in jResponseTypes)
            {
                var typeForList = new Models.Type
                {
                    Name = response["name"].ToString(),
                    URL = response["url"].ToString(),
                };

                listOfTypes.Add(typeForList);
            }

            return (listOfTypes);
        }
    }
}

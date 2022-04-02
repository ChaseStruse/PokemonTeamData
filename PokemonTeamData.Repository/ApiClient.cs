using System;
using System.Net.Http;
using System.Threading.Tasks;
using PokemonTeamData.Repository.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using PokemonTeamData.Repository.Helpers;
using System.Linq;

namespace PokemonTeamData.Repository
{
    public interface IApiClient
    {
        Task<Pokemon> GetPokemon(Uri uri);
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

                pokemon.Id = JTokenParseHelper.ParseIdToInt(joResponse);
                pokemon.Name = JTokenParseHelper.ParseNameToString(joResponse);

                var unparsedAbilities = joResponse["abilities"];
                pokemon.Abilities = JTokenParseHelper.ParseAbilitiesToList(unparsedAbilities);

                var unparsedStats = joResponse["stats"];
                pokemon.Stats = JTokenParseHelper.ParseStatisticsToList(unparsedStats);

                var unparsedTypes = joResponse["types"];
                pokemon.Types = JTokenParseHelper.ParseTypesToList(unparsedTypes);

                List<List<string>> superEffectiveWhenAttackingListWithDuplicates = new();
                List<List<string>> superEffectiveWhenGettingAttackedListWithDuplicates = new();

                List<List<string>> effectiveWhenAttackingListWithDuplicates = new();
                List<List<string>> effectiveWhenGettingAttackedListWithDuplicates = new();

                List<List<string>> noEffectWhenAttackingListWithDuplicates = new();
                List<List<string>> noEffectWhenGettingAttackedListWithDuplicates = new();

                foreach (var pokemonType in pokemon.Types)
                {
                    var typeUri = new Uri(pokemonType.URL);
                    string pokemonTypeResponse = await _client.GetStringAsync(typeUri);
                    JObject pokemonTypeJoResponse = JObject.Parse(pokemonTypeResponse);

                    
                    var superEffectiveWhenAttacking = pokemonTypeJoResponse["damage_relations"]["double_damage_to"].Children();
                    var superEffectiveWhenGettingAttacked = pokemonTypeJoResponse["damage_relations"]["double_damage_from"].Children();

                    superEffectiveWhenAttackingListWithDuplicates.Add(JTokenParseHelper.ParseEffectiveness(superEffectiveWhenAttacking));
                    superEffectiveWhenGettingAttackedListWithDuplicates.Add(JTokenParseHelper.ParseEffectiveness(superEffectiveWhenGettingAttacked));

                    var effectiveWhenAttacking = pokemonTypeJoResponse["damage_relations"]["half_damage_to"].Children();
                    var effectiveWhenGettingAttacked = pokemonTypeJoResponse["damage_relations"]["half_damage_from"].Children();

                    effectiveWhenAttackingListWithDuplicates.Add(JTokenParseHelper.ParseEffectiveness(effectiveWhenAttacking));
                    effectiveWhenGettingAttackedListWithDuplicates.Add(JTokenParseHelper.ParseEffectiveness(effectiveWhenGettingAttacked));


                    var noEffectWhenAttacking = pokemonTypeJoResponse["damage_relations"]["no_damage_to"].Children();
                    var noEffectWhenGettingAttackedBy = pokemonTypeJoResponse["damage_relations"]["no_damage_from"].Children();

                    noEffectWhenAttackingListWithDuplicates.Add(JTokenParseHelper.ParseEffectiveness(noEffectWhenAttacking));
                    noEffectWhenGettingAttackedListWithDuplicates.Add(JTokenParseHelper.ParseEffectiveness(noEffectWhenGettingAttackedBy));

                }

                pokemon.SuperEffectiveWhenAttacking = RemoveDuplicates(superEffectiveWhenAttackingListWithDuplicates);
                pokemon.SuperEffectiveWhenGettingAttacked = RemoveDuplicates(superEffectiveWhenGettingAttackedListWithDuplicates);

                pokemon.EffectiveWhenAttacking = RemoveDuplicates(effectiveWhenAttackingListWithDuplicates);
                pokemon.EffectiveWhenGettingAttacked = RemoveDuplicates(effectiveWhenGettingAttackedListWithDuplicates);

                pokemon.NoEffectWhenAttacking = RemoveDuplicates(noEffectWhenAttackingListWithDuplicates);
                pokemon.NoEffectWhenGettingAttackedBy = RemoveDuplicates(noEffectWhenGettingAttackedListWithDuplicates);


                return pokemon;
            }
            catch
            {
                return null;
            }

        }
        private static List<string> RemoveDuplicates(List<List<string>> listWithDuplicates)
        {
            List<string> distinctList = new();

            foreach(var pokemonTypeList in listWithDuplicates)
            {
                foreach(var pokemonType in pokemonTypeList)
                {
                    distinctList.Add(pokemonType);
                }
            }

            return distinctList.Distinct().ToList();
        }
    }
}

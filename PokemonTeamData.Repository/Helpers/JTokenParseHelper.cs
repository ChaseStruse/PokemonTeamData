using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PokemonTeamData.Repository.Enums;
using PokemonTeamData.Repository.Models;

namespace PokemonTeamData.Repository.Helpers
{

    public abstract class JTokenParseHelper
    {
        public static int ParseIdToInt(JToken pokemonJToken)
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

        public static string ParseNameToString(JToken pokemonJToken)
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

        public static List<Ability> ParseAbilitiesToList(JToken abilities)
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
            catch (Exception e)
            {
                throw new Exception("Unable to parse through abilities received error: " + e);
            }
        }

        public static List<Statistic> ParseStatisticsToList(JToken statistics)
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
            catch (Exception e)
            {
                throw new Exception("Unable to parse through statistics received error: " + e);
            }
        }

        public static List<PokemonType> ParseTypesToList(JToken types)
        {
            var typeList = new List<PokemonType>();

            try
            {
                foreach (var unparsedTypes in types)
                {
                    var typeToAdd = new PokemonType
                    {
                        Name = unparsedTypes["type"]["name"].ToString(),
                        URL = unparsedTypes["type"]["url"].ToString()
                    };
                    typeList.Add(typeToAdd);
                }

                return typeList;

            }
            catch (Exception e)
            {
                throw new Exception("Unable to parse through types received error: " + e);
            }
        }

        public static List<string> ParseEffectiveness(JEnumerable<JToken> effectivenessToParse)
        {

            List<string> listOfTypes = new();

            foreach (var response in effectivenessToParse)
            { 
                listOfTypes.Add(response["name"].ToString());
            }

            return (listOfTypes);
        }
    }
}

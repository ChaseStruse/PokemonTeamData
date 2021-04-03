using System;
using System.Threading.Tasks;
using PokemonTeamData.Repository.Enums;
using PokemonTeamData.Repository.Models;
using PokemonTeamData.Service;

namespace PokemonTeamData.UI
{
    public interface IUserInterface
    {
        Task MainProgramLoop();
        Task<Pokemon> GetPokemon(string pokemonName);
        Task DisplayGetPokemonMessagePromptsAsync();
        void DisplayPokemonInformation(Pokemon pokemon);
        void DisplayMainMenu();
        Task<Team> AddPokemonToTeamAsync(Team team);
        void DisplaySimpleTeamInformation(Team team);
        void DisplayAdvancedTeamInformation(Team team);
    }

    public class UserInterface : IUserInterface
    {
        private readonly IPokemonService pokemonService;

        public UserInterface()
        {
            pokemonService = new PokemonService();
        }

        public async Task MainProgramLoop()
        {
            var userWantsToExitProgram = false;
            var userTeam = new Team();

            while (!userWantsToExitProgram)
            {
                DisplayMainMenu();
                var userChoice = Console.ReadLine();

                if(userChoice == "1")
                {
                    await DisplayGetPokemonMessagePromptsAsync();
                }
                else if(userChoice == "2")
                {
                    userTeam = await AddPokemonToTeamAsync(userTeam);
                }
                else if (userChoice == "3")
                {
                    DisplaySimpleTeamInformation(userTeam);
                }
                else if (userChoice == "4")
                {
                    DisplayAdvancedTeamInformation(userTeam);
                }
                else if(userChoice == "0")
                {
                    userWantsToExitProgram = true;
                }
          
            }
        }

        public void DisplayPokemonInformation(Pokemon pokemon)
        {
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Pokemon ID: " + pokemon.Id);
            Console.WriteLine("Pokemon Name: " + pokemon.Name);
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Types: ");
            foreach (var pokemonType in pokemon.Types)
            {
                Console.WriteLine(pokemonType.Name);
            }
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Abilities:");
            foreach (var pokemonAbility in pokemon.Abilities)
            {
                Console.WriteLine(pokemonAbility.Name);
            }
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Base Stats:");
            foreach (var pokemonBaseStats in pokemon.Stats)
            {
                Console.WriteLine(pokemonBaseStats.Name);
                Console.WriteLine(pokemonBaseStats.BaseValue);
            }
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Super Effective when attacking these types: ");
            foreach (var effectivenessStat in pokemon.Effective[TypeEffectiveness.SuperEffectiveWhenAttackingThisType])
            {
                Console.WriteLine(effectivenessStat.Name);
            }
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("These types are super effective when attacking you: ");
            foreach (var effectivenessStat in pokemon.Effective[TypeEffectiveness.SuperEffectiveWhenGettingAttackedByThisType])
            {
                Console.WriteLine(effectivenessStat.Name);
            }
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Effective when attacking these types: ");
            foreach (var effectivenessStat in pokemon.Effective[TypeEffectiveness.EffectiveWhenAttackingThisType])
            {
                Console.WriteLine(effectivenessStat.Name);
            }
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("These types are effective when attacking you: ");
            foreach (var effectivenessStat in pokemon.Effective[TypeEffectiveness.EffectiveWhenGettingAttackedByThisType])
            {
                Console.WriteLine(effectivenessStat.Name);
            }
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("You have no effect attacking these types: ");
            foreach (var effectivenessStat in pokemon.Effective[TypeEffectiveness.NoEffectWhenAttackingThisType])
            {
                Console.WriteLine(effectivenessStat.Name);
            }
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("These types have no effect when attacking you: ");
            foreach (var effectivenessStat in pokemon.Effective[TypeEffectiveness.NoEffectWhenAttackingThisType])
            {
                Console.WriteLine(effectivenessStat.Name);
            }
            Console.WriteLine("----------------------------------------------");
        }

        public void DisplaySimpleTeamInformation(Team team)
        {
            foreach (var pokemon in team.Pokemon)
            {
                Console.WriteLine("Pokemon ID: " + pokemon.Id + " Pokemon: " + pokemon.Name);
            }
        }

        public void DisplayAdvancedTeamInformation(Team team)
        {
            foreach (var pokemon in team.Pokemon)
            {
                DisplayPokemonInformation(pokemon);
                Console.WriteLine();
            }
        }
        public async Task<Pokemon> GetPokemon(string pokemonName)
        {
            try
            {
                var uri = pokemonService.UriBuilder(pokemonName);
                var pokemon = await pokemonService.GetPokemon(uri);
                return pokemon;
            }
            catch 
            {
                return null;
            }
        }

        public void DisplayMainMenu()
        {
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("|   Welcome to Pokemon Team Data Main Menu   |");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("|   1 - View stats of a specific pokemon     |");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("|   2 - Add pokemon to your team             |");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("|   3 - Display pokemon on your team         |");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("|   4 - Display advanced stats on your team  |");
            Console.WriteLine("----------------------------------------------");
        }

        public async Task DisplayGetPokemonMessagePromptsAsync()
        {
            Console.WriteLine("Please enter the name of the pokemon you would like to see: ");
            var pokemonName = Console.ReadLine();

            var pokemon = await GetPokemon(pokemonName.ToLower());

            if (pokemon != null)
            {
                DisplayPokemonInformation(pokemon);
            }
            else
            {
                Console.WriteLine($"Could not retrieve pokemon {pokemonName}, please ensure it is spelled correctly");
            }

            Console.WriteLine("Would you like to search for another pokemon? (y/n)");
            var userChoice = Console.ReadLine();

            if (userChoice.ToLower() == "y")
            {
                await DisplayGetPokemonMessagePromptsAsync();
            }
        }

        public async Task<Team> AddPokemonToTeamAsync(Team team)
        {
            Console.WriteLine("Please enter the name of the pokemon you would like to add: ");
            var pokemonName = Console.ReadLine();

            var pokemonToAdd = await GetPokemon(pokemonName);

            if(pokemonToAdd != null)
            {
                team = pokemonService.AddPokemonToTeam(team, pokemonToAdd);
            }
            else
            {
                Console.WriteLine($"Could not retrieve pokemon {pokemonName}, please ensure it is spelled correctly, pokemon was not added to team");
            }

            Console.WriteLine("Would you like to add another pokemon? (y/n)");
            var userChoice = Console.ReadLine();

            if (userChoice.ToLower() == "y")
            {
                await AddPokemonToTeamAsync(team);
            }

            return team;
        }
    }
}

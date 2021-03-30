using System;
using System.Threading.Tasks;
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

            while (!userWantsToExitProgram)
            {
                DisplayMainMenu();
                var userChoice = Console.ReadLine();

                if(userChoice == "1")
                {
                    await DisplayGetPokemonMessagePromptsAsync();
                }
                else if(userChoice == "0")
                {
                    userWantsToExitProgram = true;
                }
          
            }
        }

        public void DisplayPokemonInformation(Pokemon pokemon)
        {
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
                Console.WriteLine("Could not retrieve pokemon, please ensure you typed the name in correctly");
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
            Console.WriteLine("Would you like to seach for another pokemon? (y/n)");
            var userChoice = Console.ReadLine();

            if (userChoice.ToLower() == "y")
            {
                await DisplayGetPokemonMessagePromptsAsync();
            }
        }
    }
}

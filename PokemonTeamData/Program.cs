using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PokemonTeamData.Repository;
using PokemonTeamData.Repository.Models;
using PokemonTeamData.UI;

namespace PokemonTeamData
{
    class Program
    { 
        static void Main(string[] args)
        {
            var userInterface = new UserInterface();
            userInterface.MainProgramLoop();
        }

        

    }
}

using PokemonTeamData.UI;

namespace PokemonTeamData
{
    class Program
    { 
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            UserInterface userInterface = new UserInterface();
            await userInterface.MainProgramLoop();
        }
    }
}

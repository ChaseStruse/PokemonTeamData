using System;
using System.Collections.Generic;

namespace PokemonTeamData.Repository.Models
{
    public class Team
    {
        public List<Pokemon> Pokemon { get; set; } = new List<Pokemon>();
    }
}

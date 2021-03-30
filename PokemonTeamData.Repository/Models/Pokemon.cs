using System;
using System.Collections.Generic;

namespace PokemonTeamData.Repository.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<Statistic> Stats { get; set; }
        public List<Type> Types { get; set; }
    }
}

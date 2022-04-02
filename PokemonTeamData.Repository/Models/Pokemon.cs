using System;
using System.Collections.Generic;
using PokemonTeamData.Repository.Enums;

namespace PokemonTeamData.Repository.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<Statistic> Stats { get; set; }
        public List<PokemonType> Types { get; set; }
        public List<string> SuperEffectiveWhenAttacking { get; set; } = new();
        public List<string> SuperEffectiveWhenGettingAttacked { get; set; } = new();
        public List<string> EffectiveWhenAttacking { get; set; } = new();
        public List<string> EffectiveWhenGettingAttacked { get; set; } = new();
        public List<string> NoEffectWhenAttacking { get; set; } = new();
        public List<string> NoEffectWhenGettingAttackedBy { get; set; } = new();
    }
}

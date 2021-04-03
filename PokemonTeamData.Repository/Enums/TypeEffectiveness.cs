using System;
namespace PokemonTeamData.Repository.Enums
{
    public enum TypeEffectiveness
    {
        SuperEffectiveWhenAttackingThisType,
        EffectiveWhenAttackingThisType,
        NoEffectWhenAttackingThisType,
        SuperEffectiveWhenGettingAttackedByThisType,
        EffectiveWhenGettingAttackedByThisType,
        NoEffectWhenGettingAttackedByThisType
    }
}

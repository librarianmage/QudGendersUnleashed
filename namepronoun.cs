using HarmonyLib;
using XRL.World;
using XRL.Language;
using ConsoleLib.Console;

namespace QudGendersUnleashed.NamePronoun
{
    [HarmonyPatch(typeof(GameObject))]
    [HarmonyPatch(nameof(GameObject.GetPronounProvider))]
    public static class NameOnlyPronounPatch
    {
        static IPronounProvider Postfix(IPronounProvider PronounSet, GameObject __instance)
        {
            return new NamePronounWrapper(PronounSet, __instance);
        }
    }

    // Wraps a IPronounProvider to replace =name=/=name's= with the holder's name
    public class NamePronounWrapper : IPronounProvider
    {
        private IPronounProvider BasePronouns;
        private GameObject Referrant;

        public NamePronounWrapper(IPronounProvider BasePronouns, GameObject Referrant)
        {
            this.BasePronouns = BasePronouns;
            this.Referrant = Referrant;
            // Consider: caching if replacing is needed
        }

        string ReplaceWithName(string Pronoun, bool capitalize = false)
        {
            if (Pronoun.Contains("=name"))
            {
                string DisplayName = Referrant.BaseDisplayNameStripped;
                if (capitalize) { DisplayName = ColorUtility.CapitalizeExceptFormatting(DisplayName); }
                string DisplayNamePosessive = Grammar.MakePossessive(DisplayName);
                return Pronoun.Replace("=name=", DisplayName)
                    .Replace("=name's=", DisplayNamePosessive);
            }
            else
            {
                return Pronoun;
            }
        }

        public string Name => ReplaceWithName(BasePronouns.Name);

        public string CapitalizedName => ReplaceWithName(BasePronouns.CapitalizedName, true);

        public bool Generic => BasePronouns.Generic;

        public bool Generated => BasePronouns.Generated;

        public bool Plural => BasePronouns.Plural;

        public bool PseudoPlural => BasePronouns.PseudoPlural;

        public string Subjective => ReplaceWithName(BasePronouns.Subjective);

        public string CapitalizedSubjective => ReplaceWithName(BasePronouns.CapitalizedSubjective, true);

        public string Objective => ReplaceWithName(BasePronouns.Objective);

        public string CapitalizedObjective => ReplaceWithName(BasePronouns.CapitalizedObjective, true);

        public string PossessiveAdjective => ReplaceWithName(BasePronouns.PossessiveAdjective);

        public string CapitalizedPossessiveAdjective => ReplaceWithName(BasePronouns.CapitalizedPossessiveAdjective, true);

        public string SubstantivePossessive => ReplaceWithName(BasePronouns.SubstantivePossessive);

        public string CapitalizedSubstantivePossessive => ReplaceWithName(BasePronouns.CapitalizedSubstantivePossessive);

        public string Reflexive => ReplaceWithName(BasePronouns.Reflexive);

        public string CapitalizedReflexive => ReplaceWithName(BasePronouns.CapitalizedReflexive, true);

        // Consider: The methods below here are unlikely to contain =name=/=name's=, remove wrap?
        public string PersonTerm => ReplaceWithName(BasePronouns.PersonTerm);

        public string CapitalizedPersonTerm => ReplaceWithName(BasePronouns.CapitalizedPersonTerm, true);

        public string ImmaturePersonTerm => ReplaceWithName(BasePronouns.ImmaturePersonTerm);

        public string CapitalizedImmaturePersonTerm => ReplaceWithName(BasePronouns.CapitalizedImmaturePersonTerm, true);

        public string FormalAddressTerm => ReplaceWithName(BasePronouns.FormalAddressTerm);

        public string CapitalizedFormalAddressTerm => ReplaceWithName(BasePronouns.CapitalizedFormalAddressTerm, true);

        public string OffspringTerm => ReplaceWithName(BasePronouns.OffspringTerm);

        public string CapitalizedOffspringTerm => ReplaceWithName(BasePronouns.CapitalizedOffspringTerm, true);

        public string SiblingTerm => ReplaceWithName(BasePronouns.SiblingTerm);

        public string CapitalizedSiblingTerm => ReplaceWithName(BasePronouns.CapitalizedSiblingTerm, true);

        public string ParentTerm => ReplaceWithName(BasePronouns.ParentTerm);

        public string CapitalizedParentTerm => ReplaceWithName(BasePronouns.CapitalizedParentTerm, true);

        public string IndicativeProximal => ReplaceWithName(BasePronouns.IndicativeProximal);

        public string CapitalizedIndicativeProximal => ReplaceWithName(BasePronouns.CapitalizedIndicativeProximal, true);

        public string IndicativeDistal => ReplaceWithName(BasePronouns.IndicativeDistal);

        public string CapitalizedIndicativeDistal => ReplaceWithName(BasePronouns.CapitalizedIndicativeDistal, true);

        public bool UseBareIndicative => BasePronouns.UseBareIndicative;
    }
}

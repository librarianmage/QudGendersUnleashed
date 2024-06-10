using ConsoleLib.Console;
using HarmonyLib;
using XRL.Language;
using XRL.World;

namespace QudGendersUnleashed.NamePronoun
{
    /// <summary>
    ///   Wraps the <see cref="IPronounProvider"/> returned by <see cref="GameObject.GetPronounProvider"/> with a <see cref="NamePronounWrapper" />.
    /// </summary>
    [HarmonyPatch(typeof(GameObject))]
    [HarmonyPatch(nameof(GameObject.GetPronounProvider))]
    public static class NameOnlyPronounPatch
    {
        private static IPronounProvider Postfix(IPronounProvider pronouns, GameObject __instance) =>
            NamePronounWrapper.Wrap(pronouns, __instance);
    }

    /// <summary>
    ///   Wraps a <see cref="IPronounProvider"/>, replacing <c>=name=</c>/<c>=name's=</c> with the name of the pronouns' referent.
    /// </summary>
    public class NamePronounWrapper : IPronounProvider
    {
        private readonly IPronounProvider BasePronouns;
        private readonly GameObject Referent;

        private static bool HasName(string s) => s.Contains("=name=") || s.Contains("=name's=");

        public static bool CouldBeNamePronouns(IPronounProvider pronouns) =>
            HasName(pronouns.Name)
            || HasName(pronouns.CapitalizedName)
            || HasName(pronouns.Subjective)
            || HasName(pronouns.CapitalizedSubjective)
            || HasName(pronouns.Objective)
            || HasName(pronouns.CapitalizedObjective)
            || HasName(pronouns.PossessiveAdjective)
            || HasName(pronouns.CapitalizedPossessiveAdjective)
            || HasName(pronouns.SubstantivePossessive)
            || HasName(pronouns.CapitalizedSubstantivePossessive)
            || HasName(pronouns.Reflexive)
            || HasName(pronouns.CapitalizedReflexive)
            || HasName(pronouns.PersonTerm)
            || HasName(pronouns.CapitalizedPersonTerm)
            || HasName(pronouns.ImmaturePersonTerm)
            || HasName(pronouns.CapitalizedImmaturePersonTerm)
            || HasName(pronouns.FormalAddressTerm)
            || HasName(pronouns.CapitalizedFormalAddressTerm)
            || HasName(pronouns.OffspringTerm)
            || HasName(pronouns.CapitalizedOffspringTerm)
            || HasName(pronouns.SiblingTerm)
            || HasName(pronouns.CapitalizedSiblingTerm)
            || HasName(pronouns.ParentTerm)
            || HasName(pronouns.CapitalizedParentTerm)
            || HasName(pronouns.IndicativeProximal)
            || HasName(pronouns.CapitalizedIndicativeProximal)
            || HasName(pronouns.IndicativeDistal)
            || HasName(pronouns.CapitalizedIndicativeDistal);

        public static IPronounProvider Wrap(IPronounProvider basePronouns, GameObject referent)
        {
            if (basePronouns is NamePronounWrapper p && p.Referent != referent)
            {
                return new NamePronounWrapper(p.BasePronouns, referent);
            }
            else if (CouldBeNamePronouns(basePronouns))
            {
                return new NamePronounWrapper(basePronouns, referent);
            }

            return basePronouns;
        }

        public NamePronounWrapper(IPronounProvider basePronouns, GameObject referent)
        {
            BasePronouns = basePronouns;
            Referent = referent;
        }

        public string ReplaceWithName(string pronoun, bool capitalize = false)
        {
            if (pronoun.Contains("=name"))
            {
                var displayName = Referent.BaseDisplayName;
                if (capitalize)
                {
                    displayName = ColorUtility.CapitalizeExceptFormatting(displayName);
                }
                var displayNamePossessive = Grammar.MakePossessive(displayName);
                return pronoun
                    .Replace("=name=", displayName)
                    .Replace("=name's=", displayNamePossessive);
            }
            else
            {
                return pronoun;
            }
        }

        public string Name => ReplaceWithName(BasePronouns.Name);

        public string CapitalizedName => ReplaceWithName(BasePronouns.CapitalizedName, true);

        public bool Generic => BasePronouns.Generic;

        public bool Generated => BasePronouns.Generated;

        public bool Plural => BasePronouns.Plural;

        public bool PseudoPlural => BasePronouns.PseudoPlural;

        public string Subjective => ReplaceWithName(BasePronouns.Subjective);

        public string CapitalizedSubjective =>
            ReplaceWithName(BasePronouns.CapitalizedSubjective, true);

        public string Objective => ReplaceWithName(BasePronouns.Objective);

        public string CapitalizedObjective =>
            ReplaceWithName(BasePronouns.CapitalizedObjective, true);

        public string PossessiveAdjective => ReplaceWithName(BasePronouns.PossessiveAdjective);

        public string CapitalizedPossessiveAdjective =>
            ReplaceWithName(BasePronouns.CapitalizedPossessiveAdjective, true);

        public string SubstantivePossessive => ReplaceWithName(BasePronouns.SubstantivePossessive);

        public string CapitalizedSubstantivePossessive =>
            ReplaceWithName(BasePronouns.CapitalizedSubstantivePossessive);

        public string Reflexive => ReplaceWithName(BasePronouns.Reflexive);

        public string CapitalizedReflexive =>
            ReplaceWithName(BasePronouns.CapitalizedReflexive, true);

        public string PersonTerm => ReplaceWithName(BasePronouns.PersonTerm);

        public string CapitalizedPersonTerm =>
            ReplaceWithName(BasePronouns.CapitalizedPersonTerm, true);

        public string ImmaturePersonTerm => ReplaceWithName(BasePronouns.ImmaturePersonTerm);

        public string CapitalizedImmaturePersonTerm =>
            ReplaceWithName(BasePronouns.CapitalizedImmaturePersonTerm, true);

        public string FormalAddressTerm => ReplaceWithName(BasePronouns.FormalAddressTerm);

        public string CapitalizedFormalAddressTerm =>
            ReplaceWithName(BasePronouns.CapitalizedFormalAddressTerm, true);

        public string OffspringTerm => ReplaceWithName(BasePronouns.OffspringTerm);

        public string CapitalizedOffspringTerm =>
            ReplaceWithName(BasePronouns.CapitalizedOffspringTerm, true);

        public string SiblingTerm => ReplaceWithName(BasePronouns.SiblingTerm);

        public string CapitalizedSiblingTerm =>
            ReplaceWithName(BasePronouns.CapitalizedSiblingTerm, true);

        public string ParentTerm => ReplaceWithName(BasePronouns.ParentTerm);

        public string CapitalizedParentTerm =>
            ReplaceWithName(BasePronouns.CapitalizedParentTerm, true);

        public string IndicativeProximal => ReplaceWithName(BasePronouns.IndicativeProximal);

        public string CapitalizedIndicativeProximal =>
            ReplaceWithName(BasePronouns.CapitalizedIndicativeProximal, true);

        public string IndicativeDistal => ReplaceWithName(BasePronouns.IndicativeDistal);

        public string CapitalizedIndicativeDistal =>
            ReplaceWithName(BasePronouns.CapitalizedIndicativeDistal, true);

        public bool UseBareIndicative => BasePronouns.UseBareIndicative;
    }
}

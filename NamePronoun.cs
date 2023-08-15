using HarmonyLib;
using XRL.World;
using XRL.Language;
using ConsoleLib.Console;

namespace QudGendersUnleashed.NamePronoun
{
    /// <summary>
    ///   Wraps the <see cref="IPronounProvider"/> returned by <see cref="GameObject.GetPronounProvider"/> with a <see cref="NamePronounWrapper" />.
    /// </summary>
    [HarmonyPatch(typeof(GameObject))]
    [HarmonyPatch(nameof(GameObject.GetPronounProvider))]
    public static class NameOnlyPronounPatch
    {
        static IPronounProvider Postfix(IPronounProvider pronouns, GameObject __instance)
            => NamePronounWrapper.Wrap(pronouns, __instance);
    }

    /// <summary>
    ///   Wraps a <see cref="IPronounProvider"/>, replacing <c>=name=</c>/<c>=name's=</c> with the name of the pronouns' referent.
    /// </summary>
    public class NamePronounWrapper : IPronounProvider
    {
        private IPronounProvider BasePronouns;
        private GameObject Referent;

        private static bool CouldHaveName(string s) => s.Contains("=name");

        public static bool CouldBeNamePronouns(IPronounProvider pronouns) =>
            CouldHaveName(pronouns.Name)
            || CouldHaveName(pronouns.CapitalizedName)
            || CouldHaveName(pronouns.Subjective)
            || CouldHaveName(pronouns.CapitalizedSubjective)
            || CouldHaveName(pronouns.Objective)
            || CouldHaveName(pronouns.CapitalizedObjective)
            || CouldHaveName(pronouns.PossessiveAdjective)
            || CouldHaveName(pronouns.CapitalizedPossessiveAdjective)
            || CouldHaveName(pronouns.SubstantivePossessive)
            || CouldHaveName(pronouns.CapitalizedSubstantivePossessive)
            || CouldHaveName(pronouns.Reflexive)
            || CouldHaveName(pronouns.CapitalizedReflexive)
            || CouldHaveName(pronouns.PersonTerm)
            || CouldHaveName(pronouns.CapitalizedPersonTerm)
            || CouldHaveName(pronouns.ImmaturePersonTerm)
            || CouldHaveName(pronouns.CapitalizedImmaturePersonTerm)
            || CouldHaveName(pronouns.FormalAddressTerm)
            || CouldHaveName(pronouns.CapitalizedFormalAddressTerm)
            || CouldHaveName(pronouns.OffspringTerm)
            || CouldHaveName(pronouns.CapitalizedOffspringTerm)
            || CouldHaveName(pronouns.SiblingTerm)
            || CouldHaveName(pronouns.CapitalizedSiblingTerm)
            || CouldHaveName(pronouns.ParentTerm)
            || CouldHaveName(pronouns.CapitalizedParentTerm)
            || CouldHaveName(pronouns.IndicativeProximal)
            || CouldHaveName(pronouns.CapitalizedIndicativeProximal)
            || CouldHaveName(pronouns.IndicativeDistal)
            || CouldHaveName(pronouns.CapitalizedIndicativeDistal);

        public static IPronounProvider Wrap(IPronounProvider basePronouns, GameObject referent)
        {
            if (basePronouns is NamePronounWrapper p && p.Referent != referent)
            {
                p.Referent = referent;
            }
            else if (CouldBeNamePronouns(basePronouns))
            {
                return new NamePronounWrapper(basePronouns, referent);
            }

            return basePronouns;
        }

        public NamePronounWrapper(IPronounProvider basePronouns, GameObject referent)
        {
            this.BasePronouns = basePronouns;
            this.Referent = referent;
        }

        public string ReplaceWithName(string pronoun, bool capitalize = false)
        {
            if (pronoun.Contains("=name"))
            {
                string displayName = Referent.BaseDisplayNameStripped;
                if (capitalize) {
                    displayName = ColorUtility.CapitalizeExceptFormatting(displayName);
                }
                string displayNamePossessive = Grammar.MakePossessive(displayName);
                return pronoun.Replace("=name=", displayName)
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

        public string CapitalizedSubjective => ReplaceWithName(BasePronouns.CapitalizedSubjective, true);

        public string Objective => ReplaceWithName(BasePronouns.Objective);

        public string CapitalizedObjective => ReplaceWithName(BasePronouns.CapitalizedObjective, true);

        public string PossessiveAdjective => ReplaceWithName(BasePronouns.PossessiveAdjective);

        public string CapitalizedPossessiveAdjective => ReplaceWithName(BasePronouns.CapitalizedPossessiveAdjective, true);

        public string SubstantivePossessive => ReplaceWithName(BasePronouns.SubstantivePossessive);

        public string CapitalizedSubstantivePossessive => ReplaceWithName(BasePronouns.CapitalizedSubstantivePossessive);

        public string Reflexive => ReplaceWithName(BasePronouns.Reflexive);

        public string CapitalizedReflexive => ReplaceWithName(BasePronouns.CapitalizedReflexive, true);

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

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
        static IPronounProvider Postfix(IPronounProvider PronounSet, GameObject __instance)
        {
            return NamePronounWrapper.Wrap(PronounSet, __instance);
        }
    }

    /// <summary>
    ///   Wraps a <see cref="IPronounProvider"/>, replacing <c>=name=</c>/<c>=name's=</c> with the name of the pronouns' referent.
    /// </summary>
    public class NamePronounWrapper : IPronounProvider
    {
        private IPronounProvider BasePronouns;
        private GameObject Referent;

        private static bool CouldHaveName(string S) => S.Contains("=name");

        public static bool CouldBeNamePronouns(IPronounProvider Pronouns) =>
            CouldHaveName(Pronouns.Name)
            || CouldHaveName(Pronouns.CapitalizedName)
            || CouldHaveName(Pronouns.Subjective)
            || CouldHaveName(Pronouns.CapitalizedSubjective)
            || CouldHaveName(Pronouns.Objective)
            || CouldHaveName(Pronouns.CapitalizedObjective)
            || CouldHaveName(Pronouns.PossessiveAdjective)
            || CouldHaveName(Pronouns.CapitalizedPossessiveAdjective)
            || CouldHaveName(Pronouns.SubstantivePossessive)
            || CouldHaveName(Pronouns.CapitalizedSubstantivePossessive)
            || CouldHaveName(Pronouns.Reflexive)
            || CouldHaveName(Pronouns.CapitalizedReflexive)
            || CouldHaveName(Pronouns.PersonTerm)
            || CouldHaveName(Pronouns.CapitalizedPersonTerm)
            || CouldHaveName(Pronouns.ImmaturePersonTerm)
            || CouldHaveName(Pronouns.CapitalizedImmaturePersonTerm)
            || CouldHaveName(Pronouns.FormalAddressTerm)
            || CouldHaveName(Pronouns.CapitalizedFormalAddressTerm)
            || CouldHaveName(Pronouns.OffspringTerm)
            || CouldHaveName(Pronouns.CapitalizedOffspringTerm)
            || CouldHaveName(Pronouns.SiblingTerm)
            || CouldHaveName(Pronouns.CapitalizedSiblingTerm)
            || CouldHaveName(Pronouns.ParentTerm)
            || CouldHaveName(Pronouns.CapitalizedParentTerm)
            || CouldHaveName(Pronouns.IndicativeProximal)
            || CouldHaveName(Pronouns.CapitalizedIndicativeProximal)
            || CouldHaveName(Pronouns.IndicativeDistal)
            || CouldHaveName(Pronouns.CapitalizedIndicativeDistal);

        public static IPronounProvider Wrap(IPronounProvider BasePronouns, GameObject Referent)
        {
            if (BasePronouns is NamePronounWrapper p && p.Referent != Referent)
            {
                p.Referent = Referent;
            }
            else if (CouldBeNamePronouns(BasePronouns))
            {
                return new NamePronounWrapper(BasePronouns, Referent);
            }

            return BasePronouns;
        }

        public NamePronounWrapper(IPronounProvider BasePronouns, GameObject Referrant)
        {
            this.BasePronouns = BasePronouns;
            this.Referent = Referrant;
        }

        public string ReplaceWithName(string Pronoun, bool capitalize = false)
        {
            if (Pronoun.Contains("=name"))
            {
                string DisplayName = Referent.BaseDisplayNameStripped;
                if (capitalize) {
                    DisplayName = ColorUtility.CapitalizeExceptFormatting(DisplayName);
                }
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

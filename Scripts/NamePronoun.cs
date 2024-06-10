using ConsoleLib.Console;
using XRL.Language;
using XRL.World;

namespace QudGendersUnleashed.NamePronoun
{
    /// <summary>
    ///   Wraps a <see cref="IPronounProvider"/>, replacing <c>=name=</c>/<c>=name's=</c> with the name of the pronouns' referent.
    /// </summary>
    public class NamePronounWrapper : IPronounProvider
    {
        private readonly IPronounProvider BasePronouns;
        private readonly GameObject Referent;

        private static bool HasName(string S) => S.Contains("=name=") || S.Contains("=name's=");

        public static bool CouldBeNamePronouns(IPronounProvider Pronouns) =>
            HasName(Pronouns.Name)
            || HasName(Pronouns.CapitalizedName)
            || HasName(Pronouns.Subjective)
            || HasName(Pronouns.CapitalizedSubjective)
            || HasName(Pronouns.Objective)
            || HasName(Pronouns.CapitalizedObjective)
            || HasName(Pronouns.PossessiveAdjective)
            || HasName(Pronouns.CapitalizedPossessiveAdjective)
            || HasName(Pronouns.SubstantivePossessive)
            || HasName(Pronouns.CapitalizedSubstantivePossessive)
            || HasName(Pronouns.Reflexive)
            || HasName(Pronouns.CapitalizedReflexive)
            || HasName(Pronouns.PersonTerm)
            || HasName(Pronouns.CapitalizedPersonTerm)
            || HasName(Pronouns.ImmaturePersonTerm)
            || HasName(Pronouns.CapitalizedImmaturePersonTerm)
            || HasName(Pronouns.FormalAddressTerm)
            || HasName(Pronouns.CapitalizedFormalAddressTerm)
            || HasName(Pronouns.OffspringTerm)
            || HasName(Pronouns.CapitalizedOffspringTerm)
            || HasName(Pronouns.SiblingTerm)
            || HasName(Pronouns.CapitalizedSiblingTerm)
            || HasName(Pronouns.ParentTerm)
            || HasName(Pronouns.CapitalizedParentTerm)
            || HasName(Pronouns.IndicativeProximal)
            || HasName(Pronouns.CapitalizedIndicativeProximal)
            || HasName(Pronouns.IndicativeDistal)
            || HasName(Pronouns.CapitalizedIndicativeDistal);

        public static IPronounProvider Wrap(IPronounProvider BasePronouns, GameObject Referent)
        {
            if (BasePronouns is NamePronounWrapper p && p.Referent != Referent)
            {
                return new NamePronounWrapper(p.BasePronouns, Referent);
            }
            else if (CouldBeNamePronouns(BasePronouns))
            {
                return new NamePronounWrapper(BasePronouns, Referent);
            }

            return BasePronouns;
        }

        public NamePronounWrapper(IPronounProvider BasePronouns, GameObject Referent)
        {
            this.BasePronouns = BasePronouns;
            this.Referent = Referent;
        }

        public string ReplaceWithName(string Pronoun, bool Capitalize = false)
        {
            if (Pronoun.Contains("=name"))
            {
                var displayName = Referent.BaseDisplayName;
                if (Capitalize)
                {
                    displayName = ColorUtility.CapitalizeExceptFormatting(displayName);
                }
                var displayNamePossessive = Grammar.MakePossessive(displayName);
                return Pronoun
                    .Replace("=name=", displayName)
                    .Replace("=name's=", displayNamePossessive);
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

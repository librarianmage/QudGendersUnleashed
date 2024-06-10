using System.Collections.Generic;
using System.Linq;
using ConsoleLib.Console;
using XRL.World;

namespace QudGendersUnleashed
{
    public static class Formatting
    {
        public static readonly string FromGenderText = Markup.Color("W", "<from gender>");
        public static readonly string CreateNewText = Markup.Color("W", "<create new>");

        public static string FormatGender(Gender g)
        {
            var name = Markup.Color("M", g.Name);
            var summary = Markup.Color("c", g.GetBasicSummary());

            return $"{name}\n{summary}";
        }

        public static string FormatPronounSet(PronounSet p)
        {
            var name = Markup.Color("M", p.GetShortName());
            var summary = Markup.Color("c", p.GetBasicSummary());

            if (p.FromGender)
            {
                var suspects = Gender
                    .Find(g => !g.DoNotReplicateAsPronounSet)
                    .Where(g => new PronounSet(g).Name == p.Name)
                    .Select(g => Markup.Color("m", g.Name));

                var from = suspects.Any() ? string.Join(" / ", suspects) : "unknown";
                var extra = Markup.Color("m", $"(from {from})");

                return $"{name} {extra}\n{summary}";
            }

            return $"{name}\n{summary}";
        }

        public static string FormatFromGenderPronounOption(Gender g)
        {
            var g1 = g ?? Gender.Get("nonspecific");
            if (g1 is null)
            {
                return FromGenderText;
            }

            var name = Markup.Color("m", g1.Name);
            var extra = Markup.Color("m", $"({name})");
            var summary = Markup.Color("c", g1.GetBasicSummary());

            return $"{FromGenderText} {extra}\n{summary}";
        }
    }
}

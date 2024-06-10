using System.Linq;
using ConsoleLib.Console;
using XRL.World;

namespace QudGendersUnleashed
{
    public static class Formatting
    {
        public static readonly string FromGenderText = Markup.Color("W", "<from gender>");
        public static readonly string CreateNewText = Markup.Color("W", "<create new>");

        public static string FormatGender(Gender G)
        {
            var name = Markup.Color("M", G.Name);
            var summary = Markup.Color("c", G.GetBasicSummary());

            return $"{name}\n{summary}";
        }

        public static string FormatPronounSet(PronounSet P)
        {
            var name = Markup.Color("M", P.GetShortName());
            var summary = Markup.Color("c", P.GetBasicSummary());

            if (P.FromGender)
            {
                var suspects = Gender
                    .Find(G => !G.DoNotReplicateAsPronounSet)
                    .Where(G => new PronounSet(G).Name == P.Name)
                    .Select(G => Markup.Color("m", G.Name));

                var from = suspects.Any() ? string.Join(" / ", suspects) : "unknown";
                var extra = Markup.Color("m", $"(from {from})");

                return $"{name} {extra}\n{summary}";
            }

            return $"{name}\n{summary}";
        }

        public static string FormatFromGenderPronounOption(Gender G)
        {
            var g1 = G ?? Gender.Get("nonspecific");
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

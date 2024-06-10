using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleLib.Console;
using XRL;
using XRL.CharacterBuilds.Qud.UI;
using XRL.UI;
using XRL.World;

// TODO: refactor

namespace QudGendersUnleashed
{
    /// <summary>Selectors for gender and pronoun sets.</summary>
    [HasModSensitiveStaticCache]
    public static class Selectors
    {
        private static readonly string FromGenderText = Markup.Color("W", "<from gender>");
        private static readonly string CreateNewText = Markup.Color("W", "<create new>");

        [ModSensitiveStaticCache]
        private static Dictionary<PronounSet, Gender> _PronounGenderMapping;

        public static Dictionary<PronounSet, Gender> PronounGenderMapping
        {
            get
            {
                if (_PronounGenderMapping == null)
                {
                    _PronounGenderMapping = new Dictionary<PronounSet, Gender>();

                    var suspects = Gender.Find(g => !g.DoNotReplicateAsPronounSet);
                    foreach (var g in suspects)
                    {
                        var setName = new PronounSet(g).Name;
                        var set = PronounSet.GetIfExists(setName);
                        if (set != null)
                        {
                            _PronounGenderMapping[set] = g;
                        }
                    }
                }

                return _PronounGenderMapping;
            }
        }

        private static string FormatName(string name, string color = "M")
        {
            if (ColorUtility.HasFormatting(name))
            {
                return Markup.Color("y", name);
            }
            else
            {
                return Markup.Color(color, name);
            }
        }

        private static string FormatGender(Gender g)
        {
            var name = FormatName(g.Name);
            var summary = Markup.Color("c", g.GetBasicSummary());

            return $"{name}\n{summary}";
        }

        private static string FormatPronounSet(PronounSet p)
        {
            var name = FormatName(p.GetShortName());

            var extra = "";

            if (PronounGenderMapping.TryGetValue(p, out var g))
            {
                extra = Markup.Color("m", $" (from {FormatName(g.Name, "m")})");
            }

            var summary = Markup.Color("c", p.GetBasicSummary());

            return $"{name}{extra}\n{summary}";
        }

        private static string FormatFromGenderPronounOption(Gender g1)
        {
            var g = g1 ?? Gender.Get("nonspecific");
            if (g == null)
            {
                return FromGenderText;
            }

            var gName = FormatName(g.Name, "m");

            var extra = Markup.Color("m", $"({gName})");

            var summary = Markup.Color("c", g.GetBasicSummary());

            return $"{FromGenderText} {extra}\n{summary}";
        }

        public static async Task<Gender> ChooseGenderAsync(Gender current)
        {
            var availableGenders = Gender.GetAllPersonal();
            var options = availableGenders.Select(gender => FormatGender(gender)).ToList();
            options.Add(CreateNewText);

            var initial = availableGenders.IndexOf(current);
            if (initial < 0)
            {
                initial = 0;
            }

            var index = await Popup.ShowOptionListAsync(
                Title: "Choose Gender",
                Options: options.ToArray(),
                AllowEscape: true,
                DefaultSelected: initial
            );

            if (index <= -1)
            {
                return null;
            }
            else if (0 <= index && index < options.Count - 1)
            {
                return availableGenders[index];
            }
            else
            {
                var baseIndex = await Popup.ShowOptionListAsync(
                    Title: "Select Base Gender",
                    Options: availableGenders.Select(gender => gender.Name).ToArray(),
                    AllowEscape: true,
                    DefaultSelected: initial
                );

                if (baseIndex <= -1)
                {
                    return null;
                }

                var baseGender = availableGenders[baseIndex];
                var newGender = new Gender(baseGender);

                if (await newGender.CustomizeAsync())
                {
                    return newGender;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<Gender> OnChooseGenderAsync(
            QudCustomizeCharacterModuleWindow window
        ) => await ChooseGenderAsync(window?.module?.data?.gender);

        public static void ChooseGender()
        {
            var newGender = ChooseGenderAsync(The.Player.GetGender()).Result;
            The.Player.SetGender(newGender.Register());
        }

        public static async Task<PronounSet> ChoosePronounSetAsync(
            Gender currentGender,
            PronounSet currentPronounSet,
            PronounSet placeholder
        )
        {
            var availablePronounSets = PronounSet.GetAllPersonal();
            var options = new List<string> { FormatFromGenderPronounOption(currentGender) };
            options.AddRange(
                availablePronounSets.Select(pronounSet => FormatPronounSet(pronounSet))
            );
            options.Add(CreateNewText);

            int initialPos,
                newPos;
            if (currentPronounSet == null)
            {
                initialPos = 0;
                newPos = 0;
            }
            else
            {
                var setIndex = availablePronounSets.IndexOf(currentPronounSet);

                initialPos = setIndex + 1;
                newPos = setIndex;
                if (newPos < 0)
                {
                    newPos = 0;
                }
            }

            var n = await Popup.ShowOptionListAsync(
                "Choose Pronoun Set",
                options.ToArray(),
                AllowEscape: true,
                DefaultSelected: initialPos
            );

            if (n <= -1)
            {
                return null;
            }
            else if (n == 0)
            {
                return placeholder;
            }
            else if (1 <= n && n < options.Count - 1)
            {
                return availablePronounSets[n];
            }
            else
            {
                var b = await Popup.ShowOptionListAsync(
                    "Select Base Set",
                    availablePronounSets.Select(PronounSet => PronounSet.Name).ToArray(),
                    AllowEscape: true,
                    DefaultSelected: newPos
                );
                if (b <= -1)
                {
                    return null;
                }

                var basePronounSet = availablePronounSets[b];
                var newPronounSet = new PronounSet(basePronounSet);

                if (await newPronounSet.CustomizeAsync())
                {
                    return newPronounSet;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<PronounSet> OnChoosePronounSetAsync(
            QudCustomizeCharacterModuleWindow window,
            PronounSet fromGenderPlaceholder
        )
        {
            var data = window?.module?.data;
            return await ChoosePronounSetAsync(
                data?.gender,
                data?.pronounSet,
                fromGenderPlaceholder
            );
        }

        public static void ChoosePronounSet()
        {
            var sentinel = new PronounSet();
            var newPronounSet = ChoosePronounSetAsync(
                The.Player.GetGender(),
                The.Player.GetPronounSet(),
                sentinel
            ).Result;
            if (newPronounSet == sentinel)
            {
                var n = new PronounSet(The.Player.GetGender());
                The.Player.SetPronounSet(n);
            }
            else if (newPronounSet != null)
            {
                The.Player.SetPronounSet(newPronounSet.Register());
            }
        }
    }
}

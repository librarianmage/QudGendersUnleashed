using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleLib.Console;
using XRL;
using XRL.CharacterBuilds.Qud.UI;
using XRL.UI;
using XRL.World;

namespace QudGendersUnleashed.Selectors
{
    [HasModSensitiveStaticCache]
    public static class Selectors
    {
        private static string FromGenderText = Markup.Color("W", "<from gender>");
        private static string CreateNewText = Markup.Color("W", "<create new>");

        [ModSensitiveStaticCache]
        private static Dictionary<PronounSet, Gender> _PronounGenderMapping;

        public static Dictionary<PronounSet, Gender> PronounGenderMapping {
            get {
                if (_PronounGenderMapping == null)
                {
                    _PronounGenderMapping = new Dictionary<PronounSet, Gender>();

                    var suspects = Gender.Find(g => !g.DoNotReplicateAsPronounSet);
                    foreach (Gender g in suspects)
                    {
                        var setName = new PronounSet(g).Name;
                        var pronounSet = PronounSet.GetIfExists(setName);
                        if (pronounSet != null) _PronounGenderMapping.Add(pronounSet, g);
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
            string name = FormatName(g.Name);
            string summary = Markup.Color("c", g.GetBasicSummary());

            return $"{name}\n{summary}";
        }

        private static string FormatPronounSet(PronounSet p)
        {
            string name = FormatName(p.GetShortName());

            string extra = "";

            Gender g;
            if (PronounGenderMapping.TryGetValue(p, out g))
            {
                extra = Markup.Color("m", $" (from {FormatName(g.Name, "m")})");
            }

            string summary = Markup.Color("c", p.GetBasicSummary());

            return $"{name}{extra}\n{summary}";
        }

        private static string FormatFromGenderPronounOption(Gender g1)
        {
            Gender g = g1 ?? Gender.Get("nonspecific");
            if (g == null) return FromGenderText;

            string gName = FormatName(g.Name, "m");

            string extra = Markup.Color("m", $"({gName})");

            string summary = Markup.Color("c", g.GetBasicSummary());

            return $"{FromGenderText} {extra}\n{summary}";
        }

        public static async Task<Gender> ChooseGenderAsync(Gender current)
        {
            var availableGenders = Gender.GetAllPersonal();
            var options = availableGenders.Select(gender=>FormatGender(gender)).ToList();
            options.Add(CreateNewText);

            var initial = availableGenders.IndexOf(current);
            if (initial < 0) initial = 0;

            int n = await Popup.ShowOptionListAsync("Choose Gender", options.ToArray(), AllowEscape: true, DefaultSelected: initial);

            if (n <= -1) return null;
            else if (n == options.Count - 1)
            {
                int b = await Popup.ShowOptionListAsync("Select Base Gender", availableGenders.Select(PronounSet => PronounSet.Name).ToArray(), AllowEscape: true, DefaultSelected: initial);
                    if( b > -1 )
                    {
                        var baseGender = availableGenders[b];
                        var newGender = new Gender(baseGender);
                        bool ok = await newGender.CustomizeAsync();
                        if( ok ) return newGender;
                    }
            }
            else return availableGenders[n];

            return null;
        }

        public static async Task<Gender> OnChooseGenderAsync(QudCustomizeCharacterModuleWindow window)
        {
            return await ChooseGenderAsync(window?.module?.data?.gender);
        }

        public static void ChooseGender()
        {
            var newGender = ChooseGenderAsync(The.Player.GetGender()).Result;

            The.Player.SetGender(newGender.Register());
        }

        public static async Task<PronounSet> ChoosePronounSetAsync(Gender currentGender, PronounSet currentPronounSet, PronounSet placeholder)
        {
            var availablePronounSets = PronounSet.GetAllPersonal();

            var options =  new List<string>();
            options.Add(FormatFromGenderPronounOption(currentGender));
            options.AddRange( availablePronounSets.Select(pronounSet => FormatPronounSet(pronounSet)));

            options.Add(CreateNewText);

            var PSpos = availablePronounSets.IndexOf(currentPronounSet);
            var initialPos = PSpos + 1;
            var newPos = PSpos;
            if (newPos < 0) newPos = 0;

            int n = await Popup.ShowOptionListAsync("Choose Pronoun Set", options.ToArray(), AllowEscape: true, DefaultSelected: initialPos);

            if( n > -1 )
            {
                if( n == options.Count-1 )
                {
                    int b = await Popup.ShowOptionListAsync("Select Base Set", availablePronounSets.Select(PronounSet => PronounSet.Name).ToArray(), AllowEscape: true, DefaultSelected: newPos);
                    if( b > -1 )
                    {
                        var basePronounSet = availablePronounSets[b];
                        var newPronounSet = new PronounSet(basePronounSet);
                        bool ok = await newPronounSet.CustomizeAsync();
                        if( ok ) return newPronounSet;
                    }
                }
                else
                {
                    if( n == 0 ) return placeholder;
                    return availablePronounSets[n-1];
                }
            }

            return null;
        }

        public static async Task<PronounSet> OnChoosePronounSetAsync(QudCustomizeCharacterModuleWindow window, PronounSet fromGenderPlaceholder)
        {
            var data = window?.module?.data;
            return await ChoosePronounSetAsync(data?.gender, data?.pronounSet, fromGenderPlaceholder);
        }

        public static void ChoosePronounSet()
        {
            var sentinel = new PronounSet();
            var newPronounSet = ChoosePronounSetAsync(The.Player.GetGender(), The.Player.GetPronounSet(), sentinel).Result;
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

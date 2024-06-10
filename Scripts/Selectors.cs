using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XRL;
using XRL.UI;
using XRL.World;

namespace QudGendersUnleashed
{
    /// <summary>Selectors for gender and pronoun sets.</summary>
    public static class Selectors
    {
        #region Genders
        public static async Task<Gender> SelectGenderAsync(Gender CurrentGender)
        {
            var genders = Gender.GetAllPersonal();
            var options = genders.Select(G => Formatting.FormatGender(G)).ToList();
            options.Add(Formatting.CreateNewText);

            var initialSelection = genders.IndexOf(CurrentGender);
            if (initialSelection < 0)
            {
                initialSelection = 0;
            }

            var idx = await Popup.ShowOptionListAsync(
                Title: "Choose Gender",
                Options: options.ToArray(),
                AllowEscape: true,
                DefaultSelected: initialSelection
            );

            if (idx <= -1)
            {
                return null;
            }
            else if (0 <= idx && idx < options.Count - 1)
            {
                return genders[idx];
            }
            else
            {
                var baseIdx = await Popup.ShowOptionListAsync(
                    Title: "Select Base Gender",
                    Options: genders.Select(G => G.Name).ToArray(),
                    AllowEscape: true,
                    DefaultSelected: initialSelection
                );

                if (baseIdx <= -1)
                {
                    return null;
                }

                var baseGender = genders[baseIdx];
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

        public static void ChooseGender(bool Message = false)
        {
            var newGender = SelectGenderAsync(The.Player.GetGender()).Result;
            if (newGender is not null)
            {
                The.Player.SetGender(newGender.Register());
                if (Message)
                {
                    Popup.Show($"Set gender to {newGender.Name}");
                }
            }
        }
        #endregion

        #region Pronouns
        public static async Task<PronounSet> ChoosePronounSetAsync(
            Gender CurrentGender,
            PronounSet CurrentPronounSet,
            PronounSet Placeholder
        )
        {
            var pronounSets = PronounSet.GetAllPersonal();
            var options = new List<string>
            {
                Formatting.FormatFromGenderPronounOption(CurrentGender)
            };
            options.AddRange(pronounSets.Select(P => Formatting.FormatPronounSet(P)));
            options.Add(Formatting.CreateNewText);

            int initialSelection,
                newSelection;
            if (CurrentPronounSet is null)
            {
                initialSelection = 0;
                newSelection = 0;
            }
            else
            {
                var setIdx = pronounSets.IndexOf(CurrentPronounSet);

                if (setIdx < 0)
                {
                    initialSelection = 0;
                    newSelection = 0;
                }
                else
                {
                    initialSelection = setIdx + 1;
                    newSelection = setIdx;
                }
            }

            var n = await Popup.ShowOptionListAsync(
                "Choose Pronoun Set",
                options.ToArray(),
                AllowEscape: true,
                DefaultSelected: initialSelection
            );

            if (n <= -1)
            {
                return null;
            }
            else if (n == 0)
            {
                return Placeholder;
            }
            else if (1 <= n && n < options.Count - 1)
            {
                return pronounSets[n - 1];
            }
            else
            {
                var b = await Popup.ShowOptionListAsync(
                    "Select Base Set",
                    pronounSets.Select(P => P.Name).ToArray(),
                    AllowEscape: true,
                    DefaultSelected: newSelection
                );
                if (b <= -1)
                {
                    return null;
                }

                var basePronounSet = pronounSets[b];
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

        public static void ChoosePronounSet(bool Message = false)
        {
            var sentinel = new PronounSet();
            var newPronounSet = ChoosePronounSetAsync(
                The.Player.GetGender(),
                The.Player.GetPronounSet(),
                sentinel
            ).Result;
            if (newPronounSet == sentinel)
            {
                var set = new PronounSet(The.Player.GetGender());
                The.Player.SetPronounSet(set);
                if (Message)
                {
                    Popup.Show($"set pronoun set from gender ({The.Player.GetGender().Name})", LogMessage: false);
                }
            }
            else if (newPronounSet is not null)
            {
                The.Player.SetPronounSet(newPronounSet.Register());
                if (Message)
                {
                    Popup.Show($"set pronoun set to {newPronounSet.Name}", LogMessage: false);
                }
            }
        }
        #endregion
    }
}

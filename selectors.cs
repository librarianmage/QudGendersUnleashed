using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XRL;
using XRL.World;
using XRL.CharacterBuilds.Qud.UI;
using XRL.UI;
using System.Threading.Tasks;

namespace QudGendersUnleashed.PronounAndGenderSelectorPatches
{
    [HarmonyPatch]
    public static class CharacterCreationGenderPatch
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.DeclaredMethod(AccessTools.Inner(typeof(QudCustomizeCharacterModuleWindow), "<OnChooseGenderAsync>d__5"), "MoveNext");
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo oldPReefMethod = AccessTools.Method(typeof(Gender), nameof(Gender.GetAllGenericPersonalSingular));
            MethodInfo oldMStairMethod = AccessTools.Method(typeof(Gender), nameof(Gender.GetAllGenericPersonal));
            MethodInfo newMethod = AccessTools.Method(typeof(Gender), nameof(Gender.GetAllPersonal));

            IEnumerable<CodeInstruction> replacePReef = HarmonyLib.Transpilers.MethodReplacer(instructions, oldPReefMethod, newMethod);
            IEnumerable<CodeInstruction> replaceMStair = HarmonyLib.Transpilers.MethodReplacer(replacePReef, oldMStairMethod, newMethod);

            return replaceMStair;
        }
    }

    [HarmonyPatch]
    public static class CharacterCreationPronounPatch
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.DeclaredMethod(AccessTools.Inner(typeof(QudCustomizeCharacterModuleWindow), "<OnChoosePronounSetAsync>d__7"), "MoveNext");
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {

            MethodInfo oldPReefMethod = AccessTools.Method(typeof(PronounSet), nameof(PronounSet.GetAllGenericPersonalSingular));
            MethodInfo oldMStairMethod = AccessTools.Method(typeof(PronounSet), nameof(PronounSet.GetAllGenericPersonal));
            MethodInfo newMethod = AccessTools.Method(typeof(PronounSet), nameof(PronounSet.GetAllPersonal));

            IEnumerable<CodeInstruction> replacePReef = HarmonyLib.Transpilers.MethodReplacer(instructions, oldPReefMethod, newMethod);
            IEnumerable<CodeInstruction> replaceMStair = HarmonyLib.Transpilers.MethodReplacer(replacePReef, oldMStairMethod, newMethod);

            return replaceMStair;
        }
    }

    // Technically unused
    [HarmonyPatch(typeof(PronounAndGenderSets))]
    [HarmonyPatch(nameof(PronounAndGenderSets.ShowChangePronounSet))]
    public static class PlaytimePronounPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo oldMethod = AccessTools.Method(typeof(PronounSet), nameof(PronounSet.GetAllGenericPersonal));
            MethodInfo newMethod = AccessTools.Method(typeof(PronounSet), nameof(PronounSet.GetAllPersonal));
            return HarmonyLib.Transpilers.MethodReplacer(instructions, oldMethod, newMethod);
        }
    }


    [HarmonyPatch(typeof(StatusScreen))]
    [HarmonyPatch(nameof(StatusScreen.Show))]
    public static class StatusScreenPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo oldMethod = AccessTools.Method(typeof(PronounAndGenderSets), nameof(PronounAndGenderSets.ShowChangePronounSet));
            MethodInfo newMethod = AccessTools.Method(typeof(StatusScreenPatch), nameof(StatusScreenPatch.ChangePronounSet));
            return HarmonyLib.Transpilers.MethodReplacer(instructions, oldMethod, newMethod);
        }

        public static void ChangePronounSet(GameObject Player)
        {
            Task<PronounSet> newPronounTask = OnChoosePronounSetAsync(Player);
            newPronounTask.Wait();
            PronounSet newPronoun = newPronounTask.Result;

            if (newPronoun != null)
            {
                Player.SetPronounSet(newPronoun.Register());
            }
        }

        public static async Task<PronounSet> OnChoosePronounSetAsync(GameObject Player)
        {
            List<PronounSet> availablePronounSets = PronounSet.GetAllPersonal();
            IEnumerable<string> pronounNames = availablePronounSets.Select((PronounSet pronounSet) => pronounSet.Name);

            PronounSet currentPronounSet = Player.GetPronounSet();
            int indexCurrentPronounSet = availablePronounSets.FindIndex(p => p == currentPronounSet);

            List<string> options = new List<string>();
            options.Add("<from gender>");
            options.AddRange(pronounNames);
            options.Add("<create new>");

            int index = await Popup.AsyncShowOptionsList(
                Title: "Change Pronoun Set",
                Options: options.ToArray(),
                AllowEscape: true,
                defaultSelected: indexCurrentPronounSet + 1);

            if (index > -1)
            {
                if (options[index] != "<create new>")
                {
                    if (index == 0)
                    {
                        // Selected <from gender>
                        Gender playerGender = Player.GetGender();
                        return new PronounSet(playerGender);
                    }
                    else
                    {
                        return availablePronounSets[index - 1];
                    }
                }

                int basePronounIndex = await Popup.AsyncShowOptionsList(
                    Title: "Select Base Set",
                    Options: pronounNames.ToArray(),
                    RespectOptionNewlines: false,
                    AllowEscape: true);

                if (basePronounIndex > -1)
                {
                    PronounSet original = availablePronounSets[basePronounIndex];
                    PronounSet newPronounSet = new PronounSet(original);
                    if (await newPronounSet.CustomizeAsync())
                    {
                        return newPronounSet;
                    }
                }
            }
            return null;
        }
    }
}

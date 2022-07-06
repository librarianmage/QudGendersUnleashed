using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XRL;
using XRL.World;
using XRL.CharacterBuilds.Qud.UI;
using XRL.UI;

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
            MethodInfo oldMethod = AccessTools.Method(typeof(Gender), nameof(Gender.GetAllGenericPersonalSingular));
            MethodInfo newMethod = AccessTools.Method(typeof(Gender), nameof(Gender.GetAllPersonal));
            return HarmonyLib.Transpilers.MethodReplacer(instructions, oldMethod, newMethod);
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

            MethodInfo oldMethod = AccessTools.Method(typeof(PronounSet), nameof(PronounSet.GetAllGenericPersonalSingular));
            MethodInfo newMethod = AccessTools.Method(typeof(PronounSet), nameof(PronounSet.GetAllPersonal));
            return HarmonyLib.Transpilers.MethodReplacer(instructions, oldMethod, newMethod);
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
            List<PronounSet> availablePronounSets = PronounSet.GetAllPersonal();
            PronounSet currentPronounSet = Player.GetPronounSet();
            int indexCurrentPronounSet = availablePronounSets.FindIndex(p => p.Name == currentPronounSet.Name);

            List<string> options = new List<string>();
            options.Add("<from gender>");
            options.AddRange(availablePronounSets.Select((PronounSet pronounSet) => pronounSet.Name));
            // options.Add("<create new>");

            var index = Popup.ShowOptionList(
                Title: "Change Pronoun Set",
                Options: options.ToArray(),
                AllowEscape: true,
                defaultSelected: indexCurrentPronounSet + 1);

            if (index > -1)
            {
                // Option was selected
                if (options[index] != "<create new>")
                {
                    // Option was not <create new>

                    PronounSet selected;
                    if (index == 0)
                    {
                        Gender playerGender = Player.GetGender();
                        selected = new PronounSet(playerGender);
                    }
                    else
                    {
                        selected = availablePronounSets[index - 1];
                    }

                    Player.SetPronounSet(selected.Register());

                }
                else
                {
                    // Currently unimplemented because the current customization process uses async stuff and this is in the old UI
                }
            }
        }
    }
}

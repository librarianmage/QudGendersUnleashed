using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using XRL;
using XRL.World;
using XRL.CharacterBuilds.Qud.UI;

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




}

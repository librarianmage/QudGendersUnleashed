using HarmonyLib;
using XRL;
using XRL.World;
using XRL.CharacterBuilds.Qud.UI;
using System.Threading.Tasks;

namespace QudGendersUnleashed.Selectors.Patches
{
    [HarmonyPatch(typeof(QudCustomizeCharacterModuleWindow))]
    [HarmonyPatch(nameof(QudCustomizeCharacterModuleWindow.OnChooseGenderAsync))]
    public static class GenderPatch
    {
        static bool Prefix() => false;

        static Task<Gender> Postfix(Task<Gender> _, QudCustomizeCharacterModuleWindow __instance) => Selectors.OnChooseGenderAsync(__instance);
    }

    [HarmonyPatch(typeof(QudCustomizeCharacterModuleWindow))]
    [HarmonyPatch(nameof(QudCustomizeCharacterModuleWindow.OnChoosePronounSetAsync))]
    public static class PronounPatch
    {
        static bool Prefix() => false;

        static Task<PronounSet> Postfix(Task<PronounSet> _, PronounSet ___fromGenderPlaceholder, QudCustomizeCharacterModuleWindow __instance)
            => Selectors.OnChoosePronounSetAsync(__instance, ___fromGenderPlaceholder);
    }

    [HarmonyPatch(typeof(PronounAndGenderSets))]
    [HarmonyPatch(nameof(PronounAndGenderSets.ShowChangePronounSet))]
    public static class PlaytimePronounPatch
    {
        static bool Prefix() => false;
        static void Postfix() => Selectors.ChoosePronounSet();
    }
}

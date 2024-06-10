using System.Threading.Tasks;
using HarmonyLib;
using XRL;
using XRL.CharacterBuilds.Qud.UI;
using XRL.World;

namespace QudGendersUnleashed.Patches
{
    /// <summary>Patch the gender selector during character creation.</summary>
    [HarmonyPatch(typeof(QudCustomizeCharacterModuleWindow))]
    [HarmonyPatch(nameof(QudCustomizeCharacterModuleWindow.OnChooseGenderAsync))]
    public static class GenderPatch
    {
        private static bool Prefix(
            ref Task<Gender> __result,
            QudCustomizeCharacterModuleWindow __instance
        )
        {
            __result = Selectors.OnChooseGenderAsync(__instance);
            return false;
        }
    }

    /// <summary>Patch the pronoun set selector during character creation.</summary>
    [HarmonyPatch(typeof(QudCustomizeCharacterModuleWindow))]
    [HarmonyPatch(nameof(QudCustomizeCharacterModuleWindow.OnChoosePronounSetAsync))]
    public static class PronounPatch
    {
        private static bool Prefix(
            ref Task<PronounSet> __result,
            QudCustomizeCharacterModuleWindow __instance
        )
        {
            var fromGenderPlaceholder = Traverse
                .Create(__instance)
                .Field<PronounSet>("fromGenderPlaceholder")
                .Value;
            __result = Selectors.OnChoosePronounSetAsync(__instance, fromGenderPlaceholder);
            return false;
        }
    }

    /// <summary>Patch the pronoun set selector from the character sheet.</summary>
    [HarmonyPatch(typeof(PronounAndGenderSets))]
    [HarmonyPatch(nameof(PronounAndGenderSets.ShowChangePronounSet))]
    public static class PlaytimePronounPatch
    {
        private static bool Prefix()
        {
            Selectors.ChoosePronounSet();
            return false;
        }
    }
}

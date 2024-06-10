using System.Threading.Tasks;
using HarmonyLib;
using QudGendersUnleashed.NamePronoun;
using XRL;
using XRL.CharacterBuilds.Qud.UI;
using XRL.World;

namespace QudGendersUnleashed.Patches
{
    /// <summary>Patch the gender selector for during character creation.</summary>
    /// <seealso cref="QudCustomizeCharacterModuleWindow.OnChooseGenderAsync"/>
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

    /// <summary>Patch the pronoun set selector for during character creation.</summary>
    /// <seealso cref="QudCustomizeCharacterModuleWindow.OnChoosePronounSetAsync"/>
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

    /// <summary>Patch the pronoun set selector for the (old-style) character sheet.</summary>
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

    /// <summary>
    ///   Wraps the <see cref="IPronounProvider"/> returned by <see cref="GameObject.GetPronounProvider"/> with a <see cref="NamePronounWrapper" />.
    /// </summary>
    [HarmonyPatch(typeof(GameObject))]
    [HarmonyPatch(nameof(GameObject.GetPronounProvider))]
    public static class NameOnlyPronounPatch
    {
        private static IPronounProvider Postfix(IPronounProvider pronouns, GameObject __instance) =>
            NamePronounWrapper.Wrap(pronouns, __instance);
    }
}

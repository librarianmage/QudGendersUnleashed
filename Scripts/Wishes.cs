using XRL.UI;
using XRL.Wish;

namespace QudGendersUnleashed
{
    /// <summary>Various wishes of varying utility.</summary>
    [HasWishCommand]
    public static class Wishes
    {
        /// <summary>Changes the user's gender.</summary>
        /// <remarks>Will not change the user's pronouns.</remarks>
        [WishCommand(Command = "changegender")]
        public static void ChangeGender() => Selectors.ChooseGender(true);

        /// <summary>Changes the user's pronouns.</summary>
        [WishCommand(Command = "changepronouns")]
        public static void ChangePronounSet() => Selectors.ChoosePronounSet(true);

        /// <summary>Useless wish used to generate the header image.</summary>
        /// <seealso href="https://web.archive.org/web/20220311192738/https://twitter.com/chordbug/status/1188086928731713539"/>
        [WishCommand(Command = "gendermeme")]
        public static void GenderUploadForm()
        {
            string[] opts = { "Male", "Female", "Custom [Upload custom gender (max 10MB)]" };

            Popup.ShowOptionList("Gender", opts);
        }
    }
}

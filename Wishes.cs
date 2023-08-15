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
        static public void ChangeGender() => Selectors.ChooseGender();

        /// <summary>Changes the user's pronouns.</summary>
        [WishCommand(Command = "changepronouns")]
        static public void ChangePronounSet() => Selectors.ChoosePronounSet();

        /// <summary>Useless wish used to generate the header image.</summary>
        [WishCommand(Command = "gendermeme")]
        static public void GenderUploadForm()
        {
            string[] opts = {
                "Male",
                "Female",
                "Custom [Upload custom gender (max 10MB)]"
            };

            Popup.ShowOptionList("Gender", opts);
        }
    }
}

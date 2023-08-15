using XRL.UI;
using XRL.Wish;

namespace QudGendersUnleashed.Wishes
{
    /// <summary>Useless wish used to generate the header image.</summary>
    [HasWishCommand]
    public class GenderUploadForm
    {
        [WishCommand(Command = "gendermeme")]
        static public bool GenderMeme()
        {
            string[] opts = {
                "Male",
                "Female",
                "Custom [Upload custom gender (max 10MB)]"
            };

            Popup.ShowOptionList("Gender", opts);
            return true;
        }
    }
}

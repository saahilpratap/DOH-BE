using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}
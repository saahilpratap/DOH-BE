using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
